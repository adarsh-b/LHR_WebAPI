using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class ProcessedDocumentItem
    {
        public string DocumentIDsSyncedToATLAS { get; set; }
        public string DocumentIDsNotSyncedToATLAS { get; set; }
    }
}
