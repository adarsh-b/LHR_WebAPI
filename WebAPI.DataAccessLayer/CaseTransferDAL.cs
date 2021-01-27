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
                                    DiagnosisCodes = Convert.ToString(dr["DiagnosisCodes"])
                                }).ToList();
            }
            return lstCaseModel;

        }
        #endregion.

        #region GetBillFiles
        public static IEnumerable<FileData> GetBillFiles(string BillNo, string LawFirmID)
        {
            DataSet ds = new DataSet();
            List<FileData> lstData = new List<FileData>();
            Database database = null;
            database = new SqlDatabase(ConnectionString);

            using (DbCommand dbcommand = database.GetStoredProcCommand("SP_GET_BILL_DOCUMENTS"))
            {
                database.AddInParameter(dbcommand, "@SZ_BILL_NUMBER", DbType.String, BillNo);
                database.AddInParameter(dbcommand, "@SZ_LAWFIRM_CASE_ID", DbType.String, LawFirmID);
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

        #region GetNonAtlasLwfirms
        public static IEnumerable<LawFirm> GetNonAtlasLwfirms()
        {
            List<LawFirm> lawfirms = new List<LawFirm>();

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (SqlCommand command = new SqlCommand("SP_MST_LEGAL_LOGIN", con))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FLAG", "NON_ATLAS_LAWFIRMS");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lawfirms.Add(new LawFirm
                            {
                                LawFirmId = reader.GetString(0),
                                LawFirmName = reader.GetString(1),
                                Email = Convert.ToString(reader[2])
                            });
                        }
                    }
                }
            }
            return lawfirms;
        }
        #endregion

        #region GetAtlasLawfirms
        public static IEnumerable<LawFirm> GetAtlasLawfirms()
        {
            List<LawFirm> lawfirms = new List<LawFirm>();

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (SqlCommand command = new SqlCommand("SP_MST_LEGAL_LOGIN", con))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FLAG", "ATLAS_LAWFIRMS");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lawfirms.Add(new LawFirm
                            {
                                LawFirmId = reader.GetString(0),
                                LawFirmName = reader.GetString(1),
                                Email = reader.GetString(2)
                            });
                        }
                    }
                }
            }
            return lawfirms;
        }
        #endregion

        #region GetLawFirmCaseTransferBatch
        public static IEnumerable<CaseTransferBatch> GetLawFirmCaseTransferBatch(string lawfirmid)
        {
            List<CaseTransferBatch> batches = new List<CaseTransferBatch>();

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (SqlCommand command = new SqlCommand("usp_GetNonAtlasCaseTranseferQueue", con))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LawFirmID", lawfirmid);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            batches.Add(new CaseTransferBatch
                            {
                                ID = Convert.ToInt32(reader["I_ID"]),
                                CompanyID = Convert.ToString(reader["SZ_COMPANY_ID"]),
                                LawFirmID = Convert.ToString(reader["SZ_LAWFIRM_ID"]),
                                TransferDate = StringExtensions.TryParseNullable(reader["DT_TRANSFER_DATE"].ToString()),
                                BatchID = Convert.ToString(reader["SZ_BATCH_ID"]),
                                IsEmailSent = Convert.ToBoolean(reader["BT_EMAIL_SENT"] == DBNull.Value ? 0 : reader["BT_EMAIL_SENT"]),
                                UserID = Convert.ToString(reader["SZ_USER_ID"] == DBNull.Value ? null : reader["SZ_USER_ID"]),
                                BillingUserID = Convert.ToInt32(reader["I_BILLING_USER_ID"] == DBNull.Value ? null : reader["I_BILLING_USER_ID"]),
                                UpdatedByUserID = Convert.ToInt32(reader["I_UPDATE_USER_ID"] == DBNull.Value ? null : reader["I_UPDATE_USER_ID"]),
                                IsBatchDownloaded = Convert.ToBoolean(reader["BT_BATCH_DOWNLOADED"] == DBNull.Value ? 0 : reader["BT_BATCH_DOWNLOADED"]),
                                BatchDownloadDate = StringExtensions.TryParseNullable(reader["DT_BATCHDOWNLOAD_DATE"].ToString())
                            });
                        }
                    }
                }
            }
            return batches;
        }
        #endregion

        #region GetTransferredCasesByBatch
        public static IEnumerable<CaseModel> GetTransferredCasesByBatch(string lawfirmID, string batchID)
        {
            IEnumerable<CaseModel> lstCaseModel = null;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (SqlCommand command = new SqlCommand("usp_GetTransferredCaseByBatch", con))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@LawFirmID", lawfirmID);
                    command.Parameters.AddWithValue("@BatchID", batchID);

                    DataTable dtCase = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dtCase);

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
                                        DateofAccident = StringExtensions.TryParseNullable(dr["dt_date_of_accident"].ToString()),
                                        AssignedLawFirmId = Convert.ToString(dr["sz_assigned_lawfirm_id"]),
                                        TransferAmount = Convert.ToString(dr["transfer_amount"]),
                                        DateOfTransferred = StringExtensions.TryParseNullable(dr["dt_date_of_transferred"].ToString()),
                                        BillDate = StringExtensions.TryParseNullable(dr["pom_date"].ToString()),
                                        Providerid = Convert.ToString(dr["provider_id"]),
                                        Insurancecompanyid = Convert.ToString(dr["insurancecompanyid"]),
                                        DenialReason1 = GetReasons(Convert.ToString(dr["denial"])).ContainsKey(0) ? GetReasons(Convert.ToString(dr["denial"]))[0] : string.Empty,
                                        DenialReason2 = GetReasons(Convert.ToString(dr["denial"])).ContainsKey(1) ? GetReasons(Convert.ToString(dr["denial"]))[1] : string.Empty,
                                        DenialReason3 = GetReasons(Convert.ToString(dr["denial"])).ContainsKey(2) ? GetReasons(Convert.ToString(dr["denial"]))[2] : string.Empty,
                                        TreatmentDetails = Convert.ToString(dr["TreatmentDetails"]),
                                        DiagnosisCodes = Convert.ToString(dr["DiagnosisCodes"])
                                    }).ToList();
                }
                return lstCaseModel;
            }
        }
        #endregion

        #region UpdateTranferredBatchStatus
        public static void UpdateTranferredBatchStatus(string LawFirmId, string BatchID, string fileUploadBasePath, string fileName, string basePathID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (SqlCommand command = new SqlCommand("UpdateTransferredBatchStatus", con))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter pCompanyId = new SqlParameter("@LawFirmId", LawFirmId);
                    pCompanyId.DbType = DbType.String;
                    command.Parameters.Add(pCompanyId);

                    SqlParameter pBatchNumber = new SqlParameter("@BatchNumber", BatchID);
                    pBatchNumber.DbType = DbType.String;
                    command.Parameters.Add(pBatchNumber);

                    SqlParameter pfileUploadBasePath = new SqlParameter("@FileUploadBasePath", fileUploadBasePath);
                    pfileUploadBasePath.DbType = DbType.String;
                    command.Parameters.Add(pfileUploadBasePath);

                    SqlParameter pzipFileName = new SqlParameter("@FileName", fileName);
                    pzipFileName.DbType = DbType.String;
                    command.Parameters.Add(pzipFileName);

                    SqlParameter pBasePathID = new SqlParameter("@BasePathID", basePathID);
                    pBasePathID.DbType = DbType.String;
                    command.Parameters.Add(pBasePathID);

                    command.ExecuteNonQuery();
                }
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

        #region GetWithdrawnCases
        public static IEnumerable<WithdrawnCase> GetWithdrawnCases(string lawFirmID)
        {
            List<WithdrawnCase> withdrawnCases = new List<WithdrawnCase>();

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (SqlCommand command = new SqlCommand("SP_GetWithdrawnCases", con))
                {
                    command.CommandTimeout = 0;
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter pLawFirmID = new SqlParameter("@LawFirmId", lawFirmID);
                    pLawFirmID.DbType = DbType.String;
                    command.Parameters.Add(pLawFirmID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            withdrawnCases.Add(new WithdrawnCase
                            {
                                WithdrawlID = Convert.ToInt32(reader.GetValue(0)),
                                BillNumber = reader.GetString(1),
                                CompanyID = Convert.ToString(reader[2]),
                                LawFirmID = Convert.ToString(reader[3]),
                                DateWithdrawn = Convert.ToDateTime(reader[4]),
                            });
                        }
                    }
                }
            }
            return withdrawnCases;
        }
        #endregion

        #region UpdateWithdrawnCases
        public static void UpdateWithdrawnCases(List<WithdrawnCase> withdrawnCases)
        {
            if (withdrawnCases.Count() > 0)
            {
                DBHelper.DataCollection<WithdrawnCase> dataToUpdate = new DBHelper.DataCollection<WithdrawnCase>();
                dataToUpdate.AddRange(withdrawnCases.Select(d => d));

                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    using (SqlCommand command = new SqlCommand("SP_UpdateWithdrawnCaseStatus", con))
                    {
                        command.CommandTimeout = 0;
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter pTransferedCaseData = new SqlParameter("@WithdrawnCaseData", dataToUpdate);
                        pTransferedCaseData.SqlDbType = System.Data.SqlDbType.Structured;
                        command.Parameters.Add(pTransferedCaseData);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        #endregion

        #region NotifyAttorneyCaseWithdrawl
        public static void NotifyAttorneyCaseWithdrawl()
        {
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (SqlCommand command = new SqlCommand("SP_GetWthdrawnCasesFromAttorney", con))
                {
                    command.CommandTimeout = 0;
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                }
            }

            //Send Case Transfer Notification Email to Attorneys
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    MailMessage mailMessage = new MailMessage();

                    if (Convert.ToString(row["EmailReciepients"]).Trim() != "")
                    {
                        SmtpClient smtpClient = new SmtpClient();
                        string emailBody = string.Empty;
                        string emailSubject = "NOTIFICATION: GoGYB - Case Withdrawl from attorney";
                        string emailReciepient = String.Format("Dear {0}", Convert.ToString(row["LawFirmName"]));
                        string mailbody = "";
                        string strFileName = "Litigation_Desk_" + (new Random()).Next(1, 10000).ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmssms") + ".xlsx";
                        string strBasePath = ConfigurationManager.AppSettings["TempFilesPath"].ToString();
                        string strAttachmentFileName = string.Concat(strBasePath, strFileName);

                        DataView caseSummary = new DataView(ds.Tables[1]);
                        caseSummary.RowFilter = "LawFirmID = '" + Convert.ToString(row["LawFirmID"]) + "'";

                        DataView view = new DataView(ds.Tables[3]);
                        view.RowFilter = "AssignedLawFirmID = '" + Convert.ToString(row["LawFirmID"]) + "'";

                        DataTable casesForLF = view.ToTable();
                        casesForLF.Columns.Remove("AssignedLawFirmID");

                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            wb.Worksheets.Add(casesForLF, "WithfrawnCases");
                            wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            wb.Style.Font.Bold = true;
                            wb.SaveAs(strAttachmentFileName);
                        }

                        mailMessage.To.Add(Convert.ToString(row["EmailReciepients"]));
                        string strEmailFrom = ConfigurationManager.AppSettings["FromMailID"].ToString();
                        string strEmailPassword = ConfigurationManager.AppSettings["Password"].ToString();
                        smtpClient.Credentials = new NetworkCredential(strEmailFrom, strEmailPassword);
                        smtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"].ToString());
                        smtpClient.Host = ConfigurationManager.AppSettings["SMTPServer"].ToString();
                        smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"].ToString());
                        mailMessage.IsBodyHtml = true;
                        mailMessage.From = new MailAddress(strEmailFrom);

                        Attachment attachment = null;
                        attachment = new Attachment(strAttachmentFileName);
                        mailMessage.Attachments.Add(attachment);
                        attachment.ContentDisposition.Inline = false;
                        mailMessage.IsBodyHtml = true;

                        StreamReader streamReader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Template/EmailTemplate.html"));
                        do
                        {
                            emailBody = string.Concat(emailBody, streamReader.ReadLine(), "\r\n");
                        }
                        while (streamReader.Peek() != -1);

                        mailbody = String.Format("Please be notified that medical provider(s) have withdrawn previously assigned cases from your office on {0} as per folliwing details:<BR><BR>",
                            DateTime.Now.ToString("MM/dd/yyyy"));

                        foreach (DataRowView summaryRow in caseSummary)
                        {
                            mailbody = mailbody + "<B>" + Convert.ToString(summaryRow.Row["SZ_COMPANY_NAME"]) + " = " + Convert.ToString(summaryRow.Row["NumberOfCases"]) + " Cases <BR></B>";
                        }

                        mailbody = mailbody + "<BR>The details of withdrawn cases is attached with this email. For any further clarification about these cases, please contact related medical provider."
                                        + "<BR><BR>Please do not reply to this mail since it is not monitored.";

                        emailBody = emailBody.ToString().Replace("@@EmailReciepient@@", emailReciepient);
                        emailBody = emailBody.ToString().Replace("@@EmailSubject@@", emailSubject);
                        emailBody = emailBody.ToString().Replace("@@EmailBody@@", mailbody);

                        mailMessage.Bcc.Add(strEmailFrom);
                        mailMessage.Subject = emailSubject;
                        mailMessage.Body = emailBody;
                        smtpClient.EnableSsl = true;

                        try
                        {
                            smtpClient.Send(mailMessage);
                        }
                        catch (Exception)
                        {

                        }

                        mailMessage.Dispose();

                        if (File.Exists(strAttachmentFileName))
                        {
                            File.Delete(strAttachmentFileName);
                        }
                    }
                }
            }

            //Send Case Transfer Notification Email to Medical Providers
            if (ds.Tables[2].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[2].Rows)
                {
                    MailMessage mailMessage = new MailMessage();

                    if (Convert.ToString(row["SZ_ADMIN_EMAIL"]).Trim() != "")
                    {
                        SmtpClient smtpClient = new SmtpClient();
                        string emailBody = string.Empty;
                        string emailSubject = "NOTIFICATION: GoGYB - Case Withdrawl from attorney";
                        string emailReciepient = String.Format("Dear {0}", Convert.ToString(row["SZ_COMPANY_NAME"]));
                        string mailbody = "";
                        string strFileName = "Litigation_Desk_" + (new Random()).Next(1, 10000).ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmssms") + ".xlsx";
                        string strBasePath = ConfigurationManager.AppSettings["TempFilesPath"].ToString();
                        string strAttachmentFileName = string.Concat(strBasePath, strFileName);

                        DataView view = new DataView(ds.Tables[3]);
                        view.RowFilter = "SZ_COMPANY_ID = '" + Convert.ToString(row["SZ_COMPANY_ID"]) + "'";

                        DataTable casesForProvider = view.ToTable();
                        casesForProvider.Columns.Remove("SZ_COMPANY_ID");

                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            wb.Worksheets.Add(casesForProvider, "AssignedCases");
                            wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            wb.Style.Font.Bold = true;
                            wb.SaveAs(strAttachmentFileName);
                        }

                        mailMessage.To.Add(Convert.ToString(row["SZ_ADMIN_EMAIL"]));
                        string strEmailFrom = ConfigurationManager.AppSettings["FromMailID"].ToString();
                        string strEmailPassword = ConfigurationManager.AppSettings["Password"].ToString();
                        smtpClient.Credentials = new NetworkCredential(strEmailFrom, strEmailPassword);
                        smtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"].ToString());
                        smtpClient.Host = ConfigurationManager.AppSettings["SMTPServer"].ToString();
                        smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"].ToString());
                        mailMessage.IsBodyHtml = true;
                        mailMessage.From = new MailAddress(strEmailFrom);

                        Attachment attachment = null;
                        attachment = new Attachment(strAttachmentFileName);
                        mailMessage.Attachments.Add(attachment);
                        attachment.ContentDisposition.Inline = false;
                        mailMessage.IsBodyHtml = true;

                        StreamReader streamReader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Template/EmailTemplate.html"));
                        do
                        {
                            emailBody = string.Concat(emailBody, streamReader.ReadLine(), "\r\n");
                        }
                        while (streamReader.Peek() != -1);


                        mailbody = String.Format("Please be notified that previously assigned cases have been withdrawn from attorneys.", DateTime.Now.ToString("MM/dd/yyyy"));
                        mailbody = mailbody + "<BR><BR>For your reference, details of these withdrawn cases is attached with this email."
                                            + "<BR><BR>Please do not reply to this mail since it is not monitored.";

                        emailBody = emailBody.ToString().Replace("@@EmailReciepient@@", emailReciepient);
                        emailBody = emailBody.ToString().Replace("@@EmailSubject@@", emailSubject);
                        emailBody = emailBody.ToString().Replace("@@EmailBody@@", mailbody);

                        mailMessage.Bcc.Add(strEmailFrom);
                        mailMessage.Subject = emailSubject;
                        mailMessage.Body = emailBody;
                        smtpClient.EnableSsl = true;

                        try
                        {
                            smtpClient.Send(mailMessage);
                        }
                        catch (Exception)
                        {

                        }
                        mailMessage.Dispose();

                        if (File.Exists(strAttachmentFileName))
                        {
                            File.Delete(strAttachmentFileName);
                        }
                    }
                }
            }
        }
        #endregion

        #region NotifyGYBCaseTransfer
        public static void NotifyGYBCaseTransfer()
        {
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (SqlCommand command = new SqlCommand("SP_GetCasesAssignedToAttorney", con))
                {
                    command.CommandTimeout = 0;
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                }
            }

            //Send Case Transfer Notification Email to Attorneys
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    MailMessage mailMessage = new MailMessage();

                    if (Convert.ToString(row["EmailReciepients"]).Trim() != "")
                    {
                        SmtpClient smtpClient = new SmtpClient();
                        string emailBody = string.Empty;
                        string emailSubject = "NOTIFICATION: GoGYB - Case assignment to attorney";
                        string emailReciepient = String.Format("Dear {0}", Convert.ToString(row["LawFirmName"]));
                        string mailbody = "";

                        DataView caseSummary = new DataView(ds.Tables[1]);
                        caseSummary.RowFilter = "LawFirmID = '" + Convert.ToString(row["LawFirmID"]) + "'";

                        mailMessage.To.Add(Convert.ToString(row["EmailReciepients"]));
                        string strEmailFrom = ConfigurationManager.AppSettings["FromMailID"].ToString();
                        string strEmailPassword = ConfigurationManager.AppSettings["Password"].ToString();
                        smtpClient.Credentials = new NetworkCredential(strEmailFrom, strEmailPassword);
                        smtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"].ToString());
                        smtpClient.Host = ConfigurationManager.AppSettings["SMTPServer"].ToString();
                        smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"].ToString());
                        mailMessage.IsBodyHtml = true;
                        mailMessage.From = new MailAddress(strEmailFrom);
                        mailMessage.IsBodyHtml = true;

                        StreamReader streamReader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Template/EmailTemplate.html"));
                        do
                        {
                            emailBody = string.Concat(emailBody, streamReader.ReadLine(), "\r\n");
                        }
                        while (streamReader.Peek() != -1);

                        mailbody = String.Format("Please be notified that new cases have been assigned to you on {0} by medical providers as per following details:<BR><BR>", DateTime.Now.ToString("MM/dd/yyyy"));
                        foreach (DataRowView summaryRow in caseSummary)
                        {
                            mailbody = mailbody + "<B>" + Convert.ToString(summaryRow.Row["SZ_COMPANY_NAME"]) + " = " + Convert.ToString(summaryRow.Row["NumberOfCases"]) + " Cases <BR></B>";
                        }

                        if (Convert.ToString(row["IsAutoSync"]) == "True")
                        {
                            mailbody = mailbody + "<BR><BR>These cases will be automatically transferred to you case manager application in next 24 hours. You will get a follow - up email notification when these cases and associated documents are transferred to your case manager application.";
                        }
                        else if (Convert.ToString(row["HasFileShareSubscription"]) == "True")
                        {
                            mailbody = mailbody + "<BR><BR>You will receive a follow - up email notification when file dump of these cases and related documents are ready for download. You can also login to GreenBills to access these cases using https://www.gogreenbills.com with your firm's login credentials.";
                        }
                        else
                        {
                            mailbody = mailbody + "<BR><BR>You can login to GreenBills to access these cases using https://www.gogreenbills.com with your firm's login credential.";
                        }

                        mailbody = mailbody +"<BR><BR>Please do not reply to this mail since it is not monitored.";
                        emailBody = emailBody.ToString().Replace("@@EmailReciepient@@", emailReciepient);
                        emailBody = emailBody.ToString().Replace("@@EmailSubject@@", emailSubject);
                        emailBody = emailBody.ToString().Replace("@@EmailBody@@", mailbody);

                        mailMessage.Bcc.Add(strEmailFrom);
                        mailMessage.Subject = emailSubject;
                        mailMessage.Body = emailBody;
                        smtpClient.EnableSsl = true;

                        try
                        {
                            smtpClient.Send(mailMessage);
                        }
                        catch (Exception)
                        {

                        }

                        mailMessage.Dispose();
                    }
                }
            }

            //Send Case Transfer Notification Email to Medical Providers
            if (ds.Tables[2].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[2].Rows)
                {
                    MailMessage mailMessage = new MailMessage();

                    if (Convert.ToString(row["SZ_ADMIN_EMAIL"]).Trim() != "")
                    {
                        SmtpClient smtpClient = new SmtpClient();
                        string emailBody = string.Empty;
                        string emailSubject = "NOTIFICATION: GoGYB - Case assignment to attorney";
                        string emailReciepient = String.Format("Dear {0}", Convert.ToString(row["SZ_COMPANY_NAME"]));
                        string mailbody = "";

                        DataView caseSummary = new DataView(ds.Tables[1]);
                        caseSummary.RowFilter = "SZ_COMPANY_ID = '" + Convert.ToString(row["SZ_COMPANY_ID"]) + "'";

                        mailMessage.To.Add(Convert.ToString(row["SZ_ADMIN_EMAIL"]));
                        string strEmailFrom = ConfigurationManager.AppSettings["FromMailID"].ToString();
                        string strEmailPassword = ConfigurationManager.AppSettings["Password"].ToString();
                        smtpClient.Credentials = new NetworkCredential(strEmailFrom, strEmailPassword);
                        smtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"].ToString());
                        smtpClient.Host = ConfigurationManager.AppSettings["SMTPServer"].ToString();
                        smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"].ToString());
                        mailMessage.IsBodyHtml = true;
                        mailMessage.From = new MailAddress(strEmailFrom);
                        mailMessage.IsBodyHtml = true;

                        StreamReader streamReader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Template/EmailTemplate.html"));
                        do
                        {
                            emailBody = string.Concat(emailBody, streamReader.ReadLine(), "\r\n");
                        }
                        while (streamReader.Peek() != -1);


                        mailbody = String.Format("Please be notified that new cases have been litigated and assigned to attorneys on {0}", DateTime.Now.ToString("MM/dd/yyyy")) + " as per below details:<BR><BR>";

                        foreach (DataRowView summaryRow in caseSummary)
                        {
                            mailbody = mailbody + "<B>" + Convert.ToString(summaryRow.Row["LawFirmName"]) + " = " + Convert.ToString(summaryRow.Row["NumberOfCases"]) + " Cases<BR></B>";
                        }

                        mailbody = mailbody + "<BR><BR>For more details, please login to GreenBills at https://www.gogreenbills.com using your firm's login credentials."
                                            + "<BR><BR>Please do not reply to this mail since it is not monitored.";


                        emailBody = emailBody.ToString().Replace("@@EmailReciepient@@", emailReciepient);
                        emailBody = emailBody.ToString().Replace("@@EmailSubject@@", emailSubject);
                        emailBody = emailBody.ToString().Replace("@@EmailBody@@", mailbody);

                        mailMessage.Bcc.Add(strEmailFrom);
                        mailMessage.Subject = emailSubject;
                        mailMessage.Body = emailBody;
                        smtpClient.EnableSsl = true;

                        try
                        {
                            smtpClient.Send(mailMessage);
                        }
                        catch (Exception)
                        {

                        }
                        mailMessage.Dispose();
                    }
                }
            }


        }
        #endregion

        #region GetNotAtlasClientsBillData
        public static string GetNotAtlasClientsBillData(string BillNumbers)
        {
            string json = "";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (SqlCommand command = new SqlCommand("SP_GET_DOCUMENT_INFO_ATT_DUMP", con))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandTimeout = 0;

                    SqlParameter pBillNumber = new SqlParameter("@SZ_BILL_NUMBER", BillNumbers);
                    pBillNumber.DbType = DbType.String;
                    command.Parameters.Add(pBillNumber);

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);

                    json = JsonConvert.SerializeObject(dt, Formatting.Indented);
                }
            }
            return json;
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
                                                  NodeType = Convert.ToString(dr["NodeType"]),
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
                                                  NodeType = Convert.ToString(dr["NodeType"]),
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

        #region GetCaseDocumentList
        public static IList<DocumentNode> GetCaseDocumentList(string caseId, string companyId)
        {
            SqlConnection oConnection = new SqlConnection(ConnectionString);
            List<DocumentNode> nodeList = new List<DocumentNode>();

            try
            {
                oConnection.Open();
                SqlCommand command = new SqlCommand("[dbo].[spGetDocumentManager]", oConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CaseId", caseId);
                command.Parameters.AddWithValue("@CompanyID", companyId);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DocumentNode node = new DocumentNode();
                    node.NodeId = Convert.ToInt32(reader["NodeId"]);
                    node.ParentNodeId = reader["ParentNodeId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ParentNodeId"]);
                    node.NodeName = Convert.ToString(reader["NodeName"]);
                    node.FilePath = Convert.ToString(reader["FilePath"]);
                    node.BasePathId = Convert.ToString(reader["BasePathId"]);
                    node.NodeLevel = Convert.ToInt32(reader["NodeLevel"]);
                    node.IsFolder = Convert.ToInt32(reader["IsFolder"]) == 0 ? false : true;

                    nodeList.Add(node);
                }
            }
            catch (Exception io)
            {
                oConnection.Close();
                oConnection = null;
                throw io;
            }
            finally
            {
                if (oConnection != null)
                {
                    oConnection.Close();
                    oConnection = null;
                }
            }
            return nodeList;
        }
        #endregion
    }
}
