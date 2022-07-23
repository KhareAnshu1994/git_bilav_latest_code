using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexWeightageToSFPT_Utility
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Utility Started");
            ProcessToUpload bll = new ProcessToUpload();
            bll.ActivityStart();
            Console.WriteLine("Utility Completed");
        }
    }
}
