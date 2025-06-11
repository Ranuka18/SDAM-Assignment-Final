using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace SDAM_Assignment
{
     public class Product
    {
        public int ProductId { get; set; }
        public int SellerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }

        private static string connectionString = "server=localhost;user=root;password=;database=marketplace;";

        public bool Save()
        {
            string query = "INSERT INTO products (seller_id, name, description, price, image_path) " +
                           "VALUES (@sid, @name, @desc, @price, @img)";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@sid", SellerId);
                    cmd.Parameters.AddWithValue("@name", Name);
                    cmd.Parameters.AddWithValue("@desc", Description);
                    cmd.Parameters.AddWithValue("@price", Price);
                    cmd.Parameters.AddWithValue("@img", ImagePath);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

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

        public bool Update()
        {
            string query = "UPDATE products SET name = @name, description = @desc, price = @price, image_path = @img " +
                           "WHERE product_id = @id";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", ProductId);
                cmd.Parameters.AddWithValue("@name", Name);
                cmd.Parameters.AddWithValue("@desc", Description);
                cmd.Parameters.AddWithValue("@price", Price);
                cmd.Parameters.AddWithValue("@img", ImagePath);
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
