using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Index_Download.classes
{
    public class Lbma
    {
        public DateTime date { get; set; }

        public string currentValue { get; set; }

        public static CustomError getGoldValueAsync()
        {
            CustomError custom_error = new CustomError();
            var year = DateTime.UtcNow.AddMinutes(330).Year;
            try
            {
                Console.WriteLine("============Lbma Gold Index Process Started=============");

                   
                WebRequest request = WebRequest.Create("http://lbma.datanauts.co.uk/table?metal=gold&year=" + year + "&type=daily");
                // WebRequest request = WebRequest.Create("http://www.lbma.org.uk/precious-metal-prices"); Commented By Ghanshyam on 06
                request.Method = "GET";
                //ServicePointManager.MaxServicePointIdleTime = 1000;

                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;



                WebResponse response = request.GetResponse();

                HttpWebResponse resp = (HttpWebResponse)response;

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    string responseFromServer = reader.ReadToEnd();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(responseFromServer);

                    commonHelper GOLD_rate = new commonHelper();



                    var current_date = doc.DocumentNode.SelectNodes("//table[@class='data']/tbody/tr[1]/td[1]").FirstOrDefault();
                    if (current_date != null)
                    {

                        GOLD_rate.date = DateTime.ParseExact(current_date.InnerText, "dd-MMM-yy", CultureInfo.InvariantCulture);
                        GOLD_rate.currentValue = doc.DocumentNode.SelectNodes("//table[@class='data']/tbody/tr[1]/td[3]").FirstOrDefault().InnerText;
                        GOLD_rate.currentName = "LBMA Gold Rate";
                        //custom_error = commonHelper.saveDataToDB(GOLD_rate);
                        commonHelper.saveData(GOLD_rate);
                    }
                    reader.Close();
                    response.Close();
                }
                else
                {
                    Console.WriteLine("Request Faild!");
                    commonHelper.WriteLog("Lbma Gold Index Process Failed", "E");
                    custom_error.status = "fail";
                    custom_error.error_msg = "Request Failed";
                    custom_error.is_success = false;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                commonHelper.WriteLog("Lbma Gold Index Process Exception :" + ex.Message, "E");
                custom_error.index_name = "Lbma Gold Index Process";
                custom_error.status = "fail";
                custom_error.error_msg = ex.Message;
                custom_error.is_success = false;
            }
            return custom_error;
        }
        public static CustomError getSilverValue()
        {
            CustomError custom_error = new CustomError();
            try
            {
                Console.WriteLine("======== Lbma Silver Index Process Started=========");
                var year = DateTime.UtcNow.AddMinutes(330).Year;
                WebRequest request = WebRequest.Create("http://lbma.datanauts.co.uk/table?metal=silver&year=" + year + "&type=daily");
                request.Method = "GET";
                WebResponse response = request.GetResponse();
                HttpWebResponse resp = (HttpWebResponse)response;
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    string responseFromServer = reader.ReadToEnd();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(responseFromServer);

                    commonHelper Silver_rate = new commonHelper();



                    var current_date = doc.DocumentNode.SelectNodes("//table[@class='data']/tbody/tr[1]/td[1]").FirstOrDefault();
                    if (current_date != null)
                    {

                        Silver_rate.date = DateTime.ParseExact(current_date.InnerText, "dd-MMM-yy", CultureInfo.InvariantCulture);
                        Silver_rate.currentValue = doc.DocumentNode.SelectNodes("//table[@class='data']/tbody/tr[1]/td[2]").FirstOrDefault().InnerText;
                        Silver_rate.currentName = "LBMA Silver Rate";
                        //custom_error = commonHelper.saveDataToDB(Silver_rate);
                        commonHelper.saveData(Silver_rate);
                    }
                    reader.Close();
                    response.Close();

                }
                else
                {
                    Console.WriteLine("Request Faild!");
                    commonHelper.WriteLog("Lbma Silver Index Process failed", "E");
                    custom_error.status = "fail";
                    custom_error.error_msg = "Request Failed";
                    custom_error.is_success = false;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                commonHelper.WriteLog("Lbma Silver Index Process Exception", "E");
                custom_error.index_name = "Lbma Silver Index Process";
                custom_error.status = "fail";
                custom_error.error_msg = "Request Failed";
                custom_error.is_success = false;


            }
            return custom_error;
        }
    }
}
