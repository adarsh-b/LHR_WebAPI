using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class BillTransferReconcillation
    {
        public BillTransferReconcillation()
        {
            BillTransferSummary = new List<BillTransferSummary>();
            TransferredBills = new List<TransferredBillInfo>();
            NonAcknowledgedBills = new List<TransferredBillInfo>();
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IList<BillTransferSummary> BillTransferSummary { get; set; }
        public IList<TransferredBillInfo> TransferredBills { get; set; }
        public IList<TransferredBillInfo> NonAcknowledgedBills { get; set; }
    }
}
