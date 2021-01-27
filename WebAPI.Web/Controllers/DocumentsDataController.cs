using CaseTranswerAPI.Service.GoGreen;
using CaseTranswerAPI.Models.GoGreen;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Http;
using CaseTranswerAPI.Utility.Extensions;

namespace CaseTranswerAPI.Web.Controllers
{
    [Authorize]

    public class DocumentsDataController : ApiController
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
                (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // POST: api/DownloadCopy
        public IHttpActionResult Post([FromBody]BillDetails bills, [FromUri] string domainType)
        {

           
            //List<BillDetails> lst = new List<BillDetails>();
            //List<BillDetails> downloadedBills = new List<BillDetails>();
            string strBaseFilePath = null;
            byte[] Filebytes = null;
            List<FileData> lstActualData = new List<FileData>();
            try
            {
                GoGreenService service = new GoGreenService();
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
                        service.UpdateBillStatus(bills.companyId, bills.billNumber, bills.LawFirmId);

                    }
                    catch (Exception ex)
                    {
                        log.Error("Error" + ex.Message + ex.StackTrace);
                    }
                }


                return Ok(lstActualData);
            }
            catch (Exception ex)
            {
                log.Error("Error" + ex.Message + ex.StackTrace);
                return Ok(lstActualData);
            }
        }
    }
}
