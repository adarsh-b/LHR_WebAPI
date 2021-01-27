using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.DataAccessLayer
{
    public class ApplicationSettings
    {
        static private SqlDataReader sqlDr;
        static private SqlConnection sqlCon;
        static private string strsqlCon = string.Empty;

        static ApplicationSettings()
        {
            strsqlCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        }
        static public string GetParameterValue(string ParameterName)
        {
            string ParameterValue = string.Empty;
            sqlCon = new SqlConnection(strsqlCon);
            try
            {
                sqlCon.Open();
                sqlDr = new SqlCommand("SELECT " + ParameterName + " FROM tblBasePath where BasePathId=( select ParameterValue from tblapplicationsettings where parametername = 'BasePathId')", sqlCon).ExecuteReader();
                while (sqlDr.Read())
                {
                    ParameterValue = sqlDr[ParameterName].ToString();
                }
                sqlDr.Close();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
            }
            return ParameterValue;
        }

        static public string GetParameterValue(string ParameterName, string basePathID)
        {
            string ParameterValue = string.Empty;
            sqlCon = new SqlConnection(strsqlCon);
            try
            {
                sqlCon.Open();
                sqlDr = new SqlCommand("SELECT " + ParameterName + " FROM tblBasePath where BasePathId=" + basePathID, sqlCon).ExecuteReader();
                while (sqlDr.Read())
                {
                    ParameterValue = sqlDr[ParameterName].ToString();
                }
                sqlDr.Close();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
            }
            return ParameterValue;
        }
    }
}
