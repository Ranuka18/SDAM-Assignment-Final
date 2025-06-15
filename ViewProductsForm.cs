using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace Seller
{
    public partial class ViewProductsForm: Form
    {
        public ViewProductsForm()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            flowLayoutPanelProducts.Controls.Clear();

            List<Product> products = Product.LoadProducts();

            foreach (var product in products)
            {
                Panel card = CreateProductCard(product.Name, product.Description, product.Price, product.ImagePath);
                flowLayoutPanelProducts.Controls.Add(card);
            }
        }

        private Panel CreateProductCard(string name, string description, decimal price, string imagePath)
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
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = File.Exists(imagePath) ? Image.FromFile(imagePath) : null
            };

            Label lblName = new Label
            {
                Text = name,
                Top = picture.Bottom + 5,
                Left = 10,
                Width = 180,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Label lblDesc = new Label
            {
                Text = description,
                Top = lblName.Bottom + 5,
                Left = 10,
                Width = 180,
                Height = 40,
                AutoEllipsis = true
            };

            Label lblPrice = new Label
            {
                Text = $"${price:F2}",
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
