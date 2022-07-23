using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;
using System.Globalization;
using Npgsql;
using System.Data;

namespace IndexValReaderUtility
{
    public class commonHelper
    {
        private static NpgsqlConnection conn;
        static string strCon = ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString;
        public DateTime date { get; set; }
        public string currentName { get; set; }
        public string currentValue { get; set; }
        public static void WriteLog(string message)
        {
            string ErrorLogDir = ConfigurationManager.AppSettings["ErrorLogFile"].ToString();
            if (!Directory.Exists(ErrorLogDir))
                Directory.CreateDirectory(ErrorLogDir);

            ErrorLogDir += "\\IndexVal_UtilityErrorLog_" + ".txt";

            using (StreamWriter sw = new StreamWriter(ErrorLogDir, true))
            {
                sw.WriteLine(DateTime.Now.ToString("dd-MMM-yy HH:mm:ss") + "\t" + message);
            }
        }
        public static void create_index_val_file(string message)
        {
            try
            {
                string IndexvalLogDir = ConfigurationManager.AppSettings["IndexValLogFile"].ToString();
                if (!Directory.Exists(IndexvalLogDir))
                    Directory.CreateDirectory(IndexvalLogDir);
                IndexvalLogDir += "\\IndexVal_" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
                using (StreamWriter sw = new StreamWriter(IndexvalLogDir, true))
                {
                    sw.WriteLine(message);
                    sw.Dispose();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in mail sending");
                commonHelper.WriteLog(ex.Message);
            }

        }
        public static bool saveLogs(string objString)
        {
            try
            {
                Console.WriteLine("Saving data");

                var current_date = DateTime.UtcNow.AddMinutes(330).ToShortDateString();
                current_date = current_date.Replace("/", "_");
                string appDirectory = Directory.GetCurrentDirectory();

                string dir_path = appDirectory + "/Log_File";

                string file_path = appDirectory + "/Log_File/" + current_date + "_log.json";

                if (!Directory.Exists(dir_path))
                {
                    Directory.CreateDirectory(dir_path);
                }

                StreamWriter log;

                if (!File.Exists(file_path))
                {
                    log = new StreamWriter(file_path);
                }
                else
                {
                    log = File.AppendText(file_path);
                }
                var finalString = DateTime.UtcNow.AddMinutes(330) + " - " + objString;
                log.WriteLine(finalString);
                log.WriteLine();
                log.Close();
                // Console.WriteLine("Saved data successfully");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Saving process exception");

                commonHelper error = new commonHelper();
                error.currentName = "Saving data common helper";
                error.currentValue = ex.Message;
                error.date = DateTime.UtcNow.AddMinutes(330);
                commonHelper.saveErrorLogData(error);

                return false;
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
        public static DataTable GetAudits(string IndexName)
        {
            DataTable dt = new DataTable();
            conn = new NpgsqlConnection(strCon);
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM INDEX_VAL WHERE IndexName='" + IndexName + "' AND TO_CHAR(indexdate,'YYYY-MM-DD') >= (SELECT to_CHAR(sysdate, 'YYYY-MM-DD') FROM DUAL)", conn))
                {
                    using (NpgsqlDataAdapter adp = new NpgsqlDataAdapter(cmd))
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
        public static bool saveDataToDB(commonHelper objModel)
        {

            DataTable dt = commonHelper.GetAudits(objModel.currentName);
            Console.WriteLine("DT COUNT GHANSHYAM : " + dt.Rows.Count);
            if (dt.Rows.Count < 1)
            {

                try
                {
                    conn = new NpgsqlConnection(strCon);
                    conn.Open();
                    Console.WriteLine("Connected to Database server.");
                    string strInsertQuery = "INSERT INTO INDEX_VAL(IndexName,CurrentValue,IndexDate) VALUES('" + objModel.currentName + "','" + objModel.currentValue + "','" + Helper.ToOracleDateFormat(objModel.date.ToString()) + "')";
                    using (NpgsqlCommand oraCommand = new NpgsqlCommand(strInsertQuery, conn))
                    {
                        try
                        {
                            if (conn.State != ConnectionState.Open)
                            {
                                conn.Open();
                                Console.WriteLine("Reconnected to Database server.");
                            }
                            oraCommand.ExecuteNonQuery();
                            commonHelper.WriteLog(strInsertQuery);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(":Failed, Error inserting records in  :" + ex.Message);
                            commonHelper.WriteLog(": Failed, Error inserting records in  :" + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    commonHelper.WriteLog(": Failed, Error opening connection to database. \n" + ex.Message);
                }
            }
            return true;
        }

        public static bool saveData(commonHelper objModel)
        {
            try
            {
                string downloadDirectory = ConfigurationManager.AppSettings["DestinationDirectory"].ToString();
                var current_date = DateTime.Now.ToString("dd-MMM-yyyy").Replace("-", "_");
                string file_path = downloadDirectory + "/" + current_date + "_result.json";
                if (File.Exists(file_path))
                {
                    var jsonData = File.ReadAllText(file_path);
                    var logList = JsonConvert.DeserializeObject<List<commonHelper>>(jsonData) ?? new List<commonHelper>();
                    logList.Add(objModel);
                    jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(logList, Newtonsoft.Json.Formatting.Indented);
                    System.IO.File.WriteAllText(file_path, jsonData);
                    Console.WriteLine("Json file updated for : " + objModel.currentName);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Saving process Exception : " + ex.Source);
                Console.WriteLine("Saving process Exception msg : " + ex.Message);
                commonHelper.WriteLog("Saved data Exception : " + ex.Source);
                return false;
            }
        }
        public static DateTime GetFormatedDate(string ParaDate)
        {
            DateTime indexDate = Convert.ToDateTime(ParaDate);
            indexDate = Convert.ToDateTime(indexDate.ToString("yyyy/MM/dd"), CultureInfo.InvariantCulture);
            return indexDate;
        }
        public static string Day(string day)
        {
            switch (day)
            {
                case "1": return "01";
                case "2": return "02";
                case "3": return "03";
                case "4": return "04";
                case "5": return "05";
                case "6": return "06";
                case "7": return "07";
                case "8": return "08";
                case "9": return "09";
                default: return day;

            }
        }
    }
}
