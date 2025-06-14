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
    public partial class ViewOrdersForm: Form
    {
        private int sellerId;
        public ViewOrdersForm(int sellerId)
        {
            InitializeComponent();
            FormStyler.ApplyTheme(this);
            this.sellerId = sellerId;
        }
    }
}
