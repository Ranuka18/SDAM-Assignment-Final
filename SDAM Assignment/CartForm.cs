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
    public partial class CartForm: Form
    {
        private Buyer buyer;
        public CartForm(Buyer buyer)
        {
            InitializeComponent();
            FormStyler.ApplyTheme(this);
            this.buyer = buyer; 
            LoadCart();         
        }
        private void LoadCart()
        {
            flowLayoutPanelCart.Controls.Clear();

            var details = CartItem.GetCartDetails(buyer.Id);
            decimal total = CartItem.CalculateTotal(buyer.Id);
            lblTotal.Text = $"Total: Rs. {total:F2}";

            foreach (var (item, product) in details)
            {
                Panel panel = new Panel
                {
                    Width = 300,
                    Height = 80,
                    Tag = "NoTheme",
                    BackColor = Color.WhiteSmoke,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(5)
                };

                Label lbl = new Label
                {
                    Text = $"{product.Name} - Qty: {item.Quantity} - Rs. {product.Price * item.Quantity:F2}",
                    AutoSize = true,
                    Left = 10,
                    Top = 10
                };

                Button btnRemove = new Button
                {
                    Text = "Remove",
                    Left = 200,
                    Top = 10,
                    Width = 75,
                    BackColor = Color.Red
                };

                btnRemove.Click += (s, e) =>
                {
                    CartItem.RemoveFromCart(buyer.Id, item.ProductId);
                    LoadCart();
                };

                panel.Controls.Add(lbl);
                panel.Controls.Add(btnRemove);
                flowLayoutPanelCart.Controls.Add(panel);
            }
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            CheckoutForm checkout = new CheckoutForm(buyer);
            checkout.ShowDialog();
            LoadCart();
        }

        private void btnClearCat_Click(object sender, EventArgs e)
        {
            CartItem.ClearCart(buyer.Id);  // Clear the cart in database
            MessageBox.Show("Cart has been cleared.");
            LoadCart();
        }

       
    }
}
