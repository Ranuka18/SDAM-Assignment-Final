using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mysqlx.Crud;
using SDAM_Assignment.Controllers;
using SDAM_Assignment.Helpers;

namespace SDAM_Assignment
{
    public partial class SellerAllOrdersForm : Form
    {
        private readonly Seller seller;

        public SellerAllOrdersForm(Seller seller)
        {
            InitializeComponent();
            this.seller = seller;
            LoadOrders();  
            FormStyler.ApplyTheme(this);
        }

        private void LoadOrders() 
        {
            flowLayoutPanelOrders.Controls.Clear();

            var orders = OrderController.GetOrdersForSeller(seller.Id);

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
                    OrderController.UpdateStatus(order.OrderId, "Shipped");
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
                    using (var reasonForm = new CancelReasonForm(order.OrderId))
                        if (reasonForm.ShowDialog() == DialogResult.OK)
                    {
                        string reason = reasonForm.Reason;
                        OrderController.UpdateStatus(order.OrderId, "Cancelled", reason);
                        LoadOrders();
                    }
                };

                panel.Controls.Add(lbl);
                panel.Controls.Add(btnShip);
                panel.Controls.Add(btnCancel);
                flowLayoutPanelOrders.Controls.Add(panel);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadOrders();  
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}