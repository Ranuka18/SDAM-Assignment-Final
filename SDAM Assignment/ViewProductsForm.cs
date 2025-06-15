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
using SDAM_Assignment.Helpers;
using SDAM_Assignment.Controllers;
using System.Diagnostics;


namespace SDAM_Assignment
{
    public partial class ViewProductsForm : Form
    {
        private Seller seller;
        private Seller currentSeller;

        public ViewProductsForm(Seller seller)
        {
            InitializeComponent();
            FormStyler.ApplyTheme(this);
            this.seller = seller;
            this.currentSeller = seller; // Initialize currentSeller
            LoadProductsBySeller();
        }

        private void LoadProductsBySeller()
        {
            try
            {
                flowLayoutPanelProducts.Controls.Clear();
                List<Product> products = SellerController.GetMyProducts(seller.Id);

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
                MessageBox.Show("Error loading products: " + ex.Message,
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private Panel CreateProductCard(Product product)
        {
            Panel card = new Panel
            {
                Width = 200,
                Height = 360,
                Tag = "NoTheme",
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

            // Load image from byte array
            if (product.Image_data != null && product.Image_data.Length > 0)
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream(product.Image_data))
                    {
                        picture.Image = Image.FromStream(ms);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error loading image: {ex.Message}");
                    picture.Image = null;
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
                Text = $"Rs. {product.Price:F2}",
                Top = 240,
                Left = 10,
                Width = 180,
                ForeColor = Color.Green,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Button btnEdit = new Button
            {
                Text = "Edit",
                Width = 80,
                Height = 30,
                Top = 280,
                Left = 10,
                BackColor = Color.LightBlue
            };

            btnEdit.Click += (s, e) =>
            {
                EditProductForm editForm = new EditProductForm(product);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadProductsBySeller();
                }
            };

            Button btnDelete = new Button
            {
                Text = "Delete",
                Width = 80,
                Height = 30,
                Top = 280,
                Left = 100,
                BackColor = Color.IndianRed,
                ForeColor = Color.White
            };

            btnDelete.Click += (s, e) => HandleProductDeletion(product);

            Button btnViewReviews = new Button
            {
                Text = "View Reviews",
                Width = 100,
                Height = 30,
                Top = 320,
                Left = 50,
                BackColor = Color.LightBlue
            };

            btnViewReviews.Click += (s, e) =>
            {
                ViewReviewsForm reviewsForm = new ViewReviewsForm(product.ProductId);
                reviewsForm.ShowDialog();
            };

            card.Controls.Add(picture);
            card.Controls.Add(lblName);
            card.Controls.Add(lblDesc);
            card.Controls.Add(lblPrice);
            card.Controls.Add(btnEdit);
            card.Controls.Add(btnDelete);
            card.Controls.Add(btnViewReviews);

            return card;
        }

        private void HandleProductDeletion(Product product)
        {
            if (!ConfirmDeletion(product.Name)) return;

            int orderCount = OrderController.GetOrderCountForProduct(product.ProductId);

            if (orderCount == 0)
            {
                if (SellerController.DeleteProduct(product.ProductId))
                {
                    MessageBox.Show("Product deleted successfully.", "Success",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProductsBySeller();
                }
                else
                {
                    MessageBox.Show("Failed to delete product.", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                var result = MessageBox.Show(
                    $"This product has {orderCount} active order(s).\n\n" +
                    "Would you like to manage these orders before deletion?",
                    "Active Orders Exist",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    using (var orderForm = new SellerOrdersForm(currentSeller, product.ProductId))
                    {
                        if (orderForm.ShowDialog() == DialogResult.OK &&
                            SellerController.DeleteProduct(product.ProductId))
                        {
                            MessageBox.Show("Product deleted successfully after order resolution.",
                                          "Success",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Information);
                            LoadProductsBySeller();
                        }
                    }
                }
            }
        }

        private bool ConfirmDeletion(string productName)
        {
            return MessageBox.Show($"Are you sure you want to delete '{productName}'?",
                                 "Confirm Deletion",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadProductsBySeller();
        }
    }
}



