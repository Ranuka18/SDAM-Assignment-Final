using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.IO;

namespace SDAM_Assignment.Controllers
{
    public static class BuyerController
    {
        private static readonly string _connectionString = "server=localhost;user=root;database=marketplace;password=;";

        public static void LoadProductCards(FlowLayoutPanel panel, int buyerId)
        {
            try
            {
                panel.Controls.Clear();
                List<Product> products = ProductController.LoadAll();

                foreach (Product product in products)
                {
                    Panel card = CreateProductCard(product, buyerId);
                    panel.Controls.Add(card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}");
            }
        }

        private static Panel CreateProductCard(Product product, int buyerId)
        {
            Panel card = new Panel
            {
                Width = 250,
                Height = 320,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10),
                BackColor = Color.White,
                Tag = buyerId
            };

            // Product Image
            PictureBox picture = new PictureBox
            {
                Width = 230,
                Height = 140,
                Top = 10,
                Left = 10,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            LoadProductImage(picture, product.Image_data);

            // Product Name
            Label lblName = new Label
            {
                Text = product.Name,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Width = 230,
                Top = picture.Bottom + 5,
                Left = 10
            };

            // Product Description
            Label lblDesc = new Label
            {
                Text = product.Description,
                Width = 230,
                Height = 40,
                Top = lblName.Bottom + 2,
                Left = 10,
                Font = new Font("Segoe UI", 8),
                AutoEllipsis = true
            };

            // Product Price
            Label lblPrice = new Label
            {
                Text = $"Rs. {product.Price:F2}",
                ForeColor = Color.Green,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Width = 230,
                Top = lblDesc.Bottom + 2,
                Left = 10
            };

            // Add to Cart Button
            Button btnAddToCart = new Button
            {
                Text = "🛒 Add to Cart",
                Width = 100,
                Height = 30,
                Top = lblPrice.Bottom + 5,
                Left = 10,
                Tag = product.ProductId
            };
            btnAddToCart.Click += (sender, e) => AddToCartHandler(sender, buyerId);

            // View Reviews Button
            Button btnViewReviews = new Button
            {
                Text = "⭐ Reviews",
                Width = 100,
                Height = 30,
                Top = lblPrice.Bottom + 5,
                Left = btnAddToCart.Right + 10,
                Tag = product.ProductId
            };
            btnViewReviews.Click += ViewReviewsHandler;

            card.Controls.AddRange(new Control[] { picture, lblName, lblDesc, lblPrice, btnAddToCart, btnViewReviews });
            return card;
        }

        private static void LoadProductImage(PictureBox pictureBox, byte[] imageData)
        {
            try
            {
                if (imageData != null && imageData.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        pictureBox.Image = Image.FromStream(ms);
                    }
                }
            }
            catch
            {
                pictureBox.Image = null;
            }
        }

        private static void AddToCartHandler(object sender, int buyerId)
        {
            Button button = (Button)sender;
            int productId = (int)button.Tag;

            try
            {
                bool success = CartItemsController.AddToCart(buyerId, productId);
                MessageBox.Show(success ? "Added to cart!" : "Failed to add to cart");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private static void ViewReviewsHandler(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int productId = (int)button.Tag;
            new ViewReviewsForm(productId).ShowDialog();
        }

        public static List<Buyer> GetAllBuyers()
        {
            List<Buyer> buyers = new List<Buyer>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM users WHERE role = 'buyer'";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        buyers.Add(new Buyer
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Email = reader.GetString("email"),
                            Phone = reader.GetString("phone_no")
                        });
                    }
                }
            }

            return buyers;
        }

        public static bool DeleteBuyer(int buyerId)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "DELETE FROM users WHERE id = @id AND role = 'buyer'";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", buyerId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}