using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SDAM_Assignment.Controllers;
using SDAM_Assignment.Helpers;

namespace SDAM_Assignment
{
    public partial class CartForm: Form
    {
        private  Buyer buyer;
        private CartItemsController cartItemsController;
        public CartForm(Buyer buyer)
        {
            InitializeComponent();
            this.buyer = buyer;
            LoadCart();
            FormStyler.ApplyTheme(this);
        }
        private void LoadCart()
        {
            try
            {
                flowLayoutPanelCart.Controls.Clear();
                flowLayoutPanelCart.AutoScroll = true; // Ensure scrolling works
                flowLayoutPanelCart.Visible = true; // Make sure panel is visible

                // Debug output
                Debug.WriteLine($"Loading cart for buyer ID: {buyer?.Id}");

                // 1. Verify cart items exist
                List<CartItem> items = CartItemsController.GetCartItems(buyer.Id);
                Debug.WriteLine($"Found {items.Count} cart items");

                if (items.Count == 0)
                {
                    lblTotal.Text = "Your cart is empty";
                    lblTotal.Visible = true;
                    return;
                }

                // 2. Calculate and display total
                decimal total = CartItemsController.CalculateTotal(buyer.Id);
                lblTotal.Text = $"Total: Rs. {total:F2}";
                lblTotal.Visible = true;

                // 3. Display each item
                foreach (var item in items)
                {
                    Debug.WriteLine($"Processing item - ProductID: {item.ProductId}, Qty: {item.Quantity}");

                    Product product = ProductController.GetById(item.ProductId);

                    if (product == null)
                    {
                        Debug.WriteLine($"Product {item.ProductId} not found!");
                        continue;
                    }

                    Debug.WriteLine($"Displaying product: {product.Name}");

                    Panel panel = new Panel
                    {
                        Width = flowLayoutPanelCart.Width - 30, // Dynamic width
                        Height = 90,
                        BorderStyle = BorderStyle.FixedSingle,
                        Margin = new Padding(5),
                        BackColor = SystemColors.Control,
                        Tag = product.ProductId // Store product ID for reference
                    };

                    Label lbl = new Label
                    {
                        Text = $"{product.Name} - Qty: {item.Quantity} - Rs. {product.Price * item.Quantity:F2}",
                        AutoSize = false,
                        Width = panel.Width - 100,
                        Height = 30,
                        Left = 10,
                        Top = 10,
                        Font = new Font("Segoe UI", 9)
                    };

                    Button btnRemove = new Button
                    {
                        Text = "Remove",
                        Left = panel.Width - 90,
                        Top = 10,
                        Width = 80,
                        Height = 30,
                        BackColor = Color.OrangeRed,
                        ForeColor = Color.White,
                        Tag = product.ProductId
                    };
                    btnRemove.Click += (s, e) => RemoveItem((int)btnRemove.Tag);

                    panel.Controls.Add(lbl);
                    panel.Controls.Add(btnRemove);
                    flowLayoutPanelCart.Controls.Add(panel);

                    // Force immediate UI update
                    panel.Refresh();
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading cart: {ex.ToString()}");
                MessageBox.Show($"Error loading cart: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            finally
            {
                flowLayoutPanelCart.PerformLayout();
            }
        }

        private void RemoveItem(int productId)
        {
            CartItemsController.RemoveFromCart(buyer.Id, productId);
            LoadCart(); // Refresh the cart
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            CheckoutForm checkout = new CheckoutForm(buyer);
            checkout.ShowDialog();
            LoadCart();
        }

        private void btnClearCat_Click(object sender, EventArgs e)
        {
            CartItemsController.ClearCart(buyer.Id);
            MessageBox.Show("Cart has been cleared.");
            LoadCart();
        }

       
    }
}
