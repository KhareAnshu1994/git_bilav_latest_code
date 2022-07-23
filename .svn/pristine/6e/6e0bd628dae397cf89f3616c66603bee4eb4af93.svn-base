using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Index_Download.classes
{
    public class asiaindex
    {
        public DateTime date { get; set; }

        public string currentValue { get; set; }

        public static CustomError getBseSensexIndex()
        {
            CustomError custom_error = new CustomError();
            try
            {

                Console.WriteLine("=============ASIA BseSensexIndex started=============");

                 String url = "https://www.spglobal.com/spdji/en/idsexport/file.xls?hostIdentifier=48190c8c-42c4-46af-8d1a-0cd5db894797&redesignExport=true&languageId=1&selectedModule=PerformanceGraphView&selectedSubModule=Graph&yearFlag=oneYearFlag&indexId=1852744";
               

                WebClient Client = new WebClient();
                NameValueCollection common_setting = (NameValueCollection)ConfigurationManager.GetSection("customAppSettingsGroup/CommonAppSettings");

                Console.WriteLine("=======> Excel File Downloading...");
                if (File.Exists(common_setting["MoveDirPath"] + "/BSE_index.xls"))
                {
                    File.Delete(common_setting["MoveDirPath"] + "/BSE_index.xls");
                }
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Client.DownloadFile(url, common_setting["MoveDirPath"] + "/BSE_index.xls");

                Console.WriteLine("=======> Downloading Complete");
                // below line of code uncommented for combined code
                custom_error = ProcessBseSensexIndex(common_setting["MoveDirPath"]);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                commonHelper.WriteLog("ASIA Bse Sensex Index Exception", "E");

                custom_error.index_name = "ASIA Bse Sensex Index";
                custom_error.error_msg = ex.Message;
                custom_error.status = "fail";
                custom_error.is_success = false;
            }
            return custom_error;
        }

        public static CustomError ProcessBseSensexIndex(string dir_path)
        {
            CustomError custom_error = new CustomError();
            try
            {
                string con = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dir_path + "/BSE_index.xls;" + @"Extended Properties='Excel 8.0;HDR=Yes;'";
                using (OleDbConnection connection = new OleDbConnection(con))
                {
                    connection.Open();
                    System.Data.DataTable dbSchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, null);

                    var firstSheetName = dbSchema.Rows[0]["TABLE_NAME"];

                    OleDbCommand command = new OleDbCommand("select * from [" + firstSheetName + "]", connection);

                    List<asiaindex> BSEIndex = new List<asiaindex>();

                    using (OleDbDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (dr[0].ToString().Trim() != string.Empty)
                            {

                                //var date = DateTime.ParseExact(dr[0].ToString().Trim(), "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);

                                asiaindex index = new asiaindex();
                                index.date = commonHelper.GetFormatedDate(dr[0].ToString().Trim());
                                index.currentValue = dr[1].ToString().Trim();
                                BSEIndex.Add(index);

                            }

                        }
                        if (BSEIndex.Count() > 0)
                        {
                            commonHelper helper = new commonHelper();
                            helper.currentName = "Asia Index BSE Sensex";
                            helper.date = BSEIndex.LastOrDefault().date;
                            helper.currentValue = BSEIndex.LastOrDefault().currentValue;
                            //custom_error = commonHelper.saveDataToDB(helper);
                            commonHelper.saveData(helper);
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
        public static CustomError getHealthCareIndex()
        {
            CustomError custom_error = new CustomError();
            try
            {

                Console.WriteLine("============ASIA Health Care stared==========");

                String url = "https://www.spglobal.com/spdji/en/idsexport/file.xls?hostIdentifier=48190c8c-42c4-46af-8d1a-0cd5db894797&redesignExport=true&languageId=1&selectedModule=PerformanceGraphView&selectedSubModule=Graph&yearFlag=oneYearFlag&indexId=92028522";

                WebClient Client = new WebClient();
                NameValueCollection common_setting = (NameValueCollection)ConfigurationManager.GetSection("customAppSettingsGroup/CommonAppSettings");

                Console.WriteLine("=======> Excel File Downloading...");
                if (File.Exists(common_setting["MoveDirPath"] + "/HealthCare_index.xls"))
                {
                    File.Delete(common_setting["MoveDirPath"] + "/HealthCare_index.xls");
                }
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Client.DownloadFile(url, common_setting["MoveDirPath"] + "/HealthCare_index.xls");

                //below line uncomment for combined code
                custom_error = ProcessHealthCareIndex(common_setting["MoveDirPath"]);

                Console.WriteLine("=======> Downloading Complete");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                commonHelper.WriteLog("error in function getHealthCareIndex() :" + ex.Message, "E");

                custom_error.index_name = "Asia Index Health Care";
                custom_error.error_msg = ex.Message;
                custom_error.status = "fail";
                custom_error.is_success = false;
            }
            return custom_error;
        }

        public static CustomError ProcessHealthCareIndex(string dir_path)
        {
            CustomError custom_error = new CustomError();
            try
            {
                string con = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dir_path + "/HealthCare_index.xls;" + @"Extended Properties='Excel 8.0;HDR=Yes;'";
                using (OleDbConnection connection = new OleDbConnection(con))
                {
                    connection.Open();
                    System.Data.DataTable dbSchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, null);

                    var firstSheetName = dbSchema.Rows[0]["TABLE_NAME"];

                    OleDbCommand command = new OleDbCommand("select * from [" + firstSheetName + "]", connection);


                    List<asiaindex> HealthCareIndex = new List<asiaindex>();

                    //DataTable dt = new DataTable();
                    //OleDbDataAdapter objAdapter1 = new OleDbDataAdapter();
                    //objAdapter1.SelectCommand = command;
                    //DataSet objDataset1 = new DataSet();
                    //objAdapter1.Fill(objDataset1, firstSheetName.ToString());
                    //dt = objDataset1.Tables[firstSheetName.ToString()];

                    using (OleDbDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string valnm = dr[0].ToString().Trim();
                            if (dr[0].ToString().Trim() != string.Empty)
                            {

                                // var date = DateTime.ParseExact(dr[0].ToString().Trim(), "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);

                                asiaindex index = new asiaindex();
                                index.date = commonHelper.GetFormatedDate(dr[0].ToString().Trim());
                                index.currentValue = dr[1].ToString().Trim();

                                HealthCareIndex.Add(index);

                            }

                        }
                        if (HealthCareIndex.Count() > 0)
                        {
                            commonHelper helper = new commonHelper();
                            helper.currentName = "Asia Index Health Care";
                            helper.date = HealthCareIndex.LastOrDefault().date;
                            helper.currentValue = HealthCareIndex.LastOrDefault().currentValue;
                            //custom_error = commonHelper.saveDataToDB(helper);
                            commonHelper.saveData(helper);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                commonHelper.WriteLog("error in function ProcessHealthCareIndex() :" + ex.Message, "E");
                Console.WriteLine("exception occured while reading excel : " + ex.Message);
                custom_error.index_name = "Asia Index Health Care";
                custom_error.error_msg = ex.Message;
                custom_error.status = "fail";
                custom_error.is_success = false;
            }
            return custom_error;
        }
    }
}

