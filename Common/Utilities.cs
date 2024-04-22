using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using LuxeIQ.ViewModels;

namespace LuxeIQ.Common
{
    public static class Utilities
    {
        public static IWebHostEnvironment HostingEnvironment { get; set; }

        private static Random random = new Random();

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static T GetGuid<T>() where T : class
        {
            if (typeof(T) == typeof(string))
            {
                return Guid.NewGuid().ToString() as T;
            }
            else
            {
                return Guid.NewGuid() as T;
            }
        }

        public static DynamicParameters DictionaryToDynamicParameters(Dictionary<string, string> objDict)
        {
            var dbArgs = new DynamicParameters();
            foreach (var pair in objDict) dbArgs.Add(pair.Key, (string.IsNullOrEmpty(pair.Value) ? (object)DBNull.Value : (object)pair.Value));
            return dbArgs;
        }


        public static string GetEnvVarVal(string pKey)
        {
            return Environment.GetEnvironmentVariable(pKey);
        }

        public static string SHA1HashStringForUTF8String(string pStr)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(pStr);

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

        public static long ConvertToUnixTime(DateTime datetime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (long)(datetime - sTime).TotalSeconds;
        }

        public static DateTime ConvertToLocalTime(DateTime datetime)
        {
            if (datetime != DateTime.MinValue)
            {
                DateTime utcDateTime = DateTime.SpecifyKind(
                    datetime,
                    DateTimeKind.Utc);
                return utcDateTime.ToLocalTime();
            }

            return DateTime.MinValue;
        }
        public static string RemoveSpecialCharsCRMAnalytics(string strText)
        {
            return Regex.Replace(strText, @"^[0-9\$_&#@*!]", string.Empty);
        }
        public static string RemoveSpecialCharsDE(string strText)
        {
            return Regex.Replace(strText, @"[^0-9a-zA-Z\-_]", string.Empty);
        }
        public static string RemoveSpecialChars(string strText)
        {
            return Regex.Replace(strText, @"[^0-9a-zA-Z\._-]", string.Empty);
        }
        public static string RemoveSpecialCharsForPGConstraints(string strText)
        {
            return Regex.Replace(strText, @"[^0-9a-zA-Z\._]", string.Empty);
        }
        public static MatchCollection ValidSpecialChars(string strText)
        {
            MatchCollection lmatches = Regex.Matches(strText, @"[^0-9a-zA-Z\._-]");
            return lmatches;
        }

