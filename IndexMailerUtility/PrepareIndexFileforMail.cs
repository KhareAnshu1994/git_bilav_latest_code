using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Npgsql;
using System.Data;
using System.IO;

namespace IndexMailerUtility
{
    public class PrepareIndexFileforMail
    {

        string strCon = ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString;
        string IndexvalLogDir = ConfigurationManager.AppSettings["IndexValLogFile"].ToString();
        string index_val_backup_location = ConfigurationManager.AppSettings["file_move_to_path"].ToString();
        string[] ignore_indexes = ConfigurationManager.AppSettings.Get("remove_indexes").Split(',');
        public void PrepareTextFile()
        {
            try
            {
                DateTime val_date = DateTime.Now;

                TimeSpan now = DateTime.Now.TimeOfDay;

                TimeSpan start = new TimeSpan(15, 0, 0);
                //if (now < start)
                //{
                //    val_date = DateTime.Now.AddDays(-1);
                //}
                Console.WriteLine("val_date :" + val_date);
                using (NpgsqlConnection conn = new NpgsqlConnection(strCon))
                {
                    conn.Open();
                    Console.WriteLine("Connection opened succesfully!");
                    DataTable dt = new DataTable();
                    if (now < start)
                    {
                        using (NpgsqlCommand cmd_check = new NpgsqlCommand("SELECT * FROM (SELECT index_date FROM TBL_INDEX_VAL order by index_date desc ) as index", conn))
                        {
                            using (NpgsqlDataAdapter adp = new NpgsqlDataAdapter(cmd_check))
                            {
                                adp.Fill(dt);
                                Console.WriteLine("index date :" + dt.Rows[0]["index_date"].ToString());
                                commonHelper.WriteLog("index date :" + dt.Rows[0]["index_date"].ToString());
                                if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
                                {
                                    val_date = DateTime.Now.AddDays(-3);
                                }
                                else
                                {
                                    val_date = Convert.ToDateTime(dt.Rows[0]["index_date"]);
                                }
                                dt = new DataTable();
                            }
                        }
                    }

                    using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM TBL_INDEX_VAL WHERE TO_CHAR(index_date,'YYYY-MM-DD') = '" + val_date.ToString("yyyy-MM-dd") + "'", conn))
                    {
                        using (NpgsqlDataAdapter adp = new NpgsqlDataAdapter(cmd))
                        {
                            adp.Fill(dt);
                        }
                        if (dt.Rows.Count > 0)
                        {
                            Console.WriteLine("dt count :" + dt.Rows.Count);
                            string file_headers_index_val = "asof" + "\t" + "instrument" + "\t" + "closing_val";
                            commonHelper.create_index_val_file(file_headers_index_val);
                            foreach (DataRow dr in dt.Rows)
                            {
                                DateTime index_date = Convert.ToDateTime(dr["Index_Date"].ToString());

                                string index_name = string.IsNullOrWhiteSpace(dr["Index_Name"].ToString()) ? "" : dr["Index_Name"].ToString();

                                decimal index_value = string.IsNullOrWhiteSpace(dr["Current_Value"].ToString()) ? 0 : Convert.ToDecimal(dr["Current_Value"]);

                                string IndexvalText = "" + index_date.ToString("dd/MM/yyyy").Replace("-", "/") + "\t" + index_name + "\t" + index_value;
                                Console.WriteLine(IndexvalText);
                                if (ignore_indexes.Contains(index_name.ToUpper()))
                                {
                                    continue;
                                }
                                commonHelper.create_index_val_file(IndexvalText);
                            }
                            string FILES_PATH = IndexvalLogDir + "\\IndexVal_" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
                            if (File.Exists(FILES_PATH))
                            {

                                if (EmailHelper.SendEmail(FILES_PATH))
                                {
                                    Console.WriteLine("Email sent successfully !");
                                    try
                                    {

                                        File.Copy(FILES_PATH, index_val_backup_location + Path.GetFileName(FILES_PATH), true);
                                        if (File.Exists(FILES_PATH))
                                        {
                                            File.Delete(FILES_PATH);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        commonHelper.WriteLog("error while moving the file :" + ex.Message);
                                        Console.WriteLine("error :" + ex.Message);
                                    }

                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("something went wrong :" + ex.Message);
                commonHelper.WriteLog("something went wrong :" + ex.Message);
            }
        }

    }
}
