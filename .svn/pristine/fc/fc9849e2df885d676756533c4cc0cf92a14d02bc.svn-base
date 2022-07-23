using System;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Web;
using System.Data;
using System.Configuration;

namespace StatementDownloadUtility
{
    public class HDFCBank
    {
        private CookieContainer cc = new CookieContainer();
        private string[] CorporateId { get; set; }
        private string UserId { get; set; }
        private string Password { get; set; }
        private bool IsActive { get; set; }
        private string[] AccountNumbers { get; set; }
        //private string[] CustomerId { get; set; }
        private string StatementDownloadDir { get; set; }
        private string StatementMoveDir { get; set; }
        private string HostName { get; set; }
        private string BankName { get; set; }
        private int BankCode { get; set; }


        string SessionId = string.Empty;
        string RequestId = string.Empty;

        public HDFCBank()
        {
            Common common = new Common();
            DataTable dtHDFC = common.GetBankDetails("HDFC");

            if (dtHDFC != null && dtHDFC.Rows.Count > 0)
            {
                AccountNumbers = dtHDFC.Rows[0]["Accountno"].ToString().Split(',');
                BankCode = Convert.ToInt16(dtHDFC.Rows[0]["Bankcode"].ToString());
                BankName = dtHDFC.Rows[0]["Bankname"].ToString();
                CorporateId = dtHDFC.Rows[0]["CorporatId"].ToString().Split(',');
                Password = dtHDFC.Rows[0]["Password"].ToString();
                HostName = dtHDFC.Rows[0]["Link"].ToString();
                UserId = dtHDFC.Rows[0]["Userid"].ToString();
                IsActive = Convert.ToBoolean(dtHDFC.Rows[0]["IsActive"]);
                StatementMoveDir = ConfigurationManager.AppSettings["FileMoveDir"];
            }
            else return;
        }

        public void DownloadStatement()
        {
            try
            {
                if (!IsActive)
                    return;

                if (Login())
                {
                    Common.LogError("Login successed", "HDFC");
                    for (int i = 0; i < AccountNumbers.Length; i++)
                    {
                        try
                        {
                            doStuff(AccountNumbers[i], CorporateId[i]);
                            getFile(AccountNumbers[i], CorporateId[i]);
                        }
                        catch (Exception ex)
                        {
                            Common.LogError(ex.ToString(), "HDFC");
                        }
                    }
                    logout();
                }
            }
            catch (Exception ex)
            {
                Common.LogError(ex.ToString(), "HDFC");
                return;
            }

        }

