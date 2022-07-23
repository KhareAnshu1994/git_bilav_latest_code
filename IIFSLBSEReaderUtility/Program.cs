using IIFSLBSEReaderUtility.Classes;
using Renci.SshNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace IIFSLBSEReaderUtility
{
    public class Program
    {
        static void Main(string[] args)
        {
            DateTime startDate = DateTime.Now;
            string _folderNameSuffix = ConfigurationManager.AppSettings["FileSuffixes"].ToString();
            string[] folderNameSuffix = _folderNameSuffix.Split(',');

            LogError("***********************Starting Session " + DateTime.Now.ToString("dd MMM, yyyy HH:mm") + "********************", "IIFSLBSEReaderUtility");
            foreach (Process p in Process.GetProcesses().Where(p => p.ProcessName.Contains(System.AppDomain.CurrentDomain.FriendlyName)))
            {
                try
                {
                    if (p.StartTime.Minute < DateTime.Now.Minute)
                        p.Kill();
                }
                catch (Exception ex)
                {
                }
            }
            for (DateTime k = startDate; k.CompareTo(DateTime.Now) <= 0; k = k.AddDays(1))
            {
                for (int i = 0; i < folderNameSuffix.Length; i++)
                {
                    ProcessFiles(k.ToString("yyyyMMdd") + folderNameSuffix[i]);
                }
            }
            ReadSDCFile();
            LogError("***********************Session " + DateTime.Now.ToString("dd MMM, yyyy HH:mm") + " comleted successfully****************************", "IIFSLBSEReaderUtility");
        }
        public static void ReadSDCFile()
        {
            Console.WriteLine("okk1");
            string FileSourcePath = ConfigurationManager.AppSettings["FileSourcePath"].ToString();
            Common common = new Common();
            try
            {
                Console.WriteLine("insert database Start.");
                Console.WriteLine("okk1 path :" + FileSourcePath);
                if (Directory.Exists(FileSourcePath))
                {
                    FileInfo[] DownloadedFiles = new DirectoryInfo(FileSourcePath).GetFiles();
                    FileReader Reader = new FileReader();
                    Console.WriteLine("total file count :" + DownloadedFiles.Length);
                    LogError("File Count: " + DownloadedFiles.Length, "IIFSLBSEReaderUtility");
                    //Console.WriteLine("insert database Start.");
                    for (int i = 0; i < DownloadedFiles.Length; i++)
                    {
                        DataTable dtReadSDC = Reader.ReadSDC(DownloadedFiles[i].FullName, "\t", 0);
                        if (dtReadSDC.Rows.Count > 0)
                        {
                            Console.WriteLine("Count of data:" + dtReadSDC.Rows.Count + "  " + " File Name : " + Path.GetFileName(DownloadedFiles[i].FullName));
                            LogError("Count of data: " + dtReadSDC.Rows.Count + "  " + "File Name : " + Path.GetFileName(DownloadedFiles[i].FullName), "IIFSLBSEReaderUtility");
                            string TableType = DownloadedFiles[i].Name.Substring(DownloadedFiles[i].Name.Length - 7);

                            string[] TableName = TableType.Split('.');
                            string InsertTableName = TableName[0];

                            //insert a records 
                            common.InsertDB(dtReadSDC, InsertTableName);

                            Console.WriteLine("Save file in database End.");
                            LogError("Save file in database End." + DownloadedFiles[i].Name, "IIFSLBSEReaderUtility");
                        }
                        else
                        {
                            LogError("Records not found this file: " + DownloadedFiles[i].Name, "IIFSLBSEReaderUtility");
                        }
                    }
                }
                else
                {
                    LogError("Directory not created: ", "IIFSLBSEReaderUtility");
                }
            }
            catch (Exception ex)
            {
                LogError("Failed a File Reading: " + ex.ToString(), "IIFSLBSEReaderUtility");
            }
        }
        public static void LogError(string message, string FileName)
        {

            string ErrorLogFile = ConfigurationManager.AppSettings["ErrorLogFile"];
            if (!Directory.Exists(ErrorLogFile))
                Directory.CreateDirectory(ErrorLogFile);

            ErrorLogFile += "\\" + FileName + "ErrorLog" + DateTime.Now.ToString("dd-MMM-yy") + ".txt";
            using (StreamWriter sw = new StreamWriter(ErrorLogFile, true))
            {
                sw.WriteLine(DateTime.Now.ToString("dd-MMM-yy HH:mm:ss") + "\t" + message);
            }
        }
        public static void ProcessFiles(string cFolderName)
        {
            String Host = ConfigurationManager.AppSettings["Host"].ToString();
            int Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"].ToString());
            String Username = ConfigurationManager.AppSettings["Username"].ToString();
            String Password = ConfigurationManager.AppSettings["Password"].ToString();
            string FileSourcePath = ConfigurationManager.AppSettings["FileSourcePath"].ToString();
            string sftp_file_location = ConfigurationManager.AppSettings["sftp_file_location"].ToString();

            Console.WriteLine("File Process started" + cFolderName);
            LogError("File Process started" + cFolderName, "IIFSLBSEReaderUtility");
            if (System.IO.Directory.Exists(FileSourcePath))
            {
                Hashtable ZipFiles = new Hashtable();
                try
                {
                    using (var sftp = new SftpClient(Host, Port, Username, Password))
                    {
                        sftp.OperationTimeout = TimeSpan.FromMinutes(180); //anshu

                        sftp.Connect();
                        Console.WriteLine("Connected To SFTP");
                        if (sftp.Exists(sftp_file_location + cFolderName))
                        {
                            using (var file = File.OpenWrite(FileSourcePath + "\\" + cFolderName))
                            {
                                sftp.DownloadFile(sftp_file_location + cFolderName, file);
                                Console.WriteLine("File downloaded");
                                ZipFiles[cFolderName] = "success";
                                sftp.Delete(sftp_file_location + cFolderName);
                                Console.WriteLine("File Move" + FileSourcePath + "\\" + cFolderName);
                                LogError("File Move" + FileSourcePath + "\\" + cFolderName, "IIFSLBSEReaderUtility");
                            }
                        }
                        else
                        {
                            Console.WriteLine("File not available");
                            ZipFiles[cFolderName] = "failed";
                        }

                        sftp.Disconnect();
                    }
                }
                catch (Exception ex)
                {
                    LogError(cFolderName + " : Failed, File receiving failed. \n" + ex.Message, "IIFSLBSEReaderUtility");
                    Console.WriteLine(cFolderName + " : Failed, File receiving failed. \n" + ex.Message);
                    //sbErrorCollector.Append(cFolderName + " : Failed, File receiving failed. \n" + ex.Message).AppendLine();
                }
            }
        }
    }
}
