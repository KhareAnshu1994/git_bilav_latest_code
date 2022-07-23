using OfficeOpenXml;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SftpUploadDownloadUtility
{
    public class ProcessToUpload
    {
        readonly string DbCon = ConfigurationManager.AppSettings.Get("dbCon");
        string file_location = ConfigurationManager.AppSettings.Get("file_location");

        string sftp_host = ConfigurationManager.AppSettings.Get("spft_host");
        string sftp_user = ConfigurationManager.AppSettings.Get("sftp_user");
        string sftp_password = ConfigurationManager.AppSettings.Get("sftp_password");

        string sftp_upload_location = ConfigurationManager.AppSettings.Get("sftp_upload_path");
        string upload_file_list = ConfigurationManager.AppSettings.Get("upload_file_list");
        public void ActivityStart()
        {

            string[] file_list = Directory.GetFiles(file_location);


            foreach (string source_file in file_list)
            {

                if (File.Exists(source_file))
                {
                    if (UploadSFTPFile(sftp_host, sftp_user, sftp_password, source_file, sftp_upload_location, 22))
                    {
                        Console.WriteLine("File uploaded on SFTP");
                        WriteLog("file uploded on SFTP with row count :" + Path.GetFileName(source_file));
                        File.Delete(source_file);
                        WriteLog("file deleted successfully :-" + Path.GetFileName(source_file));
                    }
                }
                else
                {
                    WriteLog("file not available !");
                    Console.WriteLine("file not available !");
                }
            }
        }
        private bool UploadSFTPFile(string host, string username, string password, string sourcefile, string destination, int port)
        {
            try
            {
                using (SftpClient client = new SftpClient(host, port, username, password))
                {
                    client.Connect();
                    client.ChangeDirectory(destination);
                    using (FileStream fs = new FileStream(sourcefile, FileMode.Open))
                    {
                        client.BufferSize = 4 * 1024;
                        client.UploadFile(fs, Path.GetFileName(sourcefile));
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog("error in UploadSFTPFile():" + ex.Message);
                Console.WriteLine("SFTP upload error !" + ex.Message);
                return false;
            }
        }
        public static void WriteLog(string message)
        {
            string ErrorLogDir = ConfigurationManager.AppSettings["ErrorLogFile"].ToString();
            if (!Directory.Exists(ErrorLogDir))
                Directory.CreateDirectory(ErrorLogDir);

            ErrorLogDir += "\\sftp_upload_error_log_" + ".txt";

            using (StreamWriter sw = new StreamWriter(ErrorLogDir, true))
            {
                sw.WriteLine(DateTime.Now.ToString("dd-MMM-yy HH:mm:ss") + "\t" + message);
            }
        }
    }
}
