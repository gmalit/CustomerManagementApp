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
            // Measure message size and determine form width
            int textWidth = TextRenderer.MeasureText(message, new Font("Segoe UI", 10)).Width;
            int formWidth = Math.Max(300, Math.Min(textWidth + 60, 600)); // Min 300, Max 600

            // Form settings
            this.Text = "Input Required";
            this.Width = formWidth;
            this.Height = 160;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Message label
            Label lblMessage = new Label()
            {
                Left = 20,
                Top = 10,
                Width = formWidth - 40,
                Text = message,
                AutoSize = true
            };
            this.Controls.Add(lblMessage);

            // Input text box
            txtInput = new TextBox()
            {
                Left = 20,
                Top = lblMessage.Bottom + 10,
                Width = formWidth - 40
            };
            this.Controls.Add(txtInput);

            // OK button
            btnOk = new Button()
            {
                Text = "OK",
                Width = 80,
                Top = txtInput.Bottom + 10,
                Left = formWidth / 2 - 90
            };
            btnOk.Click += (sender, e) => { this.InputText = txtInput.Text; this.DialogResult = DialogResult.OK; this.Close(); };
            this.Controls.Add(btnOk);

            // Cancel button
            btnCancel = new Button()
            {
                Text = "Cancel",
                Width = 80,
                Top = txtInput.Bottom + 10,
                Left = formWidth / 2 + 10
            };
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
