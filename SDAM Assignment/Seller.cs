using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SDAM_Assignment
{
    public class Seller : User
    {
        public Seller() : base() { }

        public override void OpenDashboard()
        {
            if (this.Id > 0)
            {
                SellerDashBoard sellerForm = new SellerDashBoard(this.Id);
                sellerForm.Show();
            }
            else
            {
                MessageBox.Show("Invalid seller ID.");
            }
        }

        public bool AddProduct(string name, string description, decimal price, string imagePath)
        {
            Product product = new Product
            {
                SellerId = this.Id,
                Name = name,
                Description = description,
                Price = price,
                ImagePath = imagePath
            };
            return product.Save();
        }

        public List<Product> GetMyProducts()
        {
            return Product.LoadProductsBySeller(this.Id);
        }

        public bool DeleteProduct(int productId)
        {
            return Product.Delete(productId);
        }

        public bool UpdateProduct(Product product)
        {
            return product.Update();
        }

        public static Seller GetSellerById(int id)
        {
            using (var conn = Database.GetConnection())
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
