using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Data.Common;


namespace SDAM_Assignment
{
    public partial class ViewProductsForm : Form
    {
        public ViewProductsForm()
        {
            InitializeComponent();
            LoadProducts();
        }
        private void LoadProducts()
        {
            flowLayoutPanelProducts.Controls.Clear();

            List<Product> products = Product.LoadProducts();

            foreach (var product in products)
            {
                Panel card = CreateProductCard(
                    product.Name,
                    product.Description,
                    product.Price,
                    product.ImagePath,
                    product.ProductId 
                );
                flowLayoutPanelProducts.Controls.Add(card);
            }

        }

        private Panel CreateProductCard(string name, string description, decimal price, string imagePath, int productId)
        {
            Panel card = new Panel
            {
                Width = 200,
                Height = 330,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10),
                BackColor = Color.White,
                AutoScroll = true
            };

            int padding = 10;
            int currentTop = padding;

            PictureBox picture = new PictureBox
            {
                Width = 180,
                Height = 150,
                Left = padding,
                Top = currentTop,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                try { picture.Image = Image.FromFile(imagePath); }
                catch { /* handle broken image */ }
            }

            currentTop = picture.Bottom + 5;

            Label lblName = new Label
            {
                Text = name,
                Top = currentTop,
                Left = padding,
                Width = 180,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            currentTop = lblName.Bottom + 5;

            Label lblDesc = new Label
            {
                Text = description,
                Top = currentTop,
                Left = padding,
                Width = 180,
                Height = 40,
                AutoEllipsis = true
            };

            currentTop = lblDesc.Bottom + 5;

            Label lblPrice = new Label
            {
                Text = $"${price:F2}",
                Top = currentTop,
                Left = padding,
                Width = 180,
                ForeColor = Color.Green,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            currentTop = lblPrice.Bottom + 10;

            Button btnDelete = new Button
            {
                Text = "Delete",
                Width = 100,
                Height = 30,
                Top = currentTop,
                Left = (card.Width - 100) / 2,
                BackColor = Color.IndianRed,
                ForeColor = Color.White
            };

            btnDelete.Click += (s, e) =>
            {
                var result = MessageBox.Show("Are you sure you want to delete this product?", "Confirm", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    DeleteProduct(productId);
                    LoadProducts(); // refresh
                }
            };

            // Add controls to the card
            card.Controls.Add(picture);
            card.Controls.Add(lblName);
            card.Controls.Add(lblDesc);
            card.Controls.Add(lblPrice);
            card.Controls.Add(btnDelete);

            return card;
        }


        private void DeleteProduct(int productId)
        {
            using (var conn = Database.GetConnection())
            {
                string query = "DELETE FROM products WHERE product_id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", productId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Product deleted successfully.");
        }
    }
}





