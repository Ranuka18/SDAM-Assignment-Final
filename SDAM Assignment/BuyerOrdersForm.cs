using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SDAM_Assignment.Helpers;

namespace SDAM_Assignment
{
    public partial class BuyerOrdersForm : Form
    {
        private Buyer buyer;

        public BuyerOrdersForm(Buyer buyer)
        {
            InitializeComponent();
            this.buyer = buyer;
            LoadOrders();
            FormStyler.ApplyTheme(this);
        }

        private void LoadOrders()
        {
            flowLayoutPanelOrders.Controls.Clear();
            var orders = Order.GetOrdersForBuyer(buyer.Id);

            foreach (var order in orders)
            {
                Product product = Product.GetById(order.ProductId);

                Panel panel = new Panel
                {
                    Width = 400,
                    Height = 140,
                    Tag = "NoTheme",
                    BackColor = Color.WhiteSmoke,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(5)
                };

                string labelText = $"Product: {product.Name} | Status: {order.Status}";
                if (order.Status == "Cancelled" && !string.IsNullOrEmpty(order.CancelReason))
                {
                    labelText += $"\nReason: {order.CancelReason}";
                }

                Label lbl = new Label
                {
                    Text = labelText,
                    AutoSize = true,
                    Left = 10,
                    Top = 10,
                    BackColor = Color.LightPink,
                };
                panel.Controls.Add(lbl);

                Button btnReceived = new Button
                {
                    Text = "Mark as Received",
                    Left = 10,
                    Top = 40,
                    Width = 150,
                    Enabled = order.Status == "Shipped"
                };
                btnReceived.Click += (s, e) =>
                {
                    Order.UpdateStatus(order.OrderId, "Received");
                    LoadOrders();
                };
                panel.Controls.Add(btnReceived);

                if (order.Status == "Pending")
                {
                    Button btnCancel = new Button
                    {
                        Text = "Cancel Order",
                        Left = 170,
                        Top = 40,
                        Width = 150,
                        BackColor = Color.OrangeRed,
                        ForeColor = Color.White
                    };

                    btnCancel.Click += (s, e) =>
                    {
                        var result = MessageBox.Show("Are you sure you want to cancel this order?", "Cancel Order", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            Order.UpdateStatus(order.OrderId, "Cancelled", "Cancelled by buyer");
                            LoadOrders();
                        }
                    };

                    panel.Controls.Add(btnCancel);
                }

                if (order.Status == "Received")
                {
                    Button btnReview = new Button
                    {
                        Text = "📝 Add Review",
                        Width = 100,
                        Height = 30,
                        Top = btnReceived.Bottom + 5,
                        Left = 10
                    };

                    btnReview.Click += (s, e) =>
                    {
                        AddReviewForm reviewForm = new AddReviewForm(buyer.Id, product.ProductId);
                        reviewForm.ShowDialog();
                    };

                    panel.Controls.Add(btnReview);
                }

                flowLayoutPanelOrders.Controls.Add(panel);
            }
        }
    }
}
