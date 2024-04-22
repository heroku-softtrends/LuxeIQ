using System.Net.Mail;
using System.Net;
using System.Text;
using LuxeIQ.Common;

namespace LuxeIQ.Services
{
    public static class EmailServices
    {
        public static string SendAlertForUserCreateMessage(string username, string toEmail, Int64 userId)
        {
            try
            {
                if (string.IsNullOrEmpty(toEmail))
                {
                    Console.WriteLine("Error: Recipient email is null");
                    return "";
                }
                string dbName = string.Empty;
                string Subject = $"{(string.IsNullOrEmpty(username) ? "" : string.Format("[{0}] ", username))}Your LuxeIQ user created";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<div style=\"font-family:'Calibri, sans-serif';\">");
                sb.AppendLine("<h2 style=\"font-size: 16px;font-weight: bold;margin-bottom: 20px;\">LuxeIQ Services</h2>");
                bool isSend = true;
                sb.AppendLine("<h2 style=\"font-size: 14px;font-weight: bold;margin-bottom: 20px;\">Set Password using below link</h2>");
                string luserId = LuxeIQ.Common.Utilities.ToBase64Encoding(LuxeIQ.Common.Utilities.EncryptText(userId.ToString()));
                sb.AppendLine("<h2 style=\"font-size: 14px;font-weight: bold;margin-bottom: 20px;\"><a href='" + Constants.LUXEIQ_APP_URL + "setpassword?id=" + luserId + "'>Set Password</a></h2>");
                sb.AppendLine("</div>");
                if (isSend)
                {
                    using (var client = new SmtpClient(Constants.EMAIL_SERVER_NAME.Trim(), Constants.EMAIL_PORT))
                    {
                        //set Credentials
                        client.Credentials = new NetworkCredential(Constants.DecryptText(Constants.EMAIL_USER_ID.Trim()), Constants.DecryptText(Constants.EMAIL_PASSWORD.Trim()));
                        client.EnableSsl = Constants.EMAIL_ENCRYPTION.Equals("SSL", StringComparison.OrdinalIgnoreCase) ? true : false;
                        //init mailMessage instance
                        MailMessage mailMessage = new MailMessage();
                        //add from address
                        mailMessage.From = new MailAddress(Constants.DecryptText(Constants.EMAIL_USER_ID.Trim()), "Welcome to LuxiIQ system.");
                        //set subject
                        mailMessage.Subject = Subject;
                        //add to email addresses
                        mailMessage.To.Add(new MailAddress(toEmail, ""));
                        //set message 
                        mailMessage.Body = sb.ToString();
                        mailMessage.IsBodyHtml = true;
                        mailMessage.Priority = MailPriority.High;
                        client.Send(mailMessage);
                        Console.WriteLine("The mail has been sent to {0} successfully.", toEmail);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex);
                return "fail";
            }
            return "success";
        }
    }
}
