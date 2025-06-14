using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SDAM_Assignment
{
    public class Seller : User
    {
        public Seller() : base() { }

        public override void OpenDashboard()
        {
            if (this.Id > 0)
            {
                SellerDashBoard sellerForm = new SellerDashBoard(this.Id);
                sellerForm.Show();
            }
            else
            {
                MessageBox.Show("Invalid seller ID.");
            }
        }

    }
}
