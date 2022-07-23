using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIFSLNSEEmailUtility
{
    class IIFSLEmail
    {
        public string FileName { get; set; }
        public string AllTimestampedFiles { get; set; }
        public DateTime EmailReceivedDateTime { get; set; }
        public string FileExtension { get; set; }
        public string EmailReceivedFrom { get; set; }
        public List<string> EmailReceiveFromCC { get; set; }
        public string TimeStampNumber { get; set; }
        public List<string> ClientEmailIds { get; set; }
        public List<string> CCEmailIds { get; set; }
        public List<string> ClientCellnumbers { get; set; }
        public string RMEmailId { get; set; }
        public string RMMobileNumber { get; set; }
        public string FileNameWithExtension { get; set; }
        public bool IsArchived { get; set; }
        public int PageCount { get; set; }
        public string TimeStamp { get; set; }
        public string ClientId { get; set; }
        public string CreateTime { get; set; }
        public string InvestorName { get; set; }
        public string ArchivedFilePath { get; set; }
        public string ArchivedFolder { get; set; }
        public string DistributorId { get; set; }
        public string DistributorName { get; set; }
        public List<string> AttachmentNames { get; set; }
        public string AttachmentDownloadDirectory { get; set; }
        public string TimestampDirectory { get; set; }
        public string Remark { get; set; }
        public bool IsAttachmentDownloaded { get; set; }
        public EmailStatus Status { get; set; }
       
        public string GroupInvestorName { get; set; }
        public string SMSInvestorName { get; set; }
        public string StampId { get; set; }
        public string Action { get; set; }
        public int DownloadedFilesCount { get; set; }
    }
}
