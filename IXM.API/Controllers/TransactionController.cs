using IXM.DB;
using IXM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IXM.Common;
using IXM.Constants;
using System.Text.Json;
using IXM.DB.Services;
using Microsoft.AspNetCore.Identity;
using IXM.Models.Core;

namespace IXM.API.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IXMDBContext _context;
        private readonly IIXMTransactionRepo _dataservice;
        private readonly IIXMDBRepo _general;
        private readonly IDataValidator _datavalidator;
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileservice;
        private readonly IDataImport _dataimport;
        private readonly IXMDBIdentity _identitycontext;
        private readonly ILogger<TransactionController> _logger;
        private readonly UserManager<ApplicationUser> _usermanager;
        public TransactionController(IXMDBContext context, IXMDBIdentity identity,
                                    ILogger<TransactionController> logger,
                                    UserManager<ApplicationUser> usermanager,
                                    IIXMDBRepo general,
                                    IIXMTransactionRepo dataservice,
                                    IDataImport dataimport,
                                    IDataValidator datavalidator,
                                    IConfiguration configuration)
        {
            _context = context;
            _identitycontext = identity;
            _usermanager = usermanager;
            _logger = logger;
            _general = general;
            _dataimport = dataimport;
            _datavalidator = datavalidator;
            _dataservice = dataservice;
            _configuration = configuration;
            //_fileservice = fileService;
        }

        [AllowAnonymous]
        [HttpPost("Payment/ProcessMembers")]
        public async Task<IActionResult> ProcessMembers(int RMBLID, int SystemId)
        {

            ApplicationUser usr = await _general.General.GetCurrentUserAsync(HttpContext);

            try
            
            {

                if (usr != null)
                {

                    await _general.CustomUpdates.RemittanceUpdateDBDetails("0", usr.SYSTEM_UNAME, RMBLID.ToString());

                    await _dataservice.Transaction.ProcessMembers(usr, RMBLID);


                }
                else
                {
                    _logger.LogError("This section requires valid user access. Processing Remittance {@RMBLID}",RMBLID);
                }

                return Ok();

            }

            catch (Exception ex)

            {
                return NotFound(ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpPost("Payment/UpdateMembers")]
        public async Task<IActionResult> UpdateMembers(int RMBLID, List<IxmRemittanceMatchTypes> MatchTypes)
        {

            ApplicationUser usr = await _general.General.GetCurrentUserAsync(HttpContext);

            try

            {
                await _dataservice.Transaction.WrapperUpdateMemberDetails(usr.SYSTEM_UNAME, RMBLID, MatchTypes);

                await _general.CustomUpdates.RemittanceUpdateDBDetails("1", usr.SYSTEM_UNAME, RMBLID.ToString());

                return Ok();

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpGet("Payment")]
        public async Task<IActionResult> PaymentGet(string PaymentGUID)
        {

            ApplicationUser usr = await _general.General.GetCurrentUserAsync(HttpContext);
            try
            {

                // await _general.AppLog.LogTotableF(IxmAppLogType.LogPayment, IxmAppSourceObjects.TPAYMENT, SystemId.ToString(), COMPANYID.ToString(), "Sending to Messaging Queue for further Processing 'Payment Generation'", usr.UserName);


                var lPayment = _dataservice.Transaction.GetPayment(PaymentGUID);

                return Ok(lPayment);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpPost("Payment/Generate")]
        public async Task<IActionResult> PaymentGenerate(int PERIODID, int COMPANYID, int SystemId, int Source)
        {

            ApplicationUser usr = await _general.General.GetCurrentUserAsync(HttpContext);
            try
            {

                await _general.AppLog.LogTotableF(IxmAppLogType.LogPayment, IxmAppSourceObjects.TPAYMENT, SystemId.ToString(), COMPANYID.ToString(), "Sending to Queue for Processing 'Payment Generation'", usr.UserName);

                /*await _publishEndpoint.Publish(new PaymentCreateEvent
                {
                    COMPANYID = COMPANYID,
                    PERIODID = PERIODID,
                    SYSTEMID = SystemId,
                    OBJECTID = Source,
                    USERNAME = (string)usr.SYSTEM_UNAME
                });*/


                //_dataservice.Transaction.PaymentGenerate(PERIODID, COMPANYID, SystemId, Source, usr.SYSTEM_UNAME);

                return Ok();

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPost("Payment/Confirm")]
        public async Task<IActionResult> PaymentConfirmPost(string PaymentGuid)
        {

            ApplicationUser usr = await _general.General.GetCurrentUserAsync(HttpContext);
            try
            {
                int lresult = await _dataservice.Transaction.PaymentConfirmation(PaymentGuid, usr.SYSTEM_UNAME);

                if (lresult == 0)
                {
                    return Ok();

                } else
                {

                    return NotFound();

                }

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpGet("Payment/Confirm")]
        public async Task<IActionResult> PaymentConfirmGet(string? UserGuid)
        { 

            ApplicationUser usr = await _general.General.GetCurrentUserAsync(HttpContext);
            try
            {
                UserGuid = UserGuid == null ? "" : UserGuid;

                var lPayments = _dataservice.Transaction.GetPaymentConfirmation(UserGuid);


                return Ok(lPayments);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Payment/Simulate")]
        public IActionResult PaymentSimulate(int PERIODID, int COMPANYID)
        {
            try
            {
                var prData = _context.TRMBL.ReadModel().Where(a => a.COMPANYID == COMPANYID).ToList();

                return prData == null ? NotFound() : Ok(prData);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Payment/Delete")]
        public IActionResult PaymentDelete(int PAYMENTID)
        {
            try
            {
                var prData = _context.TRMBL.ReadModel().Where(a => a.COMPANYID == PAYMENTID).ToList();

                return prData == null ? NotFound() : Ok(prData);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Invoice/Generate")]
        public IActionResult InvoiceGenerate(int PERIODID, int COMPANYID)
        {
            try
            {
                var prData = _context.TRMBLD.Where(a => a.RMBLID == COMPANYID).ToList();

                string json = JsonSerializer.Serialize(prData);

                return prData == null ? NotFound() : Ok(json);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpPost("Invoice/Simulate")]
        public IActionResult InvoiceSimulate(int PERIODID, int COMPANYID)
        {
            try
            {
                var prData = _context.TRMBLD.Where(a => a.RMBLID == COMPANYID).ToList();

                string json = JsonSerializer.Serialize(prData);

                return prData == null ? NotFound() : Ok(json);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost("Invoice/Delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> InvoiceDelete(int INVOICEID)
        {
            try
            {
                

            }
            catch (Exception ex)
            {
                return NotFound("Error Encountered :: " + ex.Message);
            }

            return Ok();


        }


        [AllowAnonymous]
        [HttpPost("Creditnote/Generate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> RemittanceImportUpdates(int PERIODID, int COMPANYID)
        {


            
            return Ok();

        }
        [AllowAnonymous]
        [HttpPost("Creditnote/Delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> CreditnoteDelete(int CREDITNOTEID)
        {


            return Ok();

        }
    }
}
