using OpenPop.Mime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPop.Pop3;
using BilavEmailUtility.Classes;
using System.IO;
using System.Net.Mail;
using Npgsql;

namespace BilavEmailUtility
{
    public enum EmailStatus
    {
        InvalidAttachmentFormat,
        NoAttachments,
        RecipientNotRegistered,
        RecipientNotRegisteredAndTimestamped,
        RecipientRegistered,
        RegisteredANDTimestamped,
        Timestamped,
        UnableToReadOrTimestamp
    }
    public class EmailReading
    {
        public void ProcessEmails()
        {
            string PrimaryMailServer = ConfigurationManager.AppSettings["PrimaryMailServer"];
            string SecondryMailServer = ConfigurationManager.AppSettings["SecondryMailServer"];
            string MailPort = ConfigurationManager.AppSettings["MailServerPort"];
            string CorporateEmail = ConfigurationManager.AppSettings["CorporateEmailId"];
            string CorporatPass = ConfigurationManager.AppSettings["CorporateEmailPass"];
            bool MailServerUseSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["MailServerUseSSL"]);
            string MailNotReadToRecipients = ConfigurationManager.AppSettings["MailNotReadToRecipients"];

            int TotalEmailCount = 0;
            EmailSettings EmlSettins = new EmailSettings();
            Message EmailMessage = null;
            DataTable dtExisting = getDataTable("SELECT * FROM emailaudit WHERE date(createddate)=date(current_date)");
            DataRow[] aRow;
            Pop3Client PopClient = new OpenPop.Pop3.Pop3Client();
            List<string> mailCcIds = new List<string>();
            List<string> messageUids = new List<string>();
            int currentMessageId = 0;

            try
            {
                currentMessageId = 0;
                Helper.WriteLog("Email Utility started");
                Console.WriteLine("Email Utility started");
                PopClient.Connect(PrimaryMailServer, Convert.ToInt16(MailPort), MailServerUseSSL);
                Console.WriteLine("connected to exchange server !");
                PopClient.Authenticate(CorporateEmail, CorporatPass, AuthenticationMethod.UsernameAndPassword);
                Console.WriteLine("Opened mailbox:");
                TotalEmailCount = PopClient.GetMessageCount();
                Helper.WriteLog("Total mail count :" + TotalEmailCount);
                messageUids = PopClient.GetMessageUids();
            }
            catch (Exception ex)
            {
                Helper.WriteLog("Email Utility trying to connect to mail server " + PrimaryMailServer + " and " + SecondryMailServer + " Error " + ex.ToString());
                SendEmail(MailNotReadToRecipients.Split(','), null, "Unable to connect to Mail server" + PrimaryMailServer + " and " + SecondryMailServer, "Dear Team, </br> </br> DTS Email Utility was unable to Connect to mailserver " + PrimaryMailServer + " and " + SecondryMailServer + ". </br> Error </br> " + ex.ToString(), null, null);
                Console.WriteLine("Unable to connect to Mail server " + SecondryMailServer + " Error is " + ex.ToString());
                return;

            }

            for (int i = 0; i < TotalEmailCount; i++)
            {
                try
                {
                    EmlSettins = new EmailSettings();
                    mailCcIds = new List<string>();
                    currentMessageId = i + 1;

                    aRow = dtExisting.Select("MessageUid = '" + messageUids[i].ToString() + "'");
                    if (aRow.Length < 1)
                    {
                        EmailMessage = PopClient.GetMessage(currentMessageId);

                        Console.WriteLine(EmailMessage.Headers.From.Address.ToString());
                        Helper.WriteLog("Email From : " + EmailMessage.Headers.From.Address.ToString() + " | Subject : " + EmailMessage.Headers.Subject);
                        PreserveEmail(EmailMessage, messageUids[i]);
                        Console.WriteLine(EmailMessage.Headers.From.Address.ToString());
                    }
                    else
                        continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to read email at index " + currentMessageId.ToString() + " " + ex.ToString());
                    Helper.WriteLog("Unable to read email at index " + currentMessageId.ToString() + " " + ex.ToString());
                    SendEmail(MailNotReadToRecipients.Split(','), null, "Unable to read email at index " + currentMessageId.ToString(), "Dear Team, </br> </br> DTS Email Utility was unable to read email at index " + currentMessageId.ToString() + ". </br> Error </br> " + ex.ToString(), null, null);
                    continue;
                }

                EmlSettins.EmailReceivedDateTime = EmailMessage.Headers.DateSent.ToLocalTime();
                if (EmlSettins.EmailReceivedDateTime.Date.CompareTo(DateTime.Now.Date) != 0)
                    continue;
                EmlSettins.EmailReceivedFrom = EmailMessage.Headers.From.Address.ToString();

                EmlSettins.EmailAttachment = EmailMessage.FindAllAttachments();

                VaildateAttachments(EmlSettins);
            }
        }
        private DateTime getEmailDate(Message message)
        {
            string mailHeaderIdentifier = ConfigurationManager.AppSettings.Get("MailHeaderIdentifier").ToString();
            DateTime mailDateTime = new DateTime(0001, 01, 1);

            for (int i = 0; i < message.Headers.Received.Count; i++)
            {
                if (!string.IsNullOrEmpty(mailHeaderIdentifier))
                {
                    string[] hIdentifiers = mailHeaderIdentifier.Split(',');

                    for (int j = 0; j < hIdentifiers.Length; j++)
                    {
                        if (message.Headers.Received[i].Raw.ToString().ToLower().Trim().Contains(hIdentifiers[j].Trim().ToLower()))
                        {
                            if (mailDateTime != null)
                            {
                                if (message.Headers.Received[i].Date.ToLocalTime().CompareTo(mailDateTime) >= 0)
                                    mailDateTime = message.Headers.Received[i].Date.ToLocalTime();
                            }
                            else
                                mailDateTime = message.Headers.Received[i].Date.ToLocalTime();
                        }
                    }
                }
            }
            if (mailDateTime.CompareTo(new DateTime(0001, 01, 1)) == 0)
                mailDateTime = message.Headers.DateSent.ToLocalTime();

            return mailDateTime;
        }

