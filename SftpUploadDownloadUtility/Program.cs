using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SftpUploadDownloadUtility
{
    public class Program
    {
        readonly static bool Is_Upload = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("Is_Upload"));
        readonly static bool Is_Download = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("Is_Download"));
        static void Main(string[] args)
        {
            if (Is_Upload)
            {
                ProcessToUpload ptu = new ProcessToUpload();
                ptu.ActivityStart();
            }
            if (Is_Download)
            {
                Console.WriteLine("Sftp download started");
                ProcessToDownload ptd = new ProcessToDownload();
                ptd.ActivityStart();
                Console.WriteLine("Sftp download completed");
            }

        }
    }
}
