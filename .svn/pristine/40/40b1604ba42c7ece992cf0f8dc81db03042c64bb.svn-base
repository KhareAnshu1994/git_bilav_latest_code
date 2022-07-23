using HtmlAgilityPack;
using Renci.SshNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Index_Download.classes
{
    public class AmfiPortal
    {
        public DateTime date { get; set; }
        public string currentValue { get; set; }
        public string AmfiIndexName { get; set; }
        public static CustomError ProcessAmfiPortalIndex()
        {



            NameValueCollection common_setting = (NameValueCollection)ConfigurationManager.GetSection("customAppSettingsGroup/CommonAppSettings");


            string Host = common_setting["Host"].ToString();
            int Port = Convert.ToInt32(common_setting["Port"].ToString());
            string Username = common_setting["Username"].ToString();
            string Password = common_setting["Password"].ToString();
            string local_destination = common_setting["MoveDirPath"].ToString();
            string RemoteFolder = common_setting["sftp_file_location"].ToString();

            try
            {
                using (SftpClient client = new SftpClient(Host, Port, Username, Password))
                {
                    client.Connect();
                    Console.WriteLine("Connected to sftp");
                    var fileList = client.ListDirectory(RemoteFolder).ToList();
                    Console.WriteLine("total files :" + fileList.Count);
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
                                        Console.WriteLine("file downloaded :" + remote_filenm.Name);
                                    }
                                    catch (Exception ex)
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SFTP upload error !" + ex.Message);
            }

            var files = Directory.GetFiles(common_setting["MoveDirPath"].ToString() + "\\", "*.xls*", SearchOption.AllDirectories);

            commonHelper.WriteLog("historcalpath :" + files.Count(), "E");
            CustomError custom_error = new CustomError();
            foreach (string file_nm in files)
            {
                if (file_nm.Contains("HistoricalIndices"))
                {
                    try
                    {
                        string con = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + file_nm + ";Extended Properties='HTML Import;HDR=Yes;'";
                        commonHelper.WriteLog("historical path :" + con, "E");
                        using (OleDbConnection connection = new OleDbConnection(con))
                        {
                            connection.Open();
                            System.Data.DataTable dbSchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, null);

                            var firstSheetName = dbSchema.Rows[0]["TABLE_NAME"];

                            OleDbCommand command = new OleDbCommand("select * from [" + firstSheetName + "]", connection);

                            string[] AmfiPortalIndex = Convert.ToString(common_setting["AmfiPortalIndex"]).Split(',');
                            List<AmfiPortal> AmfiIndex = new List<AmfiPortal>();
                            int AmfiIndexCount = 0;
                            using (OleDbDataReader dr = command.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    if (dr[0].ToString().Trim() != string.Empty)
                                    {
                                        AmfiIndexCount++;
                                        // commonHelper.WriteLog(" --------------------------######## Index #########> " + dr[0].ToString().Trim());

                                        if (AmfiPortalIndex.Contains(dr[0].ToString().Trim()))
                                        {
                                            AmfiPortal index = new AmfiPortal();
                                            index.AmfiIndexName = dr[0].ToString();
                                            if (index.AmfiIndexName == "Index")
                                                continue;
                                            index.date = commonHelper.GetFormatedDate(dr[1].ToString().Trim());
                                            index.currentValue = dr[2].ToString().Trim();
                                            AmfiIndex.Add(index);
                                        }
                                        //else
                                        //{
                                        //    commonHelper.WriteLog(" --------------------------######## Not Matched #########> " + dr[0].ToString().Trim(), "S");
                                        //}
                                    }
                                }
                                
                                if (AmfiIndex.Count() > 0)
                                {
                                    int incrment = 0;
                                    foreach (var val in AmfiIndex)
                                    {
                                        commonHelper helper = new commonHelper();
                                        helper.currentName = val.AmfiIndexName;
                                        //helper.date = DateTime.Now.Date.AddDays(-1);
                                        helper.date = val.date;
                                        helper.currentValue = val.currentValue;
                                        commonHelper.saveData(helper);
                                        //custom_error = commonHelper.saveDataToDB(helper);

                                        //commonHelper.saveData(helper);
                                        incrment++;
                                    }

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("exception occured while reading excel : " + ex.Message);
                        custom_error.index_name = "Amfi Portal Index";
                        custom_error.status = "fail";
                        custom_error.error_msg = ex.Message;
                        custom_error.is_success = false;
                    }
                    File.Delete(file_nm);
                }
            }
            return custom_error;
        }
    }
}
