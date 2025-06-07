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

namespace SDAM_Assignment
{
    public partial class AddProductForm: Form
    {
        string imagePath = "";
        int sellerId;
        public AddProductForm(int sellerId)
        {
            InitializeComponent();
            this.sellerId = sellerId;

            btnUploadImage.Click += btnUploadImage_Click;
            btnSave.Click += btnSave_Click;
        }
        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                imagePath = ofd.FileName;
                pictureBox1.Image = Image.FromFile(imagePath);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Product product = new Product
                {
                    SellerId = sellerId,
                    Name = txtName.Text,
                    Description = txtDescription.Text,
                    Price = Convert.ToDecimal(txtPrice.Text),
                    ImagePath = imagePath
                };

                product.Save_btn();
                MessageBox.Show("Product added successfully!");

                
                txtName.Text = "";
                txtDescription.Text = "";
                txtPrice.Text = "";
                pictureBox1.Image = null;
                imagePath = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
