using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;

namespace StatementReaderUtility
{
    public class FileReader
    {
        public DataTable ReadExcel(string filePath, string BankName = "ExcelRead")
        {
            try
            {
                bool lFlag = false;
                string FileArchivePath = ConfigurationManager.AppSettings["FileArchivePath"];
                DataTable dt = new DataTable();
                using (OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties= HTML Import;"))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    else
                        con.Open();

                    DataTable tableList = con.GetSchema("Tables");
                    if (tableList != null)
                    {
                        for (int i = 0; i < tableList.Rows.Count; i++)
                        {
                            if (tableList.Rows[i]["TABLE_NAME"].ToString().Contains("Table1"))
                            {
                                lFlag = true;
                                break;
                            }
                        }
                        if (lFlag)
                        {
                            using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Table1]", con))
                            {
                                using (OleDbDataAdapter adp = new OleDbDataAdapter(cmd))
                                {
                                    adp.Fill(dt);
                                    try
                                    {
                                        File.Delete(filePath);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    return dt;
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                File.Delete(filePath);
                            }
                            catch (Exception)
                            {
                            }
                            return null;
                        }
                    }
                    else
                    {
                        try
                        {
                            File.Delete(filePath);
                        }
                        catch (Exception)
                        {
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Common.LogError("Unable to read Excel file maybe because of incompatible build platform or Microsoft.ACE.OLEDB.12.0 is not registered on the machine", BankName);
                Console.WriteLine("Unable to read Excel file maybe because of incompatible build platform or Microsoft.ACE.OLEDB.12.0 is not registered on the machine");
                return null;
            }
        }
        public DataTable ReadCSV(string filePath, string delimiter, int startIndex)
        {
            string FileArchivePath = ConfigurationManager.AppSettings["FileArchivePath"];

            DataTable dt = new DataTable();
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
            try
            {
               File.Delete(filePath);
            }
            catch (Exception)
            {
            }
            return dt;
        }
        public DataTable ReadText(string filePath, string delimiter, int startIndex)
        {
            DataTable dt = new DataTable();
            string[] columns = null;

            string FileArchivePath = ConfigurationManager.AppSettings["FileArchivePath"];

            DirectoryInfo existingFiles = new DirectoryInfo(FileArchivePath);
            foreach (FileInfo file in existingFiles.GetFiles())
            {
                file.Delete();
            }

            string tempFileName = Path.GetFileNameWithoutExtension(filePath) + ".txt";
            File.Move(filePath, Path.Combine(FileArchivePath, tempFileName));

            filePath = Path.Combine(FileArchivePath, tempFileName);

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
            try
            {
                File.Delete(filePath);
            }
            catch (Exception)
            {

                throw;
            }
            return dt;
        }
    }
}