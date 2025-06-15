using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace SDAM_Assignment
{
    public class Buyer : User
    {
        public Buyer() : base() { }

        public override void OpenDashboard()
        {
            BuyerDashboard dashboard = new BuyerDashboard(this);
            dashboard.Show();
        }
    }
}
