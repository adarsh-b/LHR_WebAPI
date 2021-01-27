using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class LawFirm
    {
        public string LawFirmId { get; set; }
        public string LawFirmName { get; set; }
        public string Email { get; set; }
        public string AccountDomain { get; set; }
        public string DomainId { get; set; }
    }

    public class LawFirmID
    {
        public string LawFirmId { get; set; }
    }
}
