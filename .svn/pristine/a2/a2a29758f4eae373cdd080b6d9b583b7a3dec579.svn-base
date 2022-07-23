using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace IIFSLNSEBillaUtility
{
    class FileReader
    {
        private static String TempDir = ConfigurationManager.AppSettings["TempDir"];
        private static String FileFormat = ConfigurationManager.AppSettings["FileFormat"];
        private static String FinalDir = ConfigurationManager.AppSettings["FinalDir"];
        private static String ArchiveDir = ConfigurationManager.AppSettings["ArchiveDir"];

        private static string AttachmentDownloadDir = ConfigurationManager.AppSettings["AttachmentDownloadDir"].ToString();
        private static string FinalDownloadDir = ConfigurationManager.AppSettings["MoveDirectory"].ToString();
        private static string BilavDownloadDir = ConfigurationManager.AppSettings["BilavDownloadDirectory"].ToString();
        private static int ArchiveAfterDays;
        private static int DeleteAfterDays;


        private static String SMTPServer = ConfigurationManager.AppSettings["SMTPServer"];
        private static String Email = ConfigurationManager.AppSettings["email"];
        private static int SMTPPort;
        
        string SendMailLog = ConfigurationManager.AppSettings["SendMailLog"];

        public void FileReadingStart()
        {
            bool IsProceed = true;
            string[] NSE_FILES = Directory.GetFiles(BilavDownloadDir, "*.zip"); // Provide specific zip name.

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
                }
                catch (Exception ex2)
                {
                    Console.WriteLine("ERROR :\n" + ex2.Message);
                }
            }
            renameNS();
            clearArchive();
            archivefiles();
            //WriteLog("***********************Session " + DateTime.Now.ToString("dd MMM, yyyy HH:mm") + " comleted successfully****************************");
        }
        public static void WriteLog(string message)
        {
            string ErrorLogDir = ConfigurationManager.AppSettings["ErrorLogFile"];
            if (!Directory.Exists(ErrorLogDir))
                Directory.CreateDirectory(ErrorLogDir);

            ErrorLogDir += "\\IIFS_NSE__Billa_Email_ErrorLog_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";

            using (StreamWriter sw = new StreamWriter(ErrorLogDir, true))
            {
                sw.WriteLine(DateTime.Now.ToString("dd-MMM-yy HH:mm:ss") + "\t" + message);
            }
        }
        private static bool isWriteAllowed(string filename)
        {
            String dirPath = Path.GetDirectoryName(filename);
            bool result = false;
            if (Directory.Exists(dirPath))
            {
                if (Directory.GetAccessControl(dirPath) != null)
                {

                    foreach (FileSystemAccessRule rule in Directory.GetAccessControl(dirPath).GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier)))
                    {
                        if ((FileSystemRights.Write & rule.FileSystemRights) != FileSystemRights.Write)
                            continue;

                        if (rule.AccessControlType == AccessControlType.Allow)
                            result = true;
                    }
                }
            }
            else { throw new Exception("Directory does not exist: " + filename); }
            return result;
        }

        private void clearArchive()
        {
            Console.WriteLine("Clear Archive File ");
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(ArchiveDir);
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    if (DateTime.Now.AddDays(-DeleteAfterDays) > file.LastAccessTime)
                        file.Delete();
                }
            }
            catch (Exception ex)
            {
                //WriteLog("clearArchive Error: " + ex.ToString());
            }
        }
        private static void intialize()
        {

            

            if (TempDir == null)
                throw new Exception("Temp dir not specified");
            if (!isWriteAllowed(TempDir))
                throw new Exception("No permission to write to TempDir");

            if (FinalDir == null)
                throw new Exception("Final dir not specified");
            if (!isWriteAllowed(FinalDir))
                throw new Exception("No permission to write to FinalDir");

            if (ArchiveDir == null)
                throw new Exception("Archive dir not specified");
            if (!isWriteAllowed(ArchiveDir))
                throw new Exception("No permission to write to ArchiveDir");

            if (!int.TryParse(ConfigurationManager.AppSettings["ArchiveAfterDays"], out ArchiveAfterDays))
                ArchiveAfterDays = 1;

            if (!int.TryParse(ConfigurationManager.AppSettings["DeleteAfterDays"], out DeleteAfterDays))
                DeleteAfterDays = 15;

           
            if (SMTPServer == null)
                throw new Exception("SMTP server not specified");

            if (Email == null)
                throw new Exception("Email ID not specified");
            if (!int.TryParse(ConfigurationManager.AppSettings["SMTPPort"], out SMTPPort))
                SMTPPort = 587;
            
        }
        private static void renameNS()
        {
            Console.WriteLine("Rename NSE File ");
            Regex reg = new Regex(FileFormat);
            //Regex reg = new Regex("^FIPR140925");
            string filename;

            Match m;


            DirectoryInfo dirInfo = new DirectoryInfo(FinalDownloadDir);
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                filename = file.Name.Substring(0, file.Name.IndexOf('.'));
                m = reg.Match(filename);
                if (m.Success)
                {
                    m = new Regex(".$").Match(filename);
                    string strYear = DateTime.Now.Year.ToString().Substring(2, 1);
                    filename = new Regex(".$").Replace(filename, strYear + m.ToString());//From 2014 Take 1
                    filename = filename.Remove(2, 1);//Added by Momin on 22 October 2014 by Momin to Remove E from NSE
                    Console.WriteLine(filename);
                    if (!File.Exists(FinalDownloadDir + filename + file.Extension))
                    {
                        File.Move(file.FullName, FinalDownloadDir + filename + file.Extension);
                        //Console.WriteLine("File Move");
                    }
                    else
                    {
                        file.Delete();
                        //Console.WriteLine("File Delete");
                    }
                }

            }
            //Console.ReadLine();
        }

       

        private void archivefiles()
        {
            Console.WriteLine("Archive File ");
            try
            {

                DirectoryInfo dirInfo = new DirectoryInfo(FinalDownloadDir);
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    if (DateTime.Now.AddDays(-ArchiveAfterDays) > file.LastAccessTime)
                        File.Move(file.FullName, ArchiveDir + file.Name);
                }
            }
            catch (Exception ex)
            {
                //WriteLog("archive files Error: " + ex.ToString());
            }
        }
        private static void deleteAllFiles()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(TempDir);
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                file.Delete();
            }
            //log.Info("Temp directory cleaned");
        }
    }
}
