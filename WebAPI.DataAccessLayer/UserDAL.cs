using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text;

namespace WebAPI.DataAccessLayer
{
    public class UserDAL
    {
        static string AuthUser = ConfigurationManager.AppSettings["AuthUser"].ToString();
        static string AuthPassword = ConfigurationManager.AppSettings["AuthPassword"].ToString();
        #region Validateuser
        public static bool ValidateUser(string userName, string passWord)
        {

            if (userName == AuthUser && AuthPassword == passWord)
            {
                return true;
            }
            return false;

        }
        #endregion
    }
}
