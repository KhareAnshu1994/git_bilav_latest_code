using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIFSLNSEEmailUtility
{
    public class Helper
    {
        public static void LogError(string message)
        {
            string ErrorLogDir = ConfigurationManager.AppSettings["ErrorLogFile"];
            if (!Directory.Exists(ErrorLogDir))
                Directory.CreateDirectory(ErrorLogDir);

            ErrorLogDir += "\\IIFS_NSE_Email_ErrorLog_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";

            using (StreamWriter sw = new StreamWriter(ErrorLogDir, true))
            {
                sw.WriteLine(DateTime.Now.ToString("dd-MMM-yy HH:mm:ss") + "\t" + message);
            }
        }
        public static DataTable ReadCSV(string filePath, string delimiter, int startIndex)
        {
            DataTable dt = new DataTable();
            try
            {
                string FileArchivePath = ConfigurationManager.AppSettings["MoveDirectory"].ToString();

                string[] columns = null;

                var lines = File.ReadAllLines(filePath);

                if (lines.Count() > 0)
                {
                    columns = lines[startIndex].Split(new string[] { delimiter }, StringSplitOptions.None);

                    foreach (var column in columns)
                        if (column.Length > 1)
                            dt.Columns.Add(column.Replace("\"", ""));
                }

                for (int i = startIndex + 1; i < lines.Count(); i++)
                {
                    DataRow dr = dt.NewRow();
                    string[] values = lines[i].Split(new string[] { delimiter }, StringSplitOptions.None);

                    for (int j = 0; j < values.Count() && j < columns.Count(); j++)
                    {
                        dr[j] = values[j].Replace("\"", "");
                    }
                    dt.Rows.Add(dr);

                }
                Console.WriteLine("File read Success");
                return dt;
            }
            catch (Exception ex)
            {
                Helper.LogError(filePath + ": Failed, File read. \n" + ex.Message);
                return dt;
            }
        }

       
    }
}
