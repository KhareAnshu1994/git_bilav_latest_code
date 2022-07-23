using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace IIFSLBSEReaderUtility.Classes
{
    public class FileReader
    {
        public DataTable ReadSDC(string filePath, string delimiter, int startIndex)
        {            
            Console.WriteLine("Read the file");
            DataTable dt = new DataTable();
            try
            {
                string[] columns = null;

                var lines = File.ReadAllLines(filePath);
                lines = lines.Take(lines.Count() - 1).ToArray();

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
            }
            catch (Exception ex)
            {
                Common.LogError("ReadSDC: Unable to read SDC file :" + Path.GetFileName(filePath)  +":  "+ ex.ToString(), "IIFSLBSEReaderUtility");                
            }
            return dt;
        }
    }
}
