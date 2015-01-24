using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launcher
{
    public class AuthPrompt
    {
        public static string[] ShowDialog(Form owner, string text, string caption)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParityLauncher));

            Form prompt = new Form();
            prompt.ShowInTaskbar = false;
            prompt.MaximizeBox = false;
            prompt.MinimizeBox = false;
            prompt.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            prompt.Width = 500;
            prompt.Height = 180;
            prompt.Text = caption;
            prompt.StartPosition = FormStartPosition.CenterParent;
            Label textLabel = new Label() { Left = 50, Top = 20, AutoSize = false, Width = prompt.Width - 100, Text = text };
            TextBox textBox_username = new TextBox() { Left = 50, Top = 50, Width = 400 };
            TextBox textBox_password = new TextBox() { Left = 50, Top = 80, Width = 400, UseSystemPasswordChar = true };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 110 };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox_username);
            prompt.Controls.Add(textBox_password);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            prompt.ShowDialog(owner);
            return new string[] { textBox_username.Text, textBox_password.Text };
        }
    }
}
