
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilavCrisilEmailUtility
{
    public class crisilVal
    {
        public decimal hybridAgressivIndex { get; set; }
        public decimal TbillIndex { get; set; }
    }
    class FileReader
    {
        string _FolderNameSuffix = ConfigurationManager.AppSettings["FolderNameSuffix"].ToString();
        string _FolderName = ConfigurationManager.AppSettings["FolderName"].ToString();

        List<string> downloadedfiles = new List<string>();
        List<string> downloadedfiles_xlsx = new List<string>();
        string xlsxfileName = null;
        string xlsfileName = null;

        string FinalDownloadDir = ConfigurationManager.AppSettings["MoveDirectory"].ToString();
        string BilavDownloadDir = ConfigurationManager.AppSettings["BilavDownloadDirectory"].ToString();
        public void FileReadingStart()
        {
            string[] CheckDownloadXLSFile = System.IO.Directory.GetFiles(FinalDownloadDir, "*.xls");
            for (int m = 0; m < CheckDownloadXLSFile.Length; m++)
            {
                xlsfileName = CheckDownloadXLSFile[m].ToString();
                string DownloadFile = Path.GetFileName(xlsxfileName);
            }
            DirectoryInfo dirInfoxlsx = new DirectoryInfo(FinalDownloadDir);
            foreach (FileInfo file in dirInfoxlsx.GetFiles())
            {
                if (!downloadedfiles.Contains(file.Name) && file.Name.ToLower().Contains(".xls"))
                    downloadedfiles.Add(file.Name);
            }
            if (downloadedfiles.Count > 0)
            {
                WriteLog("Downloaded File Count : " + downloadedfiles.Count);
            }
            if (xlsfileName != null)
            {
                if (downloadedfiles.Count > 0)
                {
                    ReadXLSXFile();
                }
                else
                {
                    Console.WriteLine("After downloaded no file found .");
                }
            }
            else
            {
                Console.WriteLine("After extracted file does not match");
            }
        }
        public static void WriteLog(string message)
        {
            string ErrorLogDir = ConfigurationManager.AppSettings["ErrorLogFile"].ToString();
            if (!Directory.Exists(ErrorLogDir))
                Directory.CreateDirectory(ErrorLogDir);

            ErrorLogDir += "\\Crisil_Index_Log_" + ".txt";

            using (StreamWriter sw = new StreamWriter(ErrorLogDir, true))
            {
                sw.WriteLine(DateTime.Now.ToString("dd-MMM-yy HH:mm:ss") + "\t" + message);
            }
        }
        public void ReadXLSXFile()
        {
            try
            {

                FileInfo[] DownloadedFiles = new DirectoryInfo(FinalDownloadDir).GetFiles();
                for (int i = 0; i < DownloadedFiles.Length; i++)
                {
                    crisilVal CrslObj = ReadXLSX(DownloadedFiles[i].FullName, DownloadedFiles[i].Name, Path.GetExtension(DownloadedFiles[i].Name), true);
                    if (CrslObj != null)
                    {
                        PushToDb(CrslObj);

                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog("failed. while Reading File: " + ex.Message);
            }
            WriteLog("==========Read XLSX File Function End");
        }
        public crisilVal ReadXLSX(string filePath, string filename, string Extension, bool isHDR)
        {
            DataTable ExcelToTable = new DataTable();
            crisilVal crislval = new crisilVal();
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
                for (int k = 1; k < dtExcelSchema.Rows.Count; k++)
                {
                    string SheetName = dtExcelSchema.Rows[k]["TABLE_NAME"].ToString();
                    OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [" + SheetName + "]", connExcel);
                    OleDbDataAdapter objAdapter1 = new OleDbDataAdapter();
                    objAdapter1.SelectCommand = objCmdSelect;
                    cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                    oda.SelectCommand = cmdExcel;
                    oda.Fill(ExcelToTable);
                }
                connExcel.Close();
                for (int k = ExcelToTable.Rows.Count - 1; k >= 1; k--)
                {
                    DataRow drow = ExcelToTable.Rows[k];
                    object ID = drow[0];
                    if (ID != null && !String.IsNullOrEmpty(ID.ToString().Trim()))
                    {
                        if (Convert.ToDateTime(drow[0]).ToString("MM/dd/yyyy") == DateTime.Now.ToString("MM/dd/yyyy"))
                        {
                            crislval.hybridAgressivIndex = Convert.ToDecimal(drow[1]);
                            crislval.TbillIndex = Convert.ToDecimal(drow[2]);
                            WriteLog(" hybridAgressivIndex : " + crislval.hybridAgressivIndex + " TbillIndex : " + crislval.TbillIndex);
                        }
                        break;
                    }
                }
                Console.WriteLine(filename + " " + " read Success");
                Console.WriteLine("File read Success");
            }
            catch (Exception ex)
            {
                WriteLog(filePath + ": Failed, File read. \n" + ex.Message);
            }
            return crislval;
        }

        public void PushToDb(crisilVal crslval)
        {
            string Indexname = string.Empty;
            decimal CurentValue = 0;
            string strCon = ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString;
            try
            {

                for (int K1 = 1; K1 <= 2; K1++)
                {
                    if (K1 == 1)
                    {
                        Indexname = "CRISIL Hybrid 25+75 - Aggressive Index";
                        CurentValue = crslval.hybridAgressivIndex;

                    }
                    else
                    {
                        Indexname = "CRISIL 1 Year T-Bill Index";
                        CurentValue = crslval.TbillIndex;

                    }

                    DataTable dtExisting = getDataTable(Indexname);

                    bool flg = true;
                    foreach (DataRow dr in dtExisting.Rows)
                    {
                        Console.WriteLine("indexdate ===" + Convert.ToDateTime(dr["IndexDate"]) + " ===curent name :=== " + dr["IndexName"].ToString());
                        DateTime dtt = Convert.ToDateTime(dr["IndexDate"]);
                        if (dtt.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy"))
                        {
                            flg = false;
                        }
                    }

                    if (flg)
                    {

                        OracleConnection conn = new OracleConnection(strCon);
                        conn.Open();
                        Console.WriteLine("Connected to Database server.");
                        using (OracleCommand oraCommand = new OracleCommand("INSERT INTO INDEX_VAL(IndexName,CurrentValue,IndexDate) VALUES('" + Indexname + "','" + CurentValue + "',(select Sysdate from Dual))", conn))
                        {
                            try
                            {
                                if (conn.State != ConnectionState.Open)
                                {
                                    conn.Open();
                                    Console.WriteLine("Reconnected to Database server.");
                                }
                                oraCommand.ExecuteNonQuery();
                                WriteLog(": Record inserted in database FOR-- " + Indexname);
                                Console.WriteLine(" :  Record inserted in database");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(":Failed, Error inserting records in  :" + ex.Message);
                                WriteLog(": Failed, Error inserting records in  :" + ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(": Failed, Error opening connection to database. \n" + ex.Message);
            }
        }

        private DataTable getDataTable(string IndexName)
        {
            DataTable dt = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString;
            try
            {
                using (var connection = new OracleConnection(connString))
                {

                    OracleCommand selectcmd = new OracleCommand("SELECT * FROM INDEX_VAL WHERE IndexName='" + IndexName + "'", connection);
                    OracleDataAdapter orclda = new OracleDataAdapter(selectcmd);
                    try
                    {
                        connection.Open();
                        orclda.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        WriteLog("Error Fill data :" + ex.ToString());
                    }
                    finally { connection.Close(); }
                }
            }
            catch (Exception ex)
            {
                WriteLog("Error getDataTable :" + ex.ToString());
            }
            return dt;
        }
    }
}
