using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using YamlDotNet.Core.Tokens;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LuxeIQ.Common
{
    public class ConnectionFactory : IDisposable
    {
        public IDbConnection DbConnection { get; set; }

        public ConnectionFactory() { }
        public ConnectionFactory(string pDatabaseUrl)
        {

            if (pDatabaseUrl.StartsWith("postgres://"))
            {

                DbConnection = new NpgsqlConnection(GetPGConnectionStringFromUrl(pDatabaseUrl));
            } 
            else
                DbConnection = new NpgsqlConnection(pDatabaseUrl);

            if (DbConnection.State == ConnectionState.Broken || DbConnection.State == ConnectionState.Closed)
            {
                DbConnection.Open();
            }
        }

        public void OpenConnection()
        {
            if (DbConnection.State == ConnectionState.Broken || DbConnection.State == ConnectionState.Closed)
            {
                DbConnection.Open();
            }
        }

        //public void Dispose()
        //{
        //    if (DbConnection == null) return;

        //    try
        //    {
        //        DbConnection.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error: {0}", ex.Message);
        //    }

        //    DbConnection.Dispose();
        //}

        public void Dispose()
        {
            if (DbConnection == null) return;

            if (DbConnection.State == ConnectionState.Open)
            {
                DbConnection.Close();
            }
            DbConnection.Dispose();
        }

        public static string GetPGConnectionStringFromUrl(string pDatabaseUrl, bool pIsPooling = false)
        {
            if (!string.IsNullOrEmpty(pDatabaseUrl))
            {
                string conStrParts = pDatabaseUrl.Replace("//", "");
                string[] strConn = conStrParts.Split(new char[] { '/', ':', '?' });
                strConn = strConn.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim()).ToArray();
                if (pIsPooling)
                {
                    return string.Format("Host={0};Port={1};Database={2};User ID={3};Password={4};sslmode=Require;Trust Server Certificate=true;Pooling=true;MinPoolSize=1;MaxPoolSize=30;No Reset On Close=true;Timeout=25;Command Timeout=0;Read Buffer Size=16000;Write Buffer Size=16000;", strConn[2].Substring(strConn[2].LastIndexOf("@") + 1), strConn[3], strConn[4], strConn[1], strConn[2].Substring(0, strConn[2].LastIndexOf("@")));
                }
                else
                {
                    return string.Format("Host={0};Port={1};Database={2};User ID={3};Password={4};sslmode=Require;Trust Server Certificate=true;Pooling=false;No Reset On Close=true;Timeout=25;Command Timeout=0;Read Buffer Size=16000;Write Buffer Size=16000;", strConn[2].Substring(strConn[2].LastIndexOf("@") + 1), strConn[3], strConn[4], strConn[1], strConn[2].Substring(0, strConn[2].LastIndexOf("@")));
                }
            }

            return string.Empty;
        }

        
        public static string GetConStrProperty(string pDatabaseUrl, string pPropertyName)
        {
            string propertyVal = string.Empty;
            try
            {
                if (pDatabaseUrl.ToLower().Contains("redshift"))
                {
                    string[] strConn = pDatabaseUrl.Trim().Replace("//", "").Split(new char[] { '/', ';', '?', ':' }).Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim()).ToArray();
                    propertyVal = strConn.Where(x => x.ToLower().StartsWith(pPropertyName.ToLower())).Select(x => x.Substring(x.IndexOf("=") + 1)).FirstOrDefault();
                }
                else if (pDatabaseUrl.ToLower().StartsWith("postgres://"))
                {
                    string[] strConn = pDatabaseUrl.Trim().Replace("//", "").Split(new char[] { '/', ':', '?' }).Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim()).ToArray();
                    switch (pPropertyName)
                    {
                        case "database":
                            if (strConn != null && strConn.Length > 3)
                                propertyVal = strConn[4];
                            break;
                        case "host":
                            if (strConn != null && strConn.Length > 1)
                                propertyVal = strConn[2].Substring(strConn[2].LastIndexOf("@") + 1);
                            break;
                        case "port":
                            if (strConn != null && strConn.Length > 2)
                                propertyVal = strConn[3];
                            break;
                        case "userid":
                            if (strConn != null && strConn.Length > 0)
                                propertyVal = strConn[1];
                            break;
                        case "password":
                            if (strConn != null && strConn.Length > 1)
                                propertyVal = strConn[2].Substring(0, strConn[2].LastIndexOf("@"));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
            return propertyVal;
        }
    }
}