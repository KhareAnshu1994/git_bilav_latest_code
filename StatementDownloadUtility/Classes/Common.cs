using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatementDownloadUtility
{
    public class Common
    {
        public DataTable GetBankDetails(string BankName)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "GetBankByBankName";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.Add("@BankName", SqlDbType.VarChar).Value =  BankName;
                conn.Open();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public int InsertBankScriptReports(string BankName, string FileName, long fileSize, string fileExtn, DateTime ScriptRuntime, string ScriptStatus, string Remark)
        {
            int intRow = 0;
            string strRecords = string.Empty;
            string strQuery = string.Empty;
            DataSet ds = new DataSet();

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "InsertBankScriptReports";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BankName", SqlDbType.VarChar).Value = BankName;
            cmd.Parameters.Add("@FileName", SqlDbType.VarChar).Value = FileName;
            cmd.Parameters.Add("@FileSize", SqlDbType.Real).Value = fileSize;
            cmd.Parameters.Add("@FileExtension", SqlDbType.VarChar).Value = fileExtn;
            cmd.Parameters.Add("@ScriptRuntime", SqlDbType.DateTime).Value = ScriptRuntime;
            cmd.Parameters.Add("@ScriptStatus", SqlDbType.VarChar).Value = ScriptStatus;
            cmd.Parameters.Add("@Remark", SqlDbType.VarChar).Value = Remark;

            try
            {
                conn.Open();
                intRow = cmd.ExecuteNonQuery();
                intRow = 0;
            }
            catch (Exception )
            {
                intRow = 1;
            }
            finally
            {
                conn.Close();
            }
            return intRow;
        }

        public static void LogError(string message,string BankName)
        {
            string ErrorLogFile = ConfigurationManager.AppSettings["ErrorLogFile"];
            using (StreamWriter sw = new StreamWriter(Path.Combine(ErrorLogFile, BankName+"ErrorLog"+ DateTime.Now.ToString("dd-MMM-yy")+ ".txt"),true))
            {
                sw.WriteLine(DateTime.Now.ToString("dd-MMM-yy HH:mm:ss")+"\t"+ message);
            }
        }
    }
}