        private void SendEmail(string[] toRecipients, string[] ccRecipients, string subject, string emailBody, string[] attachments, string attachmentDir = "")
        {
            string PrimaryMailServer = ConfigurationManager.AppSettings["SmtpPrimaryMailServer"];
            string SecondryMailServer = ConfigurationManager.AppSettings["SmtpSecondryMailServer"];
            string SenderEmailId = "";
            int serverFlag = 0;

            if (ccRecipients == null)
                SenderEmailId = ConfigurationManager.AppSettings["DTSAdmin"];
            else
                SenderEmailId = ConfigurationManager.AppSettings["SenderEmailId"];

            string EmailTemplateDir = ConfigurationManager.AppSettings["EmailTemplateDir"];
            SmtpClient smtpClient;

        retry:
            if (serverFlag == 2)
                return;
            if (serverFlag == 0)
                smtpClient = new SmtpClient(PrimaryMailServer);
            else
                smtpClient = new SmtpClient(SecondryMailServer);

            try
            {

                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                MailAddress mailAddress = new MailAddress(SenderEmailId);
                message.Sender = mailAddress;
                message.From = mailAddress;
                message.Body = emailBody;
                message.Subject = subject;

                if (toRecipients != null)
                {
                    toRecipients = toRecipients.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    for (int i = 0; i < toRecipients.Length; i++)
                    {
                        if (!toRecipients[i].Contains("corporate@uti.co.in"))
                            message.To.Add(toRecipients[i]);
                    }
                }

                if (ccRecipients != null)
                {
                    ccRecipients = ccRecipients.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    for (int i = 0; i < ccRecipients.Length; i++)
                    {
                        if (!ccRecipients[i].Contains("corporate@uti.co.in"))
                            message.CC.Add(ccRecipients[i]);
                    }
                }

                if (attachments != null)
                    for (int k = 0; k < attachments.Length; k++)
                    {
                        if (File.Exists(attachmentDir + "\\" + attachments[k]))
                        {
                            System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(attachmentDir + "\\" + attachments[k]);
                            message.Attachments.Add(attach);
                        }
                    }

                smtpClient.Send(message);
                smtpClient.Dispose();
            }
            catch (SmtpException se)
            {
                serverFlag++;
                Helper.WriteLog("Error sending email via secondry mail sever " + se.ToString());
                Console.WriteLine("Error sending email " + se.ToString());
                goto retry;
            }
            catch (Exception ex)
            {
                Helper.WriteLog("Error sending email via primary mail server " + ex.ToString());
                Console.WriteLine("Error sending email " + ex.ToString());
            }
        }

