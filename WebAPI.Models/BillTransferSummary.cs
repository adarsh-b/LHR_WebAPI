using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class BillTransferSummary
    {
        public string LawFirmID { get; set; }
        public string LawFirmName { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public int TransferredBillCount { get; set; }
        public Decimal TransferredBillAmount { get; set; }
        public int AcknowlledgedBillCount { get; set; }
        public Decimal AcknowlledgedBillAmount { get; set; }
    }
}
