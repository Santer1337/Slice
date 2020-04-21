using System;

namespace SliceAPI
{
    class Program
    {
        // NOTE: This example uses many unnecessary features that are only used to stylize the console, they are not required for Slice to function!

        // Initialize your API info in a global object for future use
        static readonly SliceApp exampleApp = new SliceApp("API_TOKEN", "APP_SECRET", "APP_VERSION");

        static void Main(string[] args)
        {
            Console.Title = "Slice.NET - Example (Created by slice.to)";

            // Connect to our servers using your API info initialized
            exampleApp.Connect();

            // Check to see if the connection failed or not
            if (!SliceCore.Connected)
            {
                WriteLineColor("\n\n   Failed to our Authentication servers! Please try again later.", ConsoleColor.DarkYellow);
                Console.ReadLine();
                return;
            }

            // All application properties
            //Console.WriteLine(exampleApp.Name);
            //Console.WriteLine(exampleApp.Version);
            //Console.WriteLine(exampleApp.Freemode);
            //Console.WriteLine(exampleApp.Hash);

            // Main menu
            Menu:
            Console.Clear();
            WriteColor("\n\n   https://slice.to the best authentication system for security, simplicity and usability!", ConsoleColor.DarkYellow);
            WriteColor("\n\n   [", ConsoleColor.DarkRed);
            WriteColor("1");
            WriteColor("]. Login\n", ConsoleColor.DarkRed);
            WriteColor("   [", ConsoleColor.DarkRed);
            WriteColor("2");
            WriteColor("]. Register\n", ConsoleColor.DarkRed);
            WriteColor("   [", ConsoleColor.DarkRed);
            WriteColor("3");
            WriteColor("]. Log Data\n", ConsoleColor.DarkRed);
            WriteColor("   [", ConsoleColor.DarkRed);
            WriteColor("4");
            WriteColor("]. PC Identification\n", ConsoleColor.DarkRed);
            WriteColor("   [", ConsoleColor.DarkRed);
            WriteColor("5");
            WriteColor("]. Exit", ConsoleColor.DarkRed);
            WriteColor("\n\n   Option: ");
            switch (Console.ReadLine())
            {
                case "1":
                    LoginExample();
                    goto Menu;
                case "2":
                    RegisterExample();
                    goto Menu;
                case "3":
                    LogExample();
                    goto Menu;
                case "4":
                    IdentificationExample();
                    goto Menu;
                case "5":
                    return;
            }
        }

        static void LoginExample()
        {
            Console.Clear();

            WriteColor("\n\n   [", ConsoleColor.DarkRed);
            WriteColor("*");
            WriteColor("] Username: ", ConsoleColor.DarkRed);
            string username = Console.ReadLine(); // Ask for username

            WriteColor("   [", ConsoleColor.DarkRed);
            WriteColor("*");
            WriteColor("] Password: ", ConsoleColor.DarkRed);
            string password = Console.ReadLine(); // Ask for password

            SliceUser user = new SliceUser(username, password); // Initialize a new user using the entered info
            bool loginResult = user.Login(exampleApp); // Attempt to login the user using the initialized credentials
            // NOTE: The default login function has message boxes for responses from our servers, you can remove those to make your own!
            if (loginResult) // Login success
            {
                WriteLineColor("   Login successful!", ConsoleColor.Green);
                // All user properties
                WriteLineColor($"   IP Address: {user.IPAddress}", ConsoleColor.Green);
                WriteLineColor($"   Hardware ID: {user.HardwareID}", ConsoleColor.Green);
            }
            else // Login fail
            {
                WriteLineColor("   Login failed!", ConsoleColor.DarkRed);
                Console.ReadLine();
                return;
            }

            // All server-sided variables will be downloaded on a successful login attempt
            WriteColor("\n   Grab Variables", ConsoleColor.DarkYellow);
            WriteColor("\n   [", ConsoleColor.DarkRed);
            WriteColor("*");
            WriteColor("] Variable Name: ", ConsoleColor.DarkRed);
            string varName = Console.ReadLine();

            // Attempt to grab variable, if it doesn't exist or the user isn't logged in it will return a default message variable
            string varData = exampleApp.GrabVariable(varName);
            WriteLineColor($"   Variable Data: {varData}", ConsoleColor.Yellow);
            Console.ReadLine();
            return;
        }

        static void RegisterExample()
        {
            Console.Clear();

            WriteColor("\n\n   [", ConsoleColor.DarkRed);
            WriteColor("*");
            WriteColor("] Username: ", ConsoleColor.DarkRed);
            string username = Console.ReadLine(); // Ask for username

            WriteColor("   [", ConsoleColor.DarkRed);
            WriteColor("*");
            WriteColor("] Password: ", ConsoleColor.DarkRed);
            string password = Console.ReadLine(); // Ask for password

            WriteColor("   [", ConsoleColor.DarkRed);
            WriteColor("*");
            WriteColor("] Token: ", ConsoleColor.DarkRed);
            string token = Console.ReadLine(); // Ask for token

            SliceUser user = new SliceUser(username, password, token); // Initialize a new user using the entered info
            bool registerResult = user.Register(exampleApp); // Attempt to register the user using the initialized credentials
            // NOTE: The default register function has message boxes for responses from our servers, you can remove those to make your own!
            if (registerResult) // Register success
            {
                WriteLineColor("   Registration successful!", ConsoleColor.Green);
            }
            else // Register fail
            {
                WriteLineColor("   Registration failed!", ConsoleColor.DarkRed);
                Console.ReadLine();
                return;
            }

            Console.ReadLine();
            return;
        }

        static void LogExample()
        {
            Console.Clear();

            WriteColor("\n\n   [", ConsoleColor.DarkRed);
            WriteColor("*");
            WriteColor("] Log Data: ", ConsoleColor.DarkRed);
            string logData = Console.ReadLine(); // Ask for data

            exampleApp.SendDataLog(logData); // Send the data to be logged
            WriteLineColor("   Data logged!", ConsoleColor.Green);

            // This exampe is very simple!
            // You can log much more important data such as:
            // - Login attempts
            // - Register attempts
            // - Tampering attempts
            // - IP changes
            // - Much more, pretty much anything you can store as a string

            Console.ReadLine();
            return;
        }

        static void IdentificationExample()
        {
            Console.Clear();

            // Pulls some PC info using custom methods
            WriteLineColor($"\n\n   PC Name: {Identification.ComputerName}"); // Essentially just Environment.MachineName
            WriteLineColor($"   PC Username: {Identification.ComputerUserName}"); // Essentially just Environment.UserName
            WriteLineColor($"   Hardware ID: {Identification.HardwareID}"); // Combines many PC components into one string
            WriteLineColor($"   IP Address: {Identification.IPAddress}"); // Uses our own API to get ipv4 address

            Console.ReadLine();
            return;
        }



        // IGNORE THIS //
        static void WriteLineColor(string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        // IGNORE THIS //

        // IGNORE THIS //
        static void WriteColor(string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }
        // IGNORE THIS //
    }
}