        private void VaildateAttachments(EmailSettings emailsettings)
        {
            string AttachmentDownloadDir = ConfigurationManager.AppSettings["AttachmentDownloadDir"];
            string[] downloadformats = ConfigurationManager.AppSettings["dowanloadFormats"].Split(',');
            List<string> downloadedfiles = new List<string>();
            emailsettings.AttachmentDownloadDirectory = AttachmentDownloadDir;

            if (emailsettings.EmailAttachment.Count > 0)
            {
                for (int i = 0; i < emailsettings.EmailAttachment.Count; i++)
                {
                    try
                    {
                        if (Array.IndexOf(downloadformats, Path.GetExtension(emailsettings.EmailAttachment[i].FileName).ToLower()) >= 0)
                        {
                            string tempPath = Path.Combine(emailsettings.AttachmentDownloadDirectory, emailsettings.EmailAttachment[i].FileName);
                            try
                            {
                                FileStream stream = new FileStream(tempPath, FileMode.Create);
                                BinaryWriter writer = new BinaryWriter(stream);
                                writer.Write(emailsettings.EmailAttachment[i].Body);
                                writer.Flush();
                                writer.Close();

                                if (!downloadedfiles.Contains(emailsettings.EmailAttachment[i].FileName))
                                    downloadedfiles.Add(emailsettings.EmailAttachment[i].FileName);

                                emailsettings.IsAttachmentDownloaded = true;
                            }
                            catch (Exception ex)
                            {
                                Helper.WriteLog("Unable to download the attachment from emailId " + emailsettings.EmailReceivedFrom + " at " + emailsettings.EmailReceivedDateTime + " Error is " + ex.ToString());
                                emailsettings.IsAttachmentDownloaded = false;
                                emailsettings.Remark = "Unable to download the attachment from emailId " + emailsettings.EmailReceivedFrom + " Error is " + ex.ToString();
                            }
                        }
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine("Attachment download error :\n" + ex2.Message);
                        Helper.WriteLog("Attachment download error :\n" + ex2.Message);
                        continue;
                    }
                }
                emailsettings.AttachmentNames = downloadedfiles;
            }
        }
        public DataTable getDataTable(string SelectQuery)
        {
            DataTable dt = new DataTable();
            string connString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            try
            {
                using (var connection = new NpgsqlConnection(connString))
                {
                    connection.Open();
                    Console.WriteLine("Connection opened");
                    using (NpgsqlCommand selectcmd = new NpgsqlCommand(SelectQuery, connection))
                    {
                        using (NpgsqlDataAdapter orclda = new NpgsqlDataAdapter(selectcmd))
                        {
                            orclda.Fill(dt);
                            Console.WriteLine("dt count :" + dt.Rows.Count);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in getDatatable data :" + ex.Message);
                Console.WriteLine("Inner exception  :" + ex.InnerException);
                Console.WriteLine("stack trace : " + ex.StackTrace);
                Console.WriteLine("error I.tostrin() " + ex.ToString());
                Console.WriteLine("error Source :" + ex.Source);
                Helper.WriteLog("Error in function getDataTable :" + ex.Message);
            }
            return dt;
        }

        private void PreserveEmail(Message message, string MessageUid)
        {
            try
            {
                using (NpgsqlConnection objConn = new NpgsqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString()))
                {
                    objConn.Open();
                    string cmdQuery = "INSERT INTO EmailAudit(FromEmailID,MessageUid,EmailReceivedDate, EmailInfo)" +
                                      "VALUES(:p_EmailId, :p_MessageUid, :p_EmailReceivedDate, :p_EmailInfo)";
                    using (NpgsqlCommand objCmd = new NpgsqlCommand(cmdQuery, objConn))
                    {
                        
                        objCmd.Parameters.AddWithValue("p_EmailId", message.Headers.From.Address.ToString());
                        objCmd.Parameters.AddWithValue("p_MessageUid", MessageUid);
                         objCmd.Parameters.AddWithValue("p_EmailReceivedDate", message.Headers.DateSent.ToLocalTime());
                       // objCmd.Parameters.AddWithValue("p_EmailReceivedDate", DateTime.Now);
                        objCmd.Parameters.AddWithValue("p_EmailInfo", message.Headers.From.ToString());
                        objCmd.Parameters.AddWithValue("PCreatedDate", DateTime.Now);

                        //objCmd.Parameters.Add(new OracleParameter("p_EmailId", OracleDbType.Varchar2)).Value = message.Headers.From.Address.ToString();
                        //objCmd.Parameters.Add(new OracleParameter("p_MessageUid", OracleDbType.Varchar2)).Value = MessageUid;
                        //objCmd.Parameters.Add(new OracleParameter("p_EmailReceivedDate", OracleDbType.Date)).Value = message.Headers.DateSent.ToLocalTime();
                        //objCmd.Parameters.Add(new OracleParameter("p_EmailInfo", OracleDbType.Varchar2)).Value = message.Headers.From.ToString();
                        try
                        {
                            objCmd.ExecuteNonQuery();
                            Helper.WriteLog("New Insert : MessageUid :" + MessageUid + " EmailFrom :" + message.Headers.From.Address.ToString() + " Subject : " + message.Headers.Subject);
                        }
                        catch (Exception ex)
                        {
                            Helper.WriteLog("PreserveEmail Exception: ExecuteNonQuery {0}" + ex.ToString());
                            System.Console.WriteLine("Exception: {0}", ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteLog("Unable to Preserve Email. " + ex.ToString());
                Console.WriteLine("Unable to Preserve Email. " + ex.ToString());
            }
        }
    }
}
