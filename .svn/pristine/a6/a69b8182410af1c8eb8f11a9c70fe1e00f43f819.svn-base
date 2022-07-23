using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatementDownloadUtility
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("HDFC download Started");
            HDFCBank hdfc = new HDFCBank();
            hdfc.DownloadStatement();
            Console.WriteLine("HDFC download Completed");
            //ICICIBank icici = new ICICIBank();
            //icici.DownloadStatement();

            //SBIBank sbi = new SBIBank();
            //sbi.downloadFile();

            //IDBIBank idbi = new IDBIBank();
            //idbi.DownloadStatement();

            Console.WriteLine("AXIS download started");
            AxisBank axis = new AxisBank();
            axis.ProcessEmails();
            Console.WriteLine("AXIS download completed");
        }
    }
}
