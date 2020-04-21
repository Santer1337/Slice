using System;
using System.Windows.Forms;
using SliceAPI;

namespace Slice.NET_Form_Example
{
    public partial class LoginSuccess : Form
    {
        public LoginSuccess()
        {
            InitializeComponent();
        }

        //  Grab variables
        private void button2_Click(object sender, EventArgs e)
        {
            string varName = textBox4.Text; // Get name of variable

            string varData = Main.exampleApp.GrabVariable(varName); // Attempt to grab variable, if it doesn't exist or the user isn't logged in it will return a default message variable

            MessageBox.Show($"Variable: {varData}", "Slice.NET", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e) => Close();
    }
}
