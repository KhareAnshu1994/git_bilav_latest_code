using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Index_Download.classes
{
    public class Fbil
    {
        public DateTime date { get; set; }

        public string currentValue { get; set; }

        public static CustomError getINRValue()
        {
            CustomError custom_error = new CustomError();
            try
            {
                Console.WriteLine("FBIL Index Process Started");

                WebRequest request = WebRequest.Create("http://www.fbil.org.in/");
                request.Method = "GET";
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                WebResponse response = request.GetResponse();
                HttpWebResponse resp = (HttpWebResponse)response;
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    string responseFromServer = reader.ReadToEnd();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(responseFromServer);

                    commonHelper INR_rate = new commonHelper();

                    var current_date = doc.DocumentNode.SelectNodes("//table[@id='grdRefeRate']/tr[2]/td[1]").FirstOrDefault();
                    if (current_date != null)
                    {
                        var current_time = doc.DocumentNode.SelectNodes("//table[@id='grdRefeRate']/tr[2]/td[2]").FirstOrDefault();
                        INR_rate.date = Convert.ToDateTime(current_date.InnerText + " " + current_time.InnerText);
                        INR_rate.currentValue = doc.DocumentNode.SelectNodes("//table[@id='grdRefeRate']/tr[2]/td[3]").FirstOrDefault().InnerText;
                        INR_rate.currentName = "FBIL INR Value";
                        //custom_error = commonHelper.saveDataToDB(INR_rate);
                        commonHelper.saveData(INR_rate);
                    }
                    reader.Close();
                    response.Close();
                    Console.WriteLine("============= FBIL Index Process End=================");


                }
                else
                {
                    Console.WriteLine("FBIL Index Process Failed");
                    custom_error.status = "fail";
                    custom_error.is_success = false;
                    commonHelper.WriteLog("FBIL Index Process Failed","E");


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("FBIL Index Process Exception :" + ex.Message);
                commonHelper.WriteLog("FBIL Index Process Exception","E");
                custom_error.status = "fail";
                custom_error.is_success = false;
            }
            return custom_error;
        }
    }
}
