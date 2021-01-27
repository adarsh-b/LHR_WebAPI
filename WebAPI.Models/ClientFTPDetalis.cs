using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class ClientFTPDetalis
    {
        public string FtpAddress
        {
            get;
            set;
        }
        public string UserName 
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
        public string Path
        {
            get;
            set;
        }

    }
}
