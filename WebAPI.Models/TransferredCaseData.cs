using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class TransferredCaseData
    {
        public int ID { get; set; }
        public string GYBBillNumber { get; set; }
        public decimal BillAmount { get; set; }
        public decimal TransferAmount { get; set; }
        public string GYBAccountID { get; set; }
        public string GYBLawFirmID { get; set; }
        public string GYBProviderID { get; set; }
        public string GYBInsuranceCompanyID { get; set; }
        public DateTime TransferdDate { get; set; }
        public string AtlasCaseID { get; set; }
        public string AtlasCaseIndexNumber { get; set; }
        public decimal AtlasPrincipalAmountCollected { get; set; }
        public decimal AtlasInterestAmountCollected { get; set; }
        public string TransferdStatus { get; set; }
        public string AtlasCaseStatus { get; set; }
        public DateTime AtlasLastTransactionDate { get; set; }
    }
}
