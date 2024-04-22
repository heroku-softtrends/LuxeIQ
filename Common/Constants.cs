using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LuxeIQ.Common
{
    public static class Constants
    {
        public const string AES_KEY = "TWFya2V0aW5nQ29ubmVjdG9yMjAxN0BTb2Z0cmVuZHM=";
        public const int RESTCLIENT_TIMEOUT = 120000;
        public const int RESTCLIENT_READWRITETIMEOUT = 3200;
        public const string ADDON_DB_DEFAULT_SCHEMA = "public";
        public const string POSTGRES_DEFAULT_SCHEMA = "public";
        public const string DEFAULT_AUTH_COOKIE_SCHEME = "LuxeIQCookieScheme";
        public const string DEFAULT_AUTH_COOKIE_NAME = ".LuxeIQCookies";
        public const string DEFAULT_AUTH_SESSION_NAME = ".LuxeIQSessions";
        public const string HEROKU_ACCESS_TOKEN = "HATOKEN";
        public const string HEROKU_REFRESH_TOKEN = "HRTOKEN";
        public const string HEROKU_TOKEN_EXPIREDIN = "HTEXPIREDIN";
        public const string HEROKU_USERID = "HRUSERID";
        public const string HEROKU_USER_NAME = "HRUSERNAME";
        //public static readonly string username = "admin";
        //public static readonly string password = "a";
        public static readonly string username = Environment.GetEnvironmentVariable("USERNAME");
        public static readonly string password = Environment.GetEnvironmentVariable("PASSWORD");

        public static readonly string connectionString = ConnectionFactory.GetPGConnectionStringFromUrl(Environment.GetEnvironmentVariable("DATABASE_URL"));
       // public static readonly string connectionString = ConnectionFactory.GetPGConnectionStringFromUrl("postgres://ndncicgyngplvp:dda13daafd1b188937e9ff4bd785d5a6e7fd7a994ead0c48e88a8a7567ede099@ec2-3-210-218-157.compute-1.amazonaws.com:5432/df66mpo80a7mv");

        //Email credentials
        public const string EMAIL_SERVER_NAME = "smtp.office365.com";
        public const string EMAIL_USER_ID = "HHSCWN9XNl/1BxmWoCA6iWuii0Bc8NEbSHg/XVU9nugyMPZ2ROc6JUx4IkGKbg2E";
        public const string EMAIL_PASSWORD = "nRPeGSuaHoCydhqgC7h1XZku8/Tswvi50f9XTInhFxc=";
        public const int EMAIL_PORT = 587;
        public const string EMAIL_ENCRYPTION = "SSL";
        public const int maxIntValue = int.MaxValue;

        //public static readonly int maxLoopCountForImport = Environment.GetEnvironmentVariable("MAX_LOOP_COUNT");
        public static int maxLoopCountForImport = 1000;

        //public static readonly string LUXEIQ_APP_URL = "https://localhost:44393/"; 
        public static readonly string LUXEIQ_APP_URL = Environment.GetEnvironmentVariable("LUXEIQ_APP_URL");

        public static string LUXEIQ_LOGIN_USER = "";
        public static long? LUXEIQ_Manufacturer_Territory = 0;
        public static string? LUXEIQ_LOGIN_USER_TYPE = "";
        public static long? LUXEIQ_LOGIN_USER_ID = 0;
        public static string DecryptText(string encryptedText)
        {
            string decryptedValue = string.Empty;
            try
            {
                using (var aesAlg = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Constants.AES_KEY, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    aesAlg.Key = pdb.GetBytes(32);
                    aesAlg.IV = pdb.GetBytes(16);
                    var src = Convert.FromBase64String(encryptedText);
                    using (ICryptoTransform decrypt = aesAlg.CreateDecryptor())
                    {
                        byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                        decryptedValue = Encoding.Unicode.GetString(dest);
                    }
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
            }
            return decryptedValue;
        }

    }
}
