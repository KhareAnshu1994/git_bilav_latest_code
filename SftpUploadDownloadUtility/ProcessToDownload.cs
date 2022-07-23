using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SftpUploadDownloadUtility
{
    public class sftp_files
    {
        public string file_name { get; set; }
    }
    public class ProcessToDownload
    {
        readonly string DbCon = ConfigurationManager.AppSettings.Get("dbCon");
        string local_destination = ConfigurationManager.AppSettings.Get("file_location");
        string remote_folder_path = ConfigurationManager.AppSettings.Get("sftp_upload_path");
        string sftp_host = ConfigurationManager.AppSettings.Get("spft_host");
        string sftp_user = ConfigurationManager.AppSettings.Get("sftp_user");
        string sftp_password = ConfigurationManager.AppSettings.Get("sftp_password");

        public void ActivityStart()
        {

            if (DownloadSFTPFile(sftp_host, sftp_user, sftp_password, remote_folder_path, local_destination, 22))
            {
                Console.WriteLine("File download from SFTP");
                WriteLog("File download from SFTP");
            }

        }
        private bool DownloadSFTPFile(string host, string username, string password, string RemoteFolder, string local_destination, int port)
        {
            try
            {
                using (SftpClient client = new SftpClient(host, port, username, password))
                {
                    client.Connect();
                    Console.WriteLine("Connected to sftp");
                    var fileList = client.ListDirectory(RemoteFolder).ToList();
                    Console.WriteLine("total files :" + fileList.Count);
                    WriteLog("SFTP files count :" + fileList.Count);
                    foreach (var remote_filenm in fileList)
                    {
                        string derived_filnm = remote_filenm.Name;

                        if (!derived_filnm.StartsWith(".") && !derived_filnm.StartsWith(".."))
                        {
                            if (client.Exists(RemoteFolder + derived_filnm) && Path.HasExtension(derived_filnm))
                            {
                                using (var file = File.OpenWrite(local_destination + "\\" + remote_filenm.Name))
                                {
                                    try
                                    {
                                        client.DownloadFile(RemoteFolder + remote_filenm.Name, file);
                                        client.Delete(RemoteFolder + derived_filnm);
                                        WriteLog("file deleted after downloaded :" + remote_filenm.Name);
                                        Console.WriteLine("file downloaded :" + remote_filenm.Name);
                                    }
                                    catch (Exception ex)
                                    {
                                        WriteLog("file downloaded error :" + ex.Message);
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                    return true;
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

            ErrorLogDir += "\\sftp_download_error_log_" + ".txt";

            using (StreamWriter sw = new StreamWriter(ErrorLogDir, true))
            {
                sw.WriteLine(DateTime.Now.ToString("dd-MMM-yy HH:mm:ss") + "\t" + message);
            }
        }
    }
}
