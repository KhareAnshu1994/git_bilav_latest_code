using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace BlmbergCsvToDbUtility
{
    //public class TradeFeedFileProces
    //{
    //    static private NpgsqlConnection conn;
    //    static string strCon = ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString;

    //    List<string> QueryList = new List<string>();
    //    string strQuery = string.Empty;

    //    DataTable dtFinal = new DataTable();
    //    public List<XmlSchema> Schemas { get; set; }
    //    public List<String> Errors { get; set; }
    //    public List<String> Warnings { get; set; }

    //    // string XmlFleLoaction = @"C:\Users\CEPL\Desktop\R04_BatchTrade_08012020_081128.xml";

    //    //string XmlFleLoaction = ConfigurationManager.AppSettings["xmlFileLocation"];
    //    //  string XsdFileLoaction = @"D:\UTI PROJ\Development\Bloomberg Utilities\ReqFile\BB_TradeFeed_V20.xsd";


    //    public void ProcessingXml(string xmlFilNm)
    //    {
    //        DataSet dataSet = new DataSet();
    //        try
    //        {
    //            dataSet.ReadXml(xmlFilNm, XmlReadMode.InferSchema);

    //            //Console.WriteLine("tradefeed rows count --------------------->" + dataSet.Tables["TradeFeed"].Rows.Count);
    //            //Console.WriteLine("Common rows count --------------------->" + dataSet.Tables["Common"].Rows.Count);
    //            //Console.WriteLine("ShortNotes rows count --------------------->" + dataSet.Tables["ShortNotes"].Rows.Count);
    //            //Console.WriteLine("ShortNote rows count --------------------->" + dataSet.Tables["ShortNote"].Rows.Count);
    //            //Console.WriteLine("LongNotes rows count --------------------->" + dataSet.Tables["LongNotes"].Rows.Count);
    //            //Console.WriteLine("LongNote rows count --------------------->" + dataSet.Tables["LongNote"].Rows.Count);
    //            //Console.WriteLine("BloombergFunctions rows count --------------------->" + dataSet.Tables["BloombergFunctions"].Rows.Count);
    //            //Console.WriteLine("FunctionName rows count --------------------->" + dataSet.Tables["FunctionName"].Rows.Count);
    //            //Console.WriteLine("TrasactionCosts rows count --------------------->" + dataSet.Tables["TransactionCosts"].Rows.Count);
    //            //Console.WriteLine("TrTrasactionCost rows count --------------------->" + dataSet.Tables["TransactionCost"].Rows.Count);
    //            //Console.WriteLine("RepoFields rows count --------------------->" + dataSet.Tables["RepoFields"].Rows.Count);
    //            //Console.WriteLine("ExtendedFields rows count --------------------->" + dataSet.Tables["ExtendedFields"].Rows.Count);
    //            //Console.WriteLine("Field rows count --------------------->" + dataSet.Tables["Field"].Rows.Count);
    //        }
    //        catch (Exception exx)
    //        {
    //            Console.WriteLine("errror while feeling dataset from xml \n" + exx.Message);
    //            Helper.WriteLogXml("errror while feeling dataset from xml \n" + exx.Message);
    //        }
    //        string filname = Path.GetFileNameWithoutExtension(xmlFilNm);
    //        string[] FileDetails = filname.Split('_');

    //        DateTime FileDate = DateTime.ParseExact(FileDetails[2], "ddMMyyyy", CultureInfo.InvariantCulture);
    //        int fileNumber = Convert.ToInt32(FileDetails[3]);
    //        Console.WriteLine("total table in dataset : --->" + dataSet.Tables.Count);

    //        int TradeFeedInsert = 0, Ut_NotesInsert = 0, TransCostInsert = 0, RepoFieldInsert = 0, Des_FieldInsert = 0;

    //        bool IsAlreadyPresent = DbOperation.ValidateRecord(dataSet, filname);

    //        //if (!IsAlreadyPresent) // commented for testing purpose letter i will remove for production 09-MAR-2020

    //        if (!IsAlreadyPresent)
    //        {
    //            foreach (DataRow comDrow in dataSet.Tables["Common"].Rows)
    //            {
    //                int common_id = Convert.ToInt32(comDrow["TradeFeed_Id"]);
    //                using (conn = new NpgsqlConnection(strCon))
    //                {
    //                    conn.Open();
    //                    try
    //                    {
    //                        CommonModel comModl = new CommonModel();

    //                        DataColumnCollection commColumns = dataSet.Tables["Common"].Columns;

    //                        try
    //                        {

    //                            if (commColumns.Contains("BLOOMBERGFIRMID"))
    //                            {
    //                                comModl.BLOOMBERGFIRMID = string.IsNullOrEmpty(comDrow["BLOOMBERGFIRMID"].ToString()) ? 0 : Convert.ToInt32(comDrow["BLOOMBERGFIRMID"]); //varchar
    //                            }

    //                            if (commColumns.Contains("TRANSACTIONNUMBER"))
    //                                comModl.TRANSACTIONNUMBER = string.IsNullOrEmpty(comDrow["TRANSACTIONNUMBER"].ToString()) ? 0 : Convert.ToInt32(comDrow["TRANSACTIONNUMBER"]);

    //                            if (commColumns.Contains("SECURITYIDENTIFIERFLAG"))
    //                                comModl.SECURITYIDENTIFIERFLAG = string.IsNullOrEmpty(comDrow["SECURITYIDENTIFIERFLAG"].ToString()) ? "" : comDrow["SECURITYIDENTIFIERFLAG"].ToString();

    //                            if (commColumns.Contains("SECURITYIDENTIFIER"))
    //                                comModl.SECURITYIDENTIFIER = string.IsNullOrEmpty(comDrow["SECURITYIDENTIFIER"].ToString()) ? "" : comDrow["SECURITYIDENTIFIER"].ToString();

    //                            if (commColumns.Contains("SECURITYCURRENCYISOCODE"))
    //                                comModl.SECURITYCURRENCYISOCODE = string.IsNullOrEmpty(comDrow["SECURITYCURRENCYISOCODE"].ToString()) ? "" : comDrow["SECURITYCURRENCYISOCODE"].ToString();

    //                            if (commColumns.Contains("SECURITYPRODUCTKEY"))
    //                                comModl.SECURITYPRODUCTKEY = string.IsNullOrEmpty(comDrow["SECURITYPRODUCTKEY"].ToString()) ? 0 : Convert.ToInt32(comDrow["SECURITYPRODUCTKEY"]); // number ? int

    //                            if (commColumns.Contains("BLOOMBERGIDENTIFIER"))
    //                                comModl.BLOOMBERGIDENTIFIER = string.IsNullOrEmpty(comDrow["BLOOMBERGIDENTIFIER"].ToString()) ? "" : comDrow["BLOOMBERGIDENTIFIER"].ToString();

    //                            if (commColumns.Contains("TICKER"))
    //                                comModl.TICKER = string.IsNullOrEmpty(comDrow["TICKER"].ToString()) ? "" : comDrow["TICKER"].ToString();

    //                            if (commColumns.Contains("COUPONSTRIKEPRICE"))
    //                                comModl.COUPONSTRIKEPRICE = string.IsNullOrEmpty(comDrow["COUPONSTRIKEPRICE"].ToString()) ? "" : comDrow["COUPONSTRIKEPRICE"].ToString(); //number ?

    //                            if (commColumns.Contains("MATURITYDATEEXPIRATIONDATE"))
    //                                comModl.MATURITYDATEEXPIRATIONDATE = string.IsNullOrEmpty(comDrow["MATURITYDATEEXPIRATIONDATE"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("01-01-1900")) : Convert.ToDateTime(comDrow["MATURITYDATEEXPIRATIONDATE"].ToString()); // date

    //                            if (commColumns.Contains("SERIESEXCHANGECODE"))
    //                                comModl.SERIESEXCHANGECODE = string.IsNullOrEmpty(comDrow["SERIESEXCHANGECODE"].ToString()) ? "" : comDrow["SERIESEXCHANGECODE"].ToString(); // number >no need to change

    //                            if (commColumns.Contains("BUYSELLCOVERSHORTFLAG"))
    //                                comModl.BUYSELLCOVERSHORTFLAG = string.IsNullOrEmpty(comDrow["BUYSELLCOVERSHORTFLAG"].ToString()) ? "" : comDrow["BUYSELLCOVERSHORTFLAG"].ToString();

    //                            if (commColumns.Contains("RECORDTYPE"))
    //                                comModl.RECORDTYPE = string.IsNullOrEmpty(comDrow["RECORDTYPE"].ToString()) ? 0 : Convert.ToInt32(comDrow["RECORDTYPE"]); // number ? int

    //                            if (commColumns.Contains("TRADEDATE"))
    //                                comModl.TRADEDATE = string.IsNullOrEmpty(comDrow["TRADEDATE"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("01-01-1900")) : Convert.ToDateTime(comDrow["TRADEDATE"]);

    //                            if (commColumns.Contains("ASOFTRADEDATE"))
    //                                comModl.ASOFTRADEDATE = string.IsNullOrEmpty(comDrow["ASOFTRADEDATE"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("01-01-1900")) : Convert.ToDateTime(comDrow["ASOFTRADEDATE"]);

    //                            if (commColumns.Contains("SETTLEMENTDATE"))

    //                                comModl.SETTLEMENTDATE = string.IsNullOrEmpty(comDrow["SETTLEMENTDATE"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("01-01-1900")) : Convert.ToDateTime(comDrow["SETTLEMENTDATE"]);

    //                            if (commColumns.Contains("PRICE"))
    //                                comModl.PRICE = string.IsNullOrEmpty(comDrow["PRICE"].ToString()) ? 0 : Convert.ToDecimal(comDrow["PRICE"]);

    //                            if (commColumns.Contains("YIELD"))
    //                                comModl.YIELD = string.IsNullOrEmpty(comDrow["YIELD"].ToString()) ? 0 : Convert.ToDecimal(comDrow["YIELD"]);

    //                            if (commColumns.Contains("TRADEAMOUNT"))
    //                                comModl.TRADEAMOUNT = string.IsNullOrEmpty(comDrow["TRADEAMOUNT"].ToString()) ? 0 : Convert.ToDecimal(comDrow["TRADEAMOUNT"]);

    //                            if (commColumns.Contains("BLOOMBERGFIRMID"))
    //                                comModl.CUSTOMERACCOUNTCOUNTERPARTY = string.IsNullOrEmpty(comDrow["CUSTOMERACCOUNTCOUNTERPARTY"].ToString()) ? "" : comDrow["CUSTOMERACCOUNTCOUNTERPARTY"].ToString();

    //                            if (commColumns.Contains("ACCOUNTCOUNTERPARTYSHORTNAME"))
    //                                comModl.ACCOUNTCOUNTERPARTYSHORTNAME = string.IsNullOrEmpty(comDrow["ACCOUNTCOUNTERPARTYSHORTNAME"].ToString()) ? "" : comDrow["ACCOUNTCOUNTERPARTYSHORTNAME"].ToString();

    //                            if (commColumns.Contains("SETTLEMENTLOCATIONINDICATOR"))
    //                                comModl.SETTLEMENTLOCATIONINDICATOR = string.IsNullOrEmpty(comDrow["SETTLEMENTLOCATIONINDICATOR"].ToString()) ? "" : comDrow["SETTLEMENTLOCATIONINDICATOR"].ToString();

    //                            if (commColumns.Contains("PRODUCTSUBFLAG"))
    //                                comModl.PRODUCTSUBFLAG = string.IsNullOrEmpty(comDrow["PRODUCTSUBFLAG"].ToString()) ? 0 : Convert.ToInt32(comDrow["PRODUCTSUBFLAG"]); //number

    //                            if (commColumns.Contains("PRINCIPALLOANAMOUNT"))
    //                                comModl.PRINCIPALLOANAMOUNT = string.IsNullOrEmpty(comDrow["PRINCIPALLOANAMOUNT"].ToString()) ? 0 : Convert.ToDecimal(comDrow["PRINCIPALLOANAMOUNT"]);

    //                            if (commColumns.Contains("ACCRUEDINTERESTREPOINTEREST"))
    //                                comModl.ACCRUEDINTERESTREPOINTEREST = string.IsNullOrEmpty(comDrow["ACCRUEDINTERESTREPOINTEREST"].ToString()) ? 0 : Convert.ToDecimal(comDrow["ACCRUEDINTERESTREPOINTEREST"]); // number ? decimal

    //                            if (commColumns.Contains("TRADERACCOUNTNAME"))
    //                                comModl.TRADERACCOUNTNAME = string.IsNullOrEmpty(comDrow["TRADERACCOUNTNAME"].ToString()) ? "" : comDrow["TRADERACCOUNTNAME"].ToString(); // number it should be varchar

    //                            if (commColumns.Contains("NUMBEROFDAYSACCRUED"))
    //                                comModl.NUMBEROFDAYSACCRUED = string.IsNullOrEmpty(comDrow["NUMBEROFDAYSACCRUED"].ToString()) ? 0 : Convert.ToInt32(comDrow["NUMBEROFDAYSACCRUED"]);

    //                            if (commColumns.Contains("COMMON_ID"))
    //                                comModl.COMMON_ID = string.IsNullOrEmpty(comDrow["COMMON_ID"].ToString()) ? 0 : Convert.ToInt32(comDrow["COMMON_ID"].ToString());

    //                            if (commColumns.Contains("TIMEOFSLATE"))
    //                                comModl.TIMEOFSLATE = string.IsNullOrEmpty(comDrow["TIMEOFSLATE"].ToString()) ? "" : Convert.ToDateTime(comDrow["TIMEOFSLATE"].ToString()).ToString("hh:mm:ss"); //varchar

    //                            if (commColumns.Contains("TIMEOFSALESTICKET"))
    //                                comModl.TIMEOFSALESTICKET = string.IsNullOrEmpty(comDrow["TIMEOFSALESTICKET"].ToString()) ? "" : Convert.ToDateTime(comDrow["TIMEOFSALESTICKET"].ToString()).ToString("hh:mm:ss"); // varchar

    //                            if (commColumns.Contains("LASTLOGIN"))
    //                                comModl.LASTLOGIN = string.IsNullOrEmpty(comDrow["LASTLOGIN"].ToString()) ? "" : comDrow["LASTLOGIN"].ToString();

    //                            if (commColumns.Contains("MASTERTICKETNUMBER"))
    //                                comModl.MASTERTICKETNUMBER = string.IsNullOrEmpty(comDrow["MASTERTICKETNUMBER"].ToString()) ? "" : comDrow["MASTERTICKETNUMBER"].ToString();

    //                            if (commColumns.Contains("ConcessionStipulationVarianceRepoRate"))
    //                                comModl.CONCSTIPULATIONVARREPORATE = string.IsNullOrEmpty(comDrow["ConcessionStipulationVarianceRepoRate"].ToString()) ? 0 : Convert.ToDecimal(comDrow["ConcessionStipulationVarianceRepoRate"]); // number ? dec

    //                            if (commColumns.Contains("SALESPERSONLOGIN"))
    //                                comModl.SALESPERSONLOGIN = string.IsNullOrEmpty(comDrow["SALESPERSONLOGIN"].ToString()) ? "" : comDrow["SALESPERSONLOGIN"].ToString();

    //                            if (commColumns.Contains("SETTLEMENTCURRENCYISOCODE"))
    //                                comModl.SETTLEMENTCURRENCYISOCODE = string.IsNullOrEmpty(comDrow["SETTLEMENTCURRENCYISOCODE"].ToString()) ? "" : comDrow["SETTLEMENTCURRENCYISOCODE"].ToString();

    //                            if (commColumns.Contains("SETTLEMENTCURRENCYRATE"))
    //                                comModl.SETTLEMENTCURRENCYRATE = string.IsNullOrEmpty(comDrow["SETTLEMENTCURRENCYRATE"].ToString()) ? 0 : Convert.ToDecimal(comDrow["SETTLEMENTCURRENCYRATE"]);

    //                            if (commColumns.Contains("CANCELDUETOCORRECTION"))
    //                                comModl.CANCELDUETOCORRECTION = string.IsNullOrEmpty(comDrow["CANCELDUETOCORRECTION"].ToString()) ? "" : comDrow["CANCELDUETOCORRECTION"].ToString();

    //                            if (commColumns.Contains("INVERTFLAG"))
    //                                comModl.INVERTFLAG = string.IsNullOrEmpty(comDrow["INVERTFLAG"].ToString()) ? "" : comDrow["INVERTFLAG"].ToString();

    //                            if (commColumns.Contains("WORKOUTDATE"))
    //                                comModl.WORKOUTDATE = string.IsNullOrEmpty(comDrow["WORKOUTDATE"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("01-01-1900")) : Convert.ToDateTime(comDrow["WORKOUTDATE"]);

    //                            if (commColumns.Contains("WORKOUTPRICE"))

    //                                comModl.WORKOUTPRICE = string.IsNullOrEmpty(comDrow["WORKOUTPRICE"].ToString()) ? 0 : Convert.ToDecimal(comDrow["WORKOUTPRICE"].ToString());

    //                            if (commColumns.Contains("AUTOEXTRADEFLAG"))

    //                                comModl.AUTOEXTRADEFLAG = string.IsNullOrEmpty(comDrow["AUTOEXTRADEFLAG"].ToString()) ? "" : comDrow["AUTOEXTRADEFLAG"].ToString();

    //                            if (commColumns.Contains("TRADERTOTRADERTRADEFLAG"))
    //                                comModl.TRADERTOTRADERTRADEFLAG = string.IsNullOrEmpty(comDrow["TRADERTOTRADERTRADEFLAG"].ToString()) ? "" : comDrow["TRADERTOTRADERTRADEFLAG"].ToString();

    //                            if (commColumns.Contains("VERSION"))
    //                                comModl.VERSION = string.IsNullOrEmpty(comDrow["VERSION"].ToString()) ? 0 : Convert.ToInt32(comDrow["VERSION"]);// number

    //                            if (commColumns.Contains("UNIQUEBLOOMBERGID"))
    //                                comModl.UNIQUEBLOOMBERGID = string.IsNullOrEmpty(comDrow["UNIQUEBLOOMBERGID"].ToString()) ? "" : comDrow["UNIQUEBLOOMBERGID"].ToString();

    //                            if (commColumns.Contains("EXTENDEDPRECISIONPRICE"))
    //                                comModl.EXTENDEDPRECISIONPRICE = string.IsNullOrEmpty(comDrow["EXTENDEDPRECISIONPRICE"].ToString()) ? 0 : Convert.ToDecimal(comDrow["EXTENDEDPRECISIONPRICE"]);

    //                            if (commColumns.Contains("SYSTEMDATE"))
    //                                comModl.SYSTEMDATE = string.IsNullOrEmpty(comDrow["SYSTEMDATE"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("01-01-1900")) : Convert.ToDateTime(comDrow["SYSTEMDATE"].ToString());

    //                            if (commColumns.Contains("IMPACTFLAG"))
    //                            {
    //                                bool imactflg = string.IsNullOrEmpty(comDrow["IMPACTFLAG"].ToString()) ? false : Convert.ToBoolean(comDrow["IMPACTFLAG"]);

    //                                if (imactflg)
    //                                {
    //                                    comModl.IMPACTFLAG = "1";
    //                                }
    //                                else
    //                                {
    //                                    comModl.IMPACTFLAG = "0";
    //                                }
    //                            }
    //                            if (commColumns.Contains("TRADERACCOUNTNUMBER"))
    //                                comModl.TRADERACCOUNTNUMBER = string.IsNullOrEmpty(comDrow["TRADERACCOUNTNUMBER"].ToString()) ? 0 : Convert.ToInt32(comDrow["TRADERACCOUNTNUMBER"].ToString());

    //                            if (commColumns.Contains("SETTLEMENTLOCATIONABBREVIATION"))
    //                                comModl.SETTLEMENTLOCATIONABBREVIATION = string.IsNullOrEmpty(comDrow["SETTLEMENTLOCATIONABBREVIATION"].ToString()) ? "" : comDrow["SETTLEMENTLOCATIONABBREVIATION"].ToString();

    //                            if (commColumns.Contains("TRANSACTIONTYPE"))
    //                                comModl.TRANSACTIONTYPE = string.IsNullOrEmpty(comDrow["TRANSACTIONTYPE"].ToString()) ? "" : comDrow["TRANSACTIONTYPE"].ToString();
    //                            if (commColumns.Contains("INTERESTATMATURITY"))
    //                                comModl.INTERESTATMATURITY = string.IsNullOrEmpty(comDrow["INTERESTATMATURITY"].ToString()) ? 0 : Convert.ToDecimal(comDrow["INTERESTATMATURITY"]); // number ? dec

    //                            if (commColumns.Contains("ENTEREDTICKETUSERID"))
    //                                comModl.ENTEREDTICKETUSERID = string.IsNullOrEmpty(comDrow["ENTEREDTICKETUSERID"].ToString()) ? 0 : Convert.ToInt32(comDrow["ENTEREDTICKETUSERID"].ToString());

    //                            if (commColumns.Contains("ALLOCATEDTICKETUSERID"))
    //                                comModl.ALLOCATEDTICKETUSERID = string.IsNullOrEmpty(comDrow["ALLOCATEDTICKETUSERID"].ToString()) ? 0 : Convert.ToInt32(comDrow["ALLOCATEDTICKETUSERID"]);

    //                            if (commColumns.Contains("OMREASONCODE"))
    //                                comModl.OMREASONCODE = string.IsNullOrEmpty(comDrow["OMREASONCODE"].ToString()) ? "" : comDrow["OMREASONCODE"].ToString();

    //                            if (commColumns.Contains("OMSBLOCKNAME"))
    //                                comModl.OMSBLOCKNAME = string.IsNullOrEmpty(comDrow["OMSBLOCKNAME"].ToString()) ? "" : comDrow["OMSBLOCKNAME"].ToString();

    //                            if (commColumns.Contains("QUOTETYPEINDICATOR"))
    //                                comModl.QUOTETYPEINDICATOR = string.IsNullOrEmpty(comDrow["QUOTETYPEINDICATOR"].ToString()) ? 0 : Convert.ToInt32(comDrow["QUOTETYPEINDICATOR"]); //number ?int

    //                            if (commColumns.Contains("ISSUEDATE"))
    //                                comModl.ISSUEDATE = string.IsNullOrEmpty(comDrow["ISSUEDATE"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("01-01-1900")) : Convert.ToDateTime(comDrow["ISSUEDATE"]);

    //                            if (commColumns.Contains("FIRMTRADEDATE"))
    //                                comModl.FIRMTRADEDATE = string.IsNullOrEmpty(comDrow["FIRMTRADEDATE"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("01-01-1900")) : Convert.ToDateTime(comDrow["FIRMTRADEDATE"]);

    //                            if (commColumns.Contains("FIRMASOFDATE"))
    //                                comModl.FIRMASOFDATE = string.IsNullOrEmpty(comDrow["FIRMASOFDATE"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("01-01-1900")) : Convert.ToDateTime(comDrow["FIRMASOFDATE"]);

    //                            if (commColumns.Contains("FIRMTIMEOFSLATE"))
    //                                comModl.FIRMTIMEOFSLATE = string.IsNullOrEmpty(comDrow["FIRMTIMEOFSLATE"].ToString()) ? "" : Convert.ToDateTime(comDrow["FIRMTIMEOFSLATE"].ToString()).ToString("hh:mm:ss"); // varchar

    //                            if (commColumns.Contains("FIRMTIMEOFSALESTICKET"))
    //                                comModl.FIRMTIMEOFSALESTICKET = string.IsNullOrEmpty(comDrow["FIRMTIMEOFSALESTICKET"].ToString()) ? "" : Convert.ToDateTime(comDrow["FIRMTIMEOFSALESTICKET"].ToString()).ToString("hh:mm:ss"); // varchar

    //                            if (commColumns.Contains("STEPOUTBROKER"))
    //                                comModl.STEPOUTBROKER = string.IsNullOrEmpty(comDrow["STEPOUTBROKER"].ToString()) ? "" : comDrow["STEPOUTBROKER"].ToString();

    //                            if (commColumns.Contains("SOFTDOLLARFLAG"))
    //                                comModl.SOFTDOLLARFLAG = string.IsNullOrEmpty(comDrow["SOFTDOLLARFLAG"].ToString()) ? "" : comDrow["SOFTDOLLARFLAG"].ToString();

    //                            if (commColumns.Contains("TIPFACTORESTIMATED"))
    //                                comModl.TIPFACTORESTIMATED = string.IsNullOrEmpty(comDrow["TIPFACTORESTIMATED"].ToString()) ? "" : comDrow["TIPFACTORESTIMATED"].ToString();

    //                            if (commColumns.Contains("TOTALTRADEAMOUNT"))
    //                                comModl.TOTALTRADEAMOUNT = string.IsNullOrEmpty(comDrow["TOTALTRADEAMOUNT"].ToString()) ? 0 : Convert.ToDecimal(comDrow["TOTALTRADEAMOUNT"]);

    //                            if (commColumns.Contains("MASTERACCOUNT"))
    //                                comModl.MASTERACCOUNT = string.IsNullOrEmpty(comDrow["MASTERACCOUNT"].ToString()) ? "" : comDrow["MASTERACCOUNT"].ToString();

    //                            if (commColumns.Contains("MASTERACCOUNTNAME"))
    //                                comModl.MASTERACCOUNTNAME = string.IsNullOrEmpty(comDrow["MASTERACCOUNTNAME"].ToString()) ? "" : comDrow["MASTERACCOUNTNAME"].ToString();

    //                            if (commColumns.Contains("CTMMATCHSTATUS"))
    //                                comModl.CTMMATCHSTATUS = string.IsNullOrEmpty(comDrow["CTMMATCHSTATUS"].ToString()) ? "" : comDrow["CTMMATCHSTATUS"].ToString();

    //                            if (commColumns.Contains("MATCHDATE"))
    //                                comModl.MATCHDATE = string.IsNullOrEmpty(comDrow["MATCHDATE"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("01-01-1900")) : Convert.ToDateTime(comDrow["MATCHDATE"]);

    //                            if (commColumns.Contains("ISDIRTYPRICE"))
    //                                comModl.ISDIRTYPRICE = string.IsNullOrEmpty(comDrow["ISDIRTYPRICE"].ToString()) ? "" : comDrow["ISDIRTYPRICE"].ToString();

    //                            if (commColumns.Contains("TSAMINDICATOR"))
    //                                comModl.TSAMINDICATOR = string.IsNullOrEmpty(comDrow["TSAMINDICATOR"].ToString()) ? "" : comDrow["TSAMINDICATOR"].ToString();

    //                            if (commColumns.Contains("TAXLOTMETHOD"))
    //                                comModl.TAXLOTMETHOD = string.IsNullOrEmpty(comDrow["TAXLOTMETHOD"].ToString()) ? "" : comDrow["TAXLOTMETHOD"].ToString();

    //                            if (commColumns.Contains("SECURITYPRICE"))
    //                                comModl.SECURITYPRICE = string.IsNullOrEmpty(comDrow["SECURITYPRICE"].ToString()) ? 0 : Convert.ToDecimal(comDrow["SECURITYPRICE"]);

    //                            if (commColumns.Contains("LOCALEXCHANGE"))
    //                                comModl.LOCALEXCHANGE = string.IsNullOrEmpty(comDrow["LOCALEXCHANGE"].ToString()) ? "" : comDrow["LOCALEXCHANGE"].ToString();

    //                            if (commColumns.Contains("SETTLEMENTAMOUNT"))
    //                                comModl.SETTLEMENTAMOUNT = string.IsNullOrEmpty(comDrow["SETTLEMENTAMOUNT"].ToString()) ? 0 : Convert.ToDecimal(comDrow["SETTLEMENTAMOUNT"]);

    //                            if (commColumns.Contains("SECURITYPRICE"))
    //                                comModl.FUNDCCYPRINCIPAL = string.IsNullOrEmpty(comDrow["SECURITYPRICE"].ToString()) ? 0 : Convert.ToDecimal(comDrow["FUNDCCYPRINCIPAL"]);

    //                            if (commColumns.Contains("FUNDCCYACCRUED"))
    //                                comModl.FUNDCCYACCRUED = string.IsNullOrEmpty(comDrow["FUNDCCYACCRUED"].ToString()) ? 0 : Convert.ToDecimal(comDrow["FUNDCCYACCRUED"].ToString());

    //                            if (commColumns.Contains("FUNDCCYTOTALCOMMISSION"))
    //                                comModl.FUNDCCYTOTALCOMMISSION = string.IsNullOrEmpty(comDrow["FUNDCCYTOTALCOMMISSION"].ToString()) ? 0 : Convert.ToDecimal(comDrow["FUNDCCYTOTALCOMMISSION"]);

    //                            if (commColumns.Contains("REDEMPTIONCCYPRINCIPAL"))
    //                                comModl.REDEMPTIONCCYPRINCIPAL = string.IsNullOrEmpty(comDrow["REDEMPTIONCCYPRINCIPAL"].ToString()) ? 0 : Convert.ToDecimal(comDrow["REDEMPTIONCCYPRINCIPAL"]);

    //                            if (commColumns.Contains("REDEMPTIONCCYACCRUED"))
    //                                comModl.REDEMPTIONCCYACCRUED = string.IsNullOrEmpty(comDrow["REDEMPTIONCCYACCRUED"].ToString()) ? 0 : Convert.ToDecimal(comDrow["REDEMPTIONCCYACCRUED"]);

    //                            if (commColumns.Contains("REDEMPTIONTOTALCOMMISSION"))
    //                                comModl.REDEMPTIONTOTALCOMMISSION = string.IsNullOrEmpty(comDrow["REDEMPTIONTOTALCOMMISSION"].ToString()) ? 0 : Convert.ToDecimal(comDrow["REDEMPTIONTOTALCOMMISSION"]);

    //                            if (commColumns.Contains("SETTLEMENTCCYPRINCIPAL"))
    //                                comModl.SETTLEMENTCCYPRINCIPAL = string.IsNullOrEmpty(comDrow["SETTLEMENTCCYPRINCIPAL"].ToString()) ? 0 : Convert.ToDecimal(comDrow["SETTLEMENTCCYPRINCIPAL"]);

    //                            if (commColumns.Contains("SETTLEMENTCCYACCRUED"))
    //                                comModl.SETTLEMENTCCYACCRUED = string.IsNullOrEmpty(comDrow["SETTLEMENTCCYACCRUED"].ToString()) ? 0 : Convert.ToDecimal(comDrow["SETTLEMENTCCYACCRUED"]);

    //                            if (commColumns.Contains("DAYSTOMATURITY"))
    //                                comModl.DAYSTOMATURITY = string.IsNullOrEmpty(comDrow["DAYSTOMATURITY"].ToString()) ? 0 : Convert.ToInt32(comDrow["DAYSTOMATURITY"]);

    //                            if (commColumns.Contains("SETTLEMENTCCYTOTALCOMMISSION"))
    //                                comModl.SETTLEMENTCCYTOTALCOMMISSION = string.IsNullOrEmpty(comDrow["SETTLEMENTCCYTOTALCOMMISSION"].ToString()) ? 0 : Convert.ToDecimal(comDrow["SETTLEMENTCCYTOTALCOMMISSION"]);

    //                            if (commColumns.Contains("PRIMARYEXCHANGECODE"))
    //                                comModl.PRIMARYEXCHANGECODE = string.IsNullOrEmpty(comDrow["PRIMARYEXCHANGECODE"].ToString()) ? "" : comDrow["PRIMARYEXCHANGECODE"].ToString();

    //                            if (commColumns.Contains("EXECUTIONPLATFORM"))
    //                                comModl.EXECUTIONPLATFORM = string.IsNullOrEmpty(comDrow["EXECUTIONPLATFORM"].ToString()) ? "" : comDrow["EXECUTIONPLATFORM"].ToString();

    //                            if (commColumns.Contains("CLIENTAUTH"))
    //                                comModl.CLIENTAUTH = string.IsNullOrEmpty(comDrow["CLIENTAUTH"].ToString()) ? "" : comDrow["CLIENTAUTH"].ToString();

    //                            if (commColumns.Contains("LASTLOGINUUID"))
    //                                comModl.LASTLOGINUUID = string.IsNullOrEmpty(comDrow["LASTLOGINUUID"].ToString()) ? 0 : Convert.ToInt32(comDrow["LASTLOGINUUID"]);

    //                            if (commColumns.Contains("ORIGINALTKTID"))
    //                                comModl.ORIGINALTKTID = string.IsNullOrEmpty(comDrow["ORIGINALTKTID"].ToString()) ? 0 : Convert.ToInt32(comDrow["ORIGINALTKTID"]);

    //                            if (commColumns.Contains("SETTLEMENTCCYPRICE"))
    //                                comModl.SETTLEMENTCCYPRICE = string.IsNullOrEmpty(comDrow["SETTLEMENTCCYPRICE"].ToString()) ? 0 : Convert.ToDecimal(comDrow["SETTLEMENTCCYPRICE"]);

    //                            if (commColumns.Contains("ORDERSTATUS"))
    //                                comModl.ORDERSTATUS = string.IsNullOrEmpty(comDrow["ORDERSTATUS"].ToString()) ? "" : comDrow["ORDERSTATUS"].ToString();

    //                            if (commColumns.Contains("RTTMINDICATOR"))
    //                                comModl.RTTMINDICATOR = string.IsNullOrEmpty(comDrow["RTTMINDICATOR"].ToString()) ? "" : comDrow["RTTMINDICATOR"].ToString();

    //                            if (commColumns.Contains("RTTMREFERENCEID"))
    //                                comModl.RTTMREFERENCEID = string.IsNullOrEmpty(comDrow["RTTMREFERENCEID"].ToString()) ? "" : comDrow["RTTMREFERENCEID"].ToString();

    //                            if (commColumns.Contains("CounterpartyEncodedLongName"))
    //                                comModl.CPARTYENCODEDLONGNAME = string.IsNullOrEmpty(comDrow["CounterpartyEncodedLongName"].ToString()) ? "" : comDrow["CounterpartyEncodedLongName"].ToString();

    //                            if (commColumns.Contains("BLOOMBERGGLOBALIDENTIFIER"))
    //                                comModl.BLOOMBERGGLOBALIDENTIFIER = string.IsNullOrEmpty(comDrow["BLOOMBERGGLOBALIDENTIFIER"].ToString()) ? "" : comDrow["BLOOMBERGGLOBALIDENTIFIER"].ToString();

    //                            if (commColumns.Contains("SECONDHALFTDR2TDRTRADE"))
    //                                comModl.SECONDHALFTDR2TDRTRADE = string.IsNullOrEmpty(comDrow["SECONDHALFTDR2TDRTRADE"].ToString()) ? "" : comDrow["SECONDHALFTDR2TDRTRADE"].ToString();

    //                            if (commColumns.Contains("CashReversalOffsetTicketIndicator"))
    //                                comModl.CASHREVLOFFSETTICKETIND = string.IsNullOrEmpty(comDrow["CashReversalOffsetTicketIndicator"].ToString()) ? "" : comDrow["CashReversalOffsetTicketIndicator"].ToString();

    //                            if (commColumns.Contains("LIMITPRICE"))
    //                                comModl.LIMITPRICE = string.IsNullOrEmpty(comDrow["LIMITPRICE"].ToString()) ? 0 : Convert.ToDecimal(comDrow["LIMITPRICE"].ToString());

    //                            if (commColumns.Contains("TRADEFLATFLAG"))
    //                            {
    //                                bool Tradflg = string.IsNullOrEmpty(comDrow["TRADEFLATFLAG"].ToString()) ? false : Convert.ToBoolean(comDrow["TRADEFLATFLAG"]);

    //                                if (Tradflg)
    //                                {
    //                                    comModl.TRADEFLATFLAG = "1";
    //                                }
    //                                else
    //                                {
    //                                    comModl.TRADEFLATFLAG = "0";
    //                                }
    //                            }

    //                            //comModl.TRADEFLATFLAG = string.IsNullOrEmpty(comDrow["TRADEFLATFLAG"].ToString()) ? "" : comDrow["TRADEFLATFLAG"].ToString();
    //                            if (commColumns.Contains("MSRBREPORTABLE"))
    //                                comModl.MSRBREPORTABLE = string.IsNullOrEmpty(comDrow["MSRBREPORTABLE"].ToString()) ? "" : comDrow["MSRBREPORTABLE"].ToString();

    //                            if (commColumns.Contains("TRADEFEED_ID"))
    //                                comModl.TRADEFEED_ID = string.IsNullOrEmpty(comDrow["TRADEFEED_ID"].ToString()) ? 0 : Convert.ToInt32(comDrow["TRADEFEED_ID"]); // varchar

    //                            comModl.FILENAME = filname;
    //                            comModl.FILEDATE = FileDate;
    //                            comModl.FILENUMBER = fileNumber;

    //                            // start of new column adding
    //                            if (commColumns.Contains("ACCRUEDINTERESTOVERDRIVE"))
    //                                comModl.ACCRUEDINTERESTOVERDRIVE = string.IsNullOrEmpty(comDrow["ACCRUEDINTERESTOVERDRIVE"].ToString()) ? "" : comDrow["ACCRUEDINTERESTOVERDRIVE"].ToString();

    //                            if (commColumns.Contains("AFFILIATEDTICKETNUMBER"))
    //                                comModl.AFFILIATEDTICKETNUMBER = string.IsNullOrEmpty(comDrow["AFFILIATEDTICKETNUMBER"].ToString()) ? 0 : Convert.ToInt32(comDrow["AFFILIATEDTICKETNUMBER"]);

    //                            if (commColumns.Contains("AGENCYLINKEDTICKETNUMBER"))
    //                                comModl.AGENCYLINKEDTICKETNUMBER = string.IsNullOrEmpty(comDrow["AGENCYLINKEDTICKETNUMBER"].ToString()) ? 0 : Convert.ToInt32(comDrow["AGENCYLINKEDTICKETNUMBER"]);

    //                            if (commColumns.Contains("AGENCYTRADEREFERENCETICKETNUMB"))
    //                                comModl.AGENCYTRADEREFERENCETICKETN = string.IsNullOrEmpty(comDrow["AGENCYTRADEREFERENCETICKETNUMB"].ToString()) ? 0 : Convert.ToInt32(comDrow["AGENCYTRADEREFERENCETICKETNUMB"]);

    //                            if (commColumns.Contains("ALERTID"))
    //                                comModl.ALERTID = string.IsNullOrEmpty(comDrow["ALERTID"].ToString()) ? "" : comDrow["ALERTID"].ToString();

    //                            if (commColumns.Contains("ALTERNATIVESECURITYID"))
    //                                comModl.ALTERNATIVESECURITYID = string.IsNullOrEmpty(comDrow["ALTERNATIVESECURITYID"].ToString()) ? "" : comDrow["ALTERNATIVESECURITYID"].ToString();

    //                            if (commColumns.Contains("ASOFDATETIMEZONETXT"))
    //                                comModl.ASOFDATETIMEZONETXT = string.IsNullOrEmpty(comDrow["ASOFDATETIMEZONETXT"].ToString()) ? "" : comDrow["ASOFDATETIMEZONETXT"].ToString();

    //                            if (commColumns.Contains("AUTOEXAPPLNAME"))
    //                                comModl.AUTOEXAPPLNAME = string.IsNullOrEmpty(comDrow["AUTOEXAPPLNAME"].ToString()) ? 0 : Convert.ToInt32(comDrow["AUTOEXAPPLNAME"]);

    //                            if (commColumns.Contains("AUTOEXBROKERNAME"))
    //                                comModl.AUTOEXBROKERNAME = string.IsNullOrEmpty(comDrow["AUTOEXBROKERNAME"].ToString()) ? 0 : Convert.ToInt32(comDrow["AUTOEXBROKERNAME"]);

    //                            if (commColumns.Contains("AUTOEXDEALERSINCOMP"))
    //                                comModl.AUTOEXDEALERSINCOMP = string.IsNullOrEmpty(comDrow["AUTOEXDEALERSINCOMP"].ToString()) ? 0 : Convert.ToInt32(comDrow["AUTOEXDEALERSINCOMP"]);

    //                            if (commColumns.Contains("AUTOEXNUMBER"))
    //                                comModl.AUTOEXNUMBER = string.IsNullOrEmpty(comDrow["AUTOEXNUMBER"].ToString()) ? 0 : Convert.ToInt32(comDrow["AUTOEXNUMBER"]);

    //                            if (commColumns.Contains("AUTOEXORDERINQUIRY"))
    //                                comModl.AUTOEXORDERINQUIRY = string.IsNullOrEmpty(comDrow["AUTOEXORDERINQUIRY"].ToString()) ? "" : comDrow["AUTOEXORDERINQUIRY"].ToString();

    //                            if (commColumns.Contains("AUTOEXSTATUS"))
    //                                comModl.AUTOEXSTATUS = string.IsNullOrEmpty(comDrow["AUTOEXSTATUS"].ToString()) ? "" : comDrow["AUTOEXSTATUS"].ToString();

    //                            if (commColumns.Contains("AVGLIFE"))
    //                                comModl.AVGLIFE = string.IsNullOrEmpty(comDrow["AVGLIFE"].ToString()) ? 0 : Convert.ToDecimal(comDrow["AVGLIFE"].ToString());

    //                            if (commColumns.Contains("BANKDESTINATIONID"))
    //                                comModl.BANKDESTINATIONID = string.IsNullOrEmpty(comDrow["BANKDESTINATIONID"].ToString()) ? "" : comDrow["BANKDESTINATIONID"].ToString();

    //                            if (commColumns.Contains("BASEPRICEONTKT"))
    //                                comModl.BASEPRICEONTKT = string.IsNullOrEmpty(comDrow["BASEPRICEONTKT"].ToString()) ? 0 : Convert.ToDecimal(comDrow["BASEPRICEONTKT"].ToString());

    //                            if (commColumns.Contains("BASEPRICEONTKT"))
    //                                comModl.BASEPRICEONTKT = string.IsNullOrEmpty(comDrow["BASEPRICEONTKT"].ToString()) ? 0 : Convert.ToDecimal(comDrow["BASEPRICEONTKT"].ToString());

    //                            if (commColumns.Contains("BASKETID"))
    //                                comModl.BASKETID = string.IsNullOrEmpty(comDrow["BASKETID"].ToString()) ? "" : comDrow["BASKETID"].ToString();

    //                            if (commColumns.Contains("BLOOMBERGFUNCTIONS"))
    //                                comModl.BLOOMBERGFUNCTIONS = string.IsNullOrEmpty(comDrow["BLOOMBERGFUNCTIONS"].ToString()) ? "" : comDrow["BLOOMBERGFUNCTIONS"].ToString();

    //                            if (commColumns.Contains("BLOOMBERGREFERENCENUMBER"))
    //                                comModl.BLOOMBERGREFERENCENUMBER = string.IsNullOrEmpty(comDrow["BLOOMBERGREFERENCENUMBER"].ToString()) ? "" : comDrow["BLOOMBERGREFERENCENUMBER"].ToString();

    //                            if (commColumns.Contains("BUYSIDEREASONCODE"))
    //                                comModl.BUYSIDEREASONCODE = string.IsNullOrEmpty(comDrow["BUYSIDEREASONCODE"].ToString()) ? 0 : Convert.ToInt32(comDrow["BUYSIDEREASONCODE"]);

    //                            if (commColumns.Contains("BXTLOGIN"))
    //                                comModl.BXTLOGIN = string.IsNullOrEmpty(comDrow["BXTLOGIN"].ToString()) ? "" : comDrow["BXTLOGIN"].ToString();

    //                            if (commColumns.Contains("CANCELDATE"))
    //                                comModl.CANCELDATE = string.IsNullOrEmpty(comDrow["CANCELDATE"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("01-01-1900")) : Convert.ToDateTime(comDrow["CANCELDATE"]);

    //                            if (commColumns.Contains("CANCELDUETOMATCH"))
    //                                comModl.CANCELDUETOMATCH = string.IsNullOrEmpty(comDrow["CANCELDUETOMATCH"].ToString()) ? "" : comDrow["CANCELDUETOMATCH"].ToString();

    //                            if (commColumns.Contains("CANCELTRANSACTIONPNL_ATTIBUTIO"))
    //                                comModl.CANCELTRANSACTIONPNL_ATTIBUT = string.IsNullOrEmpty(comDrow["CANCELTRANSACTIONPNL_ATTIBUTIO"].ToString()) ? 0 : Convert.ToDecimal(comDrow["CANCELTRANSACTIONPNL_ATTIBUTIO"].ToString());

    //                            if (commColumns.Contains("CASHBROKERCODE"))
    //                                comModl.CASHBROKERCODE = string.IsNullOrEmpty(comDrow["CASHBROKERCODE"].ToString()) ? "" : comDrow["CASHBROKERCODE"].ToString();

    //                            if (commColumns.Contains("CASHLINKEDREVERSALTICKETNUMBER"))
    //                                comModl.CASHLINKEDREVERSALTICKETNUMBER = string.IsNullOrEmpty(comDrow["CASHLINKEDREVERSALTICKETNUMBER"].ToString()) ? 0 : Convert.ToInt32(comDrow["CASHLINKEDREVERSALTICKETNUMBER"]);

    //                            if (commColumns.Contains("CASHREVERSALDATE"))
    //                                comModl.CASHREVERSALDATE = string.IsNullOrEmpty(comDrow["CASHREVERSALDATE"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("01-01-1900")) : Convert.ToDateTime(comDrow["CASHREVERSALDATE"]);

    //                            if (commColumns.Contains("CLEARINGBROKER"))
    //                                comModl.CLEARINGBROKER = string.IsNullOrEmpty(comDrow["CLEARINGBROKER"].ToString()) ? "" : comDrow["CLEARINGBROKER"].ToString();

    //                            if (commColumns.Contains("CONCESSIONSTIPULATIONVARIANCER"))
    //                                comModl.CONCESSIONSTIPULATIONVARIANCER = string.IsNullOrEmpty(comDrow["CONCESSIONSTIPULATIONVARIANCER"].ToString()) ? 0 : Convert.ToDecimal(comDrow["CONCESSIONSTIPULATIONVARIANCER"].ToString());

    //                            if (commColumns.Contains("CONTINGENTFLAT"))
    //                                comModl.CONTINGENTFLAT = string.IsNullOrEmpty(comDrow["CONTINGENTFLAT"].ToString()) ? "" : comDrow["CONTINGENTFLAT"].ToString();

    //                            if (commColumns.Contains("CONTRACTSIZE"))
    //                                comModl.CONTRACTSIZE = string.IsNullOrEmpty(comDrow["CONTRACTSIZE"].ToString()) ? 0 : Convert.ToInt32(comDrow["CONTRACTSIZE"]);

    //                            if (commColumns.Contains("CONVEXITY"))
    //                                comModl.CONVEXITY = string.IsNullOrEmpty(comDrow["CONVEXITY"].ToString()) ? 0 : Convert.ToDecimal(comDrow["CONVEXITY"].ToString());

    //                            if (commColumns.Contains("CORRECTDACTUALNAV"))
    //                                comModl.CORRECTDACTUALNAV = string.IsNullOrEmpty(comDrow["CORRECTDACTUALNAV"].ToString()) ? "" : comDrow["CORRECTDACTUALNAV"].ToString();

    //                            if (commColumns.Contains("CORRECTEDTRANSACTIONPNL_ATTRIB"))
    //                                comModl.CORRECTEDTRANSACTIONPNL_ATTRIB = string.IsNullOrEmpty(comDrow["CORRECTEDTRANSACTIONPNL_ATTRIB"].ToString()) ? 0 : Convert.ToDecimal(comDrow["CORRECTEDTRANSACTIONPNL_ATTRIB"].ToString());

    //                            if (commColumns.Contains("CORRECTIONASORIGINAL"))
    //                                comModl.CORRECTIONASORIGINAL = string.IsNullOrEmpty(comDrow["CORRECTIONASORIGINAL"].ToString()) ? "" : comDrow["CORRECTIONASORIGINAL"].ToString();

    //                            if (commColumns.Contains("CURRENTFLOATERCOUPON"))
    //                                comModl.CURRENTFLOATERCOUPON = string.IsNullOrEmpty(comDrow["CURRENTFLOATERCOUPON"].ToString()) ? 0 : Convert.ToDecimal(comDrow["CURRENTFLOATERCOUPON"].ToString());

    //                            if (commColumns.Contains("CUSTODYSAFEKEEPINGNUMBER"))
    //                                comModl.CUSTODYSAFEKEEPINGNUMBER = string.IsNullOrEmpty(comDrow["CUSTODYSAFEKEEPINGNUMBER"].ToString()) ? "" : comDrow["CUSTODYSAFEKEEPINGNUMBER"].ToString();

    //                            if (commColumns.Contains("CUTTRADE"))
    //                                comModl.CUTTRADE = string.IsNullOrEmpty(comDrow["CUTTRADE"].ToString()) ? "" : comDrow["CUTTRADE"].ToString();

    //                            if (commColumns.Contains("DATETRADEWASAUTHORIZED"))
    //                                comModl.DATETRADEWASAUTHORIZED = string.IsNullOrEmpty(comDrow["DATETRADEWASAUTHORIZED"].ToString()) ? "" : comDrow["DATETRADEWASAUTHORIZED"].ToString();

    //                            if (commColumns.Contains("DELAYEDDELIVERYDATE"))
    //                                comModl.DELAYEDDELIVERYDATE = string.IsNullOrEmpty(comDrow["DELAYEDDELIVERYDATE"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("01-01-1900")) : Convert.ToDateTime(comDrow["DELAYEDDELIVERYDATE"]);

    //                            if (commColumns.Contains("DIRTYPRICE"))
    //                                comModl.DIRTYPRICE = string.IsNullOrEmpty(comDrow["DIRTYPRICE"].ToString()) ? "" : comDrow["DIRTYPRICE"].ToString();

    //                            if (commColumns.Contains("DISCOUNTRATE"))
    //                                comModl.DISCOUNTRATE = string.IsNullOrEmpty(comDrow["DISCOUNTRATE"].ToString()) ? 0 : Convert.ToInt32(comDrow["DISCOUNTRATE"]);

    //                            if (commColumns.Contains("EDITTRANSATIONPNL_ATTRIBUTION"))
    //                                comModl.EDITTRANSATIONPNL_ATTRIBUTION = string.IsNullOrEmpty(comDrow["EDITTRANSATIONPNL_ATTRIBUTION"].ToString()) ? 0 : Convert.ToDecimal(comDrow["EDITTRANSATIONPNL_ATTRIBUTION"].ToString());

    //                            if (commColumns.Contains("ELECTRONICEXECUTIONFLAG"))
    //                                comModl.ELECTRONICEXECUTIONFLAG = string.IsNullOrEmpty(comDrow["ELECTRONICEXECUTIONFLAG"].ToString()) ? "" : comDrow["ELECTRONICEXECUTIONFLAG"].ToString();

    //                            if (commColumns.Contains("ELECTRONICTRADE"))
    //                                comModl.ELECTRONICTRADE = string.IsNullOrEmpty(comDrow["ELECTRONICTRADE"].ToString()) ? "" : comDrow["ELECTRONICTRADE"].ToString();

    //                            if (commColumns.Contains("ENTRYOFMUTUALFUNDSHARESWASBYSE"))
    //                                comModl.ENTRYOFMUTUALFUNDSHARESWASBYSE = string.IsNullOrEmpty(comDrow["ENTRYOFMUTUALFUNDSHARESWASBYSE"].ToString()) ? "" : comDrow["ENTRYOFMUTUALFUNDSHARESWASBYSE"].ToString();

    //                            if (commColumns.Contains("EQPLSUBFLAG"))
    //                                comModl.EQPLSUBFLAG = string.IsNullOrEmpty(comDrow["EQPLSUBFLAG"].ToString()) ? 0 : Convert.ToInt32(comDrow["EQPLSUBFLAG"]);

    //                            if (commColumns.Contains("ESTIMATEFLAG"))
    //                                comModl.ESTIMATEFLAG = string.IsNullOrEmpty(comDrow["ESTIMATEFLAG"].ToString()) ? "" : comDrow["ESTIMATEFLAG"].ToString();

    //                            if (commColumns.Contains("FEEDREASONCODE"))
    //                                comModl.FEEDREASONCODE = string.IsNullOrEmpty(comDrow["FEEDREASONCODE"].ToString()) ? "" : comDrow["FEEDREASONCODE"].ToString();

    //                            if (commColumns.Contains("FIRMCANCELDATE"))
    //                                comModl.FIRMCANCELDATE = string.IsNullOrEmpty(comDrow["FIRMCANCELDATE"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("01-01-1900")) : Convert.ToDateTime(comDrow["FIRMCANCELDATE"]);

    //                            if (commColumns.Contains("FXSTRATEGYCODE"))
    //                                comModl.FXSTRATEGYCODE = string.IsNullOrEmpty(comDrow["FXSTRATEGYCODE"].ToString()) ? "" : comDrow["FXSTRATEGYCODE"].ToString();

    //                            if (commColumns.Contains("GENERALLEDGERACCOUNT"))
    //                                comModl.GENERALLEDGERACCOUNT = string.IsNullOrEmpty(comDrow["GENERALLEDGERACCOUNT"].ToString()) ? "" : comDrow["GENERALLEDGERACCOUNT"].ToString();

    //                            if (commColumns.Contains("GOODMILLIONNUMBER"))
    //                                comModl.GOODMILLIONNUMBER = string.IsNullOrEmpty(comDrow["GOODMILLIONNUMBER"].ToString()) ? 0 : Convert.ToInt32(comDrow["GOODMILLIONNUMBER"]);

    //                            if (commColumns.Contains("GSPREAD"))
    //                                comModl.GSPREAD = string.IsNullOrEmpty(comDrow["GSPREAD"].ToString()) ? 0 : Convert.ToDecimal(comDrow["GSPREAD"].ToString());

    //                            if (commColumns.Contains("GSPREADCURVEID"))
    //                                comModl.GSPREADCURVEID = string.IsNullOrEmpty(comDrow["GSPREADCURVEID"].ToString()) ? "" : comDrow["GSPREADCURVEID"].ToString();

    //                            if (commColumns.Contains("HEDGETICKETNUMBER"))
    //                                comModl.HEDGETICKETNUMBER = string.IsNullOrEmpty(comDrow["HEDGETICKETNUMBER"].ToString()) ? "" : comDrow["HEDGETICKETNUMBER"].ToString();

    //                            if (commColumns.Contains("INUNITSFLAG"))
    //                                comModl.INUNITSFLAG = string.IsNullOrEmpty(comDrow["INUNITSFLAG"].ToString()) ? "" : comDrow["INUNITSFLAG"].ToString();

    //                            if (commColumns.Contains("IPOFLAG"))
    //                                comModl.IPOFLAG = string.IsNullOrEmpty(comDrow["IPOFLAG"].ToString()) ? "" : comDrow["IPOFLAG"].ToString();

    //                            if (commColumns.Contains("ISCFD"))
    //                                comModl.ISCFD = string.IsNullOrEmpty(comDrow["ISCFD"].ToString()) ? "" : comDrow["ISCFD"].ToString();

    //                            if (commColumns.Contains("ISSUERCOMPANYID"))
    //                                comModl.ISSUERCOMPANYID = string.IsNullOrEmpty(comDrow["ISSUERCOMPANYID"].ToString()) ? "" : comDrow["ISSUERCOMPANYID"].ToString();

    //                            if (commColumns.Contains("JAPANNUMBER"))
    //                                comModl.JAPANNUMBER = string.IsNullOrEmpty(comDrow["JAPANNUMBER"].ToString()) ? "" : comDrow["JAPANNUMBER"].ToString();

    //                            if (commColumns.Contains("JOURNALREASONCODE"))
    //                                comModl.JOURNALREASONCODE = string.IsNullOrEmpty(comDrow["JOURNALREASONCODE"].ToString()) ? "" : comDrow["JOURNALREASONCODE"].ToString();

    //                            if (commColumns.Contains("LONGNOTES"))
    //                                comModl.LONGNOTES = string.IsNullOrEmpty(comDrow["LONGNOTES"].ToString()) ? "" : comDrow["LONGNOTES"].ToString();

    //                            if (commColumns.Contains("LOTNUMBER"))
    //                                comModl.LOTNUMBER = string.IsNullOrEmpty(comDrow["LOTNUMBER"].ToString()) ? "" : comDrow["LOTNUMBER"].ToString();

    //                            if (commColumns.Contains("MIDDLEOFFICETRADEINDICATOR"))
    //                                comModl.MIDDLEOFFICETRADEINDICATOR = string.IsNullOrEmpty(comDrow["MIDDLEOFFICETRADEINDICATOR"].ToString()) ? "" : comDrow["MIDDLEOFFICETRADEINDICATOR"].ToString();

    //                            if (commColumns.Contains("MMKTISSUERCODE"))
    //                                comModl.MMKTISSUERCODE = string.IsNullOrEmpty(comDrow["MMKTISSUERCODE"].ToString()) ? "" : comDrow["MMKTISSUERCODE"].ToString();

    //                            if (commColumns.Contains("MORTGAGEBRADYINDEXBONDFACTOR"))
    //                                comModl.MORTGAGEBRADYINDEXBONDFACTOR = string.IsNullOrEmpty(comDrow["MORTGAGEBRADYINDEXBONDFACTOR"].ToString()) ? "" : comDrow["MORTGAGEBRADYINDEXBONDFACTOR"].ToString();

    //                            if (commColumns.Contains("MSRBPRICECODE"))
    //                                comModl.MSRBPRICECODE = string.IsNullOrEmpty(comDrow["MSRBPRICECODE"].ToString()) ? 0 : Convert.ToInt32(comDrow["MSRBPRICECODE"]);

    //                            if (commColumns.Contains("MTGEFACTORDATE"))
    //                                comModl.MTGEFACTORDATE = string.IsNullOrEmpty(comDrow["MTGEFACTORDATE"].ToString()) ? "" : comDrow["MTGEFACTORDATE"].ToString();

    //                            if (commColumns.Contains("NAMEOFUSERWHOAUTHORIZEDTRADE"))
    //                                comModl.NAMEOFUSERWHOAUTHORIZEDTRADE = string.IsNullOrEmpty(comDrow["NAMEOFUSERWHOAUTHORIZEDTRADE"].ToString()) ? "" : comDrow["NAMEOFUSERWHOAUTHORIZEDTRADE"].ToString();

    //                            if (commColumns.Contains("NAVESTIMATED"))
    //                                comModl.NAVESTIMATED = string.IsNullOrEmpty(comDrow["NAVESTIMATED"].ToString()) ? "" : comDrow["NAVESTIMATED"].ToString();

    //                            if (commColumns.Contains("NETWIRE"))
    //                                comModl.NETWIRE = string.IsNullOrEmpty(comDrow["NETWIRE"].ToString()) ? 0 : Convert.ToInt32(comDrow["NETWIRE"]);

    //                            if (commColumns.Contains("NEWTRANSACTIONPNL_ATTRIBUTION"))
    //                                comModl.NEWTRANSACTIONPNL_ATTRIBUTION = string.IsNullOrEmpty(comDrow["NEWTRANSACTIONPNL_ATTRIBUTION"].ToString()) ? 0 : Convert.ToDecimal(comDrow["NEWTRANSACTIONPNL_ATTRIBUTION"].ToString());

    //                            if (commColumns.Contains("NONREGSETTLEREPORATE"))
    //                                comModl.NONREGSETTLEREPORATE = string.IsNullOrEmpty(comDrow["NONREGSETTLEREPORATE"].ToString()) ? 0 : Convert.ToDecimal(comDrow["NONREGSETTLEREPORATE"].ToString());

    //                            if (commColumns.Contains("OCCOPTIONTICKER"))
    //                                comModl.OCCOPTIONTICKER = string.IsNullOrEmpty(comDrow["OCCOPTIONTICKER"].ToString()) ? "" : comDrow["OCCOPTIONTICKER"].ToString();

    //                            if (commColumns.Contains("OMSGOODTILDATE"))
    //                                comModl.OMSGOODTILDATE = string.IsNullOrEmpty(comDrow["OMSGOODTILDATE"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("01-01-1900")) : Convert.ToDateTime(comDrow["OMSGOODTILDATE"]);

    //                            if (commColumns.Contains("OMSLIMITPX"))
    //                                comModl.OMSLIMITPX = string.IsNullOrEmpty(comDrow["OMSLIMITPX"].ToString()) ? 0 : Convert.ToDecimal(comDrow["OMSLIMITPX"].ToString());

    //                            if (commColumns.Contains("OMSORDERNUMBER"))
    //                                comModl.OMSORDERNUMBER = string.IsNullOrEmpty(comDrow["OMSORDERNUMBER"].ToString()) ? 0 : Convert.ToInt32(comDrow["OMSORDERNUMBER"]);

    //                            if (commColumns.Contains("OMSSTOPPX"))
    //                                comModl.OMSSTOPPX = string.IsNullOrEmpty(comDrow["OMSSTOPPX"].ToString()) ? 0 : Convert.ToDecimal(comDrow["OMSSTOPPX"].ToString());

    //                            if (commColumns.Contains("OMSTIMEINFORCE"))
    //                                comModl.OMSTIMEINFORCE = string.IsNullOrEmpty(comDrow["OMSTIMEINFORCE"].ToString()) ? "" : comDrow["OMSTIMEINFORCE"].ToString();

    //                            if (commColumns.Contains("OMSTRANSACTIONID"))
    //                                comModl.OMSTRANSACTIONID = string.IsNullOrEmpty(comDrow["OMSTRANSACTIONID"].ToString()) ? 0 : Convert.ToInt32(comDrow["OMSTRANSACTIONID"]);

    //                            if (commColumns.Contains("OPTIONCONTRACTSIZE"))
    //                                comModl.OPTIONCONTRACTSIZE = string.IsNullOrEmpty(comDrow["OPTIONCONTRACTSIZE"].ToString()) ? 0 : Convert.ToDecimal(comDrow["OPTIONCONTRACTSIZE"].ToString());

    //                            if (commColumns.Contains("OPTIONSDELTA"))
    //                                comModl.OPTIONSDELTA = string.IsNullOrEmpty(comDrow["OPTIONSDELTA"].ToString()) ? "" : comDrow["OPTIONSDELTA"].ToString();

    //                            if (commColumns.Contains("ORDERTYPE"))
    //                                comModl.ORDERTYPE = string.IsNullOrEmpty(comDrow["ORDERTYPE"].ToString()) ? "" : comDrow["ORDERTYPE"].ToString();

    //                            if (commColumns.Contains("PARTIALALLOCATIONEXISTS"))
    //                                comModl.PARTIALALLOCATIONEXISTS = string.IsNullOrEmpty(comDrow["PARTIALALLOCATIONEXISTS"].ToString()) ? "" : comDrow["PARTIALALLOCATIONEXISTS"].ToString();

    //                            if (commColumns.Contains("PAYNOTIONALAMOUNT"))
    //                                comModl.PAYNOTIONALAMOUNT = string.IsNullOrEmpty(comDrow["PAYNOTIONALAMOUNT"].ToString()) ? 0 : Convert.ToDecimal(comDrow["PAYNOTIONALAMOUNT"].ToString());

    //                            if (commColumns.Contains("PLATFORMNAME"))
    //                                comModl.PLATFORMNAME = string.IsNullOrEmpty(comDrow["PLATFORMNAME"].ToString()) ? "" : comDrow["PLATFORMNAME"].ToString();

    //                            if (commColumns.Contains("POOLNUMBER"))
    //                                comModl.POOLNUMBER = string.IsNullOrEmpty(comDrow["POOLNUMBER"].ToString()) ? 0 : Convert.ToInt32(comDrow["POOLNUMBER"]);

    //                            if (commColumns.Contains("PREPAYMENTSPEEDANDTYPECONTRACT"))
    //                                comModl.PREPAYMENTSPEEDANDTYPECONTRACT = string.IsNullOrEmpty(comDrow["PREPAYMENTSPEEDANDTYPECONTRACT"].ToString()) ? 0 : Convert.ToInt32(comDrow["PREPAYMENTSPEEDANDTYPECONTRACT"]);

    //                            if (commColumns.Contains("PRIMARYMMKTID"))
    //                                comModl.PRIMARYMMKTID = string.IsNullOrEmpty(comDrow["PRIMARYMMKTID"].ToString()) ? "" : comDrow["PRIMARYMMKTID"].ToString();

    //                            if (commColumns.Contains("PRIMEBROKER"))
    //                                comModl.PRIMEBROKER = string.IsNullOrEmpty(comDrow["PRIMEBROKER"].ToString()) ? "" : comDrow["PRIMEBROKER"].ToString();

    //                            if (commColumns.Contains("PRINCIPALAGENCYFLAG"))
    //                                comModl.PRINCIPALAGENCYFLAG = string.IsNullOrEmpty(comDrow["PRINCIPALAGENCYFLAG"].ToString()) ? "" : comDrow["PRINCIPALAGENCYFLAG"].ToString();

    //                            if (commColumns.Contains("PROGRAMBASKETFLAG"))
    //                                comModl.PROGRAMBASKETFLAG = string.IsNullOrEmpty(comDrow["PROGRAMBASKETFLAG"].ToString()) ? "" : comDrow["PROGRAMBASKETFLAG"].ToString();

    //                            if (commColumns.Contains("PUTCALLINDICATOR"))
    //                                comModl.PUTCALLINDICATOR = string.IsNullOrEmpty(comDrow["PUTCALLINDICATOR"].ToString()) ? "" : comDrow["PUTCALLINDICATOR"].ToString();

    //                            if (commColumns.Contains("RECEIVENOTIONAL"))
    //                                comModl.RECEIVENOTIONAL = string.IsNullOrEmpty(comDrow["RECEIVENOTIONAL"].ToString()) ? 0 : Convert.ToDecimal(comDrow["RECEIVENOTIONAL"].ToString());

    //                            if (commColumns.Contains("RELATEDSLATETICKETNUMBER"))
    //                                comModl.RELATEDSLATETICKETNUMBER = string.IsNullOrEmpty(comDrow["RELATEDSLATETICKETNUMBER"].ToString()) ? 0 : Convert.ToInt32(comDrow["RELATEDSLATETICKETNUMBER"]);

    //                            if (commColumns.Contains("REPORTTOFCA"))
    //                                comModl.REPORTTOFCA = string.IsNullOrEmpty(comDrow["REPORTTOFCA"].ToString()) ? "" : comDrow["REPORTTOFCA"].ToString();

    //                            if (commColumns.Contains("RETAILFEED"))
    //                                comModl.RETAILFEED = string.IsNullOrEmpty(comDrow["RETAILFEED"].ToString()) ? "" : comDrow["RETAILFEED"].ToString();

    //                            if (commColumns.Contains("RETAILFEEDWEBTRADEFIELD"))
    //                                comModl.RETAILFEEDWEBTRADEFIELD = string.IsNullOrEmpty(comDrow["RETAILFEEDWEBTRADEFIELD"].ToString()) ? "" : comDrow["RETAILFEEDWEBTRADEFIELD"].ToString();

    //                            if (commColumns.Contains("SALESPERSONNAME"))
    //                                comModl.SALESPERSONNAME = string.IsNullOrEmpty(comDrow["SALESPERSONNAME"].ToString()) ? "" : comDrow["SALESPERSONNAME"].ToString();

    //                            if (commColumns.Contains("SBBSREPOINTEREST"))
    //                                comModl.SBBSREPOINTEREST = string.IsNullOrEmpty(comDrow["SBBSREPOINTEREST"].ToString()) ? 0 : Convert.ToInt32(comDrow["SBBSREPOINTEREST"]);

    //                            if (commColumns.Contains("SBBSREPORATE"))
    //                                comModl.SBBSREPORATE = string.IsNullOrEmpty(comDrow["SBBSREPORATE"].ToString()) ? 0 : Convert.ToInt32(comDrow["SBBSREPORATE"]);


    //                            if (commColumns.Contains("SECONDARYTRANSACTIONTICKETNUMB"))
    //                                comModl.SECONDARYTRANSACTIONTICKETNUMB = string.IsNullOrEmpty(comDrow["SECONDARYTRANSACTIONTICKETNUMB"].ToString()) ? 0 : Convert.ToInt32(comDrow["SECONDARYTRANSACTIONTICKETNUMB"]);


    //                            if (commColumns.Contains("TRADEFEED_ID"))
    //                                comModl.SECURITYISPENCEQUOTED = string.IsNullOrEmpty(comDrow["RTTMREFERENCEID"].ToString()) ? "" : comDrow["RTTMREFERENCEID"].ToString();

    //                            if (commColumns.Contains("SHORTNOTES"))
    //                                comModl.SHORTNOTES = string.IsNullOrEmpty(comDrow["SHORTNOTES"].ToString()) ? "" : comDrow["SHORTNOTES"].ToString();

    //                            if (commColumns.Contains("SOFTDOLLARPERCENTAGE"))
    //                                comModl.SOFTDOLLARPERCENTAGE = string.IsNullOrEmpty(comDrow["SOFTDOLLARPERCENTAGE"].ToString()) ? 0 : Convert.ToInt32(comDrow["SOFTDOLLARPERCENTAGE"]);

    //                            if (commColumns.Contains("SOURCECODE"))
    //                                comModl.SOURCECODE = string.IsNullOrEmpty(comDrow["SOURCECODE"].ToString()) ? 0 : Convert.ToInt32(comDrow["SOURCECODE"]);

    //                            if (commColumns.Contains("SPREAD"))
    //                                comModl.SPREAD = string.IsNullOrEmpty(comDrow["SPREAD"].ToString()) ? 0 : Convert.ToInt32(comDrow["SPREAD"]);

    //                            if (commColumns.Contains("STEPOUTSHARES"))
    //                                comModl.STEPOUTSHARES = string.IsNullOrEmpty(comDrow["STEPOUTSHARES"].ToString()) ? 0 : Convert.ToInt32(comDrow["STEPOUTSHARES"]);

    //                            if (commColumns.Contains("STRIPCODE"))
    //                                comModl.STRIPCODE = string.IsNullOrEmpty(comDrow["STRIPCODE"].ToString()) ? "" : comDrow["STRIPCODE"].ToString();

    //                            if (commColumns.Contains("STRIPTICKETFLAG"))
    //                                comModl.STRIPTICKETFLAG = string.IsNullOrEmpty(comDrow["STRIPTICKETFLAG"].ToString()) ? "" : comDrow["STRIPTICKETFLAG"].ToString();

    //                            if (commColumns.Contains("SWAPSPREAD"))
    //                                comModl.SWAPSPREAD = string.IsNullOrEmpty(comDrow["SWAPSPREAD"].ToString()) ? 0 : Convert.ToDecimal(comDrow["SWAPSPREAD"].ToString());

    //                            if (commColumns.Contains("SYSTEMATICINTERNALIZERBIC"))
    //                                comModl.SYSTEMATICINTERNALIZERBIC = string.IsNullOrEmpty(comDrow["SYSTEMATICINTERNALIZERBIC"].ToString()) ? "" : comDrow["SYSTEMATICINTERNALIZERBIC"].ToString();

    //                            if (commColumns.Contains("TAXLOTS"))
    //                                comModl.TAXLOTS = string.IsNullOrEmpty(comDrow["TAXLOTS"].ToString()) ? "" : comDrow["TAXLOTS"].ToString();

    //                            if (commColumns.Contains("TKTUSESBASEPRICE"))
    //                                comModl.TKTUSESBASEPRICE = string.IsNullOrEmpty(comDrow["TKTUSESBASEPRICE"].ToString()) ? "" : comDrow["TKTUSESBASEPRICE"].ToString();

    //                            if (commColumns.Contains("TRACEPRICEMEMO"))
    //                                comModl.TRACEPRICEMEMO = string.IsNullOrEmpty(comDrow["TRACEPRICEMEMO"].ToString()) ? "" : comDrow["TRACEPRICEMEMO"].ToString();

    //                            if (commColumns.Contains("TRACEREPORTABLE"))
    //                                comModl.TRACEREPORTABLE = string.IsNullOrEmpty(comDrow["TRACEREPORTABLE"].ToString()) ? "" : comDrow["TRACEREPORTABLE"].ToString();

    //                            if (commColumns.Contains("TRACESPECIALPRICE"))
    //                                comModl.TRACESPECIALPRICE = string.IsNullOrEmpty(comDrow["TRACESPECIALPRICE"].ToString()) ? "" : comDrow["TRACESPECIALPRICE"].ToString();

    //                            if (commColumns.Contains("TRACESPECIALPROCESSINGFLAG"))
    //                                comModl.TRACESPECIALPROCESSINGFLAG = string.IsNullOrEmpty(comDrow["TRACESPECIALPROCESSINGFLAG"].ToString()) ? "" : comDrow["TRACESPECIALPROCESSINGFLAG"].ToString();

    //                            if (commColumns.Contains("TRADECREATEDBYMORTGAGEFACTORCO"))
    //                                comModl.TRADECREATEDBYMORTGAGEFACTORCO = string.IsNullOrEmpty(comDrow["TRADECREATEDBYMORTGAGEFACTORCO"].ToString()) ? "" : comDrow["TRADECREATEDBYMORTGAGEFACTORCO"].ToString();

    //                            if (commColumns.Contains("TRADETYPE"))
    //                                comModl.TRADETYPE = string.IsNullOrEmpty(comDrow["TRADETYPE"].ToString()) ? "" : comDrow["TRADETYPE"].ToString();

    //                            if (commColumns.Contains("TRANSACTIONCODE"))
    //                                comModl.TRANSACTIONCODE = string.IsNullOrEmpty(comDrow["TRANSACTIONCODE"].ToString()) ? "" : comDrow["TRANSACTIONCODE"].ToString();

    //                            if (commColumns.Contains("TRANSACTIONNUMBEROFORIGINTRANS"))
    //                                comModl.TRANSACTIONNUMBEROFORIGINTRANS = string.IsNullOrEmpty(comDrow["TRANSACTIONNUMBEROFORIGINTRANS"].ToString()) ? 0 : Convert.ToInt32(comDrow["TRANSACTIONNUMBEROFORIGINTRANS"]);

    //                            if (commColumns.Contains("TWENTYFOURHOURTRADEFLAG"))
    //                                comModl.TWENTYFOURHOURTRADEFLAG = string.IsNullOrEmpty(comDrow["TWENTYFOURHOURTRADEFLAG"].ToString()) ? "" : comDrow["TWENTYFOURHOURTRADEFLAG"].ToString();

    //                            if (commColumns.Contains("TWENTYFOURHOURTRADETICKETNUMBE"))
    //                                comModl.TWENTYFOURHOURTRADETICKETNUMBE = string.IsNullOrEmpty(comDrow["TWENTYFOURHOURTRADETICKETNUMBE"].ToString()) ? 0 : Convert.ToInt32(comDrow["TWENTYFOURHOURTRADETICKETNUMBE"]);

    //                            if (commColumns.Contains("UNDERLYINGCUSIP"))
    //                                comModl.UNDERLYINGCUSIP = string.IsNullOrEmpty(comDrow["UNDERLYINGCUSIP"].ToString()) ? "" : comDrow["UNDERLYINGCUSIP"].ToString();

    //                            if (commColumns.Contains("UNDERLYINGOPTIONISIN"))
    //                                comModl.UNDERLYINGOPTIONISIN = string.IsNullOrEmpty(comDrow["UNDERLYINGOPTIONISIN"].ToString()) ? "" : comDrow["UNDERLYINGOPTIONISIN"].ToString();

    //                            if (commColumns.Contains("UNIQUETRADEIDENTIFIER"))
    //                                comModl.UNIQUETRADEIDENTIFIER = string.IsNullOrEmpty(comDrow["UNIQUETRADEIDENTIFIER"].ToString()) ? "" : comDrow["UNIQUETRADEIDENTIFIER"].ToString();


    //                            if (commColumns.Contains("UNIQUETRADEIDENTIFIERFORCLEARI"))
    //                                comModl.UNIQUETRADEIDENTIFIERFORCLEARI = string.IsNullOrEmpty(comDrow["UNIQUETRADEIDENTIFIERFORCLEARI"].ToString()) ? "" : comDrow["UNIQUETRADEIDENTIFIERFORCLEARI"].ToString();

    //                            if (commColumns.Contains("UNWINDTRANSACTIONPNL_ATTRIBUTI"))
    //                                comModl.UNWINDTRANSACTIONPNL_ATTRIBUTI = string.IsNullOrEmpty(comDrow["UNWINDTRANSACTIONPNL_ATTRIBUTI"].ToString()) ? 0 : Convert.ToDecimal(comDrow["UNWINDTRANSACTIONPNL_ATTRIBUTI"].ToString());

    //                            if (commColumns.Contains("USERGRPNTRADINGDESKCODE"))
    //                                comModl.USERGRPNTRADINGDESKCODE = string.IsNullOrEmpty(comDrow["USERGRPNTRADINGDESKCODE"].ToString()) ? "" : comDrow["USERGRPNTRADINGDESKCODE"].ToString();

    //                            if (commColumns.Contains("USERGRPNTRADINGDESKNAME"))
    //                                comModl.USERGRPNTRADINGDESKNAME = string.IsNullOrEmpty(comDrow["USERGRPNTRADINGDESKNAME"].ToString()) ? "" : comDrow["USERGRPNTRADINGDESKNAME"].ToString();

    //                            if (commColumns.Contains("USERNUMBEROFPERSONWHOAPPROVEDT"))
    //                                comModl.USERNUMBEROFPERSONWHOAPPROVEDT = string.IsNullOrEmpty(comDrow["USERNUMBEROFPERSONWHOAPPROVEDT"].ToString()) ? "" : comDrow["USERNUMBEROFPERSONWHOAPPROVEDT"].ToString();

    //                            if (commColumns.Contains("VARIABLEPENSIONLIVREESPREAD"))
    //                                comModl.VARIABLEPENSIONLIVREESPREAD = string.IsNullOrEmpty(comDrow["VARIABLEPENSIONLIVREESPREAD"].ToString()) ? 0 : Convert.ToDecimal(comDrow["VARIABLEPENSIONLIVREESPREAD"].ToString());

    //                            if (commColumns.Contains("WICONVERSIONFLAG"))
    //                                comModl.WICONVERSIONFLAG = string.IsNullOrEmpty(comDrow["WICONVERSIONFLAG"].ToString()) ? "" : comDrow["WICONVERSIONFLAG"].ToString();

    //                            if (commColumns.Contains("WIMUNIFLAG"))
    //                                comModl.WIMUNIFLAG = string.IsNullOrEmpty(comDrow["WIMUNIFLAG"].ToString()) ? "" : comDrow["WIMUNIFLAG"].ToString();

    //                            if (commColumns.Contains("YIELDENTEREDTRADE"))
    //                                comModl.YIELDENTEREDTRADE = string.IsNullOrEmpty(comDrow["YIELDENTEREDTRADE"].ToString()) ? "" : comDrow["YIELDENTEREDTRADE"].ToString();

    //                        }
    //                        catch (Exception exxx)
    //                        {
    //                            Console.WriteLine("Error while reading data : \n" + exxx);
    //                        }
    //                        // end of adding new columns


    //                        //string strInsertQuery = @"INSERT INTO UT_TRADEFEEDS(BLOOMBERGFIRMID,TRANSACTIONNUMBER,SECURITYIDENTIFIERFLAG,SECURITYIDENTIFIER,SECURITYCURRENCYISOCODE,SECURITYPRODUCTKEY,BLOOMBERGIDENTIFIER,TICKER,COUPONSTRIKEPRICE,MATURITYDATEEXPIRATIONDATE,SERIESEXCHANGECODE,BUYSELLCOVERSHORTFLAG,RECORDTYPE,TRADEDATE,ASOFTRADEDATE,SETTLEMENTDATE,PRICE,YIELD,
    //                        //                                               TRADEAMOUNT,CUSTOMERACCOUNTCOUNTERPARTY,ACCOUNTCOUNTERPARTY,SETTLEMENTLOCATIONINDICATOR,PRODUCTSUBFLAG,PRINCIPALLOANAMOUNT,ACCRUEDINTERESTREPOINTEREST,TRADERACCOUNTNAME,NUMBEROFDAYSACCRUED,COMMON_ID,TIMEOFSLATE,TIMEOFSALESTICKET,LASTLOGIN,MASTERTICKETNUMBER,CONCSTIPULATIONVARREPORATE)

    //                        //                                             VALUES(:pBLOOMBERGFIRMID,:pTRANSACTIONNUMBER,:pSECURITYIDENTIFIERFLAG,:pSECURITYIDENTIFIER,:pSECURITYCURRENCYISOCODE,:pSECURITYPRODUCTKEY,:pBLOOMBERGIDENTIFIER,:pTICKER,:pCOUPONSTRIKEPRICE,:pMATURITYDATEEXPIRATIONDATE,:pSERIESEXCHANGECODE,:pBUYSELLCOVERSHORTFLAG,:pRECORDTYPE,:pTRADEDATE,:pASOFTRADEDATE,:pSETTLEMENTDATE,:pPRICE,:pYIELD
    //                        //                                                     ,:pTRADEAMOUNT,:pCUSTOMERACCOUNTCOUNTERPARTY,:pACCOUNTCOUNTERPARTY,:pSETTLEMENTLOCATIONINDICATOR,:pPRODUCTSUBFLAG,:pPRINCIPALLOANAMOUNT,:pACCRUEDINTERESTREPOINTEREST,:pTRADERACCOUNTNAME,:pNUMBEROFDAYSACCRUED,:pCOMMON_ID,:pTIMEOFSLATE,:pTIMEOFSALESTICKET,:pLASTLOGIN,:pMASTERTICKETNUMBER,:pCONCSTIPULATIONVARREPORATE)";

    //                        //string strInsertQuery = @"INSERT INTO COMMON_FEEDS(BLOOMBERGFIRMID,TRANSACTIONNUMBER,SECURITYIDENTIFIERFLAG,SECURITYIDENTIFIER,SECURITYCURRENCYISOCODE,SECURITYPRODUCTKEY,BLOOMBERGIDENTIFIER,TICKER,COUPONSTRIKEPRICE,MATURITYDATEEXPIRATIONDATE,SERIESEXCHANGECODE,BUYSELLCOVERSHORTFLAG,RECORDTYPE,TRADEDATE,ASOFTRADEDATE,SETTLEMENTDATE,PRICE,YIELD,
    //                        //                                            TRADEAMOUNT,CUSTOMERACCOUNTCOUNTERPARTY,ACCOUNTCOUNTERPARTY,SETTLEMENTLOCATIONINDICATOR,PRODUCTSUBFLAG,PRINCIPALLOANAMOUNT,ACCRUEDINTERESTREPOINTEREST,TRADERACCOUNTNAME,NUMBEROFDAYSACCRUED,COMMON_ID,TIMEOFSLATE,TIMEOFSALESTICKET,LASTLOGIN,MASTERTICKETNUMBER,CONCSTIPULATIONVARREPORATE,
    //                        //                                            SALESPERSONLOGIN,SETTLEMENTCURRENCYISOCODE,SETTLEMENTCURRENCYRATE ,CANCELDUETOCORRECTION ,INVERTFLAG,WORKOUTDATE,WORKOUTPRICE,AUTOEXTRADEFLAG,TRADERTOTRADERTRADEFLAG,VERSION,UNIQUEBLOOMBERGID,EXTENDEDPRECISIONPRICE,SYSTEMDATE,IMPACTFLAG,TRADERACCOUNTNUMBER,SETTLEMENTLOCATIONABBR,TRANSACTIONTYPE,
    //                        //                                               INTERESTATMATURITY,ENTEREDTICKETUSERID,ALLOCATEDTICKETUSERID,OMREASONCODE,OMSBLOCKNAME,QUOTETYPEINDICATOR,ISSUEDATE,FIRMTRADEDATE,FIRMASOFDATE,FIRMTIMEOFSLATE,FIRMTIMEOFSALESTICKET,STEPOUTBROKER,SOFTDOLLARFLAG,TIPFACTORESTIMATED,TOTALTRADEAMOUNT,MASTERACCOUNT,MASTERACCOUNTNAME,CTMMATCHSTATUS,MATCHDATE,
    //                        //                                                ISDIRTYPRICE,TSAMINDICATOR,TAXLOTMETHOD,SECURITYPRICE,LOCALEXCHANGE,SETTLEMENTAMOUNT,FUNDCCYPRINCIPAL,FUNDCCYACCRUED,FUNDCCYTOTALCOMMISSION,REDEMPTIONCCYPRINCIPAL,REDEMPTIONCCYACCRUED,REDEMPTIONTOTALCOMMISSION ,SETTLEMENTCCYPRINCIPAL,SETTLEMENTCCYACCRUED,DAYSTOMATURITY,SETTLEMENTCCYTOTALCOMMN,
    //                        //                                                PRIMARYEXCHANGECODE,EXECUTIONPLATFORM,CLIENTAUTH,LASTLOGINUUID,ORIGINALTKTID,SETTLEMENTCCYPRICE,ORDERSTATUS,RTTMINDICATOR,RTTMREFERENCEID,CPARTYENCODEDLONGNAME,BLOOMBERGGLOBALIDENTIFIER,SECONDHALFTDR2TDRTRADE,CASHREVLOFFSETTICKETIND
    //                        //                                              ,LIMITPRICE,TRADEFLATFLAG,MSRBREPORTABLE,TRADEFEED_ID,FILENAME,FILEDATE,FILENUMBER,
    //                        //                                               ACCRUEDINTERESTOVERDRIVE,AFFILIATEDTICKETNUMBER,AGENCYLINKEDTICKETNUMBER,AGENCYTRADEREFERENCETICKETNUMB,ALERTID,ALTERNATIVESECURITYID,ASOFDATETIMEZONETXT,AUTOEXAPPLNAME,AUTOEXBROKERNAME,AUTOEXDEALERSINCOMP,AUTOEXNUMBER
    //                        //                                              ,AUTOEXORDERINQUIRY,AUTOEXSTATUS,AVGLIFE,BANKDESTINATIONID,BASEPRICEONTKT,BASEYIELDONTKT,BASKETID,BLOOMBERGFUNCTIONS,BLOOMBERGREFERENCENUMBER,BUYSIDEREASONCODE,BXTLOGIN,CANCELDATE,CANCELDUETOMATCH,CANCELTRANSACTIONPNL_ATTIBUTIO
    //                        //                                              ,CASHBROKERCODE,CASHLINKEDREVERSALTICKETNUMBER,CASHREVERSALDATE,CLEARINGBROKER,CONCESSIONSTIPULATIONVARIANCER,CONTINGENTFLAT,CONTRACTSIZE,CONVEXITY,CORRECTDACTUALNAV,CORRECTEDTRANSACTIONPNL_ATTRIB,CORRECTIONASORIGINAL
    //                        //                                              ,CURRENTFLOATERCOUPON,CUSTODYSAFEKEEPINGNUMBER,CUTTRADE,DATETRADEWASAUTHORIZED,DELAYEDDELIVERYDATE,DIRTYPRICE,DISCOUNTRATE,EDITTRANSATIONPNL_ATTRIBUTION,ELECTRONICEXECUTIONFLAG,ELECTRONICTRADE,ENTRYOFMUTUALFUNDSHARESWASBYSE
    //                        //                                              ,EQPLSUBFLAG,ESTIMATEFLAG,FEEDREASONCODE,FIRMCANCELDATE,FXSTRATEGYCODE,GENERALLEDGERACCOUNT,GOODMILLIONNUMBER,GSPREAD,GSPREADCURVEID,HEDGETICKETNUMBER,INUNITSFLAG,IPOFLAG,ISCFD,ISSUERCOMPANYID,JAPANNUMBER
    //                        //                                              ,JOURNALREASONCODE,LONGNOTES,LOTNUMBER,MIDDLEOFFICETRADEINDICATOR,MMKTISSUERCODE,MORTGAGEBRADYINDEXBONDFACTOR,MSRBPRICECODE,MTGEFACTORDATE,NAMEOFUSERWHOAUTHORIZEDTRADE,NAVESTIMATED,NETWIRE,NEWTRANSACTIONPNL_ATTRIBUTION
    //                        //                                              ,NONREGSETTLEREPORATE,OCCOPTIONTICKER,OMSGOODTILDATE,OMSLIMITPX,OMSORDERNUMBER,OMSSTOPPX,OMSTIMEINFORCE,OMSTRANSACTIONID,OPTIONCONTRACTSIZE,OPTIONSDELTA,ORDERTYPE,PARTIALALLOCATIONEXISTS,PAYNOTIONALAMOUNT,PLATFORMNAME,POOLNUMBER
    //                        //                                              ,PREPAYMENTSPEEDANDTYPECONTRACT,PRIMARYMMKTID,PRIMEBROKER,PRINCIPALAGENCYFLAG,PROGRAMBASKETFLAG,PUTCALLINDICATOR,RECEIVENOTIONAL,RELATEDSLATETICKETNUMBER,REPORTTOFCA,RETAILFEED,RETAILFEEDWEBTRADEFIELD,SALESPERSONNAME,SBBSREPOINTEREST
    //                        //                                              ,SBBSREPORATE,SECONDARYTRANSACTIONTICKETNUMB,SECURITYISPENCEQUOTED,SHORTNOTES,SOFTDOLLARPERCENTAGE,SOURCECODE,SPREAD,STEPOUTSHARES,STRIPCODE,STRIPTICKETFLAG,SWAPSPREAD,SYSTEMATICINTERNALIZERBIC,TAXLOTS,TKTUSESBASEPRICE,TRACEPRICEMEMO,
    //                        //                                              TRACEREPORTABLE,TRACESPECIALPRICE,TRACESPECIALPROCESSINGFLAG,TRADECREATEDBYMORTGAGEFACTORCO,TRADETYPE,TRANSACTIONCODE,TRANSACTIONNUMBEROFORIGINTRANS,TWENTYFOURHOURTRADEFLAG,TWENTYFOURHOURTRADETICKETNUMBE
    //                        //                                              ,UNDERLYINGCUSIP,UNDERLYINGOPTIONISIN,UNIQUETRADEIDENTIFIER,UNIQUETRADEIDENTIFIERFORCLEARI,UNWINDTRANSACTIONPNL_ATTRIBUTI,USERGRPNTRADINGDESKCODE,USERGRPNTRADINGDESKNAME,USERNUMBEROFPERSONWHOAPPROVEDT,VARIABLEPENSIONLIVREESPREAD,WICONVERSIONFLAG,WIMUNIFLAG,YIELDENTEREDTRADE)

    //                        //                                         VALUES(:pBLOOMBERGFIRMID,:pTRANSACTIONNUMBER,:pSECURITYIDENTIFIERFLAG,:pSECURITYIDENTIFIER,:pSECURITYCURRENCYISOCODE,:pSECURITYPRODUCTKEY,:pBLOOMBERGIDENTIFIER,:pTICKER,:pCOUPONSTRIKEPRICE,:pMATURITYDATEEXPIRATIONDATE,:pSERIESEXCHANGECODE,:pBUYSELLCOVERSHORTFLAG,:pRECORDTYPE,:pTRADEDATE,:pASOFTRADEDATE,:pSETTLEMENTDATE,:pPRICE,:pYIELD
    //                        //                                            ,:pTRADEAMOUNT,:pCUSTOMERACCOUNTCOUNTERPARTY,:pACCOUNTCOUNTERPARTY,:pSETTLEMENTLOCATIONINDICATOR,:pPRODUCTSUBFLAG,:pPRINCIPALLOANAMOUNT,:pACCRUEDINTERESTREPOINTEREST,:pTRADERACCOUNTNAME,:pNUMBEROFDAYSACCRUED,:pCOMMON_ID,:pTIMEOFSLATE,:pTIMEOFSALESTICKET,:pLASTLOGIN,:pMASTERTICKETNUMBER,:pCONCSTIPULATIONVARREPORATE
    //                        //                                            ,:pSALESPERSONLOGIN,:pSETTLEMENTCURRENCYISOCODE,:pSETTLEMENTCURRENCYRATE,:pCANCELDUETOCORRECTION,:pINVERTFLAG,:pWORKOUTDATE,:pWORKOUTPRICE,:pAUTOEXTRADEFLAG,:pTRADERTOTRADERTRADEFLAG,:pVERSION,:pUNIQUEBLOOMBERGID,:pEXTENDEDPRECISIONPRICE,:pSYSTEMDATE,:pIMPACTFLAG,:pTRADERACCOUNTNUMBER,:pSETTLEMENTLOCATIONABBR,:pTRANSACTIONTYPE
    //                        //                                            ,:pINTERESTATMATURITY,:pENTEREDTICKETUSERID,:pALLOCATEDTICKETUSERID,:pOMREASONCODE,:pOMSBLOCKNAME,:pQUOTETYPEINDICATOR,:pISSUEDATE,:pFIRMTRADEDATE,:pFIRMASOFDATE,:pFIRMTIMEOFSLATE,:pFIRMTIMEOFSALESTICKET,:pSTEPOUTBROKER,:pSOFTDOLLARFLAG,:pTIPFACTORESTIMATED,:pTOTALTRADEAMOUNT,:pMASTERACCOUNT,:pMASTERACCOUNTNAME,:pCTMMATCHSTATUS,:pMATCHDATE
    //                        //                                            ,:pISDIRTYPRICE,:pTSAMINDICATOR,:pTAXLOTMETHOD,:pSECURITYPRICE,:pLOCALEXCHANGE,:pSETTLEMENTAMOUNT,:pFUNDCCYPRINCIPAL,:pFUNDCCYACCRUED,:pFUNDCCYTOTALCOMMISSION,:pREDEMPTIONCCYPRINCIPAL,:pREDEMPTIONCCYACCRUED,:pREDEMPTIONTOTALCOMMISSION,:pSETTLEMENTCCYPRINCIPAL,:pSETTLEMENTCCYACCRUED,:pDAYSTOMATURITY,:pSETTLEMENTCCYTOTALCOMMN
    //                        //                                            ,:pPRIMARYEXCHANGECODE,:pEXECUTIONPLATFORM,:pCLIENTAUTH,:pLASTLOGINUUID,:pORIGINALTKTID,:pSETTLEMENTCCYPRICE,:pORDERSTATUS,:pRTTMINDICATOR,:pRTTMREFERENCEID,:pCPARTYENCODEDLONGNAME,:pBLOOMBERGGLOBALIDENTIFIER,:pSECONDHALFTDR2TDRTRADE,:pCASHREVLOFFSETTICKETIND
    //                        //                                            ,:pLIMITPRICE,:pTRADEFLATFLAG,:pMSRBREPORTABLE,:pTRADEFEED_ID,:pFILENAME ,:pFILEDATE,:pFILENUMBER
    //                        //                                             ,:pACCRUEDINTERESTOVERDRIVE,:pAFFILIATEDTICKETNUMBER,:pAGENCYLINKEDTICKETNUMBER,:pAGENCYTRADEREFERENCETICKETNUMB,:pALERTID,:pALTERNATIVESECURITYID,:pASOFDATETIMEZONETXT,:pAUTOEXAPPLNAME,:pAUTOEXBROKERNAME,:pAUTOEXDEALERSINCOMP,:pAUTOEXNUMBER
    //                        //                                             ,:pAUTOEXORDERINQUIRY,:pAUTOEXSTATUS,:pAVGLIFE,:pBANKDESTINATIONID,:pBASEPRICEONTKT,:pBASEYIELDONTKT,:pBASKETID,:pBLOOMBERGFUNCTIONS,:pBLOOMBERGREFERENCENUMBER,:pBUYSIDEREASONCODE,:pBXTLOGIN,:pCANCELDATE,:pCANCELDUETOMATCH,:pCANCELTRANSACTIONPNL_ATTIBUTIO
    //                        //                                             ,:pCASHBROKERCODE,:pCASHLINKEDREVERSALTICKETNUMBER,:pCASHREVERSALDATE,:pCLEARINGBROKER,:pCONCESSIONSTIPULATIONVARIANCER,:pCONTINGENTFLAT,:pCONTRACTSIZE,:pCONVEXITY,:pCORRECTDACTUALNAV,:pCORRECTEDTRANSACTIONPNL_ATTRIB,:pCORRECTIONASORIGINAL
    //                        //                                             ,:pCURRENTFLOATERCOUPON,:pCUSTODYSAFEKEEPINGNUMBER,:pCUTTRADE,:pDATETRADEWASAUTHORIZED,:pDELAYEDDELIVERYDATE,:pDIRTYPRICE,:pDISCOUNTRATE,:pEDITTRANSATIONPNL_ATTRIBUTION,:pELECTRONICEXECUTIONFLAG,:pELECTRONICTRADE,:pENTRYOFMUTUALFUNDSHARESWASBYSE
    //                        //                                             ,:pEQPLSUBFLAG,:pESTIMATEFLAG,:pFEEDREASONCODE,:pFIRMCANCELDATE,:pFXSTRATEGYCODE,:pGENERALLEDGERACCOUNT,:pGOODMILLIONNUMBER,:pGSPREAD,:pGSPREADCURVEID,:pHEDGETICKETNUMBER,:pINUNITSFLAG,:pIPOFLAG,:pISCFD,:pISSUERCOMPANYID,:pJAPANNUMBER
    //                        //                                             ,:pJOURNALREASONCODE,:pLONGNOTES,:pLOTNUMBER,:pMIDDLEOFFICETRADEINDICATOR,:pMMKTISSUERCODE,:pMORTGAGEBRADYINDEXBONDFACTOR,:pMSRBPRICECODE,:pMTGEFACTORDATE,:pNAMEOFUSERWHOAUTHORIZEDTRADE,:pNAVESTIMATED,:pNETWIRE,:pNEWTRANSACTIONPNL_ATTRIBUTION
    //                        //                                            ,:pNONREGSETTLEREPORATE,:pOCCOPTIONTICKER,:pOMSGOODTILDATE,:pOMSLIMITPX,:pOMSORDERNUMBER,:pOMSSTOPPX,:pOMSTIMEINFORCE,:pOMSTRANSACTIONID,:pOPTIONCONTRACTSIZE,:pOPTIONSDELTA,:pORDERTYPE,:pPARTIALALLOCATIONEXISTS,:pPAYNOTIONALAMOUNT,:pPLATFORMNAME,:pPOOLNUMBER
    //                        //                                            ,:pPREPAYMENTSPEEDANDTYPECONTRACT,:pPRIMARYMMKTID,:pPRIMEBROKER,:pPRINCIPALAGENCYFLAG,:pPROGRAMBASKETFLAG,:pPUTCALLINDICATOR,:pRECEIVENOTIONAL,:pRELATEDSLATETICKETNUMBER,:pREPORTTOFCA,:pRETAILFEED,:pRETAILFEEDWEBTRADEFIELD,:pSALESPERSONNAME,:pSBBSREPOINTEREST
    //                        //                                            ,:pSBBSREPORATE,:pSECONDARYTRANSACTIONTICKETNUMB,:pSECURITYISPENCEQUOTED,:pSHORTNOTES,:pSOFTDOLLARPERCENTAGE,:pSOURCECODE,:pSPREAD,:pSTEPOUTSHARES,:pSTRIPCODE,:pSTRIPTICKETFLAG,:pSWAPSPREAD,:pSYSTEMATICINTERNALIZERBIC,:pTAXLOTS,:pTKTUSESBASEPRICE,:pTRACEPRICEMEMO
    //                        //                                            ,:pTRACEREPORTABLE,:pTRACESPECIALPRICE,:pTRACESPECIALPROCESSINGFLAG,:pTRADECREATEDBYMORTGAGEFACTORCO,:pTRADETYPE,:pTRANSACTIONCODE,:pTRANSACTIONNUMBEROFORIGINTRANS,:pTWENTYFOURHOURTRADEFLAG,:pTWENTYFOURHOURTRADETICKETNUMBE
    //                        //                                            ,:pUNDERLYINGCUSIP,:pUNDERLYINGOPTIONISIN,:pUNIQUETRADEIDENTIFIER,:pUNIQUETRADEIDENTIFIERFORCLEARI,:pUNWINDTRANSACTIONPNL_ATTRIBUTI,:pUSERGRPNTRADINGDESKCODE,:pUSERGRPNTRADINGDESKNAME,:pUSERNUMBEROFPERSONWHOAPPROVEDT,:pVARIABLEPENSIONLIVREESPREAD,:pWICONVERSIONFLAG,:pWIMUNIFLAG,:pYIELDENTEREDTRADE)";

    //                        string strInsertQuery = @"INSERT INTO COMMON_FEEDS(BLOOMBERGFIRMID,TRANSACTIONNUMBER,SECURITYIDENTIFIERFLAG,SECURITYIDENTIFIER,SECURITYCURRENCYISOCODE,SECURITYPRODUCTKEY,BLOOMBERGIDENTIFIER,TICKER,COUPONSTRIKEPRICE,MATURITYDATEEXPIRATIONDATE,SERIESEXCHANGECODE,BUYSELLCOVERSHORTFLAG,RECORDTYPE,TRADEDATE,ASOFTRADEDATE,SETTLEMENTDATE,PRICE,YIELD,
    //                                                                    TRADEAMOUNT,CUSTOMERACCOUNTCOUNTERPARTY,ACCOUNTCOUNTERPARTY,SETTLEMENTLOCATIONINDICATOR,PRODUCTSUBFLAG,PRINCIPALLOANAMOUNT,ACCRUEDINTERESTREPOINTEREST,TRADERACCOUNTNAME,NUMBEROFDAYSACCRUED,COMMON_ID,TIMEOFSLATE,TIMEOFSALESTICKET,LASTLOGIN,MASTERTICKETNUMBER,CONCSTIPULATIONVARREPORATE,
    //                                                                    SALESPERSONLOGIN,SETTLEMENTCURRENCYISOCODE,SETTLEMENTCURRENCYRATE ,CANCELDUETOCORRECTION ,INVERTFLAG,WORKOUTDATE,WORKOUTPRICE,AUTOEXTRADEFLAG,TRADERTOTRADERTRADEFLAG,VERSION,UNIQUEBLOOMBERGID,EXTENDEDPRECISIONPRICE,SYSTEMDATE,IMPACTFLAG,TRADERACCOUNTNUMBER,SETTLEMENTLOCATIONABBR,TRANSACTIONTYPE,
    //                                                                       INTERESTATMATURITY,ENTEREDTICKETUSERID,ALLOCATEDTICKETUSERID,OMREASONCODE,OMSBLOCKNAME,QUOTETYPEINDICATOR,ISSUEDATE,FIRMTRADEDATE,FIRMASOFDATE,FIRMTIMEOFSLATE,FIRMTIMEOFSALESTICKET,STEPOUTBROKER,SOFTDOLLARFLAG,TIPFACTORESTIMATED,TOTALTRADEAMOUNT,MASTERACCOUNT,MASTERACCOUNTNAME,CTMMATCHSTATUS,MATCHDATE,
    //                                                                        ISDIRTYPRICE,TSAMINDICATOR,TAXLOTMETHOD,SECURITYPRICE,LOCALEXCHANGE,SETTLEMENTAMOUNT,FUNDCCYPRINCIPAL,FUNDCCYACCRUED,FUNDCCYTOTALCOMMISSION,REDEMPTIONCCYPRINCIPAL,REDEMPTIONCCYACCRUED,REDEMPTIONTOTALCOMMISSION ,SETTLEMENTCCYPRINCIPAL,SETTLEMENTCCYACCRUED,DAYSTOMATURITY,SETTLEMENTCCYTOTALCOMMN,
    //                                                                        PRIMARYEXCHANGECODE,EXECUTIONPLATFORM,CLIENTAUTH,LASTLOGINUUID,ORIGINALTKTID,SETTLEMENTCCYPRICE,ORDERSTATUS,RTTMINDICATOR,RTTMREFERENCEID,CPARTYENCODEDLONGNAME,BLOOMBERGGLOBALIDENTIFIER,SECONDHALFTDR2TDRTRADE,CASHREVLOFFSETTICKETIND
    //                                                                      ,LIMITPRICE,TRADEFLATFLAG,MSRBREPORTABLE,TRADEFEED_ID,FILENAME,FILEDATE,FILENUMBER,
    //                                                                       ACCRUEDINTERESTOVERDRIVE,AFFILIATEDTICKETNUMBER,AGENCYLINKEDTICKETNUMBER,AGENCYTRADEREFERENCETICKETNUMB,ALERTID,ALTERNATIVESECURITYID,ASOFDATETIMEZONETXT,AUTOEXAPPLNAME,AUTOEXBROKERNAME,AUTOEXDEALERSINCOMP,AUTOEXNUMBER
    //                                                                      ,AUTOEXORDERINQUIRY,AUTOEXSTATUS,AVGLIFE,BANKDESTINATIONID,BASEPRICEONTKT,BASEYIELDONTKT,BASKETID,BLOOMBERGFUNCTIONS,BLOOMBERGREFERENCENUMBER,BUYSIDEREASONCODE,BXTLOGIN,CANCELDATE,CANCELDUETOMATCH,CANCELTRANSACTIONPNL_ATTIBUTIO
    //                                                                      ,CASHBROKERCODE,CASHLINKEDREVERSALTICKETNUMBER,CASHREVERSALDATE,CLEARINGBROKER,CONCESSIONSTIPULATIONVARIANCER,CONTINGENTFLAT,CONTRACTSIZE,CONVEXITY,CORRECTDACTUALNAV,CORRECTEDTRANSACTIONPNL_ATTRIB,CORRECTIONASORIGINAL
    //                                                                        ,CURRENTFLOATERCOUPON,CUSTODYSAFEKEEPINGNUMBER,CUTTRADE,DATETRADEWASAUTHORIZED,DELAYEDDELIVERYDATE,DIRTYPRICE,DISCOUNTRATE,EDITTRANSATIONPNL_ATTRIBUTION,ELECTRONICEXECUTIONFLAG,ELECTRONICTRADE,ENTRYOFMUTUALFUNDSHARESWASBYSE
    //                                                                        ,EQPLSUBFLAG,ESTIMATEFLAG,FEEDREASONCODE,FIRMCANCELDATE,FXSTRATEGYCODE,GENERALLEDGERACCOUNT,GOODMILLIONNUMBER,GSPREAD,GSPREADCURVEID,HEDGETICKETNUMBER,INUNITSFLAG,IPOFLAG,ISCFD,ISSUERCOMPANYID,JAPANNUMBER
    //                                                                        ,JOURNALREASONCODE,LONGNOTES,LOTNUMBER,MIDDLEOFFICETRADEINDICATOR,MMKTISSUERCODE,MORTGAGEBRADYINDEXBONDFACTOR,MSRBPRICECODE,MTGEFACTORDATE,NAMEOFUSERWHOAUTHORIZEDTRADE,NAVESTIMATED,NETWIRE,NEWTRANSACTIONPNL_ATTRIBUTION
    //                                                                        ,NONREGSETTLEREPORATE,OCCOPTIONTICKER,OMSGOODTILDATE,OMSLIMITPX,OMSORDERNUMBER,OMSSTOPPX,OMSTIMEINFORCE,OMSTRANSACTIONID,OPTIONCONTRACTSIZE,OPTIONSDELTA,ORDERTYPE,PARTIALALLOCATIONEXISTS,PAYNOTIONALAMOUNT,PLATFORMNAME,POOLNUMBER
    //                                                                        ,PREPAYMENTSPEEDANDTYPECONTRACT,PRIMARYMMKTID,PRIMEBROKER,PRINCIPALAGENCYFLAG,PROGRAMBASKETFLAG,PUTCALLINDICATOR,RECEIVENOTIONAL,RELATEDSLATETICKETNUMBER,REPORTTOFCA,RETAILFEED,RETAILFEEDWEBTRADEFIELD,SALESPERSONNAME,SBBSREPOINTEREST
    //                                                                        ,SBBSREPORATE,SECONDARYTRANSACTIONTICKETNUMB,SECURITYISPENCEQUOTED,SHORTNOTES,SOFTDOLLARPERCENTAGE,SOURCECODE,SPREAD,STEPOUTSHARES,STRIPCODE,STRIPTICKETFLAG,SWAPSPREAD,SYSTEMATICINTERNALIZERBIC,TAXLOTS,TKTUSESBASEPRICE,TRACEPRICEMEMO,
    //                                                                        TRACEREPORTABLE,TRACESPECIALPRICE,TRACESPECIALPROCESSINGFLAG,TRADECREATEDBYMORTGAGEFACTORCO,TRADETYPE,TRANSACTIONCODE,TRANSACTIONNUMBEROFORIGINTRANS,TWENTYFOURHOURTRADEFLAG,TWENTYFOURHOURTRADETICKETNUMBE
    //                                                                        ,UNDERLYINGCUSIP,UNDERLYINGOPTIONISIN,UNIQUETRADEIDENTIFIER,UNIQUETRADEIDENTIFIERFORCLEARI,UNWINDTRANSACTIONPNL_ATTRIBUTI,USERGRPNTRADINGDESKCODE,USERGRPNTRADINGDESKNAME,USERNUMBEROFPERSONWHOAPPROVEDT,VARIABLEPENSIONLIVREESPREAD,WICONVERSIONFLAG,WIMUNIFLAG,YIELDENTEREDTRADE)
                                               
    //                                                                 VALUES(:pBLOOMBERGFIRMID,:pTRANSACTIONNUMBER,:pSECURITYIDENTIFIERFLAG,:pSECURITYIDENTIFIER,:pSECURITYCURRENCYISOCODE,:pSECURITYPRODUCTKEY,:pBLOOMBERGIDENTIFIER,:pTICKER,:pCOUPONSTRIKEPRICE,:pMATURITYDATEEXPIRATIONDATE,:pSERIESEXCHANGECODE,:pBUYSELLCOVERSHORTFLAG,:pRECORDTYPE,:pTRADEDATE,:pASOFTRADEDATE,:pSETTLEMENTDATE,:pPRICE,:pYIELD
    //                                                                    ,:pTRADEAMOUNT,:pCUSTOMERACCOUNTCOUNTERPARTY,:pACCOUNTCOUNTERPARTY,:pSETTLEMENTLOCATIONINDICATOR,:pPRODUCTSUBFLAG,:pPRINCIPALLOANAMOUNT,:pACCRUEDINTERESTREPOINTEREST,:pTRADERACCOUNTNAME,:pNUMBEROFDAYSACCRUED,:pCOMMON_ID,:pTIMEOFSLATE,:pTIMEOFSALESTICKET,:pLASTLOGIN,:pMASTERTICKETNUMBER,:pCONCSTIPULATIONVARREPORATE
    //                                                                    ,:pSALESPERSONLOGIN,:pSETTLEMENTCURRENCYISOCODE,:pSETTLEMENTCURRENCYRATE,:pCANCELDUETOCORRECTION,:pINVERTFLAG,:pWORKOUTDATE,:pWORKOUTPRICE,:pAUTOEXTRADEFLAG,:pTRADERTOTRADERTRADEFLAG,:pVERSION,:pUNIQUEBLOOMBERGID,:pEXTENDEDPRECISIONPRICE,:pSYSTEMDATE,:pIMPACTFLAG,:pTRADERACCOUNTNUMBER,:pSETTLEMENTLOCATIONABBR,:pTRANSACTIONTYPE
    //                                                                    ,:pINTERESTATMATURITY,:pENTEREDTICKETUSERID,:pALLOCATEDTICKETUSERID,:pOMREASONCODE,:pOMSBLOCKNAME,:pQUOTETYPEINDICATOR,:pISSUEDATE,:pFIRMTRADEDATE,:pFIRMASOFDATE,:pFIRMTIMEOFSLATE,:pFIRMTIMEOFSALESTICKET,:pSTEPOUTBROKER,:pSOFTDOLLARFLAG,:pTIPFACTORESTIMATED,:pTOTALTRADEAMOUNT,:pMASTERACCOUNT,:pMASTERACCOUNTNAME,:pCTMMATCHSTATUS,:pMATCHDATE
    //                                                                    ,:pISDIRTYPRICE,:pTSAMINDICATOR,:pTAXLOTMETHOD,:pSECURITYPRICE,:pLOCALEXCHANGE,:pSETTLEMENTAMOUNT,:pFUNDCCYPRINCIPAL,:pFUNDCCYACCRUED,:pFUNDCCYTOTALCOMMISSION,:pREDEMPTIONCCYPRINCIPAL,:pREDEMPTIONCCYACCRUED,:pREDEMPTIONTOTALCOMMISSION,:pSETTLEMENTCCYPRINCIPAL,:pSETTLEMENTCCYACCRUED,:pDAYSTOMATURITY,:pSETTLEMENTCCYTOTALCOMMN
    //                                                                    ,:pPRIMARYEXCHANGECODE,:pEXECUTIONPLATFORM,:pCLIENTAUTH,:pLASTLOGINUUID,:pORIGINALTKTID,:pSETTLEMENTCCYPRICE,:pORDERSTATUS,:pRTTMINDICATOR,:pRTTMREFERENCEID,:pCPARTYENCODEDLONGNAME,:pBLOOMBERGGLOBALIDENTIFIER,:pSECONDHALFTDR2TDRTRADE,:pCASHREVLOFFSETTICKETIND
    //                                                                    ,:pLIMITPRICE,:pTRADEFLATFLAG,:pMSRBREPORTABLE,:pTRADEFEED_ID,:pFILENAME ,:pFILEDATE,:pFILENUMBER
    //                                                                     ,:pACCRUEDINTERESTOVERDRIVE,:pAFFILIATEDTICKETNUMBER,:pAGENCYLINKEDTICKETNUMBER,:pAGENCYTRADEREFERENCETICKETN,:pALERTID,:pALTERNATIVESECURITYID,:pASOFDATETIMEZONETXT,:pAUTOEXAPPLNAME,:pAUTOEXBROKERNAME,:pAUTOEXDEALERSINCOMP,:pAUTOEXNUMBER
    //                                                                     ,:pAUTOEXORDERINQUIRY,:pAUTOEXSTATUS,:pAVGLIFE,:pBANKDESTINATIONID,:pBASEPRICEONTKT,:pBASEYIELDONTKT,:pBASKETID,:pBLOOMBERGFUNCTIONS,:pBLOOMBERGREFERENCENUMBER,:pBUYSIDEREASONCODE,:pBXTLOGIN,:pCANCELDATE,:pCANCELDUETOMATCH,:pCANCELTRANSACTIONPNL_ATTIBUT
    //                                                                     ,:pCASHBROKERCODE,:pCASHLINKEDREVERSALTICKETNUMB,:pCASHREVERSALDATE,:pCLEARINGBROKER,:pCONCESSIONSTIPULATIONVARIANC,:pCONTINGENTFLAT,:pCONTRACTSIZE,:pCONVEXITY,:pCORRECTDACTUALNAV,:pCORRECTEDTRANSACTIONPNL_ATTR,:pCORRECTIONASORIGINAL
    //                                                                    ,:pCURRENTFLOATERCOUPON,:pCUSTODYSAFEKEEPINGNUMBER,:pCUTTRADE,:pDATETRADEWASAUTHORIZED,:pDELAYEDDELIVERYDATE,:pDIRTYPRICE,:pDISCOUNTRATE,:pEDITTRANSATIONPNL_ATTRIBUTION,:pELECTRONICEXECUTIONFLAG,:pELECTRONICTRADE,:pENTRYOFMUTUALFUNDSHARESWASBYS
    //                                                                     ,:pEQPLSUBFLAG,:pESTIMATEFLAG,:pFEEDREASONCODE,:pFIRMCANCELDATE,:pFXSTRATEGYCODE,:pGENERALLEDGERACCOUNT,:pGOODMILLIONNUMBER,:pGSPREAD,:pGSPREADCURVEID,:pHEDGETICKETNUMBER,:pINUNITSFLAG,:pIPOFLAG,:pISCFD,:pISSUERCOMPANYID,:pJAPANNUMBER
    //                                                                     ,:pJOURNALREASONCODE,:pLONGNOTES,:pLOTNUMBER,:pMIDDLEOFFICETRADEINDICATOR,:pMMKTISSUERCODE,:pMORTGAGEBRADYINDEXBONDFACTOR,:pMSRBPRICECODE,:pMTGEFACTORDATE,:pNAMEOFUSERWHOAUTHORIZEDTRADE,:pNAVESTIMATED,:pNETWIRE,:pNEWTRANSACTIONPNL_ATTRIBUTION
    //                                                                     ,:pNONREGSETTLEREPORATE,:pOCCOPTIONTICKER,:pOMSGOODTILDATE,:pOMSLIMITPX,:pOMSORDERNUMBER,:pOMSSTOPPX,:pOMSTIMEINFORCE,:pOMSTRANSACTIONID,:pOPTIONCONTRACTSIZE,:pOPTIONSDELTA,:pORDERTYPE,:pPARTIALALLOCATIONEXISTS,:pPAYNOTIONALAMOUNT,:pPLATFORMNAME,:pPOOLNUMBER
    //                                                                     ,:pPREPAYMENTSPEEDANDTYPECONTRAC,:pPRIMARYMMKTID,:pPRIMEBROKER,:pPRINCIPALAGENCYFLAG,:pPROGRAMBASKETFLAG,:pPUTCALLINDICATOR,:pRECEIVENOTIONAL,:pRELATEDSLATETICKETNUMBER,:pREPORTTOFCA,:pRETAILFEED,:pRETAILFEEDWEBTRADEFIELD,:pSALESPERSONNAME,:pSBBSREPOINTEREST
    //                                                                     ,:pSBBSREPORATE,:pSECONDARYTRANSACTIONTICKETNUM,:pSECURITYISPENCEQUOTED,:pSHORTNOTES,:pSOFTDOLLARPERCENTAGE,:pSOURCECODE,:pSPREAD,:pSTEPOUTSHARES,:pSTRIPCODE,:pSTRIPTICKETFLAG,:pSWAPSPREAD,:pSYSTEMATICINTERNALIZERBIC,:pTAXLOTS,:pTKTUSESBASEPRICE,:pTRACEPRICEMEMO
    //                                                                     ,:pTRACEREPORTABLE,:pTRACESPECIALPRICE,:pTRACESPECIALPROCESSINGFLAG,:pTRADECREATEDBYMORTGAGEFACTORC,:pTRADETYPE,:pTRANSACTIONCODE,:pTRANSACTIONNUMBEROFORIGINTRAN,:pTWENTYFOURHOURTRADEFLAG,:pTWENTYFOURHOURTRADETICKETNUMB
    //                                                                     ,:pUNDERLYINGCUSIP,:pUNDERLYINGOPTIONISIN,:pUNIQUETRADEIDENTIFIER,:pUNIQUETRADEIDENTIFIERFORCLEAR,:pUNWINDTRANSACTIONPNL_ATTRIBUT,:pUSERGRPNTRADINGDESKCODE,:pUSERGRPNTRADINGDESKNAME,:pUSERNUMBEROFPERSONWHOAPPROVED,:pVARIABLEPENSIONLIVREESPREAD,:pWICONVERSIONFLAG,:pWIMUNIFLAG,:pYIELDENTEREDTRADE)";








    //                        using (NpgsqlCommand oraCommand = new NpgsqlCommand(strInsertQuery, conn))
    //                        {
    //                            oraCommand.Parameters.Add("pBLOOMBERGFIRMID", comModl.BLOOMBERGFIRMID);
    //                            oraCommand.Parameters.Add("pTRANSACTIONNUMBER", comModl.TRANSACTIONNUMBER);
    //                            oraCommand.Parameters.Add("pSECURITYIDENTIFIERFLAG", comModl.SECURITYIDENTIFIERFLAG);
    //                            oraCommand.Parameters.Add("pSECURITYIDENTIFIER", comModl.SECURITYIDENTIFIER);
    //                            oraCommand.Parameters.Add("pSECURITYCURRENCYISOCODE", comModl.SECURITYCURRENCYISOCODE);
    //                            oraCommand.Parameters.Add("pSECURITYPRODUCTKEY", comModl.SECURITYPRODUCTKEY);
    //                            oraCommand.Parameters.Add("pBLOOMBERGIDENTIFIER", comModl.BLOOMBERGIDENTIFIER);
    //                            oraCommand.Parameters.Add("pTICKER", comModl.TICKER);
    //                            oraCommand.Parameters.Add("pCOUPONSTRIKEPRICE", comModl.COUPONSTRIKEPRICE);
    //                            oraCommand.Parameters.Add("pMATURITYDATEEXPIRATIONDATE", Helper.ToOracleDateFormat(comModl.MATURITYDATEEXPIRATIONDATE.ToString()));
    //                            oraCommand.Parameters.Add("pSERIESEXCHANGECODE", comModl.SERIESEXCHANGECODE);
    //                            oraCommand.Parameters.Add("pBUYSELLCOVERSHORTFLAG", comModl.BUYSELLCOVERSHORTFLAG);
    //                            oraCommand.Parameters.Add("pRECORDTYPE", comModl.RECORDTYPE);
    //                            oraCommand.Parameters.Add("pTRADEDATE", Helper.ToOracleDateFormat(comModl.TRADEDATE.ToString())); //today
    //                            oraCommand.Parameters.Add("pASOFTRADEDATE", Helper.ToOracleDateFormat(comModl.ASOFTRADEDATE.ToString()));
    //                            oraCommand.Parameters.Add("pSETTLEMENTDATE", Helper.ToOracleDateFormat(comModl.SETTLEMENTDATE.ToString()));
    //                            oraCommand.Parameters.Add("pPRICE", comModl.PRICE);
    //                            oraCommand.Parameters.Add("pYIELD", comModl.YIELD);


    //                            oraCommand.Parameters.Add("pTRADEAMOUNT", comModl.TRADEAMOUNT);
    //                            oraCommand.Parameters.Add("pCUSTOMERACCOUNTCOUNTERPARTY", comModl.CUSTOMERACCOUNTCOUNTERPARTY);
    //                            oraCommand.Parameters.Add("pACCOUNTCOUNTERPARTY", comModl.ACCOUNTCOUNTERPARTYSHORTNAME);
    //                            oraCommand.Parameters.Add("pSETTLEMENTLOCATIONINDICATOR", comModl.SETTLEMENTLOCATIONINDICATOR);
    //                            oraCommand.Parameters.Add("pPRODUCTSUBFLAG", comModl.PRODUCTSUBFLAG);
    //                            oraCommand.Parameters.Add("pPRINCIPALLOANAMOUNT", comModl.PRINCIPALLOANAMOUNT);
    //                            oraCommand.Parameters.Add("pACCRUEDINTERESTREPOINTEREST", comModl.ACCRUEDINTERESTREPOINTEREST);
    //                            oraCommand.Parameters.Add("pTRADERACCOUNTNAME", comModl.TRADERACCOUNTNAME);
    //                            oraCommand.Parameters.Add("pNUMBEROFDAYSACCRUED", comModl.NUMBEROFDAYSACCRUED);
    //                            oraCommand.Parameters.Add("pCOMMON_ID", comModl.COMMON_ID);
    //                            oraCommand.Parameters.Add("pTIMEOFSLATE", comModl.TIMEOFSLATE);
    //                            oraCommand.Parameters.Add("pTIMEOFSALESTICKET", comModl.TIMEOFSALESTICKET);
    //                            oraCommand.Parameters.Add("pLASTLOGIN", comModl.LASTLOGIN);
    //                            oraCommand.Parameters.Add("pMASTERTICKETNUMBER", comModl.MASTERTICKETNUMBER);
    //                            oraCommand.Parameters.Add("pCONCSTIPULATIONVARREPORATE", comModl.CONCSTIPULATIONVARREPORATE);
    //                            oraCommand.Parameters.Add("pSALESPERSONLOGIN", comModl.SALESPERSONLOGIN);
    //                            oraCommand.Parameters.Add("pSETTLEMENTCURRENCYISOCODE", comModl.SETTLEMENTCURRENCYISOCODE);
    //                            oraCommand.Parameters.Add("pSETTLEMENTCURRENCYRATE", comModl.SETTLEMENTCURRENCYRATE);
    //                            oraCommand.Parameters.Add("pCANCELDUETOCORRECTION", comModl.CANCELDUETOCORRECTION);
    //                            oraCommand.Parameters.Add("pINVERTFLAG", comModl.INVERTFLAG);
    //                            oraCommand.Parameters.Add("pWORKOUTDATE", Helper.ToOracleDateFormat(comModl.WORKOUTDATE.ToString()));//today
    //                            oraCommand.Parameters.Add("pWORKOUTPRICE", comModl.WORKOUTPRICE);
    //                            oraCommand.Parameters.Add("pAUTOEXTRADEFLAG", comModl.AUTOEXTRADEFLAG);
    //                            oraCommand.Parameters.Add("pTRADERTOTRADERTRADEFLAG", comModl.TRADERTOTRADERTRADEFLAG);
    //                            oraCommand.Parameters.Add("pVERSION", comModl.VERSION);
    //                            oraCommand.Parameters.Add("pUNIQUEBLOOMBERGID", comModl.UNIQUEBLOOMBERGID);
    //                            oraCommand.Parameters.Add("pEXTENDEDPRECISIONPRICE", comModl.EXTENDEDPRECISIONPRICE);
    //                            oraCommand.Parameters.Add("pSYSTEMDATE", Helper.ToOracleDateFormat(comModl.SYSTEMDATE.ToString())); //today
    //                            oraCommand.Parameters.Add("pIMPACTFLAG", comModl.IMPACTFLAG);
    //                            oraCommand.Parameters.Add("pTRADERACCOUNTNUMBER", comModl.TRADERACCOUNTNUMBER);
    //                            oraCommand.Parameters.Add("pSETTLEMENTLOCATIONABBR", comModl.SETTLEMENTLOCATIONABBREVIATION);
    //                            oraCommand.Parameters.Add("pTRANSACTIONTYPE", comModl.TRANSACTIONTYPE);
    //                            oraCommand.Parameters.Add("pINTERESTATMATURITY", comModl.INTERESTATMATURITY);
    //                            oraCommand.Parameters.Add("pENTEREDTICKETUSERID", comModl.ENTEREDTICKETUSERID);
    //                            oraCommand.Parameters.Add("pALLOCATEDTICKETUSERID", comModl.ALLOCATEDTICKETUSERID);
    //                            oraCommand.Parameters.Add("pOMREASONCODE", comModl.OMREASONCODE);
    //                            oraCommand.Parameters.Add("pOMSBLOCKNAME", comModl.OMSBLOCKNAME);
    //                            oraCommand.Parameters.Add("pQUOTETYPEINDICATOR", comModl.QUOTETYPEINDICATOR);
    //                            oraCommand.Parameters.Add("pISSUEDATE", Helper.ToOracleDateFormat(comModl.ISSUEDATE.ToString()));//today
    //                            oraCommand.Parameters.Add("pFIRMTRADEDATE", Helper.ToOracleDateFormat(comModl.FIRMTRADEDATE.ToString())); //today
    //                            oraCommand.Parameters.Add("pFIRMASOFDATE", Helper.ToOracleDateFormat(comModl.FIRMASOFDATE.ToString())); //today
    //                            oraCommand.Parameters.Add("pFIRMTIMEOFSLATE", comModl.FIRMTIMEOFSLATE);
    //                            oraCommand.Parameters.Add("pFIRMTIMEOFSALESTICKET", comModl.FIRMTIMEOFSALESTICKET);
    //                            oraCommand.Parameters.Add("pSTEPOUTBROKER", comModl.STEPOUTBROKER);
    //                            oraCommand.Parameters.Add("pSOFTDOLLARFLAG", comModl.SOFTDOLLARFLAG);
    //                            oraCommand.Parameters.Add("pTIPFACTORESTIMATED", comModl.TIPFACTORESTIMATED);
    //                            oraCommand.Parameters.Add("pTOTALTRADEAMOUNT", comModl.TOTALTRADEAMOUNT);
    //                            oraCommand.Parameters.Add("pMASTERACCOUNT", comModl.MASTERACCOUNT);
    //                            oraCommand.Parameters.Add("pMASTERACCOUNTNAME", comModl.MASTERACCOUNTNAME);
    //                            oraCommand.Parameters.Add("pCTMMATCHSTATUS", comModl.CTMMATCHSTATUS);
    //                            oraCommand.Parameters.Add("pMATCHDATE", Helper.ToOracleDateFormat(comModl.MATCHDATE.ToString()));
    //                            oraCommand.Parameters.Add("pISDIRTYPRICE", comModl.ISDIRTYPRICE);
    //                            oraCommand.Parameters.Add("pTSAMINDICATOR", comModl.TSAMINDICATOR);
    //                            oraCommand.Parameters.Add("pTAXLOTMETHOD", comModl.TAXLOTMETHOD);
    //                            oraCommand.Parameters.Add("pSECURITYPRICE", comModl.SECURITYPRICE);
    //                            oraCommand.Parameters.Add("pLOCALEXCHANGE", comModl.LOCALEXCHANGE);
    //                            oraCommand.Parameters.Add("pSETTLEMENTAMOUNT", comModl.SETTLEMENTAMOUNT);
    //                            oraCommand.Parameters.Add("pFUNDCCYPRINCIPAL", comModl.FUNDCCYPRINCIPAL);
    //                            oraCommand.Parameters.Add("pFUNDCCYACCRUED", comModl.FUNDCCYACCRUED);
    //                            oraCommand.Parameters.Add("pFUNDCCYTOTALCOMMISSION", comModl.FUNDCCYTOTALCOMMISSION);
    //                            oraCommand.Parameters.Add("pREDEMPTIONCCYPRINCIPAL", comModl.REDEMPTIONCCYPRINCIPAL);
    //                            oraCommand.Parameters.Add("pREDEMPTIONCCYACCRUED", comModl.REDEMPTIONCCYACCRUED);
    //                            oraCommand.Parameters.Add("pREDEMPTIONTOTALCOMMISSION", comModl.REDEMPTIONTOTALCOMMISSION);
    //                            oraCommand.Parameters.Add("pSETTLEMENTCCYPRINCIPAL", comModl.SETTLEMENTCCYPRINCIPAL);
    //                            oraCommand.Parameters.Add("pSETTLEMENTCCYACCRUED", comModl.SETTLEMENTCCYACCRUED);
    //                            oraCommand.Parameters.Add("pDAYSTOMATURITY", comModl.DAYSTOMATURITY);
    //                            oraCommand.Parameters.Add("pSETTLEMENTCCYTOTALCOMMN", comModl.SETTLEMENTCCYTOTALCOMMISSION);


    //                            oraCommand.Parameters.Add("pPRIMARYEXCHANGECODE", comModl.PRIMARYEXCHANGECODE);
    //                            oraCommand.Parameters.Add("pEXECUTIONPLATFORM", comModl.EXECUTIONPLATFORM);
    //                            oraCommand.Parameters.Add("pCLIENTAUTH", comModl.CLIENTAUTH);
    //                            oraCommand.Parameters.Add("pLASTLOGINUUID", comModl.LASTLOGINUUID);
    //                            oraCommand.Parameters.Add("pORIGINALTKTID", comModl.ORIGINALTKTID);
    //                            oraCommand.Parameters.Add("pSETTLEMENTCCYPRICE", comModl.SETTLEMENTCCYPRICE); // cecimal
    //                            oraCommand.Parameters.Add("pORDERSTATUS", comModl.ORDERSTATUS);
    //                            oraCommand.Parameters.Add("pRTTMINDICATOR", comModl.RTTMINDICATOR);
    //                            oraCommand.Parameters.Add("pRTTMREFERENCEID", comModl.RTTMREFERENCEID);
    //                            oraCommand.Parameters.Add("pCPARTYENCODEDLONGNAME", comModl.CPARTYENCODEDLONGNAME);
    //                            oraCommand.Parameters.Add("pBLOOMBERGGLOBALIDENTIFIER", comModl.BLOOMBERGGLOBALIDENTIFIER);
    //                            oraCommand.Parameters.Add("pSECONDHALFTDR2TDRTRADE", comModl.SECONDHALFTDR2TDRTRADE);
    //                            oraCommand.Parameters.Add("pCASHREVLOFFSETTICKETIND", comModl.CASHREVLOFFSETTICKETIND);

    //                            oraCommand.Parameters.Add("pLIMITPRICE", comModl.LIMITPRICE); // cecimal

    //                            oraCommand.Parameters.Add("pTRADEFLATFLAG", comModl.TRADEFLATFLAG);
    //                            oraCommand.Parameters.Add("pMSRBREPORTABLE", comModl.MSRBREPORTABLE);
    //                            oraCommand.Parameters.Add("pTRADEFEED_ID", comModl.TRADEFEED_ID);
    //                            oraCommand.Parameters.Add("pFILENAME", comModl.FILENAME);
    //                            oraCommand.Parameters.Add("pFILEDATE", Helper.ToOracleDateFormat(comModl.FILEDATE.ToString())); // today
    //                            oraCommand.Parameters.Add("pFILENUMBER", comModl.FILENUMBER);

    //                            // new para start from here

    //                            try
    //                            {
    //                                oraCommand.Parameters.Add("pACCRUEDINTERESTOVERDRIVE", comModl.ACCRUEDINTERESTOVERDRIVE);
    //                                oraCommand.Parameters.Add("pAFFILIATEDTICKETNUMBER", comModl.AFFILIATEDTICKETNUMBER);
    //                                oraCommand.Parameters.Add("pAGENCYLINKEDTICKETNUMBER", comModl.AGENCYLINKEDTICKETNUMBER);
    //                                oraCommand.Parameters.Add("pAGENCYTRADEREFERENCETICKETN", comModl.AGENCYTRADEREFERENCETICKETN); //issue with this column
    //                                oraCommand.Parameters.Add("pALERTID", comModl.ALERTID);


    //                                //Console.WriteLine("success 1");
    //                                oraCommand.Parameters.Add("pALTERNATIVESECURITYID", comModl.ALTERNATIVESECURITYID);
    //                                oraCommand.Parameters.Add("pASOFDATETIMEZONETXT", comModl.ASOFDATETIMEZONETXT);//date
    //                                oraCommand.Parameters.Add("pAUTOEXAPPLNAME", comModl.AUTOEXAPPLNAME);
    //                                oraCommand.Parameters.Add("pAUTOEXBROKERNAME", comModl.AUTOEXBROKERNAME);
    //                                oraCommand.Parameters.Add("pAUTOEXDEALERSINCOMP", comModl.AUTOEXDEALERSINCOMP);
    //                                oraCommand.Parameters.Add("pAUTOEXNUMBER", comModl.AUTOEXNUMBER);


    //                                oraCommand.Parameters.Add("pAUTOEXORDERINQUIRY", comModl.AUTOEXORDERINQUIRY);
    //                                oraCommand.Parameters.Add("pAUTOEXSTATUS", comModl.AUTOEXSTATUS);
    //                                oraCommand.Parameters.Add("pAVGLIFE", comModl.AVGLIFE);
    //                                oraCommand.Parameters.Add("pBANKDESTINATIONID", comModl.BANKDESTINATIONID);
    //                                oraCommand.Parameters.Add("pBASEPRICEONTKT", comModl.BASEPRICEONTKT);
    //                                oraCommand.Parameters.Add("pBASEYIELDONTKT", comModl.BASEYIELDONTKT);
    //                                oraCommand.Parameters.Add("pBASKETID", comModl.BASKETID);

    //                                oraCommand.Parameters.Add("pBLOOMBERGFUNCTIONS", comModl.BLOOMBERGFUNCTIONS);
    //                                oraCommand.Parameters.Add("pBLOOMBERGREFERENCENUMBER", comModl.BLOOMBERGREFERENCENUMBER);
    //                                oraCommand.Parameters.Add("pBUYSIDEREASONCODE", comModl.BUYSIDEREASONCODE);
    //                                oraCommand.Parameters.Add("pBXTLOGIN", comModl.BXTLOGIN);
    //                                oraCommand.Parameters.Add("pCANCELDATE", Helper.ToOracleDateFormat(comModl.CANCELDATE.ToString()));//date
    //                                oraCommand.Parameters.Add("pCANCELDUETOMATCH", comModl.CANCELDUETOMATCH);
    //                                oraCommand.Parameters.Add("pCANCELTRANSACTIONPNL_ATTIBUT", comModl.CANCELTRANSACTIONPNL_ATTIBUT);



    //                                oraCommand.Parameters.Add("pCASHBROKERCODE", comModl.CASHBROKERCODE);
    //                                oraCommand.Parameters.Add("pCASHLINKEDREVERSALTICKETNUMB", comModl.CASHLINKEDREVERSALTICKETNUMBER);
    //                                oraCommand.Parameters.Add("pCASHREVERSALDATE", Helper.ToOracleDateFormat(comModl.CASHREVERSALDATE.ToString())); //date
    //                                oraCommand.Parameters.Add("pCLEARINGBROKER", comModl.CLEARINGBROKER);
    //                                oraCommand.Parameters.Add("pCONCESSIONSTIPULATIONVARIANC", comModl.CONCESSIONSTIPULATIONVARIANCER);
    //                                oraCommand.Parameters.Add("pCONTINGENTFLAT", comModl.CONTINGENTFLAT);
    //                                oraCommand.Parameters.Add("pCONTRACTSIZE", comModl.CONTRACTSIZE);
    //                                oraCommand.Parameters.Add("pCONVEXITY", comModl.CONVEXITY);
    //                                //Console.WriteLine("success 4");
    //                                oraCommand.Parameters.Add("pCORRECTDACTUALNAV", comModl.CORRECTDACTUALNAV);
    //                                oraCommand.Parameters.Add("pCORRECTEDTRANSACTIONPNL_ATTR", comModl.CORRECTEDTRANSACTIONPNL_ATTRIB);
    //                                oraCommand.Parameters.Add("pCORRECTIONASORIGINAL", comModl.CORRECTIONASORIGINAL);


    //                                oraCommand.Parameters.Add("pCURRENTFLOATERCOUPON", comModl.CURRENTFLOATERCOUPON);
    //                                oraCommand.Parameters.Add("pCUSTODYSAFEKEEPINGNUMBER", comModl.CUSTODYSAFEKEEPINGNUMBER);
    //                                oraCommand.Parameters.Add("pCUTTRADE", comModl.CUTTRADE);
    //                                // Console.WriteLine("success 5");
    //                                oraCommand.Parameters.Add("pDATETRADEWASAUTHORIZED", comModl.DATETRADEWASAUTHORIZED);//date
    //                                oraCommand.Parameters.Add("pDELAYEDDELIVERYDATE", Helper.ToOracleDateFormat(comModl.DELAYEDDELIVERYDATE.ToString()));//date
    //                                oraCommand.Parameters.Add("pDIRTYPRICE", comModl.DIRTYPRICE);
    //                                oraCommand.Parameters.Add("pDISCOUNTRATE", comModl.DISCOUNTRATE);
    //                                oraCommand.Parameters.Add("pEDITTRANSATIONPNL_ATTRIBUTION", comModl.EDITTRANSATIONPNL_ATTRIBUTION);
    //                                oraCommand.Parameters.Add("pELECTRONICEXECUTIONFLAG", comModl.ELECTRONICEXECUTIONFLAG);
    //                                oraCommand.Parameters.Add("pELECTRONICTRADE", comModl.ELECTRONICTRADE);
    //                                oraCommand.Parameters.Add("pENTRYOFMUTUALFUNDSHARESWASBYS", comModl.ENTRYOFMUTUALFUNDSHARESWASBYSE);
    //                                oraCommand.Parameters.Add("pEQPLSUBFLAG", comModl.EQPLSUBFLAG);
    //                                oraCommand.Parameters.Add("pESTIMATEFLAG", comModl.ESTIMATEFLAG);
    //                                //Console.WriteLine("success 6");
    //                                oraCommand.Parameters.Add("pFEEDREASONCODE", comModl.FEEDREASONCODE);
    //                                oraCommand.Parameters.Add("pFIRMCANCELDATE", Helper.ToOracleDateFormat(comModl.FIRMCANCELDATE.ToString()));//date
    //                                oraCommand.Parameters.Add("pFXSTRATEGYCODE", comModl.FXSTRATEGYCODE);
    //                                oraCommand.Parameters.Add("pGENERALLEDGERACCOUNT", comModl.GENERALLEDGERACCOUNT);
    //                                oraCommand.Parameters.Add("pGOODMILLIONNUMBER", comModl.GOODMILLIONNUMBER);
    //                                oraCommand.Parameters.Add("pGSPREAD", comModl.GSPREAD);
    //                                oraCommand.Parameters.Add("pGSPREADCURVEID", comModl.GSPREADCURVEID);
    //                                oraCommand.Parameters.Add("pHEDGETICKETNUMBER", comModl.HEDGETICKETNUMBER);
    //                                oraCommand.Parameters.Add("pINUNITSFLAG", comModl.INUNITSFLAG);
    //                                oraCommand.Parameters.Add("pIPOFLAG", comModl.IPOFLAG);
    //                                oraCommand.Parameters.Add("pISCFD", comModl.ISCFD);
    //                                //Console.WriteLine("success 7");
    //                                oraCommand.Parameters.Add("pISSUERCOMPANYID", comModl.ISSUERCOMPANYID);
    //                                oraCommand.Parameters.Add("pJAPANNUMBER", comModl.JAPANNUMBER);
    //                                oraCommand.Parameters.Add("pJOURNALREASONCODE", comModl.JOURNALREASONCODE);
    //                                oraCommand.Parameters.Add("pLONGNOTES", comModl.LONGNOTES);
    //                                oraCommand.Parameters.Add("pLOTNUMBER", comModl.LOTNUMBER);
    //                                oraCommand.Parameters.Add("pMIDDLEOFFICETRADEINDICATOR", comModl.MIDDLEOFFICETRADEINDICATOR);
    //                                oraCommand.Parameters.Add("pMMKTISSUERCODE", comModl.MMKTISSUERCODE);
    //                                oraCommand.Parameters.Add("pMORTGAGEBRADYINDEXBONDFACTOR", comModl.MORTGAGEBRADYINDEXBONDFACTOR);
    //                                oraCommand.Parameters.Add("pMSRBPRICECODE", Helper.ToOracleDateFormat(comModl.MSRBPRICECODE.ToString()));
    //                                oraCommand.Parameters.Add("pMTGEFACTORDATE", comModl.MTGEFACTORDATE);//date
    //                                oraCommand.Parameters.Add("pNAMEOFUSERWHOAUTHORIZEDTRADE", comModl.NAMEOFUSERWHOAUTHORIZEDTRADE);
    //                                oraCommand.Parameters.Add("pNAVESTIMATED", comModl.NAVESTIMATED);
    //                                oraCommand.Parameters.Add("pNETWIRE", comModl.NETWIRE);
    //                                //Console.WriteLine("success 8");
    //                                oraCommand.Parameters.Add("pNEWTRANSACTIONPNL_ATTRIBUTION", comModl.NEWTRANSACTIONPNL_ATTRIBUTION);
    //                                oraCommand.Parameters.Add("pNONREGSETTLEREPORATE", comModl.NONREGSETTLEREPORATE);
    //                                oraCommand.Parameters.Add("pOCCOPTIONTICKER", comModl.OCCOPTIONTICKER);
    //                                oraCommand.Parameters.Add("pOMSGOODTILDATE", Helper.ToOracleDateFormat(comModl.OMSGOODTILDATE.ToString()));//date
    //                                oraCommand.Parameters.Add("pOMSLIMITPX", comModl.OMSLIMITPX);
    //                                oraCommand.Parameters.Add("pOMSORDERNUMBER", comModl.OMSORDERNUMBER);
    //                                oraCommand.Parameters.Add("pOMSSTOPPX", comModl.OMSSTOPPX);
    //                                oraCommand.Parameters.Add("pOMSTIMEINFORCE", comModl.OMSTIMEINFORCE);
    //                                oraCommand.Parameters.Add("pOMSTRANSACTIONID", comModl.OMSTRANSACTIONID);
    //                                oraCommand.Parameters.Add("pOPTIONCONTRACTSIZE", comModl.OPTIONCONTRACTSIZE);
    //                                oraCommand.Parameters.Add("pOPTIONSDELTA", comModl.OPTIONSDELTA);
    //                                oraCommand.Parameters.Add("pORDERTYPE", comModl.ORDERTYPE);
    //                                // Console.WriteLine("success 9");
    //                                oraCommand.Parameters.Add("pPARTIALALLOCATIONEXISTS", comModl.PARTIALALLOCATIONEXISTS);
    //                                oraCommand.Parameters.Add("pPAYNOTIONALAMOUNT", comModl.PAYNOTIONALAMOUNT);
    //                                oraCommand.Parameters.Add("pPLATFORMNAME", comModl.PLATFORMNAME);
    //                                oraCommand.Parameters.Add("pPOOLNUMBER", comModl.POOLNUMBER);
    //                                oraCommand.Parameters.Add("pPREPAYMENTSPEEDANDTYPECONTRAC", comModl.PREPAYMENTSPEEDANDTYPECONTRACT);
    //                                oraCommand.Parameters.Add("pPRIMARYMMKTID", comModl.PRIMARYMMKTID);
    //                                oraCommand.Parameters.Add("pPRIMEBROKER", comModl.PRIMEBROKER);
    //                                oraCommand.Parameters.Add("pPRINCIPALAGENCYFLAG", comModl.PRINCIPALAGENCYFLAG);
    //                                oraCommand.Parameters.Add("pPROGRAMBASKETFLAG", comModl.PROGRAMBASKETFLAG);
    //                                oraCommand.Parameters.Add("pPUTCALLINDICATOR", comModl.PUTCALLINDICATOR);
    //                                oraCommand.Parameters.Add("pRECEIVENOTIONAL", comModl.RECEIVENOTIONAL);
    //                                //  Console.WriteLine("success 10");
    //                                oraCommand.Parameters.Add("pRELATEDSLATETICKETNUMBER", comModl.RELATEDSLATETICKETNUMBER);
    //                                oraCommand.Parameters.Add("pREPORTTOFCA", comModl.REPORTTOFCA);
    //                                oraCommand.Parameters.Add("pRETAILFEED", comModl.RETAILFEED);
    //                                oraCommand.Parameters.Add("pRETAILFEEDWEBTRADEFIELD", comModl.RETAILFEEDWEBTRADEFIELD);
    //                                oraCommand.Parameters.Add("pSALESPERSONNAME", comModl.SALESPERSONNAME);
    //                                oraCommand.Parameters.Add("pSBBSREPOINTEREST", comModl.SBBSREPOINTEREST);
    //                                oraCommand.Parameters.Add("pSBBSREPORATE", comModl.SBBSREPORATE);
    //                                oraCommand.Parameters.Add("pSECONDARYTRANSACTIONTICKETNUM", comModl.SECONDARYTRANSACTIONTICKETNUMB);
    //                                oraCommand.Parameters.Add("pSECURITYISPENCEQUOTED", comModl.SECURITYISPENCEQUOTED);
    //                                oraCommand.Parameters.Add("pSHORTNOTES", comModl.SHORTNOTES);
    //                                oraCommand.Parameters.Add("pSOFTDOLLARPERCENTAGE", comModl.SOFTDOLLARPERCENTAGE);
    //                                oraCommand.Parameters.Add("pSOURCECODE", comModl.SOURCECODE);
    //                                oraCommand.Parameters.Add("pSPREAD", comModl.SPREAD);

    //                                // Console.WriteLine("success 11");
    //                                oraCommand.Parameters.Add("pSTEPOUTSHARES", comModl.STEPOUTSHARES);
    //                                oraCommand.Parameters.Add("pSTRIPCODE", comModl.STRIPCODE);
    //                                oraCommand.Parameters.Add("pSTRIPTICKETFLAG", comModl.STRIPTICKETFLAG);
    //                                oraCommand.Parameters.Add("pSWAPSPREAD", comModl.SWAPSPREAD);
    //                                oraCommand.Parameters.Add("pSYSTEMATICINTERNALIZERBIC", comModl.SYSTEMATICINTERNALIZERBIC);
    //                                oraCommand.Parameters.Add("pTAXLOTS", comModl.TAXLOTS);
    //                                oraCommand.Parameters.Add("pTKTUSESBASEPRICE", comModl.TKTUSESBASEPRICE);
    //                                oraCommand.Parameters.Add("pTRACEPRICEMEMO", comModl.TRACEPRICEMEMO);
    //                                oraCommand.Parameters.Add("pTRACEREPORTABLE", comModl.TRACEREPORTABLE);
    //                                oraCommand.Parameters.Add("pTRACESPECIALPRICE", comModl.TRACESPECIALPRICE);
    //                                oraCommand.Parameters.Add("pTRACESPECIALPROCESSINGFLAG", comModl.TRACESPECIALPROCESSINGFLAG);
    //                                oraCommand.Parameters.Add("pTRADECREATEDBYMORTGAGEFACTORC", comModl.TRADECREATEDBYMORTGAGEFACTORCO);
    //                                oraCommand.Parameters.Add("pTRADETYPE", comModl.TRADETYPE);

    //                                // Console.WriteLine("success 12");

    //                                oraCommand.Parameters.Add("pTRANSACTIONCODE", comModl.TRANSACTIONCODE);
    //                                oraCommand.Parameters.Add("pTRANSACTIONNUMBEROFORIGINTRAN", comModl.TRANSACTIONNUMBEROFORIGINTRANS);
    //                                oraCommand.Parameters.Add("pTWENTYFOURHOURTRADEFLAG", comModl.TWENTYFOURHOURTRADEFLAG);
    //                                oraCommand.Parameters.Add("pTWENTYFOURHOURTRADETICKETNUMB", comModl.TWENTYFOURHOURTRADETICKETNUMBE);
    //                                oraCommand.Parameters.Add("pUNDERLYINGCUSIP", comModl.UNDERLYINGCUSIP);
    //                                oraCommand.Parameters.Add("pUNDERLYINGOPTIONISIN", comModl.UNDERLYINGOPTIONISIN);
    //                                oraCommand.Parameters.Add("pUNIQUETRADEIDENTIFIER", comModl.UNIQUETRADEIDENTIFIER);
    //                                oraCommand.Parameters.Add("pUNIQUETRADEIDENTIFIERFORCLEAR", comModl.UNIQUETRADEIDENTIFIERFORCLEARI);
    //                                oraCommand.Parameters.Add("pUNWINDTRANSACTIONPNL_ATTRIBUT", comModl.UNWINDTRANSACTIONPNL_ATTRIBUTI);
    //                                oraCommand.Parameters.Add("pUSERGRPNTRADINGDESKCODE", comModl.USERGRPNTRADINGDESKCODE);
    //                                oraCommand.Parameters.Add("pUSERGRPNTRADINGDESKNAME", comModl.USERGRPNTRADINGDESKNAME);
    //                                oraCommand.Parameters.Add("pUSERNUMBEROFPERSONWHOAPPROVED", comModl.USERNUMBEROFPERSONWHOAPPROVEDT);
    //                                oraCommand.Parameters.Add("pVARIABLEPENSIONLIVREESPREAD", comModl.VARIABLEPENSIONLIVREESPREAD);
    //                                oraCommand.Parameters.Add("pWICONVERSIONFLAG", comModl.WICONVERSIONFLAG);
    //                                oraCommand.Parameters.Add("pWIMUNIFLAG", comModl.WIMUNIFLAG);
    //                                oraCommand.Parameters.Add("pYIELDENTEREDTRADE", comModl.YIELDENTEREDTRADE);
    //                            }
    //                            catch (Exception eer2)
    //                            {
    //                                Console.WriteLine("error para : \n" + eer2.Message);
    //                            }
    //                            // end para adding finish

    //                            try
    //                            {

    //                                oraCommand.ExecuteNonQuery();
    //                                TradeFeedInsert++;
    //                                Console.WriteLine("------------------------------------------------------------------commonDT-----------------------------------");
    //                                //Helper.WriteLogXml("Record inserted in [UT_TRADEFEEDS]");
    //                                Console.WriteLine("record inserted successfully [common_feeds]");
    //                            }
    //                            catch (Exception ex)
    //                            {
    //                                Console.WriteLine("error executeNonQuery :[common_feeds] \n" + ex.Message);

    //                                Helper.WriteLogXml("error Source : [common_feeds]\n" + ex.Data.Values);
    //                                Console.WriteLine("Stop----------TradeFeedID : " + common_id + " Bloomberg Identifier :" + comModl.UNIQUEBLOOMBERGID);
    //                                Console.ReadLine();
    //                            }
    //                        }

    //                        bool isAllowEntry = false;
    //                        if (isAllowEntry)
    //                        {

    //                            // -----------------------------------Processing for UT_NOTES table------------------------------------------------

    //                            DataRow[] shortnotes;
    //                            DataRow[] longNotes;

    //                            if (dataSet.Tables.Contains("ShortNotes"))
    //                            {
    //                                shortnotes = dataSet.Tables["ShortNotes"].Select("Common_Id ='" + common_id + "'");
    //                                if (shortnotes.Length > 0)
    //                                {
    //                                    int shortnoteid = Convert.ToInt32(shortnotes[0].ItemArray[0]);
    //                                    int TotalShortNotesRecord = 0;
    //                                    foreach (DataRow dtshortnote in dataSet.Tables["ShortNote"].Select("ShortNotes_Id ='" + shortnoteid + "'"))
    //                                    {
    //                                        SNL_NoteModel SNM = new SNL_NoteModel();
    //                                        SNM.index = Convert.ToInt32(dtshortnote.ItemArray[0]);// Index field
    //                                        SNM.Text = dtshortnote.ItemArray[1].ToString().Trim(); // Text field
    //                                        SNM.Note_ID = shortnoteid;// shortnote_id
    //                                        SNM.NoteCategory = "SN";
    //                                        // insert short note logic 

    //                                        string strInsertNotes = @"INSERT INTO UT_NOTES(INDX,NOTE_TEXT,NOTE_ID,NOTE_CATEGORY,COMMON_ID,FILENAME,FILEDATE,FILENUMBER) 
    //                                                                          VALUES(:pINDEX,:pNOTE_TEXT,:pNOTE_ID,:pNOTE_CATEGORY,:pCOMMON_ID,:pFILENAME,:pFILEDATE,:pFILENUMBER)";

    //                                        //string strInsertNotes = @"INSERT INTO UT_NOTES(INDEX,NOTE_TEXT,NOTE_ID,NOTE_CATEGORY,COMMON_ID,FILENAME,FILEDATE,FILENUMBER) 
    //                                        //                                      VALUES(:pINDEX,:pNOTE_TEXT,:pNOTE_ID,:pNOTE_CATEGORY,:pCOMMON_ID,:pFILENAME,:pFILEDATE,:pFILENUMBER)";

    //                                        //string strInsertNotes2 = @"INSERT INTO UT_NOTES(INDX,NOTE_TEXT,NOTE_ID,NOTE_CATEGORY,COMMON_ID) VALUES(" + SNM.index + ",'" + SNM.Text + "'," + SNM.Note_ID + ",'" + SNM.NoteCategory + "'," + common_id + ")";

    //                                        //Helper.WriteLogXml("strInsert note \n" + strInsertNotes2);
    //                                        //Console.WriteLine(strInsertNotes2);

    //                                        using (NpgsqlCommand oracmdNotes = new NpgsqlCommand(strInsertNotes, conn))
    //                                        {
    //                                            oracmdNotes.Parameters.Add("pINDEX", SNM.index);
    //                                            oracmdNotes.Parameters.Add("pNOTE_TEXT", SNM.Text);
    //                                            oracmdNotes.Parameters.Add("pNOTE_ID", SNM.Note_ID);
    //                                            oracmdNotes.Parameters.Add("pNOTE_CATEGORY", SNM.NoteCategory);
    //                                            oracmdNotes.Parameters.Add("pCOMMON_ID", common_id);

    //                                            oracmdNotes.Parameters.Add("pFILENAME", comModl.FILENAME);
    //                                            oracmdNotes.Parameters.Add("pFILEDATE", comModl.FILEDATE);
    //                                            oracmdNotes.Parameters.Add("pFILENUMBER", 1); // comModl.FILENUMBER
    //                                            try
    //                                            {
    //                                                oracmdNotes.ExecuteNonQuery();
    //                                                TotalShortNotesRecord++;
    //                                                Ut_NotesInsert++;
    //                                                //Helper.WriteLogXml("Record inserted in [UT_NOTES] [SN]");
    //                                                // Console.WriteLine("record inserted successfully [UT_NOTES]");
    //                                            }
    //                                            catch (Exception ex)
    //                                            {
    //                                                Console.WriteLine("error executeNonQuery in [UT_NOTES] \n" + ex.Message);
    //                                                Helper.WriteLogXml("Error in [UT_NOTES] [SN] \n" + ex.Message);
    //                                            }
    //                                        }

    //                                        strQuery = string.Empty;
    //                                    }

    //                                    Console.WriteLine("Total Inserted [Short Notes]:-------------------------> " + TotalShortNotesRecord);
    //                                }
    //                            }
    //                            if (dataSet.Tables.Contains("LongNotes"))
    //                            {
    //                                longNotes = dataSet.Tables["LongNotes"].Select("Common_Id ='" + common_id + "'");
    //                                int TotalLongNotesRecord = 0;

    //                                if (longNotes.Length > 0)
    //                                {
    //                                    int longnoteid = Convert.ToInt32(longNotes[0].ItemArray[0]);

    //                                    foreach (DataRow dtlongnote in dataSet.Tables["LongNote"].Select("LongNotes_Id ='" + longnoteid + "'"))
    //                                    {
    //                                        SNL_NoteModel SNM = new SNL_NoteModel();
    //                                        SNM.index = Convert.ToInt32(dtlongnote.ItemArray[0]);// Index field
    //                                        SNM.Text = dtlongnote.ItemArray[1].ToString().Trim(); // Text field
    //                                        SNM.Note_ID = longnoteid;// shortnote_id
    //                                        SNM.NoteCategory = "LN";
    //                                        // insert short note logic 
    //                                        string strInsertNotes = @"INSERT INTO UT_NOTES(INDX,NOTE_TEXT,NOTE_ID,NOTE_CATEGORY,COMMON_ID,FILENAME,FILEDATE,FILENUMBER) 
    //                                                                                             VALUES(:pINDEX,:pNOTE_TEXT,:pNOTE_ID,:pNOTE_CATEGORY,:pCOMMON_ID,:pFILENAME,:pFILEDATE,:pFILENUMBER)";
    //                                        using (NpgsqlCommand oracmdNotes = new NpgsqlCommand(strInsertNotes, conn))
    //                                        {
    //                                            oracmdNotes.Parameters.Add("pINDEX", SNM.index);
    //                                            oracmdNotes.Parameters.Add("pNOTE_TEXT", SNM.Text);
    //                                            oracmdNotes.Parameters.Add("pNOTE_ID", SNM.Note_ID);
    //                                            oracmdNotes.Parameters.Add("pNOTE_CATEGORY", SNM.NoteCategory);
    //                                            oracmdNotes.Parameters.Add("pCOMMON_ID", common_id);

    //                                            oracmdNotes.Parameters.Add("pFILENAME", comModl.FILENAME);
    //                                            oracmdNotes.Parameters.Add("pFILEDATE", comModl.FILEDATE);
    //                                            oracmdNotes.Parameters.Add("pFILENUMBER", 1); //comModl.FILENUMBER
    //                                            try
    //                                            {
    //                                                oracmdNotes.ExecuteNonQuery();
    //                                                TotalLongNotesRecord++;
    //                                                Ut_NotesInsert++;
    //                                                //Helper.WriteLogXml("Record inserted in [UT_NOTES] [LN]");
    //                                                // Console.WriteLine("Record inserted in [UT_NOTES] [LN]");
    //                                            }
    //                                            catch (Exception ex)
    //                                            {
    //                                                Console.WriteLine("error executeNonQuery in [UT_NOTES][LN] \n" + ex.Message);
    //                                                Helper.WriteLogXml("Error in [UT_NOTES] [LN] \n" + ex.Message);

    //                                            }
    //                                        }

    //                                    }
    //                                }
    //                                Console.WriteLine("Total Inserted [Long Notes] :------------------>" + TotalLongNotesRecord);
    //                            }
    //                            // -----------------------------------Processing for UT_REPOFIELDS table------------------------------------------------

    //                            if (dataSet.Tables.Contains("RepoFields"))
    //                            {
    //                                DataRow[] drRepoFields;
    //                                DataTable DtRepoFields = dataSet.Tables["RepoFields"].Clone();
    //                                drRepoFields = dataSet.Tables["RepoFields"].Select("TradeFeed_Id ='" + common_id + "'");
    //                                if (drRepoFields.Length > 0)
    //                                {
    //                                    //drRepoFields = dataSet.Tables["RepoFields"].Select("TradeFeed_Id ='688'");
    //                                    Console.WriteLine("drRepoFields count :-----> " + drRepoFields.Count());
    //                                    if (drRepoFields.Count() > 0)
    //                                    {
    //                                        for (int k = 0; k < drRepoFields.Count(); k++)
    //                                        {
    //                                            DtRepoFields.Rows.Add(drRepoFields[k].ItemArray);
    //                                        }

    //                                        Console.WriteLine("DtRepoFields table count :-----> " + DtRepoFields.Rows.Count);

    //                                        DataColumnCollection RepoColumns = DtRepoFields.Columns;

    //                                        int TotalRepoRecord = 0;
    //                                        foreach (DataRow RepoRow in DtRepoFields.Rows)
    //                                        {

    //                                            RepoFieldsModel RFM = new RepoFieldsModel();

    //                                            if (RepoColumns.Contains("HAIRCUT"))
    //                                                RFM.Haircut = string.IsNullOrEmpty(RepoRow["HAIRCUT"].ToString()) ? 0 : Convert.ToDecimal(RepoRow["HAIRCUT"]);

    //                                            if (RepoColumns.Contains("TERMINATIONDAYS"))
    //                                                RFM.TerminationDays = string.IsNullOrEmpty(RepoRow["TERMINATIONDAYS"].ToString()) ? 0 : Convert.ToInt32(RepoRow["TERMINATIONDAYS"]);

    //                                            if (RepoColumns.Contains("TRANSACTIONTYPE"))
    //                                                RFM.TransactionType = string.IsNullOrEmpty(RepoRow["TRANSACTIONTYPE"].ToString()) ? "" : RepoRow["TRANSACTIONTYPE"].ToString();

    //                                            if (RepoColumns.Contains("DELIVERYTYPE"))
    //                                                RFM.DeliveryType = string.IsNullOrEmpty(RepoRow["DELIVERYTYPE"].ToString()) ? "" : RepoRow["DELIVERYTYPE"].ToString();

    //                                            if (RepoColumns.Contains("DAYCOUNT"))
    //                                                RFM.DayCount = string.IsNullOrEmpty(RepoRow["DAYCOUNT"].ToString()) ? 0 : Convert.ToInt32(RepoRow["DAYCOUNT"]);

    //                                            if (RepoColumns.Contains("ALLINPRICE"))
    //                                                RFM.AllInPrice = string.IsNullOrEmpty(RepoRow["ALLINPRICE"].ToString()) ? 0 : Convert.ToDecimal(RepoRow["ALLINPRICE"]);

    //                                            if (RepoColumns.Contains("FORWARDPRICE"))
    //                                                RFM.ForwardPrice = string.IsNullOrEmpty(RepoRow["FORWARDPRICE"].ToString()) ? 0 : Convert.ToDecimal(RepoRow["FORWARDPRICE"]);

    //                                            if (RepoColumns.Contains("MARGIN"))
    //                                                RFM.Margin = string.IsNullOrEmpty(RepoRow["MARGIN"].ToString()) ? 0 : Convert.ToDecimal(RepoRow["MARGIN"]);

    //                                            if (RepoColumns.Contains("CUMULATIVEACCRUEDINTEREST"))
    //                                                RFM.CumulativeAccruedInterest = string.IsNullOrEmpty(RepoRow["CUMULATIVEACCRUEDINTEREST"].ToString()) ? 0 : Convert.ToDecimal(RepoRow["CUMULATIVEACCRUEDINTEREST"]);

    //                                            if (RepoColumns.Contains("TERMINATIONDATE"))
    //                                                RFM.TerminationDate = string.IsNullOrEmpty(RepoRow["TERMINATIONDATE"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("01-01-1900")) : Convert.ToDateTime(RepoRow["TERMINATIONDATE"]);

    //                                            if (RepoColumns.Contains("TERMINATIONMONEY"))
    //                                                RFM.TerminationMoney = string.IsNullOrEmpty(RepoRow["TERMINATIONMONEY"].ToString()) ? 0 : Convert.ToDecimal(RepoRow["TERMINATIONMONEY"]);

    //                                            if (RepoColumns.Contains("TERMPRICE"))
    //                                                RFM.TermPrice = string.IsNullOrEmpty(RepoRow["TERMPRICE"].ToString()) ? 0 : Convert.ToDecimal(RepoRow["TERMPRICE"]);

    //                                            if (RepoColumns.Contains("HAIRCUTOPTION"))
    //                                                RFM.HaircutOption = string.IsNullOrEmpty(RepoRow["HAIRCUTOPTION"].ToString()) ? 0 : Convert.ToInt32(RepoRow["HAIRCUTOPTION"]);

    //                                            if (RepoColumns.Contains("ADJTERMINATIONMONEY"))
    //                                                RFM.AdjTerminationMoney = string.IsNullOrEmpty(RepoRow["ADJTERMINATIONMONEY"].ToString()) ? 0 : Convert.ToDecimal(RepoRow["ADJTERMINATIONMONEY"]);

    //                                            if (RepoColumns.Contains("REPOREPRICEORIGINALTICKET"))
    //                                                RFM.RepoRepriceOriginalTicket = string.IsNullOrEmpty(RepoRow["REPOREPRICEORIGINALTICKET"].ToString()) ? 0 : Convert.ToInt32(RepoRow["REPOREPRICEORIGINALTICKET"]);

    //                                            if (RepoColumns.Contains("CARRYOVERREPOTRADEFLAG"))
    //                                            {
    //                                                bool flg = string.IsNullOrEmpty(RepoRow["CARRYOVERREPOTRADEFLAG"].ToString()) ? false : Convert.ToBoolean(RepoRow["CARRYOVERREPOTRADEFLAG"]);

    //                                                if (flg)
    //                                                {
    //                                                    RFM.CarryoverRepoTradeFlag = "1";
    //                                                }
    //                                                else
    //                                                {
    //                                                    RFM.CarryoverRepoTradeFlag = "0";
    //                                                }
    //                                            }

    //                                            //RFM.CarryoverRepoTradeFlag = string.IsNullOrEmpty(RepoRow["CARRYOVERREPOTRADEFLAG"].ToString()) ? "" : RepoRow["CARRYOVERREPOTRADEFLAG"].ToString();
    //                                            if (RepoColumns.Contains("EXTENDEDPRECISIONREPOPRICE"))
    //                                                RFM.ExtendedPrecisionRepoPrice = string.IsNullOrEmpty(RepoRow["EXTENDEDPRECISIONREPOPRICE"].ToString()) ? 0 : Convert.ToDecimal(RepoRow["EXTENDEDPRECISIONREPOPRICE"]);

    //                                            if (RepoColumns.Contains("EXTENDEDPRECISIONFORWARDPRICE"))
    //                                                RFM.ExtendedPrecisionForwardPrice = string.IsNullOrEmpty(RepoRow["EXTENDEDPRECISIONFORWARDPRICE"].ToString()) ? 0 : Convert.ToDecimal(RepoRow["EXTENDEDPRECISIONFORWARDPRICE"]);

    //                                            if (RepoColumns.Contains("EXTENDEDPRECISIONACCRUEDPRICE"))
    //                                                RFM.ExtendedPrecisionAccruedPrice = string.IsNullOrEmpty(RepoRow["EXTENDEDPRECISIONACCRUEDPRICE"].ToString()) ? 0 : Convert.ToDecimal(RepoRow["EXTENDEDPRECISIONACCRUEDPRICE"]);

    //                                            if (RepoColumns.Contains("TRADEFEED_ID"))
    //                                                RFM.TradeFeed_Id = string.IsNullOrEmpty(RepoRow["TRADEFEED_ID"].ToString()) ? 0 : Convert.ToInt32(RepoRow["TRADEFEED_ID"]);

    //                                            string strInsertRepoQuery = @"INSERT INTO UT_REPOFIELDS(HAIRCUT,TERMINATIONDAYS,TRANSCATIONTYPE,DELIVERYTYPE,DAYCOUNT,ALLINPRICE,FORWARDPRICE,MARGIN,CUMULATIVEACCRUEDINTEREST,TERMINATIONDATE,TERMINATIONMONEY,TERMPRICE,HAIRCUTOPTION,ADJTERMINATIONMONEY,REPOREPRICEORIGINALTICKET,CARRYOVERREPOTRADEFLAG,EXTENDEDPRECISIONREPOPRICE,EXTENDEDPRECISIONFORWARDPRICE,EXTENDEDPRECISIONACCRUEDPRICE,TRADEFEED_ID,FILENAME,FILEDATE,FILENUMBER) 
    //                                                                                             VALUES(:pHAIRCUT,:pTERMINATIONDAYS,:pTRANSCATIONTYPE,:pDELIVERYTYPE,:pDAYCOUNT,:pALLINPRICE,:pFORWARDPRICE,:pMARGIN,:pCUMULATIVEACCRUEDINTEREST,:pTERMINATIONDATE,:pTERMINATIONMONEY,:pTERMPRICE,:pHAIRCUTOPTION,:pADJTERMINATIONMONEY,:pREPOREPRICEORIGINALTICKET,:pCARRYOVERREPOTRADEFLAG,:pEXTENDEDPRECISIONREPOPRICE,:pEXTENDEDPRECISIONFORWARDPRICE,:pEXTENDEDPRECISIONACCRUEDPRICE,:pTRADEFEED_ID,:pFILENAME,:pFILEDATE,:pFILENUMBER)";
    //                                            using (NpgsqlCommand oraCmdRepo = new NpgsqlCommand(strInsertRepoQuery, conn))
    //                                            {
    //                                                oraCmdRepo.Parameters.Add("pHAIRCUT", RFM.Haircut);
    //                                                oraCmdRepo.Parameters.Add("pTERMINATIONDAYS", RFM.TerminationDays);
    //                                                oraCmdRepo.Parameters.Add("pTRANSCATIONTYPE", RFM.TransactionType);
    //                                                oraCmdRepo.Parameters.Add("pDELIVERYTYPE", RFM.DeliveryType);
    //                                                oraCmdRepo.Parameters.Add("pDAYCOUNT", RFM.DayCount);
    //                                                oraCmdRepo.Parameters.Add("pALLINPRICE", RFM.AllInPrice);
    //                                                oraCmdRepo.Parameters.Add("pFORWARDPRICE", RFM.ForwardPrice);
    //                                                oraCmdRepo.Parameters.Add("pMARGIN", RFM.Margin);
    //                                                oraCmdRepo.Parameters.Add("pCUMULATIVEACCRUEDINTEREST", RFM.CumulativeAccruedInterest);
    //                                                oraCmdRepo.Parameters.Add("pTERMINATIONDATE", RFM.TerminationDate);
    //                                                oraCmdRepo.Parameters.Add("pTERMINATIONMONEY", RFM.TerminationMoney);
    //                                                oraCmdRepo.Parameters.Add("pTERMPRICE", RFM.TermPrice);
    //                                                oraCmdRepo.Parameters.Add("pHAIRCUTOPTION", RFM.HaircutOption);
    //                                                oraCmdRepo.Parameters.Add("pADJTERMINATIONMONEY", RFM.AdjTerminationMoney);
    //                                                oraCmdRepo.Parameters.Add("pREPOREPRICEORIGINALTICKET", RFM.RepoRepriceOriginalTicket);
    //                                                oraCmdRepo.Parameters.Add("pCARRYOVERREPOTRADEFLAG", RFM.CarryoverRepoTradeFlag);
    //                                                oraCmdRepo.Parameters.Add("pEXTENDEDPRECISIONREPOPRICE", RFM.ExtendedPrecisionRepoPrice);
    //                                                oraCmdRepo.Parameters.Add("pEXTENDEDPRECISIONFORWARDPRICE", RFM.ExtendedPrecisionForwardPrice);
    //                                                oraCmdRepo.Parameters.Add("pEXTENDEDPRECISIONACCRUEDPRICE", RFM.ExtendedPrecisionAccruedPrice);
    //                                                oraCmdRepo.Parameters.Add("pTRADEFEED_ID", common_id);
    //                                                oraCmdRepo.Parameters.Add("pFILENAME", comModl.FILENAME);
    //                                                oraCmdRepo.Parameters.Add("pFILEDATE", comModl.FILEDATE);
    //                                                oraCmdRepo.Parameters.Add("pFILENUMBER", comModl.FILENUMBER);
    //                                                try
    //                                                {
    //                                                    oraCmdRepo.ExecuteNonQuery();
    //                                                    TotalRepoRecord++;
    //                                                    RepoFieldInsert++;
    //                                                }
    //                                                catch (Exception ex)
    //                                                {
    //                                                    Console.WriteLine("error executeNonQuery in [UT_REPOFIELDS] \n" + ex.Message);
    //                                                    Helper.WriteLogXml("Error in [UT_REPOFIELDS]  \n" + ex.Message);

    //                                                }
    //                                            }
    //                                        }

    //                                        Console.WriteLine("Total Inserted [UT_REPOFIELDS] :-------------> " + TotalRepoRecord);
    //                                    }
    //                                }
    //                            }
    //                            // -----------------------------------Processing for UT_TRANSACTIONCOST table------------------------------------------------

    //                            if (dataSet.Tables.Contains("TransactionCosts"))
    //                            {
    //                                int TotalTransactionCostRecord = 0;
    //                                DataRow[] transCosts = dataSet.Tables["TransactionCosts"].Select("TradeFeed_Id = '" + common_id + "'");
    //                                if (transCosts.Length > 0)
    //                                {
    //                                    int TransactionCosts_id = Convert.ToInt32(transCosts[0].ItemArray[0]);
    //                                    DataRow[] aTransactionCostRow = dataSet.Tables["TransactionCost"].Select("TRANSACTIONCOSTS_ID ='" + TransactionCosts_id + "'");

    //                                    DataColumnCollection TranscostColumns = dataSet.Tables["TransactionCost"].Columns;
    //                                    foreach (DataRow drTransactionCost in aTransactionCostRow)
    //                                    {
    //                                        TrasactionCostModel TCM = new TrasactionCostModel();
    //                                        if (TranscostColumns.Contains("Type"))
    //                                            TCM.Type = string.IsNullOrEmpty(drTransactionCost["Type"].ToString()) ? 0 : Convert.ToInt32(drTransactionCost["Type"]);

    //                                        if (TranscostColumns.Contains("Currency"))
    //                                            TCM.Currency = string.IsNullOrEmpty(drTransactionCost["Currency"].ToString()) ? "" : drTransactionCost["Currency"].ToString();

    //                                        if (TranscostColumns.Contains("Cost"))
    //                                            TCM.Cost = string.IsNullOrEmpty(drTransactionCost["Cost"].ToString()) ? 0 : Convert.ToDecimal(drTransactionCost["Cost"]);

    //                                        if (TranscostColumns.Contains("EffectOnFinalMoney"))
    //                                            TCM.EffectOnFinalMoney = string.IsNullOrEmpty(drTransactionCost["EffectOnFinalMoney"].ToString()) ? "" : drTransactionCost["EffectOnFinalMoney"].ToString();

    //                                        if (TranscostColumns.Contains("Override"))
    //                                        {
    //                                            bool flg2 = string.IsNullOrEmpty(drTransactionCost["Override"].ToString()) ? false : Convert.ToBoolean(drTransactionCost["Override"]);
    //                                            if (flg2)
    //                                            {
    //                                                TCM.Override = "1";
    //                                            }
    //                                            else
    //                                            {
    //                                                TCM.Override = "0";
    //                                            }
    //                                        }

    //                                        if (TranscostColumns.Contains("CurrencyRate"))
    //                                            TCM.CurrencyRate = string.IsNullOrEmpty(drTransactionCost["CurrencyRate"].ToString()) ? 0 : Convert.ToDecimal(drTransactionCost["CurrencyRate"]);

    //                                        if (TranscostColumns.Contains("TransactionCosts_Id"))
    //                                            TCM.TransactionCosts_Id = string.IsNullOrEmpty(drTransactionCost["TransactionCosts_Id"].ToString()) ? 0 : Convert.ToInt32(drTransactionCost["TransactionCosts_Id"]);


    //                                        string strInsertTransCostQuery = @"INSERT INTO UT_TRANSACTIONCOST(STYPE,CURRENCY,COST,EFFECTONFINALMONEY,OVERRIDE,CURRENCYRATE,TRANSACTIONCOSTS_ID,TRADEFEED_ID,FILENAME,FILEDATE,FILENUMBER) 
    //                                                                                             VALUES(:pSTYPE,:pCURRENCY,:pCOST,:pEFFECTONFINALMONEY,:pOVERRIDE,:pCURRENCYRATE,:pTRANSACTIONCOSTS_ID,:pTRADEFEED_ID,:pFILENAME,:pFILEDATE,:pFILENUMBER)";
    //                                        //read and insert 
    //                                        using (NpgsqlCommand oraCmdTransCost = new NpgsqlCommand(strInsertTransCostQuery, conn))
    //                                        {
    //                                            oraCmdTransCost.Parameters.Add(":pSTYPE", TCM.Type);
    //                                            oraCmdTransCost.Parameters.Add(":pCURRENCY", TCM.Currency);
    //                                            oraCmdTransCost.Parameters.Add(":pCOST", TCM.Cost);
    //                                            oraCmdTransCost.Parameters.Add(":pEFFECTONFINALMONEY", TCM.EffectOnFinalMoney);
    //                                            oraCmdTransCost.Parameters.Add(":pOVERRIDE", TCM.Override);
    //                                            oraCmdTransCost.Parameters.Add(":pCURRENCYRATE", TCM.CurrencyRate);
    //                                            oraCmdTransCost.Parameters.Add(":pTRANSACTIONCOSTS_ID", TCM.TransactionCosts_Id);
    //                                            oraCmdTransCost.Parameters.Add(":pTRADEFEED_ID", common_id);
    //                                            oraCmdTransCost.Parameters.Add(":pFILENAME", comModl.FILENAME);
    //                                            oraCmdTransCost.Parameters.Add(":pFILEDATE", comModl.FILEDATE);
    //                                            oraCmdTransCost.Parameters.Add(":pFILENUMBER", comModl.FILENUMBER);

    //                                            try
    //                                            {
    //                                                oraCmdTransCost.ExecuteNonQuery();
    //                                                TotalTransactionCostRecord++;
    //                                                TransCostInsert++;
    //                                                // Console.WriteLine("Record inserted in [UT_DES_FIELDS] --- >[ UT_DES_FIELDS ]");
    //                                            }
    //                                            catch (Exception ex)
    //                                            {
    //                                                Console.WriteLine("Insert error in table [UT_TRANSACTIONCOST]  \n" + ex.Message);
    //                                                Helper.WriteLogXml("Insert error in table [UT_TRANSACTIONCOST]  \n" + ex.Message);

    //                                            }
    //                                        }
    //                                    }
    //                                }
    //                                Console.WriteLine("Total Inserted [UT_TRANSACTIONCOST] :------------------>" + TotalTransactionCostRecord);
    //                            }
    //                            // -----------------------------------Processing for UT_DES_FIELDS table------------------------------------------------

    //                            //int Tradefeed_id = 0;
    //                            if (dataSet.Tables.Contains("ExtendedFields"))
    //                            {
    //                                int TotalFieldRecord = 0;
    //                                DataRow[] result = dataSet.Tables["ExtendedFields"].Select("TradeFeed_Id = '" + common_id + "'");
    //                                if (result.Length > 0)
    //                                {
    //                                    int ExtendedFields_id = Convert.ToInt32(result[0].ItemArray[0]);
    //                                    DataRow[] aFieldRow = dataSet.Tables["Field"].Select("ExtendedFields_id ='" + ExtendedFields_id + "'");

    //                                    DataColumnCollection FeildColumns = dataSet.Tables["Field"].Columns;

    //                                    foreach (DataRow drFields in aFieldRow)
    //                                    {
    //                                        if (!string.IsNullOrEmpty(drFields["FIELD_TEXT"].ToString()))
    //                                        {


    //                                            DESFieldModel DFM = new DESFieldModel();
    //                                            if (FeildColumns.Contains("CALCRT"))
    //                                                DFM.CALCRT = string.IsNullOrEmpty(drFields["CALCRT"].ToString()) ? "" : drFields["CALCRT"].ToString();

    //                                            if (FeildColumns.Contains("NAME"))
    //                                                DFM.NAME = string.IsNullOrEmpty(drFields["NAME"].ToString()) ? "" : drFields["NAME"].ToString();

    //                                            if (FeildColumns.Contains("FIELD_TEXT"))
    //                                                DFM.FIELD_TEXT = string.IsNullOrEmpty(drFields["FIELD_TEXT"].ToString()) ? "" : drFields["FIELD_TEXT"].ToString();

    //                                            DFM.EXTENDEDFIELDS_ID = ExtendedFields_id;
    //                                            DFM.TRADEFEED_ID = common_id;


    //                                            string strInsertFieldQuery = @"INSERT INTO UT_DES_FIELDS(CALCRT,NAME,FIELD_TEXT,EXTENDEDFIELDS_ID,TRADEFEED_ID,FILENAME,FILEDATE,FILENUMBER) 
    //                                                                                             VALUES(:pCALCRT,:pNAME,:pFIELD_TEXT,:pEXTENDEDFIELDS_ID,:pTRADEFEED_ID,:pFILENAME,:pFILEDATE,:pFILENUMBER)";
    //                                            //read and insert 
    //                                            using (NpgsqlCommand oraCmdFields = new NpgsqlCommand(strInsertFieldQuery, conn))
    //                                            {
    //                                                oraCmdFields.Parameters.Add("pCALCRT", DFM.CALCRT);
    //                                                oraCmdFields.Parameters.Add("pNAME", DFM.NAME);
    //                                                oraCmdFields.Parameters.Add("pFIELD_TEXT", DFM.FIELD_TEXT);
    //                                                oraCmdFields.Parameters.Add("pEXTENDEDFIELDS_ID", ExtendedFields_id);
    //                                                oraCmdFields.Parameters.Add("pTRADEFEED_ID", DFM.TRADEFEED_ID);

    //                                                oraCmdFields.Parameters.Add("pFILENAME", comModl.FILENAME);
    //                                                oraCmdFields.Parameters.Add("pFILEDATE", comModl.FILEDATE);
    //                                                oraCmdFields.Parameters.Add("pFILENUMBER", comModl.FILENUMBER);
    //                                                try
    //                                                {
    //                                                    oraCmdFields.ExecuteNonQuery();
    //                                                    TotalFieldRecord++;
    //                                                    Des_FieldInsert++;
    //                                                    // Helper.WriteLogXml("Record inserted in [UT_DES_FIELDS] --- >[ UT_DES_FIELDS ]");
    //                                                    //Console.WriteLine("Record inserted in [UT_DES_FIELDS] --- >[ UT_DES_FIELDS ]");
    //                                                }
    //                                                catch (Exception ex)
    //                                                {
    //                                                    Console.WriteLine("Insert error in table [UT_DES_FIELDS]  \n" + ex.Message);
    //                                                    Helper.WriteLogXml("Insert error in table [UT_DES_FIELDS]  \n" + ex.Message);
    //                                                }
    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            TotalFieldRecord++;
    //                                            Des_FieldInsert++;
    //                                        }
    //                                    }
    //                                }
    //                                Console.WriteLine("Total Insert Count [UT_DES_FIELDS]---------->" + TotalFieldRecord);
    //                            }
    //                        }
    //                    }
    //                    catch (Exception exx)
    //                    {
    //                        Console.WriteLine("error : \n" + exx.Message);
    //                        Helper.WriteLogXml("Error in last catch  \n" + exx.Message);

    //                    }
    //                }

    //            }
    //            int Ut_notes = dataSet.Tables["ShortNote"].Rows.Count + dataSet.Tables["LongNote"].Rows.Count;

    //            Helper.WriteLogXml("------------------------------Inserting Process Start for the file  [" + filname + "]-------------------------------");
    //            Helper.WriteLogXml("Common_Feeds ------------>Actual Count ====[" + dataSet.Tables["TradeFeed"].Rows.Count + "] ======> Inserted Count  :" + TradeFeedInsert);
    //            Helper.WriteLogXml("UT_Notes ----------------->Actual Count ====[" + Ut_notes + "] ===> Inserted Count :" + Ut_NotesInsert);
    //            Helper.WriteLogXml("UT_RepoFileds ------------>Actual Count ====[" + dataSet.Tables["TransactionCost"].Rows.Count + "] ===. Inserted Count : " + TransCostInsert);
    //            Helper.WriteLogXml("UT_TransCost--------------->Actual Count ====[" + dataSet.Tables["RepoFields"].Rows.Count + "] ===. Inserted Count : " + RepoFieldInsert);
    //            Helper.WriteLogXml("UT_DES_Field-------------- >Actual Count ====[" + dataSet.Tables["Field"].Rows.Count + "] ===. Inserted Count :" + Des_FieldInsert);
    //            Helper.WriteLogXml("-----------------------------------Process End of file  [" + xmlFilNm + "]-------------------------------");
    //        }
    //        else
    //        {
    //            Helper.WriteLogXml("Record already present in database");
    //        }
    //        // Below line Remove after testing done
    //        IsAlreadyPresent = false;
    //        if (IsAlreadyPresent)
    //        {
    //            try
    //            {
    //                string targetFile = Path.Combine(@"\\10.11.30.158\dofa-shared\bbg\ToFADb\backup\", Path.GetFileName(xmlFilNm));
    //                if (File.Exists(targetFile))
    //                {
    //                    File.Delete(targetFile);
    //                }
    //                File.Copy(xmlFilNm, targetFile);
    //                File.Delete(xmlFilNm);
    //            }
    //            catch (Exception exx)
    //            {
    //                Helper.WriteLog("File move failed : " + exx.Message, "E");
    //            }
    //        }
    //    }

    //    public void GetxmlFiles()
    //    {
    //        string[] filesInFolder = Directory.GetFiles(@"\\10.11.30.158\dofa-shared\bbg\ToFADb\");
    //        //string[] filesInFolder = Directory.GetFiles(@"D:\UTI PROJ\Development\Bloomberg Utilities\ReqFile");
    //        Console.WriteLine("File count : \n" + filesInFolder.Count());
    //        foreach (string filePath in filesInFolder)
    //        {
    //            try
    //            {
    //                ProcessingXml(filePath);
    //            }
    //            catch (Exception excheck)
    //            {
    //                Console.WriteLine("Failed inside function [GetxmlFiles] \n" + excheck.Message);
    //            }
    //        }
    //    }
    //}
}

