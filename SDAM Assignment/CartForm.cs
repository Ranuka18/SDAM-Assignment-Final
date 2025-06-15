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
                flowLayoutPanelCart.AutoScroll = true;
                flowLayoutPanelCart.Visible = true;

                Debug.WriteLine($"Loading cart for buyer ID: {buyer?.Id}");

                List<CartItem> items = CartItemsController.GetCartItems(buyer.Id);
                Debug.WriteLine($"Found {items.Count} cart items");

                if (items.Count == 0)
                {
                    lblTotal.Text = "Your cart is empty";
                    lblTotal.Visible = true;
                    return;
                }

                decimal total = CartItemsController.CalculateTotal(buyer.Id);
                lblTotal.Text = $"Total: Rs. {total:F2}";
                lblTotal.Visible = true;

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
                        Width = flowLayoutPanelCart.Width - 30,
                        Height = 120,
                        BorderStyle = BorderStyle.FixedSingle,
                        Margin = new Padding(5),
                        BackColor = SystemColors.Control,
                        Tag = product.ProductId
                    };

                    // Product Name Label
                    Label lblName = new Label
                    {
                        Text = product.Name,
                        AutoSize = false,
                        Width = panel.Width - 100,
                        Height = 20,
                        Left = 10,
                        Top = 10,
                        Font = new Font("Segoe UI", 9, FontStyle.Bold)
                    };

                    // Price Label
                    Label lblPrice = new Label
                    {
                        Text = $"Rs. {product.Price:F2} each",
                        AutoSize = false,
                        Width = panel.Width - 100,
                        Height = 20,
                        Left = 10,
                        Top = 30,
                        Font = new Font("Segoe UI", 8)
                    };

                    // Quantity Controls
                    Button btnDecrease = new Button
                    {
                        Text = "-",
                        Width = 30,
                        Height = 30,
                        Left = 10,
                        Top = 55,
                        Tag = product.ProductId,
                        Font = new Font("Segoe UI", 9, FontStyle.Bold)
                    };
                    btnDecrease.Click += (s, e) => AdjustQuantity((int)btnDecrease.Tag, -1);

                    Label lblQuantity = new Label
                    {
                        Text = item.Quantity.ToString(),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Width = 40,
                        Height = 30,
                        Left = 45,
                        Top = 55,
                        BorderStyle = BorderStyle.FixedSingle,
                        Font = new Font("Segoe UI", 9)
                    };

                    Button btnIncrease = new Button
                    {
                        Text = "+",
                        Width = 30,
                        Height = 30,
                        Left = 90,
                        Top = 55,
                        Tag = product.ProductId,
                        Font = new Font("Segoe UI", 9, FontStyle.Bold)
                    };
                    btnIncrease.Click += (s, e) => AdjustQuantity((int)btnIncrease.Tag, 1);

                    // Item Total
                    Label lblItemTotal = new Label
                    {
                        Text = $"Total: Rs. {product.Price * item.Quantity:F2}",
                        AutoSize = false,
                        Width = panel.Width - 120,
                        Height = 20,
                        Left = 130,
                        Top = 60,
                        Font = new Font("Segoe UI", 9, FontStyle.Bold)
                    };

                    // Remove Button
                    Button btnRemove = new Button
                    {
                        Text = "Remove",
                        Left = panel.Width - 90,
                        Top = 30,
                        Width = 80,
                        Height = 25,
                        BackColor = Color.OrangeRed,
                        ForeColor = Color.White,
                        Tag = product.ProductId,
                        Font = new Font("Segoe UI", 8)
                    };
                    btnRemove.Click += (s, e) => RemoveItem((int)btnRemove.Tag);

                    panel.Controls.Add(lblName);
                    panel.Controls.Add(lblPrice);
                    panel.Controls.Add(btnDecrease);
                    panel.Controls.Add(lblQuantity);
                    panel.Controls.Add(btnIncrease);
                    panel.Controls.Add(lblItemTotal);
                    panel.Controls.Add(btnRemove);
                    flowLayoutPanelCart.Controls.Add(panel);
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

        private void AdjustQuantity(int productId, int change)
        {
            try
            {
                CartItemsController.AdjustQuantity(buyer.Id, productId, change);
                LoadCart();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating quantity: {ex.Message}",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void RemoveItem(int productId)
        {
            CartItemsController.RemoveFromCart(buyer.Id, productId);
            LoadCart();
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
