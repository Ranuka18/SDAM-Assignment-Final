using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SDAM_Assignment.Controllers
{
    public static class SellerController
    {
        private static readonly string connectionString = "server=localhost;user=root;password=;database=marketplace;";

        public static bool AddProduct(int sellerId, string name, string description, decimal price, byte[] image_data)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO products 
                  (seller_id, name, description, price, image_data) 
                  VALUES (@sellerId, @name, @description, @price, @image_data)";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@sellerId", sellerId);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@image_data", image_data ?? (object)DBNull.Value);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static List<Product> GetMyProducts(int sellerId)
        {
            List<Product> products = new List<Product>();
            string query = "SELECT product_id, name, description, price, image_data FROM products WHERE seller_id = @sid";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@sid", sellerId);
                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            ProductId = reader.GetInt32("product_id"),
                            Name = reader.GetString("name"),
                            Description = reader.GetString("description"),
                            Price = reader.GetDecimal("price"),
                            Image_data = reader["image_data"] as byte[],
                            SellerId = sellerId
                        });
                    }
                }
            }
            return products;
        }

        public static (bool CanDelete, int OrderCount) CheckProductDeletionStatus(int productId)
        {
            int orderCount = OrderController.GetOrderCountForProduct(productId);
            return (orderCount == 0, orderCount);
        }

        public static bool DeleteProduct(int productId)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // First, delete related reviews
                    string deleteReviewsQuery = "DELETE FROM reviews WHERE product_id = @productId";
                    using (var cmd = new MySqlCommand(deleteReviewsQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@productId", productId);
                        cmd.ExecuteNonQuery();
                    }

                    // Then delete related orders
                    string deleteOrdersQuery = "DELETE FROM orders WHERE product_id = @productId";
                    using (var cmd = new MySqlCommand(deleteOrdersQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@productId", productId);
                        cmd.ExecuteNonQuery();
                    }

                    // Finally delete the product
                    string deleteProductQuery = "DELETE FROM products WHERE product_id = @productId";
                    using (var cmd = new MySqlCommand(deleteProductQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@productId", productId);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    // Log the error or handle it appropriately
                    Console.WriteLine($"Error deleting product: {ex.Message}");
                    return false;
                }
            }
        }

        public static bool UpdateProduct(Product product)
        {
            return ProductController.Update(product);
        }

        public static Seller GetSellerById(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM users WHERE id = @id AND role = 'seller'";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();

                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Seller
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Name = reader["name"].ToString(),
                        Email = reader["email"].ToString(),
                        Phone = reader["phone_no"].ToString()
                    };
                }
            }
            return null;
        }
    }
}