using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDAM_Assignment
{
    public class Buyer : User
    {
        public Buyer() : base() { }

        public override void OpenDashboard()
        {
            MessageBox.Show("Welcome Buyer! Opening Shopping Area...");
        }
    }

}
