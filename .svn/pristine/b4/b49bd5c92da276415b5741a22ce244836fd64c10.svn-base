using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlmbergCsvToDbUtility
{
    public static class DbOperation
    {
        static private NpgsqlConnection conn;
        static string strCon = ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString;

        public static bool pushToDb(string strInsertQuery)
        {
            bool IsSuccess = false;
            try
            {
                using (var conn = new NpgsqlConnection(strCon))               
                {
                    conn.Open();                   
                    using (NpgsqlCommand oraCommand = new NpgsqlCommand(strInsertQuery, conn))
                    {
                        try
                        {
                            if (conn.State != ConnectionState.Open)
                            {
                                conn.Open();
                                Console.WriteLine("Reconnected to Database server.");
                            }
                            Helper.WriteLog("Preapared script : \n" + strInsertQuery, "S");
                            oraCommand.ExecuteNonQuery();

                            Helper.WriteLog("Inserted successfully", "S");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(":Failed, Error inserting records in  :" + ex.Message);
                            Helper.WriteLog("Insert error : \n" + ex.Message, "E");
                        }
                    }

                    IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(": Failed, Error opening connection to database. \n" + ex.Message);
                IsSuccess = false;
                Helper.WriteLog("pushToDb error : \n" + ex.Message, "E");
            }

            return IsSuccess;
        }

        public static DataTable GetAudits(string FILENAME)
        {
            string strTableName = string.Empty;
            if (FILENAME.ToUpper().Contains("R01_DERIVATIVE"))
            {
                strTableName = "UT_DERIDEALS";
            }
            else if (FILENAME.ToUpper().Contains("R01_EQUITY"))
            {
                strTableName = "UT_EQUITYDEALS";
            }
            else
            {
                strTableName = "UT_MFDEALS";
            }

            DataTable dt = new DataTable();
            conn = new NpgsqlConnection(strCon);
            try
            {

                if (conn.State != ConnectionState.Open)
                    conn.Open();                
                using (NpgsqlCommand selectcmd = new NpgsqlCommand("SELECT * FROM  " + strTableName + " WHERE  FILENAME='" + FILENAME + "'", conn))
                {                    
                    using (NpgsqlDataAdapter adp = new NpgsqlDataAdapter(selectcmd))
                    {
                        adp.Fill(dt);

                        Console.WriteLine("Table : " + strTableName + " --- > Count is : " + dt.Rows.Count);
                    }
                }
                Console.WriteLine(":UT_MFDEALS  read success.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(":Failed. UT_MFDEALS read Failed. \n" + ex.Message);
                Helper.WriteLog(" GetAudits error : \n" + ex.Message, "E");
            }

            return dt;
        }

        public static void BulkInsert(DataTable dt)
        {
            try
            {
                using (conn = new NpgsqlConnection(strCon))
                {
                    conn.Open();
                   // using (var bulkCopy = new Oracle.DataAccess.Client.OracleBulkCopy(conn, OracleBulkCopyOptions.UseInternalTransaction))
                    //using (var bulkCopy = new NpgsqlBatch(conn,NpgsqlRawCopyStream,NpgsqlTransaction))
                    //{
                    //    bulkCopy.DestinationTableName = "UT_MFDEALS";
                    //    bulkCopy.BulkCopyTimeout = 600;
                    //    bulkCopy.WriteToServer(dt);
                    //    Helper.WriteLog("Bulk insert successfully", "S");
                    //}

                }
            }
            catch (Exception ex)
            {
                Helper.WriteLog("Error while bulk insert : \n" + ex.Message, "E");
                Console.WriteLine("Error while bulk insert : \n" + ex.Message);
            }
        }

        public static void DeleteQuery()
        {
            try
            {
                conn = new NpgsqlConnection(strCon);
                conn.Open();
                Console.WriteLine("Connected to Database server.");
                using (NpgsqlCommand oraCommand = new NpgsqlCommand("delete from UT_DERIDEALS", conn))
                {
                    try
                    {
                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                            Console.WriteLine("Reconnected to Database server.");
                        }
                        oraCommand.ExecuteNonQuery();
                        Helper.WriteLog(": Record deleted", "S");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(":Failed, Error deleting records in  :" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteLog(": Failed, Error opening connection to database. \n" + ex.Message, "E");
            }

        }


        public static void DeletePartialRecord(string FILENAME)
        {
            string strTableName = string.Empty;
            if (FILENAME.ToUpper().Contains("R01_DERIVATIVE"))
            {
                strTableName = "UT_DERIDEALS";
            }
            else if (FILENAME.ToUpper().Contains("R01_EQUITY"))
            {
                strTableName = "UT_EQUITYDEALS";
            }
            else
            {
                strTableName = "UT_MFDEALS";
            }

            try
            {
                conn = new NpgsqlConnection(strCon);
                conn.Open();
                Console.WriteLine("Connected to Database server.");
                using (NpgsqlCommand oraCommand = new NpgsqlCommand("delete from " + strTableName + " WHERE  FILENAME='" + FILENAME + "'", conn))
                {
                    try
                    {
                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                            Console.WriteLine("Reconnected to Database server.");
                        }
                        oraCommand.ExecuteNonQuery();
                        Helper.WriteLog(": Record deleted", "S");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(":Failed, Error deleting records in  :" + ex.Message, "E");
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteLog(": Failed, Error opening connection to database. \n" + ex.Message, "E");
            }

        }


        public static bool ValidateRecord(DataSet ds, string strFileNM)
        {
            bool IsAllPresent = false;
            try
            {
                string[] strTable = "COMMON_FEEDS,UT_NOTES,UT_REPOFIELDS,UT_TRANSACTIONCOST,UT_DES_FIELDS".Split(',');

                int UT_TRADEFEEDS_Count = 0, UT_NOTES_Count = 0, UT_REPOFIELDS_Count = 0, UT_TRANSACTIONCOST_Count = 0, UT_DES_FIELDS_Count = 0;
                int dsRepoFieldCount = 0, dsTransactionCost = 0;

                foreach (string Oratable in strTable)
                {
                    DataTable dtRecord = new DataTable();
                    using (conn = new NpgsqlConnection(strCon))
                    {
                        conn.Open();
                        using (NpgsqlCommand ocmd = new NpgsqlCommand("SELECT * FROM  " + Oratable + " WHERE  FILENAME='" + strFileNM + "'", conn))
                        {
                            using (NpgsqlDataAdapter adp = new NpgsqlDataAdapter(ocmd))
                            {
                                adp.Fill(dtRecord);
                                if (dtRecord.Rows.Count > 0)
                                {
                                    if (Oratable == "COMMON_FEEDS")
                                        UT_TRADEFEEDS_Count = dtRecord.Rows.Count;

                                    if (Oratable == "UT_NOTES")
                                        UT_NOTES_Count = dtRecord.Rows.Count;

                                    if (Oratable == "UT_REPOFIELDS")
                                        UT_REPOFIELDS_Count = dtRecord.Rows.Count;

                                    if (Oratable == "UT_TRANSACTIONCOST")
                                        UT_TRANSACTIONCOST_Count = dtRecord.Rows.Count;

                                    if (Oratable == "UT_DES_FIELDS")
                                        UT_DES_FIELDS_Count = dtRecord.Rows.Count;
                                }
                            }

                        }
                    }
                }
                if (ds.Tables.Contains("RepoFields"))
                    dsRepoFieldCount = ds.Tables["RepoFields"].Rows.Count;

                if (ds.Tables.Contains("TransactionCost"))
                    dsTransactionCost = ds.Tables["TransactionCost"].Rows.Count;

                //if ((UT_TRADEFEEDS_Count == ds.Tables["TradeFeed"].Rows.Count) && (UT_TRADEFEEDS_Count > 0) && (UT_REPOFIELDS_Count == ds.Tables["RepoFields"].Rows.Count) && (UT_TRANSACTIONCOST_Count == ds.Tables["TransactionCost"].Rows.Count))
                if ((UT_TRADEFEEDS_Count == ds.Tables["TradeFeed"].Rows.Count) && (UT_TRADEFEEDS_Count > 0))
                {
                    Helper.WriteLogXml("All record already present in tabls");
                    IsAllPresent = true;
                }
                else if (UT_TRADEFEEDS_Count > 0 || UT_REPOFIELDS_Count > 0)
                {
                    if (!IsAllPresent)
                    {
                        Helper.WriteLogXml(" ====================> Start partial deletion");
                        foreach (string Oratable in strTable)
                        {
                            using (conn = new NpgsqlConnection(strCon))
                            {
                                conn.Open();
                                using (NpgsqlCommand Delcmd = new NpgsqlCommand("DELETE FROM  " + Oratable + " WHERE  FILENAME='" + strFileNM + "'", conn))
                                {
                                    if (conn.State != ConnectionState.Open)
                                    {
                                        conn.Open();
                                    }
                                    Delcmd.ExecuteNonQuery();
                                    Helper.WriteLogXml("Deleted data File Name :[" + strFileNM + " ] Table Name :" + Oratable);
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception exx)
            {
                Console.WriteLine("Error in function : \n" + exx.Message);
                Helper.WriteLogXml("Error in function : \n" + exx.Message);
            }
            return IsAllPresent;
        }

    }
}

