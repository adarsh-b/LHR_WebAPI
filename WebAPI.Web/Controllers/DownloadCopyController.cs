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
    public class DownloadCopyController : ApiController
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
                (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // POST: api/DownloadCopy
        public IHttpActionResult Post(IEnumerable<BillDetails> lstBillDetails, [FromUri] string domainType)
        {

            byte[] fileBytes = null;
            FtpWebRequest request = null;
            FtpWebRequest request1 = null;
            FtpWebRequest request2 = null;
            GoGreenService service = new GoGreenService();
            List<BillDetails> lst = new List<BillDetails>();
            List<BillDetails> downloadedBills = new List<BillDetails>();
            List<FileData> lstActualData = new List<FileData>();
            try
            {

                foreach (BillDetails bills in lstBillDetails)
                {
                    IEnumerable<FileData> lstFileData = service.GetBillFiles(bills.billNumber, "");
                    foreach (FileData data in lstFileData)
                    {
                        try
                        {
                            string NodeType = StringExtensions.GetNodeType(data.NodeType);
                            if (NodeType == "")
                                NodeType = "UNCATEGORIZED";
                            if (!string.IsNullOrEmpty(NodeType))
                            {
                                lstActualData.Add(data);

                                Users user = StringExtensions.GetUserName(data.BasePathId);
                                request = (FtpWebRequest)WebRequest.Create(new Uri(ConfigurationManager.AppSettings["GoGybDocuments"].ToString() + data.FilePath));
                                //request.Credentials = new NetworkCredential("ramu.g", "india@90");
                                //request.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["GoGybusername1"].ToString(),
                                //    ConfigurationManager.AppSettings["GoGybpassword1"].ToString());

                                request.Credentials = new NetworkCredential(user.UserName, user.Password);

                                request.Proxy = null;
                                request.KeepAlive = false;
                                request.Method = WebRequestMethods.Ftp.DownloadFile;
                                using (var resp = (FtpWebResponse)request.GetResponse())
                                {
                                    //Read the FileName and convert it to Byte array.
                                    using (StreamReader fileStream = new StreamReader(resp.GetResponseStream()))
                                    {
                                        fileBytes = Encoding.UTF8.GetBytes(fileStream.ReadToEnd());
                                        fileStream.Close();
                                    }
                                }



                                ClientFTPDetalis ftpdetails = StringExtensions.GetClientFTPDetalis(bills.DomainId);
                                string DestinationPath;
                                string domainPath;

                                if (ftpdetails.Path == "")
                                {
                                    DestinationPath = ftpdetails.FtpAddress +
                                     DateTime.Today.ToString("yyyy-MM-dd").Replace("-", "/") + "/" + bills.caseId + "/";
                                    domainPath = DateTime.Today.ToString("yyyy-MM-dd").Replace("-", "/") + "/" + bills.caseId + "/";
                                }
                                else
                                {
                                    DestinationPath = ftpdetails.FtpAddress + bills.DomainId + "/" +
                                     DateTime.Today.ToString("yyyy-MM-dd").Replace("-", "/") + "/"+ bills.caseId + "/";

                                    domainPath = bills.DomainId + "/" +
                                         DateTime.Today.ToString("yyyy-MM-dd").Replace("-", "/") + "/" + bills.caseId + "/";
                                }



                                string[] folderArray = domainPath.Split('/');
                                string folderName = "";
                                for (int i = 0; i < folderArray.Length; i++)
                                {
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(folderArray[i]))
                                        {
                                            folderName = string.IsNullOrEmpty(folderName) ? folderArray[i] : folderName + "/" + folderArray[i];
                                            FtpWebRequest request4 = (FtpWebRequest)WebRequest.Create(ftpdetails.FtpAddress + folderName);
                                            request4.Method = WebRequestMethods.Ftp.MakeDirectory;
                                            //request4.Credentials = new NetworkCredential("atlasuser", "india@90");
                                            request4.Credentials = new NetworkCredential(ftpdetails.UserName, ftpdetails.Password);
                                            var response = request4.GetResponse();
                                            response.Close();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }

                                }

                                try
                                {
                                    request2 = (FtpWebRequest)WebRequest.Create(new Uri(ftpdetails.FtpAddress + domainPath + "//" +
                                       Path.GetFileName(ftpdetails.FtpAddress + data.FilePath)));
                                    request2.Method = WebRequestMethods.Ftp.UploadFile;
                                    // request2.Credentials = new NetworkCredential("atlasuser", "india@90");
                                    request2.Credentials = new NetworkCredential(ftpdetails.UserName, ftpdetails.Password);
                                    request2.ContentLength = fileBytes.Length;
                                    request2.UsePassive = true;
                                    request2.UseBinary = true;
                                    request2.ServicePoint.ConnectionLimit = fileBytes.Length;
                                    request2.EnableSsl = false;

                                    using (Stream requestStream = request2.GetRequestStream())
                                    {
                                        requestStream.Write(fileBytes, 0, fileBytes.Length);
                                        requestStream.Close();
                                    }
                                    FtpWebResponse response1 = (FtpWebResponse)request2.GetResponse();
                                    response1.Close();
                                }
                                catch (Exception ex)
                                {
                                }

                                if (bills.Filelist == null)
                                {
                                    bills.Filelist = new List<FileData>();
                                }

                                bills.Filelist.Add(new FileData
                                {
                                    NodeType = NodeType,
                                    FileName = Path.GetFileName(ftpdetails.FtpAddress + data.FilePath),
                                    PhysicalBasePath = domainPath + "\\" + Path.GetFileName(ftpdetails.FtpAddress + data.FilePath)
                                });

                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    if (bills.Filelist == null)
                    {
                        bills.Filelist = new List<FileData>();
                    }

                    if (bills.Filelist.Count == lstActualData.Count)
                    {
                        service.UpdateBillStatus(bills.companyId, bills.billNumber, bills.LawFirmId);
                        downloadedBills.Add(bills);
                    }
                }

                return Ok(downloadedBills);
            }
            catch (Exception ex)
            {
                log.Error("Error" + ex.Message + ex.StackTrace);
                return Ok(downloadedBills);
            }
        }
    }
}
