using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIFSLNSEEmailUtility
{
    class InsertData
    {
        string ItemDecimal = "##############################.##";
        int RowCount = 0;

        string INDEX_NAME = string.Empty;
        DateTime DATE;
        string MKT = string.Empty;
        string SERIES = string.Empty;
        string SYMBOL = string.Empty;
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
        double INDEXED_UNITS;
        double INVESTIBLE_FACTOR;
        double CAP_FACTOR;
        double ADJ_CLOSE_PRICE;
        double INDEX_MKT_CAP;
        double WEIGHTAGE;
        DateTime INP_DT_TM;

        public void InsertDB(DataTable dtReadCSV, string TableType)
        {
            DateTime a = Convert.ToDateTime("1/1/0001");
            RowCount = 0;
            try
            {
                DataColumnCollection columns = dtReadCSV.Columns;
                for (int j = 0; j < dtReadCSV.Rows.Count; j++)
                {
                    INDEX_NAME = dtReadCSV.Rows[j]["INDEX_NAME"].ToString();
                    DATE = Convert.ToDateTime(dtReadCSV.Rows[j]["DATE"].ToString(), System.Globalization.CultureInfo.GetCultureInfo("ur-PK").DateTimeFormat);
                    MKT = dtReadCSV.Rows[j]["MKT"].ToString();
                    SERIES = dtReadCSV.Rows[j]["SERIES"].ToString();
                    SYMBOL = dtReadCSV.Rows[j]["SYMBOL"].ToString();
                    ISIN = dtReadCSV.Rows[j]["ISIN"].ToString();
                    SECURITY_NAME = dtReadCSV.Rows[j]["SECURITY_NAME"].ToString();
                    BASIC_INDUSTRY = dtReadCSV.Rows[j]["BASIC_INDUSTRY"].ToString();
                    PREV_CL_PR = Convert.ToDouble(dtReadCSV.Rows[j]["PREV_CL_PR"]);
                    OPEN_PRICE = Convert.ToDouble(dtReadCSV.Rows[j]["OPEN_PRICE"]);
                    HIGH_PRICE = Convert.ToDouble(dtReadCSV.Rows[j]["HIGH_PRICE"]);
                    LOW_PRICE = Convert.ToDouble(dtReadCSV.Rows[j]["LOW_PRICE"]);
                    CLOSE_PRICE = Convert.ToDouble(dtReadCSV.Rows[j]["CLOSE_PRICE"]);
                    NET_TRDVAL = Convert.ToDouble(dtReadCSV.Rows[j]["NET_TRDVAL"]);
                    NET_TRDQTY = Convert.ToDouble(dtReadCSV.Rows[j]["NET_TRDQTY"]);
                    IND_SEC = dtReadCSV.Rows[j]["IND_SEC"].ToString();
                    CORP_IND = dtReadCSV.Rows[j]["CORP_IND"].ToString();
                    TRADES = Convert.ToDouble(dtReadCSV.Rows[j]["TRADES"]);
                    HI_52_WK = Convert.ToDouble(dtReadCSV.Rows[j]["HI_52_WK"]);
                    LO_52_WK = Convert.ToDouble(dtReadCSV.Rows[j]["LO_52_WK"]);

                    if (columns.Contains("ISSUE_CAP"))
                    {
                        ISSUE_CAP = Convert.ToDouble(dtReadCSV.Rows[j]["ISSUE_CAP"]);
                    }
                    else
                    {
                        ISSUE_CAP = 0;
                    }
                    if (columns.Contains("INDEXED_UNITS"))
                    {
                        INDEXED_UNITS = Convert.ToDouble(dtReadCSV.Rows[j]["INDEXED_UNITS"]);
                    }
                    else
                    {
                        INDEXED_UNITS = 0;
                    }
                    if (dtReadCSV.Rows[j]["INVESTIBLE_FACTOR"] is string && dtReadCSV.Rows[j]["INVESTIBLE_FACTOR"].ToString().Contains("-"))
                    {
                        INVESTIBLE_FACTOR = 0;
                    }
                    else
                    {
                        INVESTIBLE_FACTOR = Convert.ToDouble(dtReadCSV.Rows[j]["INVESTIBLE_FACTOR"]);
                    }



                    CAP_FACTOR = Convert.ToDouble(dtReadCSV.Rows[j]["CAP_FACTOR"]);
                    ADJ_CLOSE_PRICE = Convert.ToDouble(dtReadCSV.Rows[j]["ADJ_CLOSE_PRICE"]);
                    INDEX_MKT_CAP = Convert.ToDouble(dtReadCSV.Rows[j]["INDEX_MKT_CAP"]);
                    WEIGHTAGE = Convert.ToDouble(dtReadCSV.Rows[j]["WEIGHTAGE"]);

                    InsertCSVFilesData(TableType, INDEX_NAME, DATE, MKT, SERIES, SYMBOL, ISIN, SECURITY_NAME, BASIC_INDUSTRY, PREV_CL_PR, OPEN_PRICE, HIGH_PRICE, LOW_PRICE, CLOSE_PRICE,
                       NET_TRDVAL, NET_TRDQTY, IND_SEC, CORP_IND, TRADES, HI_52_WK, LO_52_WK, ISSUE_CAP, INDEXED_UNITS, INVESTIBLE_FACTOR, CAP_FACTOR, ADJ_CLOSE_PRICE,
                       INDEX_MKT_CAP, WEIGHTAGE);
                }
                Helper.LogError("Current File Saved in database row affected : " + RowCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ReadCSVFile Failed. when inserting a data" + ex.Message);
                Helper.LogError("Insert DB failed. when inserting a data" + ex.ToString());
            }
        }


        //anshu
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
        //anshu
        public void InsertCSVFilesData(string TableName, string INDEX_NAME, DateTime DATE, string MKT, string SERIES, string SYMBOL, string ISIN,
           string SECURITY_NAME, string BASIC_INDUSTRY, double PREV_CL_PR, double OPEN_PRICE, double HIGH_PRICE, double LOW_PRICE,
           double CLOSE_PRICE, double NET_TRDVAL, double NET_TRDQTY, string IND_SEC, string CORP_IND, double TRADES, double HI_52_WK, double LO_52_WK,
           double ISSUE_CAP, double INDEXED_UNITS, double INVESTIBLE_FACTOR, double CAP_FACTOR, double ADJ_CLOSE_PRICE, double INDEX_MKT_CAP, double WEIGHTAGE)
        {

            String cmdQuery = "";
            string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();


            try
            {
                if (TableName == "pr")
                {
                    cmdQuery = "INSERT INTO UT_NSEWTEOD (INDEX_NAME ,VALUE_DATE, ISIN ,MKT ,SERIES_NAME,SYMBOL_NAME ,SECURITY_NAME,BASIC_INDUSTRY,PREV_CL_PR, OPEN_PRICE, HIGH_PRICE, LOW_PRICE, CLOSE_PRICE, NET_TRDVAL, NET_TRDATY, IND_SEC, CORP_IND, TRADES, HI_52_WK, LO_52_WK, ISSUE_CAP, INDEXED_UNITS, INVESTIBLE_FACTTOR, CAP_FACTOR, ADJ_CLOSE_PRICE, INDEX_MKT_CAP, WEIGHTAGE ,INP_DT_TM )" +
                  "VALUES(:PINDEX_NAME,:PVALUE_DATE,:PISIN,:PMKT,:PSERIES_NAME,:PSYMBOL_NAME ,:PSECURITY_NAME ,:PBASIC_INDUSTRY, :PPREV_CL_PR, :POPEN_PRICE , :PHIGH_PRICE , :PLOW_PRICE , :PCLOSE_PRICE, :PNET_TRDVAL, :PNET_TRDATY, :PIND_SEC, :PCORP_IND, :PTRADES, :PHI_52_WK, :PLO_52_WK, :PISSUE_CAP,:PINDEXED_UNITS, :PINVESTIBLE_FACTTOR, :PCAP_FACTOR, :PADJ_CLOSE_PRICE, :PINDEX_MKT_CAP, :PWEIGHTAGE ,:PINP_DT_TM)";


                    Console.WriteLine("table name :" + TableName);
                    DataTable dt = getDataTable("select * from  UT_NSEWTEOD where INDEX_NAME='" + INDEX_NAME + "' AND VALUE_DATE = '" + DATE + "'AND ISIN = '" + ISIN + "'");

                    if (dt.Rows.Count < 1)
                    {
                        using (var connection = new NpgsqlConnection(ConnectionString))
                        {
                            connection.Open();
                            using (NpgsqlCommand cmd = new NpgsqlCommand(cmdQuery, connection))
                            {

                                cmd.Parameters.AddWithValue("PINDEX_NAME", INDEX_NAME);
                                cmd.Parameters.AddWithValue("PVALUE_DATE", DATE);
                                cmd.Parameters.AddWithValue("PMKT", MKT);
                                cmd.Parameters.AddWithValue("PSERIES_NAME", SERIES);
                                cmd.Parameters.AddWithValue("PSYMBOL_NAME", SYMBOL);
                                cmd.Parameters.AddWithValue("PISIN", ISIN);
                                cmd.Parameters.AddWithValue("PSECURITY_NAME", SECURITY_NAME);
                                cmd.Parameters.AddWithValue("PBASIC_INDUSTRY", BASIC_INDUSTRY);
                                cmd.Parameters.AddWithValue("PPREV_CL_PR", PREV_CL_PR);
                                cmd.Parameters.AddWithValue("POPEN_PRICE", OPEN_PRICE);
                                cmd.Parameters.AddWithValue("PHIGH_PRICE", HIGH_PRICE);
                                cmd.Parameters.AddWithValue("PLOW_PRICE", LOW_PRICE);
                                cmd.Parameters.AddWithValue("PCLOSE_PRICE", CLOSE_PRICE);
                                cmd.Parameters.AddWithValue("PNET_TRDVAL", NET_TRDVAL);
                                cmd.Parameters.AddWithValue("PNET_TRDATY", NET_TRDQTY);
                                cmd.Parameters.AddWithValue("PIND_SEC", IND_SEC);
                                cmd.Parameters.AddWithValue("PCORP_IND", CORP_IND);
                                cmd.Parameters.AddWithValue("PTRADES", TRADES);
                                cmd.Parameters.AddWithValue("PHI_52_WK", HI_52_WK);
                                cmd.Parameters.AddWithValue("PLO_52_WK", LO_52_WK);
                                cmd.Parameters.AddWithValue("PISSUE_CAP", ISSUE_CAP);
                                cmd.Parameters.AddWithValue("PINDEXED_UNITS", INDEXED_UNITS);
                                cmd.Parameters.AddWithValue("PINVESTIBLE_FACTTOR", INVESTIBLE_FACTOR);
                                cmd.Parameters.AddWithValue("PCAP_FACTOR", CAP_FACTOR);
                                cmd.Parameters.AddWithValue("PADJ_CLOSE_PRICE", ADJ_CLOSE_PRICE);
                                cmd.Parameters.AddWithValue("PINDEX_MKT_CAP", INDEX_MKT_CAP);
                                cmd.Parameters.AddWithValue("PWEIGHTAGE", WEIGHTAGE);
                                cmd.Parameters.AddWithValue("PINP_DT_TM", DateTime.Now);
                                cmd.ExecuteNonQuery();
                                Console.WriteLine("Data successfully save");
                            }
                        }
                    }


                    else
                    {
                        Console.WriteLine("Record Already Present !");
                    }

                }

                else
                {
                    cmdQuery = "INSERT INTO UT_NSEWTBOD (INDEX_NAME ,VALUE_DATE, ISIN ,MKT ,SERIES_NAME,SYMBOL_NAME ,SECURITY_NAME,BASIC_INDUSTRY,PREV_CL_PR, OPEN_PRICE, HIGH_PRICE, LOW_PRICE, CLOSE_PRICE, NET_TRDVAL, NET_TRDATY, IND_SEC, CORP_IND, TRADES, HI_52_WK, LO_52_WK, ISSUE_CAP, INDEXED_UNITS, INVESTIBLE_FACTTOR, CAP_FACTOR, ADJ_CLOSE_PRICE, INDEX_MKT_CAP, WEIGHTAGE ,INP_DT_TM )" +
                   "VALUES(:PINDEX_NAME,:PVALUE_DATE,:PISIN,:PMKT,:PSERIES_NAME,:PSYMBOL_NAME ,:PSECURITY_NAME ,:PBASIC_INDUSTRY, :PPREV_CL_PR, :POPEN_PRICE , :PHIGH_PRICE , :PLOW_PRICE , :PCLOSE_PRICE, :PNET_TRDVAL, :PNET_TRDATY, :PIND_SEC, :PCORP_IND, :PTRADES, :PHI_52_WK, :PLO_52_WK, :PISSUE_CAP,:PINDEXED_UNITS, :PINVESTIBLE_FACTTOR, :PCAP_FACTOR, :PADJ_CLOSE_PRICE, :PINDEX_MKT_CAP, :PWEIGHTAGE ,:PINP_DT_TM)";



                    DataTable dt = getDataTable("select * from UT_NSEWTBOD where INDEX_NAME='" + INDEX_NAME + "' AND VALUE_DATE = '" + DATE + "'AND ISIN = '" + ISIN + "'");


                    if (dt.Rows.Count < 1)
                    {
                        using (var connection = new NpgsqlConnection(ConnectionString))
                        {
                            connection.Open();
                            using (NpgsqlCommand cmd = new NpgsqlCommand(cmdQuery, connection))
                            {

                                cmd.Parameters.AddWithValue("PINDEX_NAME", INDEX_NAME);
                                cmd.Parameters.AddWithValue("PVALUE_DATE", DATE);
                                cmd.Parameters.AddWithValue("PMKT", MKT);
                                cmd.Parameters.AddWithValue("PSERIES_NAME", SERIES);
                                cmd.Parameters.AddWithValue("PSYMBOL_NAME", SYMBOL);
                                cmd.Parameters.AddWithValue("PISIN", ISIN);
                                cmd.Parameters.AddWithValue("PSECURITY_NAME", SECURITY_NAME);
                                cmd.Parameters.AddWithValue("PBASIC_INDUSTRY", BASIC_INDUSTRY);
                                cmd.Parameters.AddWithValue("PPREV_CL_PR", PREV_CL_PR);
                                cmd.Parameters.AddWithValue("POPEN_PRICE", OPEN_PRICE);
                                cmd.Parameters.AddWithValue("PHIGH_PRICE", HIGH_PRICE);
                                cmd.Parameters.AddWithValue("PLOW_PRICE", LOW_PRICE);
                                cmd.Parameters.AddWithValue("PCLOSE_PRICE", CLOSE_PRICE);
                                cmd.Parameters.AddWithValue("PNET_TRDVAL", NET_TRDVAL);
                                cmd.Parameters.AddWithValue("PNET_TRDATY", NET_TRDQTY);
                                cmd.Parameters.AddWithValue("PIND_SEC", IND_SEC);
                                cmd.Parameters.AddWithValue("PCORP_IND", CORP_IND);
                                cmd.Parameters.AddWithValue("PTRADES", TRADES);
                                cmd.Parameters.AddWithValue("PHI_52_WK", HI_52_WK);
                                cmd.Parameters.AddWithValue("PLO_52_WK", LO_52_WK);
                                cmd.Parameters.AddWithValue("PISSUE_CAP", ISSUE_CAP);
                                cmd.Parameters.AddWithValue("PINDEXED_UNITS", INDEXED_UNITS);
                                cmd.Parameters.AddWithValue("PINVESTIBLE_FACTTOR", INVESTIBLE_FACTOR);
                                cmd.Parameters.AddWithValue("PCAP_FACTOR", CAP_FACTOR);
                                cmd.Parameters.AddWithValue("PADJ_CLOSE_PRICE", ADJ_CLOSE_PRICE);
                                cmd.Parameters.AddWithValue("PINDEX_MKT_CAP", INDEX_MKT_CAP);
                                cmd.Parameters.AddWithValue("PWEIGHTAGE", WEIGHTAGE);
                                cmd.Parameters.AddWithValue("PINP_DT_TM", DateTime.Now);
                                cmd.ExecuteNonQuery();
                                Console.WriteLine("Data successfully save");
                            }
                        }
                    }

                    else
                    {
                        Console.WriteLine("Record Already Present !");
                    }
                }

                
            }
            catch (Exception ex)
            {
                Helper.LogError("Insert CSV Files Data: " + ex.ToString());
            }
        }
    }
}













