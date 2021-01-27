using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.DataAccessLayer;



namespace WebAPI.Service
{
    public class CaseTransferService
    {
        #region GetTransferredCases
        public IEnumerable<CaseModel> GetTransferredCases(IEnumerable<LawFirmID> lstLawFirms)
        {
            return CaseTransferDAL.GetTransferredCases(lstLawFirms);
        }
        #endregion

        #region GetBillFiles
        public IEnumerable<FileData> GetBillFiles(string BillNo, string LawFirmID)
        {
            return CaseTransferDAL.GetBillFiles(BillNo, LawFirmID);
        }
        #endregion

        #region UpdateBillStatus
        public int UpdateBillStatus(string CompanyId, string BillNumber, string LawFirmId)
        {
            return CaseTransferDAL.UpdateBillStatus(CompanyId, BillNumber, LawFirmId);
        }
        #endregion

        #region UpdateAtlasCaseToGYB
        public void UpdateAtlasCaseToGYB(IEnumerable<TransferredCaseData> data)
        {
            CaseTransferDAL.UpdateAtlasCaseToGYB(data);
        }
        #endregion

        #region GetNonAtlasLwfirms
        public IEnumerable<LawFirm> GetNonAtlasLwfirms()
        {
            return CaseTransferDAL.GetNonAtlasLwfirms();
        }
        #endregion

        #region GetAtlasLawfirms
        public IEnumerable<LawFirm> GetAtlasLawfirms()
        {
            return CaseTransferDAL.GetAtlasLawfirms();
        }
        #endregion

        #region GetLawFirmCaseTransferBatch
        public IEnumerable<CaseTransferBatch> GetLawFirmCaseTransferBatch(string lawfirmid)
        {
            return CaseTransferDAL.GetLawFirmCaseTransferBatch(lawfirmid);
        }
        #endregion

        #region GetTransferredCasesByBatch
        public IEnumerable<CaseModel> GetTransferredCasesByBatch(string lawfirmID, string batchID)
        {
            return CaseTransferDAL.GetTransferredCasesByBatch(lawfirmID, batchID);
        }
        #endregion

        #region UpdateTranferredBatchStatus
        public void UpdateTranferredBatchStatus(string LawFirmId, string BatchID, string fileUploadBasePath, string zipFileName, string basePathID)
        {
            CaseTransferDAL.UpdateTranferredBatchStatus(LawFirmId, BatchID, fileUploadBasePath, zipFileName, basePathID);
        }
        #endregion

        #region GetReconcillationReport
        public BillTransferReconcillation GetReconcillationReport(string lawFirmCaseManagerAppName)
        {
            return CaseTransferDAL.GetReconcillationReport(lawFirmCaseManagerAppName);
        }
        #endregion

        #region GetWithdrawnCases
        public IEnumerable<WithdrawnCase> GetWithdrawnCases(string lawFirmID)
        {
            return CaseTransferDAL.GetWithdrawnCases(lawFirmID);
        }
        #endregion

        #region UpdateWithdrawnCases
        public void UpdateWithdrawnCases(List<WithdrawnCase> withdrawnCases)
        {
            CaseTransferDAL.UpdateWithdrawnCases(withdrawnCases);
        }
        #endregion

        #region NotifyAttorneyCaseWithdrawl
        public void NotifyAttorneyCaseWithdrawl()
        {
            CaseTransferDAL.NotifyAttorneyCaseWithdrawl();
        }
        #endregion

        #region NotifyGYBCaseTransfer
        public void NotifyGYBCaseTransfer()
        {
            CaseTransferDAL.NotifyGYBCaseTransfer();
        }
        #endregion

        #region GetNotAtlasClientsBillData
        public string GetNotAtlasClientsBillData(string BillNumbers)
        {
            return CaseTransferDAL.GetNotAtlasClientsBillData(BillNumbers);
        }
        #endregion

        #region GetAllBillDocumentsByCaseID
        public IEnumerable<FileData> GetAllBillDocumentsByCaseID(string BillNumber)
        {
            return CaseTransferDAL.GetAllBillDocumentsByCaseID(BillNumber);
        }
        #endregion

        #region GetParameterValue
        public string GetParameterValue(string ParameterName)
        {
            return ApplicationSettings.GetParameterValue(ParameterName);
        }
        #endregion

        #region GetParameterValue
        public string GetParameterValue(string ParameterName, string basePathID)
        {
            return ApplicationSettings.GetParameterValue(ParameterName, basePathID);
        }
        #endregion

        #region GetCaseDocumentList
        public IList<DocumentNode> GetCaseDocumentList(string caseId, string companyId)
        {
            return CaseTransferDAL.GetCaseDocumentList(caseId, companyId);
        }
        #endregion
    }
}
