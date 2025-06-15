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
            this.buyer = buyer ?? throw new ArgumentNullException(nameof(buyer));
            LoadOrders();
        }

        private void LoadOrders()
        {
            flowLayoutPanelOrders.Controls.Clear();

            try
            {
                var orders = OrderController.GetOrdersForBuyer(buyer.Id);
                DisplayOrders(orders);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayOrders(List<Order> orders)
        {
            if (orders == null || orders.Count == 0)
            {
                ShowNoOrdersMessage();
                return;
            }

            foreach (var order in orders)
            {
                Product product = ProductController.GetById(order.ProductId);
                if (product == null) continue;

                var orderPanel = CreateOrderPanel(order, product);
                flowLayoutPanelOrders.Controls.Add(orderPanel);
            }
        }

        private void ShowNoOrdersMessage()
        {
            flowLayoutPanelOrders.Controls.Add(new Label
            {
                Text = "No orders found.",
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray
            });
        }

        private Panel CreateOrderPanel(Order order, Product product)
        {
            Panel panel = new Panel
            {
                Width = 400,
                Height = order.Status == "Received" ? 170 : 140,
                BackColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5),
                Tag = "NoTheme"
            };

            AddOrderInfoLabel(panel, order, product);
            AddActionButtons(panel, order, product);

            return panel;
        }

        private void AddOrderInfoLabel(Panel panel, Order order, Product product)
        {
            string labelText = $"Product: {product.Name}\nStatus: {order.Status}";
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
                MaximumSize = new Size(380, 0)
            });
        }

        private void AddActionButtons(Panel panel, Order order, Product product)
        {
            // Mark as Received button
            var btnReceived = CreateReceivedButton(order);
            panel.Controls.Add(btnReceived);

            // Cancel Order button (only for pending orders)
            if (order.Status == "Pending")
            {
                var btnCancel = CreateCancelButton(order);
                panel.Controls.Add(btnCancel);
            }

            // Review button (only for received orders)
            if (order.Status == "Received")
            {
                var btnReview = CreateReviewButton(product);
                panel.Controls.Add(btnReview);
            }
        }

        private Button CreateReceivedButton(Order order)
        {
            return new Button
            {
                Text = "Mark as Received",
                Left = 10,
                Top = 60,
                Width = 150,
                Tag = order.OrderId,
                Enabled = order.Status == "Shipped",
                BackColor = order.Status == "Shipped" ? SystemColors.Control : Color.LightGray
            }.WithClickHandler((s, e) =>
            {
                if (OrderController.UpdateStatus((int)((Button)s).Tag, "Received"))
                    LoadOrders();
            });
        }

        private Button CreateCancelButton(Order order)
        {
            return new Button
            {
                Text = "Cancel Order",
                Left = 170,
                Top = 60,
                Width = 150,
                BackColor = Color.OrangeRed,
                ForeColor = Color.White,
                Tag = order.OrderId
            }.WithClickHandler((s, e) =>
            {
                if (MessageBox.Show("Are you sure you want to cancel this order?", "Confirm Cancellation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (OrderController.UpdateStatus((int)((Button)s).Tag, "Cancelled", "Buyer cancelled"))
                        LoadOrders();
                }
            });
        }

        private Button CreateReviewButton(Product product)
        {
            return new Button
            {
                Text = "📝 Add Review",
                Width = 100,
                Height = 30,
                Top = 100,
                Left = 10,
                Tag = new { BuyerId = buyer.Id, ProductId = product.ProductId }
            }.WithClickHandler(async (s, e) =>
            {
                await HandleReviewSubmission((dynamic)((Button)s).Tag);
            });
        }

        private async Task HandleReviewSubmission(dynamic reviewInfo)
        {
            try
            {
                if (await ReviewExists(reviewInfo.BuyerId, reviewInfo.ProductId))
                {
                    MessageBox.Show("You've already reviewed this product!", "Review Exists");
                    return;
                }

                using (var form = new AddReviewForm(reviewInfo.BuyerId, reviewInfo.ProductId))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        if (await SubmitReview(reviewInfo.BuyerId, reviewInfo.ProductId, form.Rating, form.Comment))
                        {
                            MessageBox.Show("Thank you for your review!", "Success");
                            LoadOrders();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting review: {ex.Message}", "Error");
            }
        }

        private async Task<bool> ReviewExists(int buyerId, int productId)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT COUNT(*) FROM reviews WHERE buyer_id = @buyerId AND product_id = @productId";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@buyerId", buyerId);
                    cmd.Parameters.AddWithValue("@productId", productId);
                    return Convert.ToInt32(await cmd.ExecuteScalarAsync()) > 0;
                }
            }
        }

        private async Task<bool> SubmitReview(int buyerId, int productId, int rating, string comment)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = @"INSERT INTO reviews (buyer_id, product_id, rating, comment, review_date)
                    VALUES (@buyerId, @productId, @rating, @comment, NOW())";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@buyerId", buyerId);
                    cmd.Parameters.AddWithValue("@productId", productId);
                    cmd.Parameters.AddWithValue("@rating", rating);
                    cmd.Parameters.AddWithValue("@comment", comment);
                    return await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadOrders();
        }
    }

    // Extension method for cleaner button event handling
    public static class ButtonExtensions
    {
        public static Button WithClickHandler(this Button button, EventHandler handler)
        {
            button.Click += handler;
            return button;
        }
    }
}