using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class WithdrawnCase
    {
        public int WithdrawlID { get; set; }
        public string BillNumber { get; set; }
        public string CompanyID { get; set; }
        public string LawFirmID { get; set; }
        public DateTime DateWithdrawn { get; set; }
    }
}
