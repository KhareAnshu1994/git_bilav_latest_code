using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace StatementReaderUtility
{
    class Program
    {
        static void Main(string[] args)
        {

            foreach (Process p in Process.GetProcesses().Where(p => p.ProcessName.Contains(System.AppDomain.CurrentDomain.FriendlyName)))
            {
                try
                {
                    if (p.StartTime.Minute < DateTime.Now.Minute)
                        p.Kill();
                }
                catch (Exception e)
                {
                }
            }

            ReadFiles();
        }

        public static void ReadFiles()
        {
            Common common = new Common();
            string FileSourcePath = ConfigurationManager.AppSettings["FileSourcePath"].ToString();
            string BankName = string.Empty;
            string TransDate = string.Empty;
            string ValueDate = string.Empty;
            string Reference = string.Empty;
            string Narration = string.Empty;
            string DebitAmt = string.Empty;
            string CreditAmt = string.Empty;
            string AccountNo = string.Empty;
            string BankCode = string.Empty;
            string DRCR = string.Empty;

            if (Directory.Exists(FileSourcePath))
            {
                FileInfo[] DownloadedFiles = new DirectoryInfo(FileSourcePath).GetFiles();
                FileReader Reader = new FileReader();

                for (int i = 0; i < DownloadedFiles.Length; i++)
                {
                    if (DownloadedFiles[i].Name.ToUpper().Contains("HDFC"))
                    {
                        DataTable dtReadCSV = Reader.ReadCSV(DownloadedFiles[i].FullName, "|", 1);
                        if (dtReadCSV.Columns.Count < 5)
                        {
                            File.Delete(DownloadedFiles[i].FullName);
                            continue;
                        }
                        for (int j = 0; j < dtReadCSV.Rows.Count; j++)
                        {
                            Reference = "";
                            string val_trans_date = dtReadCSV.Rows[j]["Transaction+Date"].ToString();
                            DateTime trans_date = DateTime.ParseExact(val_trans_date, "dd/MM/yy HH:mm:ss", CultureInfo.InvariantCulture);
                            TransDate = trans_date.ToString("MM/dd/yyyy HH:mm:ss");

                            string val_value_date = dtReadCSV.Rows[j]["Value+Date"].ToString();
                            DateTime value_Date = DateTime.ParseExact(val_value_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            ValueDate = value_Date.ToString("dd/MM/yyyy HH:mm:ss");

                            Narration = dtReadCSV.Rows[j]["Transaction+Description"].ToString();
                            DRCR = dtReadCSV.Rows[j]["Debit+/+Credit"].ToString();
                            if (DRCR == "D")
                            {
                                DebitAmt = dtReadCSV.Rows[j]["Transaction+Amount"].ToString() == "" ? "0" : dtReadCSV.Rows[j]["Transaction+Amount"].ToString();
                                CreditAmt = "0";
                            }
                            else if (DRCR == "C")
                            {
                                CreditAmt = dtReadCSV.Rows[j]["Transaction+Amount"].ToString() == "" ? "0" : dtReadCSV.Rows[j]["Transaction+Amount"].ToString();
                                DebitAmt = "0";
                            }

                            AccountNo = DownloadedFiles[i].Name.Replace("HDFC_", "").Replace(DownloadedFiles[i].Extension, "");
                            common.InsertStatement("HDFC", TransDate, ValueDate, Reference, Narration, DebitAmt, CreditAmt, AccountNo);
                        }

                    }
                    else if (DownloadedFiles[i].Name.ToUpper().Contains("IDBI"))
                    {
                        DataTable dtReadExcel = Reader.ReadExcel(DownloadedFiles[i].FullName, "IDBI");

                        if (dtReadExcel != null)
                        {
                            for (int j = 0; j < dtReadExcel.Rows.Count; j++)
                            {
                                Reference = "";
                                TransDate = Convert.ToDateTime(dtReadExcel.Rows[j]["Txn Posted Date"].ToString()).ToString("MM/dd/yyyy") + " " + Convert.ToDateTime(dtReadExcel.Rows[j]["Txn Posted Time"].ToString()).ToString("HH:mm:ss");
                                ValueDate = Convert.ToDateTime(dtReadExcel.Rows[j]["Value Date"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                                Narration = dtReadExcel.Rows[j]["Description"].ToString();
                                DRCR = dtReadExcel.Rows[j]["Cr/Dr"].ToString();
                                if (DRCR == "DR")
                                {
                                    DebitAmt = dtReadExcel.Rows[j]["Txn Amount(INR)"].ToString().Trim() == "" ? "0" : dtReadExcel.Rows[j]["Txn Amount(INR)"].ToString();
                                    CreditAmt = "0";
                                }
                                else if (DRCR == "CR")
                                {
                                    CreditAmt = dtReadExcel.Rows[j]["Txn Amount(INR)"].ToString().Trim() == "" ? "0" : dtReadExcel.Rows[j]["Txn Amount(INR)"].ToString();
                                    DebitAmt = "0";
                                }
                                AccountNo = DownloadedFiles[i].Name.Replace("IDBI_", "").Replace(DownloadedFiles[i].Extension, "");
                                common.InsertStatement("IDBI", TransDate, ValueDate, Reference, Narration, DebitAmt, CreditAmt, AccountNo);
                            }
                        }

                    }
                    else if (DownloadedFiles[i].Name.ToUpper().Contains("SBI"))
                    {
                        DataTable dtReadExcel = Reader.ReadText(DownloadedFiles[i].FullName, "\t", 19);
                        if (dtReadExcel != null && dtReadExcel.Columns.Count < 5)
                        {
                            File.Delete(DownloadedFiles[i].FullName);
                            continue;
                        }
                        if (dtReadExcel != null)
                        {
                            for (int j = 0; j < dtReadExcel.Rows.Count; j++)
                            {
                                Reference = "";
                                TransDate = Convert.ToDateTime(dtReadExcel.Rows[j]["Txn Date"].ToString()).ToString("MM/dd/yyyy");
                                ValueDate = Convert.ToDateTime(dtReadExcel.Rows[j]["Value Date"].ToString()).ToString("dd/MM/yyyy");
                                Narration = dtReadExcel.Rows[j]["Description"].ToString();
                                DebitAmt = dtReadExcel.Rows[j]["        Debit"].ToString().Trim() == "" ? "0" : dtReadExcel.Rows[j]["        Debit"].ToString();
                                CreditAmt = dtReadExcel.Rows[j]["Credit"].ToString().Trim() == "" ? "0" : dtReadExcel.Rows[j]["Credit"].ToString();
                                AccountNo = DownloadedFiles[i].Name.Replace("SBI_", "").Replace(DownloadedFiles[i].Extension, "");
                                common.InsertStatement("SBI", TransDate, ValueDate, Reference, Narration, DebitAmt, CreditAmt, AccountNo);
                            }
                        }

                    }
                    else if (DownloadedFiles[i].Name.ToUpper().Contains("AXIS"))
                    {
                        DataTable dtReadCSV = Reader.ReadCSV(DownloadedFiles[i].FullName, ",", 1);
                        if (dtReadCSV.Columns.Count < 5)
                        {
                            File.Delete(DownloadedFiles[i].FullName);
                            continue;
                        }
                        string[] strAccountNo = DownloadedFiles[i].Name.Split('_');
                        if (strAccountNo.Length > 0)
                        {
                            AccountNo = strAccountNo[0].ToString();
                        }

                        for (int j = 0; j < dtReadCSV.Rows.Count; j++)
                        {
                            Reference = "";
                            TransDate = Convert.ToDateTime(dtReadCSV.Rows[j][" Tran. Date "].ToString()).ToString("MM/dd/yyyy") + " " + Convert.ToDateTime(dtReadCSV.Rows[j][" Tran. Time "].ToString()).ToString("HH:mm:ss");
                            ValueDate = Convert.ToDateTime(dtReadCSV.Rows[j][" Value Date "].ToString()).ToString("dd/MM/yyyy");
                            Narration = dtReadCSV.Rows[j][" Particulars "].ToString();
                            DebitAmt = dtReadCSV.Rows[j][" Debit Amt. "].ToString() == "" ? "0" : dtReadCSV.Rows[j][" Debit Amt. "].ToString();
                            CreditAmt = dtReadCSV.Rows[j][" Credit Amt. "].ToString() == "" ? "0" : dtReadCSV.Rows[j][" Credit Amt. "].ToString();
                            common.InsertStatement("Axis", TransDate, ValueDate, Reference, Narration, DebitAmt, CreditAmt, AccountNo);
                        }
                    }
                    else if (DownloadedFiles[i].Name.ToUpper().Contains("ICICI"))
                    {
                        DataTable dtReadExcel = Reader.ReadText(DownloadedFiles[i].FullName, "|", 7);
                        if (dtReadExcel == null || dtReadExcel.Columns.Count < 5)
                        {
                            File.Delete(DownloadedFiles[i].FullName);
                            continue;
                        }

                        if (dtReadExcel != null)
                        {
                            for (int j = 0; j < dtReadExcel.Rows.Count; j++)
                            {
                                if (!string.IsNullOrEmpty(dtReadExcel.Rows[j]["Txn Posted Date"].ToString()))
                                {
                                    Reference = "";
                                    TransDate = Convert.ToDateTime(dtReadExcel.Rows[j]["Txn Posted Date"].ToString()).ToString("MM/dd/yyyy HH:mm:ss");
                                    ValueDate = Convert.ToDateTime(dtReadExcel.Rows[j]["Value Date"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                                    Narration = dtReadExcel.Rows[j]["Description"].ToString();
                                    DRCR = dtReadExcel.Rows[j]["Cr/Dr"].ToString();


                                    if (DRCR == "DR")
                                    {
                                        DebitAmt = dtReadExcel.Rows[j]["Transaction Amount(INR)"].ToString().Trim() == "" ? "0" : dtReadExcel.Rows[j]["Transaction Amount(INR)"].ToString().Replace(",", "");
                                        CreditAmt = "0";
                                    }
                                    else if (DRCR == "CR")
                                    {
                                        CreditAmt = dtReadExcel.Rows[j]["Transaction Amount(INR)"].ToString().Trim() == "" ? "0" : dtReadExcel.Rows[j]["Transaction Amount(INR)"].ToString().Replace(",", "");
                                        DebitAmt = "0";
                                    }
                                    AccountNo = DownloadedFiles[i].Name.Replace("ICICI_", "").Replace(DownloadedFiles[i].Extension, "");
                                    common.InsertStatement("ICICI", TransDate, ValueDate, Reference, Narration, DebitAmt, CreditAmt, AccountNo);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
