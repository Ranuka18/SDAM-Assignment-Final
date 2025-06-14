using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SDAM_Assignment
{
    public class Admin : User
    {
        public Admin() : base() { }

        public override void OpenDashboard()
        {
            AdminDashboard dashboard = new AdminDashboard(this);
            dashboard.Show();
        }

    }
}
