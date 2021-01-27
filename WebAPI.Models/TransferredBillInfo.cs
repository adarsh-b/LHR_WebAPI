using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class TransferredBillInfo
    {
        public string LawFirmID { get; set; }
        public string LawFirmName { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public string CaseNumber { get; set; }
        public string CaseID { get; set; }
        public string PatientName { get; set; }
        public string BillNumber { get; set; }
        public decimal BillAmount { get; set; }
        public decimal TransferedAmount { get; set; }
        public DateTime TransferDate { get; set; }
    }
}
