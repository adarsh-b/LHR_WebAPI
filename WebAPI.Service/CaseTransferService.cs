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

        #region UpdateAtlasCaseToGYB
        public void UpdateAtlasCaseToGYB(IEnumerable<TransferredCaseData> data)
        {
            CaseTransferDAL.UpdateAtlasCaseToGYB(data);
        }
        #endregion

        #region GetBillFiles
        public IEnumerable<FileData> GetBillFiles(string BillNo)
        {
            return CaseTransferDAL.GetBillFiles(BillNo);
        }
        #endregion

        #region GetAllBillDocumentsByCaseID
        public IEnumerable<FileData> GetAllBillDocumentsByCaseID(string BillNumber)
        {
            return CaseTransferDAL.GetAllBillDocumentsByCaseID(BillNumber);
        }
        #endregion

        #region UpdateBillStatus
        public int UpdateBillStatus(string CompanyId, string BillNumber, string LawFirmId)
        {
            return CaseTransferDAL.UpdateBillStatus(CompanyId, BillNumber, LawFirmId);
        }
        #endregion

        #region GetReconcillationReport
        public BillTransferReconcillation GetReconcillationReport(string lawFirmCaseManagerAppName)
        {
            return CaseTransferDAL.GetReconcillationReport(lawFirmCaseManagerAppName);
        }
        #endregion

        #region GetParameterValue
        public string GetParameterValue(string ParameterName)
        {
            return ApplicationSettings.GetParameterValue(ParameterName);
        }
        #endregion

        #region GetParameterValue
        public string GetPhysicalBasePath(string ParameterName, string basePathID)
        {
            return ApplicationSettings.GetPhysicalBasePath(ParameterName, basePathID);
        }
        #endregion
    }
}
