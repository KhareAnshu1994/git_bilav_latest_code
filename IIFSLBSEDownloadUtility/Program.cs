using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIFSLBSEDownloadUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("BSE Download Start");
            BSE_Utility BSE = new BSE_Utility();
            BSE.IIFSLBSEUtility();
            Console.WriteLine("BSE Download End");
        }
    }
}
