using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Linq;
using System.Net;
using System.Data;
using System;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Web;
using System.Text;
using System.IO;

namespace StatementDownloadUtility
{
    public class ICICIBank
    {
        private CookieContainer cc = new CookieContainer();
        public string CorporateId { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string[] AccountNumbers { get; set; }
        public string[] CustomerId { get; set; }
        public string StatementDownloadDir { get; set; }
        public string StatementMoveDir { get; set; }
        public string HostName { get; set; }

        string sessionLink = string.Empty;
        string referer = string.Empty;
        //static FirefoxProfile fireFoxProfile = null;
        //IWebDriver driver = null;
        //IWebElement element = null;

        public ICICIBank()
        {
            Common common = new Common();
            DataTable dtICICI = common.GetBankDetails("ICICI");
            if (dtICICI != null && dtICICI.Rows.Count > 0)
            {
                AccountNumbers = dtICICI.Rows[0]["ACCOUNTNO"].ToString().Split(',');
                CorporateId = dtICICI.Rows[0]["CORPORATID"].ToString();
                Password = dtICICI.Rows[0]["PASSWORD"].ToString();
                HostName = dtICICI.Rows[0]["LINK"].ToString();
                UserId = dtICICI.Rows[0]["USERID"].ToString();
                IsActive = Convert.ToBoolean(dtICICI.Rows[0]["IsActive"]);
                StatementMoveDir = ConfigurationManager.AppSettings["FileMoveDir"];
            }
            else return;
            //DownloadStatement();
        }

        public void DownloadStatement()
        {
            try
            {
                if (!IsActive)
                    return;
                if (Login())
                {
                    for (int i = 0; i < AccountNumbers.Length; i++)
                    {
                        try
                        {
                            //doStuff(AccountNumbers[i], CorporateId[i]);
                            //getFile(AccountNumbers[i], CorporateId[i]);
                        }
                        catch (Exception ex)
                        {
                            Common.LogError(ex.ToString(), "ICICI");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogError(ex.ToString(), "ICICI");
                return;
            }
        }

        private bool Login()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(HostName + "//cib.icicibank.com/corp/BANKAWAY?Action.CorpUser.Init1.001=Y&AppSignonBankId=ICI&AppType=corporate");
            req.Method = "GET";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.CookieContainer = cc;
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                if (resp.Cookies.Count == 0)
                {
                    Common.LogError("Unable to aquire Cookies. Status Code is " + resp.StatusCode.ToString(), "ICICI");
                    return false;
                }
                resp.Close();
            }
            catch (Exception ex)
            {
                Common.LogError("Unable to aquire Cookies" + ex.ToString(), "ICICI");
                return false;
            }

            HttpWebRequest req1 = (HttpWebRequest)WebRequest.Create(HostName + "//cib.icicibank.com/corp/BANKAWAY?Action.CorpUser.Init1.001=Y&AppSignonBankId=ICI&AppType=corporate");
            req1.CookieContainer = cc;
            req1.Method = "POST";
            NameValueCollection nvc = HttpUtility.ParseQueryString(string.Empty);
            nvc.Add("bwayparam", "ckV86B8MzsfUf6l/x+n0+t4IJihuajWcTYEYoka8fIk=");
            nvc.Add("AuthenticationFG.USER_PRINCIPAL", "UTIMUTUALFUND1872005.PRAMILAJ");
            nvc.Add("dummy1", "");
            nvc.Add("MIN_LENGTH_OF_PASSWORD", "12");
            nvc.Add("PWD_SPECIAL_CHAR_MANDATORY", "Y");
            nvc.Add("PWDDIGITMAND", "Y");
            nvc.Add("PASSWORD_STRENGTH", "Too+short|Weak|Fair|Strong|Very+Strong");
            nvc.Add("IMAGE_PATH", "L001/consumer/images");
            nvc.Add("AuthenticationFG.ACCESS_CODE", "15d29524f11d2245dac5c129ad3420461b7b703e79a0501a6ffeb20fd7a83921c1e9ec6e4af40b74339998b66e6edf2ab4eb0b6ffc5f2398d063466cb90fd2ba875d808da537fb7b9f169c12d73d0e2df8b6493456b48ec44e11a43a7d0121ca095be708836fed0a80168360c490489e28b7a4cad25d640e4bdd3f889ce5ea78");
            nvc.Add("MIN_LENGTH_OF_PASSWORD", "12");
            nvc.Add("PWD_SPECIAL_CHAR_MANDATORY", "Y");
            nvc.Add("PWDDIGITMAND", "Y");
            nvc.Add("PASSWORD_STRENGTH", "Too+short|Weak|Fair|Strong|Very+Strong");
            nvc.Add("IMAGE_PATH", "L001/consumer/images");
            nvc.Add("AuthenticationFG.MENU_ID", "3");
            nvc.Add("Action.VALIDATE_CREDENTIALS_UX", "PROCEED");
            nvc.Add("FG_BUTTONS__", "VALIDATE_CREDENTIALS,STU_VALIDATE_CREDENTIALS,VALIDATE_CREDENTIALS_DIG_CERT,BACK,CLEAR_VALUES");
            nvc.Add("AuthenticationFG.IS_FIRST_AUTHENTICATION", "Y");
            nvc.Add("QS", "");
            nvc.Add("USER_ID_COOKIE", "");
            nvc.Add("CATEGORY_ID", "");
            nvc.Add("AuthenticationFG.PREFERRED_LANGUAGE", "001");
            nvc.Add("userType", "1");
            nvc.Add("bankId", "ICI");
            nvc.Add("languageId", "001");
            nvc.Add("FORMSGROUP_ID__", "AuthenticationFG");
            nvc.Add("AuthenticationFG.REPORTTITLE", "AuthenticationScreen");
            nvc.Add("RIA_TARGETS", "null");
            nvc.Add("JS_ENABLED_FLAG", "Y");
            nvc.Add("DECRYPT_FLAG", "Y");
            nvc.Add("CHECKBOX_NAMES__", "");
            nvc.Add("Requestid", "1");
            nvc.Add("__JS_ENCRYPT_KEY__", "10001,9acc898893f576bedf70ded5f037472e5b4cb51592513bfb2ce091a0839f2df55f00e41c646e0279ff2983a1bad633d2cb1eaa74685014063a02ed4396e22710a4170b7a12e8e52c74678f9db7ed0a297653a95792c212ba5f1d5129149264e31ccf4695e2ef2bd0fda3d67ea2d0c2ad997d478c9c0d5a07651a038e46f90129,131");
            nvc.Add("deviceDNA", "");
            nvc.Add("executionTime", "0");
            nvc.Add("desc", "");
            nvc.Add("mesc", "mi=2;cd=200;id=50");
            nvc.Add("dnaError", "");
            nvc.Add("mescIterationCount", "0");
            nvc.Add("isDNADone", "false");
            nvc.Add("arcotFlashCookie", "");

            string postData = nvc.ToString();
            byte[] data = Encoding.ASCII.GetBytes(postData);
            req1.ContentType = "application/x-www-form-urlencoded";
            req1.ContentLength = data.Length;

            try
            {
                Stream reqStream = req1.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                HttpWebResponse resp = (HttpWebResponse)req1.GetResponse();
                if (resp.StatusCode != HttpStatusCode.OK)
                {
                    Common.LogError("Login Failed. Status Code is" + resp.StatusCode.ToString(), "ICICI");
                    return false;
                }

                Stream Answer = resp.GetResponseStream();
                StreamReader _Answer = new StreamReader(Answer);
                string page = _Answer.ReadToEnd();

                if (page.ToLower().Contains("account locked"))
                {
                    Common.LogError("Accont Locked", "ICICI");
                    return false;
                }
                else
                {
                    Int32 sessInd = page.IndexOf("var sessionLink =	'");
                    sessionLink = page.Substring(sessInd + "var	sessionLink = '".Length, 18);
                    resp.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Common.LogError("Login Failed" + ex.ToString(), "ICICI");
                return false;
            }
        }

        private void doStuff(string accNo, string custId)
        {
            HttpWebRequest req1 = (HttpWebRequest)WebRequest.Create(HostName + "/corporate/entry?fldAppId=CO&fldTxnId=MNU&fldScrnSeqNbr=31&fldSessionId=" + sessionLink);
            string startDate = DateTime.Now.AddDays(-1).Day + "/" + DateTime.Now.AddDays(-1).Month + "/" + DateTime.Now.AddDays(-1).Year;
            string endDate = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;


            req1.CookieContainer = cc;
            req1.Method = "GET";
            HttpWebResponse resp = (HttpWebResponse)req1.GetResponse();
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                Common.LogError("Unable to get welcome screen. Status Code is " + resp.StatusCode.ToString(), "ICICI");
                return;
            }
            resp.Close();

            HttpWebRequest req2 = (HttpWebRequest)WebRequest.Create(HostName + "//cib.icicibank.com/corp/BANKAWAY?Action.CorpUser.Init1.001=Y&AppSignonBankId=ICI&AppType=corporate");
            req2.CookieContainer = cc;
            req2.Method = "POST";
            req2.ContentType = "application/x-www-form-urlencoded";

            NameValueCollection postColl = HttpUtility.ParseQueryString(String.Empty);
            postColl.Add("AuthenticationFG.ACCESS_CODE", "65a69896fa321596d766e6b4180f5cdeceb54f5433371eed0e03dd6d2126d16607f9ccc176acfd9be9aac1c6ab0a59dfdd9528b989aa444f4971de95fb04f087163b375f0c0f1822062a1a37dd99de25f6883a2744018efcbbf2b78bd38297ee7bff724d4db2e25d369e9ab534d1530d2a75b93039b823cde804d2cd9945e0f6");
            postColl.Add("MIN_LENGTH_OF_PASSWORD", "6");
            postColl.Add("PWD_SPECIAL_CHAR_MANDATORY", "Y");
            postColl.Add("PWDDIGITMAND", "Y");
            postColl.Add("PASSWORD_STRENGTH", "Too+short|Weak|Fair|Strong|Very+Strong");
            postColl.Add("IMAGE_PATH", "L001/consumer/images");
            postColl.Add("AuthenticationFG.MENU_ID", "3");
            postColl.Add("Action.VALIDATE_CREDENTIALS_UX", "PROCEED");
            postColl.Add("FG_BUTTONS", "VALIDATE_CREDENTIALS,STU_VALIDATE_CREDENTIALS,VALIDATE_CREDENTIALS_DIG_CERT,BACK,CLEAR_VALUES");
            postColl.Add("AuthenticationFG.IS_FIRST_AUTHENTICATION", "Y");
            postColl.Add("QS", "");
            postColl.Add("USER_ID_COOKIE", "");
            postColl.Add("AuthenticationFG.PREFERRED_LANGUAGE", "001");
            postColl.Add("userType", "1");
            postColl.Add("bankId", "ICI");
            postColl.Add("languageId", "001");
            postColl.Add("FORMSGROUP_ID", "AuthenticationFG");
            postColl.Add("AuthenticationFG.REPORTTITLE", "AuthenticationScreen");
            postColl.Add("RIA_TARGETS", "null");
            postColl.Add("JS_ENABLED_FLAG", "Y");
            postColl.Add("DECRYPT_FLAG", "Y");
            postColl.Add("CHECKBOX_NAMES", "");
            postColl.Add("Requestid", "1");
            postColl.Add("__JS_ENCRYPT_KEY__", "10001,8ef9fe4a4ea9f3e8c3a550718bd9ba8ea2798d37d9cee8cf39f40f6e6453b340e2e4bc16bf436264fdcae468ecb0e6e7f7beccfc5dd5f2a6d2662fcd34dcd8a55039f5ff3e6516d1893462ccb9ad773e2a961b25dc61321c91571c7eb27f3cd1eb42b4fad371dfbefb111350991024d1ff3a7679e71f1f23be4af297fe0bad41,131");
            postColl.Add("deviceDNA", "");
            postColl.Add("executionTime", "0");
            postColl.Add("desc", "");
            postColl.Add("mesc", "mi=2;cd=200;id=50");
            postColl.Add("dnaError", "");
            postColl.Add("mescIterationCount", "0");
            postColl.Add("isDNADone", "false");
            postColl.Add("arcotFlashCookie", "");

            try
            {
                String postData = postColl.ToString();
                byte[] data = Encoding.ASCII.GetBytes(postData);
                req2.ContentLength = data.Length;
                Stream reqStream = req2.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                resp = (HttpWebResponse)req2.GetResponse();
                if (resp.StatusCode != HttpStatusCode.OK)
                {
                    Common.LogError("Unable to get activity screen. Status Code is " + resp.StatusCode.ToString(), "ICICI");
                    return;
                }
                resp.Close();
            }
            catch (Exception ex)
            {
                Common.LogError("Unable to get activity screen" + ex.ToString(), "ICICI");
                return;
            }
        }

        private void getFile(string accNo, string custId)
        {
            DateTime ScriptRuntime = DateTime.Now;
            string strFileName = string.Empty;
            long fileSize = 0;
            string fileExtn = string.Empty;

            try
            {
                HttpWebRequest req3 = (HttpWebRequest)WebRequest.Create(HostName + "//cib.icicibank.com/corp/BANKAWAY?Action.CorpUser.Init1.001=Y&AppSignonBankId=ICI&AppType=corporate");
                req3.CookieContainer = cc;

                string startDate = DateTime.Now.AddDays(-1).Day + "/" + DateTime.Now.AddDays(-1).Month + "/" + DateTime.Now.AddDays(-1).Year;
                string endDate = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                req3.Method = "POST";
                req3.ContentType = "application/x-www-form-urlencoded";

                NameValueCollection postColl2 = HttpUtility.ParseQueryString(String.Empty);
                postColl2.Clear();
                postColl2.Add("AuthenticationFG.ACCESS_CODE", "65a69896fa321596d766e6b4180f5cdeceb54f5433371eed0e03dd6d2126d16607f9ccc176acfd9be9aac1c6ab0a59dfdd9528b989aa444f4971de95fb04f087163b375f0c0f1822062a1a37dd99de25f6883a2744018efcbbf2b78bd38297ee7bff724d4db2e25d369e9ab534d1530d2a75b93039b823cde804d2cd9945e0f6");
                postColl2.Add("MIN_LENGTH_OF_PASSWORD", "6");
                postColl2.Add("PWD_SPECIAL_CHAR_MANDATORY", "Y");
                postColl2.Add("PWDDIGITMAND", "Y");
                postColl2.Add("PASSWORD_STRENGTH", "Too+short|Weak|Fair|Strong|Very+Strong");
                postColl2.Add("IMAGE_PATH", "L001/consumer/images");
                postColl2.Add("AuthenticationFG.MENU_ID", "3");
                postColl2.Add("Action.VALIDATE_CREDENTIALS_UX", "PROCEED");
                postColl2.Add("FG_BUTTONS", "VALIDATE_CREDENTIALS,STU_VALIDATE_CREDENTIALS,VALIDATE_CREDENTIALS_DIG_CERT,BACK,CLEAR_VALUES");
                postColl2.Add("AuthenticationFG.IS_FIRST_AUTHENTICATION", "Y");
                postColl2.Add("QS", "");
                postColl2.Add("USER_ID_COOKIE", "");
                postColl2.Add("AuthenticationFG.PREFERRED_LANGUAGE", "001");
                postColl2.Add("userType", "1");
                postColl2.Add("bankId", "ICI");
                postColl2.Add("languageId", "001");
                postColl2.Add("FORMSGROUP_ID", "AuthenticationFG");
                postColl2.Add("AuthenticationFG.REPORTTITLE", "AuthenticationScreen");
                postColl2.Add("RIA_TARGETS", "null");
                postColl2.Add("JS_ENABLED_FLAG", "Y");
                postColl2.Add("DECRYPT_FLAG", "Y");
                postColl2.Add("CHECKBOX_NAMES", "");
                postColl2.Add("Requestid", "1");
                postColl2.Add("__JS_ENCRYPT_KEY__", "10001,8ef9fe4a4ea9f3e8c3a550718bd9ba8ea2798d37d9cee8cf39f40f6e6453b340e2e4bc16bf436264fdcae468ecb0e6e7f7beccfc5dd5f2a6d2662fcd34dcd8a55039f5ff3e6516d1893462ccb9ad773e2a961b25dc61321c91571c7eb27f3cd1eb42b4fad371dfbefb111350991024d1ff3a7679e71f1f23be4af297fe0bad41,131");
                postColl2.Add("deviceDNA", "");
                postColl2.Add("executionTime", "0");
                postColl2.Add("desc", "");
                postColl2.Add("mesc", "mi=2;cd=200;id=50");
                postColl2.Add("dnaError", "");
                postColl2.Add("mescIterationCount", "0");
                postColl2.Add("isDNADone", "false");
                postColl2.Add("arcotFlashCookie", "");


                string postData = postColl2.ToString();
                byte[] data = Encoding.ASCII.GetBytes(postData);
                req3.ContentLength = data.Length;
                Stream reqStream = req3.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
                HttpWebResponse resp = (HttpWebResponse)req3.GetResponse();
                if (resp.StatusCode != HttpStatusCode.OK)
                {
                    Common.LogError("Unable to get statement screen. Status Code is " + resp.StatusCode.ToString(), "ICICI");
                    return;
                }

                Stream Answer = resp.GetResponseStream();
                string fileName = "ICICI" + accNo + ".CSV";
                FileStream fstr = File.Create(StatementMoveDir + "\\" + fileName);
                Answer.CopyTo(fstr);
                fileSize = fstr.Length;
                fstr.Close();
                resp.Close();

            }
            catch (Exception ex)
            {
                Common.LogError(ex.ToString(), "ICICI");
                return;
            }

        }

        private void logout()
        {
            HttpWebRequest req3 = (HttpWebRequest)WebRequest.Create(HostName + "//cib.icicibank.com/corp/BANKAWAY?Action.CorpUser.Init1.001=Y&AppSignonBankId=ICI&AppType=corporate");
            req3.CookieContainer = cc;
            req3.Method = "POST";
            req3.ContentType = "application/x-www-form-urlencoded";

            NameValueCollection postColl2 = HttpUtility.ParseQueryString(String.Empty);
            postColl2.Clear();
            postColl2.Add("", "CO");
            postColl2.Add("", "LGF");
            postColl2.Add("", "99");
            postColl2.Add("", sessionLink);
            postColl2.Add("", "AC");
            postColl2.Add("", "ASB");

            string postData = postColl2.ToString();
            byte[] data = Encoding.ASCII.GetBytes(postData);
            req3.ContentLength = data.Length;
            Stream reqStream = req3.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();

            HttpWebResponse resp = (HttpWebResponse)req3.GetResponse();
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(String.Format("Logged out unsuccessful"));
            }

            resp.Close();
        }

        //public void DownloadStatement()
        //{
        //    fireFoxProfile = new FirefoxProfile();
        //    fireFoxProfile.SetPreference("browser.download.folderlist", 2);
        //    fireFoxProfile.SetPreference("browser.download.dir", StatementMoveDir);
        //    fireFoxProfile.SetPreference("browser.download.downloadDir", StatementMoveDir);
        //    fireFoxProfile.SetPreference("browser.download.defaultFolder", StatementMoveDir);
        //    fireFoxProfile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf");
        //    fireFoxProfile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/vnd.ms-excel");
        //    fireFoxProfile.SetPreference("pdfjs.disabled", true);
        //    fireFoxProfile.SetPreference("plugin.scan.Acrobat", "99.0");
        //    fireFoxProfile.SetPreference("plugin.scan.plid.all", false);
        //    driver = new FirefoxDriver(fireFoxProfile);

        //    driver.Navigate().GoToUrl("https://cib.icicibank.com/corp/BANKAWAY?Action.CorpUser.Init1.001=Y&AppSignonBankId=ICI&AppType=corporate");
        //    driver.Manage().Window.Maximize();
        //    element = driver.FindElement(By.Name("CorporateSignonCorpId"));
        //    element.SendKeys(CorporateId);

        //    element = driver.FindElement(By.Name("CorporateSignonUserName"));
        //    element.SendKeys(UserId);

        //    element = driver.FindElement(By.Name("CorporateSignonPassword"));
        //    element.SendKeys(Password);

        //    element = driver.FindElement(By.Id("arcotsubmit"));
        //    element.Click();

        //    string WelcomePage = driver.PageSource;
        //    sessionLink = WelcomePage.Substring(WelcomePage.IndexOf("Accounts") - 500, 500);
        //    sessionLink = sessionLink.Substring(sessionLink.IndexOf("href") + 6, sessionLink.Length - sessionLink.IndexOf("href") - 9);

        //    ICookieJar cj = driver.Manage().Cookies;
        //    ReadOnlyCollection<OpenQA.Selenium.Cookie> cookieColl = cj.AllCookies;
        //    CookieContainer cc = new CookieContainer();
        //    foreach (OpenQA.Selenium.Cookie cook in cookieColl)
        //    {
        //        cc.Add(new System.Net.Cookie(cook.Name, cook.Value, cook.Path, cook.Domain));
        //    }

        //    //DownloadFile();
        //}

        //private void DownloadFile()
        //{ }
    }
}
