using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using WatiN.Core;
using WatiN.Core.Native.Windows;

namespace StatementDownloadUtility
{
    public class IDBIBank
    {
        public string CorporateId { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string[] AccountNumbers { get; set; }
        public string[] CustomerId { get; set; }
        public string StatementDownloadDir { get; set; }
        public string StatementMoveDir { get; set; }
        public string HostName { get; set; }

        CookieContainer cookieContainer = new CookieContainer();
        WebBrowser wbIDBI = new WebBrowser();
        string sessionLink = string.Empty;
        string referer = string.Empty;

        static FirefoxProfile fireFoxProfile = null;
        IWebDriver driver = null;
        IWebElement element = null;

        Common common = new Common();

        public IDBIBank()
        {
            Common common = new Common();
            DataTable dtIDBI = common.GetBankDetails("IDBI");

            if (dtIDBI != null && dtIDBI.Rows.Count > 0)
            {
                AccountNumbers = dtIDBI.Rows[0]["ACCOUNTNO"].ToString().Split(',');
                CorporateId = dtIDBI.Rows[0]["CORPORATID"].ToString();
                Password = dtIDBI.Rows[0]["PASSWORD"].ToString();
                HostName = dtIDBI.Rows[0]["LINK"].ToString();
                UserId = dtIDBI.Rows[0]["USERID"].ToString();
                IsActive = Convert.ToBoolean(dtIDBI.Rows[0]["IsActive"]);
                StatementMoveDir = ConfigurationManager.AppSettings["FileMoveDir"];
            }
            else return;
         
        }

        public void DownloadStatement()
        {

            if (!IsActive)
                return;

            //using (var browser = new IE(HostName))
            //{
            //    browser.ShowWindow(NativeMethods.WindowShowStyle.Hide);
            //    browser.TextField(Find.ById("corporateId")).TypeText(CorporateId);
            //    browser.TextField(Find.ById("customerId")).TypeText(UserId);
            //    browser.TextField(Find.ById("password")).TypeText(Password);
            //    browser.Button(Find.ByClass("bttn")).Click();
            //    sessionLink = browser.NativeDocument.Url;
            //    referer = browser.Frames[0].Url;
            //    downloadFile();
            //}
            fireFoxProfile = new FirefoxProfile();
            fireFoxProfile.SetPreference("browser.download.folderlist", 2);
            fireFoxProfile.SetPreference("browser.download.dir", StatementMoveDir);
            fireFoxProfile.SetPreference("browser.download.downloadDir", StatementMoveDir);
            fireFoxProfile.SetPreference("browser.download.defaultFolder", StatementMoveDir);
            fireFoxProfile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf");
            fireFoxProfile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/vnd.ms-excel");
            fireFoxProfile.SetPreference("pdfjs.disabled", true);
            fireFoxProfile.SetPreference("plugin.scan.Acrobat", "99.0");
            fireFoxProfile.SetPreference("plugin.scan.plid.all", false);

            try
            {
                driver = new FirefoxDriver(fireFoxProfile);
                driver.Manage().Timeouts().SetPageLoadTimeout(new TimeSpan(0, 0, 60));
                try
                {
                    driver.Navigate().GoToUrl(HostName + "/corp/BANKAWAY?Action.CorpUser.Init.001=y&AppSignonBankId=IBKL&AppType=corporate");
                }
                catch 
                {
                    return;
                }

                try
                {
                    driver.Manage().Window.Maximize();
                }
                catch (Exception)
                {
                    Actions actions = new Actions(driver);
                    actions.SendKeys(OpenQA.Selenium.Keys.Escape);
                }
                

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("javascript: MessageDisplay()");

                element = driver.FindElement(By.Id("corporateId"));
                if (element == null) throw new Exception("Phishing control button not found");
                element.SendKeys(CorporateId);

                element = driver.FindElement(By.Id("customerId"));
                if (element == null) throw new Exception("username field not found");
                element.SendKeys(UserId);

                element = driver.FindElement(By.Id("password"));
                if (element == null) throw new Exception("Password field not found");
                element.SendKeys(Password);

                element = driver.FindElement(By.ClassName("bttn"));
                if (element == null) throw new Exception("Login button not found");
                element.Click();

                //if (driver.PageSource.IndexOf("My Accounts") <= 0)
                //{
                //    Common.LogError("Unable to Login. Incorrect UserId or Password", "SBI");
                //    return;
                //}
                sessionLink = referer = driver.Url;

                ICookieJar cj = driver.Manage().Cookies;
                ReadOnlyCollection<OpenQA.Selenium.Cookie> cookieColl = cj.AllCookies;
                CookieContainer cc = new CookieContainer();
                foreach (OpenQA.Selenium.Cookie cook in cookieColl)
                {
                    cc.Add(new System.Net.Cookie(cook.Name, cook.Value, cook.Path, cook.Domain));
                }
                downloadFile();
            }
            catch (Exception ex)
            {
                Common.LogError("Unable to Login. " + ex.ToString(), "SBI");
            }
            finally
            {
                if (driver != null)
                    driver.Quit();

                driver.Dispose();
            }

           
        }

        private void downloadFile()
        {
            try
            {
                sessionLink = sessionLink.Replace("BANKAWAY", "BANKAWAYTRAN");
                sessionLink = sessionLink.Replace(sessionLink.Substring(sessionLink.IndexOf("bwayparam"), sessionLink.Length - sessionLink.IndexOf("bwayparam")), "bwayparam=Z6spFdlqDaPKgNAEQTtyQQvllcgGgEGNCKyXM4ldEhULVyX%2BI3EL0fKQ4XkL9lXIrt0pzikYOcAI%0D%0AGqwiEbMCgfHQPhTxEeZcAgJz3RRYx2douX8ISYJqmMZvVpYqTXUiyWrQPxy2ok4%3D");
                referer = referer.Replace(referer.Substring(referer.IndexOf("bwayparam"), referer.Length - referer.IndexOf("bwayparam")), "bwayparam=Z6spFdlqDaHGkc8kXCpzQQz0l8oWulquQJjcFJRWQgVJC3c%3D");

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sessionLink);
                req.Method = "GET";
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                req.CookieContainer = cookieContainer;
                req.Referer = referer;
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream Answer23 = resp.GetResponseStream();
                StreamReader _Answe32r = new StreamReader(Answer23);
                string page = _Answe32r.ReadToEnd();
                resp.Close();

                sessionLink = sessionLink.Replace(sessionLink.Substring(sessionLink.IndexOf("bwayparam"), sessionLink.Length - sessionLink.IndexOf("bwayparam")), "bwayparam=YKcvEf9gHtOf");
                referer = referer.Replace(referer.Substring(referer.IndexOf("bwayparam"), referer.Length - referer.IndexOf("bwayparam")), "bwayparam=Z6spFdlqDaPKgNAEQTtyQQvllcgGgEGNCKyXM4ldEhULVyX%2BI3EL0fKQ4XkL9lXIrt0pzikYOcAI%0D%0AGqwiEbMCgfHQPhTxEeZcAgJz3RRYx2douX8ISYJqmMZvVpYqTXUiyWrQPxy2ok4%3D");
                HttpWebRequest selectacc = (HttpWebRequest)WebRequest.Create(sessionLink);
                selectacc.CookieContainer = cookieContainer;
                selectacc.Method = "POST";
                selectacc.Referer = referer;
                selectacc.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.79 Safari/537.36 Edge/14.14393";
                string postData2 = "Options.SelectList=Option.Accounts.QuerySelection&userAccountIndex=0&Action.Go.x=8&Action.Go.y=13";
                byte[] data2 = Encoding.ASCII.GetBytes(postData2);
                selectacc.ContentType = "application/x-www-form-urlencoded";
                selectacc.ContentLength = data2.Length;

                Stream reqStream2 = selectacc.GetRequestStream();
                reqStream2.Write(data2, 0, data2.Length);
                reqStream2.Close();

                HttpWebResponse respe = (HttpWebResponse)selectacc.GetResponse();
                Stream Answer2323 = respe.GetResponseStream();
                StreamReader _Answe32r32 = new StreamReader(Answer2323);
                string page23 = _Answe32r32.ReadToEnd();
                respe.Close();


                for (int i = 0; i < AccountNumbers.Length; i++)
                {
                    string accIndex = page23.Substring(page23.IndexOf(AccountNumbers[i]) - 20, 20);
                    if (accIndex.Split('"').Length > 0)
                        accIndex = accIndex.Split('"')[1];

                    sessionLink = sessionLink.Replace(sessionLink.Substring(sessionLink.IndexOf("bwayparam"), sessionLink.Length - sessionLink.IndexOf("bwayparam")), "bwayparam=XLInBsxeed%2FQmsYIVg%3D%3D");
                    referer = referer.Replace(referer.Substring(referer.IndexOf("bwayparam"), referer.Length - referer.IndexOf("bwayparam")), "bwayparam=YKcvEf9gHtOf");

                    HttpWebRequest accSelReq = (HttpWebRequest)WebRequest.Create(sessionLink);
                    accSelReq.CookieContainer = cookieContainer;
                    accSelReq.Method = "POST";
                    accSelReq.Referer = referer;
                    accSelReq.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.79 Safari/537.36 Edge/14.14393";
                    string postData = "userAccountIndex=" + accIndex + "&Options.SelectList=Option.Accounts.QuerySelection&txnSrcLatestNoOfTxns=&txnSrchSortOrder=A&txnSrcFromDate=" + DateTime.Now.Date.Day + "%2F" + DateTime.Now.Date.Month + "%2F" + DateTime.Now.Date.Year + "&dateformat=dd%2FMM%2Fyyyy&txnSrcToDate=" + DateTime.Now.Date.Day + "%2F" + DateTime.Now.Date.Month + "%2F" + DateTime.Now.Date.Year + "&txnSrcMinAmt=&txnSrcMaxAmt=&txnSrchBeginChkNo=&txnSrchEndChkNo=&txnSrcFromValueDate=&txnSrcToValueDate=&txnSrcOrgAmt=&txnSrcOrgCur=&txnSrcRemarks=&txnSrchTxnType=B&txnSrcNoOfTxns=&accountquery=0&Action.Go.x=7&Action.Go.y=10";
                    byte[] data = Encoding.ASCII.GetBytes(postData);
                    accSelReq.ContentType = "application/x-www-form-urlencoded";
                    accSelReq.ContentLength = data.Length;

                    Stream reqStream = accSelReq.GetRequestStream();
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();

                    HttpWebResponse accSelRes = (HttpWebResponse)accSelReq.GetResponse();
                    accSelRes.Close();

                    HttpWebRequest fileReq = (HttpWebRequest)WebRequest.Create(sessionLink);
                    fileReq.CookieContainer = cookieContainer;
                    fileReq.Method = "POST";
                    fileReq.Referer = referer;
                    fileReq.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.79 Safari/537.36 Edge/14.14393";
                    postData = "userAccountIndex=" + accIndex + "&Options.SelectList=Option.Accounts.QuerySelection&txnSrcLatestNoOfTxns=&txnSrchSortOrder=A&txnSrcFromDate=" + DateTime.Now.Date.Day + "%2F" + DateTime.Now.Date.Month + "%2F" + DateTime.Now.Date.Year + "&dateformat=dd%2FMM%2Fyyyy&txnSrcToDate=" + DateTime.Now.Date.Day + "%2F" + DateTime.Now.Date.Month + "%2F" + DateTime.Now.Date.Year + "&txnSrcMinAmt=&txnSrcMaxAmt=&txnSrchBeginChkNo=&txnSrchEndChkNo=&txnSrcFromValueDate=&txnSrcToValueDate=&txnSrcOrgAmt=&txnSrcOrgCur=&txnSrcRemarks=&txnSrchTxnType=B&txnSrcNoOfTxns=&accountquery=4&Action.Accounts.QuerySelection.QueryStatement.PDF=Statement";
                    data = Encoding.ASCII.GetBytes(postData);
                    fileReq.ContentType = "application/x-www-form-urlencoded";
                    fileReq.ContentLength = data.Length;

                    reqStream = fileReq.GetRequestStream();
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();

                    HttpWebResponse resp3 = (HttpWebResponse)fileReq.GetResponse();

                    const int readSize = 256;
                    byte[] buffer = new byte[readSize];
                    MemoryStream ms = new MemoryStream();

                    int count = resp3.GetResponseStream().Read(buffer, 0, readSize);
                    while (count > 0)
                    {
                        ms.Write(buffer, 0, count);
                        count = resp3.GetResponseStream().Read(buffer, 0, readSize);
                    }
                    ms.Position = 0;
                    resp3.GetResponseStream().Close();

                    StreamReader validatePage = new StreamReader(ms);
                    string validatePage2 = validatePage.ReadToEnd();

                    if (!validatePage2.Contains("No record"))
                    {
                        ms.Position = 0;
                        FileStream fstr = File.Create(StatementMoveDir + "\\IDBI_" + AccountNumbers[i] + ".xls");
                        ms.CopyTo(fstr);
                        fstr.Close();
                        resp.Close();
                    }
                }


            }
            catch (Exception ex)
            {
                Common.LogError("Error downloading files " + ex.ToString(), "IDBI");
            }
            finally {
                if (driver != null)
                    driver.Quit();

                driver.Dispose();
            }
        }
    }
}
