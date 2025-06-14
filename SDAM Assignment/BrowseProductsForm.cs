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
            this.buyer = buyer;

            BuyerController.LoadProductCards(flowLayoutPanelProducts, buyer.Id);
        }

        public bool AddToCart(Product product, int quantity = 1)
        {
            return CartItemsController.AddToCart(buyer.Id, product.ProductId, quantity);
        }

        public List<CartItem> GetCartItems()
        {
            return CartItemsController.GetCartItems(buyer.Id);
        }

        public bool RemoveFromCart(int productId)
        {
            return CartItemsController.RemoveFromCart(buyer.Id, productId);
        }

    }
}

