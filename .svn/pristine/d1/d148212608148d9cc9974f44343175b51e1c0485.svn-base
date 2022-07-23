using MimeKit;
using Newtonsoft.Json;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Index_Download.classes
{
    public class CustomError
    {
        public string index_name { get; set; }
        public string status { get; set; }
        public string error_msg { get; set; }
        public bool is_success { get; set; }
    }
    public class commonHelper
    {
        public static OracleConnection conn;
        public static string strCon = ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString;
        public static NameValueCollection error_setting = (NameValueCollection)ConfigurationManager.GetSection("customAppSettingsGroup/CommonAppSettings");
        public DateTime date { get; set; }
        public string currentName { get; set; }
        public string currentValue { get; set; }

        public static bool saveData(commonHelper objModel)
        {
            try
            {
                var current_date = DateTime.Now.ToString("ddMMyyyy");
                current_date = current_date.Replace("/", "_");


                //string appDirectory = Directory.GetCurrentDirectory();
                string appDirectory = error_setting["store_json_path"].ToString();

                string dir_path = appDirectory + "/Result_File";

                string file_path = appDirectory + "/Result_File/" + current_date + "_INDEX.json";

                if (!Directory.Exists(dir_path))
                {
                    Directory.CreateDirectory(dir_path);
                }
                if (!File.Exists(file_path))
                {
                    var logFile = File.Create(file_path);
                    logFile.Close();
                }
                var jsonData = File.ReadAllText(file_path);

                var logList = JsonConvert.DeserializeObject<List<commonHelper>>(jsonData) ?? new List<commonHelper>();

                logList.Add(objModel);

                jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(logList, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(file_path, jsonData);
                Console.WriteLine("index added in json file !");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Saving process Exception msg : " + ex.Message);
                commonHelper.WriteLog("Saved data Exception : " + ex.Source, "E");
                return false;
            }
        }
        public DataTable GetAudits(string IndexName)
        {
            DataTable dt = new DataTable();
            conn = new OracleConnection(strCon);
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                using (OracleCommand cmd = new OracleCommand("SELECT * FROM INDEX_VAL WHERE IndexName='" + IndexName + "' AND TO_CHAR(indexdate,'YYYY-MM-DD') >= (SELECT to_CHAR(sysdate, 'YYYY-MM-DD') FROM DUAL)", conn))
                {
                    using (OracleDataAdapter adp = new OracleDataAdapter(cmd))
                    {
                        adp.Fill(dt);
                    }
                }
                Console.WriteLine(": INDEX_VAL read success.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(":Failed. INDEX_VAL read Failed. \n" + ex.Message);
            }

            return dt;
        }

        public static DateTime GetFormatedDate(string ParaDate)
        {
            DateTime indexDate = Convert.ToDateTime(ParaDate);
            indexDate = Convert.ToDateTime(indexDate.ToString("yyyy/MM/dd"), CultureInfo.InvariantCulture);
            return indexDate;
        }


        public static bool saveDataToDB()
        {

            List<commonHelper> objListModel = new List<commonHelper>();
            commonHelper objIndexVal = new commonHelper();
            CustomError custom_error = new CustomError();
            custom_error.status = "success";
            custom_error.is_success = true;
            string downloadDirectory = error_setting["store_json_path"].ToString();

            string file_path = downloadDirectory + "/Result_File/" + DateTime.Now.ToString("ddMMyyyy") + "_INDEX.json";
            Console.WriteLine(file_path);
            if (File.Exists(file_path))
            {
                var jsonData = File.ReadAllText(file_path);
                Console.WriteLine("File read success");
                objListModel = JsonConvert.DeserializeObject<List<commonHelper>>(jsonData) ?? new List<commonHelper>();
                Console.WriteLine(objListModel.Count);
                if (objListModel.Count > 0)
                {
                    using (NpgsqlConnection conn = new NpgsqlConnection(strCon))
                    {
                        conn.Open();
                        foreach (commonHelper objModel in objListModel)
                        {
                            DataTable dt = new DataTable();
                            using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  tbl_index_val where index_name='" + objModel.currentName + "' AND date(index_date) = '" + objModel.date.ToString("yyyy-MM-dd") + "'", conn))
                            {
                                using (NpgsqlDataAdapter adp = new NpgsqlDataAdapter(cmd))
                                {
                                    adp.Fill(dt);

                                }
                            }
                            if (dt.Rows.Count < 1)
                            {
                                Console.WriteLine("Connected to Database server.");
                                string strQueryInsert = "insert into tbl_index_val(index_name,current_value,index_date) values('" + objModel.currentName + "','" + objModel.currentValue + "','" + objModel.date + "')";
                                using (NpgsqlCommand oraCommand = new NpgsqlCommand(strQueryInsert, conn))
                                {
                                    try
                                    {
                                        if (conn.State != ConnectionState.Open)
                                        {
                                            conn.Open();
                                            Console.WriteLine("Reconnected to Database server.");
                                        }
                                        oraCommand.ExecuteNonQuery();
                                        commonHelper.WriteLog(strQueryInsert, "S");
                                        Console.WriteLine(objModel.currentName + " :  Record inserted in database");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(":Failed, Error inserting records in  :" + ex.Message);
                                        commonHelper.WriteLog(": Failed, Error inserting records in  :" + ex.Message, "E");
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine(":Records already present. No new Inserts  :");
                                commonHelper.WriteLog("Index already preasent :" + objModel.currentName + "| index date | " + objModel.date + " | index_value :" + objModel.currentValue, "E");
                            }
                        }
                    }
                }
                File.Delete(file_path);
                return true;
            }
            else
            {
                Console.WriteLine("File not found !");

            }
            return false;
        }
        public static void WriteLog(string message, string ErrorType)
        {
            string ErrorLogDir = error_setting["ErrorLogFile"].ToString();
            if (!Directory.Exists(ErrorLogDir))
                Directory.CreateDirectory(ErrorLogDir);

            if (ErrorType == "S")// S means SUCCESS
            {
                ErrorLogDir += "\\index_success_log" + ".txt";
            }
            else
            {
                ErrorLogDir += "\\index_errorLog_" + ".txt";
            }
            using (StreamWriter sw = new StreamWriter(ErrorLogDir, true))
            {
                sw.WriteLine(DateTime.Now.ToString("dd-MMM-yy HH:mm:ss") + "\t" + message);
            }
        }
        public static bool saveErrorLogData(commonHelper objModel)
        {
            try
            {

                var current_date = DateTime.UtcNow.AddMinutes(330).ToShortDateString();
                current_date = current_date.Replace("/", "_");
                string appDirectory = Directory.GetCurrentDirectory();

                string dir_path = appDirectory + "/Error_Log_File";

                string file_path = appDirectory + "/Error_Log_File/" + current_date + "_error_log.json";


                if (!Directory.Exists(dir_path))
                {
                    Directory.CreateDirectory(dir_path);
                }
                if (!File.Exists(file_path))
                {
                    var logFile = File.Create(file_path);
                    logFile.Close();
                }
                var jsonData = File.ReadAllText(file_path);

                var logList = JsonConvert.DeserializeObject<List<commonHelper>>(jsonData) ?? new List<commonHelper>();

                logList.Add(objModel);

                jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(logList, Newtonsoft.Json.Formatting.Indented);

                System.IO.File.WriteAllText(file_path, jsonData);


                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public void ResultFileMove()
        {
            NameValueCollection comm_setting = (NameValueCollection)ConfigurationManager.GetSection("customAppSettingsGroup/CommonAppSettings");

            string MoveDirPath = comm_setting["MoveDirPath"].ToString();

            DateTime StartTime = DateTime.Now;

            string appDirectory = Directory.GetCurrentDirectory();

            string dir_path = appDirectory + "\\Result_File";

            string Resultfile_path = appDirectory + "\\Result_File";

            DirectoryInfo folder = new DirectoryInfo(Resultfile_path);

            try
            {
                foreach (FileInfo file in folder.GetFiles())
                {
                    if (file.Extension.Equals(".json"))
                    {
                        try
                        {
                            string fileName = file.Name;
                            string sourcePath = @"" + Resultfile_path;
                            string targetPath = @"" + MoveDirPath;

                            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                            string destFile = System.IO.Path.Combine(targetPath, fileName);

                            if (!System.IO.Directory.Exists(targetPath))
                            {
                                System.IO.Directory.CreateDirectory(targetPath);
                            }
                            System.IO.File.Copy(sourceFile, destFile, true);
                            System.IO.File.Delete(sourceFile);
                            Console.WriteLine("Result.json: file Move Successfully:" + destFile, "IndexDownload Utility");
                        }
                        catch (SecurityException) { throw new Exception("Security Exception - no permission to write to destination folder"); }
                        catch (DirectoryNotFoundException) { throw new Exception("The destination directory not found"); }
                        catch (IOException) { throw new Exception("IO Exceoption perhaps the destimation file already exists"); }
                    }
                    else
                    {
                        Console.WriteLine("Result.json file moved successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while moving json file : " + ex.Message);
                commonHelper.WriteLog("Error while moving json file : " + ex.Source, "E");
            }

        }
    }
}
