using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using OfficeOpenXml;
using System.IO;
using Renci.SshNet;

namespace IndexWeightageToSFPT_Utility
{
    public class ProcessToUpload
    {
        readonly string DbCon = ConfigurationManager.AppSettings.Get("dbCon");
        string file_locationToSave = ConfigurationManager.AppSettings.Get("file_save_location");

        string sftp_host = ConfigurationManager.AppSettings.Get("spft_host");
        string sftp_user = ConfigurationManager.AppSettings.Get("sftp_user");
        string sftp_password = ConfigurationManager.AppSettings.Get("sftp_password");

        string sftp_upload_location = ConfigurationManager.AppSettings.Get("sftp_upload_path");
        public void ActivityStart()
        {
            DataTable DT = new DataTable();
            using (NpgsqlConnection con = new NpgsqlConnection(DbCon))
            {

                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  eq_portfolio_sp_get_index_weighate()", con))
                {
                    cmd.Connection = con;
                    using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                    {
                        SqDA.Fill(DT);
                    }
                }
                if (DT.Rows.Count > 0)
                {
                    string source_file_name = file_locationToSave + "\\" + "INDEX_WT.xlsx";
                    if (File.Exists(source_file_name))
                    {
                        File.Delete(source_file_name);
                    }

                    using (ExcelPackage pck = new ExcelPackage(source_file_name))
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");
                        ws.Cells["A1"].LoadFromDataTable(DT, true);
                        pck.Save();
                        WriteLog("file generated successfully -:" + Path.GetFileName(source_file_name));
                        Console.WriteLine("file generated successfully");
                    }
                    if (File.Exists(source_file_name))
                    {
                        if (UploadSFTPFile(sftp_host, sftp_user, sftp_password, source_file_name, sftp_upload_location, 22))
                        {
                            Console.WriteLine("File uploaded on SFTP");
                            WriteLog("file uploded on SFTP with row count :" + DT.Rows.Count);
                            File.Delete(source_file_name);
                            WriteLog("file deleted successfully :-" + Path.GetFileName(source_file_name));
                        }

                    }
                    else
                    {
                        WriteLog("file not available !");
                        Console.WriteLine("file not available !");
                    }
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
