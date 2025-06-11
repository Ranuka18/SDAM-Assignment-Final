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
    public partial class AdminDashboard: Form
    {
<<<<<<< HEAD
        private Admin currentAdmin;
        public AdminDashboard(Admin admin)
        {
            InitializeComponent();
            currentAdmin = admin;
=======
        private int adminId;
        public AdminDashboard(int adminId)
        {
            InitializeComponent();
            this.adminId = adminId;
>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8
        }

        private void btnViewSellers_Click(object sender, EventArgs e)
        {
<<<<<<< HEAD
            ViewSellers form = new ViewSellers(currentAdmin);
=======
            ViewSellers form = new ViewSellers();
>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8
            form.ShowDialog();
        }

        private void btnViewBuyers_Click(object sender, EventArgs e)
        {
            ViewBuyers form = new ViewBuyers();
            form.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Restart();
        }
    }
}
