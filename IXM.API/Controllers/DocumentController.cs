using IXM.DB;
using IXM.Models;
using IXM.Models.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IXM.API.Services;
using IXM.Common;
using IXM.Constants;
using Microsoft.AspNetCore.StaticFiles;
using IXM.DB.Services;
using IXM.GeneralSQL;
using IXM.Common.Constant;
using DocumentFormat.OpenXml.Drawing.Charts;
//using MassTransit;

namespace IXM.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IXMDBContext _context;
        private readonly IIXMDBRepo _dbrepo;
        private readonly IDataService _dataservice;
        private readonly IDataValidator _datavalidator;
        private readonly IConfiguration _configuration;
        private readonly IIXMCommonRepo _commonrepo;
        private readonly IIXMDocumentRepo _docrepo;
        private readonly IGeneralSQL _generalSQL;
        private readonly IIXMTransactionRepo _transrepo;
        //private readonly IMessagePublish _messagePublish;
        private readonly IXMDBIdentity _identitycontext;
        private readonly ILogger<DocumentController> _logger;
        public GenFunctions genFunctions = new GenFunctions();
        private readonly CancellationToken cancellationToken = new CancellationToken();
        //      private readonly IPublishEndpoint _publishEndpoint;

        public DocumentController(IXMDBContext context,
                                    IIXMDBRepo dbrepo,
                                    IIXMTransactionRepo transrepo,
                                    //IPublishEndpoint publishEndpoint,
                                    IIXMDocumentRepo docrepo,
                                    IXMDBIdentity identity,
                                    ILogger<DocumentController> logger,
                                    IDataService dataservice,
                                    //IMessagePublish messagePublish,
                                    IGeneralSQL generalSQL,
                                    IDataValidator datavalidator,
                                    IConfiguration configuration,
                                    IIXMCommonRepo commonRepo)
        {
            _context = context;
            _dbrepo = dbrepo;
            _identitycontext = identity;
            _logger = logger;
            _docrepo = docrepo;
            _generalSQL = generalSQL;
            _datavalidator = datavalidator;
            _dataservice = dataservice;
            _configuration = configuration;
            _commonrepo = commonRepo;
            //_publishEndpoint = publishEndpoint;
            _transrepo = transrepo;
            //_messagePublish = messagePublish;
        }


        [Authorize]
        [HttpGet("Company")]
        public IActionResult GetCompanyDocuments(string pCompanyId)
        {
            try
            {
                var prData = _context.MOBJECT_DOC_API.FromSqlRaw<MOBJECT_DOC_API>(_dataservice.GetPureDocuments(pCompanyId)).ToList();
                var BaseDocFolder = _configuration.GetConnectionString("BaseDocFolder");
                var Baseurl = _configuration.GetConnectionString("BaseURL");

                if (prData.Count() > 0)
                {
                    prData.ForEach(b =>
                    {

                        var prDocType = _context.MCODE.FromSqlRaw<MCODE>($"SELECT MCODE.*,'' DESCRIPTION FROM MCODE WHERE CODE_VALUE = @p0", b.DOCTYPE.ToString()).ToList();
                        b.DOCFILEURI = Baseurl.ToString() + "APPFILES/" + prDocType.First().CODE_FPATH.ToString() + "/" + b.DOCUMENTNAME.ToString();
                        b.CODE_TEXT = prDocType.First().CODE_TEXT.ToString();

                    });
                }

                return prData == null ? NotFound() : Ok(prData);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }



        [Authorize]
        [HttpGet("FileIdDownload")]
        public async Task<IActionResult> GetFileIDDocument(IxmAppDocumentType FileType, string FileID, string SystemID)
        {
            try
            {
                var pSystem = _identitycontext.MSYSTEM.Where(a => a.SYSTEMID == SystemID).First();

                if (pSystem == null)
                {
                    return BadRequest("System Improper");
                }
                try
                {

                    var prData = _context.MOBJECT_DOC.FromSqlRaw<MOBJECT_DOC>(_generalSQL.GetDocumentByObjectId(FileID)).ToList();
                    var lFName = prData.First().SFOLDERNAME;


                    if (FileType == IxmAppDocumentType.XLS_REMITTANCEERR)
                    {
                        lFName = lFName + prData.First().DOCUMENTNAME;
                    }
                    //Determine the Content Type of the File.
                    string contentType = "";
                    new FileExtensionContentTypeProvider().TryGetContentType(lFName, out contentType);
                    //Read the File data into FileStream.
                    FileStream fileStream = new FileStream(lFName, FileMode.Open, FileAccess.Read);

                    //Send the File to Download.
                    return new FileStreamResult(fileStream, contentType);

                }
                catch (Exception e)
                {

                    return NotFound("Error :: " + e.Message);
                }

            }
            catch (Exception ex)
            {
                return NotFound("File Not Found ");
            }



        }

        [AllowAnonymous]
        [HttpGet("FileIdTransfer")]
        public async Task<IActionResult> TransferFileIDDocument(string FileID, string SystemID)
        {
            CancellationToken stoppingToken = new CancellationToken();
            try
            {
                var pSystem = _identitycontext.MSYSTEM.Where(a => a.SYSTEMID == SystemID).First();
                if (pSystem == null)
                {
                    return BadRequest("System Improper");
                }
                try
                {

                    var prHeader = _context.TRMBL.ReadModel().Where(a => a.OBJECTID == Convert.ToInt32(FileID))
                                                           .Single();

                    var lReturn1 = _transrepo.Transaction.TransferRemittanceToServer(SystemID, FileID, prHeader.PERIODID, prHeader, prHeader.MODBY, "");
                    {
                        ///Error Messaging
                    }
                    

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }catch (Exception ex){
            }

            return Ok();
        }


        [Authorize]
        [HttpGet("FileNameDownload")]
        public async Task<IActionResult> GetFileNameDocument(string SystemId, IxmAppDocumentType DocType, IxmAppTemplateFileType FileType, int PeriodId, int CompanyId)
        {

            string PhysicalFileName = "";

            ApplicationUser usr = await _dbrepo.General.GetCurrentUserAsync(HttpContext);
            if (usr == null)
            {
                return NotFound("File Not Found ");
            }


            try
            {
                var pSystem = await _identitycontext.MSYSTEM.Where(a => a.SYSTEMID == SystemId).FirstAsync();

                if (pSystem == null)
                {
                    return BadRequest("System Improper");
                }

                var BaseDocFolder = _configuration.GetConnectionString("BaseDocFolder");
                var lSystemType = "Live";

                _logger.LogInformation("File Type : {@0}, DocType {@1}, Bas Folder {@2}", FileType.ToString(), DocType.ToString(), BaseDocFolder);

                var path = genFunctions.GetTemplateDocument(pSystem.SYSTEMNAME, DocType, FileType, lSystemType, BaseDocFolder);

                PhysicalFileName = path.Item2;

                PhysicalFileName = PhysicalFileName.Replace("APPFILES/", "");

                string contentType = "";

                _logger.LogInformation("File name : {@0}", PhysicalFileName);

                //Stream fileStream;
                MemoryStream fileStream = new MemoryStream();
                //= new MemoryStream();

                if ((DocType == IxmAppDocumentType.XLS_TEMPLATE) && (FileType == IxmAppTemplateFileType.Schedule) && (PeriodId > 0))
                {

                    var fileResult = await _dbrepo.FileIngest.XlsExportRemittanceContentInfo(usr.SYSTEM_UNAME, PhysicalFileName, PeriodId, CompanyId, SystemId);
                    if (fileResult != null)
                    {
                        //var fileS = new MemoryStream(fileResult.GetBuffer(), 0, (int)fileResult.Length);
                        fileResult.CopyTo(fileStream);
                    }


                } else
                {
                    var fileS = new FileStream(PhysicalFileName, FileMode.Open, FileAccess.Read);
                    fileS.CopyTo(fileStream);

                }




                var _oimage = _commonrepo.FileService.GetDocumentData(PhysicalFileName);
                new FileExtensionContentTypeProvider().TryGetContentType(PhysicalFileName, out contentType);

                _logger.LogInformation("File Status : {@0}", _oimage);

                if (_oimage.Item1 == -1)
                {

                    _logger.LogInformation("File not found : {@0}", PhysicalFileName);
                    return NotFound("File Not Found 1 " + PhysicalFileName);

                }
                else if (_oimage.Item1 == 0)
                {

                    _logger.LogInformation("File found 0 : {@0}", PhysicalFileName);
                    //return new FileStreamResult(fileStream, contentType);
                    return File(fileStream.ToArray(), contentType);

                }
                else return BadRequest();


            }
            catch (Exception ex)
            {
                return NotFound("File Error :: " + ex.Message);
            }



        }



        [AllowAnonymous]
        [HttpGet("ReportDownload")]
        public async Task<IActionResult> GetReportDocument(string SystemId, int PeriodId, int? ReportId, IxmStaticReports? TReport)
        {

            string PhysicalFileName = "";

            ApplicationUser usr = await _dbrepo.General.GetCurrentUserAsync(HttpContext);
            if (usr == null)
            {
                //return NotFound("Invalid Report Request");
            }


            try
            {
                var pSystem = await _identitycontext.MSYSTEM.Where(a => a.SYSTEMID == SystemId).FirstAsync();

                if (pSystem == null)
                {
                    return BadRequest("System Improper");
                }

                var pReport = new MREPORT();

                if (TReport != null)
                {


                    var Lrep = (IxmStaticReports)Enum.ToObject(typeof(IxmStaticReports), TReport.Value); //Wednesday

                    var p1 = Lrep.ToString();
                    string p2 = TReport.ToString();

                    string lS = nameof(TReport.Value);
                    pReport = await _context.MREPORT.Where(a => a.TECHNICALNAME == p2).FirstAsync();

                }
                else
                {
                    pReport = await _context.MREPORT.Where(a => a.REPORTID == ReportId).FirstAsync();

                }




                if (pReport == null)
                {
                    return BadRequest("Invalid Report Request");
                }

                var pPeriod = await _context.MPERIOD.Where(a => a.PRID == PeriodId).FirstAsync();


                if (pReport == null)
                {
                    return BadRequest("Invalid Report Request");
                }



                var BaseDocFolder = _configuration.GetConnectionString("BaseDocFolder");
                var lSystemType = "Live";
                
                PhysicalFileName = PhysicalFileName.Replace("APPFILES/", "");

                string contentType = "";

                var path = genFunctions.GetTemplateDocument(pSystem.SYSTEMNAME, IxmAppDocumentType.XLS_REPORT , IxmAppTemplateFileType.Report, lSystemType, BaseDocFolder);

                PhysicalFileName = path.Item2;

                if (pReport.CRT_PERIOD == 1)
                {
                    PhysicalFileName = PhysicalFileName + pReport.FILENAME;
                    PhysicalFileName = PhysicalFileName.Replace("[PERIOD]", pPeriod.MYEARMONTH);
                }


                _logger.LogInformation("File name : {@0}", PhysicalFileName);

                //Stream fileStream;
                //MemoryStream fileStream = new MemoryStream();
                //= new MemoryStream();


                new FileExtensionContentTypeProvider().TryGetContentType(PhysicalFileName, out contentType);

                //Read the File data into FileStream.
                FileStream fileStream = new FileStream(PhysicalFileName, FileMode.Open, FileAccess.Read);

                //Send the File to Download.
                return new FileStreamResult(fileStream, contentType);

                /*
                var _oimage = _commonrepo.FileService.GetDocumentData(PhysicalFileName);

                _logger.LogInformation("File Status : {@0}", _oimage);

                if (_oimage.Item1 == -1)
                {

                    _logger.LogInformation("File not found : {@0}", PhysicalFileName);
                    return NotFound("File Not Found 1 " + PhysicalFileName);

                }
                else if (_oimage.Item1 == 0)
                {

                    _logger.LogInformation("File found 0 : {@0} : {@1}", PhysicalFileName,_oimage. );
                    //return new FileStreamResult(fileStream, contentType);
                    return File(fileStream.ToArray(), contentType);

                }
                else return BadRequest();*/


            }
            catch (Exception ex)
            {
                return NotFound("File Error :: " + ex.Message);
            }



        }


        [Authorize]
        [HttpPost("FileUpload")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> FileUpload([FromForm] MOBJECT_DOC model)
        {
            _logger.LogInformation("FileUpload...Getting started");
            var status = new IxmReturnStatus();
            int lPeriodId = -1;

            ApplicationUser usr = await _dbrepo.General.GetCurrentUserAsync(HttpContext);

            _logger.LogInformation("FileUpload...Getting user {@usr}", usr);
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.StatusMessage = "Please pass a valid dataset with your request.";
                _logger.LogTrace(status.StatusMessage);
                return Ok(status);

            }

            if (usr != null)
            {
                _logger.LogInformation("FileUpload Step 2 on Status: {@status}", status);
                //int _organizerresult = -1;
                string lNewFIle = "";


                if (model.ORGIMAGEFILE != null)
                {
                    var _fileresult = await _commonrepo.FileService.SaveDocumentToFileSystem(model, model.ORGIMAGEFILE);
                    if ((_fileresult.Item1 == 1) && (model != null))
                    {


                        await _dbrepo.AppLog.LogTotableF(IxmAppLogType.LogSystem, GetSourcObject(model.DOCTYPE), model.SYSTEMID, model.SOURCEID.ToString(),
                                                            "File Saved to Server : " + _fileresult.Item2 + " File Size : " + _fileresult.Item4, model.INSERTED_BY);

                    }
                    else
                    {
                        status.StatusCode = -1;
                        status.StatusMessage = "Error on updating " + _fileresult.Item2;
                        await _dbrepo.AppLog.LogTotableF(IxmAppLogType.LogB2B, IxmAppSourceObjects.B2BLOADS, model.SYSTEMID, model.SOURCEID.ToString(), _fileresult.Item2 + " :: File : " + model.ORGIMAGEFILE.FileName, model.INSERTED_BY);

                        return Ok(status);

                    }


                    if (model.OBJECTID == -1)
                    {
                        _logger.LogInformation("Upload file saved : {@fileresult}", _fileresult);
                        var _organizerresult = _docrepo.Document.AddEditDocumentToDB(_logger, model);


                        if (_organizerresult.Item1 < 0)
                        {
                            return BadRequest("Error Encountered, while attempting to Save Upload File to DB.");
                        }
                    }

                    lNewFIle = _fileresult.Item2;
                }

                else

                {

                    _logger.LogInformation("A file to be uploaded is expected with this operation : {@model}", model);
                    return BadRequest("A file to be uploaded is expected with this operation");

                }


                /// Completed File Update to Database
                /// When Uploading Remittance related documents
                /// 

                if (model.DOCTYPE == "XLS_REMITTANCE")
                {
                    await ImportRemmittance(model, lPeriodId, lNewFIle);
                }

                /// 
                /// 

                return Ok();
            }
            else
            {
                _logger.LogInformation("A valid user is required for Data File Upload operation : {@model}", model);
                return BadRequest("A valid user is required for Data File Upload operation");
            }

        }


        [Authorize]
        [HttpPost("/General/FileUpload")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> GeneralFileUpload([FromForm] DOCUMENT_UPLOAD model)
        {
            _logger.LogInformation("FileUpload...Getting started");
            var status = new IxmReturnStatus();
            int lPeriodId = -1;

            ApplicationUser usr = await _dbrepo.General.GetCurrentUserAsync(HttpContext);

            _logger.LogInformation("FileUpload...Getting user {@usr}", usr);
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.StatusMessage = "Please pass a valid dataset with your request.";
                _logger.LogTrace(status.StatusMessage);
                return Ok(status);

            }

            if (usr != null)
            {
                _logger.LogInformation("FileUpload Step 2 on Status: {@status}", status);
                //int _organizerresult = -1;
                string lNewFIle = "";


                MOBJECT_DOC docModel = _dbrepo.General.GetObjectDoc(model);

                if (model.ORGIMAGEFILE != null)
                {
                    var _fileresult = await _commonrepo.FileService.SaveDocumentToFileSystem(docModel, model.ORGIMAGEFILE);
                    if ((_fileresult.Item1 == 1) && (model != null))
                    {


                        await _dbrepo.AppLog.LogTotableF(IxmAppLogType.LogSystem, GetSourcObject(docModel.DOCTYPE), model.SYSTEMID, model.SOURCEID.ToString(),
                                                            "File Saved to Server : " + _fileresult.Item2 + " File Size : " + _fileresult.Item4, model.INSERTED_BY);

                    }
                    else
                    {
                        status.StatusCode = -1;
                        status.StatusMessage = "Error on updating " + _fileresult.Item2;
                        await _dbrepo.AppLog.LogTotableF(IxmAppLogType.LogB2B, IxmAppSourceObjects.B2BLOADS, model.SYSTEMID, model.SOURCEID.ToString(), _fileresult.Item2 + " :: File : " + model.ORGIMAGEFILE.FileName, model.INSERTED_BY);

                        return Ok(status);

                    }


                    if (model.OBJECTID == -1)
                    {
                        _logger.LogInformation("Upload file saved : {@fileresult}", _fileresult);
                        var _organizerresult = _docrepo.Document.AddEditDocumentToDB(_logger, docModel);


                        if (_organizerresult.Item1 < 0)
                        {
                            return BadRequest("Error Encountered, while attempting to Save Upload File to DB.");
                        }
                    }

                    lNewFIle = _fileresult.Item2;
                }

                else

                {

                    _logger.LogInformation("A file to be uploaded is expected with this operation : {@model}", model);
                    return BadRequest("A file to be uploaded is expected with this operation");

                }


                /// Completed File Update to Database
                /// When Uploading Remittance related documents
                /// 

                /// 
                /// 

                return Ok();
            }
            else
            {
                _logger.LogInformation("A valid user is required for Data File Upload operation : {@model}", model);
                return BadRequest("A valid user is required for Data File Upload operation");
            }

        }

        private IxmAppSourceObjects GetSourcObject(string docType)
        {

            if (docType == "EVENT") { return IxmAppSourceObjects.TEVENT; }
            else if (docType == "XLS_REMITTANCE") { return IxmAppSourceObjects.B2BLOADS; }
            else if (docType == "REMITTANCE_POP") { return IxmAppSourceObjects.B2BLOADS; }
            else if (docType == "REMITTANCE_EMAIL") { return IxmAppSourceObjects.B2BLOADS; }
            else { return IxmAppSourceObjects.B2BLOADS; }

        }

        private async Task ImportRemmittance(MOBJECT_DOC model, int lPeriodId, string lNewFile)
        {

            _logger.LogInformation("Remmittance FileUpload :: Loading For Period {@Period}", model.FYEARMONTH);

            if (model.FYEARMONTH != null)
            {
                var checkresult = _datavalidator.HasRemmittanceLoaded(model.FYEARMONTH, model.SOURCEID.ToString());
                if (checkresult.Item1 != 0)
                {
                    //Return  messsage notifying that schedule has already been loaded.
                    var ls = model.ORGIMAGEFILE == null ? "" : model.ORGIMAGEFILE.FileName;
                    await _dbrepo.AppLog.LogTotableE(IxmAppLogType.LogB2B, IxmAppSourceObjects.B2BLOADS, model.SYSTEMID, model.SOURCEID.ToString(),
                                                        checkresult.Item3 + " :: File : " + ls, model.INSERTED_BY);
                    return;
                }
                else { lPeriodId = checkresult.Item2; } //Pass the Period to be loaded.

                _logger.LogInformation("Remmittance FileUpload :: Loading For Period {@Period}", model.FYEARMONTH);

            }

            Tuple<int, string> _regresult = new Tuple<int, string>(0, "NoData");

            if (model.FYEARMONTH != null)
            {
                _regresult = _dataservice.RegisterBusinessUploadToRemmittance(_logger, model, lPeriodId, 0);
                if (_regresult.Item1 < 0)
                {

                    await _dbrepo.AppLog.LogTotableE(IxmAppLogType.LogB2B, IxmAppSourceObjects.TRMBL, model.SYSTEMID, _regresult.Item1.ToString(), "Note :: " + _regresult.Item2, model.INSERTED_BY);
                    return;
                    //return BadRequest(_regresult.Item2);
                }


                await _dbrepo.AppLog.LogTotableF(IxmAppLogType.LogB2B, IxmAppSourceObjects.TRMBL, model.SYSTEMID, _regresult.Item1.ToString(), "File Registered to DB Server.", model.INSERTED_BY);

                if (model.FYEARMONTH == null)
                {
                    //status.StatusCode = _organizerresult.Item1;
                   // status.StatusCode = model.OBJECTID;
                    //status.StatusMessage = "File Loaded Successfully";
                    //return Ok(status);

                }

                if (model.FYEARMONTH != null)
                {

                    string lFld = _configuration.GetConnectionString("BaseDocFolder");
                    /*
                     * Sending to Messaghe Queue
                     * 
                    await _dbrepo.AppLog.LogTotableF(IxmAppLogType.LogB2B, IxmAppSourceObjects.TRMBL, model.SYSTEMID, _regresult.Item1.ToString(), "Remmittance :: Sending to Messaging Queue for further Processing", model.INSERTED_BY);

                    //Send to MessageQueue
                    await _publishEndpoint.Publish(new RemittanceCreateEvent
                    {
                        COMPANYID = model.SOURCEID,
                        RMBLID = _regresult.Item1,
                        PERIODID = lPeriodId,
                        SYSTEMID = model.SYSTEMID,
                        OBJECTID = model.OBJECTID,
                        FILENAME = lNewFile, // Get the new filename
                        USERNAME = model.INSERTED_BY, // (string)usr.SYSTEM_UNAME,
                        DOCFOLDER = lFld
                    });

                    * Receiving from Message QUeue
                    * 
                    await _dbrepo.AppLog.LogTotableF(IxmAppLogType.LogB2B, IxmAppSourceObjects.TRMBL, context.Message.SYSTEMID, nameof(RemittanceCreateEvent), "Consuming Remittance Message Queue for Processing", context.Message.USERNAME);
                    var lresult = await _dbrepo.FileIngest.XlsRemittanceInjest(context.Message.USERNAME, context.Message.OBJECTID.ToString(), model.SYSTEMID.ToString(), Convert.ToChar('Y'));
                    _logger.LogInformation("Result : XLsRemittanceInject :: Records loaded to Memory {@1}", lresult.Count);


                    */

                    var lret = await _transrepo.Transaction.GenerateRemittanceFromDataFile(model, _regresult.Item1.ToString(), model.INSERTED_BY, lFld, lNewFile);
                    _logger.LogInformation("Remmittance File Loaded Successfully : Upload feedback : {@user}, System : {@system} allocated Remittance ID : {@id}", lret.Item2, model.INSERTED_BY, model.SYSTEMID);



                }

                }
                if (model.OBJECTID > 0)
                {
                    var iresult = _docrepo.Document.EditOrganizerImage(model);
                }

                /*

                if (_organizerresult.Item1 > 0)
                {
                    status.StatusCode = 1;
                    status.StatusMessage = "File Loaded Successfully.";

                }
                else
                {
                    status.StatusCode = -1;
                    status.StatusMessage = "Error on updating Organizer";
                    _logger.LogTrace(status.StatusMessage);

                }
                */
                _logger.LogInformation("Remmittance File Loaded Successfully : User : {@user}, System : {@system} allocated Remittance ID : {@id}", model.INSERTED_BY, model.SYSTEMID, _regresult.Item1);
                //return Ok(status);





        }

    }
}
