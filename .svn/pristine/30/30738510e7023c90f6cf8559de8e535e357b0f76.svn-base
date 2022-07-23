using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using Renci.SshNet;
using System.Globalization;
using System.Threading;
using Npgsql;

namespace IndexValReaderUtility
{
    public class FileAccess
    {
        private NpgsqlConnection conn;
        string strCon = ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString;

        List<commonHelper> objListModel = new List<commonHelper>();
        commonHelper objIndexVal = new commonHelper();
        public void FileProcessing()
        {
            string downloadDirectory = ConfigurationManager.AppSettings["DestinationDirectory"].ToString();
            try
            {
                string file_path = downloadDirectory + "/" + DateTime.Now.ToString("ddMMyyyy") + "_INDEX.json";
                Console.WriteLine(file_path);
                if (File.Exists(file_path))
                {
                    var jsonData = File.ReadAllText(file_path);
                    Console.WriteLine("File read success");
                    objListModel = JsonConvert.DeserializeObject<List<commonHelper>>(jsonData) ?? new List<commonHelper>();
                    if (objListModel.Count > 0)
                    {
                        conn = new NpgsqlConnection(strCon);
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
                                        commonHelper.WriteLog(strQueryInsert);
                                        Console.WriteLine(objModel.currentName + " :  Record inserted in database");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(":Failed, Error inserting records in  :" + ex.Message);
                                        commonHelper.WriteLog(": Failed, Error inserting records in  :" + ex.Message);
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine(":Records already present. No new Inserts  :");
                                commonHelper.WriteLog("Index already preasent :" + objModel.currentName + "| index date | " + objModel.date + " | index_value :" + objModel.currentValue);
                            }
                        }
                    }
                    File.Delete(file_path);
                }
                else
                {
                    Console.WriteLine("File not found !");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Saving process Exception msg : " + ex.Message);
                commonHelper.WriteLog("Saved data Exception : " + ex.Source);
                objListModel = null;
            }
        }
    }
}
