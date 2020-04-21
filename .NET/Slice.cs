#region Includes
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
#endregion

namespace SliceAPI
{
    // Thank you for choosing Slice Authentication Services! <3 //

    // This is your code and you are meant to modify/personalize it! //

    /* WARNING: Do not modify and code below without knowledge of how it works, you may
     * damage security or completey break our functions. However this cs file is simply
     * an example of how to connect to our services and do so securely. If you find any
     * issues with security or any other aspects of this class please contact us at our
     * support email, "support@slice.to". If you do wish to personalize any code and/or
     * data below please do so with caution!                                            */

    #region API Example Version 1.0.0

    // Critical API SliceConstants [Not Recommended To Modify]
    internal static class SliceConstants
    {
        public static string Name       = "Slice Authentication";
        public static string ApiUrl     = "https://api.slice.to/auth/";
        public static string Version    = "1.0.0";
        public static string ApiChar    = "|";
        public static string UserAgent  = "Slice.NET";
    }

    // Application initialization
    internal class SliceApp
    {
        internal string ApiToken  = "";
        internal string Secret    = "";
        internal string Version   = "";

        public string Name          { get; set; }
        public string DownloadLink  { get; set; }
        public string Hash          { get; set; }
        public bool Freemode        { get; set; }

        public Dictionary<string, string> Variables = new Dictionary<string, string>();

        public SliceApp(string token, string secret, string version)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(secret) || string.IsNullOrWhiteSpace(version))
                throw new Exception("Invalid API information");

