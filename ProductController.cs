using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Xml.Linq;

namespace SDAM_Assignment.Controllers
{
    public static class ProductController
    {
        private static readonly string connectionString = "server=localhost;user=root;password=;database=marketplace;";

        public static List<Product> LoadProductsBySeller(int sellerId)
        {
            List<Product> products = new List<Product>();
            string query = "SELECT * FROM products WHERE seller_id = @sid";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@sid", sellerId);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            ProductId = Convert.ToInt32(reader["product_id"]),
                            SellerId = Convert.ToInt32(reader["seller_id"]),
                            Name = reader["name"].ToString(),
                            Description = reader["description"].ToString(),
                            Price = Convert.ToDecimal(reader["price"]),
                            ImagePath = reader["image_path"].ToString()
                        });
                    }
                }
            }
            return products;
        }

        public static List<Product> LoadAll()
        {
            List<Product> products = new List<Product>();
            string query = "SELECT * FROM products";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            ProductId = Convert.ToInt32(reader["product_id"]),
                            SellerId = Convert.ToInt32(reader["seller_id"]),
                            Name = reader["name"].ToString(),
                            Description = reader["description"].ToString(),
                            Price = Convert.ToDecimal(reader["price"]),
                            ImagePath = reader["image_path"].ToString()
                        });
                    }
                }
            }
            return products;
        }

        public static Product GetById(int productId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM products WHERE product_id = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", productId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Product
                            {
                                ProductId = Convert.ToInt32(reader["product_id"]),
                                SellerId = Convert.ToInt32(reader["seller_id"]),
                                Name = reader["name"].ToString(),
                                Description = reader["description"].ToString(),
                                Price = Convert.ToDecimal(reader["price"]),
                                ImagePath = reader["image_path"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }

        public static List<Product> GetProductsByIds(List<int> productIds)
        {
            var products = new List<Product>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM products WHERE product_id IN @ids";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ids", productIds);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductId = Convert.ToInt32(reader["product_id"]),
                                SellerId = Convert.ToInt32(reader["seller_id"]),
                                Name = reader["name"].ToString(),
                                Description = reader["description"].ToString(),
                                Price = Convert.ToDecimal(reader["price"]),
                                ImagePath = reader["image_path"].ToString()
                            });
                        }
                    }
                }
            }
            return products;
        }

        public static bool Save(Product product)
        {
            string query = "INSERT INTO products (seller_id, name, description, price, image_path) " +
                         "VALUES (@sid, @name, @desc, @price, @img)";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@sid", product.SellerId);
                    cmd.Parameters.AddWithValue("@name", product.Name);
                    cmd.Parameters.AddWithValue("@desc", product.Description);
                    cmd.Parameters.AddWithValue("@price", product.Price);
                    cmd.Parameters.AddWithValue("@img", product.ImagePath);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static bool Delete(int productId)
        {
            string query = "DELETE FROM products WHERE product_id = @id";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", productId);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static bool Update(Product product)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"UPDATE products 
                     SET name = @name, price = @price, description = @description, image_path = @imagePath 
                     WHERE product_id = @id";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", product.Name);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.Parameters.AddWithValue("@description", product.Description);
                cmd.Parameters.AddWithValue("@imagePath", product.ImagePath);
                cmd.Parameters.AddWithValue("@id", product.ProductId);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}