        private bool Login()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(HostName + "/corporate/CorporateLogin.html");
            req.Method = "GET";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.CookieContainer = cc;
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                if (resp.Cookies.Count == 0)
                {
                    Common.LogError("Unable to aquire Cookies. Status Code is " + resp.StatusCode.ToString(), "HDFC");
                    return false;
                }
                resp.Close();

            }
            catch (Exception ex)
            {
                Common.LogError("Unable to aquire Cookies" + ex.ToString(), "HDFC");
                return false;
            }
            HttpWebRequest req1 = (HttpWebRequest)WebRequest.Create(HostName + "/corporate/entry");
            req1.CookieContainer = cc;
            req1.Method = "POST";
            NameValueCollection nvc = HttpUtility.ParseQueryString(string.Empty);
            nvc.Add("fldLoginUserId", UserId);//ASMITA@UTIMF
            nvc.Add("fldPassword", Password);//Asmita2016
            nvc.Add("fldAppId", "CO");
            nvc.Add("fldTxnId", "LGN");
            nvc.Add("fldScrnSeqNbr", "01");
            nvc.Add("fldLangId", "eng");
            nvc.Add("fldDeviceId", "01");

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
                //Console.WriteLine("GetResponse");
                if (resp.StatusCode != HttpStatusCode.OK)
                {
                    Common.LogError("Login Failed. Status Code is " + resp.StatusCode.ToString(), "HDFC");
                    return false;
                }
                Stream Answer = resp.GetResponseStream();
                StreamReader _Answer = new StreamReader(Answer);
                string page = _Answer.ReadToEnd();
                //Console.WriteLine("page.ToLower()");
                if (page.ToLower().Contains("account locked"))
                {
                    Common.LogError("Accont Locked", "HDFC");
                    return false;
                }
                else
                {
                    Int32 sessInd = page.IndexOf("var	sessionId 		=	'");
                    SessionId = page.Substring(sessInd + "var	sessionId 		=	'".Length, 18);
                    //Int32 reqInd = page.IndexOf("var	requestId 		=	'");
                    //SessionId = page.Substring(reqInd + "var	requestId 		=	'".Length, 29);
                    resp.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Common.LogError("Login Failed" + ex.ToString(), "HDFC");
                return false;
            }
        }

        private void doStuff(string accNo, string custId)
        {
            HttpWebRequest req1 = (HttpWebRequest)WebRequest.Create(HostName + "/corporate/entry?fldAppId=CO&fldTxnId=MNU&fldScrnSeqNbr=31&fldSessionId=" + SessionId);
            string startDate = DateTime.Now.AddDays(-1).Day + "/" + DateTime.Now.AddDays(-1).Month + "/" + DateTime.Now.AddDays(-1).Year;
            string endDate = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;


            req1.CookieContainer = cc;
            req1.Method = "GET";
            HttpWebResponse resp = (HttpWebResponse)req1.GetResponse();
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                Common.LogError("Unable to get welcome screen. Status Code is " + resp.StatusCode.ToString(), "HDFC");
                return;
            }
            resp.Close();

            HttpWebRequest req2 = (HttpWebRequest)WebRequest.Create(HostName + "/corporate/entry");
            req2.CookieContainer = cc;
            req2.Method = "POST";
            req2.ContentType = "application/x-www-form-urlencoded";

            NameValueCollection postColl = HttpUtility.ParseQueryString(String.Empty);
            postColl.Add("fldAppId", "CO");
            postColl.Add("fldTxnId", "CAA");
            postColl.Add("fldScrnSeqNbr", "04");
            postColl.Add("fldSessionId", SessionId);
            postColl.Add("fldRequestId", RequestId);
            postColl.Add("fldFilterApplied", "Y");
            postColl.Add("fldStartRecNbr", "0");
            postColl.Add("fldEndRecNbr", "0");
            postColl.Add("fldShowAll", "Y");
            postColl.Add("fldSelCurr", "INR");
            postColl.Add("fldActionType", "12");
            postColl.Add("fldTxnCustId", custId);
            postColl.Add("fldBrnCode", "60");
            postColl.Add("fldAccountNo", accNo);
            postColl.Add("fldNbrRecords", "5");
            postColl.Add("fldStartTxnDate", startDate);
            postColl.Add("fldEndTxnDate", endDate);
            postColl.Add("fldStartValDate", "");
            postColl.Add("fldEndValDate", "");
            postColl.Add("fldStartAmount", "");
            postColl.Add("fldEndAmount", "");
            postColl.Add("fldCrDr", "B");
            postColl.Add("fldFcatRefNo", "");
            postColl.Add("fldSortOrder", "DSC");
            postColl.Add("fldDnldType", "CSV");
            postColl.Add("fldDelimiter", "|");
            postColl.Add("fldSelectedListFields", "txndate~Transaction+Date#valuedate~Value+Date#amount~Transaction+Amount#debitcredit~Debit+/+Credit#description~Transaction+Description");

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
                    Common.LogError("Unable to get activity screen. Status Code is " + resp.StatusCode.ToString(), "HDFC");
                    return;
                }
                resp.Close();
            }
            catch (Exception ex)
            {
                Common.LogError("Unable to get activity screen" + ex.ToString(), "HDFC");
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
                HttpWebRequest req3 = (HttpWebRequest)WebRequest.Create(HostName + "/corporate/entry");
                req3.CookieContainer = cc;

                string startDate = DateTime.Now.AddDays(-1).Day + "/" + DateTime.Now.AddDays(-1).Month + "/" + DateTime.Now.AddDays(-1).Year;
                string endDate = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                req3.Method = "POST";
                req3.ContentType = "application/x-www-form-urlencoded";

                NameValueCollection postColl2 = HttpUtility.ParseQueryString(String.Empty);
                postColl2.Clear();
                postColl2.Add("fldAppId", "CO");
                postColl2.Add("fldTxnId", "CAA");
                postColl2.Add("fldScrnSeqNbr", "07");
                postColl2.Add("fldSessionId", SessionId);
                postColl2.Add("fldRequestId", RequestId);
                postColl2.Add("fldFilterApplied", "Y");
                postColl2.Add("fldStartRecNbr", "0");
                postColl2.Add("fldEndRecNbr", "0");
                postColl2.Add("fldShowAll", "Y");
                postColl2.Add("fldAccountNo", accNo);
                postColl2.Add("fldCrDr", "B");
                postColl2.Add("fldSortOrder", "DSC");
                postColl2.Add("fldStartTxnDate", startDate);
                postColl2.Add("fldEndTxnDate", endDate);
                postColl2.Add("fldStartValDate", "");
                postColl2.Add("fldEndValDate", "");
                postColl2.Add("fldBrnCode", "60");
                postColl2.Add("fldStartAmount", "");
                postColl2.Add("fldEndAmount", "");
                postColl2.Add("fldNbrRecords", "5");
                postColl2.Add("fldSelCurr", "INR");
                postColl2.Add("fldTxnCustId", custId);
                postColl2.Add("fldActionType", "12");
                postColl2.Add("fldFirstHolder", "UTI+LIQUID+FUND+-CASH+PLAN+-CMS+COLL+A/C");
                postColl2.Add("fldSrcAcctCurr", "INR");
                postColl2.Add("fldDnldType", "CSV");
                postColl2.Add("fldDelimiter", "|");
                postColl2.Add("fldSelectedListFields", "txndate~Transaction+Date#valuedate~Value+Date#amount~Transaction+Amount#debitcredit~Debit+/+Credit#description~Transaction+Description");


                string postData = postColl2.ToString();
                byte[] data = Encoding.ASCII.GetBytes(postData);
                req3.ContentLength = data.Length;
                Stream reqStream = req3.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
                HttpWebResponse resp = (HttpWebResponse)req3.GetResponse();
                if (resp.StatusCode != HttpStatusCode.OK)
                {
                    Common.LogError("Unable to get statement screen. Status Code is " + resp.StatusCode.ToString(), "HDFC");
                    return;
                }

                Stream Answer = resp.GetResponseStream();
                string fileName = "HDFC_" + accNo + ".CSV";
                FileStream fstr = File.Create(StatementMoveDir + "\\" + fileName);
                Answer.CopyTo(fstr);
                fileSize = fstr.Length;
                fstr.Close();
                resp.Close();
                Common.LogError("File downloaded :", "HDFC");

            }
            catch (Exception ex)
            {
                Common.LogError(ex.ToString(), "HDFC");
                return;
            }

        }

        private void logout()
        {
            HttpWebRequest req3 = (HttpWebRequest)WebRequest.Create(HostName + "/corporate/entry");
            req3.CookieContainer = cc;
            req3.Method = "POST";
            req3.ContentType = "application/x-www-form-urlencoded";

            NameValueCollection postColl2 = HttpUtility.ParseQueryString(String.Empty);
            postColl2.Clear();
            postColl2.Add("fldAppId", "CO");
            postColl2.Add("fldTxnId", "LGF");
            postColl2.Add("fldScrnSeqNbr", "99");
            postColl2.Add("fldSessionId", SessionId);
            postColl2.Add("fldGroup", "AC");
            postColl2.Add("fldDefTxnId", "ASB");

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
            else
            {
                Common.LogError("Logged out successfull", "HDFC");
            }

            resp.Close();
        }
    }
}
