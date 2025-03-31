using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomerManagementApp
{
    public partial class CustomInputDialog : Form
    {
        private TextBox txtInput;
        private Button btnOk, btnCancel;
        public string InputText { get; private set; }

        public CustomInputDialog(string message)
        {
            // Form settings
            this.Text = "Input Required";
            this.Width = 350;
            this.Height = 150;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Message label
            Label lblMessage = new Label() { Left = 20, Top = 10, Width = 300, Text = message };
            this.Controls.Add(lblMessage);

            // Input text box
            txtInput = new TextBox() { Left = 20, Top = 35, Width = 300 };
            this.Controls.Add(txtInput);

            // OK button
            btnOk = new Button() { Text = "OK", Left = 160, Width = 70, Top = 65 };
            btnOk.Click += (sender, e) => { this.InputText = txtInput.Text; this.DialogResult = DialogResult.OK; this.Close(); };
            this.Controls.Add(btnOk);

            // Cancel button
            btnCancel = new Button() { Text = "Cancel", Left = 250, Width = 70, Top = 65 };
            btnCancel.Click += (sender, e) => { this.InputText = null; this.DialogResult = DialogResult.Cancel; this.Close(); };
            this.Controls.Add(btnCancel);
        }

        public static string Show(string message)
        {
            using (CustomInputDialog dialog = new CustomInputDialog(message))
            {
                return dialog.ShowDialog() == DialogResult.OK ? dialog.InputText : null;
            }
        }
    }
}
