using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SDAM_Assignment
{
    public class CartItem
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        private static string connectionString = "server=localhost;user=root;password=;database=marketplace;";

        public static bool AddToCart(int buyerId, int productId, int quantity = 1)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string checkQuery = "SELECT cart_id, quantity FROM cart WHERE buyer_id = @buyerId AND product_id = @productId";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@buyerId", buyerId);
                        checkCmd.Parameters.AddWithValue("@productId", productId);

                        using (var reader = checkCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int id = Convert.ToInt32(reader["cart_id"]);
                                int existingQty = Convert.ToInt32(reader["quantity"]);
                                reader.Close();

                                string updateQuery = "UPDATE cart SET quantity = @qty WHERE cart_id = @id";
                                using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                                {
                                    updateCmd.Parameters.AddWithValue("@qty", existingQty + quantity);
                                    updateCmd.Parameters.AddWithValue("@id", id);
                                    return updateCmd.ExecuteNonQuery() > 0;
                                }
                            }
                        }
                    }

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
            catch (Exception ex)
            {
                MessageBox.Show("Cart error: " + ex.Message);
                return false;
            }
        }


        public static List<CartItem> GetCartItems(int buyerId)
        {
            List<CartItem> items = new List<CartItem>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM cart WHERE buyer_id = @buyerId";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@buyerId", buyerId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new CartItem
                            {
                                Id = Convert.ToInt32(reader["cart_id"]),
                                BuyerId = Convert.ToInt32(reader["buyer_id"]),
                                ProductId = Convert.ToInt32(reader["product_id"]),
                                Quantity = Convert.ToInt32(reader["quantity"])
                            });
                        }
                    }
                }
            }

            return items;
        }

        public static List<(CartItem Item, Product Product)> GetCartDetails(int buyerId)
        {
            List<(CartItem, Product)> details = new List<(CartItem, Product)>();
            var items = GetCartItems(buyerId);

            foreach (var item in items)
            {
                Product prod = Product.GetById(item.ProductId); 
                details.Add((item, prod));
            }

            return details;
        }

        public static decimal CalculateTotal(int buyerId)
        {
            decimal total = 0;
            var items = GetCartItems(buyerId);

            foreach (var item in items)
            {
                Product prod = Product.GetById(item.ProductId);
                total += prod.Price * item.Quantity;
            }

            return total;
        }



        public static bool RemoveFromCart(int buyerId, int productId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM cart WHERE buyer_id = @buyerId AND product_id = @productId";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@buyerId", buyerId);
                    cmd.Parameters.AddWithValue("@productId", productId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static void ClearCart(int buyerId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM cart WHERE buyer_id = @buyerId";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@buyerId", buyerId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
