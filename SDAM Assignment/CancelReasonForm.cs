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
    public partial class CancelReasonForm : Form
    {
        private int orderId;
        public string Reason { get; private set; }

        public CancelReasonForm(int orderId, SellerOrdersForm parent = null)
        {
            InitializeComponent();
            FormStyler.ApplyTheme(this);

            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            this.orderId = orderId;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string reason = txtReason.Text.Trim();

            if (string.IsNullOrWhiteSpace(reason))
            {
                MessageBox.Show("Please enter a reason.");
                return;
            }

            Reason = reason;              
            this.DialogResult = DialogResult.OK; 
            this.Close();                 
        }
    }
}