            ApiToken    = token;
            Secret      = secret;
            Version     = version;
        }
    }

    // User initialization
    internal class SliceUser
    {
        public string Username  = "";
        public string Password  = "";
        public string Token     = "ALREADY_REGISTERED";

        public string IPAddress     { get; set; }
        public string HardwareID    { get; set; }

        public SliceUser(string username, string password, string token = null)
        {
            Username = username;
            Password = Security.Hash(password);
            Token = token;
        }
    }

    // Main Authentication methods
    internal static class SliceCore
    {
        internal static bool Connected;
        internal static bool LoggedIn;
        internal static void Connect(this SliceApp app)
        {
            if (Connected)
            {
                MessageBox.Show($"This application is already connected to our servers! Please try again later or contact the developer of this SliceApp.", SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }

            string[] response = new string[] { };

            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.UserAgent, SliceConstants.UserAgent);

                    Security.StartSession();

                    string token = Guid.NewGuid().ToString();

                    response = Encoding.Default.GetString(client.UploadValues(SliceConstants.ApiUrl, new NameValueCollection
                    {
                        ["session_id"] = Security.IV,
                        ["request_id"] = Security.Key,
                        ["api_token"] = Security.Encrypt(app.ApiToken),
                        ["app_secret"] = Security.Encrypt(app.Secret),
                        ["app_version"] = Security.Encrypt(app.Version),
                        ["token"] = Security.Encrypt(token),
                        ["action"] = Security.Encrypt("connect")

                    })).Split(SliceConstants.ApiChar.ToCharArray());

                    if (response[0] != Security.Encrypt(token))
                    {
                        MessageBox.Show($"Invalid response from our servers! Please try again later.", SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    switch (Security.Decrypt(response[2])) 
                    {
                        case "success":
                            Connected = true;
                            MessageBox.Show(Security.Decrypt(response[1]), SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Information);
                            break;
                        case "error":
                            MessageBox.Show(Security.Decrypt(response[1]), SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                            Security.EndSession();
                            return;
                        case "warning":
                            MessageBox.Show(Security.Decrypt(response[1]), SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Warning);
                            Security.EndSession();
                            return;
                        default:
                            MessageBox.Show("Unknown error occured during request! Please try again later.", SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Warning);
                            Environment.Exit(0);
                            return;
                    }

                    if (Connected)
                    {
                        app.Name            = Security.Decrypt(response[3]);
                        app.Version         = Security.Decrypt(response[4]);
                        app.Freemode        = bool.Parse(Security.Decrypt(response[5]));
                        app.Hash            = Security.Decrypt(response[6]);

                        // Uncomment if you want to check hash
                        //if (app.Hash != Security.Md5CheckSum(Process.GetCurrentProcess().MainModule.FileName))
                        //{
                        //    MessageBox.Show($"Couldn't verify the integrity of this application!", SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                        //    Environment.Exit(0);
                        //}
                    }
                }
                catch
                {
                    MessageBox.Show($"Error while connecting to our authentication servers!", SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(0);
                }
            }

            if (Security.SessionStarted)
                Security.EndSession();

            return;
        }
        internal static void SendDataLog(this SliceApp app, string data)
        {
            if (!Connected)
            {
                MessageBox.Show($"This applications isn't connected to our servers!", SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }

            string[] response = new string[] { };

            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.UserAgent, SliceConstants.UserAgent);

                    Security.StartSession();

                    string token = Guid.NewGuid().ToString();

                    response = Encoding.Default.GetString(client.UploadValues(SliceConstants.ApiUrl, new NameValueCollection
                    {
                        ["session_id"]      = Security.IV,
                        ["request_id"]      = Security.Key,
                        ["api_token"]       = Security.Encrypt(app.ApiToken),
                        ["app_secret"]      = Security.Encrypt(app.Secret),
                        ["log_data"]        = Security.Encrypt(data),
                        ["token"]           = Security.Encrypt(token),
                        ["action"]          = Security.Encrypt("log")
                    })).Split(SliceConstants.ApiChar.ToCharArray());

                    if (response[0] != Security.Encrypt(token))
                    {
                        MessageBox.Show($"Invalid response from our servers! Please try again later.", SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // We do not check if the log failed to keep it slient (It most likely didn't)
                }
                catch
                {
                    MessageBox.Show($"Error while connecting to our authentication servers!", SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(0);
                }
            }

            if (Security.SessionStarted)
                Security.EndSession();

            return;
        }
        internal static bool Login(this SliceUser user, SliceApp app)
        {
            if (!Connected)
            {
                MessageBox.Show($"This application isn't connected to our servers!", SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }

            if (app.Freemode)
                return true;

            string[] response = new string[] { };

            bool Successful = false;

            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.UserAgent, SliceConstants.UserAgent);

                    Security.StartSession();

                    string token = Guid.NewGuid().ToString();

                    response = Encoding.Default.GetString(client.UploadValues(SliceConstants.ApiUrl, new NameValueCollection
                    {
                        ["session_id"] = Security.IV,
                        ["request_id"] = Security.Key,
                        ["api_token"] = Security.Encrypt(app.ApiToken),
                        ["app_secret"] = Security.Encrypt(app.Secret),
                        ["username"] = Security.Encrypt(user.Username),
                        ["password"] = Security.Encrypt(user.Password),
                        ["token"] = Security.Encrypt(token),
                        ["action"] = Security.Encrypt("login")
                    })).Split(SliceConstants.ApiChar.ToCharArray());

                    if (response[0] != Security.Encrypt(token))
                    {
                        MessageBox.Show($"Invalid response from our servers! Please try again later.", SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }

                    switch (Security.Decrypt(response[2]))
                    {
                        case "success":
                            Successful = true;
                            LoggedIn = true;
                            MessageBox.Show(Security.Decrypt(response[1]), SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Information);
                            break;
                        case "error":
                            MessageBox.Show(Security.Decrypt(response[1]), SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                            Security.EndSession();
                            return false;
                        case "warning":
                            MessageBox.Show(Security.Decrypt(response[1]), SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Warning);
                            Security.EndSession();
                            return false;
                        default:
                            MessageBox.Show("Unknown error occured during request! Please try again later.", SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Warning);
                            Environment.Exit(0);
                            return false;
                    }

                    if (Successful)
                    {
                        user.IPAddress = Security.Decrypt(response[3]);
                        user.HardwareID = Security.Decrypt(response[4]);

                        // This is pretty hacky and will be refined
                        string tempVars = Security.Decrypt(response[5]);
                        foreach (string pair in tempVars.Split('~'))
                        {
                            string[] items = pair.Split('^');
                            try
                            {
                                app.Variables.Add(items[0], items[1]);
                            }
                            catch
                            {
                                // TODO: Maybe add a handler for failed variables
                            }
                        }

                        Security.EndSession();
                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error while connecting to our authentication servers!\n{ex.ToString()}", SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            if (Security.SessionStarted)
                Security.EndSession();

            return false;
        }
        internal static bool Register(this SliceUser user, SliceApp app)
        {
            if (!Connected)
            {
                MessageBox.Show($"This application isn't connected to our servers!", SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }

            string[] response = new string[] { };

            bool Successful = false;

            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.UserAgent, SliceConstants.UserAgent);

                    Security.StartSession();

                    string token = Guid.NewGuid().ToString();

                    response = Encoding.Default.GetString(client.UploadValues(SliceConstants.ApiUrl, new NameValueCollection
                    {
                        ["session_id"] = Security.IV,
                        ["request_id"] = Security.Key,
                        ["api_token"] = Security.Encrypt(app.ApiToken),
                        ["app_secret"] = Security.Encrypt(app.Secret),
                        ["username"] = Security.Encrypt(user.Username),
                        ["password"] = Security.Encrypt(user.Password),
                        ["user_token"] = Security.Encrypt(user.Token),
                        ["hwid"] = Security.Encrypt(Identification.HardwareID),
                        ["token"] = Security.Encrypt(token),
                        ["action"] = Security.Encrypt("register")
                    })).Split(SliceConstants.ApiChar.ToCharArray());

                    if (response[0] != Security.Encrypt(token))
                    {
                        MessageBox.Show($"Invalid response from our servers! Please try again later.", SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }

                    switch (Security.Decrypt(response[2]))
                    {
                        case "success":
                            Successful = true;
                            MessageBox.Show(Security.Decrypt(response[1]), SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Information);
                            break;
                        case "error":
                            MessageBox.Show(Security.Decrypt(response[1]), SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                            Security.EndSession();
                            return false;
                        case "warning":
                            MessageBox.Show(Security.Decrypt(response[1]), SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Warning);
                            Security.EndSession();
                            return false;
                        default:
                            MessageBox.Show("Unknown error occured during request! Please try again later.", SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Warning);
                            Environment.Exit(0);
                            return false;
                    }

                    if (Successful)
                        Security.EndSession();
                        return true;
                }
                catch
                {
                    MessageBox.Show($"Error while connecting to our authentication servers!", SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            if (Security.SessionStarted)
                Security.EndSession();

            return false;
        }
        internal static bool Logout(this SliceUser user)
        {
            if (!Connected || !LoggedIn)
            {
                MessageBox.Show($"This application has been setup incorrectly!", SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }

            user.IPAddress      = string.Empty;
            user.HardwareID     = string.Empty;
            user.Username       = string.Empty;
            user.Password       = string.Empty;

            return true;
        }
        public static string GrabVariable(this SliceApp app, string name)
        {
            try
            {
                if (LoggedIn || app.Freemode)
                    return app.Variables[name];
                else
                    return "[Not logged in!]";
            }
            catch
            {
                return "[The requested variable doesn't exist!]";
            }
        }
    }

    // Security/Encryption methods
    internal static class Security
    {
        internal static string Key { get; set; }
        internal static string IV { get; set; }

        internal static bool SessionStarted = false;

        internal static Random random = new Random();

        private static string RandonLinqString(int length)
        {
            return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz", length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        internal static void StartSession()
        {
            if (SessionStarted)
            {
                MessageBox.Show($"Warning: A session is already in progress and cannot be restarted! ", SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SessionStarted = true;
            Key = Convert.ToBase64String(Encoding.Default.GetBytes(RandonLinqString(32)));
            IV = Convert.ToBase64String(Encoding.Default.GetBytes(RandonLinqString(16)));
        }

        internal static void EndSession()
        {
            if (!SessionStarted)
            {
                MessageBox.Show($"Warning: Cannot end session as no session is currently started! ", SliceConstants.Name, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SessionStarted = false;
            Key = null;
            IV = null;
        }

        public static string Md5CheckSum(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(input));

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                    sb.Append(hashBytes[i].ToString("X2"));

                return sb.ToString();
            }
        }

        internal static string Encrypt(string value) => EncryptString(value, SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(Encoding.Default.GetString(Convert.FromBase64String(Key)))), Encoding.ASCII.GetBytes(Encoding.Default.GetString(Convert.FromBase64String(IV))));

        internal static string Decrypt(string value) => DecryptString(value, SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(Encoding.Default.GetString(Convert.FromBase64String(Key)))), Encoding.ASCII.GetBytes(Encoding.Default.GetString(Convert.FromBase64String(IV))));

        private static string EncryptString(string plainText, byte[] key, byte[] iv)
        {
            using (var encryptor = Aes.Create())
            {
                encryptor.Mode = CipherMode.CBC;
                encryptor.Key = key;
                encryptor.IV = iv;

                using (var memoryStream = new MemoryStream())
                {
                    using (var aesEncryptor = encryptor.CreateEncryptor())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write))
                        {

                            byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);

                            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                            cryptoStream.FlushFinalBlock();

                            byte[] cipherBytes = memoryStream.ToArray();

                            return Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);
                        }
                    }
                }
            }
        }

        private static string DecryptString(string cipherText, byte[] key, byte[] iv)
        {
            using (var encryptor = Aes.Create())
            {
                encryptor.Mode = CipherMode.CBC;
                encryptor.Key = key;
                encryptor.IV = iv;

                using (var memoryStream = new MemoryStream())
                {
                    using (var aesDecryptor = encryptor.CreateDecryptor())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write))
                        {

                            byte[] cipherBytes = Convert.FromBase64String(cipherText);

                            cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                            cryptoStream.FlushFinalBlock();

                            byte[] plainBytes = memoryStream.ToArray();

                            return Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
                        }
                    }
                }
            }
        }

        internal static string Hash(string randomString)
        {
            StringBuilder hash = new StringBuilder();
            foreach (byte theByte in new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(randomString))) hash.Append(theByte.ToString("x2"));
            return hash.ToString();
        }
    }

    // Hardware Identification methods
    internal static class Identification
    {
        public static string HardwareID
        {
            get
            {
                string drive = "C";
                foreach (DriveInfo compDrive in DriveInfo.GetDrives())
                {
                    if (compDrive.IsReady)
                    {
                        drive = compDrive.RootDirectory.ToString();
                        break;
                    }
                }

                if (drive.EndsWith(":\\"))
                    drive = drive.Substring(0, drive.Length - 2);

                ManagementObject disk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
                disk.Get();

                string cpuID = "";
                ManagementClass managClass = new ManagementClass("win32_processor");
                ManagementObjectCollection managCollec = managClass.GetInstances();
                foreach (ManagementObject managObj in managCollec)
                {
                    if (cpuID == "")
                    {
                        cpuID = managObj.Properties["processorID"].Value.ToString();
                        break;
                    }
                }

                return Convert.ToBase64String(Encoding.UTF8.GetBytes(cpuID.Substring(13) + cpuID.Substring(1, 4) + disk["VolumeSerialNumber"].ToString() + cpuID.Substring(4, 4)));
            }
        }

        public static string ComputerUserName
        {
            get
            {
                return Environment.UserName;
            }
        }

        public static string ComputerName
        {
            get
            {
                return Environment.MachineName;
            }
        }

        public static string IPAddress
        {
            get
            {
                try
                {
                    WebClient client = new WebClient();
                    return client.DownloadString("https://api.slice.to/auth/?ip");
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
    }
    #endregion
}
