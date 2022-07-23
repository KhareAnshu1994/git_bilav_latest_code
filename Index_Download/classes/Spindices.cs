using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Index_Download.classes
{
    public class Spindices
    {

        public DateTime date { get; set; }

        public string currentValue { get; set; }

        public static CustomError getBse100Index()
        {
            CustomError custom_error = new CustomError();
            try
            {

                Console.WriteLine("==========BSE 100 Index stared=============");


                String url = "https://us.spindices.com/idsexport/file.xls?hostIdentifier=48190c8c-42c4-46af-8d1a-0cd5db894797&redesignExport=true&selectedModule=PerformanceGraphView&selectedSubModule=Graph&yearFlag=oneYearFlag&indexId=1852727";

                WebClient Client = new WebClient();
                Client.Headers.Add("user-agent", "Only a test!");

                NameValueCollection common_setting = (NameValueCollection)ConfigurationManager.GetSection("customAppSettingsGroup/CommonAppSettings");

                Console.WriteLine("=====> Excel File Downloading...");

                ServicePointManager.Expect100Continue = true;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                if (File.Exists(common_setting["MoveDirPath"] + "/BSE_100_index.xls"))
                {
                    File.Delete(common_setting["MoveDirPath"] + "/BSE_100_index.xls");
                }
                Client.DownloadFile(url, common_setting["MoveDirPath"] + "/BSE_100_index.xls");

                custom_error = Helper.ProcessBse100Index(common_setting["MoveDirPath"]);

                Console.WriteLine("============> Downloading Complete");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                commonHelper.WriteLog("SP Indices BSE 100 Index exception :" + ex.Message, "E");

                custom_error.index_name = "SP Indices BSE 100";
                custom_error.status = "fail";
                custom_error.is_success = false;
                custom_error.error_msg = ex.Message;
            }
            return custom_error;
        }

        public static CustomError getBse200Index()
        {
            CustomError custom_error = new CustomError();
            try
            {

                Console.WriteLine("============BSE 200 Index stared================");

                String url = "https://us.spindices.com/idsexport/file.xls?hostIdentifier=48190c8c-42c4-46af-8d1a-0cd5db894797&redesignExport=true&selectedModule=PerformanceGraphView&selectedSubModule=Graph&yearFlag=oneYearFlag&indexId=1852730";

                WebClient Client = new WebClient();
                Client.Headers.Add("user-agent", "Only a test!");
                NameValueCollection common_setting = (NameValueCollection)ConfigurationManager.GetSection("customAppSettingsGroup/CommonAppSettings");

                Console.WriteLine("==========> Excel File Downloading...");

                ServicePointManager.Expect100Continue = true;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                if (File.Exists(common_setting["MoveDirPath"] + "/BSE_200_index.xls"))
                {
                    File.Delete(common_setting["MoveDirPath"] + "/BSE_200_index.xls");
                }
                Client.DownloadFile(url, common_setting["MoveDirPath"] + "/BSE_200_index.xls");

                custom_error = Helper.ProcessBse200Index(common_setting["MoveDirPath"]);
                Console.WriteLine("=========> Downloading Complete");


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                commonHelper.WriteLog("SP Indices BSE 200 Index exception :" + ex.Message, "E");
                custom_error.index_name = "SP Indices BSE 200";
                custom_error.status = "fail";
                custom_error.is_success = false;
                custom_error.error_msg = ex.Message;

            }
            return custom_error;
        }

        public static CustomError getBseSensexNext50()
        {
            CustomError custom_error = new CustomError();
            try
            {

                Console.WriteLine("============BSE sensex next 50 started================");

                String url = "https://us.spindices.com/idsexport/file.xls?hostIdentifier=48190c8c-42c4-46af-8d1a-0cd5db894797&redesignExport=true&selectedModule=PerformanceGraphView&selectedSubModule=Graph&yearFlag=oneYearFlag&indexId=92321493";

                WebClient Client = new WebClient();
                Client.Headers.Add("user-agent", "Only a test!");
                NameValueCollection common_setting = (NameValueCollection)ConfigurationManager.GetSection("customAppSettingsGroup/CommonAppSettings");

                Console.WriteLine("==========> Excel File Downloading...");

                ServicePointManager.Expect100Continue = true;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                if (File.Exists(common_setting["MoveDirPath"] + "/BSE_Next_50_index.xls"))
                {
                    File.Delete(common_setting["MoveDirPath"] + "/BSE_Next_50_index.xls");
                }
                Client.DownloadFile(url, common_setting["MoveDirPath"] + "/BSE_Next_50_index.xls");

                Console.WriteLine("=========> Downloading Complete");

                custom_error = Helper.ProcessBseSensexNext50(common_setting["MoveDirPath"]);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                commonHelper.WriteLog("BSE Next 50 Index exception :" + ex.Message, "E");
                custom_error.index_name = "BSE Next 50";
                custom_error.status = "fail";
                custom_error.is_success = false;
                custom_error.error_msg = ex.Message;
            }
            return custom_error;
        }
    }

}
