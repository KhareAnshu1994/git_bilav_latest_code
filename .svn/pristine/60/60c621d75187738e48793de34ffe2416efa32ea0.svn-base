using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Index_Download.classes
{
    public class MarketWatch
    {
        NameValueCollection common_setting = (NameValueCollection)ConfigurationManager.GetSection("customAppSettingsGroup/CommonAppSettings");
        public bool GetMarketWatchesIndex()
        {
            int morning_starttime = Convert.ToInt32(common_setting["MorningStartTime"].ToString());
            int morning_endtime = Convert.ToInt32(common_setting["MorningEndTime"].ToString());

            int evening_starttime = Convert.ToInt32(common_setting["EveningStartTime"].ToString());
            int evening_endtime = Convert.ToInt32(common_setting["EveningEndTime"].ToString());

            TimeSpan start9AM = new TimeSpan(morning_starttime, 0, 0); //09 o'clock
            TimeSpan end11AM = new TimeSpan(morning_endtime, 0, 0); //11 o'clock

            TimeSpan start16PM = new TimeSpan(evening_starttime, 0, 0); //09 o'clock
            TimeSpan end16PM = new TimeSpan(evening_endtime, 0, 0); //11 o'clock
            Console.WriteLine("ok1");
            try
            {
                Console.WriteLine("===========Bloomberg(MarketWatch) Index started==============");
                //commonHelper.saveLogs("Base India Index started");
                WebRequest request = WebRequest.Create("https://www.marketwatch.com/investing/index/comp");
                request.Method = "GET";
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebResponse response = request.GetResponse();
                Console.WriteLine("ok2");
                HttpWebResponse resp = (HttpWebResponse)response;

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    string responseFromServer = reader.ReadToEnd();
                    Console.WriteLine("ok3");
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(responseFromServer);

                    var indexData = doc.DocumentNode.SelectNodes("/html/body/div[1]/div[5]/div[2]/div[3]/div[4]/table").FirstOrDefault();

                    List<List<string>> table = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div[5]/div[2]/div[3]/div[4]/table")
            .Descendants("tr")
            .Skip(1)
            .Where(tr => tr.Elements("td").Count() > 1)
            .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
            .ToList();
                    NameValueCollection common_setting2 = (NameValueCollection)ConfigurationManager.GetSection("customAppSettingsGroup/CommonAppSettings");
                    string[] Req9AMIndexes = common_setting2["bloom9AMindexes"].Split(',');
                    string[] Req16PMIndexes = common_setting2["bloom16PMindexes"].Split(',');

                    foreach (var item in table)
                    {

                        commonHelper index = new commonHelper();
                        index.currentName = item[0];
                        index.currentValue = item[1].Replace(",", "");


                        TimeSpan now = DateTime.Now.TimeOfDay;
                        Console.WriteLine("ok4");
                        if ((now > start9AM) && (now < end11AM))
                        {
                            Console.WriteLine("ok5");
                            if (Req9AMIndexes.Contains(index.currentName))
                            {
                                index.date = DateTime.Now.AddDays(-1);
                                commonHelper.saveData(index);
                            }
                            //match found
                        }

                        if ((now > start16PM) && (now < end16PM))
                        {
                            Console.WriteLine("ok6");
                            if (Req16PMIndexes.Contains(index.currentName))
                            {
                                index.date = DateTime.Now;
                                commonHelper.saveData(index);
                            }
                            //match found
                        }
                    }
                    reader.Close();
                    response.Close();
                }
                else
                {
                    Console.WriteLine("Request Faild!");
                    // commonHelper.saveLogs("Base India Index failed");
                }

                // commonHelper.saveLogs("Base India Index ended");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // commonHelper.saveLogs("Base India Index exception");
                commonHelper.WriteLog("failed to get index of market watch :"+ex.Message,"E");
                commonHelper error = new commonHelper();
                error.currentName = "Bloomberg/MarketWatch Index";
                error.currentValue = ex.Message;
                error.date = DateTime.UtcNow.AddMinutes(330);
                //commonHelper.saveErrorLogData(error);

                return false;
            }

        }

    }
}
