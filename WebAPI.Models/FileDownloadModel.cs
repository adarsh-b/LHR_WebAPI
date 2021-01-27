using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class FileDownloadModel
    {
        public string FilePath { get; set; }
        public string BasePathId { get; set; }
        public bool UseCustomBasePath { get; set; }
        public string CustomPhysicalBasePath { get; set; }
    }
}
