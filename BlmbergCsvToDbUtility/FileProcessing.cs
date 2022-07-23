using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlmbergCsvToDbUtility
{
    public class FileProcessing
    {
        static private NpgsqlConnection conn;
        static string strCon = ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString;
        public void CsvOperation()
        {
            string strFileName = string.Empty;

            string[] filesInFolder = Directory.GetFiles(ConfigurationManager.AppSettings["Csv"].ToString());

            string[] RequiredFiles = ConfigurationManager.AppSettings["CsvFiles"].Split(',');
            int fileno = 0;
            Helper.WriteLog("Total files in ToMfund folders :" + filesInFolder.Length, "S");
            Console.WriteLine("Total files in ToMfund folders :" + filesInFolder.Length);

            foreach (string mfund_file in filesInFolder)
            {
                foreach (string dd in RequiredFiles)
                {
                    string filnm = Path.GetFileNameWithoutExtension(mfund_file);

                    if (filnm.ToUpper().Contains(dd))
                    {

                        try
                        {
                            File.Copy(mfund_file, Path.Combine(ConfigurationManager.AppSettings["TempCsv"].ToString(), Path.GetFileName(mfund_file)), true);

                            string targetFile_temp = Path.Combine(ConfigurationManager.AppSettings["Backup"].ToString(), Path.GetFileName(mfund_file));
                            if (!File.Exists(targetFile_temp))
                            {
                                File.Copy(mfund_file, Path.Combine(ConfigurationManager.AppSettings["AMC"].ToString(), Path.GetFileName(mfund_file)), true);
                            }
                            File.Delete(mfund_file);
                        }
                        catch (Exception ex)
                        {
                            Helper.WriteLog("File copying error [tempfile/AMC/ToMfund Delete]" + ex.Message, "S");
                        }
                        break;
                    }
                }
            }

            string[] filesInTempFolder = Directory.GetFiles(ConfigurationManager.AppSettings["TempCsv"].ToString());
            Helper.WriteLog("Total files in temp folders :" + filesInTempFolder.Length, "S");
            foreach (string filePath in filesInTempFolder)
            {
                bool isFileMoved = false;
                strFileName = Path.GetFileName(filePath);
                fileno++;
                DataModel DM = new DataModel();
                DataTable dbcheck = new DataTable();
                DataTable FileData = new DataTable();
                if (File.Exists(filePath))
                {
                    try
                    {

                        FileData = Helper.ReadCSV(filePath, ",", 0);
                        dbcheck = dbcheck = DbOperation.GetAudits(Path.GetFileNameWithoutExtension(filePath));
                        Helper.WriteLog("----------------------------------------------------------------------------------------", "S");
                        if (FileData.Rows.Count > 0)
                        {
                            if (FileData.Rows.Count == dbcheck.Rows.Count)
                            {
                                fileMovedToBackup(filePath);
                                continue;
                            }
                            //if (FileData.Rows.Count == dbcheck.Rows.Count)
                            //{
                            //    Helper.WriteLog("All record present in database of file : " + Path.GetFileNameWithoutExtension(filePath), "S");
                            //    fileMovedToBackup(filePath);
                            //    continue;
                            //}
                            //else
                            //{
                            //    if (dbcheck.Rows.Count > 0)
                            //    {
                            //        DbOperation.DeletePartialRecord(Path.GetFileNameWithoutExtension(filePath));
                            //        Helper.WriteLog("Partial records deleted for new insert : " + Path.GetFileNameWithoutExtension(filePath), "S");
                            //    }
                            //}
                            Common comm = new Common();
                            if (strFileName.ToUpper().Contains("R01_EQUITY"))
                            {
                                if (comm.ProcessR01_Equity(FileData, Path.GetFileNameWithoutExtension(filePath), fileno))
                                {
                                    isFileMoved = true;
                                }
                            }
                            else if (strFileName.ToUpper().Contains("R01_DERIVATIVE"))
                            {
                                if (comm.ProcessR01_DERIVATIVES(FileData, Path.GetFileNameWithoutExtension(filePath), fileno))
                                {
                                    isFileMoved = true;
                                }
                            }
                            else
                            {
                                if (comm.ProcessFormat3(FileData, Path.GetFileNameWithoutExtension(filePath), fileno))
                                {
                                    isFileMoved = true;
                                }
                            }
                            fileMovedToBackup(filePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Reading from dataTable failed : " + ex.Message);
                        Helper.WriteLog("error inside foreach : " + ex.Message, "E");

                    }
                    // Helper.WriteLog("Is file moved : " + isFileMoved, "S");

                }
            }
        }
        public void fileMovedToBackup(string filePath)
        {
            try
            {
                string targetFile = Path.Combine(ConfigurationManager.AppSettings["Backup"].ToString(), Path.GetFileName(filePath));
                if (File.Exists(targetFile))
                {
                    File.Delete(targetFile);
                }
                File.Copy(filePath, targetFile);
                Helper.WriteLog("file moved to target :" + targetFile, "S");
                Console.WriteLine("file moved to target :" + targetFile);
                File.Delete(filePath);
                //Helper.WriteLog("file copied tp target  :" + filePath, "S");
            }
            catch (Exception exx)
            {
                Helper.WriteLog("File move failed : " + exx.Message, "E");
            }

        }
    }
}
