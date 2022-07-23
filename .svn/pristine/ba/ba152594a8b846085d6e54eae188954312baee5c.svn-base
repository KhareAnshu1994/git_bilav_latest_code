using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using System.Data;
using System.Diagnostics;
using Npgsql;

namespace IIFSLBSEReaderUtility.Classes
{
    public class Common
    {

        string FileSourcePath = ConfigurationManager.AppSettings["FileSourcePath"].ToString();
        string INDEX_NAME = string.Empty;
        string INDEX_CODE = string.Empty;
        string INDEX_KEY = string.Empty;
        string EFFECTIVE_DATE = string.Empty;
        string COMPANY_NAME = string.Empty;
        string RIC = string.Empty;
        string BLOOMBERG_TICKER = string.Empty;
        string CUSIP = string.Empty;
        string ISIN = string.Empty;
        string SEDOL = string.Empty;
        string TICKER = string.Empty;
        string GV_KEY = string.Empty;
        string STOCK_KEY = string.Empty;
        string GICS_CODE = string.Empty;
        //string NET_TRDQTY = string.Empty;
        string DJI_IND_CODE = string.Empty;
        string ALTERNATE_CLASS_CODE = string.Empty;
        string MIC = string.Empty;
        string COUNTRY_OF_DOMICILE = string.Empty;
        string COUNTRY_OF_LISTING = string.Empty;
        string REGION = string.Empty;
        string SIZE = string.Empty;
        string CAP_RANGE = string.Empty;
        string CURRENCY_CODE = string.Empty;
        decimal LOCAL_PRICE;
        string FX_RATE = string.Empty;
        Int64 SHARES_OUTSTANDING;
        decimal MARKET_CAP;
        decimal IWF;
        Int64 AWF;
        string GROWTH = string.Empty;
        string VALUE = string.Empty;
        decimal INDEX_SHARES;
        decimal INDEX_MARKET_CAP;
        decimal INDEX_WEIGHT;
        decimal DAILY_PRICE_RETURN;
        decimal DAILY_TOTAL_RETURN;
        decimal DIVIDEND;
        decimal NET_DIVIDEND;
        string INP_DT_TM = string.Empty;
        double DefualtDailyPriceTotalReturn = 0000000000000000.000000000000;
        double DefualtDividendNet = 0000000000000000.00;
        int RowCount;
        string ItemDecimal = "################.##";
        public bool InsertDB(DataTable dtReadSDC, string InsertTableName)
        {
            try
            {
                RowCount = 0;
                for (int j = 0; j < dtReadSDC.Rows.Count; j++)
                {
                    INDEX_NAME = dtReadSDC.Rows[j]["INDEX NAME"].ToString();
                    INDEX_CODE = dtReadSDC.Rows[j]["INDEX CODE"].ToString();
                    INDEX_KEY = dtReadSDC.Rows[j]["INDEX KEY"].ToString();
                    EFFECTIVE_DATE = dtReadSDC.Rows[j]["EFFECTIVE DATE"].ToString();
                    COMPANY_NAME = dtReadSDC.Rows[j]["COMPANY"].ToString();
                    RIC = dtReadSDC.Rows[j]["RIC"].ToString();
                    BLOOMBERG_TICKER = dtReadSDC.Rows[j]["BLOOMBERG TICKER"].ToString();
                    CUSIP = dtReadSDC.Rows[j]["CUSIP"].ToString();
                    ISIN = dtReadSDC.Rows[j]["ISIN"].ToString();
                    SEDOL = dtReadSDC.Rows[j]["SEDOL"].ToString();
                    TICKER = dtReadSDC.Rows[j]["TICKER"].ToString();
                    GV_KEY = dtReadSDC.Rows[j]["GV KEY"].ToString();
                    STOCK_KEY = dtReadSDC.Rows[j]["STOCK KEY"].ToString();
                    GICS_CODE = dtReadSDC.Rows[j]["GICS CODE"].ToString();
                    DJI_IND_CODE = dtReadSDC.Rows[j]["DJI INDUSTRY CODE"].ToString();
                    ALTERNATE_CLASS_CODE = dtReadSDC.Rows[j]["ALTERNATE CLASSIFICATION CODE"].ToString();
                    MIC = dtReadSDC.Rows[j]["MIC"].ToString();
                    COUNTRY_OF_DOMICILE = dtReadSDC.Rows[j]["COUNTRY OF DOMICILE"].ToString();
                    COUNTRY_OF_LISTING = dtReadSDC.Rows[j]["COUNTRY OF LISTING"].ToString();
                    REGION = dtReadSDC.Rows[j]["REGION"].ToString();
                    SIZE = dtReadSDC.Rows[j]["SIZE"].ToString();
                    CAP_RANGE = dtReadSDC.Rows[j]["CAP RANGE"].ToString();
                    CURRENCY_CODE = dtReadSDC.Rows[j]["CURRENCY CODE"].ToString();
                    LOCAL_PRICE = Convert.ToDecimal(dtReadSDC.Rows[j]["LOCAL PRICE"]);
                    FX_RATE = dtReadSDC.Rows[j]["FX RATE"].ToString();

                    SHARES_OUTSTANDING = Convert.ToInt64(dtReadSDC.Rows[j]["SHARES OUTSTANDING"]);
                    MARKET_CAP = Convert.ToDecimal(dtReadSDC.Rows[j]["MARKET CAP"]);
                    IWF = Convert.ToDecimal(dtReadSDC.Rows[j]["IWF"]);
                    AWF = Convert.ToInt64(dtReadSDC.Rows[j]["AWF"]);
                    GROWTH = dtReadSDC.Rows[j]["GROWTH"].ToString();
                    VALUE = dtReadSDC.Rows[j]["VALUE"].ToString();
                    INDEX_SHARES = Convert.ToDecimal(dtReadSDC.Rows[j]["INDEX SHARES"]);
                    INDEX_MARKET_CAP = Convert.ToDecimal(dtReadSDC.Rows[j]["INDEX MARKET CAP"]);

                    //LogError("DAILY PRICE RETURN : DAILY_PRICE_RETURN = " +dtReadSDC.Rows[j]["DAILY PRICE RETURN"].ToString() == "" ? 0000000000000000.00 : Convert.ToDouble(dtReadSDC.Rows[j]["DAILY PRICE RETURN"]), "ReadSDCFile");

                    INDEX_WEIGHT = Convert.ToDecimal(dtReadSDC.Rows[j]["INDEX WEIGHT"]);
                    DAILY_PRICE_RETURN = dtReadSDC.Rows[j]["DAILY PRICE RETURN"].ToString() == "" ? Convert.ToDecimal(String.Format("{0:0000000000000000.000000000000}", DefualtDailyPriceTotalReturn)) : Convert.ToDecimal(dtReadSDC.Rows[j]["DAILY PRICE RETURN"]);
                    DAILY_TOTAL_RETURN = dtReadSDC.Rows[j]["DAILY TOTAL RETURN"].ToString() == "" ? Convert.ToDecimal(String.Format("{0:0000000000000000.000000000000}", DefualtDailyPriceTotalReturn)) : Convert.ToDecimal(dtReadSDC.Rows[j]["DAILY TOTAL RETURN"]);
                    DIVIDEND = dtReadSDC.Rows[j]["DIVIDEND"].ToString() == "" ? Convert.ToDecimal(String.Format("{0:0000000000000000.00}", DefualtDividendNet)) : Convert.ToDecimal(dtReadSDC.Rows[j]["DIVIDEND"]);
                    NET_DIVIDEND = dtReadSDC.Rows[j]["NET DIVIDEND"].ToString() == "" ? Convert.ToDecimal(String.Format("{0:0000000000000000.00}", DefualtDividendNet)) : Convert.ToDecimal(dtReadSDC.Rows[j]["NET DIVIDEND"]);

                    InsertSDCFilesData(InsertTableName, INDEX_NAME, INDEX_CODE, INDEX_KEY, EFFECTIVE_DATE, COMPANY_NAME,
                                              RIC, BLOOMBERG_TICKER, CUSIP, ISIN, SEDOL, TICKER, GV_KEY, STOCK_KEY, GICS_CODE,
                                              DJI_IND_CODE, ALTERNATE_CLASS_CODE, MIC, COUNTRY_OF_DOMICILE, COUNTRY_OF_LISTING, REGION, SIZE,
                                              CAP_RANGE, CURRENCY_CODE, LOCAL_PRICE, FX_RATE, SHARES_OUTSTANDING, MARKET_CAP, IWF, AWF,
                                              GROWTH, VALUE, INDEX_SHARES, INDEX_MARKET_CAP, INDEX_WEIGHT, DAILY_PRICE_RETURN, DAILY_TOTAL_RETURN,
                                              DIVIDEND, NET_DIVIDEND);


                }
                LogError("No of Record inserted :" + RowCount, "IIFSLBSEReaderUtility");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed,InsertDB Save:" + ex.ToString(), "IIFSLBSEReaderUtility");
                LogError("Failed,InsertDB Save:" + ex.ToString(), "IIFSLBSEReaderUtility");
                LogError("No of Record inserted :" + RowCount, "IIFSLBSEReaderUtility");
            }
            return true;

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
                        selectcmd.Parameters.AddWithValue("pINDEX_CODE", INDEX_CODE);
                        selectcmd.Parameters.AddWithValue("pEFFECTIVE_DATE", EFFECTIVE_DATE);
                        selectcmd.Parameters.AddWithValue("pISIN", ISIN);
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


        public void InsertSDCFilesData(string TableType, string INDEX_NAME, string INDEX_CODE, string INDEX_KEY, string EFFECTIVE_DATE, string COMPANY_NAME,
       string RIC, string BLOOMBERG_TICKER, string CUSIP, string ISIN, string SEDOL, string TICKER, string GV_KEY, string STOCK_KEY, string GICS_CODE,
       string DJI_IND_CODE, string ALTERNATE_CLASS_CODE, string MIC, string COUNTRY_OF_DOMICILE, string COUNTRY_OF_LISTING, string REGION, string SIZE,
       string CAP_RANGE, string CURRENCY_CODE, decimal LOCAL_PRICE, string FX_RATE, Int64 SHARES_OUTSTANDING, decimal MARKET_CAP, decimal IWF, Int64 AWF,
   string GROWTH, string VALUE, decimal INDEX_SHARES, decimal INDEX_MARKET_CAP, decimal INDEX_WEIGHT, decimal DAILY_PRICE_RETURN, decimal DAILY_TOTAL_RETURN,
   decimal DIVIDEND, decimal NET_DIVIDEND)
        {
            try
            {

                //LogError("======================InsertSDCFilesData", "IIFSLBSEReaderUtility");
                string StrQuery = "";
                string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                //LogError("Oracle Connection " + ConnectionString + " Table Name:= " + TableType, "IIFSLBSEReaderUtility");
                try
                {
                    if (TableType == "ADJ")
                    {

                        StrQuery = "INSERT INTO UT_BSEWTBOD(INDEX_NAME, INDEX_CODE, INDEX_KEY, EFFECTIVE_DATE, COMPANY_NAME, RIC, BLOOMBERG_TICKER, CUSIP, ISIN, SEDOL, TICKER, GV_KEY, STOCK_KEY, GICS_CODE, DJI_IND_CODE, ALTERNATE_CLASS_CODE, MIC, COUNTRY_OF_DOMICILE, COUNTRY_OF_LISTING, REGION, INDEX_SIZE, CAP_RANGE, CURRENCY_CODE, LOCAL_PRICE, FX_RATE, SHARES_OUTSTANDING, MARKET_CAP, IWF, AWF, GROWTH, INDEX_VALUE, INDEX_SHARES, INDEX_MARKET_CAP, INDEX_WEIGHT, DAILY_PRICE_RETURN, DAILY_TOTAL_RETURN, DIVIDEND, NET_DIVIDEND, INP_DT_TM)" +
                                                    "VALUES(:pINDEX_NAME,:pINDEX_CODE,:pINDEX_KEY,:pEFFECTIVE_DATE,:pCOMPANY_NAME,:pRIC,:pBLOOMBERG_TICKER,:pCUSIP,:pISIN,:pSEDOL,:pTICKER,:pGV_KEY,:pSTOCK_KEY,:pGICS_CODE,:pDJI_IND_CODE,:pALTERNATE_CLASS_CODE,:pMIC,:pCOUNTRY_OF_DOMICILE,:pCOUNTRY_OF_LISTING,:pREGION,:pSIZE,:pCAP_RANGE,:pCURRENCY_CODE,:pLOCAL_PRICE,:pFX_RATE,:pSHARES_OUTSTANDING,:pMARKET_CAP,:pIWF,:pAWF,:pGROWTH,:pVALUE,:pINDEX_SHARES,:pINDEX_MARKET_CAP,:pINDEX_WEIGHT,:pDAILY_PRICE_RETURN,:pDAILY_TOTAL_RETURN,:pDIVIDEND,:pNET_DIVIDEND,:pINP_DT_TM)";

                        Console.WriteLine("table name :" + TableType);

                        DataTable dt = getDataTable("select * from  UT_BSEWTBOD where INDEX_CODE='" + INDEX_CODE + "' AND EFFECTIVE_DATE = '" + EFFECTIVE_DATE + "'AND ISIN = '" + ISIN + "'");
                        if (dt.Rows.Count < 1)
                        {
                            using (var Oracleconn = new NpgsqlConnection(ConnectionString))
                            {
                                Oracleconn.Open();
                                using (NpgsqlCommand cmd = new NpgsqlCommand(StrQuery, Oracleconn))
                                {
                                    cmd.Parameters.AddWithValue("pINDEX_NAME", INDEX_NAME);
                                    cmd.Parameters.AddWithValue("pINDEX_CODE", INDEX_CODE);
                                    cmd.Parameters.AddWithValue("pINDEX_KEY", INDEX_KEY);
                                    cmd.Parameters.AddWithValue("pEFFECTIVE_DATE", EFFECTIVE_DATE);
                                    cmd.Parameters.AddWithValue("pCOMPANY_NAME", COMPANY_NAME);
                                    cmd.Parameters.AddWithValue("pRIC", RIC);
                                    cmd.Parameters.AddWithValue("pBLOOMBERG_TICKER", BLOOMBERG_TICKER);
                                    cmd.Parameters.AddWithValue("pCUSIP", CUSIP);
                                    cmd.Parameters.AddWithValue("pISIN", ISIN);
                                    cmd.Parameters.AddWithValue("pSEDOL", SEDOL);
                                    cmd.Parameters.AddWithValue("pTICKER", TICKER);
                                    cmd.Parameters.AddWithValue("pGV_KEY", GV_KEY);
                                    cmd.Parameters.AddWithValue("pSTOCK_KEY", STOCK_KEY);
                                    cmd.Parameters.AddWithValue("pGICS_CODE", GICS_CODE);
                                    cmd.Parameters.AddWithValue("pDJI_IND_CODE", DJI_IND_CODE);
                                    cmd.Parameters.AddWithValue("pALTERNATE_CLASS_CODE", ALTERNATE_CLASS_CODE);
                                    cmd.Parameters.AddWithValue("pMIC", MIC);
                                    cmd.Parameters.AddWithValue("pCOUNTRY_OF_DOMICILE", COUNTRY_OF_DOMICILE);
                                    cmd.Parameters.AddWithValue("pCOUNTRY_OF_LISTING", COUNTRY_OF_LISTING);
                                    cmd.Parameters.AddWithValue("pREGION", REGION);
                                    cmd.Parameters.AddWithValue("pSIZE", SIZE);
                                    cmd.Parameters.AddWithValue("pCAP_RANGE", CAP_RANGE);
                                    cmd.Parameters.AddWithValue("pCURRENCY_CODE", CURRENCY_CODE);
                                    cmd.Parameters.AddWithValue("pLOCAL_PRICE", LOCAL_PRICE);
                                    cmd.Parameters.AddWithValue("pFX_RATE", FX_RATE);
                                    cmd.Parameters.AddWithValue("pSHARES_OUTSTANDING", SHARES_OUTSTANDING);

                                    cmd.Parameters.AddWithValue("pMARKET_CAP", MARKET_CAP);
                                    cmd.Parameters.AddWithValue("pIWF", IWF);
                                    cmd.Parameters.AddWithValue("pAWF", AWF);
                                    cmd.Parameters.AddWithValue("pGROWTH", GROWTH);
                                    cmd.Parameters.AddWithValue("pVALUE", VALUE);
                                    cmd.Parameters.AddWithValue("pINDEX_SHARES", INDEX_SHARES);
                                    cmd.Parameters.AddWithValue("pINDEX_MARKET_CAP", INDEX_MARKET_CAP);
                                    cmd.Parameters.AddWithValue("pINDEX_WEIGHT", INDEX_WEIGHT);
                                    cmd.Parameters.AddWithValue("pDAILY_PRICE_RETURN", DAILY_PRICE_RETURN);
                                    cmd.Parameters.AddWithValue("pDAILY_TOTAL_RETURN", DAILY_TOTAL_RETURN);
                                    cmd.Parameters.AddWithValue("pDIVIDEND", DIVIDEND);
                                    cmd.Parameters.AddWithValue("pNET_DIVIDEND", NET_DIVIDEND);
                                    cmd.Parameters.AddWithValue("pINP_DT_TM", DateTime.Now);
                                    cmd.ExecuteNonQuery();
                                    RowCount++;
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

                        StrQuery = "INSERT INTO UT_BSEWTEOD(INDEX_NAME, INDEX_CODE, INDEX_KEY, EFFECTIVE_DATE, COMPANY_NAME, RIC, BLOOMBERG_TICKER, CUSIP, ISIN, SEDOL, TICKER, GV_KEY, STOCK_KEY, GICS_CODE, DJI_IND_CODE, ALTERNATE_CLASS_CODE, MIC, COUNTRY_OF_DOMICILE, COUNTRY_OF_LISTING, REGION, INDEX_SIZE, CAP_RANGE, CURRENCY_CODE, LOCAL_PRICE, FX_RATE, SHARES_OUTSTANDING, MARKET_CAP, IWF, AWF, GROWTH, INDEX_VALUE, INDEX_SHARES, INDEX_MARKET_CAP, INDEX_WEIGHT, DAILY_PRICE_RETURN, DAILY_TOTAL_RETURN, DIVIDEND, NET_DIVIDEND, INP_DT_TM)" +
                                                        "VALUES(:pINDEX_NAME,:pINDEX_CODE,:pINDEX_KEY,:pEFFECTIVE_DATE,:pCOMPANY_NAME,:pRIC,:pBLOOMBERG_TICKER,:pCUSIP,:pISIN,:pSEDOL,:pTICKER,:pGV_KEY,:pSTOCK_KEY,:pGICS_CODE,:pDJI_IND_CODE,:pALTERNATE_CLASS_CODE,:pMIC,:pCOUNTRY_OF_DOMICILE,:pCOUNTRY_OF_LISTING,:pREGION,:pSIZE,:pCAP_RANGE,:pCURRENCY_CODE,:pLOCAL_PRICE,:pFX_RATE,:pSHARES_OUTSTANDING,:pMARKET_CAP,:pIWF,:pAWF,:pGROWTH,:pVALUE,:pINDEX_SHARES,:pINDEX_MARKET_CAP,:pINDEX_WEIGHT,:pDAILY_PRICE_RETURN,:pDAILY_TOTAL_RETURN,:pDIVIDEND,:pNET_DIVIDEND, :pINP_DT_TM)";






                        Console.WriteLine("table name :" + TableType);
                        DataTable dt = new DataTable();
                        dt = getDataTable("select * from  UT_BSEWTEOD where INDEX_CODE='" + INDEX_CODE + "' AND EFFECTIVE_DATE = '" + EFFECTIVE_DATE + "'AND ISIN = '" + ISIN + "'");


                        if (dt.Rows.Count < 1)
                        {
                            using (var Oracleconn = new NpgsqlConnection(ConnectionString))
                            {
                                Oracleconn.Open();
                                using (NpgsqlCommand cmd = new NpgsqlCommand(StrQuery, Oracleconn))
                                {
                                    cmd.Parameters.AddWithValue("pINDEX_NAME", INDEX_NAME);
                                    cmd.Parameters.AddWithValue("pINDEX_CODE", INDEX_CODE);
                                    cmd.Parameters.AddWithValue("pINDEX_KEY", INDEX_KEY);
                                    cmd.Parameters.AddWithValue("pEFFECTIVE_DATE", EFFECTIVE_DATE);
                                    cmd.Parameters.AddWithValue("pCOMPANY_NAME", COMPANY_NAME);
                                    cmd.Parameters.AddWithValue("pRIC", RIC);
                                    cmd.Parameters.AddWithValue("pBLOOMBERG_TICKER", BLOOMBERG_TICKER);
                                    cmd.Parameters.AddWithValue("pCUSIP", CUSIP);
                                    cmd.Parameters.AddWithValue("pISIN", ISIN);
                                    cmd.Parameters.AddWithValue("pSEDOL", SEDOL);
                                    cmd.Parameters.AddWithValue("pTICKER", TICKER);
                                    cmd.Parameters.AddWithValue("pGV_KEY", GV_KEY);
                                    cmd.Parameters.AddWithValue("pSTOCK_KEY", STOCK_KEY);
                                    cmd.Parameters.AddWithValue("pGICS_CODE", GICS_CODE);
                                    cmd.Parameters.AddWithValue("pDJI_IND_CODE", DJI_IND_CODE);
                                    cmd.Parameters.AddWithValue("pALTERNATE_CLASS_CODE", ALTERNATE_CLASS_CODE);
                                    cmd.Parameters.AddWithValue("pMIC", MIC);
                                    cmd.Parameters.AddWithValue("pCOUNTRY_OF_DOMICILE", COUNTRY_OF_DOMICILE);
                                    cmd.Parameters.AddWithValue("pCOUNTRY_OF_LISTING", COUNTRY_OF_LISTING);
                                    cmd.Parameters.AddWithValue("pREGION", REGION);
                                    cmd.Parameters.AddWithValue("pSIZE", SIZE);
                                    cmd.Parameters.AddWithValue("pCAP_RANGE", CAP_RANGE);
                                    cmd.Parameters.AddWithValue("pCURRENCY_CODE", CURRENCY_CODE);
                                    cmd.Parameters.AddWithValue("pLOCAL_PRICE", LOCAL_PRICE);
                                    cmd.Parameters.AddWithValue("pFX_RATE", FX_RATE);
                                    cmd.Parameters.AddWithValue("pSHARES_OUTSTANDING", SHARES_OUTSTANDING);

                                    cmd.Parameters.AddWithValue("pMARKET_CAP", MARKET_CAP);
                                    cmd.Parameters.AddWithValue("pIWF", IWF);
                                    cmd.Parameters.AddWithValue("pAWF", AWF);
                                    cmd.Parameters.AddWithValue("pGROWTH", GROWTH);
                                    cmd.Parameters.AddWithValue("pVALUE", VALUE);
                                    cmd.Parameters.AddWithValue("pINDEX_SHARES", INDEX_SHARES);
                                    cmd.Parameters.AddWithValue("pINDEX_MARKET_CAP", INDEX_MARKET_CAP);
                                    cmd.Parameters.AddWithValue("pINDEX_WEIGHT", INDEX_WEIGHT);
                                    cmd.Parameters.AddWithValue("pDAILY_PRICE_RETURN", DAILY_PRICE_RETURN);
                                    cmd.Parameters.AddWithValue("pDAILY_TOTAL_RETURN", DAILY_TOTAL_RETURN);
                                    cmd.Parameters.AddWithValue("pDIVIDEND", DIVIDEND);
                                    cmd.Parameters.AddWithValue("pNET_DIVIDEND", NET_DIVIDEND);
                                    cmd.Parameters.AddWithValue("pINP_DT_TM", DateTime.Now);
                                    cmd.ExecuteNonQuery();
                                    RowCount++;
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
                    LogError("Connection  Failed :" + ex.Message, "IIFSLBSEReaderUtility");

                }

            }
            catch (Exception ex)
            {
                string m = ex.Message;
                LogError("InsertSDCFileData :Failed" + ex.ToString(), "IIFSLBSEReaderUtility");
            }

        }
    
        public static void LogError(string message, string FileName)
        {
            string ErrorLogFile = ConfigurationManager.AppSettings["ErrorLogFile"];
            if (!Directory.Exists(ErrorLogFile))
                Directory.CreateDirectory(ErrorLogFile);

            ErrorLogFile += "\\" + FileName + "ErrorLog" + DateTime.Now.ToString("dd-MMM-yy") + ".txt";
            using (StreamWriter sw = new StreamWriter(ErrorLogFile, true))
            {
                sw.WriteLine(DateTime.Now.ToString("dd-MMM-yy HH:mm:ss") + "\t" + message);
            }
        }
    }
}










