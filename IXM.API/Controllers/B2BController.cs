using IXM.DB;
using IXM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IXM.Models.Core;
using IXM.DB.Services;
using IXM.DB.Mapper;
using IXM.DB.Server;
using IXM.Common;
using System.Text.Json;
using IXM.GeneralSQL;
using Remittance = IXM.DB.Remittance;
using IXM.Constants;

namespace IXM.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class B2BController : ControllerBase
    {
        private readonly IXMDBContext _context;
        private readonly IDataServicesServer _dataservices;
        private readonly IDataValidator _datavalidator;
        private readonly IConfiguration _configuration;
        private readonly IIXMCommonRepo _ixmcommon;
        private readonly IGeneralSQL _generalSQL;
        private readonly IIXMDBRepo _ixmdbrepo;
        private readonly IIXMTransactionRepo _transactrepo;
        private readonly IXMDBIdentity _identitycontext;
        private readonly ILogger<OrganizerController> _logger;

        public B2BController(IXMDBContext context, IXMDBIdentity identity,
                                    ILogger<OrganizerController> logger,
                                    IDataServicesServer dataservices,
                                    IIXMCommonRepo ixmcommon,
                                    IIXMDBRepo ixmdbrepo,
                                    IGeneralSQL generalSQL,
                                    IIXMTransactionRepo transactRepo,
                                    IDataValidator datavalidator,
                                    IConfiguration configuration,
                                    IIXMCommonRepo ixmcommonrepo)
        {
            _context = context;
            _identitycontext = identity;
            _logger = logger;
            _ixmdbrepo = ixmdbrepo;
            _ixmcommon = ixmcommon;
            _generalSQL = generalSQL;
            _datavalidator = datavalidator;
            _dataservices = dataservices;
            _configuration = configuration;
            _transactrepo = transactRepo;
        }


        [Authorize]
        [HttpGet("Remmittance")]
        public IActionResult GetRemittances(int CompanyId)
        {
            try
            {
                var prData = _context.TRMBL.ReadModel().Where(a => a.COMPANYID == CompanyId).ToList();

                return prData == null ? NotFound() : Ok(prData);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Remmittance")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> RemmittanceUpdate(TRMBL_POST mTRMBL)
        {
            try
            {

                //_context.Entry(existingEntity).CurrentValues.SetValues(entity);


                GeneralMapper generalMapper = new();
                TRMBL upRMBL = _context.TRMBL.Where(a => a.RMBLID == mTRMBL.RMBLID).Single();


                TRMBL tRMBL = await generalMapper.RemmittancePostMaptoDB(mTRMBL);
                _logger.LogInformation("Remmittance Update Dataset {@0}", tRMBL);
                _context.Entry(upRMBL).CurrentValues.SetValues(tRMBL);


                var ls =_context.SaveChanges();


                //var ent = _context.Entry(tRMBL);
                /*Type type = typeof(TRMBL_POST);
                System.Reflection.PropertyInfo[] properties = type.GetProperties();
                foreach (System.Reflection.PropertyInfo property  in properties)
                {
                    if (property.Name == "RMBLID")
                    {
                        continue;
                    }
                    if (ent.Property(property.Name).CurrentValue != null) { ent.Property(property.Name).IsModified = true; }
                }*/
                //.State = EntityState.Modified;

                return Ok(ls);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }




        [Authorize]
        [HttpGet("RemmittanceByRemId")]
        public IActionResult GetRemittanceById(int RMBLID)
        {
            try
            {
                var prData = _context.TRMBL.ReadModel().Where(a => a.RMBLID == RMBLID).ToList();

                return prData == null ? NotFound() : Ok(prData);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [Authorize]
        [HttpGet("RemmittanceDetail")]
        public async Task<IActionResult> GetRemittanceDetails(int RMBLID, int PAGENO, int PAGESIZE)
        {
            try
            {
                IXM.Models.Core.Remittance request = new IXM.Models.Core.Remittance(RMBLID, PAGENO, PAGESIZE);

                var prData = await _ixmdbrepo.GetB2BDataSets.RemittanceDetailHandle(request);

                string json = JsonSerializer.Serialize(prData.ItemList);

                return prData == null ? NotFound() : Ok(json);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("NotInRemmittance")]
        public IActionResult NotInRemittance(string RMBLID)
        {
            try
            {
                var prData = _context.MMEMBER.First();

                string json = JsonSerializer.Serialize(prData);

                return prData == null ? NotFound() : Ok(json);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        //[Authorize]
        [AllowAnonymous]
        [HttpPost("RemittanceImport")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> RemittanceImport(string FileID, string RMBLID, string SystemID, char Reload)
        {
            string lwhere = "";
            try
            {
                lwhere = "User";
                ApplicationUser usr = await _ixmdbrepo.General.GetCurrentUserAsync(HttpContext);

                //if ( usr != null)
                //{

                    lwhere = "XlsRemittanceInjest";

                    var _mobject_doc = await _ixmdbrepo.FileIngest.GetFileDocumentInfo(Convert.ToInt32(FileID));
                    string lFld = _configuration.GetConnectionString("BaseDocFolder");
                    _mobject_doc.SYSTEMID = SystemID;
                    await _transactrepo.Transaction.GenerateRemittanceFromDataFile(_mobject_doc, RMBLID, _mobject_doc.INSERTED_BY, lFld, _mobject_doc.DOCUMENTNAME);


                    /*

                    var lresult = await _ixmdbrepo.FileIngest.XlsRemittanceInjest(usr.SYSTEM_UNAME, FileID, SystemID, Reload);
                    if (lresult != null)
                    {
                        lwhere = "IxmExcelValidateImport";
                        int HasIssues = _ixmcommon.DataImport.IxmExcelValidateImport(ref lresult);

                        if (HasIssues == 0)
                        {

                            lwhere = "XlsRemittanceToDB";
                            var lresult2 = await _ixmdbrepo.FileIngest.XlsRemittanceToDB(lresult, "");

                            lwhere = "RemittanceUpdateDBDetails";
                            await _ixmdbrepo.CustomUpdates.RemittanceUpdateDBDetails("0", usr.SYSTEM_UNAME, lresult.First().RMBLID.ToString());

                        }
                    }

                   */

                //}


            }
            catch (Exception ex)
            {
                return NotFound("Error Encountered :: " + lwhere + " - " +ex.Message);
            }

            return Ok();


        }



        [AllowAnonymous]
        [HttpPost("CaptureImport")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CaptureImport(Remmittance_POST CaptureData)
        {
            string lwhere = "";
            try
            {
                lwhere = "User";

                var lH = CaptureData.Remmittance;
                var lHd = CaptureData.RemmittanceDetail;

                ApplicationUser usr = await _ixmdbrepo.General.GetCurrentUserAsync(HttpContext);

                if (usr != null)
                { 

                    lwhere = "XlsRemittanceToDB";

                    if ((CaptureData.PostType == (int)IxmDataCRUDType.Insert) ||
                        (CaptureData.PostType == (int)IxmDataCRUDType.Update) ||
                        (CaptureData.PostType == (int)IxmDataCRUDType.Upsert))
                    {

                        var lresult2 = await _transactrepo.Transaction.RemmittanceCaptureGenerate(lH,lHd,usr.SYSTEM_UNAME);

                    }

                }


            }
            catch (Exception ex)
            {
                return NotFound("Error Encountered :: " + lwhere + " - " + ex.Message);
            }

            return Ok();


        }

        [Authorize]
        [HttpGet("RemittanceError")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> RemittanceValidate(int CompanyId)
        {

            try
            {
                //ApplicationUser usr = await _ixmdbrepo.General.GetCurrentUserAsync(HttpContext);

                //if (usr != null)
                //{

                List<TRMBLE> lErrors = await _transactrepo.Remittance.GetRemittanceError(CompanyId);
                //var prData = await _ixmdbrepo.GetB2BDataSets.RemittanceDetailHandle(request);
                //var Errors = await _context.TRMBLE.Where(a => a.RMBLID == RMBLID).ToListAsync();

                return Ok(lErrors);
                //await _ixmcommon.DataImport.IxmExcelExportDataIssues()

                //}
                //else
                //{
                //    _logger.LogError("User does not have access to view this section.");

                //   return BadRequest("User does not have access to view this section.");
                //}

            }
            catch (Exception e)
            {


                return BadRequest("User does not have access to view this section. Error :: "+e.Message);
            }


        }

        [Authorize]
        [HttpGet("RemittanceErrorByRemId")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> RemittanceErrorByRemId(int RMBLID, string SystemID)
        {

            try
            {
                //ApplicationUser usr = await _ixmdbrepo.General.GetCurrentUserAsync(HttpContext);

                //if (usr != null)
                //{
                var Errors = await _context.TRMBLE.Where(a => a.RMBLID == RMBLID).ToListAsync();

                return Ok(Errors);
                //await _ixmcommon.DataImport.IxmExcelExportDataIssues()

                //}
                //else
                //{
                //    _logger.LogError("User does not have access to view this section.");

                //   return BadRequest("User does not have access to view this section.");
                //}

            }
            catch (Exception e)
            {


                return BadRequest("User does not have access to view this section. Error :: " + e.Message);
            }


        }

        [Authorize]
        [HttpPost("RemittanceDBDetails")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> RemittanceImportUpdates(string RMBLID, string PROCESSID, string SystemID)
        {

            try
            {
                ApplicationUser usr = await _ixmdbrepo.General.GetCurrentUserAsync(HttpContext);

                if (usr != null)
                {


                    await _ixmdbrepo.DataServices.ProcessStartAsync(PROCESSID);

                    await _ixmdbrepo.CustomUpdates.RemittanceUpdateDBDetails("0",usr.SYSTEM_UNAME, RMBLID);

                    await _ixmdbrepo.DataServices.ProcessCompleteAsync(PROCESSID);


                }
                else 
                {
                    _logger.LogError("User does not have access to view this section.");
                }

            }
            catch (Exception e)
            {

                return BadRequest("Error encountered. Error :: " + e.Message);

            }

            return Ok(0);

        }


    }
}
