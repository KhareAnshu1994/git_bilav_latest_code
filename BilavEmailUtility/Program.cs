using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilavEmailUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            EmailReading ER = new EmailReading();
            //ER.getDataTable("SELECT FromEmailID,EmailReceivedDate FROM EmailAudit WHERE TO_CHAR(CreatedDate,'YYYY-MON-DD')>= (SELECT to_CHAR(sysdate, 'YYYY-MM-DD') FROM DUAL)");
            ER.ProcessEmails();
        }
    }
}
