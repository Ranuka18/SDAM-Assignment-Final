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
    public partial class BrowseProductsForm : Form
    {
        private Buyer buyer;

        public BrowseProductsForm(Buyer buyer)
        {
            InitializeComponent();
            FormStyler.ApplyTheme(this);
            this.buyer = buyer;
            buyer.LoadProductCards(flowLayoutPanelProducts);
        }

        public bool AddToCart(Product product, int quantity = 1)
        {
            return CartItem.AddToCart(buyer.Id, product.ProductId, quantity);
        }

        public List<CartItem> GetCartItems()
        {
            return CartItem.GetCartItems(buyer.Id);
        }

        public bool RemoveFromCart(int productId)
        {
            return CartItem.RemoveFromCart(buyer.Id, productId);
        }

    }
}

