using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class CaseTransferBatch
    {
        public int ID { get; set; }
        public string CompanyID { get; set; }
        public string LawFirmID { get; set; }
        public string BatchID { get; set; }
        public Nullable<DateTime> TransferDate { get; set; }
        public bool IsEmailSent { get; set; }
        public string UserID { get; set; }
        public int BillingUserID { get; set; }
        public int UpdatedByUserID { get; set; }
        public bool IsBatchDownloaded { get; set; }
        public Nullable<DateTime> BatchDownloadDate { get; set; }
    }
}

