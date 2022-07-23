using Npgsql;
using System;
using System.Configuration;
using System.Text;
using System.Data;

namespace BlmbergCsvToDbUtility
{
    public class Common
    {
        static private NpgsqlConnection conn;
        static string strCon = ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString;
        string strInsertQuery = string.Empty;

        /// <Operation for file of R01_InterschemeTransaction R01_MoneyMarket R01_Deposit R01_RepoDeals R01_Deals>
        /// 
        /// </summary>
        /// <param name="FileData"></param>
        /// <param name="filenm"></param>
        /// <param name="fileNumber"></param>
        public bool ProcessFormat3(DataTable FileData, string filenm, int fileNumber)
        {
            bool IsSuccess = false;
            DataModel DM = new DataModel();
            try
            {

                if (FileData.Rows.Count > 0)
                {
                    StringBuilder strQuery = new StringBuilder();
                    int rowCount = 0;


                    using (conn = new NpgsqlConnection(strCon))
                    {
                        conn.Open();

                        DataColumnCollection commColumns = FileData.Columns;
                        foreach (DataRow dr in FileData.Rows)
                        {
                            if (commColumns.Contains("prog_name"))
                                DM.PROG_NAME = string.IsNullOrEmpty(dr["prog_name"].ToString()) ? "" : dr["prog_name"].ToString();

                            if (commColumns.Contains("deal_id"))
                                DM.DEAL_ID = string.IsNullOrEmpty(dr["deal_id"].ToString()) ? "" : dr["deal_id"].ToString();

                            if (commColumns.Contains("field_name"))
                                DM.FIELD_NAME = string.IsNullOrEmpty(dr["field_name"].ToString()) ? "" : dr["field_name"].ToString();

                            if (commColumns.Contains("field_value"))
                                DM.FIELD_VALUE = string.IsNullOrEmpty(dr["field_value"].ToString()) ? "" : dr["field_value"].ToString();

                            if (commColumns.Contains("update_yn"))
                                DM.UPDATE_YN = string.IsNullOrEmpty(dr["update_yn"].ToString()) ? "" : dr["update_yn"].ToString();

                            if (commColumns.Contains("rectype"))
                                DM.RECTYPE = string.IsNullOrEmpty(dr["rectype"].ToString()) ? "" : dr["rectype"].ToString();

                            if (commColumns.Contains("currno"))
                                DM.CURRNO = string.IsNullOrEmpty(dr["currno"].ToString()) ? 0 : Convert.ToDecimal(dr["currno"].ToString());

                            if (commColumns.Contains("input_reverse"))
                                DM.INPUT_REVERSE = string.IsNullOrEmpty(dr["input_reverse"].ToString()) ? "" : dr["input_reverse"].ToString();

                            if (commColumns.Contains("srl_no"))
                                DM.SRL_NO = string.IsNullOrEmpty(dr["srl_no"].ToString()) ? 0 : Convert.ToInt32(dr["srl_no"].ToString());

                            DM.FILENAME = filenm;


                            //try
                            //{

                            //    DM.TRAN_DATE = string.IsNullOrEmpty(dr["tran_date"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("01-01-1900")) : Convert.ToDateTime(Helper.ToOracleDateFormat(dr["tran_date"].ToString()));

                            //    Helper.WriteLog("Date format of file : "+ filenm + " " + DM.TRAN_DATE, "E");
                            //}
                            //catch (Exception exx)
                            //{
                            //    Console.WriteLine("error while reading date  : " + exx.Message);
                            //    Helper.WriteLog("Date error formate format 3", filenm + "--" + exx.Message);
                            //    Helper.WriteLog("formate 3 ", filenm + " " + dr["tran_date"].ToString());

                            //}

                            if (DM.FILENAME.Split('_')[1].Contains("RepoDeals"))
                            {
                                DM.TRAN_DATE = DateTime.ParseExact(dr["tran_date"].ToString(), "dd/MM/yyyy", null);
                                //DM.TRAN_DATE = Convert.ToDateTime(Helper.ToOracleDateFormat(DM.TRAN_DATE.ToString()));
                            }
                            else
                            {
                                DM.TRAN_DATE = Convert.ToDateTime(Helper.ToOracleDateFormat(dr["tran_date"].ToString()));//  
                            }

                            DM.FILEDATE = DM.TRAN_DATE;
                            DM.FILENUMBER = fileNumber;

                            strInsertQuery = "INSERT INTO UT_MFDEALS(PROG_NAME,DEAL_ID,TRAN_DATE,FIELD_NAME,FIELD_VALUE,UPDATE_YN,RECTYPE,CURRNO,INPUT_REVERSE,SRL_NO,FILENAME,FILEDATE,FILENUMBER) VALUES('" + DM.PROG_NAME + "','" + DM.DEAL_ID + "','" + Helper.ToOracleDateFormat(DM.TRAN_DATE.ToString()) + "','" + DM.FIELD_NAME + "','" + DM.FIELD_VALUE + "','" + DM.UPDATE_YN + "','" + DM.RECTYPE + "'," + DM.CURRNO + ",'" + DM.INPUT_REVERSE + "'," + DM.SRL_NO + ",'" + DM.FILENAME + "','" + Helper.ToOracleDateFormat(DM.TRAN_DATE.ToString()) + "'," + DM.FILENUMBER + ")";

                            using (NpgsqlCommand oraCommand = new NpgsqlCommand(strInsertQuery, conn))
                            {
                                try
                                {
                                    if (conn.State != ConnectionState.Open)
                                    {
                                        conn.Open();
                                    }
                                    oraCommand.ExecuteNonQuery();
                                    rowCount++;
                                }
                                catch (Exception ex)
                                {
                                    Helper.WriteLog("Insert error in FileNm-> (" + DM.FILENAME + ") : \n" + ex.Message, "E");

                                    if (ex.Message.Contains("ORA-00001: unique constraint"))
                                        IsSuccess = true;
                                    else
                                        IsSuccess = false;
                                }
                            }
                        }
                        if (rowCount == FileData.Rows.Count)
                        {
                            IsSuccess = true;
                        }
                        Helper.WriteLog("File Name :  " + filenm + " Inserted record count : [" + rowCount + "] " + " file record count : " + FileData.Rows.Count, "S");
                    }
                }
                else
                {
                    IsSuccess = true;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Reading from dataTable failed : " + ex.Message);
                Helper.WriteLog("[ProcessFormat3 -" + filenm + " ] error inside foreach : \n" + filenm + " -- " + ex.Message, "E");
                IsSuccess = false;
            }
            return IsSuccess;
        }

        /// <Operation for file of R01_Equity>
        /// 
        /// </summary>
        /// <param name="FileData"></param>
        /// <param name="filenm"></param>
        /// <param name="fileNumber"></param>
        public bool ProcessR01_Equity(DataTable FileData, string filenm, int fileNumber)
        {
            bool IsSuccess = false;

            R01_EquiryModel DM = new R01_EquiryModel();
            try
            {

                if (FileData.Rows.Count > 0)
                {
                    StringBuilder strQuery = new StringBuilder();
                    int rowCount = 0;

                    using (conn = new NpgsqlConnection(strCon))
                    {
                        conn.Open();
                        DataColumnCollection commColumns = FileData.Columns;
                        foreach (DataRow dr in FileData.Rows)
                        {
                            if (commColumns.Contains("trade_date"))
                                DM.TRADE_DATE = DateTime.ParseExact(dr["trade_date"].ToString(), "yyyyMMdd", null);  //Convert.ToDateTime(dr["trade_date"].ToString());

                            if (commColumns.Contains("order_id"))
                                DM.ORDER_ID = string.IsNullOrEmpty(dr["order_id"].ToString()) ? "" : dr["order_id"].ToString();

                            if (commColumns.Contains("broker"))
                                DM.BROKER = string.IsNullOrEmpty(dr["order_id"].ToString()) ? "" : dr["broker"].ToString();

                            if (commColumns.Contains("broker_name"))
                                DM.BROKER_NAME = string.IsNullOrEmpty(dr["broker_name"].ToString()) ? "" : dr["broker_name"].ToString().Replace("\"", string.Empty).Replace("'", string.Empty);

                            if (commColumns.Contains("sebiregno_broker"))
                                DM.SEBIREGNO_BROKER = string.IsNullOrEmpty(dr["sebiregno_broker"].ToString()) ? "" : dr["sebiregno_broker"].ToString();

                            if (commColumns.Contains("brokerage_bps"))
                                DM.BROKERAGE = string.IsNullOrEmpty(dr["brokerage_bps"].ToString()) ? 0 : Convert.ToDecimal(dr["brokerage_bps"].ToString());

                            if (commColumns.Contains("security"))
                                DM.SECURITY = string.IsNullOrEmpty(dr["security"].ToString()) ? "" : dr["security"].ToString();

                            if (commColumns.Contains("security_name"))
                                DM.SECURITY_NAME = string.IsNullOrEmpty(dr["security_name"].ToString()) ? "" : dr["security_name"].ToString().Replace("\"", string.Empty).Replace("'", string.Empty);

                            if (commColumns.Contains("isin"))
                                DM.ISIN = string.IsNullOrEmpty(dr["isin"].ToString()) ? "" : dr["isin"].ToString();

                            if (commColumns.Contains("issuer"))
                                DM.ISUSER = string.IsNullOrEmpty(dr["issuer"].ToString()) ? "" : dr["issuer"].ToString().Replace("\"", string.Empty).Replace("'", string.Empty);

                            if (commColumns.Contains("tran_type"))
                                DM.TRAN_TYPE = string.IsNullOrEmpty(dr["tran_type"].ToString()) ? "" : dr["tran_type"].ToString();

                            if (commColumns.Contains("scheme"))
                                DM.SCHEME = string.IsNullOrEmpty(dr["scheme"].ToString()) ? "" : dr["scheme"].ToString();

                            if (commColumns.Contains("schname"))
                                DM.SCHNAME = string.IsNullOrEmpty(dr["schname"].ToString()) ? "" : dr["schname"].ToString();

                            if (commColumns.Contains("sebiregno_scheme"))
                                DM.SEBIREGNO_SCHEME = string.IsNullOrEmpty(dr["sebiregno_scheme"].ToString()) ? "" : dr["sebiregno_scheme"].ToString();

                            if (commColumns.Contains("confirmed_quantity"))
                                DM.CONFIRMED_QUANTITY = string.IsNullOrEmpty(dr["confirmed_quantity"].ToString()) ? 0 : Convert.ToDecimal(dr["confirmed_quantity"].ToString());

                            if (commColumns.Contains("confirmed_price"))
                                DM.CONFIRMED_PRICE = string.IsNullOrEmpty(dr["sebiregno_scheme"].ToString()) ? 0 : Convert.ToDecimal(dr["confirmed_price"].ToString());

                            if (commColumns.Contains("confirmed_amount"))
                                DM.CONFIRMED_AMOUNT = string.IsNullOrEmpty(dr["confirmed_amount"].ToString()) ? 0 : Convert.ToDecimal(dr["confirmed_amount"].ToString());

                            if (commColumns.Contains("stk_exch"))
                                DM.STK_EXCK = string.IsNullOrEmpty(dr["stk_exch"].ToString()) ? "" : dr["stk_exch"].ToString();

                            if (commColumns.Contains("mapin_exch"))
                                DM.MAPIN_EXCH = string.IsNullOrEmpty(dr["mapin_exch"].ToString()) ? "" : dr["mapin_exch"].ToString();

                            if (commColumns.Contains("send_addr"))
                                DM.SEND_ADDR = string.IsNullOrEmpty(dr["send_addr"].ToString()) ? "" : dr["send_addr"].ToString();

                            if (commColumns.Contains("basktyn"))
                                DM.BASKTYN = string.IsNullOrEmpty(dr["basktyn"].ToString()) ? "" : dr["basktyn"].ToString();

                            if (commColumns.Contains("asset_type"))
                                DM.ASSET_TYPE = string.IsNullOrEmpty(dr["asset_type"].ToString()) ? "" : dr["asset_type"].ToString();

                            if (commColumns.Contains("dealer"))
                                DM.DEALER = string.IsNullOrEmpty(dr["dealer"].ToString()) ? "" : dr["dealer"].ToString();

                            if (commColumns.Contains("prim_sec"))
                                DM.PRIME_SEC = string.IsNullOrEmpty(dr["prim_sec"].ToString()) ? "" : dr["prim_sec"].ToString();

                            if (commColumns.Contains("ticket_type"))
                                DM.TICKET_TYPE = string.IsNullOrEmpty(dr["ticket_type"].ToString()) ? "" : dr["ticket_type"].ToString();

                            if (commColumns.Contains("Ticket_number"))
                                DM.TICKET_NUMBER = string.IsNullOrEmpty(dr["Ticket_number"].ToString()) ? "" : dr["Ticket_number"].ToString();

                            // added on 18/02/2020 
                            if (commColumns.Contains("commission_amount"))
                                DM.COMMISSION_AMT = string.IsNullOrEmpty(dr["commission_amount"].ToString()) ? 0 : Convert.ToDecimal(dr["commission_amount"]);

                            if (commColumns.Contains("stt"))
                                DM.STT = string.IsNullOrEmpty(dr["stt"].ToString()) ? 0 : Convert.ToDecimal(dr["stt"]);

                            if (commColumns.Contains("total_transaction_cost"))
                                DM.TOT_TXN_COST = string.IsNullOrEmpty(dr["total_transaction_cost"].ToString()) ? 0 : Convert.ToDecimal(dr["total_transaction_cost"]);



                            if (commColumns.Contains("settlement_date"))
                                DM.SETTLEMENT_DATE = string.IsNullOrEmpty(dr["settlement_date"].ToString()) ? "" : dr["settlement_date"].ToString();


                            if (commColumns.Contains("reference"))
                                DM.MF_REFERENCE = string.IsNullOrEmpty(dr["reference"].ToString()) ? "" : dr["reference"].ToString();

                            if (commColumns.Contains("parsekyable_des"))
                                DM.PARSEKEY = string.IsNullOrEmpty(dr["parsekyable_des"].ToString()) ? "" : dr["parsekyable_des"].ToString();

                            if (commColumns.Contains("ticker"))
                                DM.BBGTICKER = string.IsNullOrEmpty(dr["ticker"].ToString()) ? "" : dr["ticker"].ToString();


                            DM.FILENAME = filenm;
                            DM.FILEDATE = DM.TRADE_DATE;
                            DM.FILENUMBER = fileNumber;

                            strInsertQuery = "INSERT INTO UT_EQUITYDEALS(TRADE_DATE,ORDER_ID,BROKER,BROKER_NAME,SEBIREGNO_BROKER,BROKERAGE,SECURITY,SECURITY_NAME,ISIN,ISSUER,TRAN_TYPE,SCHEME,SCHNAME,SEBIREGNO_SCHEME,CONFIRMED_QUANTITY,CONFIRMED_PRICE,CONFIRMED_AMOUNT,STK_EXCK,MAPIN_EXCH,SEND_ADDR,BASKTYN,ASSET_TYPE,DEALER,PRIME_SEC,ticket_type,Ticket_number,FILENAME,FILEDATE,FILENUMBER,COMMISSION_AMT,STT,TOT_TXN_COST,SETTLEMENT_DATE,MF_REFERENCE,PARSEKEY,BBGTICKER) VALUES('" + Helper.ToOracleDateFormat(DM.TRADE_DATE.ToString()) + "','" + DM.ORDER_ID + "','" + DM.BROKER + "','" + DM.BROKER_NAME + "','" + DM.SEBIREGNO_BROKER + "','" + DM.BROKERAGE + "','" + DM.SECURITY + "','" + DM.SECURITY_NAME + "','" + DM.ISIN + "','" + DM.ISUSER + "','" + DM.TRAN_TYPE + "','" + DM.SCHEME + "','" + DM.SCHNAME + "','" + DM.SEBIREGNO_SCHEME + "','" + DM.CONFIRMED_QUANTITY + "','" + DM.CONFIRMED_PRICE + "','" + DM.CONFIRMED_AMOUNT + "','" + DM.STK_EXCK + "','" + DM.MAPIN_EXCH + "','" + DM.SEND_ADDR + "','" + DM.BASKTYN + "','" + DM.ASSET_TYPE + "','" + DM.DEALER + "','" + DM.PRIME_SEC + "','" + DM.TICKET_TYPE + "','" + DM.TICKET_NUMBER + "','" + DM.FILENAME + "','" + Helper.ToOracleDateFormat(DM.FILEDATE.ToString()) + "'," + DM.FILENUMBER + ",'" + DM.COMMISSION_AMT + "','" + DM.STT + "','" + DM.TOT_TXN_COST + "','" + DM.SETTLEMENT_DATE + "','" + DM.MF_REFERENCE + "','" + DM.PARSEKEY + "','" + DM.BBGTICKER + "')";
                            //Helper.WriteLog(strInsertQuery, "S");
                            using (NpgsqlCommand oraCommand = new NpgsqlCommand(strInsertQuery, conn))
                            {
                                try
                                {
                                    if (conn.State != ConnectionState.Open)
                                    {
                                        conn.Open();
                                    }
                                    oraCommand.ExecuteNonQuery();
                                    rowCount++;
                                }
                                catch (Exception ex)
                                {

                                    Helper.WriteLog("[ ProcessR01_Equity FileNm-> (" + DM.FILENAME + ") ]Insert error : \n" + ex.Message, "E");

                                    if (ex.Message.Contains("ORA-00001: unique constraint"))
                                        IsSuccess = true;
                                    else
                                        IsSuccess = false;

                                }
                            }
                        }
                        if (rowCount == FileData.Rows.Count)
                        {
                            IsSuccess = true;
                        }
                        Helper.WriteLog("File Name :  " + filenm + " Inserted record count : [" + rowCount + "] " + " file record count : " + FileData.Rows.Count, "S");
                    }
                }
                else
                {
                    IsSuccess = true;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Reading from dataTable failed : " + ex.Message);
                Helper.WriteLog("[ProcessR01_Equity] error inside foreach : " + ex.Message, "E");
                IsSuccess = false;
            }
            return IsSuccess;
        }

        public bool ProcessR01_DERIVATIVES(DataTable FileData, string filenm, int fileNumber)
        {
            bool IsSuccess = false;
            R01_DerivativesModel DM = new R01_DerivativesModel();
            try
            {

                if (FileData.Rows.Count > 0)
                {
                    StringBuilder strQuery = new StringBuilder();
                    int rowCount = 0;

                    using (conn = new NpgsqlConnection(strCon))
                    {
                        conn.Open();
                        DataColumnCollection commColumns = FileData.Columns;
                        foreach (DataRow dr in FileData.Rows)
                        {
                            if (commColumns.Contains("order_id"))
                                DM.ORDER_ID = string.IsNullOrEmpty(dr["order_id"].ToString()) ? "" : dr["order_id"].ToString();

                            // DM.VALUE_DATE = DateTime.ParseExact(dr["value_date"].ToString(), "dd/MM/yyyy HH:mm:ss", null);
                            if (commColumns.Contains("value_date"))
                                DM.VALUE_DATE = string.IsNullOrEmpty(dr["value_date"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("01-01-1900")) : DateTime.ParseExact(dr["value_date"].ToString(), "dd/MM/yyyy", null);//DateTime.ParseExact(dr["value_date"].ToString(), "dd/MM/yyyy", null);

                            if (commColumns.Contains("uti_scheme"))
                                DM.UTI_SCHEME = string.IsNullOrEmpty(dr["uti_scheme"].ToString()) ? "" : dr["uti_scheme"].ToString();

                            if (commColumns.Contains("uti_broker_code"))
                                DM.UTI_BROKER_CODE = string.IsNullOrEmpty(dr["uti_broker_code"].ToString()) ? "" : dr["uti_broker_code"].ToString();

                            if (commColumns.Contains("broker_name"))
                                DM.BROKER_NAME = string.IsNullOrEmpty(dr["broker_name"].ToString()) ? "" : dr["broker_name"].ToString().Replace("\"", string.Empty).Replace("'", string.Empty);

                            if (commColumns.Contains("uti_future_code"))
                                DM.UTI_FUTURE_CODE = string.IsNullOrEmpty(dr["uti_future_code"].ToString()) ? "" : dr["uti_future_code"].ToString();

                            if (commColumns.Contains("buy_sell"))
                                DM.BUY_SELL = string.IsNullOrEmpty(dr["buy_sell"].ToString()) ? "" : dr["buy_sell"].ToString();

                            if (commColumns.Contains("quantity"))
                                DM.QUANTITY = string.IsNullOrEmpty(dr["quantity"].ToString()) ? 0 : Convert.ToDecimal(dr["quantity"].ToString());

                            if (commColumns.Contains("val"))
                                DM.VAL = string.IsNullOrEmpty(dr["val"].ToString()) ? 0 : Convert.ToDecimal(dr["val"].ToString());

                            if (commColumns.Contains("uti_dealer_code"))
                                DM.UTI_DEALER_CODE = string.IsNullOrEmpty(dr["uti_dealer_code"].ToString()) ? "" : dr["uti_dealer_code"].ToString();

                            if (commColumns.Contains("exchg_scheme_code"))
                                DM.EXCHG_SCHEME_CODE = string.IsNullOrEmpty(dr["exchg_scheme_code"].ToString()) ? "" : dr["exchg_scheme_code"].ToString();

                            if (commColumns.Contains("exchg_fut_code"))
                                DM.EXCHG_FUT_CODE = string.IsNullOrEmpty(dr["exchg_fut_code"].ToString()) ? "" : dr["exchg_fut_code"].ToString();

                            // DM.MATDT = DateTime.ParseExact(dr["matdt"].ToString(), "dd/MM/yyyy HH:mm:ss", null);
                            if (commColumns.Contains("matdt"))
                                DM.MATDT = DateTime.ParseExact(dr["matdt"].ToString(), "dd-MMM-yyyy", null);

                            if (commColumns.Contains("mat_month"))
                                DM.MAT_MONTH = string.IsNullOrEmpty(dr["mat_month"].ToString()) ? "" : dr["mat_month"].ToString();

                            if (commColumns.Contains("exchange"))
                                DM.EXCHANGE = string.IsNullOrEmpty(dr["exchange"].ToString()) ? "" : dr["exchange"].ToString();

                            if (commColumns.Contains("custd_broker_code"))
                                DM.CUST_BROKER_CODE = string.IsNullOrEmpty(dr["custd_broker_code"].ToString()) ? "" : dr["custd_broker_code"].ToString();

                            if (commColumns.Contains("brokerage_bps"))
                                DM.BROKERAGE_BPS = string.IsNullOrEmpty(dr["brokerage_bps"].ToString()) ? 0 : Convert.ToDecimal(dr["brokerage_bps"].ToString());

                            if (commColumns.Contains("stt_yn"))
                                DM.STT_YN = string.IsNullOrEmpty(dr["stt_yn"].ToString()) ? "" : dr["stt_yn"].ToString();

                            if (commColumns.Contains("call_or_put"))
                                DM.CALL_OR_PUT = string.IsNullOrEmpty(dr["call_or_put"].ToString()) ? "" : dr["call_or_put"].ToString();

                            if (commColumns.Contains("strike_price"))
                            {
                                if (dr["strike_price"].ToString() != "")
                                {
                                    DM.STRIKE_PRICE = string.IsNullOrEmpty(dr["strike_price"].ToString()) ? 0 : Convert.ToDecimal(dr["strike_price"].ToString());
                                }
                                else
                                {
                                    DM.STRIKE_PRICE = 0;
                                }
                            }

                            if (commColumns.Contains("stock_or_index"))
                                DM.STOCK_OR_INDEX = string.IsNullOrEmpty(dr["stock_or_index"].ToString()) ? "" : dr["stock_or_index"].ToString();

                            if (commColumns.Contains("asset_category"))
                                DM.ASSET_CATEGORY = string.IsNullOrEmpty(dr["asset_category"].ToString()) ? "" : dr["asset_category"].ToString();

                            if (commColumns.Contains("ticket_type"))
                                DM.TICKET_TYPE = string.IsNullOrEmpty(dr["ticket_type"].ToString()) ? "" : dr["ticket_type"].ToString();

                            DM.FILENAME = filenm;
                            DM.FILEDATE = DM.VALUE_DATE;
                            DM.FILENUMBER = fileNumber;

                            strInsertQuery = "INSERT INTO UT_DERIDEALS(ORDER_ID,VALUE_DATE,UTI_SCHEME,UTI_BROCKER_CODE,BROKER_NAME,UTI_FUTURE_CODE,BUY_SELL,QUANTITY,VAL,UTI_DEALER_CODE,EXCHG_SCHEME_CODE,EXCHG_FUT_CODE,MATDT,MAT_MONTH,EXCHANGE,CUSTD_BROKER_CODE,BROKERAGE_BPS,STT_YN,CALL_OR_PUT,STRIKE_PRICE,STOCK_OR_INDEX,ASSET_CATEGORY,TICKET_TYPE,FILENAME,FILEDATE,FILENUMBER) VALUES('" + DM.ORDER_ID + "','" + Helper.ToOracleDateFormat(DM.VALUE_DATE.ToString()) + "','" + DM.UTI_SCHEME + "','" + DM.UTI_BROKER_CODE + "','" + DM.BROKER_NAME + "','" + DM.UTI_FUTURE_CODE + "','" + DM.BUY_SELL + "','" + DM.QUANTITY + "','" + DM.VAL + "','" + DM.UTI_DEALER_CODE + "','" + DM.EXCHG_SCHEME_CODE + "','" + DM.EXCHG_FUT_CODE + "','" + Helper.ToOracleDateFormat(DM.MATDT.ToShortDateString()) + "','" + DM.MAT_MONTH + "','" + DM.EXCHANGE + "','" + DM.CUST_BROKER_CODE + "','" + DM.BROKERAGE_BPS + "','" + DM.STT_YN + "','" + DM.CALL_OR_PUT + "','" + DM.STRIKE_PRICE + "','" + DM.STOCK_OR_INDEX + "','" + DM.ASSET_CATEGORY + "','" + DM.TICKET_TYPE + "','" + DM.FILENAME + "','" + Helper.ToOracleDateFormat(DM.FILEDATE.ToString()) + "'," + DM.FILENUMBER + ")";
                            // Helper.WriteLog(strInsertQuery, "S");
                            using (NpgsqlCommand oraCommand = new NpgsqlCommand(strInsertQuery, conn))
                            {
                                try
                                {
                                    if (conn.State != ConnectionState.Open)
                                    {
                                        conn.Open();
                                    }
                                    oraCommand.ExecuteNonQuery();
                                    rowCount++;
                                }
                                catch (Exception ex)
                                {
                                    Helper.WriteLog("[ ProcessR01_DERIVATIVES FileNm-> (" + DM.FILENAME + ") ] Insert error : \n" + ex.Message, "E");

                                    if (ex.Message.Contains("ORA-00001: unique constraint"))
                                        IsSuccess = true;
                                    else
                                        IsSuccess = false;
                                }
                            }
                        }
                        if (rowCount == FileData.Rows.Count)
                        {
                            IsSuccess = true;
                        }
                        Helper.WriteLog("File Name :  " + filenm + " Inserted record count : [" + rowCount + "] " + " file record count : " + FileData.Rows.Count, "S");
                    }
                }
                else
                {
                    IsSuccess = true;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Reading from dataTable failed : " + ex.Message);
                Helper.WriteLog("[ ProcessR01_DERIVATIVES ] error inside foreach : " + ex.Message, "E");
                IsSuccess = false;
            }
            return IsSuccess;
        }
    }
}
