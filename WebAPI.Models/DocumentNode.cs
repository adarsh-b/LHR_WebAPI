using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class DocumentNode
    {
        public int NodeId { set; get; }
        public int ParentNodeId { set; get; }
        public string NodeName { set; get; }
        public int NodeLevel { set; get; }
        public bool IsFolder { set; get; }
        public string FilePath { set; get; }
        public string BasePathId { set; get; }
    }
}
