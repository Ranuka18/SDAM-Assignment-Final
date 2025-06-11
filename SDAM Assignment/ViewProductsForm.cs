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
        private Seller seller;

        public ViewProductsForm(Seller seller)
        {
            InitializeComponent();
            this.seller = seller;

            LoadProductsBySeller();
        }

        private void LoadProductsBySeller()
        {
            try
            {
                flowLayoutPanelProducts.Controls.Clear();

                List<Product> products = seller.GetMyProducts(); 

                if (products.Count == 0)
                {
                    flowLayoutPanelProducts.Controls.Add(new Label
                    {
                        Text = "No products found.",
                        AutoSize = true,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = Color.Gray
                    });
                    return;
                }

                foreach (var product in products)
                {
                    flowLayoutPanelProducts.Controls.Add(CreateProductCard(product));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
            }
        }

        private Panel CreateProductCard(Product product)
        {
            Panel card = new Panel
            {
                Width = 200,
                Height = 330,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10),
                BackColor = Color.White
            };

            PictureBox picture = new PictureBox
            {
                Width = 180,
                Height = 150,
                Top = 10,
                Left = 10,
                SizeMode = PictureBoxSizeMode.StretchImage
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
                Top = picture.Bottom + 5,
                Left = 10,
                Width = 180,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Label lblDesc = new Label
            {
                Text = product.Description,
                Top = lblName.Bottom + 5,
                Left = 10,
                Width = 180,
                Height = 40,
                AutoEllipsis = true
            };

            Label lblPrice = new Label
            {
                Text = $"Rs. {product.Price:F2}",
                Top = lblDesc.Bottom + 5,
                Left = 10,
                Width = 180,
                ForeColor = Color.Green,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Button btnDelete = new Button
            {
                Text = "Delete",
                Width = 100,
                Height = 30,
                Top = lblPrice.Bottom + 10,
                Left = (card.Width - 100) / 2,
                BackColor = Color.IndianRed,
                ForeColor = Color.White
            };

            btnDelete.Click += (s, e) =>
            {
                if (ConfirmDeletion(product.Name))
                {
                    if (seller.DeleteProduct(product.ProductId)) 
                    {
                        MessageBox.Show("Product deleted.");
                        LoadProductsBySeller(); 
                    }
                    else
                    {
                        MessageBox.Show("Delete failed.");
                    }
                }
            };

            card.Controls.Add(picture);
            card.Controls.Add(lblName);
            card.Controls.Add(lblDesc);
            card.Controls.Add(lblPrice);
            card.Controls.Add(btnDelete);

            return card;
        }

        private bool ConfirmDeletion(string productName)
        {
            return MessageBox.Show($"Are you sure you want to delete '{productName}'?",
                                   "Confirm Deletion",
                                   MessageBoxButtons.YesNo,
                                   MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}





