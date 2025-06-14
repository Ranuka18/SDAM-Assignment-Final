using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SDAM_Assignment.Controllers;
using SDAM_Assignment.Helpers;

namespace SDAM_Assignment
{
    public partial class SellerOrdersForm : Form
    {
        private readonly Seller seller;
        private readonly int productId;
        public bool OrdersResolved { get; private set; }

        public SellerOrdersForm(Seller seller, int productId)
        {
            InitializeComponent();
            this.seller = seller;
            this.productId = productId;
            LoadOrdersForProduct();
            FormStyler.ApplyTheme(this);
        }

        private void LoadOrdersForProduct()
        {
            flowLayoutPanelOrders.Controls.Clear();
            var orders = OrderController.GetOrdersForProduct(productId);

            if (!orders.Any())
            {
                var emptyLabel = new Label
                {
                    Text = "No orders found for this product.",
                    AutoSize = true,
                    Font = new Font("Arial", 10, FontStyle.Italic)
                };
                flowLayoutPanelOrders.Controls.Add(emptyLabel);
                return;
            }

            foreach (var order in orders)
            {
                Product product = ProductController.GetById(order.ProductId);

                Panel panel = new Panel
                {
                    Width = 400,
                    Height = 100,
                    Tag = "NoTheme",
                    BackColor = Color.WhiteSmoke,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(5)
                };

                Label lbl = new Label
                {
                    Text = $"{product.Name}\nBuyer ID: {order.BuyerId}\nStatus: {order.Status}",
                    AutoSize = true,
                    Left = 10,
                    Top = 10
                };

                Button btnComplete = new Button
                {
                    Text = "Complete",
                    Left = 10,
                    Top = 60,
                    Width = 80,
                    Enabled = order.Status == "Pending" || order.Status == "Shipped",
                    BackColor = Color.LightGreen,
                    ForeColor = Color.Black,
                };

                btnComplete.Click += (s, e) =>
                {
                    OrderController.UpdateStatus(order.OrderId, "Completed");
                    LoadOrdersForProduct();
                    OrdersResolved = true;
                };

                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    Left = 100,
                    Top = 60,
                    Width = 80,
                    Enabled = order.Status == "Pending",
                    BackColor = Color.IndianRed,
                    ForeColor = Color.Black,
                    Tag = "NoTheme"
                };

                btnCancel.Click += (s, e) =>
                {
                    CancelReasonForm reasonForm = new CancelReasonForm(order.OrderId);
                    if (reasonForm.ShowDialog() == DialogResult.OK)
                    {
                        OrderController.UpdateStatus(order.OrderId, "Cancelled", reasonForm.Reason);
                        LoadOrdersForProduct();
                        OrdersResolved = true;
                    }
                };

                panel.Controls.Add(lbl);
                panel.Controls.Add(btnComplete);
                panel.Controls.Add(btnCancel);
                flowLayoutPanelOrders.Controls.Add(panel);
            }
        }

        private void btnResolveAll_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Mark all pending orders as completed?", "Confirm",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                if (OrderController.ResolveAllOrdersForProduct(productId))
                {
                    OrdersResolved = true;
                    MessageBox.Show("All orders marked as completed.", "Success",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadOrdersForProduct();
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = OrdersResolved ? DialogResult.OK : DialogResult.Cancel;
            this.Close();
        }
    }
}