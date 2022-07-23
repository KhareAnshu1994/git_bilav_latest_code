using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace IndexMailerUtility
{
    class EmailHelper
    {
        static string PrimaryMailServer = ConfigurationManager.AppSettings["SmtpPrimaryMailServer"];
        static string SENDER_EMAIL = ConfigurationManager.AppSettings["SenderEmail"];
        static string[] RECEIVER_EMAIL = ConfigurationManager.AppSettings["ReceiverEmail"].Split(',');
        static string SENDER_PASS = ConfigurationManager.AppSettings["SenderPass"];
        static int PORT = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
        static bool IsSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSsl"]);

        public static bool SendEmail(string attachment_file)
        {
            try
            {

                SmtpClient smtpClient;
                smtpClient = new SmtpClient(PrimaryMailServer, PORT);
                using (MailMessage message = new MailMessage())
                {
                    message.IsBodyHtml = true;
                    MailAddress mailAddress = new MailAddress(SENDER_EMAIL);
                    message.Sender = mailAddress;
                    message.From = mailAddress;
                    foreach (string to_email in RECEIVER_EMAIL)
                    {
                        if (!string.IsNullOrWhiteSpace(to_email))
                        {
                            message.To.Add(to_email);
                        }
                    }

                    if (File.Exists(attachment_file))
                    {
                        System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(attachment_file);
                        message.Attachments.Add(attach);
                    }
                    message.Body = "<b>PFA</b>";
                    message.Subject = "Index Value File as Dated " + DateTime.Now.ToString("ddMMyyyy");
                    smtpClient.Send(message);
                    //smtpClient.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error while sending mail :" + ex.Message);
                commonHelper.WriteLog("error while sending mail :" + ex.Message);
                return false;
            }

        }
    }
}
