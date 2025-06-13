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
    public partial class SellerOrdersForm : Form
    {
        private Seller seller;

        public SellerOrdersForm(Seller seller)
        {
            InitializeComponent();
            this.seller = seller;
            LoadOrders();
            FormStyler.ApplyTheme(this);
        }

        private void LoadOrders()
        {
            flowLayoutPanelOrders.Controls.Clear();
            var orders = Order.GetOrdersForSeller(seller.Id);

            foreach (var order in orders)
            {
                Product product = Product.GetById(order.ProductId);

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
                    Text = $"{product.Name} | Buyer ID: {order.BuyerId} | Status: {order.Status}",
                    AutoSize = true,
                    Left = 10,
                    Top = 10
                };

                Button btnShip = new Button
                {
                    Text = "Ship",
                    Left = 10,
                    Top = 40,
                    Width = 80,
                    Enabled = order.Status == "Pending",
                    BackColor = Color.LightBlue, 
                    ForeColor = Color.Black,
                };

                btnShip.Click += (s, e) =>
                {
                    Order.UpdateStatus(order.OrderId, "Shipped");
                    LoadOrders();
                };

                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    Left = 100,
                    Top = 40,
                    Width = 80,
                    Enabled = order.Status == "Pending",
                    BackColor = Color.IndianRed, 
                    ForeColor = Color.Black,
                    Tag = "NoTheme"
                };

                btnCancel.Click += (s, e) =>
                {
                    CancelReasonForm reasonForm = new CancelReasonForm(order.OrderId, this);
                    if (reasonForm.ShowDialog() == DialogResult.OK)
                    {
                        string reason = reasonForm.Reason;
                        Order.UpdateStatus(order.OrderId, "Cancelled", reason);
                        LoadOrders();
                    }
                };

                panel.Controls.Add(lbl);
                panel.Controls.Add(btnShip);
                panel.Controls.Add(btnCancel);
                flowLayoutPanelOrders.Controls.Add(panel);
            }
        }
    }
}
