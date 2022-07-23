using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Index_Download.classes;
using System.Collections.Specialized;
using System.Configuration;

namespace Index_Download
{

    class Program
    {
        static List<CustomError> custom_error_list = new List<CustomError>();
        static CustomError
            custom_error_fbil,
            custom_error_bse100,
            custom_error_bse200,
            custom_error_bseSensexNext50,
            custome_error_asix_healthcare,
            custome_error_asia_bse_sensex,
            custome_Volatility_Index,
            custom_error_lbmaGold,
            custom_error_lbmaSilver,
            custom_error_bseIndex,
            custom_error_bsehealthcare
            = new CustomError();
        static void Main(string[] args)
        {
            //AmfiPortal.ProcessAmfiPortalIndex();

            NameValueCollection error_setting = (NameValueCollection)ConfigurationManager.GetSection("customAppSettingsGroup/CommonAppSettings");

            string result_file_location = error_setting["store_json_path"] + "/Result_File/";
            string[] exist_files = Directory.GetFiles(result_file_location, "*.json");
            foreach (string fil_path in exist_files)
            {
                try
                {
                    File.Delete(fil_path);
                    Console.WriteLine("old result file deleted !");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("error while deleting old files !" + ex.Message);
                }
            }

            commonHelper comHelper = new commonHelper();

            var current_time = DateTime.UtcNow.AddMinutes(330).TimeOfDay;
            commonHelper.WriteLog("-----------------------Utility Started-------------------------", "S");
            var running_status = false;

            TimeSpan time_to_run;
            NameValueCollection FBIL_setting = (NameValueCollection)ConfigurationManager.GetSection("customAppSettingsGroup/FBILAppSettings");

            if (FBIL_setting.Count > 0)
            {
                time_to_run = Convert.ToDateTime(FBIL_setting["time"]).TimeOfDay;
                running_status = FBIL_setting["running_status"] == "on" ? true : false;


                if (running_status == true)
                {
                    if (current_time >= time_to_run)
                    {
                        custom_error_fbil = Fbil.getINRValue();
                    }
                }
            }

            //SP Indices BSE 100 & 200 Index

            NameValueCollection SPIndices_setting = (NameValueCollection)ConfigurationManager.GetSection("customAppSettingsGroup/SPIndicesAppSettings");

            if (SPIndices_setting.Count > 0)
            {
                time_to_run = Convert.ToDateTime(SPIndices_setting["time"]).TimeOfDay;
                running_status = SPIndices_setting["running_status"] == "on" ? true : false;


                if (running_status == true)
                {
                    if (current_time >= time_to_run)
                    {
                        custom_error_bse100 = Spindices.getBse100Index();
                        custom_error_bse200 = Spindices.getBse200Index();
                        custom_error_bseSensexNext50 = Spindices.getBseSensexNext50();
                    }
                }
            }


            //ASIA Index BSE Sensex & Health Care

            NameValueCollection ASIAIndex_setting = (NameValueCollection)ConfigurationManager.GetSection("customAppSettingsGroup/ASIAIndexAppSettings");

            if (ASIAIndex_setting.Count > 0)
            {
                time_to_run = Convert.ToDateTime(ASIAIndex_setting["time"]).TimeOfDay;
                running_status = ASIAIndex_setting["running_status"] == "on" ? true : false;


                if (running_status == true)
                {
                    if (current_time >= time_to_run)
                    {
                        custome_error_asix_healthcare = asiaindex.getHealthCareIndex();
                        custome_error_asia_bse_sensex = asiaindex.getBseSensexIndex();
                    }
                }
            }

            /* NameValueCollection ASIAIndex_setting1 = (NameValueCollection)ConfigurationManager.GetSection("customAppSettingsGroup/BseLowVolatilityAppSetting");

             if (ASIAIndex_setting.Count > 0)
             {
                 time_to_run = Convert.ToDateTime(ASIAIndex_setting1["time"]).TimeOfDay;
                 running_status = ASIAIndex_setting1["running_status"] == "on" ? true : false;


                 if (running_status == true)
                 {
                     if (current_time >= time_to_run)
                     {

                         custome_error_asia_bse_sensex = BSE_Low_Volatility_Index.getBSE_Low_Volatility_Index();
                     }
                 }
             }
 */

            NameValueCollection BSELowVolatility_setting = (NameValueCollection)ConfigurationManager.GetSection("customAppSettingsGroup/BseLowVolatilityAppSetting");

            if (BSELowVolatility_setting.Count > 0)
            {
                time_to_run = Convert.ToDateTime(BSELowVolatility_setting["time"]).TimeOfDay;
                running_status = BSELowVolatility_setting["running_status"] == "on" ? true : false;


                if (running_status == true)
                {
                    if (current_time >= time_to_run)
                    {
                        custome_Volatility_Index = BSE_Low_Volatility_Index.getBSE_Low_Volatility_Index();
                        //custome_error_asia_bse_sensex = asiaindex.getBseSensexIndex();
                    }
                }
            }



            //LBMA GOLD and SILVER

            NameValueCollection LBMA_setting = (NameValueCollection)ConfigurationManager.GetSection("customAppSettingsGroup/LBMAAppSettings");

            if (LBMA_setting.Count > 0)
            {
                time_to_run = Convert.ToDateTime(LBMA_setting["time"]).TimeOfDay;
                running_status = LBMA_setting["running_status"] == "on" ? true : false;


                if (running_status == true)
                {
                    TimeSpan time1 = TimeSpan.FromHours(1);
                    if (current_time >= time_to_run)
                    {
                        custom_error_lbmaSilver = Lbma.getGoldValueAsync();
                        custom_error_lbmaSilver = Lbma.getSilverValue();
                    }
                }
            }

            //Base India BSE Index

            NameValueCollection BaseIndia_setting = (NameValueCollection)ConfigurationManager.GetSection("customAppSettingsGroup/BaseIndiaAppSettings");

            if (BaseIndia_setting.Count > 0)
            {
                time_to_run = Convert.ToDateTime(BaseIndia_setting["time"]).TimeOfDay;
                running_status = BaseIndia_setting["running_status"] == "on" ? true : false;


                if (running_status == true)
                {
                    TimeSpan time1 = TimeSpan.FromHours(1);
                    if (current_time >= time_to_run)
                    {
                        custom_error_bseIndex = BSEINDEX.getBSEIndex();
                        custom_error_bsehealthcare = BSEINDEX.getBSEHealthCareIndex();
                    }
                }
            }

            // Amfi Portal index
            NameValueCollection AmfiPortal_setting = (NameValueCollection)ConfigurationManager.GetSection("customAppSettingsGroup/AmfiPortalSettings");

            if (AmfiPortal_setting.Count > 0)
            {
                time_to_run = Convert.ToDateTime(AmfiPortal_setting["time"]).TimeOfDay;
                running_status = AmfiPortal_setting["running_status"] == "on" ? true : false;


                if (running_status == true)
                {
                    if (current_time >= time_to_run)
                    {
                        custom_error_bseIndex = AmfiPortal.ProcessAmfiPortalIndex();
                    }
                }
            }
            //send error log

            NameValueCollection common_setting = (NameValueCollection)ConfigurationManager.GetSection("customAppSettingsGroup/CommonAppSettings");
            if (common_setting.Count > 0)
            {
                if (common_setting["send_error_log"] == "on")
                {
                    if (custom_error_fbil != null)
                    {
                        if (!custom_error_fbil.is_success)
                        {
                            Console.WriteLine("Fbil ERROR");
                        }
                    }
                    if (custom_error_bse100 != null)
                    {
                        if (!custom_error_bse100.is_success)
                        {
                            Console.WriteLine("bse100 ERROR");
                        }
                    }
                    if (custom_error_bse200 != null)
                    {
                        if (!custom_error_bse200.is_success)
                        {
                            Console.WriteLine("bse200 ERROR");
                        }
                    }

                }

            }

            // insert db process
            if (commonHelper.saveDataToDB())
            {
                Console.WriteLine("Uploaded successfully into the DB");
            }
            commonHelper.WriteLog("-----------------------Utility Completed-------------------------", "S");

        }
    }
}
