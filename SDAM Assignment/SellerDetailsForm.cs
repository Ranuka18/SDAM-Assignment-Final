using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SDAM_Assignment.Controllers;
using SDAM_Assignment.Helpers;

namespace SDAM_Assignment
{
    public partial class SellerDetailsForm : Form
    {
        private int sellerId;

        public SellerDetailsForm(int sellerId)
        {
            InitializeComponent();
            FormStyler.ApplyTheme(this);
            this.sellerId = sellerId;
            LoadSellerProducts();
        }

        private void LoadSellerProducts()
        {
            flowLayoutPanelProducts.Controls.Clear();

            List<Product> products = ProductController.LoadProductsBySeller(sellerId);

            foreach (var product in products)
            {
                Panel card = CreateProductCard(product);
                flowLayoutPanelProducts.Controls.Add(card);
            }
        }

        private Panel CreateProductCard(Product product)
        {
            Panel card = new Panel
            {
                Width = 200,
                Height = 300,
                Tag = "NoTheme",
                BackColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10)
            };

            PictureBox picture = new PictureBox
            {
                Width = 180,
                Height = 150,
                Top = 10,
                Left = 10,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            // Modified image loading to use byte array
            if (product.Image_data != null && product.Image_data.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(product.Image_data))
                {
                    picture.Image = Image.FromStream(ms);
                }
            }

            Label lblName = new Label
            {
                Text = product.Name,
                Top = 170, // Fixed position below picture box
                Left = 10,
                Width = 180,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Label lblDesc = new Label
            {
                Text = product.Description,
                Top = 195,
                Left = 10,
                Width = 180,
                Height = 40,
                AutoEllipsis = true
            };

            Label lblPrice = new Label
            {
                Text = $"${product.Price:F2}",
                Top = 240,
                Left = 10,
                Width = 180,
                ForeColor = Color.Green,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Button btnViewReviews = new Button
            {
                Text = "View Reviews",
                Width = 100,
                Height = 30,
                Top = 260,
                Left = 10,
                BackColor = Color.LightGray
            };

            btnViewReviews.Click += (s, e) =>
            {
                AdminViewReviewsForm reviewForm = new AdminViewReviewsForm(product.ProductId);
                reviewForm.ShowDialog();
            };

            card.Controls.Add(picture);
            card.Controls.Add(lblName);
            card.Controls.Add(lblDesc);
            card.Controls.Add(lblPrice);
            card.Controls.Add(btnViewReviews);

            return card;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadSellerProducts();
        }
    }
}