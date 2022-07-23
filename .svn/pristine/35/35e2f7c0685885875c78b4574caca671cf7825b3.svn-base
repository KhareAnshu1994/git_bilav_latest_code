using Npgsql;
using Oracle.DataAccess.Client;
using System;
using System.Configuration;
using System.Data;

namespace BilavNSE_WeightageUtility
{
    public class InsertData
    {
        string INDEX_NAME = string.Empty;
        DateTime DATE;
        string MKT = string.Empty;
        string SERIES_NAME = string.Empty;
        string SYMBOL_NAME = string.Empty;
        string INDEXED_UNITS = string.Empty;
        string INP_DT_TM = string.Empty;
        string ISIN = string.Empty;
        string SECURITY_NAME = string.Empty;
        string BASIC_INDUSTRY = string.Empty;
        double PREV_CL_PR;
        double OPEN_PRICE;
        double HIGH_PRICE;
        double LOW_PRICE;
        double CLOSE_PRICE;
        double NET_TRDVAL;
        double NET_TRDQTY;
        string IND_SEC = string.Empty;
        string CORP_IND = string.Empty;
        double TRADES;
        double HI_52_WK;
        double LO_52_WK;
        double ISSUE_CAP;
        double INVESTIBLE_FACTOR;
        double CAP_FACTOR;
        double ADJ_CLOSE_PRICE;
        double INDEX_MKT_CAP;
        double WEIGHTAGE;
        int RowCount = 0;

