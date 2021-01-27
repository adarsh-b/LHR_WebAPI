using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class FileData
    {
        public string FileName
        {
            get;
            set;
        }
        public string FilePath
        {
            get;
            set;
        }
        public string NodeType
        {
            get;
            set;
        }
        public int BasePathId
        {
            get;
            set;
        }
        public string PhysicalBasePath
        {
            get;
            set;
        }
        public string BasePathType
        {
            get;
            set;
        }
        public byte[] FileContent
        {
            get;
            set;
        }
        public string FileString
        {
            get;
            set;
        }

        public string DocumentType
        {
            get;
            set;
        }
    }
}
