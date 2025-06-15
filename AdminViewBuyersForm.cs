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
    public partial class AdminViewBuyersForm : Form
    {
        public AdminViewBuyersForm()
        {
            InitializeComponent();
            LoadBuyers();
            FormStyler.ApplyTheme(this);
        }

        private void LoadBuyers()
        {
            flowLayoutPanelBuyers.Controls.Clear();
            List<Buyer> buyers = BuyerController.GetAllBuyers();

            foreach (var buyer in buyers)
            {
                Panel panel = new Panel
                {
                    Width = 350,
                    Height = 100,
                    Tag = "NoTheme",
                    BackColor = Color.WhiteSmoke,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(5)
                };

                Label lbl = new Label
                {
                    Text = $"ID: {buyer.Id}\nName: {buyer.Name}\nEmail: {buyer.Email}\nPhone: {buyer.Phone}",
                    AutoSize = true,
                    Left = 10,
                    Top = 10
                };

                Button btnDelete = new Button
                {
                    Text = "Delete",
                    Width = 80,
                    Left = 250,
                    Top = 10,
                    BackColor = Color.Red
                };

                btnDelete.Click += (s, e) =>
                {
                    if (MessageBox.Show("Are you sure you want to delete this buyer?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        bool success = BuyerController.DeleteBuyer(buyer.Id);
                        LoadBuyers();
                    }
                };

                panel.Controls.Add(lbl);
                panel.Controls.Add(btnDelete);
                flowLayoutPanelBuyers.Controls.Add(panel);
            }
        }
    }
}