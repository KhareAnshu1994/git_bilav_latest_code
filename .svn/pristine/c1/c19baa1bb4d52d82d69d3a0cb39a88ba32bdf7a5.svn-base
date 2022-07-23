using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.IO;
using Ionic.Zip;
using System.Threading;
using System.Data;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using System.Data.OleDb;

namespace BilavNSE_WeightageUtility
{
    public class FileReader
    {
        List<string> downloadedfiles = new List<string>();


        public void FileReadingStart()
        {
            string FinalDownloadDir = ConfigurationManager.AppSettings["MoveDirectory"].ToString();
            string BilavDownloadDir = ConfigurationManager.AppSettings["BilavDownloadDirectory"].ToString();
            string FileNMN = "UTI" + Day((DateTime.Now.Day).ToString()) + Day(DateTime.Now.Month.ToString()) + DateTime.Now.Year.ToString().Substring(2, 2);

            bool IsProceed = true;
            string[] UTI_INDICES_FILES = Directory.GetFiles(BilavDownloadDir, FileNMN + "*.zip"); // Provide specific zip name.

            Helper.LogError("Total NSE weigtage file count : " + UTI_INDICES_FILES.Count());
            foreach (string FileNM in UTI_INDICES_FILES)
            {
                ZipFile zfile = ZipFile.Read(FileNM);
                try
                {
                    string[] FileCounts = System.IO.Directory.GetFiles(FinalDownloadDir, "*");

                    if (FileCounts.Count() > 0)
                    {
                        foreach (string DelFileNm in FileCounts)
                        {
                            try
                            {
                                File.Delete(DelFileNm);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("File Deletion issues from final download :\n" + ex.Message);
                                IsProceed = false;
                                Thread.Sleep(2000);
                            }
                        }
                    }

                    if (!IsProceed)
                    {
                        return;
                    }
                    var flattenFoldersOnExtract = zfile.FlattenFoldersOnExtract;
                    zfile.FlattenFoldersOnExtract = true;
                    zfile.ExtractAll(FinalDownloadDir, ExtractExistingFileAction.OverwriteSilently);
                    zfile.FlattenFoldersOnExtract = flattenFoldersOnExtract;
                    zfile.Dispose();
                    try
                    {
                        File.Delete(FileNM);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("File Deletion ERROR :\n" + ex.Message);
                    }

                    Console.WriteLine("File Zip extraction successfully");
                    Helper.LogError("File Zip extraction successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("File Zip extraction failed " + ex.Message);
                    Helper.LogError("File Zip extraction failed" + ex.Message);
                }

                FileInfo[] DownloadedFiles = new DirectoryInfo(FinalDownloadDir).GetFiles();

                foreach (FileInfo FI in DownloadedFiles)
                {

                    // DataTable dtReadXLSX = ReadXLSX(FI.FullName, FI.Name, System.IO.Path.GetExtension(FI.Name), true);
                    DataTable dtReadXLSX = Helper.GetDataTableFromExcel(FI.FullName, true);
                    Helper.LogError("dtReadXLSX count" + dtReadXLSX.Rows.Count);
                    if (dtReadXLSX.Rows.Count > 0)
                    {
                        InsertData Data = new InsertData();
                        Data.InsertXLSXDB(dtReadXLSX, FI.Name.Substring(0, 2), FI.Name);
                    }
                }
                try
                {
                    File.Delete(FileNM);

                }
                catch (Exception ex)
                {
                    Helper.LogError("zip file deleteion issue :" + ex.Message);
                }
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
        //public DataTable ReadXLSX(string filePath, string filename, string Extension, bool isHDR)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        string con = @"Provider=Microsoft.Jet.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0 Xml;HDR=YES;\""; //Excel 8.0;HDR=YES;\"";

        //        string conStr = "";

        //        switch (Extension.Trim().ToLower())
        //        {
        //            case ".xls": //Excel 97-03
        //                conStr = ConfigurationManager.AppSettings["Excel03ConString"];
        //                break;
        //            case ".xlsx": //Excel 07
        //                conStr = ConfigurationManager.AppSettings["Excel07ConString"];
        //                break;
        //        }
        //        conStr = String.Format(conStr, filePath, isHDR);
        //        NpgsqlConnection connExcel = new NpgsqlConnection(conStr);
        //        NpgsqlCommand cmdExcel = new NpgsqlCommand();
        //        NpgsqlDataAdapter oda = new NpgsqlDataAdapter();
        //        cmdExcel.Connection = connExcel;
        //        connExcel.Open();
        //        DataTable dtExcelSchema;
        //        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

        //        for (int k = 0; k < dtExcelSchema.Rows.Count; k++)
        //        {
        //            string SheetName = dtExcelSchema.Rows[k]["TABLE_NAME"].ToString();
        //            NpgsqlCommand objCmdSelect = new NpgsqlCommand("SELECT * FROM [" + SheetName + "]", connExcel);
        //            NpgsqlDataAdapter objAdapter1 = new NpgsqlDataAdapter();
        //            objAdapter1.SelectCommand = objCmdSelect;
        //            DataSet objDataset1 = new DataSet();
        //            objAdapter1.Fill(objDataset1, SheetName);
        //            dt = objDataset1.Tables[SheetName];
        //        }
        //        connExcel.Close();
        //        Console.WriteLine(filename + " " + " read Success");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(filePath + ": Failed, File read. \n" + ex.Message);
        //    }
        //    Console.WriteLine(filename + " : File Read End.");
        //    return dt;

        //}

    }
}
