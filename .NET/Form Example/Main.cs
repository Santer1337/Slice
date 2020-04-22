using System;
using System.Windows.Forms;
using SliceAPI;

namespace Slice.NET_Form_Example
{
    public partial class Main : Form
    {
        // Initialize your API info in a global object for future use
        internal static SliceApp exampleApp = new SliceApp("gD3KoL2PZ9enZi", "pIlOR9sXUsP6gLBt", "1.0.0");

        public Main() => InitializeComponent();

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) // If user is new
                textBox3.Enabled = true; // Allow tokens
            else // If user is already registered
                textBox3.Enabled = false; // Disallow tokens
        }

        // Main form load
        private void Main_Load(object sender, EventArgs e)
        {
            // Connect to our servers using your API info initialized
            exampleApp.Connect();

            // Check to see if the connection failed or not
            if (!SliceCore.Connected)
                Environment.Exit(0);

            // All application properties
            //MessageBox.Show(exampleApp.Name);
            //MessageBox.Show(exampleApp.Version);
            //MessageBox.Show(exampleApp.Freemode);
            //MessageBox.Show(exampleApp.Hash);

            label5.Text += Identification.ComputerUserName; // Essentially Environment.UserName
            label6.Text += Identification.ComputerName; // Essentially Environment.MachineName
            label7.Text += Identification.HardwareID; // Uses our API to grab ipv4 address
            label8.Text += Identification.IPAddress; // Combines many PC components into one string
        }

        // Log Data
        private void button2_Click(object sender, EventArgs e)
        {
            string logData = textBox4.Text; // Get the log data
            exampleApp.SendDataLog(logData); // Send the log data

            // Log function has no return value so we'll just assume it completed
            MessageBox.Show("Successfully logged information!", "Slice.NET", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Login/Register
        private void button1_Click(object sender, EventArgs e)
        {
            if (!checkBox1.Checked) // Login
            {
                SliceUser user = new SliceUser(textBox1.Text, textBox2.Text); // Initialize a new user using the entered info
                bool loginResult = user.Login(exampleApp); // Attempt to login the user using the initialized credentials
                // NOTE: The default login function has message boxes for responses from our servers, you can remove those to make your own!
                if (loginResult) // Login success
                {
                    LoginSuccess successForm = new LoginSuccess(); // We can now redirect users to the main page of our app after a login success
                    successForm.ShowDialog();
                }
                else // Login fail
                {
                   // Handle failure
                }
            }
            else // Register
            {
                SliceUser user = new SliceUser(textBox1.Text, textBox2.Text, textBox3.Text); // Initialize a new user using the entered info
                bool registerResult = user.Register(exampleApp); // Attempt to register the user using the initialized credentials
                // NOTE: The default register function has message boxes for responses from our servers, you can remove those to make your own!
                if (registerResult) // Register success
                {
                    // Handle success
                }
                else // Register fail
                {
                    // Handle failure
                }
            }
        }
    }
}
