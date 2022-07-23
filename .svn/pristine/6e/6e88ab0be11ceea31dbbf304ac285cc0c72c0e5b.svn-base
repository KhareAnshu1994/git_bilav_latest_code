using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Index_Download.classes
{
    public class BSE_Low_Volatility_Index
    {

        public DateTime date { get; set; }

        public string currentValue { get; set; }

        public string currentValue2 { get; set; }


        public static CustomError getBSE_Low_Volatility_Index()
        {
            CustomError custom_error = new CustomError();
            try
            {

                Console.WriteLine("=============ASIA BSE_Low_Volatility_Index started=============");

                //String url = "https://www.spglobal.com/spdji/en/idsexport/file.xls?hostIdentifier=48190c8c-42c4-46af-8d1a-0cd5db894797&redesignExport=true&languageId=1&selectedModule=PerformanceGraphView&selectedSubModule=Graph&yearFlag=oneYearFlag&indexId=1852744";


                string url = "https://www.asiaindex.co.in/idsexport/file.xls?hostIdentifier=4830886b-d41d-4302-b891-726f56653b1e&selectedModule=PerformanceGraphView&selectedSubModule=Graph&yearFlag=oneYearFlag&indexId=92033482 ";

                WebClient Client = new WebClient();
                Client.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
                Client.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");

                NameValueCollection common_setting = (NameValueCollection)ConfigurationManager.GetSection("customAppSettingsGroup/CommonAppSettings");

                Console.WriteLine("=======> Excel File Downloading...");
                if (File.Exists(common_setting["MoveDirPath"] + "/BSE_Low_Volatility_Index.xls"))
                {
                    File.Delete(common_setting["MoveDirPath"] + "/BSE_Low_Volatility_Index.xls");
                }
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Client.DownloadFile(url, common_setting["MoveDirPath"] + "/BSE_Low_Volatility_Index.xls");

                Console.WriteLine("=======> Downloading Complete");
                // below line of code uncommented for combined code
                custom_error = Process_BSE_Low_Volatility_Index(common_setting["MoveDirPath"]);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                commonHelper.WriteLog("Bse low volatility index Exception", "E");

                custom_error.index_name = "Bse low volatility index ";
                custom_error.error_msg = ex.Message;
                custom_error.status = "fail";
                custom_error.is_success = false;
            }
            return custom_error;
        }

        public static CustomError Process_BSE_Low_Volatility_Index(string dir_path)
        {
            CustomError custom_error = new CustomError();
            try
            {
                string con = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dir_path + "/BSE_Low_Volatility_Index.xls;" + @"Extended Properties='Excel 8.0;HDR=Yes;'";
                using (OleDbConnection connection = new OleDbConnection(con))
                {
                    connection.Open();
                    System.Data.DataTable dbSchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, null);

                    var firstSheetName = dbSchema.Rows[0]["TABLE_NAME"];

                    OleDbCommand command = new OleDbCommand("select * from [" + firstSheetName + "]", connection);

                    List<BSE_Low_Volatility_Index> BSEIndex = new List<BSE_Low_Volatility_Index>();

                    using (OleDbDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (dr[0].ToString().Trim() != string.Empty)
                            {
                                /*HtmlDocument doc = new HtmlDocument();
                                var date = doc.DocumentNode.SelectNodes("//table[@class='data']/tbody/tr[1]/td[1]").LastOrDefault();*/

                                //var date = DateTime.ParseExact(dr[0].ToString().Trim(), "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);

                                BSE_Low_Volatility_Index index = new BSE_Low_Volatility_Index();
                                index.date = commonHelper.GetFormatedDate(dr[0].ToString().Trim());
                                index.currentValue = dr[1].ToString().Trim();
                                index.currentValue2 = dr[2].ToString().Trim();

                                BSEIndex.Add(index);


                            }

                        }


                        if (BSEIndex.Count() > 0)
                        {
                            {
                                commonHelper helper = new commonHelper();
                                helper.date = BSEIndex.LastOrDefault().date;
                                helper.currentValue = BSEIndex.LastOrDefault().currentValue2;
                                helper.currentName = "PBSELOWVOL";   //changes by anshu 05/04/2022
                                commonHelper.saveData(helper);


                                helper = new commonHelper();

                                helper.date = BSEIndex.LastOrDefault().date;
                                helper.currentName = "S&P BSE Low Volatility Index";
                                helper.currentValue = BSEIndex.LastOrDefault().currentValue;
                                commonHelper.saveData(helper);
                            }



                        }
                    }

                }
            }

            catch (Exception ex)
            {
                commonHelper.WriteLog("error in function ProcessBseSensexIndex() : " + ex.Message, "E");
                Console.WriteLine("error in function ProcessBseSensexIndex()  : " + ex.Message);
                custom_error.index_name = "Asia Index Health Care";
                custom_error.error_msg = ex.Message;
                custom_error.status = "fail";
                custom_error.is_success = false;
            }
            return custom_error;
        }
    }
}

