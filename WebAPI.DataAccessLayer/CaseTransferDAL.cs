using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using WebAPI.Models;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Utility.Extensions;
using Newtonsoft.Json;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.IO;
using System.Net.Mail;
using System.Net;
using GYBGeneralOperations.Extensions;

namespace WebAPI.DataAccessLayer
{
    public static class CaseTransferDAL
    {
        static string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        static int basepathid = 1;

        #region GetTransferredCases
        public static IEnumerable<CaseModel> GetTransferredCases(IEnumerable<LawFirmID> lstLawFirms)
        {
            DataTable table = new DataTable();
            table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(lstLawFirms), typeof(DataTable));
            IEnumerable<CaseModel> lstCaseModel = null;
            Database database = null;

            database = new SqlDatabase(ConnectionString);

            DataTable dtCase = null;

            using (DbCommand dbcommand = database.GetStoredProcCommand("sp_select_lf_ggb_headerNew"))
            {
                //database.AddInParameter(dbcommand, "@LawFirmDetails", DbType.Object, table);
                SqlParameter p = new SqlParameter("@LawFirmDetails", table);
                p.SqlDbType = SqlDbType.Structured;
                dbcommand.Parameters.Add(p);

                dtCase = database.ExecuteDataSet(dbcommand).Tables[0];

                lstCaseModel = (from DataRow dr in dtCase.Rows
                                select new CaseModel()
                                {
                                    CaseId = Convert.ToInt32(dr["sz_case_id"]),
                                    CaseNo = Convert.ToInt32(dr["sz_case_no"]),
                                    PatientFirstName = Convert.ToString(dr["sz_patient_first_name"]),
                                    PatientLastName = Convert.ToString(dr["sz_patient_last_name"]),
                                    InsuranceName = Convert.ToString(dr["sz_insurance_name"]),
                                    InsuranceAddress = Convert.ToString(dr["sz_insurance_address"]),
                                    InsuranceCity = Convert.ToString(dr["sz_insurance_city"]),
                                    InsuranceState = Convert.ToString(dr["sz_state"]),
                                    InsuranceZip = Convert.ToString(dr["sz_insurance_zip"]),
                                    InsurancePhone = Convert.ToString(dr["sz_insurance_phone"]),
                                    InsuranceEmail = Convert.ToString(dr["sz_insurance_email"]),
                                    PatientAddress = Convert.ToString(dr["sz_patient_address"]),
                                    PatientStreet = Convert.ToString(dr["sz_patient_street"]),
                                    PatientCity = Convert.ToString(dr["sz_patient_city"]),
                                    PatientState = Convert.ToString(dr["sz_patient_state"]),
                                    PatientZip = Convert.ToString(dr["sz_patient_zip"]),
                                    PatientPhone = Convert.ToString(dr["sz_patient_phone"]),
                                    PolicyNumber = Convert.ToString(dr["sz_policy_number"]),
                                    ClaimNumber = Convert.ToString(dr["sz_claim_number"]),
                                    BillStatusName = Convert.ToString(dr["sz_bill_status_name"]),
                                    AttorneyName = Convert.ToString(dr["sz_attorney_name"]),
                                    AttorneyLastName = Convert.ToString(dr["sz_attorney_last_name"]),
                                    AttorneyAddress = Convert.ToString(dr["sz_attorney_address"]),
                                    AttorneyCity = Convert.ToString(dr["sz_attorney_city"]),
                                    AttorneyState = Convert.ToString(dr["sz_attorney_state"]),
                                    AttorneyZip = Convert.ToString(dr["sz_attorney_zip"]),
                                    AttorneyFax = Convert.ToString(dr["sz_attorney_fax"]),
                                    SocialSecurityNo = Convert.ToString(dr["sz_social_security_no"]),
                                    PolicyHolder = Convert.ToString(dr["SZ_POLICY_HOLDER"]),
                                    BillNumber = Convert.ToString(dr["sz_bill_number"]),
                                    FltBillAmount = Convert.ToString(dr["flt_bill_amount"]),
                                    FltPaid = Convert.ToString(dr["flt_paid"]),
                                    FltBalance = Convert.ToString(dr["flt_balance"]),
                                    FirstVisitDate = StringExtensions.TryParseNullable(dr["dt_first_visit_date"].ToString()),
                                    LastVisitDate = StringExtensions.TryParseNullable(dr["dt_last_visit_date"].ToString()),
                                    CaseTypeName = Convert.ToString(dr["sz_case_type_name"]),
                                    Location = Convert.ToString(dr["location"]),
                                    CompanyId = Convert.ToString(dr["sz_company_id"]),
                                    CompanyName = Convert.ToString(dr["Company_Name"]),
                                    ProviderName = Convert.ToString(dr["provider_name"]),
                                    ProviderAddress = Convert.ToString(dr["provider_address"]),
                                    ProviderCity = Convert.ToString(dr["provider_city"]),
                                    ProviderZip = Convert.ToString(dr["provider_zip"]),
                                    ProviderState = Convert.ToString(dr["provider_state"]),
                                    ProviderTaxId = Convert.ToString(dr["Provider tax id"]),
                                    DoctorTaxId = Convert.ToString(dr["Doctor tax id"]),
                                    DoctorName = Convert.ToString(dr["Doctor Name"]),
                                    Specialty = Convert.ToString(dr["Specialty"]),

                                    POMStampDate = Convert.ToString(dr["PomStampDate"]),
                                    POMGenerated = Convert.ToString(dr["POMGenerated"]),
                                    POMid = Convert.ToString(dr["POMid"]),

                                    DateofAccident = StringExtensions.TryParseNullable(dr["dt_date_of_accident"].ToString()),
                                    AssignedLawFirmId = Convert.ToString(dr["sz_assigned_lawfirm_id"]),
                                    TransferAmount = Convert.ToString(dr["transfer_amount"]),
                                    DateOfTransferred = StringExtensions.TryParseNullable(dr["dt_date_of_transferred"].ToString()),
                                    BillDate = StringExtensions.TryParseNullable(dr["pom_date"].ToString()),
                                    Providerid = Convert.ToString(dr["provider_id"]),
                                    Insurancecompanyid = Convert.ToString(dr["insurancecompanyid"]),
                                    DenialReason1 = StringExtensions.GetReasons(Convert.ToString(dr["denial"])).ContainsKey(0) ? StringExtensions.GetReasons(Convert.ToString(dr["denial"]))[0] : string.Empty,
                                    DenialReason2 = StringExtensions.GetReasons(Convert.ToString(dr["denial"])).ContainsKey(1) ? StringExtensions.GetReasons(Convert.ToString(dr["denial"]))[1] : string.Empty,
                                    DenialReason3 = StringExtensions.GetReasons(Convert.ToString(dr["denial"])).ContainsKey(2) ? StringExtensions.GetReasons(Convert.ToString(dr["denial"]))[2] : string.Empty,
                                    TreatmentDetails = Convert.ToString(dr["TreatmentDetails"]),
                                    DiagnosisCodes = Convert.ToString(dr["DiagnosisCodes"]),
                                    ReferringDoctorName = Convert.ToString(dr["ReferringDoctorName"]),
                                    ReferringProviderName = Convert.ToString(dr["ReferringProviderName"]),
                                    ChartNo = Convert.ToString(dr["ChartNo"])
                                }).ToList();
            }
            return lstCaseModel;

        }
        #endregion.

