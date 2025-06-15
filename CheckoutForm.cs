using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using SDAM_Assignment.Controllers;
using SDAM_Assignment.Helpers;

namespace SDAM_Assignment
{
    public partial class CheckoutForm : Form
    {
        private Buyer buyer;
        public CheckoutForm(Buyer buyer)
        {
            InitializeComponent();
            FormStyler.ApplyTheme(this);
            this.buyer = buyer;

            decimal total = CartItemsController.CalculateTotal(buyer.Id);
            lblTotal.Text = $"Total Amount: Rs. {total:F2}";

            ToggleCardFields(false); // Hide card fields initially

            radioCard.CheckedChanged += PaymentMethodChanged;
            radioCash.CheckedChanged += PaymentMethodChanged;
            btnConfirm.Click += BtnConfirm_Click;
        }

        private void PaymentMethodChanged(object sender, EventArgs e)
        {
            ToggleCardFields(radioCard.Checked);
        }

        private void ToggleCardFields(bool show)
        {
            lblCardNo.Visible = txtCardNo.Visible = show;
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            string address = txtAddress.Text.Trim();

            if (string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Please enter your shipping address.");
                return;
            }

            if (radioCard.Checked)
            {
                string cardNo = txtCardNo.Text.Trim();

                if (!Regex.IsMatch(cardNo, @"^\d{16}$"))
                {
                    MessageBox.Show("Card number must be 16 digits.");
                    return;
                }
            }

            List<CartItem> items = CartItemsController.GetCartItems(buyer.Id);

            if (items.Count == 0)
            {
                MessageBox.Show("Your cart is empty!");
                return;
            }

            bool success = true;

            foreach (var item in items)
            {
                bool placed = OrderController.PlaceOrder(buyer.Id, item.ProductId, address, "Pending");
                if (!placed)
                {
                    success = false;
                    MessageBox.Show($"Failed to place order for product ID: {item.ProductId}");
                }
            }

            if (success)
            {
                CartItemsController.ClearCart(buyer.Id);
                MessageBox.Show("Order placed successfully!");
                this.Close();
            }
        }
    }
}
