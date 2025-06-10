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
        private int adminId;
        public AdminDashboard(int adminId)
        {
            InitializeComponent();
            this.adminId = adminId;
        }

        private void btnViewSellers_Click(object sender, EventArgs e)
        {
            ViewSellers form = new ViewSellers();
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
