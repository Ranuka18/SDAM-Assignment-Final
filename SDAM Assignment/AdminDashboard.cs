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
    public partial class AdminDashboard: Form
    {
        private Admin currentAdmin;
        public AdminDashboard(Admin admin)
        {
            InitializeComponent();
            FormStyler.ApplyTheme(this);

            currentAdmin = admin;
        }

        private void btnViewSellers_Click(object sender, EventArgs e)
        {
            ViewSellers form = new ViewSellers(currentAdmin);
            form.ShowDialog();
        }

        private void btnViewBuyers_Click(object sender, EventArgs e)
        {
            AdminViewBuyersForm form = new AdminViewBuyersForm();
            form.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                LoginForm login = new LoginForm();
                login.Show();
            }
        }
    }
}
