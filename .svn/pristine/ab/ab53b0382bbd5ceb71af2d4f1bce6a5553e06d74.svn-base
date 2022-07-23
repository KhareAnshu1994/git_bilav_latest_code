using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilavCrisilIndexUtility
{
    public class crisilVal
    {
        public decimal hybridAgressivIndex { get; set; }
        public decimal TbillIndex { get; set; }
        public decimal Crisil_ShortTerm_Credit_Risk_Index { get; set; }
        public DateTime IndexDate { get; set; }
    }

    public class FileReader
    {
        string FinalDownloadDir = ConfigurationManager.AppSettings["MoveDirectory"].ToString();
        string BilavDownloadDir = ConfigurationManager.AppSettings["BilavDownloadDirectory"].ToString();
        string[] partialNames = ConfigurationManager.AppSettings["FileName"].ToString().Split(',');
        bool success = false;
        public void FileReadingStart()
        {
            try
            {
                DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(BilavDownloadDir);

                foreach (string partial_file_name in partialNames)
                {
                    FileInfo[] DownloadedFiles = hdDirectoryInWhichToSearch.GetFiles("*" + partial_file_name + "*.*");

                    Console.WriteLine("Utility Started !");
                    WriteLog("File count :" + DownloadedFiles.Count());

                    for (int i = 0; i < DownloadedFiles.Length; i++)
                    {
                        crisilVal CrslObj = new crisilVal();

                        if (DownloadedFiles[i].Name.Contains("UTI Index values"))
                        {
                            CrslObj = ReadXLSX(DownloadedFiles[i].FullName, DownloadedFiles[i].Name, Path.GetExtension(DownloadedFiles[i].Name), true);
                        }
                        else if (DownloadedFiles[i].Name.Contains("CRISIL Short Term Credit Risk Index"))
                        {
                            CrslObj = GetDataTableFromExcel(DownloadedFiles[i].FullName, true);
                        }
                        if (CrslObj != null)
                        {
                            success = PushToDb(CrslObj);
                            if (success)
                            {
                                try
                                {
                                    File.Delete(DownloadedFiles[i].FullName);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("File deleting error : " + ex.Message);
                                    WriteLog("File deleting error : " + ex.Message);
                                }
                            }
                        }

                    }
                }
                Console.WriteLine("Utility Completed !");
            }
            catch (Exception ex)
            {
                WriteLog("failed. while Reading File: " + ex.Message);
            }


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

                        crislval.IndexDate = Convert.ToDateTime(drow[0].ToString());
                        if (filename.Contains("UTI Index values"))
                        {
                            crislval.hybridAgressivIndex = string.IsNullOrWhiteSpace(drow[1].ToString()) ? 0 : Convert.ToDecimal(drow[1].ToString());
                            crislval.TbillIndex = string.IsNullOrWhiteSpace(drow[2].ToString()) ? 0 : Convert.ToDecimal(drow[2].ToString());
                        }
                        else if (filename.Contains("CRISIL Short Term Credit Risk Index"))
                        {
                            crislval.Crisil_ShortTerm_Credit_Risk_Index = string.IsNullOrWhiteSpace(drow[1].ToString()) ? 0 : Convert.ToDecimal(drow[1].ToString());
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
        public static crisilVal GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            crisilVal crs_val = new crisilVal();
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? ws.Dimension.End.Row : 1;

                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                        if (cell.Start.Column == 1)
                        {
                            var formats = new[] { "dd-MMM-yy", "d-MMM-yy" };


                            DateTime dt;
                            if (DateTime.TryParseExact(cell.Text, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt) == false)
                            {
                                Console.WriteLine("Can not parse {0}", cell.Text);
                            }
                            else
                            {
                                DateTime val_index_date = dt;
                                crs_val.IndexDate = val_index_date;
                            }
                        }
                        else if (cell.Start.Column == 2)
                        {
                            crs_val.Crisil_ShortTerm_Credit_Risk_Index = Convert.ToDecimal(cell.Text);
                        }
                    }
                }

                return crs_val;
            }
        }
        public bool PushToDb(crisilVal crslval)
        {
            string index_name = string.Empty;
            decimal current_value = 0;
            DateTime INDEX_DATE;

            try
            {
                string strCon = ConfigurationManager.AppSettings["ConnectionString"].ToString();

                for (int K1 = 1; K1 <= 2; K1++)
                {
                    if (crslval.Crisil_ShortTerm_Credit_Risk_Index == 0)
                    {
                        if (K1 == 1)
                        {
                            index_name = "CRISIL Hybrid 25+75 - Aggressive Index";
                            current_value = crslval.hybridAgressivIndex;
                            INDEX_DATE = crslval.IndexDate;

                        }
                        else
                        {
                            index_name = "CRISIL 1 Year T-Bill Index";
                            current_value = crslval.TbillIndex;
                            INDEX_DATE = crslval.IndexDate;
                        }
                    }
                    else
                    {
                        index_name = "CRISIL Short Term Credit Risk Index";
                        current_value = crslval.Crisil_ShortTerm_Credit_Risk_Index;
                        INDEX_DATE = crslval.IndexDate;
                    }
                    DataTable dtExisting = getDataTable(index_name, INDEX_DATE);


                    if (dtExisting.Rows.Count == 0)
                    {
                        using (NpgsqlConnection con = new NpgsqlConnection(strCon))
                        {
                            con.Open();

                            Console.WriteLine("Connected to Database server.");

                            using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mcyprod_sp_insert_index_value(:pindex_name,:pcurrent_value,:pindex_date)", con))
                            {
                                try
                                {
                                    cmd.Connection = con;
                                    cmd.Parameters.AddWithValue("pindex_name", index_name);
                                    cmd.Parameters.AddWithValue("pcurrent_value", current_value);
                                    cmd.Parameters.AddWithValue("pindex_date", INDEX_DATE);

                                    object Res = cmd.ExecuteScalar();
                                    WriteLog("index inserted :" + index_name);
                                    Console.WriteLine(" :  Record inserted in database");
                                }

                                catch (Exception ex)
                                {

                                    Console.WriteLine(":Failed, Error inserting records in  :" + ex.Message);
                                    WriteLog(": Failed, Error inserting records in  :" + ex.Message);
                                    return false;
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Record Already Present !");
                    }
                }

            }
            catch (Exception ex)
            {

                WriteLog(": Failed, Error opening connection to database. \n" + ex.Message);
                return false;
            }
            return true;
        }
        private DataTable getDataTable(string IndexName, DateTime IndexDate)
        {

            DataTable dt = new DataTable();
            try
            {
                string connString = ConfigurationManager.AppSettings["ConnectionString"].ToString();

                using (var connection = new NpgsqlConnection(connString))
                {
                    NpgsqlCommand selectcmd = new NpgsqlCommand("select * from tbl_index_val where index_name='" + IndexName + "' AND index_date = '" + IndexDate.ToString("yyyy-MM-dd") + "'", connection);
                    NpgsqlDataAdapter orclda = new NpgsqlDataAdapter(selectcmd);
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
