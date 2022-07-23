using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using WatiN.Core;
using WatiN.Core.Native.Windows;
using System.Data;
using System.Configuration;

namespace StatementDownloadUtility
{
    public class SBIBank
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
        string sessionLink = string.Empty;
        string referer = string.Empty;
        static FirefoxProfile fireFoxProfile = null;
        IWebDriver driver = null;
        IWebElement element = null;
        Common common = new Common();

        public SBIBank()
        {
            Common common = new Common();
            DataTable dtSBI = common.GetBankDetails("SBI");

            if (dtSBI != null && dtSBI.Rows.Count > 0)
            {
                AccountNumbers = dtSBI.Rows[0]["ACCOUNTNO"].ToString().Split(',');
                HostName = dtSBI.Rows[0]["LINK"].ToString();
                CustomerId = dtSBI.Rows[0]["CorporatId"].ToString().Split(',');
                UserId = dtSBI.Rows[0]["USERID"].ToString();
                Password = dtSBI.Rows[0]["PASSWORD"].ToString();
                IsActive = Convert.ToBoolean(dtSBI.Rows[0]["IsActive"]);
                StatementMoveDir = ConfigurationManager.AppSettings["FileMoveDir"];
            }
            else return;

        }

        public void downloadFile()
        {

            if (!IsActive)
                return;

            string strFileName = string.Empty;
            string fileExtn = string.Empty;
            try
            {
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
                    driver.Navigate().GoToUrl(HostName + "/corpuser/login.htm");
                    driver.Manage().Window.Maximize();

                    element = driver.FindElement(By.ClassName("login_button"));
                    if (element == null) throw new Exception("Phishing control button not found");
                    element.Click();

                    element = driver.FindElement(By.Id("username"));
                    if (element == null) throw new Exception("username field not found");
                    element.SendKeys(UserId);

                    element = driver.FindElement(By.Name("password"));
                    if (element == null) throw new Exception("Password field not found");
                    element.SendKeys(Password);

                    element = driver.FindElement(By.Id("Button2"));
                    if (element == null) throw new Exception("Login button not found");
                    element.Click();

                    if (driver.PageSource.IndexOf("My Accounts") <= 0)
                    {
                        Common.LogError("Unable to Login. Incorrect UserId or Password", "SBI");
                        return ;
                    }
                }
                catch (Exception ex)
                {
                    Common.LogError("Unable to Login. " + ex.ToString(), "SBI");
                }
             

                for (int i = 0; i < AccountNumbers.Length; i++)
                {
                    try
                    {
                        Common.LogError("test", "SBI");
                        ICookieJar cj = driver.Manage().Cookies;
                        ReadOnlyCollection<OpenQA.Selenium.Cookie> cookieColl = cj.AllCookies;
                        CookieContainer cc = new CookieContainer();
                        foreach (OpenQA.Selenium.Cookie cook in cookieColl)
                        {
                            cc.Add(new System.Net.Cookie(cook.Name, cook.Value, cook.Path, cook.Domain));
                        }

                        HttpWebRequest req3 = (HttpWebRequest)WebRequest.Create(HostName + "/corpuser/downloadstatement.htm");
                        req3.CookieContainer = cc;
                        string endDate = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                        req3.Method = "POST";
                        req3.ContentType = "application/x-www-form-urlencoded";
                        NameValueCollection postColl2 = HttpUtility.ParseQueryString(string.Empty);
                        postColl2.Clear();
                        postColl2.Add("fileFormat", "csvFormat");
                        postColl2.Add("order", "1");
                        postColl2.Add("currentdate", endDate);
                        postColl2.Add("accountNo", AccountNumbers[i]);
                        postColl2.Add("branchCode", "A1777");
                        postColl2.Add("productType", "A2");
                        postColl2.Add("accountType", "current");
                        postColl2.Add("pagename", "accountstatementnew");
                        postColl2.Add("fromDate", endDate);
                        postColl2.Add("toDate", endDate);
                        postColl2.Add("radiobuttonOrd", "radiobutton");
                        postColl2.Add("radiobuttonOrd", "radiobutton");
                        postColl2.Add("accountNo", "current");

                        req3.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                        req3.Referer = HostName + "/corpuser/accountstatementnew.htm";
                        req3.Connection = "true";
                        string postData = postColl2.ToString();
                        byte[] data = Encoding.ASCII.GetBytes(postData);
                        req3.ContentLength = data.Length;
                        Stream reqStream = req3.GetRequestStream();
                        reqStream.Write(data, 0, data.Length);
                        reqStream.Close();
                        HttpWebResponse resp = (HttpWebResponse)req3.GetResponse();
                        if (resp.StatusCode != HttpStatusCode.OK)
                        {
                            Common.LogError("Unable to download statement. Status Code is " + resp.StatusCode.ToString(), "SBI");
                            return;
                        }
                        const int readSize = 256;
                        byte[] buffer = new byte[readSize];
                        MemoryStream ms = new MemoryStream();

                        int count = resp.GetResponseStream().Read(buffer, 0, readSize);
                        while (count > 0)
                        {
                            ms.Write(buffer, 0, count);
                            count = resp.GetResponseStream().Read(buffer, 0, readSize);
                        }
                        ms.Position = 0;
                        resp.GetResponseStream().Close();

                        StreamReader validatePage = new StreamReader(ms);
                        string validatePage2 = validatePage.ReadToEnd();

                        if (!validatePage2.Contains("no financial transaction"))
                        {
                            ms.Position = 0;
                            FileStream fstr = File.Create(StatementMoveDir + "\\SBI_" + AccountNumbers[i] + ".xls");
                            ms.CopyTo(fstr);
                            fstr.Close();
                            resp.Close();
                            common.InsertBankScriptReports("SBI", "SBI_" + AccountNumbers[i] + ".xls", 0, "", DateTime.Now, "Success", "");
                        }
                        else
                            common.InsertBankScriptReports("SBI", "", 0, "", DateTime.Now, "Success", "No Records");

                    }
                    catch (Exception ex)
                    {
                        Common.LogError("Unable to Login. " + ex.ToString(), "SBI");
                    }
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://corp.onlinesbi.com/corpuser/logout.htm");
                req.Method = "GET";
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:36.0) Gecko/20100101 Firefox/36.0";
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                req.Referer = " https://corp.onlinesbi.com/corpuser/accountsummary.htm";
                req.CookieContainer = cookieContainer;
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                driver.Navigate().GoToUrl("https://corp.onlinesbi.com/corpuser/logout.htm");
                

                if (driver != null)
                    driver.Quit();

                driver.Dispose();
            }
        }


    }
}
