using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.DataAccessLayer;

namespace WebAPI.Service
{
    public class UserService
    {
        public bool ValidateUser(string userName, string passWord)
        {
            return UserDAL.ValidateUser(userName, passWord);
        }
    }
}
