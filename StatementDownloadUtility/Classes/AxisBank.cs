using Ionic.Zip;
using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;

namespace StatementDownloadUtility
{
    class AxisBank
    {
        Common common = new Common();
        public void ProcessEmails()
        {

            string PrimaryMailServer = ConfigurationManager.AppSettings["PrimaryMailServer"];
            string SecondryMailServer = ConfigurationManager.AppSettings["SecondryMailServer"];
            string MailPort = ConfigurationManager.AppSettings["MailServerPort"];
            string CorporateEmail = ConfigurationManager.AppSettings["MailBoxEmailId"];
            string CorporatPass = ConfigurationManager.AppSettings["MailBoxEmailPass"];

            List<string> messageUids = new List<string>();
            int TotalEmailCount = 0;

            Message EmailMessage = null;
            DataTable dtExisting = getDataTable();
            DataRow[] aRow;
            Pop3Client PopClient = new Pop3Client();
            ReceivedEmail recEmail = new ReceivedEmail();
            int currentMessageId = 0;

            try
            {
                currentMessageId = 0;
                Common.LogError("Email Utility started", "Axis");
                PopClient.Connect(PrimaryMailServer, Convert.ToInt16(MailPort), true);
                PopClient.Authenticate(CorporateEmail, CorporatPass, AuthenticationMethod.UsernameAndPassword);
                TotalEmailCount = PopClient.GetMessageCount();
                messageUids = PopClient.GetMessageUids();
                Console.WriteLine("total email count :" + messageUids.Count);
                Common.LogError("total email count :" + messageUids.Count, "Axis");
            }
            catch (Exception ex)
            {
                try
                {
                    common.InsertBankScriptReports("Axis Bank", "", 0, "", DateTime.Now, "Failed", "Unable to connect to Mail server " + PrimaryMailServer + " Error " + ex.ToString());
                    Common.LogError("Unable to connect to Mail server " + PrimaryMailServer + " Error " + ex.ToString(), "Axis");
                    PopClient.Connect(SecondryMailServer, Convert.ToInt16(MailPort), false);
                    PopClient.Authenticate(CorporateEmail, CorporatPass, AuthenticationMethod.UsernameAndPassword);
                    TotalEmailCount = PopClient.GetMessageCount();
                    messageUids = PopClient.GetMessageUids();
                }
                catch (Exception ex2)
                {
                    Common.LogError("Email Utility trying to connect to mail server " + PrimaryMailServer + " and " + SecondryMailServer + " Error " + ex2.ToString(), "Axis");
                    Console.WriteLine("Unable to connect to Mail server " + SecondryMailServer + " Error is " + ex2.ToString());
                    return;
                }
            }

            if (messageUids.Count == 0)
            {
                common.InsertBankScriptReports("Axis Bank", "", 0, "", DateTime.Now, "", "No new Emails. Email Count " + messageUids.Count);
                Console.WriteLine("Message Count " + messageUids.Count);
                Common.LogError("No new emails. Email Count " + messageUids.Count, "Axis");
                return;
            }

            for (int i = 0; i < messageUids.Count; i++)
            {
                try
                {
                    currentMessageId = i + 1;
                    aRow = dtExisting.Select("MessageUid = '" + messageUids[i] + "'");
                    if (aRow.Length < 1)
                    {
                        EmailMessage = PopClient.GetMessage(currentMessageId);
                        PreserveEmail(EmailMessage.Headers.From.Address.ToString(), messageUids[i]);
                        Console.WriteLine(EmailMessage.Headers.From.Address.ToString());
                        if (EmailMessage.Headers.From.Address.ToString().Contains("NO-REPLY.CORPORATE@UTI.CO.IN"))
                            continue;
                    }
                    else continue;
                }
                catch (Exception ex)
                {
                    common.InsertBankScriptReports("Axis Bank", "", 0, "", DateTime.Now, "Failed", "Unable to read email at index " + currentMessageId.ToString() + " " + ex.ToString());
                    Console.WriteLine("Unable to read email at index " + currentMessageId.ToString() + " " + ex.ToString());
                    Common.LogError("Unable to read email at index " + currentMessageId.ToString() + " " + ex.ToString(), "Axis");
                    continue;
                }
                if (EmailMessage.Headers.Received != null && EmailMessage.Headers.Received.Count > 0)
                    recEmail.EmailReceivedDateTime = EmailMessage.Headers.Received[0].Date;
                else
                    recEmail.EmailReceivedDateTime = EmailMessage.Headers.DateSent;

                if (recEmail.EmailReceivedDateTime.Date.CompareTo(DateTime.Now.Date) != 0)
                    continue;

                recEmail.EmailReceivedFrom = EmailMessage.Headers.From.Address.ToString();
                recEmail.EmailAttachment = EmailMessage.FindAllAttachments();
                VaildateAttachments(recEmail);

            }
            Common.LogError("Email Utility completed", "Axis");
        }


