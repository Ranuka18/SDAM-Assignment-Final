using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using Mysqlx.Crud;

namespace SDAM_Assignment.Controllers
{
    public class CartItemsController
    {
        private static string connectionString = "server=localhost;user=root;password=;database=marketplace;";

        public static bool AddToCart(int buyerId, int productId, int quantity = 1)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if this exact product already exists for this buyer
                    string checkQuery = "SELECT quantity FROM cart WHERE buyer_id = @buyerId AND product_id = @productId";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@buyerId", buyerId);
                        checkCmd.Parameters.AddWithValue("@productId", productId);

                        object result = checkCmd.ExecuteScalar();

                        if (result != null) // Item exists - update quantity
                        {
                            int existingQty = Convert.ToInt32(result);
                            string updateQuery = "UPDATE cart SET quantity = @qty WHERE buyer_id = @buyerId AND product_id = @productId";
                            using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@qty", existingQty + quantity);
                                updateCmd.Parameters.AddWithValue("@buyerId", buyerId);
                                updateCmd.Parameters.AddWithValue("@productId", productId);
                                return updateCmd.ExecuteNonQuery() > 0;
                            }
                        }
                        else // Item doesn't exist - insert new
                        {
                            string insertQuery = "INSERT INTO cart (buyer_id, product_id, quantity) VALUES (@buyerId, @productId, @qty)";
                            using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@buyerId", buyerId);
                                insertCmd.Parameters.AddWithValue("@productId", productId);
                                insertCmd.Parameters.AddWithValue("@qty", quantity);
                                return insertCmd.ExecuteNonQuery() > 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cart error: " + ex.Message);
                return false;
            }
        }

        public static List<CartItem> GetCartItems(int buyerId)
        {
            var items = new List<CartItem>();  

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                const string query = "SELECT cart_id, buyer_id, product_id, quantity FROM cart WHERE buyer_id = @buyerId";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@buyerId", buyerId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new CartItem
                            {
                                CartId = reader.GetInt32("cart_id"),
                                BuyerId = reader.GetInt32("buyer_id"),
                                ProductId = reader.GetInt32("product_id"),
                                Quantity = reader.GetInt32("quantity")
                            });
                        }
                    }
                }
            }
            return items;
        }

        public static List<(CartItem Item, Product Product)> GetCartDetails(int buyerId)
        {
            var details = new List<(CartItem, Product)>();
            var items = GetCartItems(buyerId);

            if (items.Count == 0)
                return details;

            // Get all product IDs at once
            var productIds = items.Select(i => i.ProductId).Distinct().ToList();

            // Fetch all products in one query
            var products = ProductController.GetProductsByIds(productIds);

            foreach (var item in items)
            {
                var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product == null)
                {
                    MessageBox.Show($"Product with ID {item.ProductId} not found.");
                    continue;
                }
                details.Add((item, product));
            }

            return details;
        }


        public static decimal CalculateTotal(int buyerId)
        {
            decimal total = 0;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT c.quantity, p.price 
                         FROM cart c
                         JOIN products p ON c.product_id = p.product_id
                         WHERE c.buyer_id = @buyerId";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@buyerId", buyerId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int qty = Convert.ToInt32(reader["quantity"]);
                            decimal price = Convert.ToDecimal(reader["price"]);
                            total += qty * price;
                        }
                    }
                }
            }

            return total;
        }


        public static bool RemoveFromCart(int buyerId, int productId)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM cart WHERE buyer_id = @buyerId AND product_id = @productId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@buyerId", buyerId);
                cmd.Parameters.AddWithValue("@productId", productId);
                conn.Open();
                int affectedRows = cmd.ExecuteNonQuery();
                return affectedRows > 0;
            }
        }

        public static void ClearCart(int buyerId)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM cart WHERE buyer_id = @buyerId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@buyerId", buyerId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}