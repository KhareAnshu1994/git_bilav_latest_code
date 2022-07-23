using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Index_Download.classes
{
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
        public static CustomError ProcessBse100Index(string dir_path)
        {
            CustomError custom_error = new CustomError();
            try
            {
                string con = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dir_path + "/BSE_100_index.xls;" + @"Extended Properties='Excel 8.0;HDR=Yes;'";

                using (OleDbConnection connection = new OleDbConnection(con))
                {
                    connection.Open();
                    System.Data.DataTable dbSchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, null);

                    var firstSheetName = dbSchema.Rows[0]["TABLE_NAME"];

                    OleDbCommand command = new OleDbCommand("select * from [" + firstSheetName + "]", connection);


                    List<asiaindex> BSE100Index = new List<asiaindex>();
                    using (OleDbDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (dr[0].ToString().Trim() != string.Empty)
                            {
                                asiaindex index = new asiaindex();
                                index.date = commonHelper.GetFormatedDate(dr[0].ToString().Trim());
                                index.currentValue = dr[1].ToString().Trim();
                                BSE100Index.Add(index);
                            }

                        }
                        if (BSE100Index.Count() > 0)
                        {
                            commonHelper helper = new commonHelper();
                            helper.currentName = "SP Indices BSE 100 Index";
                            helper.date = BSE100Index.LastOrDefault().date;
                            helper.currentValue = BSE100Index.LastOrDefault().currentValue;
                            //custom_error = commonHelper.saveDataToDB(helper);
                            commonHelper.saveData(helper);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("exception occured while reading excel : " + ex.Message);

                custom_error.status = "fail";
                custom_error.error_msg = ex.Message;
                custom_error.is_success = false;
            }
            return custom_error;
        }
        public static CustomError ProcessBse200Index(string dir_path)
        {
            CustomError custom_error = new CustomError();
            try
            {
                string con = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dir_path + "/BSE_200_index.xls;" + @"Extended Properties='Excel 8.0;HDR=Yes;'";
                using (OleDbConnection connection = new OleDbConnection(con))
                {
                    connection.Open();
                    System.Data.DataTable dbSchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, null);

                    var firstSheetName = dbSchema.Rows[0]["TABLE_NAME"];

                    OleDbCommand command = new OleDbCommand("select * from [" + firstSheetName + "]", connection);

                    List<asiaindex> BSE200Index = new List<asiaindex>();

                    using (OleDbDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (dr[0].ToString().Trim() != string.Empty)
                            {

                                // var date = DateTime.ParseExact(dr[0].ToString().Trim(), "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);

                                asiaindex index = new asiaindex();

                                index.date = commonHelper.GetFormatedDate(dr[0].ToString().Trim()); ;
                                index.currentValue = dr[1].ToString().Trim();
                                BSE200Index.Add(index);


                            }

                        }

                        if (BSE200Index.Count() > 0)
                        {
                            commonHelper helper = new commonHelper();
                            helper.currentName = "SP Indices BSE 200 Index";
                            helper.date = BSE200Index.LastOrDefault().date;
                            helper.currentValue = BSE200Index.LastOrDefault().currentValue;
                            //custom_error = commonHelper.saveDataToDB(helper);
                            commonHelper.saveData(helper);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("exception occured while reading excel : " + ex.Message);
                custom_error.status = "fail";
                custom_error.error_msg = ex.Message;
                custom_error.is_success = false;

            }
            return custom_error;
        }
        public static CustomError ProcessBseSensexNext50(string dir_path)
        {
            CustomError custom_error = new CustomError();
            try
            {
                string con = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dir_path + "/BSE_Next_50_index.xls;" + @"Extended Properties='Excel 8.0;HDR=Yes;'";
                using (OleDbConnection connection = new OleDbConnection(con))
                {
                    connection.Open();
                    System.Data.DataTable dbSchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, null);

                    var firstSheetName = dbSchema.Rows[0]["TABLE_NAME"];

                    OleDbCommand command = new OleDbCommand("select * from [" + firstSheetName + "]", connection);

                    List<asiaindex> BseSensexNext50Index = new List<asiaindex>();

                    using (OleDbDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (dr[0].ToString().Trim() != string.Empty)
                            {

                                // var date = DateTime.ParseExact(dr[0].ToString().Trim(), "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);

                                asiaindex index = new asiaindex();

                                index.date = commonHelper.GetFormatedDate(dr[0].ToString().Trim()); ;
                                index.currentValue = dr[1].ToString().Trim();
                                BseSensexNext50Index.Add(index);


                            }

                        }

                        if (BseSensexNext50Index.Count() > 0)
                        {
                            commonHelper helper = new commonHelper();
                            //helper.currentName = "BSE Next 50 Index";
                            helper.currentName = "SENSEX NEXT 50";
                            helper.date = BseSensexNext50Index.LastOrDefault().date;
                            helper.currentValue = BseSensexNext50Index.LastOrDefault().currentValue;
                            //  custom_error = commonHelper.saveDataToDB(helper);
                            commonHelper.saveData(helper);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("exception occured while reading excel : " + ex.Message);
                custom_error.status = "fail";
                custom_error.error_msg = ex.Message;
                custom_error.is_success = false;

            }
            return custom_error;
        }
    }
}