        private void PreserveEmail(string messageReceivedFrom, string MessageUid)
        {
            try
            {
                string EmailLogDir = ConfigurationManager.AppSettings["attendedEmailLogFile"];
                string EmailLogFile = "ReadEmails" + DateTime.Now.ToString("ddMMMyyyy") + ".txt";

                if (!System.IO.Directory.Exists(EmailLogDir))
                    Directory.CreateDirectory(EmailLogDir);


                using (StreamWriter sw = new StreamWriter(Path.Combine(EmailLogDir, EmailLogFile), true))
                {
                    sw.WriteLine(messageReceivedFrom + "||" + MessageUid);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to Preserve Email. " + ex.ToString());
            }
        }

        private void VaildateAttachments(ReceivedEmail _RecEmail)
        {
            string downloadDir = ConfigurationManager.AppSettings["AttachmentDownloadDir"];
            string moveDirectory = ConfigurationManager.AppSettings["FileMoveDir"];
            string cPath = downloadDir + "\\" + Guid.NewGuid().ToString();

            string[] fileformats = { ".zip" };
            List<string> downloadedfiles = new List<string>();

            if (_RecEmail.EmailAttachment != null && _RecEmail.EmailAttachment.Count > 0)
            {
                for (int i = 0; i < _RecEmail.EmailAttachment.Count; i++)
                {
                    if (Array.IndexOf(fileformats, Path.GetExtension(_RecEmail.EmailAttachment[i].FileName)) >= 0)
                    {
                        try
                        {
                            FileStream stream = new FileStream(Path.Combine(downloadDir, _RecEmail.EmailAttachment[i].FileName), FileMode.Create);
                            BinaryWriter writer = new BinaryWriter(stream);
                            writer.Write(_RecEmail.EmailAttachment[i].Body);
                            writer.Flush();
                            writer.Close();

                            if (!downloadedfiles.Contains(_RecEmail.EmailAttachment[i].FileName))
                                downloadedfiles.Add(_RecEmail.EmailAttachment[i].FileName);

                            _RecEmail.IsAttachmentDownloaded = true;
                        }
                        catch (Exception ex)
                        {
                            common.InsertBankScriptReports("Axis Bank", "", 0, "", DateTime.Now, "Failed", "Unable to download the attachment from emailId " + _RecEmail.EmailReceivedFrom + " at " + _RecEmail.EmailReceivedDateTime + " Error is " + ex.ToString());
                            Common.LogError("Unable to download the attachment from emailId " + _RecEmail.EmailReceivedFrom + " at " + _RecEmail.EmailReceivedDateTime + " Error is " + ex.ToString(), "Axis");
                            _RecEmail.IsAttachmentDownloaded = false;
                        }

                        if (_RecEmail.IsAttachmentDownloaded)
                        {
                            if (Path.GetExtension(_RecEmail.EmailAttachment[i].FileName).ToLower().Equals(".zip"))
                            {
                                try
                                {
                                    if (!Directory.Exists(cPath))
                                        Directory.CreateDirectory(cPath);

                                    ZipFile zfile = ZipFile.Read(downloadDir + "\\" + _RecEmail.EmailAttachment[i].FileName);

                                    var flattenFoldersOnExtract = zfile.FlattenFoldersOnExtract;
                                    zfile.FlattenFoldersOnExtract = true;
                                    zfile.ExtractAll(cPath, ExtractExistingFileAction.OverwriteSilently);
                                    zfile.FlattenFoldersOnExtract = flattenFoldersOnExtract;
                                    zfile.Dispose();

                                    for (int m = 0; m < downloadedfiles.Count; m++)
                                    {
                                        if (downloadedfiles[m].ToLower().Contains(".zip"))
                                            downloadedfiles.RemoveAt(m);
                                    }

                                    DirectoryInfo dirInfo = new DirectoryInfo(cPath);
                                    foreach (FileInfo file in dirInfo.GetFiles())
                                    {
                                        if (!downloadedfiles.Contains(file.Name) && file.Name.ToLower().Contains(".csv"))
                                            downloadedfiles.Add(file.Name);
                                    }
                                    common.InsertBankScriptReports("Axis Bank", "", 0, "", DateTime.Now, "Success", "");
                                }
                                catch (Exception ex)
                                {
                                    common.InsertBankScriptReports("Axis Bank", "", 0, "", DateTime.Now, "Failed", "Unable to extract files from " + _RecEmail.EmailAttachment[i].FileName + " received from email " + _RecEmail.EmailReceivedFrom + " at " + _RecEmail.EmailReceivedDateTime + " Error is " + ex.ToString());
                                    Common.LogError("Unable to extract files from " + _RecEmail.EmailAttachment[i].FileName + " received from email " + _RecEmail.EmailReceivedFrom + " at " + _RecEmail.EmailReceivedDateTime + " Error is " + ex.ToString(), "Axis");
                                }
                            }
                        }
                    }
                    else
                    {
                        Common.LogError("Invalid attachement format with extension " + Path.GetExtension(_RecEmail.EmailAttachment[i].FileName), "Axis");
                    }
                }
                _RecEmail.AttachmentNames = downloadedfiles;
            }
            else
            {

            }

            if (_RecEmail.AttachmentNames != null && _RecEmail.AttachmentNames.Count > 0)
            {
                for (int i = 0; i < _RecEmail.AttachmentNames.Count; i++)
                {
                    try
                    {
                        File.Move(Path.Combine(cPath, _RecEmail.AttachmentNames[i]), Path.Combine(moveDirectory, _RecEmail.AttachmentNames[i]));
                    }
                    catch (Exception ex)
                    {
                        Common.LogError(ex.ToString(), "Axis");
                    }
                }

                DirectoryInfo di = new DirectoryInfo(downloadDir);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }

            }
        }

