using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Index_Download.classes
{
    public class BSEINDEX
    {
        private string indexName { get; set; }
        private string currentValue { get; set; }
        static string[] bseRequiredIndexs = "S&P BSE SENSEX,S&P BSE SENSEX Next 50,S&P BSE SmallCap,S&P BSE MidCap,S&P BSE 100,S&P BSE 200,S&P BSE 500,S&P BSE HEALTHCARE".Split(',');
        public static CustomError getBSEIndex()
        {
            CustomError custom_error = new CustomError();
            try
            {
                Console.WriteLine("===========Base India Index started==============");


                WebRequest request = WebRequest.Create("https://api.bseindia.com/BseIndiaAPI/api/MktCapBoard/w?cat=1&type=2");

                request.Method = "GET";
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebResponse response = request.GetResponse();

                HttpWebResponse resp = (HttpWebResponse)response;

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    string responseFromServer = reader.ReadToEnd();

                    JObject json = JObject.Parse(responseFromServer);

                    var RealTime = json["RealTime"];



                    foreach (var item in RealTime)
                    {
                        commonHelper helper = new commonHelper();
                        string indexnm = item["IndexName"].ToString().Trim();
                        int indx = Array.IndexOf(bseRequiredIndexs, item["IndexName"].ToString().Trim());
                        if (indx >= 0)
                        {

                            if (indexnm == "S&P BSE SENSEX Next 50")
                            {
                                helper.currentName = "BSE SENSEX Next 50 PRI";
                            }
                            else
                            {
                                helper.currentName = item["IndexName"].ToString();
                            }
                            helper.currentValue = item["Curvalue"].ToString();
                            helper.date = Convert.ToDateTime(item["DT_TM"].ToString());
                            //custom_error = commonHelper.saveDataToDB(index);
                            commonHelper.saveData(helper);
                        }
                    }

                    reader.Close();

                    response.Close();
                }
                else
                {
                    Console.WriteLine("Request Faild!");
                    commonHelper.WriteLog("Base India Index failed", "E");
                    custom_error.index_name = "Base India Index";
                    custom_error.status = "fail";
                    custom_error.is_success = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                commonHelper.WriteLog("Base India Index exception" + ex.Message,"E");

                custom_error.index_name = "Base India Index";
                custom_error.status = "fail";
                custom_error.error_msg = ex.Message;
                custom_error.is_success = false;
            }
            return custom_error;
        }
        public static CustomError getBSEHealthCareIndex()
        {
            CustomError custom_error = new CustomError();
            try
            {
                Console.WriteLine("===========Base India Index started==============");


                WebRequest request = WebRequest.Create("https://api.bseindia.com/BseIndiaAPI/api/MktCapBoard/w?cat=2&type=2");

                request.Method = "GET";
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebResponse response = request.GetResponse();

                HttpWebResponse resp = (HttpWebResponse)response;

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    string responseFromServer = reader.ReadToEnd();

                    JObject json = JObject.Parse(responseFromServer);

                    var RealTime = json["RealTime"];



                    foreach (var item in RealTime)
                    {
                        commonHelper helper = new commonHelper();
                        string indexnm = item["IndexName"].ToString().Trim();
                        if (indexnm == "S&P BSE Healthcare")
                        {
                            helper.currentName = "BSE HEALTHCARE";
                            helper.currentValue = item["Curvalue"].ToString();
                            helper.date = Convert.ToDateTime(item["DT_TM"].ToString());
                            //custom_error = commonHelper.saveDataToDB(index);
                            commonHelper.saveData(helper);
                        }
                    }
                    reader.Close();
                    response.Close();
                }
                else
                {
                    Console.WriteLine("Request Faild!");
                    commonHelper.WriteLog("BSE HEALTHCARE failed", "E");
                    custom_error.index_name = "BSE HEALTHCARE";
                    custom_error.status = "fail";
                    custom_error.error_msg = "Request Failed";
                    custom_error.is_success = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("BSE HEALTHCARE exception" + ex.Message);
                commonHelper.WriteLog("BSE HEALTHCARE exception :" + ex.Message, "E");

                custom_error.index_name = "BSE HEALTHCARE";
                custom_error.status = "fail";
                custom_error.error_msg = ex.Message;
                custom_error.is_success = false;
            }
            return custom_error;
        }
    }
}
