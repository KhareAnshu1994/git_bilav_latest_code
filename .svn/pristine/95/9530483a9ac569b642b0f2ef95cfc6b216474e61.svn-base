using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using System.Net.Mail;
using System.Threading;

namespace IIFSLBSEDownloadUtility
{
    public class BSE_Utility
    {
        CookieContainer cookieContainer = new CookieContainer();
        string sessionLink = string.Empty;
        string referer = string.Empty;
        static FirefoxProfile fireFoxProfile = null;
        IWebDriver driver = null;
        IWebElement element = null;
        Int32 waitInterval = 1;
        string emailBody = "";
        List<string> downloadedfiles = new List<string>();

        string UserId = ConfigurationManager.AppSettings["Username"].ToString();
        string Password = ConfigurationManager.AppSettings["Password"].ToString();
        string MoveDir = ConfigurationManager.AppSettings["FileMoveDir"];
        string downloadDir = ConfigurationManager.AppSettings["DownloadDir"].ToString();
        string TemplateDir = ConfigurationManager.AppSettings["TemplateDir"].ToString();
        public void IIFSLBSEUtility()
        {
            LogError("***********************Starting Session " + DateTime.Now.ToString("dd MMM, yyyy HH:mm") + "********************", "IIFSLBSEDownloadUtility");
            string strFileName = string.Empty;
            string fileExtn = string.Empty;
            try
            {
                fireFoxProfile = new FirefoxProfile();

                fireFoxProfile.SetPreference("browser.download.folderList", 2);
                fireFoxProfile.SetPreference("browser.download.dir", downloadDir);
                fireFoxProfile.SetPreference("browser.download.downloadDir", downloadDir);
                fireFoxProfile.SetPreference("browser.download.defaultFolder", downloadDir);
                fireFoxProfile.SetPreference("pdfjs.disabled", true);
                fireFoxProfile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/octetstream");
                fireFoxProfile.SetPreference("plugin.scan.Acrobat", "99.0");
                fireFoxProfile.SetPreference("plugin.scan.plid.all", false);
                fireFoxProfile.SetPreference("browser.helperApps.alwaysAsk.force", false);
                string firefox_path = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";
                FirefoxBinary binary = new FirefoxBinary(firefox_path);
                fireFoxProfile.SetPreference("network.proxy.type", 0);

                //retry:
                try
                {
                    driver = new FirefoxDriver(fireFoxProfile);
                    driver.Navigate().GoToUrl("https://edx.standardandpoors.com/mailbox/jsp/login.jsp");
                    driver.Manage().Window.Maximize();
                    driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(60));

                    driver.Navigate().Refresh();
                    Console.WriteLine("Go for login");

                    IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                    element = driver.FindElement(By.ClassName("inputStyle"));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].setAttribute('value', '" + UserId + "')", element);
                    //((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value='UTIA2747';", element);

                    Console.WriteLine("input Username");
                    element = driver.FindElement(By.Name("password"));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].setAttribute('value', '" + Password + "')", element);
                    // ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value='g6cdm9Ed';", element);
                    Console.WriteLine("input Password");
                    try
                    {
                        driver.FindElement(By.XPath("//*[@id='xboxLogin']/div/table/tbody/tr/td[3]/table/tbody/tr[6]/td/input")).Submit();

                        Console.WriteLine("Click on Sumbit button");
                        string CurURL = driver.Url;
                        if (CurURL == "https://edx.standardandpoors.com/mailbox/jsp/MBILogin")
                        {
                            if (File.Exists(TemplateDir + "\\" + "Login_Failed.html"))
                            {
                                using (StreamReader reader = new StreamReader(TemplateDir + "\\" + "Login_Failed.html"))
                                {
                                    emailBody = reader.ReadToEnd();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Email template " + TemplateDir + "\\Login_Failed.html does not exists.");
                                LogError("IIFSLBSEUtility: Email template " + TemplateDir + "\\Login_Failed.html does not exists.", "IIFSLBSEDownloadUtility");
                            }
                            Sendemail(emailBody);
                            if (driver != null)
                                driver.Quit();

                            driver.Dispose();
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Successfully Login");
                            LogError("IIFSLBSEUtility: Successfully Login", "IIFSLBSEDownloadUtility");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError("Failed, Login :" + ex.ToString(), "IIFSLBSEDownloadUtility");
                        if (driver != null)
                            driver.Quit();

                        driver.Dispose();
                        return;
                    }
                    //get Current Url
                    Console.WriteLine("get Current Url");
                    sessionLink = referer = driver.Url;

                    //Console.WriteLine("get Current Url");
                    ICookieJar cj = driver.Manage().Cookies;
                    ReadOnlyCollection<OpenQA.Selenium.Cookie> cookieColl = cj.AllCookies;
                    CookieContainer cc = new CookieContainer();
                    foreach (OpenQA.Selenium.Cookie cook in cookieColl)
                    {
                        cc.Add(new System.Net.Cookie(cook.Name, cook.Value, cook.Path, cook.Domain));
                    }

                    //Download current date SDC specific file
                    DownloadSDCFile();
                }
                catch (Exception ex)
                {

                    if (driver != null)
                        driver.Quit();

                    driver.Dispose();
                    LogError("Failed, Unable Login :" + ex.ToString(), "IIFSLBSEDownloadUtility");
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                //driver.SwitchTo().DefaultContent();
                //driver.SwitchTo().Frame(driver.FindElement(By.Name("contents")));

                //element = driver.FindElement(By.XPath("/html/body/form/table/tbody/tr/td/table[3]/tbody/tr[3]/td[2]/b/a"));
                //element.Click();

                if (driver != null)
                    driver.Quit();

                driver.Dispose();
            }
            LogError("***********************Session " + DateTime.Now.ToString("dd MMM, yyyy HH:mm") + " comleted successfully****************************", "IIFSLBSEDownloadUtility");

        }
        private void DownloadSDCFile()
        {
            LogError("===========Download Files Function Start :", "IIFSLBSEDownloadUtility");
            Console.WriteLine("Download SDC File");
            string _FileSuffixes = ConfigurationManager.AppSettings["FileSuffixes"].ToString();
            string[] FileSuffixes = _FileSuffixes.Split(',');

            // retry:
            try
            {
                string StartDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                string EndDate = DateTime.Now.ToString("yyyy-MM-dd");

                driver.SwitchTo().DefaultContent();
                driver.SwitchTo().Frame(driver.FindElement(By.Name("view_body")));
                Console.WriteLine("view_body");

                element = (IWebElement)(new WebDriverWait(driver, TimeSpan.FromSeconds(waitInterval))).Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='StartDate']")));
                if (element == null) throw new Exception("username field not found");
                element.Clear();
                element.SendKeys(StartDate);
                Console.WriteLine("StartDate");

                element = driver.FindElement(By.XPath("//*[@id='EndDate']"));
                element.Clear();
                element.SendKeys(EndDate);
                Console.WriteLine("EndDate");


                Thread.Sleep(5000);

                element = driver.FindElement(By.XPath("//*[@name='MBISearch']/table/tbody/tr/td/table[2]/tbody/tr[6]/td/table/tbody/tr/td[1]/table[2]/tbody/tr[3]/td[2]/a/img"));
                element.Click();
                Console.WriteLine("Click on Go button");
                Thread.Sleep(5000);
                var table = driver.FindElement(By.XPath("/html/body/form/div[2]/table/tbody"));
                var rows = table.FindElements(By.TagName("tr"));
                foreach (var row in rows)
                {
                    string[] FileName = row.Text.Split(' ');
                    string FetchFileName = FileName[0];

                    //FetchFileName = FetchFileName.Substring(8);//Removing First 8 letter                    
                    for (int k = 0; k < FileSuffixes.Length; k++)
                    {
                        //FileSuffixes[k] = DateTime.Now.AddDays(-3).ToString("yyyyMMdd") + "_" + FileSuffixes[k];//concat Date
                        string _FileSuffix = DateTime.Now.ToString("yyyyMMdd") + FileSuffixes[k];//concat Date
                        if (FetchFileName != _FileSuffix) continue;
                        var rowTds = row.FindElements(By.TagName("td"));
                        foreach (var td in rowTds)
                        {
                            try
                            {
                                var a = td.FindElement(By.TagName("a"));
                                var image = td.FindElement(By.TagName("img"));
                                var href = a.GetAttribute("href");
                                if (href.Contains("javascript:myMBIExtract"))
                                {
                                    image.Click();
                                    LogError("DownloadSDCFile: Download the file name:" + FetchFileName, "IIFSLBSEDownloadUtility");
                                    Console.WriteLine("DownloadSDCFile: Download the file name:" + FetchFileName, "IIFSLBSEDownloadUtility");
                                    Thread.Sleep(5000);
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("Unable to locate element"))
                                {
                                   /// k = k - 1;
                                    continue;
                                }
                                                                  
                            }

                        }
                    }
                }
                UploadFilesOnSFTP();
                //DownloadedFileMove();
                DirectoryInfo dirInfo = new DirectoryInfo(MoveDir);
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    if (!downloadedfiles.Contains(file.Name) && file.Name.ToLower().Contains(".sdc"))
                        downloadedfiles.Add(file.Name);
                }
                LogError("DownloadSDCFile: Check Download File Count:" + downloadedfiles.Count, "IIFSLBSEDownloadUtility");