        public static MatchCollection ValidSpecialCharsDE(string strText)
        {
            MatchCollection lmatches = Regex.Matches(strText, @"[^0-9a-zA-Z\-_]");
            return lmatches;
        }
        public static MatchCollection ValidSpecialCharsCRMAnalytics(string strText)
        {
            MatchCollection lmatches = Regex.Matches(strText, @"^[0-9\$_&#@*!]");
            return lmatches;
        }
        public static string EncryptText(string stringToEncrypt)
        {
            string encryptedValue = string.Empty;
            try
            {
                using (var aesAlg = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Constants.AES_KEY, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    aesAlg.Key = pdb.GetBytes(32);
                    aesAlg.IV = pdb.GetBytes(16);
                    byte[] src = Encoding.Unicode.GetBytes(stringToEncrypt);
                    using (ICryptoTransform encrypt = aesAlg.CreateEncryptor())
                    {
                        byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);
                        encryptedValue = Convert.ToBase64String(dest);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return encryptedValue;
        }

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



        public static T GetJsonPropertyValueByKeyPath<T>(string jsonString, string keyPath) where T : class
        {
            try
            {
                if (string.IsNullOrEmpty(jsonString))
                {
                    return null;
                }

                if (string.IsNullOrEmpty(keyPath))
                {
                    if (typeof(T) == typeof(List<string>))
                    {
                        List<string> strLst = JsonConvert.DeserializeObject<T>(jsonString) as List<string>;
                        strLst = strLst.Select(s => HttpUtility.UrlDecode(s)).ToList();
                        return strLst as T;
                    }
                }
                else
                {
                    if (typeof(T) == typeof(string))
                    {
                        return ((string)JObject.Parse(jsonString).SelectToken(keyPath)) as T;
                    }
                    else if (typeof(T) == typeof(List<string>))
                    {
                        return JObject.Parse(jsonString).SelectToken(keyPath).ToList() as T;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        static public String GenerateGuid(String name)
        {
            byte[] buf = Encoding.UTF8.GetBytes(name);
            byte[] guid = new byte[16];
            if (buf.Length < 16)
            {
                Array.Copy(buf, guid, buf.Length);
            }
            else
            {
                using (SHA1 sha1 = SHA1.Create())
                {
                    byte[] hash = sha1.ComputeHash(buf);
                    // Hash is 20 bytes, but we need 16. We loose some of "uniqueness", but I doubt it will be fatal
                    Array.Copy(hash, guid, 16);
                }
            }

            // Don't use Guid constructor, it tends to swap bytes. We want to preserve original string as hex dump.
            String guidS = "{" + String.Format("{0:X2}{1:X2}{2:X2}{3:X2}-{4:X2}{5:X2}-{6:X2}{7:X2}-{8:X2}{9:X2}-{10:X2}{11:X2}{12:X2}{13:X2}{14:X2}{15:X2}",
                guid[0], guid[1], guid[2], guid[3], guid[4], guid[5], guid[6], guid[7], guid[8], guid[9], guid[10], guid[11], guid[12], guid[13], guid[14], guid[15]) + "}";

            return guidS;
        }

        public static Guid ConvertToMd5HashGUID(string value)
        {
            // convert null to empty string - null can not be hashed
            if (value == null)
                value = string.Empty;

            // get the byte representation
            var bytes = Encoding.UTF8.GetBytes(value);

            // create the md5 hash
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(bytes);

            // convert the hash to a Guid
            return new Guid(data);
        }

        public static Guid ToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        public static int ToInt(this Guid value)
        {
            byte[] bytes = value.ToByteArray();
            int iGuid = ((int)bytes[0]) | ((int)bytes[1] << 8) | ((int)bytes[2] << 16) | ((int)bytes[3] << 24);
            return iGuid;
        }


        public static string ToBase64Encoding(this string pData)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(pData);
                return Convert.ToBase64String(bytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
            return null;
        }

        public static string ToBase64Decoding(this string pData)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(pData);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
            return null;
        }

        public static IQueryable<T> GetPagingResult<T, TResult>(this IQueryable<T> pQuery, int pPageNum, int pPageSize,
        Expression<Func<T, TResult>> pOrderByProperty, bool pIsAscendingOrder, out int pRowsCount)
        {
            if (pPageSize <= 0) pPageSize = 1;

            //Total result count
            pRowsCount = pQuery.Count();

            //If page number should be > 0 else set to first page
            if (pRowsCount <= pPageSize || pPageNum <= 0) pPageNum = 1;

            //Calculate number of rows to skip on pagesize
            int excludedRows = (pPageNum - 1) * pPageSize;

            pQuery = pIsAscendingOrder ? pQuery.OrderBy(pOrderByProperty) : pQuery.OrderByDescending(pOrderByProperty);

            //Skip the required rows and take the next records of pagesize count
            return pQuery.Skip(excludedRows).Take(pPageSize);
        }


        public static DateTime ConvertTimeBySystemTimeZoneId(DateTime date, string sourceTimeZoneId = "", string destTimeZoneId = "UTC")
        {
            try
            {
                if (!string.IsNullOrEmpty(sourceTimeZoneId) && !string.IsNullOrEmpty(destTimeZoneId))
                    return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, sourceTimeZoneId, destTimeZoneId);
                else if (string.IsNullOrEmpty(destTimeZoneId))
                    return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, destTimeZoneId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }

            return date;
        }

        public static string GetSyncStatus(int syncStatus)
        {
            string result = "";
            switch (syncStatus)
            {
                case 0:
                    result = "Not Started";
                    break;
                case 1:
                    result = "Progress";
                    break;
                case 2:
                    result = "Completed";
                    break;
                case 3:
                    result = "Failed";
                    break;
                case 8:
                case 10:
                    result = "Rescheduled";
                    break;
                case 9:
                    result = "Restarted";
                    break;
                case 12:
                    result = "Interrupted";
                    break;
            }
            return result;
        }
        public static bool checkEquality<T>(T[] first, T[] second)
        {
            return first.SequenceEqual(second);
        }
        public static bool IsValidEmail(string emailAddress)
        {
            try
            {
                if (!string.IsNullOrEmpty(emailAddress) && new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(emailAddress))
                    return true;
                else
                    return false;

                //var emailChecked = new System.Net.Mail.MailAddress(emailAddress);
                //return true;
                //Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                //if (regex.Match(emailAddress).Success)
                //    return true;
                //else
                //    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static Encoding GetEncoding(MultipartSection section)
        {
            MediaTypeHeaderValue mediaType;
            var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out mediaType);
            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in 
            // most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }
            return mediaType.Encoding;
        }

        public static string DecodeBase64(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            var valueBytes = System.Convert.FromBase64String(value);
            return System.Text.Encoding.UTF8.GetString(valueBytes);
        }
        public static List<US_State> GetUS_States()
        {
            List<US_State> states;

            states = new List<US_State>(50);

            states.Add(new US_State("AL", "Alabama")); 
            states.Add(new US_State("AK", "Alaska"));
            states.Add(new US_State("AZ", "Arizona"));
            states.Add(new US_State("AR", "Arkansas"));
            states.Add(new US_State("CA", "California"));
            states.Add(new US_State("CO", "Colorado"));
            states.Add(new US_State("CT", "Connecticut"));
            states.Add(new US_State("DE", "Delaware"));
            states.Add(new US_State("DC", "District Of Columbia"));
            states.Add(new US_State("FL", "Florida"));
            states.Add(new US_State("GA", "Georgia"));
            states.Add(new US_State("HI", "Hawaii"));
            states.Add(new US_State("ID", "Idaho"));
            states.Add(new US_State("IL", "Illinois"));
            states.Add(new US_State("IN", "Indiana"));
            states.Add(new US_State("IA", "Iowa"));
            states.Add(new US_State("KS", "Kansas"));
            states.Add(new US_State("KY", "Kentucky"));
            states.Add(new US_State("LA", "Louisiana"));
            states.Add(new US_State("ME", "Maine"));
            states.Add(new US_State("MD", "Maryland"));
            states.Add(new US_State("MA", "Massachusetts"));
            states.Add(new US_State("MI", "Michigan"));
            states.Add(new US_State("MN", "Minnesota"));
            states.Add(new US_State("MS", "Mississippi"));
            states.Add(new US_State("MO", "Missouri"));
            states.Add(new US_State("MT", "Montana"));
            states.Add(new US_State("NE", "Nebraska"));
            states.Add(new US_State("NV", "Nevada"));
            states.Add(new US_State("NH", "New Hampshire"));
            states.Add(new US_State("NJ", "New Jersey"));
            states.Add(new US_State("NM", "New Mexico"));
            states.Add(new US_State("NY", "New York"));
            states.Add(new US_State("NC", "North Carolina"));
            states.Add(new US_State("ND", "North Dakota"));
            states.Add(new US_State("OH", "Ohio"));
            states.Add(new US_State("OK", "Oklahoma"));
            states.Add(new US_State("OR", "Oregon"));
            states.Add(new US_State("PA", "Pennsylvania"));
            states.Add(new US_State("RI", "Rhode Island"));
            states.Add(new US_State("SC", "South Carolina"));
            states.Add(new US_State("SD", "South Dakota"));
            states.Add(new US_State("TN", "Tennessee"));
            states.Add(new US_State("TX", "Texas"));
            states.Add(new US_State("UT", "Utah"));
            states.Add(new US_State("VT", "Vermont"));
            states.Add(new US_State("VA", "Virginia"));
            states.Add(new US_State("WA", "Washington"));
            states.Add(new US_State("WV", "West Virginia"));
            states.Add(new US_State("WI", "Wisconsin"));
            states.Add(new US_State("WY", "Wyoming"));

            return states;
        }
    }
}
