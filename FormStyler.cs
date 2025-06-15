using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDAM_Assignment.Helpers
{
    public static class FormStyler
    {
        public static readonly Color PanelBackgroundColor = Color.FromArgb(255, 128, 69); // Background color for cards
        public static readonly Color ControlBackgroundColor = Color.FromArgb(255, 204, 153); // Background color for buttons, textboxes, dropdowns
        public static readonly Color TextColor = Color.Black; // Text color for all controls

        // This method applies the theme to controls
        public static void ApplyTheme(Control control)
        {
            // Skip controls that should not be themed
            if (control.Tag != null && control.Tag.ToString() == "NoTheme")
                return;

            // Apply background color and text color
            control.BackColor = ControlBackgroundColor;
            control.ForeColor = TextColor;

            if (control is Button btn)
            {
                btn.BackColor = ControlBackgroundColor;
                btn.ForeColor = TextColor;
            }


            // Recurse into child controls
            foreach (Control child in control.Controls)
            {
                ApplyTheme(child);
            }
        }
    }
}