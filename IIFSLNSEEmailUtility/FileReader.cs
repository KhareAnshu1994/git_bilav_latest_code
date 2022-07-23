using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace IIFSLNSEEmailUtility
{
    public enum EmailStatus
    {
        NoAttachments,
        InvalidAttachmentFormat,
        FileDownloadedRegistered,
        AttachmentCountNotValid,
        RegisteredANDDownloaded
    }
    class FileReader
    {
        List<string> downloadedfiles = new List<string>();

        string FinalDownloadDir = ConfigurationManager.AppSettings["MoveDirectory"].ToString();
        string BilavDownloadDir = ConfigurationManager.AppSettings["BilavDownloadDirectory"].ToString();
        public void FileReadingStart()
        {
            bool IsProceed = true;
            string[] NSE_FILES = Directory.GetFiles(BilavDownloadDir, "NIFTY*.zip"); // Provide specific zip name.
            Helper.LogError("Total NSE file count : " + NSE_FILES.Count());
            foreach (string FileNM in NSE_FILES)
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
                    Helper.LogError("File Zip extraction successfully------>" + FileNM);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("File Zip extraction failed " + ex.Message);
                    Helper.LogError("File Zip extraction failed" + ex.Message);
                }

                string[] CheckDownloadFile = System.IO.Directory.GetFiles(FinalDownloadDir, "*.csv");
                for (int m = 0; m < CheckDownloadFile.Length; m++)
                {
                    string fileName = CheckDownloadFile[m].ToString();
                    string DownloadFile = Path.GetFileName(fileName);

                    string firstThreeLetter = DownloadFile.Substring(0, 3);

                    if (!((DownloadFile.ToLower().Contains("pr" + DateTime.Now.ToString("yyyyMMdd") + ".csv")) || (firstThreeLetter.Contains("bod"))))
                        File.Delete(CheckDownloadFile[m]);

                }
                DirectoryInfo dirInfo = new DirectoryInfo(FinalDownloadDir);
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    if (!downloadedfiles.Contains(file.Name) && file.Name.ToLower().Contains(".csv"))
                        downloadedfiles.Add(file.Name);
                }
                Helper.LogError("Extract File Count : " + downloadedfiles.Count);
                if (downloadedfiles.Count < 2)
                {
                    //Console.WriteLine("Download File Count less than 2.");
                    if (downloadedfiles.Count == 0)
                    {
                        Console.WriteLine("After extracted no file found current date.");
                    }
                    else
                    {

                        ReadCSVFile();
                        Console.WriteLine("After extracted file less than 2.");
                    }
                }
                else if (downloadedfiles.Count == 2)
                {
                    ReadCSVFile();
                }
            }
        }

        public void ReadCSVFile()
        {
            try
            {
                Console.WriteLine("Save file in database Start.");
                InsertData Data = new InsertData();
                FileInfo[] DownloadedFiles = new DirectoryInfo(FinalDownloadDir).GetFiles();
                FileReader Reader = new FileReader();
                for (int i = 0; i < DownloadedFiles.Length; i++)
                {
                    DataTable dtReadCSV = Helper.ReadCSV(DownloadedFiles[i].FullName, ",", 0);
                    if (dtReadCSV.Rows.Count > 0)
                    {
                        string TableType = DownloadedFiles[i].Name.Substring(0, 2);
                        string FirstColName = dtReadCSV.Rows[0][0].ToString();
                        int ColCount = dtReadCSV.Columns.Count;
                        //Insert Database
                        Data.InsertDB(dtReadCSV, TableType);
                    }
                    else
                    {
                        Console.WriteLine("Failed,file row count is less than one" + Path.GetFileName(DownloadedFiles[i].FullName));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("failed. Read CSV File: " + ex.Message);
                Helper.LogError("failed. Read CSV File: " + ex.Message);
            }
            Helper.LogError("==========Read CSV File Function End");
        }
    }
}
