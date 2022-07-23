using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIFSLNSEEmailUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            FileReader FR = new FileReader();
            FR.FileReadingStart();

            //Helper h = new Helper();
            //h.getDataTable("SELECT * FROM EmailAudit WHERE TO_CHAR(CreatedDate,'YYYY-MON-DD') >= (SELECT to_CHAR(sysdate, 'YYYY-MM-DD') FROM DUAL)");
        }
    }
}
