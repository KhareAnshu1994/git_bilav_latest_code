using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlmbergCsvToDbUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            Process[] pname = Process.GetProcessesByName("BlmbergCsvToDbUtility");
            Helper.WriteLog("process count :" + pname.Length, "E");
            Console.WriteLine("Utility Started");
            if (pname.Length == 1)
            {
                if (Convert.ToInt32(ConfigurationManager.AppSettings["IsCsvRun"]) == 1)
                {
                    FileProcessing FP = new FileProcessing();
                    FP.CsvOperation();
                }
            }
            Console.WriteLine("Utility Completed");
        }
    }
}
