using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlmbergCsvToDbUtility
{
    public class DataModel
    {
        public string PROG_NAME { get; set; }
        public string DEAL_ID { get; set; }
        public DateTime TRAN_DATE { get; set; }
        public string FIELD_NAME { get; set; }
        public string FIELD_VALUE { get; set; }
        public string UPDATE_YN { get; set; }
        public string RECTYPE { get; set; }
        public decimal CURRNO { get; set; }
        public string INPUT_REVERSE { get; set; }
        public int SRL_NO { get; set; }
        public string FILENAME { get; set; }
        public DateTime FILEDATE { get; set; }
        public int FILENUMBER { get; set; }
    }
    public class R01_EquiryModel
    {
        public DateTime TRADE_DATE { get; set; }
        public string ORDER_ID { get; set; }
        public string BROKER { get; set; }
        public string BROKER_NAME { get; set; }
        public string SEBIREGNO_BROKER { get; set; }
        public decimal BROKERAGE { get; set; }
        public string SECURITY { get; set; }
        public string SECURITY_NAME { get; set; }
        public string ISIN { get; set; }
        public string ISUSER { get; set; }
        public string TRAN_TYPE { get; set; }
        public string SCHEME { get; set; }
        public string SCHNAME { get; set; }
        public string SEBIREGNO_SCHEME { get; set; }
        public decimal CONFIRMED_QUANTITY { get; set; }
        public decimal CONFIRMED_PRICE { get; set; }
        public decimal CONFIRMED_AMOUNT { get; set; }
        public string STK_EXCK { get; set; }
        public string MAPIN_EXCH { get; set; }
        public string SEND_ADDR { get; set; }
        public string BASKTYN { get; set; }
        public string ASSET_TYPE { get; set; }
        public string DEALER { get; set; }
        public string PRIME_SEC { get; set; }
        public string FILENAME { get; set; }
        public DateTime FILEDATE { get; set; }
        public int FILENUMBER { get; set; }
        public string TICKET_TYPE { get; set; }
        public string TICKET_NUMBER { get; set; }

        public decimal COMMISSION_AMT { get; set; }
        public decimal STT { get; set; }
        public decimal TOT_TXN_COST { get; set; }

        public string SETTLEMENT_DATE { get; set; }
        public string MF_REFERENCE { get; set; }

        public string PARSEKEY { get; set; }
        public string BBGTICKER { get; set; }


    }
    public class R01_DerivativesModel
    {
        public string ORDER_ID { get; set; }
        public DateTime VALUE_DATE { get; set; }
        public string UTI_SCHEME { get; set; }
        public string UTI_BROKER_CODE { get; set; }
        public string BROKER_NAME { get; set; }
        public string UTI_FUTURE_CODE { get; set; }
        public string BUY_SELL { get; set; }
        public decimal QUANTITY { get; set; }
        public decimal VAL { get; set; }
        public string UTI_DEALER_CODE { get; set; }
        public string EXCHG_SCHEME_CODE { get; set; }
        public string EXCHG_FUT_CODE { get; set; }
        public DateTime MATDT { get; set; }
        public string MAT_MONTH { get; set; }
        public string EXCHANGE { get; set; }
        public string CUST_BROKER_CODE { get; set; }
        public decimal BROKERAGE_BPS { get; set; }
        public string STT_YN { get; set; }
        public string CALL_OR_PUT { get; set; }
        public decimal STRIKE_PRICE { get; set; }
        public string STOCK_OR_INDEX { get; set; }
        public string ASSET_CATEGORY { get; set; }
        public string TICKET_TYPE { get; set; }
        public string FILENAME { get; set; }
        public DateTime FILEDATE { get; set; }
        public int FILENUMBER { get; set; }
    }
    public static class Helper
    {
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

        public static void WriteLog(string message, string logType)
        {
            string ErrorLogDir = ConfigurationManager.AppSettings["ErrorLogFile"].ToString();
            if (!Directory.Exists(ErrorLogDir))
                Directory.CreateDirectory(ErrorLogDir);

            if (logType == "S")
            {
                ErrorLogDir += "\\BlmBergCSV_SuccessLog_" + ".txt";
            }
            else
            {
                ErrorLogDir += "\\BlmBergCSV_ErrorLog_" + ".txt";
            }

            using (StreamWriter sw = new StreamWriter(ErrorLogDir, true))
            {
                sw.WriteLine(DateTime.Now.ToString("dd-MMM-yy HH:mm:ss") + "\t" + message);
            }
        }

        public static void WriteLogXml(string message)
        {
            string ErrorLogDir = ConfigurationManager.AppSettings["ErrorLogFile"].ToString();
            if (!Directory.Exists(ErrorLogDir))
                Directory.CreateDirectory(ErrorLogDir);
            ErrorLogDir += "\\BlmBergxml_ErrorLog_" + ".txt";

            using (StreamWriter sw = new StreamWriter(ErrorLogDir, true))
            {
                sw.WriteLine(DateTime.Now.ToString("dd-MMM-yy HH:mm:ss") + "\t" + message);
            }
        }
        public static DataTable ReadCSV(string filePath, string delimiter, int startIndex)
        {
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
            //try
            //{
            //    File.Delete(filePath);
            //}
            //catch (Exception)
            //{
            //}
            return dt;
        }

    }
}
