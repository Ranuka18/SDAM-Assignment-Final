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
using SDAM_Assignment.Controllers;
using SDAM_Assignment.Helpers;

namespace SDAM_Assignment
{
    public partial class BuyerOrdersForm : Form
    {
        
        private Buyer buyer;
        private static string connectionString = "server=localhost;user=root;password=;database=marketplace;";

        public BuyerOrdersForm(Buyer buyer)
        {
            InitializeComponent();
            FormStyler.ApplyTheme(this);
            this.buyer = buyer;
            LoadOrders();
        }


        private void LoadOrders()
        {
            flowLayoutPanelOrders.Controls.Clear();

            try
            {
                var orders = OrderController.GetOrdersForBuyer(buyer.Id);

                if (orders == null || orders.Count == 0)
                {
                    flowLayoutPanelOrders.Controls.Add(new Label
                    {
                        Text = "No orders found.",
                        AutoSize = true,
                        Font = new Font("Segoe UI", 10),
                        ForeColor = Color.Gray
                    });
                    return;
                }

                foreach (var order in orders)
                {
                    Product product = ProductController.GetById(order.ProductId);
                    if (product == null) continue;

                    Panel panel = new Panel
                    {
                        Width = 400,
                        Height = order.Status == "Received" ? 170 : 140,
                        BackColor = Color.WhiteSmoke,
                        BorderStyle = BorderStyle.FixedSingle,
                        Margin = new Padding(5),
                        Tag = "NoTheme"
                    };

                    // Order info label
                    string labelText = $"Product: {product.Name} | Status: {order.Status}";
                    if (order.Status == "Cancelled" && !string.IsNullOrEmpty(order.CancelReason))
                    {
                        labelText += $"\nReason: {order.CancelReason}";
                    }

                    panel.Controls.Add(new Label
                    {
                        Text = labelText,
                        AutoSize = true,
                        Left = 10,
                        Top = 10,
                        BackColor = Color.LightPink
                    });

                    // Mark as Received button (always visible)
                    Button btnReceived = new Button
                    {
                        Text = "Mark as Received",
                        Left = 10,
                        Top = 40,
                        Width = 150,
                        Tag = order.OrderId,
                        Enabled = order.Status == "Shipped",
                        BackColor = order.Status == "Shipped" ? SystemColors.Control : Color.LightGray
                    };

                    if (order.Status == "Shipped")
                    {
                        btnReceived.Click += (s, e) =>
                        {
                            OrderController.UpdateStatus((int)btnReceived.Tag, "Received");
                            LoadOrders();
                        };
                    }
                    panel.Controls.Add(btnReceived);

                    // Cancel Order button
                    if (order.Status == "Pending")
                    {
                        Button btnCancel = new Button
                        {
                            Text = "Cancel Order",
                            Left = 170,
                            Top = 40,
                            Width = 150,
                            BackColor = Color.OrangeRed,
                            ForeColor = Color.White,
                            Tag = order.OrderId
                        };
                        btnCancel.Click += (s, e) =>
                        {
                            if (MessageBox.Show("Are you sure?", "Confirm",
                                MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                OrderController.UpdateStatus((int)btnCancel.Tag, "Cancelled", "Buyer cancelled");
                                LoadOrders();
                            }
                        };
                        panel.Controls.Add(btnCancel);
                    }

                    // WORKING Review button
                    if (order.Status == "Received")
                    {
                        Button btnReview = new Button
                        {
                            Text = "📝 Add Review",
                            Width = 100,
                            Height = 30,
                            Top = 70,
                            Left = 10,
                            Tag = new { BuyerId = buyer.Id, ProductId = product.ProductId }
                        };

                        btnReview.Click += (s, e) =>
                        {
                            dynamic tags = btnReview.Tag;
                            try
                            {
                                using (var conn = new MySqlConnection(connectionString))
                                {
                                    conn.Open();

                                    // Check if review exists
                                    using (var checkCmd = new MySqlCommand(
                                        "SELECT COUNT(*) FROM reviews WHERE buyer_id = @buyerId AND product_id = @productId",
                                        conn))
                                    {
                                        checkCmd.Parameters.AddWithValue("@buyerId", tags.BuyerId);
                                        checkCmd.Parameters.AddWithValue("@productId", tags.ProductId);
                                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                                        {
                                            MessageBox.Show("You've already reviewed this product!");
                                            return;
                                        }
                                    }

                                    // Show review form
                                    using (var form = new AddReviewForm(tags.BuyerId, tags.ProductId))
                                    {
                                        if (form.ShowDialog() == DialogResult.OK)
                                        {
                                            // Insert review
                                            using (var insertCmd = new MySqlCommand(
                                                "INSERT INTO reviews (buyer_id, product_id, rating, comment) " +
                                                "VALUES (@buyerId, @productId, @rating, @comment)",
                                                conn))
                                            {
                                                insertCmd.Parameters.AddWithValue("@buyerId", tags.BuyerId);
                                                insertCmd.Parameters.AddWithValue("@productId", tags.ProductId);
                                                insertCmd.Parameters.AddWithValue("@rating", form.Rating);
                                                insertCmd.Parameters.AddWithValue("@comment", form.Comment);

                                                if (insertCmd.ExecuteNonQuery() > 0)
                                                {
                                                    MessageBox.Show("Thank you for your review!");
                                                    LoadOrders(); // Refresh the list
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error submitting review: " + ex.Message);
                            }
                        };

                        panel.Controls.Add(btnReview);
                    }

                    flowLayoutPanelOrders.Controls.Add(panel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading orders: " + ex.Message);
            }
        }

        private bool CheckIfReviewed(int buyerId, int productId)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM reviews WHERE buyer_id = @buyerId AND product_id = @productId";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@buyerId", buyerId);
                    cmd.Parameters.AddWithValue("@productId", productId);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        // Helper method to add reviews
        private bool AddReviewToDatabase(int buyerId, int productId, int rating, string comment)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO reviews (buyer_id, product_id, rating, comment, review_date)
                        VALUES (@buyerId, @productId, @rating, @comment, NOW())";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@buyerId", buyerId);
                    cmd.Parameters.AddWithValue("@productId", productId);
                    cmd.Parameters.AddWithValue("@rating", rating);
                    cmd.Parameters.AddWithValue("@comment", comment);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}