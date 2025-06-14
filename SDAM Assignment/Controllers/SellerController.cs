using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SDAM_Assignment.Controllers
{
    public static class SellerController
    {
        private static readonly string connectionString = "server=localhost;user=root;password=;database=marketplace;";

        public static bool AddProduct(int sellerId, string name, string description, decimal price, string imagePath)
        {
            Product product = new Product
            {
                SellerId = sellerId,
                Name = name,
                Description = description,
                Price = price,
                ImagePath = imagePath
            };
            return ProductController.Save(product);
        }

        public static List<Product> GetMyProducts(int sellerId)
        {
            List<Product> products = new List<Product>();
            string query = "SELECT product_id, name, description, price, image_path FROM products WHERE seller_id = @sid";

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
                            ImagePath = reader.GetString("image_path"),
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
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. First delete any related product images
                        string deleteImagesQuery = "DELETE FROM product_images WHERE product_id = @productId";
                        using (var cmdImages = new MySqlCommand(deleteImagesQuery, conn, transaction))
                        {
                            cmdImages.Parameters.AddWithValue("@productId", productId);
                            cmdImages.ExecuteNonQuery();
                        }

                        // 2. Then delete the product itself
                        string deleteProductQuery = "DELETE FROM products WHERE product_id = @productId";
                        using (var cmdProduct = new MySqlCommand(deleteProductQuery, conn, transaction))
                        {
                            cmdProduct.Parameters.AddWithValue("@productId", productId);
                            int rowsAffected = cmdProduct.ExecuteNonQuery();

                            transaction.Commit();
                            return rowsAffected > 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        // Log the error if needed
                        Console.WriteLine($"Error deleting product: {ex.Message}");
                        return false;
                    }
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