        public void InsertXLSXDB(DataTable dtReadXLSX, string TableType, string filename)
        {
            DateTime a = Convert.ToDateTime("1/1/0001");
            RowCount = 0;

            dtReadXLSX.Rows.RemoveAt(dtReadXLSX.Rows.Count - 1);
            try
            {


                for (int i = 0; i < dtReadXLSX.Rows.Count; i++)
                {

                    try
                    {
                        INDEX_NAME = string.IsNullOrEmpty(dtReadXLSX.Rows[i][0].ToString()) ? null : dtReadXLSX.Rows[i][0].ToString();

                        string datestring1 = DateTime.Now.Date.ToString("dd/MM/yyyy");
                        DATE = Convert.ToDateTime(datestring1, System.Globalization.CultureInfo.GetCultureInfo("ur-PK").DateTimeFormat);
                        
                        SYMBOL_NAME = string.IsNullOrEmpty(dtReadXLSX.Rows[i][1].ToString()) ? null : dtReadXLSX.Rows[i][1].ToString();
                        BASIC_INDUSTRY = string.IsNullOrEmpty(dtReadXLSX.Rows[i][2].ToString()) ? null : dtReadXLSX.Rows[i][2].ToString();
                      

                        if (filename.ToUpper().Contains("UTI_INFRASTRUCTURE_INDEX"))
                        {
                            if (filename.Substring(0, 24) == "UTI_Infrastructure_Index")
                            {

                                if (dtReadXLSX.Rows[0][3].ToString() == "Closing Price")
                                {
                                    CLOSE_PRICE = Convert.ToDouble(dtReadXLSX.Rows[i][3]) == 0.00 ? 0.00 : Convert.ToDouble(dtReadXLSX.Rows[i][3].ToString());
                                }
                                if (dtReadXLSX.Rows[0][4].ToString() == "Share Cum")
                                {
                                    ISSUE_CAP = Convert.ToDouble(dtReadXLSX.Rows[i][4]) == 0.00 ? 0.00 : Convert.ToDouble(dtReadXLSX.Rows[i][4]);
                                }
                                if (dtReadXLSX.Rows[0][5].ToString() == "Investible Factor")
                                {
                                    INVESTIBLE_FACTOR = Convert.ToDouble(dtReadXLSX.Rows[i][5]) == 0.00 ? 0.00 : Convert.ToDouble(dtReadXLSX.Rows[i][5]);
                                }
                                if (dtReadXLSX.Rows[0][6].ToString() == "Capping Factor")
                                {
                                    CAP_FACTOR = Convert.ToDouble(dtReadXLSX.Rows[i][6]) == 0.00 ? 0.00 : Convert.ToDouble(dtReadXLSX.Rows[i][6]);
                                }
                                INDEX_MKT_CAP = Convert.ToDouble(dtReadXLSX.Rows[i][7].ToString()) == 0.00 ? 0.00 : Convert.ToDouble(dtReadXLSX.Rows[i][7].ToString());
                                WEIGHTAGE = Convert.ToDouble(dtReadXLSX.Rows[i][8].ToString()) == 0.00 ? 0.00 : Convert.ToDouble(dtReadXLSX.Rows[i][8].ToString());
                                ISIN = string.IsNullOrEmpty(dtReadXLSX.Rows[i][9].ToString()) ? null : dtReadXLSX.Rows[i][9].ToString();


                            }
                        }
                        
                        if (filename.ToUpper().Contains("UTI_TRANSPORT_&_LOGISTIC"))
                        {
                            if (filename.Substring(0, 24) == "UTI_TRANSPORT_&_LOGISTIC")
                            {
                                INDEX_MKT_CAP = Convert.ToDouble(dtReadXLSX.Rows[i][3]) == 0.00 ? 0.00 : Convert.ToDouble(dtReadXLSX.Rows[i][3]);
                                WEIGHTAGE = Convert.ToDouble(dtReadXLSX.Rows[i][4]) == 0.00 ? 0.00 : Convert.ToDouble(dtReadXLSX.Rows[i][4]);
                                ISIN = string.IsNullOrEmpty(dtReadXLSX.Rows[i][5].ToString()) ? null : dtReadXLSX.Rows[i][5].ToString();
                            }
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("While trying to insert Exception as : " + ex.Message);
                    }

                    InsertXLSXFilesData(filename, TableType, INDEX_NAME, DATE, SYMBOL_NAME, BASIC_INDUSTRY, CLOSE_PRICE, ISSUE_CAP, INVESTIBLE_FACTOR, CAP_FACTOR, INDEX_MKT_CAP, WEIGHTAGE, ISIN , INDEXED_UNITS, INP_DT_TM);
                }
                Helper.LogError("Total Insert count :" + RowCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ReadXLSXFile Failed. when inserting a data :" + ex.Message);
            }

        }
        public DataTable getDataTable(string SelectQuery)
        {
            DataTable dt = new DataTable();
            string connString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            try
            {
                using (var connection = new NpgsqlConnection(connString))
                {
                    connection.Open();
                    
                    Console.WriteLine("Connection opened");
                    using (NpgsqlCommand selectcmd = new NpgsqlCommand(SelectQuery, connection))
                    {
                        selectcmd.Parameters.AddWithValue("PINDEX_NAME", INDEX_NAME);
                        selectcmd.Parameters.AddWithValue("PVALUE_DATE", DATE);
                        selectcmd.Parameters.AddWithValue("PIsin", ISIN);
                        using (NpgsqlDataAdapter orclda = new NpgsqlDataAdapter(selectcmd))
                        {
                            orclda.Fill(dt);
                            Console.WriteLine("dt count :" + dt.Rows.Count);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in getDatatable data :" + ex.Message);
            }
            return dt;
        }
        public void InsertXLSXFilesData(string filename, string TableName, string INDEX_NAME, DateTime DATE, string SYMBOL, string BASIC_INDUSTRY, double CLOSE_PRICE,
         double ISSUE_CAP, double INVESTIBLE_FACTOR, double CAP_FACTOR, double INDEX_MKT_CAP, double WEIGHTAGE, string ISIN ,string INDEXED_UNITS ,string PINP_DT_TM)
        {
            filename = filename.Substring(0, filename.Length - 13); //8 digits for date 1 for . and 4 for extension(xlsx) so total 13
            string index_nm = INDEX_NAME;
            string MKT = "N";
            string SERIES = "EQ";
            PREV_CL_PR = 0.00;
            double NET_TRDATY = 0.00;
            IND_SEC = "Y";
            TRADES = 0.00;
            String cmdQuery = "";
            string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            try
            {
                DateTime time = DateTime.Now;

               

                    DataTable dt = getDataTable("select * from  UT_NSEWTBOD where INDEX_NAME='" + INDEX_NAME + "' AND VALUE_DATE = '" + DATE + "'AND ISIN = '" + ISIN + "'");
                    // DataTable dt = getDataTable(GetQuery.SELECT_QUERY_UT_NSEWTBOD);
                    if (dt.Rows.Count < 1)
                    {
                        using (var connection = new NpgsqlConnection(ConnectionString))
                        {
                            connection.Open();
                            using (NpgsqlCommand cmd = new NpgsqlCommand(GetQuery.UT_NSEWTBOD_INSERT_CMD, connection))
                            {
                                cmd.Parameters.AddWithValue("PINDEX_NAME", INDEX_NAME);
                                cmd.Parameters.AddWithValue("PVALUE_DATE", DATE);
                                cmd.Parameters.AddWithValue("PMKT", MKT);
                                cmd.Parameters.AddWithValue("PSERIES_NAME", SERIES_NAME);
                                cmd.Parameters.AddWithValue("PSYMBOL_NAME", SYMBOL_NAME);
                                cmd.Parameters.AddWithValue("PISIN", ISIN);
                                cmd.Parameters.AddWithValue("PSECURITY_NAME", SECURITY_NAME);
                                cmd.Parameters.AddWithValue("PBASIC_INDUSTRY", BASIC_INDUSTRY);
                                cmd.Parameters.AddWithValue("PPREV_CL_PR", PREV_CL_PR);
                                cmd.Parameters.AddWithValue("POPEN_PRICE", OPEN_PRICE);
                                cmd.Parameters.AddWithValue("PHIGH_PRICE", HIGH_PRICE);
                                cmd.Parameters.AddWithValue("PLOW_PRICE", LOW_PRICE);
                                cmd.Parameters.AddWithValue("PCLOSE_PRICE", CLOSE_PRICE);
                                cmd.Parameters.AddWithValue("PNET_TRDVAL", NET_TRDVAL);
                                cmd.Parameters.AddWithValue("PNET_TRDATY", NET_TRDATY);
                                cmd.Parameters.AddWithValue("PIND_SEC", IND_SEC);
                                cmd.Parameters.AddWithValue("PCORP_IND", CORP_IND);
                                cmd.Parameters.AddWithValue("PTRADES", TRADES);
                                cmd.Parameters.AddWithValue("PHI_52_WK", HI_52_WK);
                                cmd.Parameters.AddWithValue("PLO_52_WK", LO_52_WK);
                                cmd.Parameters.AddWithValue("PISSUE_CAP", ISSUE_CAP);
                                cmd.Parameters.AddWithValue("PINVESTIBLE_FACTTOR", INVESTIBLE_FACTOR);
                                cmd.Parameters.AddWithValue("PCAP_FACTOR", CAP_FACTOR);
                                cmd.Parameters.AddWithValue("PADJ_CLOSE_PRICE", ADJ_CLOSE_PRICE);
                                cmd.Parameters.AddWithValue("PINDEX_MKT_CAP", INDEX_MKT_CAP);
                                cmd.Parameters.AddWithValue("WEIGHTAGE", WEIGHTAGE);
                                cmd.Parameters.AddWithValue("PINDEXED_UNITS", INDEXED_UNITS);
                                cmd.Parameters.AddWithValue("PINP_DT_TM", DateTime.Now);
                                cmd.ExecuteNonQuery();
                            Console.WriteLine("data successfully saved");
                        }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Record Already Present !");
                    }
                
               
                   

                dt = new DataTable();
                Console.WriteLine("table :UT_NSEWTEOD , IndexName :" + filename + " , SECURITY_NAME:" + INDEX_NAME);


               

                    dt = getDataTable("select * from UT_NSEWTEOD where INDEX_NAME='" + INDEX_NAME + "' AND VALUE_DATE = '" + DATE + "'AND ISIN = '" + ISIN + "'");
               // dt = getDataTable(GetQuery.SELECT_QUERY_UT_NSEWTBOD);
                if (dt.Rows.Count < 1)
                {
                    using (var connection = new NpgsqlConnection(ConnectionString))
                    {
                        connection.Open();
                        using (NpgsqlCommand cmd2 = new NpgsqlCommand(GetQuery.UT_NSEWTEOD_INSERT_CMD, connection))
                        {
                            cmd2.Parameters.AddWithValue("PINDEX_NAME", INDEX_NAME);
                            cmd2.Parameters.AddWithValue("PVALUE_DATE", DATE);
                            cmd2.Parameters.AddWithValue("PMKT", MKT);//Convert.ToInt32(MKT)
                            cmd2.Parameters.AddWithValue("PSERIES_NAME", SERIES_NAME);
                            cmd2.Parameters.AddWithValue("PSYMBOL_NAME", SYMBOL_NAME);
                            cmd2.Parameters.AddWithValue("PISIN", ISIN);
                            cmd2.Parameters.AddWithValue("PSECURITY_NAME", SECURITY_NAME);
                            cmd2.Parameters.AddWithValue("PBASIC_INDUSTRY", BASIC_INDUSTRY);
                            cmd2.Parameters.AddWithValue("PPREV_CL_PR", PREV_CL_PR);
                            cmd2.Parameters.AddWithValue("POPEN_PRICE", OPEN_PRICE);
                            cmd2.Parameters.AddWithValue("PHIGH_PRICE", HIGH_PRICE);
                            cmd2.Parameters.AddWithValue("PLOW_PRICE", LOW_PRICE);
                            cmd2.Parameters.AddWithValue("PCLOSE_PRICE", CLOSE_PRICE);
                            cmd2.Parameters.AddWithValue("PNET_TRDVAL", NET_TRDVAL);
                            cmd2.Parameters.AddWithValue("PNET_TRDATY", NET_TRDATY);
                            cmd2.Parameters.AddWithValue("PIND_SEC", IND_SEC);
                            cmd2.Parameters.AddWithValue("PCORP_IND", CORP_IND);
                            cmd2.Parameters.AddWithValue("PTRADES", TRADES);
                            cmd2.Parameters.AddWithValue("PHI_52_WK", HI_52_WK);
                            cmd2.Parameters.AddWithValue("PLO_52_WK", LO_52_WK);
                            cmd2.Parameters.AddWithValue("PISSUE_CAP", ISSUE_CAP);
                            cmd2.Parameters.AddWithValue("PINVESTIBLE_FACTTOR", INVESTIBLE_FACTOR);
                            cmd2.Parameters.AddWithValue("PCAP_FACTOR", CAP_FACTOR);
                            cmd2.Parameters.AddWithValue("PADJ_CLOSE_PRICE", ADJ_CLOSE_PRICE);
                            cmd2.Parameters.AddWithValue("PINDEX_MKT_CAP", INDEX_MKT_CAP);
                            cmd2.Parameters.AddWithValue("WEIGHTAGE", WEIGHTAGE);
                            cmd2.Parameters.AddWithValue("PINDEXED_UNITS", INDEXED_UNITS);
                            cmd2.Parameters.AddWithValue("PINP_DT_TM", DateTime.Now);
                            cmd2.ExecuteNonQuery();
                            Console.WriteLine("data  successfully saved");
                        }
                         connection.Close();
                    }
                }
                else
                {
                    Console.WriteLine("Record Already Present !");
                }
            }
            catch (Exception ex)
            {
                Helper.LogError("error 2:" + ex.Message);
                
            }
        }
    }
}


