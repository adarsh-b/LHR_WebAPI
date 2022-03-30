using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class BillDocumentItem
    {
        public int ID { get; set; }
        public string CompanyID { get; set; }
        public string LawFirmID { get; set; }
        public string CaseID { get; set; }
        public string BillNumber { get; set; }
        public string NodeType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int BasePathId { get; set; }
        public string BasePathType { get; set; }
    }
}
