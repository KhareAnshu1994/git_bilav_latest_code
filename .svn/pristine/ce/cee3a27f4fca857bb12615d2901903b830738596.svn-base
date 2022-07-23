using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilavIndexCloseValueUtility
{
    public static class Helper
    {
        public static void LogError(string message)
        {
            string ErrorLogDir = ConfigurationManager.AppSettings["ErrorLogFile"];
            if (!Directory.Exists(ErrorLogDir))
                Directory.CreateDirectory(ErrorLogDir);

            ErrorLogDir += "\\Index_Close_value_ErrorLog_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";

            using (StreamWriter sw = new StreamWriter(ErrorLogDir, true))
            {
                sw.WriteLine(DateTime.Now.ToString("dd-MMM-yy HH:mm:ss") + "\t" + message);
            }
        }
        public static DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    //FileReader.index_date = Convert.ToDateTime(drow[0].ToString());
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                return tbl;
            }
        }
        public static string ToOracleDateFormat(this string cString)
        {
            DateTime output;
            DateTime checkedDate;
            if (DateTime.TryParse(cString, out checkedDate))
            {
                string[] formats = { "M/d/yyyy", "dd-MMM-yyyy", "dd/M/yyyy", "d/M/yyyy", "M-d-yyyy", "d-M-yyyy", "d-MMM-yy", "d-MMMM-yyyy", "yyyyMMdd", "yyyy-MM-dd", "yyyy/MM/dd", "yyyyMMMdd", "yyyy/MMM/dd", "yyyy-MMM-dd", "dd/MM/yyyy", "MM/dd/yyyy", "MM/dd/yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss" };

                DateTime.TryParseExact(checkedDate.Date.ToString("dd-MMM-yyyy"), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out output);
                return output.ToString("dd-MMM-yyyy");

            }
            else
                return "";
        }

    }
}
