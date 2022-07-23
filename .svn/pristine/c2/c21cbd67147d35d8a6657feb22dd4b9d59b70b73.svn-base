using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace StatementReaderUtility
{
    public class Common
    {

        SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());

        public void InsertStatement(string BankName, string TransDate, string ValueDate, string Reference, string Narration, string IsDebit, string IsCredit, string AccountNo)
        {

            SqlCommand cmd = new SqlCommand("sp_InsertTRANSACTIONINFO", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("strBankName", BankName);
            cmd.Parameters.AddWithValue("StrTXN_DT", TransDate);
            cmd.Parameters.AddWithValue("StrVALUE_DT", ValueDate);
            cmd.Parameters.AddWithValue("strREFERENCE", Reference);
            cmd.Parameters.AddWithValue("strNARRATION", Narration);
            cmd.Parameters.AddWithValue("dblDEBIT", IsDebit);
            cmd.Parameters.AddWithValue("dblCREDIT", IsCredit);
            cmd.Parameters.AddWithValue("strACCOUNTNO", AccountNo);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogError(ex.ToString(), "StatementReader");
            }
            finally
            {
                conn.Close();
            }
        }

        public static void LogError(string message, string BankName)
        {
            string ErrorLogFile = ConfigurationManager.AppSettings["ErrorLogFile"];
            using (StreamWriter sw = new StreamWriter(Path.Combine(ErrorLogFile, BankName + "ErrorLog" + DateTime.Now.ToString("dd-MMM-yy") + ".txt"), true))
            {
                sw.WriteLine(DateTime.Now.ToString("dd-MMM-yy HH:mm:ss") + "\t" + message);
            }
        }
    }
}
