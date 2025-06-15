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
    public partial class BrowseProductsForm : Form
    {
        private Buyer buyer;

        public BrowseProductsForm(Buyer buyer)
        {
            InitializeComponent();
            FormStyler.ApplyTheme(this);
            this.buyer = buyer ?? throw new ArgumentNullException(nameof(buyer));

            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                BuyerController.LoadProductCards(flowLayoutPanelProducts, buyer.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool AddToCart(Product product, int quantity = 1)
        {
            if (product == null)
            {
                MessageBox.Show("Invalid product selected", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (quantity < 1)
            {
                MessageBox.Show("Quantity must be at least 1", "Warning",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                return CartItemsController.AddToCart(buyer.Id, product.ProductId, quantity);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding to cart: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public List<CartItem> GetCartItems()
        {
            try
            {
                return CartItemsController.GetCartItems(buyer.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cart items: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<CartItem>();
            }
        }

        public bool RemoveFromCart(int productId)
        {
            try
            {
                return CartItemsController.RemoveFromCart(buyer.Id, productId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing item from cart: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            flowLayoutPanelProducts.Controls.Clear();
            LoadProducts();
        }
    }
}