        private DataTable getDataTable()
        {
            DataTable dt = new DataTable();
            string EmailLogDir = ConfigurationManager.AppSettings["attendedEmailLogFile"];
            string EmailLogFile = EmailLogDir + "\\ReadEmails" + DateTime.Now.ToString("ddMMMyyyy") + ".txt";

            try
            {
                dt.Columns.Add("EmailReceivedFrom", typeof(string));
                dt.Columns.Add("MessageUid", typeof(string));

                if (File.Exists(EmailLogFile))
                {
                    string[] lines = File.ReadAllLines(EmailLogFile);
                    for (int i = 0; i < lines.Count(); i++)
                    {
                        DataRow oRow = dt.Rows.Add();
                        oRow[0] = lines[i].Split(new string[] { "||" }, StringSplitOptions.None)[0];
                        oRow[1] = lines[i].Split(new string[] { "||" }, StringSplitOptions.None)[1];
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogError("Error " + ex.ToString(), "Axis");
            }


            return dt;
        }
    }

    class ReceivedEmail
    {
        public string MessageId { get; set; }
        public string EmailReceivedFrom { get; set; }
        public DateTime EmailReceivedDateTime { get; set; }
        public string EmailInfo { get; set; }
        public int AttachmentCount { get; set; }
        public List<string> AttachmentNames { get; set; }
        public List<OpenPop.Mime.MessagePart> EmailAttachment { get; set; }
        public bool IsAttachmentDownloaded { get; set; }
    }
}
