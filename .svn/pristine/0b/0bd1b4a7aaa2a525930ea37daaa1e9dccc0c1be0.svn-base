using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using Oracle.DataAccess.Client;
using System.Globalization;
using System.IO;
using System.Data.OleDb;
using Npgsql;

namespace BilavIndexCloseValueUtility
{
    class FileReader
    {
        int RowCount = 0;
        string INDEX_NAME = string.Empty;
        DateTime DATE;
        DateTime index_date { get; set; }
        string MKT = string.Empty;
        string SERIES = string.Empty;
        string SYMBOL = string.Empty;
        string ISIN = string.Empty;
        string SECURITY_NAME = string.Empty;
        string BASIC_INDUSTRY = string.Empty;
        double PREV_CL_PR;
        double OPEN_PRICE;
        double HIGH_PRICE;
        double LOW_PRICE;
        double CLOSE_PRICE;
        double NET_TRDVAL;
        double NET_TRDQTY;
        string IND_SEC = string.Empty;
        string CORP_IND = string.Empty;
        double TRADES;
        double HI_52_WK;
        double LO_52_WK;
        double ISSUE_CAP;
        double INVESTIBLE_FACTOR;
        double CAP_FACTOR;
        double ADJ_CLOSE_PRICE;
        double INDEX_MKT_CAP;
        double WEIGHTAGE;
        double CurrentValue;
        private object drow;

