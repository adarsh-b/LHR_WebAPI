using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
using WebAPI.Service;
using WebAPI.Utility.Extensions;
using WebAPI.Web.Models.common;
using System.Web;
using System.Text;

namespace WebAPI.Web.Controllers
{
    [AllowAnonymous]
    public class TransferController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
               (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string MSG_FAILED = "Operation Failed";
        private readonly string MSG_SUCCESS = "Operation executed successfully.";

        [Authorize]
        [HttpPost]
        [ActionName("TransferGYBBillsToAtlas")]
        public ILResponse TransferGYBBillsToAtlas([FromBody] IEnumerable<LawFirmID> lstFirms)
        {
            ILResponse response = new ILResponse();
            var context = new HttpContextWrapper(HttpContext.Current);

            try
            {
                CaseTransferService service = new CaseTransferService();
                IEnumerable<CaseModel> caseList = service.GetTransferredCases(lstFirms);

                response.MessageCode = MessageCodes.REQUEST_SUCCESS;
                response.MessageText = MSG_SUCCESS;
                response.MessageType = EnumMessageType.OPERATION_SUCCESS;
                response.Add(caseList);
                response.HasException = false;
            }
            catch (Exception ex)
            {
                response.MessageCode = MessageCodes.REQUEST_ERROR;
                response.HasException = true;
                response.MessageText = ex.Message;
                response.MessageType = EnumMessageType.OPERATION_APPLICATION_ERROR;
                response.Exception = ex.ToString();
                log.Error("Error: " + ex.Message + ex.StackTrace);
            }

            return response;
        }

        [Authorize]
        [HttpPost]
        [ActionName("UpdateAtlasCaseToGYB")]
        public ILResponse UpdateAtlasCaseToGYB([FromBody] IEnumerable<TransferredCaseData> transferredCases)
        {
            ILResponse response = new ILResponse();
            var context = new HttpContextWrapper(HttpContext.Current);

            try
            {
                CaseTransferService service = new CaseTransferService();
                service.UpdateAtlasCaseToGYB(transferredCases);

                response.MessageCode = MessageCodes.REQUEST_SUCCESS;
                response.MessageText = MSG_SUCCESS;
                response.MessageType = EnumMessageType.OPERATION_SUCCESS;
                response.HasException = false;

            }
            catch (Exception ex)
            {
                response.MessageCode = MessageCodes.REQUEST_ERROR;
                response.HasException = true;
                response.MessageText = ex.Message;
                response.MessageType = EnumMessageType.OPERATION_APPLICATION_ERROR;
                response.Exception = ex.ToString();

                log.Error("Error: " + ex.Message + ex.StackTrace);
            }

            return response;
        }

        [Authorize]
        [HttpPost]
        [ActionName("GetBillDocuments")]
        public ILResponse GetBillDocuments([FromBody]BillDetails bills)
        {
            ILResponse response = new ILResponse();
            string strBaseFilePath = null;
            byte[] Filebytes = null;
            List<FileData> lstActualData = new List<FileData>();
            try
            {
                CaseTransferService service = new CaseTransferService();
                IEnumerable<FileData> lstFileData = service.GetBillFiles(bills.billNumber, "");
                foreach (FileData data in lstFileData)
                {
                    try
                    {
                        strBaseFilePath = null;
                        Filebytes = null;
                        string[] SplitFilePath = data.FilePath.Replace('\\', '/').Split('/');
                        strBaseFilePath = data.PhysicalBasePath;
                        string NodeType = StringExtensions.GetNodeType(data.NodeType);
                        if (NodeType == "")
                            NodeType = "UNCATEGORIZED";


                        if (File.Exists(strBaseFilePath + data.FilePath))
                        {
                            Filebytes = File.ReadAllBytes(strBaseFilePath + data.FilePath);
                        }
                        FileData fileObject = new FileData();
                        fileObject.FileName = SplitFilePath[SplitFilePath.Length - 1];
                        fileObject.BasePathId = data.BasePathId;
                        fileObject.FilePath = data.FilePath.Replace('\\', '/');
                        fileObject.NodeType = NodeType;
                        fileObject.FileContent = Filebytes;

                        lstActualData.Add(fileObject);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error: " + ex.Message + ex.StackTrace);
                    }
                }

                service.UpdateBillStatus(bills.companyId, bills.billNumber, bills.LawFirmId);

                response.MessageCode = MessageCodes.REQUEST_SUCCESS;
                response.MessageText = MSG_SUCCESS;
                response.MessageType = EnumMessageType.OPERATION_SUCCESS;
                response.Add(lstActualData);
                response.HasException = false;
            }
            catch (Exception ex)
            {
                response.MessageCode = MessageCodes.REQUEST_ERROR;
                response.HasException = true;
                response.MessageText = ex.Message;
                response.MessageType = EnumMessageType.OPERATION_APPLICATION_ERROR;
                response.Exception = ex.ToString();
                log.Error("Error: " + ex.Message + ex.StackTrace);
            }
            return response;
        }

        [Authorize]
        [HttpPost]
        [ActionName("GetBillDocumentsWithoutContent")]
        public ILResponse GetBillDocumentsWithoutContent([FromBody]BillDetails bills)
        {
            ILResponse response = new ILResponse();
            DateTime startTime = DateTime.UtcNow;
            var context = new HttpContextWrapper(HttpContext.Current);
            string strBaseFilePath = null;
            List<FileData> lstActualData = new List<FileData>();

            try
            {
                CaseTransferService service = new CaseTransferService();
                IEnumerable<FileData> lstFileData = service.GetBillFiles(bills.billNumber, "");

                foreach (FileData data in lstFileData)
                {
                    try
                    {
                        strBaseFilePath = null;
                        string[] SplitFilePath = data.FilePath.Replace('\\', '/').Split('/');
                        strBaseFilePath = data.FilePath;
                        string NodeType = StringExtensions.GetNodeType(data.NodeType);
                        if (NodeType == "")
                            NodeType = "UNCATEGORIZED";

                        FileData fileObject = new FileData();
                        fileObject.FileName = SplitFilePath[SplitFilePath.Length - 1];
                        fileObject.FilePath = data.FilePath.Replace('\\', '/');
                        fileObject.NodeType = NodeType;
                        fileObject.BasePathId = data.BasePathId;
                        fileObject.PhysicalBasePath = data.PhysicalBasePath;
                        fileObject.BasePathType = data.BasePathType;
                        lstActualData.Add(fileObject);
                        

                    }
                    catch (Exception ex)
                    {
                        log.Error("Error while executing GetBillDocumentsWithoutContent : " + ex.Message + ex.StackTrace);
                    }
                }

                service.UpdateBillStatus(bills.companyId, bills.billNumber, bills.LawFirmId);

                response.MessageCode = MessageCodes.REQUEST_SUCCESS;
                response.MessageText = MSG_SUCCESS;
                response.MessageType = EnumMessageType.OPERATION_SUCCESS;
                response.Add(lstActualData);
                response.HasException = false;
            }
            catch (Exception ex)
            {
                response.MessageCode = MessageCodes.REQUEST_ERROR;
                response.HasException = true;
                response.MessageText = ex.Message;
                response.MessageType = EnumMessageType.OPERATION_APPLICATION_ERROR;
                response.Exception = ex.ToString();
                log.Error("Error while executing GetBillDocumentsWithoutContent : " + ex.Message + ex.StackTrace);
            }

            return response;
        }

        [Authorize]
        [HttpPost]
        [ActionName("GetAllBillDocumentsByCaseID")]
        public ILResponse GetAllBillDocumentsByCaseID([FromBody]BillDetails bills)
        {
            ILResponse response = new ILResponse();
            string strBaseFilePath = null;
            byte[] Filebytes = null;
            List<FileData> lstActualData = new List<FileData>();
            string NodeType = string.Empty;

            try
            {
                CaseTransferService service = new CaseTransferService();
                IEnumerable<FileData> lstFileData = service.GetAllBillDocumentsByCaseID(bills.billNumber);
                foreach (FileData data in lstFileData)
                {
                    try
                    {
                        strBaseFilePath = null;
                        Filebytes = null;
                        string[] SplitFilePath = data.FilePath.Replace('\\', '/').Split('/');
                        strBaseFilePath = data.PhysicalBasePath;

                        if (data.DocumentType == "BillDocuments")
                        {
                            NodeType = StringExtensions.GetNodeType(data.NodeType);

                            if (NodeType == "")
                            {
                                NodeType = "UNCATEGORIZED";
                            }
                        }
                        else
                        {
                            NodeType = data.NodeType;
                        }


                        if (File.Exists(strBaseFilePath + data.FilePath))
                        {
                            Filebytes = File.ReadAllBytes(strBaseFilePath + data.FilePath);
                        }
                        FileData fileObject = new FileData();
                        fileObject.FileName = SplitFilePath[SplitFilePath.Length - 1];
                        fileObject.BasePathId = data.BasePathId;
                        fileObject.FilePath = data.FilePath.Replace('\\', '/');
                        fileObject.NodeType = NodeType;
                        fileObject.FileContent = Filebytes;

                        lstActualData.Add(fileObject);
                        service.UpdateBillStatus(bills.companyId, bills.billNumber, bills.LawFirmId);

                    }
                    catch (Exception ex)
                    {
                        log.Error("Error: " + ex.Message + ex.StackTrace);
                    }
                }

                response.MessageCode = MessageCodes.REQUEST_SUCCESS;
                response.MessageText = MSG_SUCCESS;
                response.MessageType = EnumMessageType.OPERATION_SUCCESS;
                response.Add(lstActualData);
                response.HasException = false;
            }
            catch (Exception ex)
            {
                response.MessageCode = MessageCodes.REQUEST_ERROR;
                response.HasException = true;
                response.MessageText = ex.Message;
                response.MessageType = EnumMessageType.OPERATION_APPLICATION_ERROR;
                response.Exception = ex.ToString();
                log.Error("Error: " + ex.Message + ex.StackTrace);
            }
            return response;
        }

        [Authorize]
        [HttpPost]
        [ActionName("GetAllBillDocumentsByCaseIDWithoutContent")]
        public ILResponse GetAllBillDocumentsByCaseIDWithoutContent([FromBody]BillDetails bills)
        {
            ILResponse response = new ILResponse();
            DateTime startTime = DateTime.UtcNow;
            var context = new HttpContextWrapper(HttpContext.Current);
            List<FileData> lstActualData = new List<FileData>();
            string NodeType = string.Empty;

            try
            {
                CaseTransferService service = new CaseTransferService();
                IEnumerable<FileData> lstFileData = service.GetAllBillDocumentsByCaseID(bills.billNumber);

                foreach (FileData data in lstFileData)
                {
                    try
                    {
                        if (data.DocumentType == "BillDocuments")
                        {
                            NodeType = StringExtensions.GetNodeType(data.NodeType);

                            if (NodeType == "")
                            {
                                NodeType = "UNCATEGORIZED";
                            }
                        }
                        else
                        {
                            NodeType = data.NodeType;
                        }

                        string[] SplitFilePath = data.FilePath.Replace('\\', '/').Split('/');

                        FileData fileObject = new FileData();
                        fileObject.FileName = SplitFilePath[SplitFilePath.Length - 1];
                        fileObject.FilePath = data.FilePath.Replace('\\', '/');
                        fileObject.NodeType = NodeType;
                        fileObject.BasePathId = data.BasePathId;
                        fileObject.PhysicalBasePath = data.PhysicalBasePath;
                        fileObject.BasePathType = data.BasePathType;
                        fileObject.DocumentType = data.DocumentType;
                        lstActualData.Add(fileObject);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error: " + ex.Message + ex.StackTrace);
                    }
                }

                service.UpdateBillStatus(bills.companyId, bills.billNumber, bills.LawFirmId);

                response.MessageCode = MessageCodes.REQUEST_SUCCESS;
                response.MessageText = MSG_SUCCESS;
                response.MessageType = EnumMessageType.OPERATION_SUCCESS;
                response.Add(lstActualData);
                response.HasException = false;
            }
            catch (Exception ex)
            {
                response.MessageCode = MessageCodes.REQUEST_ERROR;
                response.HasException = true;
                response.MessageText = ex.Message;
                response.MessageType = EnumMessageType.OPERATION_APPLICATION_ERROR;
                response.Exception = ex.ToString();
                log.Error("Error: " + ex.Message + ex.StackTrace);
            }

            return response;
        }

        [Authorize]
        [HttpGet]
        [ActionName("GetNonAtlasLwfirms")]
        public ILResponse GetNonAtlasLwfirms()
        {
            ILResponse response = new ILResponse();
            try
            {
                CaseTransferService service = new CaseTransferService();
                var lawfirms = service.GetNonAtlasLwfirms();

                response.MessageCode = MessageCodes.REQUEST_SUCCESS;
                response.MessageText = MSG_SUCCESS;
                response.MessageType = EnumMessageType.OPERATION_SUCCESS;
                response.Add(lawfirms);
                response.HasException = false;
            }
            catch (Exception ex)
            {
                response.MessageCode = MessageCodes.REQUEST_ERROR;
                response.HasException = true;
                response.MessageText = ex.Message;
                response.MessageType = EnumMessageType.OPERATION_APPLICATION_ERROR;
                response.Exception = ex.ToString();
                log.Error("Error: " + ex.Message + ex.StackTrace);
            }

            return response;
        }

        [Authorize]
        [HttpGet]
        [ActionName("GetAtlasLawfirms")]
        public ILResponse GetAtlasLawfirms()
        {
            ILResponse response = new ILResponse();

            try
            {
                CaseTransferService service = new CaseTransferService();
                var lawfirms = service.GetAtlasLawfirms();

                response.MessageCode = MessageCodes.REQUEST_SUCCESS;
                response.MessageText = MSG_SUCCESS;
                response.MessageType = EnumMessageType.OPERATION_SUCCESS;
                response.Add(lawfirms);
                response.HasException = false;
            }
            catch (Exception ex)
            {
                response.MessageCode = MessageCodes.REQUEST_ERROR;
                response.HasException = true;
                response.MessageText = ex.Message;
                response.MessageType = EnumMessageType.OPERATION_APPLICATION_ERROR;
                response.Exception = ex.ToString();
                log.Error("Error: " + ex.Message + ex.StackTrace);
            }

            return response;
        }

        [Authorize]
        [HttpGet]
        [ActionName("GetLawFirmCaseTransferBatch")]
        public ILResponse GetLawFirmCaseTransferBatch(string lawfirmid)
        {
            ILResponse response = new ILResponse();

            try
            {
                CaseTransferService service = new CaseTransferService();
                var batches = service.GetLawFirmCaseTransferBatch(lawfirmid);

                response.MessageCode = MessageCodes.REQUEST_SUCCESS;
                response.MessageText = MSG_SUCCESS;
                response.MessageType = EnumMessageType.OPERATION_SUCCESS;
                response.Add(batches);
                response.HasException = false;
            }
            catch (Exception ex)
            {
                response.MessageCode = MessageCodes.REQUEST_ERROR;
                response.HasException = true;
                response.MessageText = ex.Message;
                response.MessageType = EnumMessageType.OPERATION_APPLICATION_ERROR;
                response.Exception = ex.ToString();
                log.Error("Error: " + ex.Message + ex.StackTrace);
            }

            return response;
        }

        [Authorize]
        [HttpGet]
        [ActionName("GetTransferredCasesByBatch")]
        public ILResponse GetTransferredCasesByBatch(string lawfirmid, string batchid)
        {
            ILResponse response = new ILResponse();
            
            try
            {
                CaseTransferService service = new CaseTransferService();
                var cases = service.GetTransferredCasesByBatch(lawfirmid, batchid);

                response.MessageCode = MessageCodes.REQUEST_SUCCESS;
                response.MessageText = MSG_SUCCESS;
                response.MessageType = EnumMessageType.OPERATION_SUCCESS;
                response.Add(cases);
                response.HasException = false;
            }
            catch (Exception ex)
            {
                response.MessageCode = MessageCodes.REQUEST_ERROR;
                response.HasException = true;
                response.MessageText = ex.Message;
                response.MessageType = EnumMessageType.OPERATION_APPLICATION_ERROR;
                response.Exception = ex.ToString();
                log.Error("Error: " + ex.Message + ex.StackTrace);
            }

            return response;
        }

        [Authorize]
        [HttpPost]
        [ActionName("UpdateTranferredBatchStatus")]
        public ILResponse UpdateTranferredBatchStatus([FromBody] TransferredBatch batch)
        {
            ILResponse response = new ILResponse();
            
            try
            {
                CaseTransferService service = new CaseTransferService();
                service.UpdateTranferredBatchStatus(batch.LawfirmID, batch.BatchID, batch.FileUploadBasePath, batch.ZipFileName, batch.BasePathID);

                response.MessageCode = MessageCodes.REQUEST_SUCCESS;
                response.MessageText = MSG_SUCCESS;
                response.MessageType = EnumMessageType.OPERATION_SUCCESS;
                response.Add("Batch Updated Successfully");
                response.HasException = false;
            }
            catch (Exception ex)
            {
                response.MessageCode = MessageCodes.REQUEST_ERROR;
                response.HasException = true;
                response.MessageText = ex.Message;
                response.MessageType = EnumMessageType.OPERATION_APPLICATION_ERROR;
                response.Exception = ex.ToString();
                log.Error("Error: " + ex.Message + ex.StackTrace);
            }

            return response;
        }

        [Authorize]
        [HttpPost]
        [ActionName("GetReconcillationReport")]
        public ILResponse GetReconcillationReport([FromUri] string lawFirmCaseManagerAppName)
        {
            ILResponse response = new ILResponse();
           
            try
            {
                CaseTransferService service = new CaseTransferService();
                var recon = service.GetReconcillationReport(lawFirmCaseManagerAppName);

                response.MessageCode = MessageCodes.REQUEST_SUCCESS;
                response.MessageText = MSG_SUCCESS;
                response.MessageType = EnumMessageType.OPERATION_SUCCESS;
                response.HasException = false;
                response.Add(recon);

            }
            catch (Exception ex)
            {
                response.MessageCode = MessageCodes.REQUEST_ERROR;
                response.HasException = true;
                response.MessageText = ex.Message;
                response.MessageType = EnumMessageType.OPERATION_APPLICATION_ERROR;
                response.Exception = ex.ToString();

                log.Error("Error: " + ex.Message + ex.StackTrace);
            }

            return response;
        }

        [Authorize]
        [ActionName("UploadFile")]
        [HttpPost]
        public ILResponse UploadFile([FromBody]FileData fileObject)
        {
            ILResponse response = new ILResponse();
            DateTime startTime = DateTime.UtcNow;
            var context = new HttpContextWrapper(HttpContext.Current);

            if (fileObject == null
                || fileObject.FileName == string.Empty || fileObject.FileName == null
                || fileObject.FilePath == string.Empty || fileObject.FilePath == null
                || fileObject.FileContent == null || fileObject.FileContent.Length == 0)
            {
                response.MessageCode = MessageCodes.REQUEST_INCOMPLETE;
                response.HasException = true;
                response.MessageText = MSG_FAILED;
                response.MessageType = EnumMessageType.OPERATION_TECHNICAL_ERROR;
                response.Exception = "Bad request. Request contains no data";

                return response;
            }

            try
            {
                CaseTransferService service = new CaseTransferService();

                string pyisicalBasePath = service.GetParameterValue("PhysicalBasePath");

                if (!Directory.Exists(pyisicalBasePath + fileObject.FilePath))
                {
                    Directory.CreateDirectory(pyisicalBasePath + fileObject.FilePath);
                }
                File.WriteAllBytes(pyisicalBasePath + fileObject.FilePath + fileObject.FileName, fileObject.FileContent);

                response.MessageCode = "1";
                response.TransactionID = "900000000000001";
                response.MessageText = MSG_SUCCESS;
                response.MessageType = EnumMessageType.OPERATION_SUCCESS;
                response.Add(service.GetParameterValue("BasePathId"));
                response.HasException = false;

                return response;
            }
            catch (Exception ex)
            {
                response.HasException = true;
                response.MessageText = ex.Message;
                response.MessageType = EnumMessageType.OPERATION_APPLICATION_ERROR;
                response.Exception = ex.ToString();
                log.Error("Error: " + ex.Message + ex.StackTrace);

                return response;
            }
        }

        [AllowAnonymous]
        [ActionName("DownloadFile")]
        [HttpPost]
        public HttpResponseMessage DownloadFile([FromBody] FileDownloadModel fileObject)
        {
            HttpResponseMessage response;
            CaseTransferService service = new CaseTransferService();
            try
            {
                string inputfilePath = Encoding.UTF8.GetString(Convert.FromBase64String(fileObject.FilePath));
                string basePathID = Encoding.UTF8.GetString(Convert.FromBase64String(fileObject.BasePathId));
                string filePath = string.Empty;

                if (fileObject.UseCustomBasePath)
                {
                    string customPhysicalBasePath = Encoding.UTF8.GetString(Convert.FromBase64String(fileObject.CustomPhysicalBasePath));
                    filePath = customPhysicalBasePath + inputfilePath;
                }
                else
                {
                    filePath = service.GetParameterValue("PhysicalBasePath", Convert.ToString(basePathID))
                        + inputfilePath;
                }

                if (File.Exists(filePath))
                {
                    var byteArray = File.ReadAllBytes(filePath);

                    var dataStream = new MemoryStream(byteArray);

                    response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StreamContent(dataStream);
                    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = Path.GetFileName(inputfilePath);
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                return response;
            }
            catch (Exception ex)
            {
                log.Error("Error: " + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        [Authorize]
        [HttpPost]
        [ActionName("GetNotAtlasClientsBillData")]
        public ILResponse GetNotAtlasClientsBillData([FromBody] KeyValuePair<string, string> BillNumbers)
        {
            ILResponse response = new ILResponse();
            DateTime startTime = DateTime.UtcNow;
            var context = new HttpContextWrapper(HttpContext.Current);

            try
            {
                CaseTransferService service = new CaseTransferService();
                var data = service.GetNotAtlasClientsBillData(BillNumbers.Value);

                response.MessageCode = MessageCodes.REQUEST_SUCCESS;
                response.MessageText = MSG_SUCCESS;
                response.MessageType = EnumMessageType.OPERATION_SUCCESS;
                response.Add(data);
                response.HasException = false;
            }
            catch (Exception ex)
            {
                response.MessageCode = MessageCodes.REQUEST_ERROR;
                response.HasException = true;
                response.MessageText = ex.Message;
                response.MessageType = EnumMessageType.OPERATION_APPLICATION_ERROR;
                response.Exception = ex.ToString();
                log.Error("Error: " + ex.Message + ex.StackTrace);
            }

            return response;
        }

        [Authorize]
        [HttpGet]
        [ActionName("GetWithdrawnCases")]
        public ILResponse GetWithdrawnCases(string lawfirmid)
        {
            ILResponse response = new ILResponse();
            DateTime startTime = DateTime.UtcNow;
            var context = new HttpContextWrapper(HttpContext.Current);

            try
            {
                CaseTransferService service = new CaseTransferService();
                var cases = service.GetWithdrawnCases(lawfirmid);

                response.MessageCode = MessageCodes.REQUEST_SUCCESS;
                response.MessageText = MSG_SUCCESS;
                response.MessageType = EnumMessageType.OPERATION_SUCCESS;
                response.Add(cases);
                response.HasException = false;
            }
            catch (Exception ex)
            {
                response.MessageCode = MessageCodes.REQUEST_ERROR;
                response.HasException = true;
                response.MessageText = ex.Message;
                response.MessageType = EnumMessageType.OPERATION_APPLICATION_ERROR;
                response.Exception = ex.ToString();
                log.Error("Error while executing GetWithdrawnCases : " + ex.Message + ex.StackTrace);
            }

            return response;
        }

        [Authorize]
        [HttpPost]
        [ActionName("UpdateWithdrawnCases")]
        public ILResponse UpdateWithdrawnCases([FromBody]List<WithdrawnCase> withdrawnCases)
        {
            ILResponse response = new ILResponse();
            DateTime startTime = DateTime.UtcNow;
            var context = new HttpContextWrapper(HttpContext.Current);

            try
            {
                CaseTransferService service = new CaseTransferService();
                service.UpdateWithdrawnCases(withdrawnCases);

                response.MessageCode = MessageCodes.REQUEST_SUCCESS;
                response.MessageText = MSG_SUCCESS;
                response.MessageType = EnumMessageType.OPERATION_SUCCESS;
                response.Add("Cases withdrawn successfully");
                response.HasException = false;
            }
            catch (Exception ex)
            {
                response.MessageCode = MessageCodes.REQUEST_ERROR;
                response.HasException = true;
                response.MessageText = ex.Message;
                response.MessageType = EnumMessageType.OPERATION_APPLICATION_ERROR;
                response.Exception = ex.ToString();
                log.Error("Error while executing UpdateWithdrawnCases : " + ex.Message + ex.StackTrace);
            }

            return response;
        }

        [Authorize]
        [HttpGet]
        [ActionName("NotifyAttorneyCaseWithdrawl")]
        public ILResponse NotifyAttorneyCaseWithdrawl()
        {
            ILResponse response = new ILResponse();
            DateTime startTime = DateTime.UtcNow;
            var context = new HttpContextWrapper(HttpContext.Current);

            try
            {
                CaseTransferService service = new CaseTransferService();
                service.NotifyAttorneyCaseWithdrawl();

                response.MessageCode = MessageCodes.REQUEST_SUCCESS;
                response.MessageText = MSG_SUCCESS;
                response.MessageType = EnumMessageType.OPERATION_SUCCESS;
                response.Add("Cases withdrawn notification sent successfully");
                response.HasException = false;
            }
            catch (Exception ex)
            {
                response.MessageCode = MessageCodes.REQUEST_ERROR;
                response.HasException = true;
                response.MessageText = ex.Message;
                response.MessageType = EnumMessageType.OPERATION_APPLICATION_ERROR;
                response.Exception = ex.ToString();
                log.Error("Error while executing NotifyAttorneyCaseWithdrawl : " + ex.Message + ex.StackTrace);
            }

            return response;
        }

        [Authorize]
        [HttpGet]
        [ActionName("NotifyGYBCaseTransfer")]
        public ILResponse NotifyGYBCaseTransfer()
        {
            ILResponse response = new ILResponse();
            DateTime startTime = DateTime.UtcNow;
            var context = new HttpContextWrapper(HttpContext.Current);

            try
            {
                CaseTransferService service = new CaseTransferService();
                service.NotifyGYBCaseTransfer();

                response.MessageCode = MessageCodes.REQUEST_SUCCESS;
                response.MessageText = MSG_SUCCESS;
                response.MessageType = EnumMessageType.OPERATION_SUCCESS;
                response.Add("Cases transfer notification sent successfully");
                response.HasException = false;
            }
            catch (Exception ex)
            {
                response.MessageCode = MessageCodes.REQUEST_ERROR;
                response.HasException = true;
                response.MessageText = ex.Message;
                response.MessageType = EnumMessageType.OPERATION_APPLICATION_ERROR;
                response.Exception = ex.ToString();
                log.Error("Error while executing NotifyGYBCaseTransfer : " + ex.Message + ex.StackTrace);
            }

            return response;
        }

        [Authorize]
        [HttpGet]
        [ActionName("GetCaseDocumentList")]
        public ILResponse GetCaseDocumentList(string caseId, string companyId)
        {
            ILResponse response = new ILResponse();
            try
            {
                CaseTransferService service = new CaseTransferService();
                var documents = service.GetCaseDocumentList(caseId, companyId);

                response.MessageCode = MessageCodes.REQUEST_SUCCESS;
                response.MessageText = MSG_SUCCESS;
                response.MessageType = EnumMessageType.OPERATION_SUCCESS;
                response.Add(documents);
                response.HasException = false;
            }
            catch (Exception ex)
            {
                response.MessageCode = MessageCodes.REQUEST_ERROR;
                response.HasException = true;
                response.MessageText = ex.Message;
                response.MessageType = EnumMessageType.OPERATION_APPLICATION_ERROR;
                response.Exception = ex.ToString();
                log.Error("Error while executing GetCaseDocumentList : " + ex.Message + ex.StackTrace);
            }

            return response;
        }
    }
}
