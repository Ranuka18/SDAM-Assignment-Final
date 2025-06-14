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
using SDAM_Assignment.Controllers;
using SDAM_Assignment.Helpers;

namespace SDAM_Assignment
{
    public partial class ViewSellers : Form
    {
        private Admin currentAdmin;

        public ViewSellers(Admin admin)
        {
            InitializeComponent();
            FormStyler.ApplyTheme(this);
            currentAdmin = admin;
            LoadSellers();
        }

        private void LoadSellers()
        {
            flowLayoutPanelSellers.Controls.Clear();

            var adminController = new AdminController();
            List<User> sellers = adminController.GetAllSellers();

            foreach (var seller in sellers)
            {
                Panel card = adminController.GenerateSellerCard(seller, LoadSellers);
                flowLayoutPanelSellers.Controls.Add(card);
            }
        }
    }
}

