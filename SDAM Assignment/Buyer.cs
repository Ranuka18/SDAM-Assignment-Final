using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace SDAM_Assignment
{
    public class Buyer : User
    {
        public Buyer() : base() { }

        public override void OpenDashboard()
        {
            BuyerDashboard dashboard = new BuyerDashboard(this);
            dashboard.Show();
        }

        public void LoadProductCards(FlowLayoutPanel panel)
        {
            panel.Controls.Clear();
            List<Product> allProducts = Product.LoadAll();

            foreach (var product in allProducts)
            {
                Panel card = CreateProductCard(product);
                panel.Controls.Add(card);
            }
        }

        public bool AddToCart(Product product, int quantity = 1)
        {
            return CartItem.AddToCart(this.Id, product.ProductId, quantity);
        }

        public static List<Buyer> GetAllBuyers()
        {
            List<Buyer> list = new List<Buyer>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM users WHERE role = 'buyer'";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Buyer
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = reader["name"].ToString(),
                            Email = reader["email"].ToString(),
                            Phone = reader["phone_no"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public static bool DeleteBuyer(int buyerId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM users WHERE id = @id AND role = 'buyer'";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", buyerId);
                return cmd.ExecuteNonQuery() > 0;
            }
        }



        private Panel CreateProductCard(Product product)
{
    Panel card = new Panel
    {
        Width = 250,
        Height = 320,
        BorderStyle = BorderStyle.FixedSingle,
        Margin = new Padding(10),
        BackColor = Color.White
    };

    PictureBox picture = new PictureBox
    {
        Width = 230,
        Height = 140,
        Top = 10,
        Left = 10,
        SizeMode = PictureBoxSizeMode.StretchImage,
        BorderStyle = BorderStyle.FixedSingle
    };

    if (!string.IsNullOrEmpty(product.ImagePath) && File.Exists(product.ImagePath))
    {
        try
        {
            picture.Image = Image.FromFile(product.ImagePath);
        }
        catch
        {
            picture.Image = null;
        }
    }

    Label lblName = new Label
    {
        Text = product.Name,
        Font = new Font("Segoe UI", 9, FontStyle.Bold),
        Width = 230,
        Height = 20,
        Top = picture.Bottom + 5,
        Left = 10
    };

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

    Label lblPrice = new Label
    {
        Text = $"Rs. {product.Price:F2}",
        ForeColor = Color.Green,
        Font = new Font("Segoe UI", 9, FontStyle.Bold),
        Width = 230,
        Height = 20,
        Top = lblDesc.Bottom + 2,
        Left = 10
    };

    Button btnAddToCart = new Button
    {
        Text = "🛒 Add to Cart",
        Width = 100,
        Height = 30,
        Top = lblPrice.Bottom + 5,
        Left = 10,
        BackColor = Color.LightBlue
    };

    btnAddToCart.Click += (s, e) =>
    {
        bool success = this.AddToCart(product);
        MessageBox.Show(success ? "Added to cart!" : "Failed to add to cart.");
    };

    Button btnViewReviews = new Button
    {
        Text = "⭐ View Reviews",
        Width = 110,
        Height = 30,
        Top = lblPrice.Bottom + 5,
        Left = btnAddToCart.Right + 10,
        BackColor = Color.LightYellow
    };

    btnViewReviews.Click += (s, e) =>
    {
        ViewReviewsForm reviewForm = new ViewReviewsForm(product.ProductId);
        reviewForm.ShowDialog();
    };

    // Add controls to the panel
    card.Controls.Add(picture);
    card.Controls.Add(lblName);
    card.Controls.Add(lblDesc);
    card.Controls.Add(lblPrice);
    card.Controls.Add(btnAddToCart);
    card.Controls.Add(btnViewReviews);

    return card;
}



    }
}