        #region UpdateAtlasCaseToGYB
        public static void UpdateAtlasCaseToGYB(IEnumerable<TransferredCaseData> data)
        {
            if (data.Count() > 0)
            {
                DataTable table = new DataTable();
                table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(data), typeof(DataTable));

                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    using (SqlCommand command = new SqlCommand("SyncAtlasCaseDataToGYB", con))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter pTransferedCaseData = new SqlParameter("@TransferredCaseData", table);
                        pTransferedCaseData.SqlDbType = System.Data.SqlDbType.Structured;
                        command.Parameters.Add(pTransferedCaseData);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        #endregion

        #region GetBillFiles
        public static IEnumerable<FileData> GetBillFiles(string BillNo)
        {
            DataSet ds = new DataSet();
            List<FileData> lstData = new List<FileData>();
            Database database = null;
            database = new SqlDatabase(ConnectionString);

            using (DbCommand dbcommand = database.GetStoredProcCommand("procGetCaseDocuments_NEW"))
            {
                database.AddInParameter(dbcommand, "@BillNumber", DbType.String, BillNo);
                ds = database.ExecuteDataSet(dbcommand);
            }

            if (ds.Tables.Count > 1)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    lstData = (from DataRow dr in ds.Tables[1].Rows
                               select new FileData()
                               {
                                   FilePath = Convert.ToString(dr["PATH"]),
                                   NodeType = Convert.ToString(dr["Type"]),
                                   PhysicalBasePath  = Convert.ToString(dr["PhysicalBasePath"]),
                                   BasePathId = Convert.ToInt32(dr.Table.Columns.Contains("basepathid") ? Convert.ToInt32(dr["basepathid"]) : basepathid),
                                   BasePathType = "3",
                               }).ToList();
                }
            }
            return lstData;
        }
        #endregion

        #region GetAllBillDocumentsByCaseID
        public static IEnumerable<FileData> GetAllBillDocumentsByCaseID(string BillNumber)
        {
            DataSet ds = new DataSet();
            List<FileData> lstData = new List<FileData>();

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (SqlCommand command = new SqlCommand("procGetCaseDocuments_NEW", con))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandTimeout = 0;

                    SqlParameter pCaseID = new SqlParameter("@BillNumber", BillNumber);
                    pCaseID.DbType = DbType.String;
                    command.Parameters.Add(pCaseID);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            lstData.AddRange((from DataRow dr in ds.Tables[0].Rows
                                              select new FileData()
                                              {
                                                  FilePath = Convert.ToString(dr["PATH"]),
                                                  NodeType = Convert.ToString(dr["Type"]),
                                                  PhysicalBasePath = Convert.ToString(dr["PhysicalBasePath"]),
                                                  BasePathId = Convert.ToInt32(dr.Table.Columns.Contains("BasePathId") ? Convert.ToInt32(dr["BasePathId"]) : basepathid),
                                                  BasePathType = Convert.ToString(dr["BasePathType"]),
                                                  DocumentType = "BillDocuments"
                                              }).ToList());
                        }
                    }

                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            lstData.AddRange((from DataRow dr in ds.Tables[1].Rows
                                              select new FileData()
                                              {
                                                  FilePath = Convert.ToString(dr["PATH"]),
                                                  NodeType = Convert.ToString(dr["Type"]),
                                                  PhysicalBasePath = Convert.ToString(dr["PhysicalBasePath"]),
                                                  BasePathId = Convert.ToInt32(dr.Table.Columns.Contains("BasePathId") ? Convert.ToInt32(dr["BasePathId"]) : basepathid),
                                                  BasePathType = Convert.ToString(dr["BasePathType"]),
                                                  DocumentType = "Miscellaneous"
                                              }).ToList());
                        }
                    }
                }
            }
            return lstData;
        }
        #endregion

        #region UpdateBillStatus
        public static int UpdateBillStatus(string CompanyId, string BillNumber, string LawFirmId)
        {
            Database database = null;
            database = new SqlDatabase(ConnectionString);

            using (DbCommand dbcommand = database.GetStoredProcCommand("UpdateBillStatus"))
            {
                database.AddInParameter(dbcommand, "@CompanyId", DbType.String, CompanyId);
                database.AddInParameter(dbcommand, "@BillNumber", DbType.String, BillNumber);
                database.AddInParameter(dbcommand, "@LawFirmId", DbType.String, LawFirmId);
                return (database.ExecuteNonQuery(dbcommand));
            }
        }
        #endregion

        #region GetReasons
        private static Dictionary<int, string> GetReasons(string reason)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();

            try
            {
                DenialReasons d = new DenialReasons();
                string[] denials = reason.Split(',');
                int i = 0;
                if (!string.IsNullOrEmpty(reason))
                {
                    if (denials.Length > 0)
                    {
                        foreach (var item in denials)
                        {
                            dic.Add(i++, item);
                        }
                    }
                }
                return dic;
            }
            catch (Exception ex)
            {
                string s = ex.Message;

                return dic;
            }

        }
        #endregion

        #region GetReconcillationReport
        public static BillTransferReconcillation GetReconcillationReport(string lawFirmCaseManagerAppName)
        {
            BillTransferReconcillation recon = new BillTransferReconcillation();
            recon.BillTransferSummary = new List<BillTransferSummary>();
            recon.TransferredBills = new List<TransferredBillInfo>();
            recon.NonAcknowledgedBills = new List<TransferredBillInfo>();

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (SqlCommand command = new SqlCommand("GetReconcillationReport", con))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter pStartDate = new SqlParameter("@StartDate", DateTime.Now);
                    pStartDate.SqlDbType = SqlDbType.Date;
                    pStartDate.Direction = ParameterDirection.InputOutput;
                    command.Parameters.Add(pStartDate);

                    SqlParameter pEndDate = new SqlParameter("@EndDate", DateTime.Now);
                    pEndDate.SqlDbType = SqlDbType.Date;
                    pEndDate.Direction = ParameterDirection.InputOutput;
                    command.Parameters.Add(pEndDate);

                    SqlParameter pLawFirmCaseManagerAppName = new SqlParameter("@LawFirmCaseManagerAppName", lawFirmCaseManagerAppName);
                    pLawFirmCaseManagerAppName.SqlDbType = SqlDbType.VarChar;
                    command.Parameters.Add(pLawFirmCaseManagerAppName);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        //Read BillTransfer Summary data
                        while (reader.Read())
                        {
                            recon.BillTransferSummary.Add(new BillTransferSummary
                            {
                                LawFirmID = reader.GetString(0),
                                LawFirmName = reader.GetString(1),
                                AccountID = reader.GetString(2),
                                AccountName = reader.GetString(3),
                                TransferredBillCount = reader.GetInt32(4),
                                TransferredBillAmount = reader.GetDecimal(5),
                                AcknowlledgedBillCount = reader.GetInt32(6),
                                AcknowlledgedBillAmount = reader.GetDecimal(7)
                            });
                        }

                        reader.NextResult();

                        //Read Transferred Bills data
                        while (reader.Read())
                        {
                            recon.TransferredBills.Add(new TransferredBillInfo
                            {
                                LawFirmID = reader.GetString(0),
                                LawFirmName = reader.GetString(1),
                                AccountID = reader.GetString(2),
                                AccountName = reader.GetString(3),
                                CaseID = reader.GetString(4),
                                CaseNumber = reader.GetString(5),
                                PatientName = reader.GetString(6),
                                BillNumber = reader.GetString(7),
                                BillAmount = Convert.ToDecimal(reader[8]),
                                TransferedAmount = Convert.ToDecimal(reader[9]),
                                TransferDate = reader.GetDateTime(10)
                            });
                        }
                        reader.NextResult();

                        //Read Transferred Bills data
                        while (reader.Read())
                        {
                            recon.NonAcknowledgedBills.Add(new TransferredBillInfo
                            {
                                LawFirmID = reader.GetString(0),
                                LawFirmName = reader.GetString(1),
                                AccountID = reader.GetString(2),
                                AccountName = reader.GetString(3),
                                CaseID = reader.GetString(4),
                                CaseNumber = reader.GetString(5),
                                PatientName = reader.GetString(6),
                                BillNumber = reader.GetString(7),
                                BillAmount = Convert.ToDecimal(reader[8]),
                                TransferedAmount = Convert.ToDecimal(reader[9]),
                                TransferDate = reader.GetDateTime(10)
                            });
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                        }

                        recon.StartDate = Convert.ToDateTime(pStartDate.Value);
                        recon.EndDate = Convert.ToDateTime(pEndDate.Value);
                    }
                }
            }
            return recon;
        }
        #endregion
    }
}
