using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDAM_Assignment
{
    public partial class SellerDetailsForm: Form
    {
        private int sellerId;

        public SellerDetailsForm(int sellerId)
        {
            InitializeComponent();
            this.sellerId = sellerId;
            LoadSellerProducts();
        }

        private void LoadSellerProducts()
        {
            flowLayoutPanelProducts.Controls.Clear();

            List<Product> products = Product.LoadProductsBySeller(sellerId);

            foreach (var product in products)
            {
                Panel card = CreateProductCard(product);
                flowLayoutPanelProducts.Controls.Add(card);
            }
        }

        private Panel CreateProductCard(Product product)
        {
            Panel card = new Panel
            {
                Width = 200,
                Height = 300,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10)
            };

            PictureBox picture = new PictureBox
            {
                Width = 180,
                Height = 150,
                Top = 10,
                Left = 10,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            if (System.IO.File.Exists(product.ImagePath))
            {
                picture.Image = Image.FromFile(product.ImagePath);
            }

            Label lblName = new Label
            {
                Text = product.Name,
                Top = picture.Bottom + 5,
                Left = 10,
                Width = 180,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Label lblDesc = new Label
            {
                Text = product.Description,
                Top = lblName.Bottom + 5,
                Left = 10,
                Width = 180,
                Height = 40,
                AutoEllipsis = true
            };

            Label lblPrice = new Label
            {
                Text = $"${product.Price:F2}",
                Top = lblDesc.Bottom + 5,
                Left = 10,
                Width = 180,
                ForeColor = Color.Green,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            card.Controls.Add(picture);
            card.Controls.Add(lblName);
            card.Controls.Add(lblDesc);
            card.Controls.Add(lblPrice);

            return card;
        }
    }
}