                //if (FileSuffixes.Length != downloadedfiles.Count)
                //{
                //    if (File.Exists(TemplateDir + "\\" + "Attachment_count.html"))
                //    {
                //        using (StreamReader reader = new StreamReader(TemplateDir + "\\" + "Attachment_count.html"))
                //        {
                //            emailBody = reader.ReadToEnd();
                //        }
                //        emailBody = emailBody.Replace("{attachment}", downloadedfiles.Count.ToString());
                //    }
                //    else
                //        Console.WriteLine("Email template " + TemplateDir + "\\Email_reply.html does not exists.");

                //    Sendemail(emailBody);
                //}
            }
            catch (Exception ex)
            {
                LogError("Failed, Downloading files :" + ex.ToString(), "IIFSLBSEDownloadUtility");
            }
            finally
            {
                if (driver != null)
                    driver.Quit();

                driver.Dispose();
            }
            LogError("===========Download Files Function End :", "IIFSLBSEDownloadUtility");
        }

        public void Sendemail(string EmailBody)
        {
            LogError("=========Send Email Start :", "IIFSLBSEDownloadUtility");
            try
            {
                string[] EmailReciepients = null;
                string EmailFrom = ConfigurationManager.AppSettings["SenderEmailId"].ToString();
                string EmailTo = ConfigurationManager.AppSettings["EmailToRecipients"].ToString();
                string EmailFromPasswd = ConfigurationManager.AppSettings["SenderEmailPwd"].ToString();
                string SMTPServer = ConfigurationManager.AppSettings["SMTPServer"].ToString();
                int SMTPPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);

                if (EmailTo != "")
                    EmailReciepients = EmailTo.Split(',');

                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                //MailAddress mailAddress = new MailAddress(EmailFrom);
                //message.Sender = mailAddress;
                message.From = new MailAddress(EmailFrom);
                message.Body = EmailBody;
                message.Subject = "IIFSL BSE Utility File log - " + DateTime.Now.ToString("dd MMM, yyyy");

                SmtpClient smtp = new SmtpClient(SMTPServer, SMTPPort);
                //smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(EmailFrom, EmailFromPasswd);
                smtp.EnableSsl = true;

                if (EmailReciepients != null)
                {
                    EmailReciepients = EmailReciepients.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    for (int i = 0; i < EmailReciepients.Length; i++)
                    {
                        message.To.Add(EmailReciepients[i]);
                    }
                }

                smtp.Send(message);
                LogError("Sendemail: Send Mail Successfully :" + EmailTo, "IIFSLBSEDownloadUtility");
                //sbErrorCollector.Append("eMail sent").AppendLine();
            }
            catch (SmtpException se)
            {
                //serverFlag = false;
                LogError("Failed, sending email via mail sever :" + se.ToString(), "IIFSLBSEDownloadUtility");
                Console.WriteLine("Error sending email " + se.ToString());
                //goto retry;
            }
            catch (Exception ex)
            {
                LogError("Failed, sending email via primary mail server :" + ex.ToString(), "IIFSLBSEDownloadUtility");
                Console.WriteLine("Error sending email " + ex.ToString());
            }
            LogError("=========Send Email End :", "IIFSLBSEDownloadUtility");
        }
        public void UploadFilesOnSFTP()
        {
            SFTPUploadProcess sup = new SFTPUploadProcess();
            sup.UploadStart();
        }
        public void DownloadedFileMove()
        {
            LogError("=========Downloaded File Move Start :", "IIFSLBSEDownloadUtility");

            string[] filePaths = Directory.GetFiles(MoveDir);
            foreach (string filePath in filePaths)
                File.Delete(filePath);

            DateTime StartTime = DateTime.Now;

            DirectoryInfo folder = new DirectoryInfo(downloadDir);
            try
            {
                foreach (FileInfo file in folder.GetFiles())
                {
                    if (file.Extension.Equals(".SDC"))
                    {

                        try
                        {
                            string fileName = file.Name;
                            string sourcePath = @"" + downloadDir;
                            string targetPath = @"" + MoveDir;

                            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                            string destFile = System.IO.Path.Combine(targetPath, fileName);

                            if (!System.IO.Directory.Exists(targetPath))
                            {
                                System.IO.Directory.CreateDirectory(targetPath);
                            }
                            System.IO.File.Move(sourceFile, destFile);
                            LogError("DownloadSDCFile: file Move Successfully:" + destFile, "IIFSLBSEDownloadUtility");
                            Console.WriteLine("DownloadSDCFile: file Move Successfully:" + destFile, "IIFSLBSEDownloadUtility");
                        }
                        catch (SecurityException) { throw new Exception("Security Exception - no permission to write to destination folder"); }
                        catch (DirectoryNotFoundException) { throw new Exception("The destination directory not found"); }
                        catch (IOException) { throw new Exception("IO Exceoption perhaps the destimation file already exists"); }
                    }
                    else
                    {
                        LogError("DownloadSDCFile: SDC file Not found :", "IIFSLBSEDownloadUtility");
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("DownloadSDCFile: Failed, Move File:" + ex.ToString(), "IIFSLBSEDownloadUtility");
            }
            LogError("=========Downloaded File Move End :", "IIFSLBSEDownloadUtility");
        }
        public static void LogError(string message, string FileName)
        {
            string ErrorLogDir = ConfigurationManager.AppSettings["ErrorLogFile"].ToString();

            if (!Directory.Exists(ErrorLogDir))
                Directory.CreateDirectory(ErrorLogDir);
            using (StreamWriter sw = new StreamWriter(Path.Combine(ErrorLogDir, FileName + "ErrorLog" + DateTime.Now.ToString("dd-MMM-yy") + ".txt"), true))
            {
                sw.WriteLine(DateTime.Now.ToString("dd-MMM-yy HH:mm:ss") + "\t" + message);
            }
        }
    }
}