        public void FileReadingStart()
        {

            string FinalDownloadDir = ConfigurationManager.AppSettings["MoveDirectory"].ToString();
            string BilavDownloadDir = ConfigurationManager.AppSettings["BilavDownloadDirectory"].ToString();
            string FileNMN = "UTI -" + Day((DateTime.Now.Day).ToString()) + Day(DateTime.Now.Month.ToString()) + DateTime.Now.Year.ToString().Substring(2, 2);

            string str_file_path = string.Empty;

            FileInfo[] DownloadedFiles = new DirectoryInfo(BilavDownloadDir).GetFiles();

            DataTable dt2 = new DataTable();

            foreach (FileInfo FI in DownloadedFiles)
            {

                if (FileNMN == Path.GetFileNameWithoutExtension(FI.Name))
                {
                    str_file_path = FI.FullName;
                    dt2 = Helper.GetDataTableFromExcel(FI.FullName, true);
                    // dt2 = ReadXLSX(FI.FullName, FI.Name, System.IO.Path.GetExtension(FI.Name), true);
                }
                Helper.LogError("dt2 count :" + dt2.Rows.Count);
            }

            if (dt2.Rows.Count > 0)
            {



                DataTable dt = new DataTable();
                dt = dt2.Clone();
                //DateTime a = Convert.ToDateTime("1/1/0001");
                DateTime index_date = DateTime.Now;
                DateTime a = DateTime.Now;
                RowCount = 0;
                Helper.LogError("==========InsertDB start");
                //dtReadXLSX.Rows.RemoveAt(dtReadXLSX.Rows.Count - 1);
                string _FileName = ConfigurationManager.AppSettings["FileName"].ToString();
                string[] FileName = _FileName.Split(',');
                try
                {

                    for (int i = 0; i < FileName.Length; i++)
                    {
                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            int index = FileName[i].IndexOf("$");
                            if (index > -1)
                            {
                                FileName[i] = FileName[i].Substring(0, index) + "&" + FileName[i].Substring(index + 1);
                                //char[] chars =  fileNameSuffix[j].ToCharArray();
                                //chars[index] = '&';
                                //String str = String.valueOf(chars);
                                //fileNameSuffix[j] = String.valueOf(chars);
                            }

                            if (FileName[i].ToString() == dt2.Rows[j][0].ToString())
                            {

                                dt.ImportRow(dt2.Rows[j]);
                                continue;
                            }
                        }
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 18)
                        {
                            Console.WriteLine("OK");
                        }
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {

                            if (j == 1)
                            {
                                if (dt.Rows[i][j].ToString() != null && !String.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()))
                                {
                                    INDEX_NAME = string.IsNullOrEmpty(dt.Rows[i]["Index Name"].ToString()) ? null : dt.Rows[i]["Index Name"].ToString();
                                    index_date = DateTime.Now;

                                    //index_date = string.IsNullOrWhiteSpace(drow["index_date"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("1900-01-01")) : Convert.ToDateTime(drow["index_date"]);
                                    CurrentValue = Convert.ToDouble(string.IsNullOrEmpty(dt.Rows[i]["TR Value"].ToString()) ? "0" : dt.Rows[i]["TR Value"]);
                                    InsertIndexCloseValue_XLSXFiles(INDEX_NAME, CurrentValue, index_date);
                                    SetDefault();

                                }
                            }
                            if (j == 2)
                            {
                                if (dt.Rows[0][1].ToString() != null && !String.IsNullOrEmpty(dt.Rows[0][1].ToString().Trim()))
                                {
                                    INDEX_NAME = string.IsNullOrEmpty("p" + dt.Rows[i]["Index Name"].ToString()) ? null : ("p" + dt.Rows[i]["Index Name"].ToString());

                                    index_date = DateTime.Now;
                                    CurrentValue = Convert.ToDouble(string.IsNullOrEmpty(dt.Rows[i]["Index close"].ToString()) ? "0" : dt.Rows[i]["Index close"]);
                                    InsertIndexCloseValue_XLSXFiles(INDEX_NAME, CurrentValue, index_date);
                                    SetDefault();
                                }

                            }
                        }

                    }
                    Helper.LogError("Current File Saved in database row affected : " + RowCount);
                }
                catch (Exception ex)
                {

                    Console.WriteLine("ReadXLSXFile Failed. when inserting a data" + ex.Message);
                    Helper.LogError("Insert DB failed. when inserting a data" + ex.ToString());
                }
                Helper.LogError("==========InsertDB End");
            }

            if (File.Exists(str_file_path))
            {
                File.Delete(str_file_path);
            }
        }
        public string Day(string day)
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

        public DataTable ReadXLSX(string filePath, string filename, string Extension, bool isHDR)
        {
            DataTable dt = new DataTable();
            try
            {
                string con = @"Provider=Microsoft.Jet.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0 Xml;HDR=YES;\""; //Excel 8.0;HDR=YES;\"";

                string conStr = "";

                switch (Extension.Trim().ToLower())
                {
                    case ".xls": //Excel 97-03
                        conStr = ConfigurationManager.AppSettings["Excel03ConString"];
                        break;
                    case ".xlsx": //Excel 07
                        conStr = ConfigurationManager.AppSettings["Excel07ConString"];
                        break;
                }
                conStr = String.Format(conStr, filePath, isHDR);
                OleDbConnection connExcel = new OleDbConnection(conStr);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();
                cmdExcel.Connection = connExcel;
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                for (int k = 0; k < dtExcelSchema.Rows.Count; k++)
                {

                    string SheetName = dtExcelSchema.Rows[k]["TABLE_NAME"].ToString();
                    OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [" + SheetName + "]", connExcel);
                    OleDbDataAdapter objAdapter1 = new OleDbDataAdapter();
                    objAdapter1.SelectCommand = objCmdSelect;
                    DataSet objDataset1 = new DataSet();
                    objAdapter1.Fill(objDataset1, SheetName);
                    dt = objDataset1.Tables[SheetName];
                }
                connExcel.Close();
                Console.WriteLine(filename + " " + " read Success");
            }

            catch (Exception ex)
            {
                Console.WriteLine(filePath + ": Failed, File read. \n" + ex.Message);
                Helper.LogError(filePath + ": Failed, File read. \n" + ex.Message);
            }
            Console.WriteLine(filename + " : File Read End.");
            return dt;

        }

        // changes in index date (cuurent_date show)
        public void InsertIndexCloseValue_XLSXFiles(string index_name, double current_value, DateTime index_date)
        {
            DataTable dt = new DataTable();
            string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            try
            {

                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  tbl_index_val where index_name='" + index_name + "' AND date(index_date) = '" + index_date.ToString("yyyy-MM-dd") + "'", conn))
                    {
                        using (NpgsqlDataAdapter adp = new NpgsqlDataAdapter(cmd))
                        {
                            adp.Fill(dt);

                        }
                    }
                    if (dt.Rows.Count < 1)
                    {
                        DateTime time = DateTime.Now;              // Use current time
                        using (NpgsqlCommand oraCommand = new NpgsqlCommand("INSERT INTO tbl_index_val" + "(index_name,current_value,index_date) VALUES('" + index_name + "','" + current_value + "','" + index_date + "')", conn))
                        {
                            try
                            {
                                if (conn.State != ConnectionState.Open)
                                {
                                    conn.Open();
                                    Console.WriteLine("Reconnected to Database server.");
                                }
                                Console.WriteLine("Inserted ========================");
                                oraCommand.ExecuteNonQuery();
                                Helper.LogError("Inserted successfully : Index Name - " + INDEX_NAME + " | Upload date - " + DATE + " | Current Value- " + current_value);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(":Failed, Error inserting records in  :" + ex.Message);
                                Helper.LogError(": Failed, Error inserting records in  :" + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Already exists !");
                    }

                }
            }
            catch (Exception ex)
            {
                Helper.LogError("Insert xls Files Data: " + ex.ToString());
            }
        }
        public void SetDefault()
        {
            INDEX_NAME = null;

            SYMBOL = null;
            BASIC_INDUSTRY = null;
            CLOSE_PRICE = 0.00;
            ISSUE_CAP = 0.00;
            INVESTIBLE_FACTOR = 0.00;
            CAP_FACTOR = 0.00;
            INDEX_MKT_CAP = 0.00;
            WEIGHTAGE = 0.00;
            ISIN = null;
        }
    }
}
