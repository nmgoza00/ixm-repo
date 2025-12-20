using IXM.DB;
using IXM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IXM.Common;
using System.Text.Json;
using IXM.DB.Services;
using Microsoft.AspNetCore.Identity;
//using MassTransit;
using IXM.Models.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace IXM.API.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class FinanceController : ControllerBase
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
        //private readonly IPublishEndpoint _publishEndpoint;
        public FinanceController(IXMDBContext context, IXMDBIdentity identity,
                                    ILogger<TransactionController> logger,
                                    UserManager<ApplicationUser> usermanager,
                                    IIXMDBRepo general,
                                    IIXMTransactionRepo dataservice,
                                    IDataImport dataimport,
                                    IDataValidator datavalidator,
                                    //IPublishEndpoint publishEndpoint,
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
            //_publishEndpoint = publishEndpoint;
            //_fileservice = fileService;
        }

        [AllowAnonymous]
        [HttpPost("Payment/Reverse")]
        public async Task<IActionResult> ReversePayment(string PaymentGuid)
        {


            ApplicationUser usr = await _general.General.GetCurrentUserAsync(HttpContext);
            try
            {


                var tRMBL = _context.TPAYMENT.Where(a => a.HGUID == PaymentGuid).FirstOrDefault();
                var tJRNL = _context.TTJRN.Where(a => a.TJRNLID == tRMBL.TJRNLID).FirstOrDefault();

                int _JrnlNumber = await _dataservice.Finance.Finance_ReversePaymentGeneralLedger(tJRNL, usr.SYSTEM_UNAME);
                tRMBL.PSTATUSID = _general.General.GetConfigStatus("TPAYMENT", 3);
                tRMBL.TJRNLID = _JrnlNumber;

                _context.SaveChanges();

                return Ok();

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
        [AllowAnonymous]
        [HttpPost("Invoice/Reverse")]
        public async Task<IActionResult> ReverseInvoice(string InvoiceGuid)
        {


            ApplicationUser usr = await _general.General.GetCurrentUserAsync(HttpContext);
            try
            {


                var tRMBL = _context.TINVOICE.Where(a => a.HGUID == InvoiceGuid).FirstOrDefault();
                var tJRNL = _context.TTJRN.Where(a => a.TJRNLID == tRMBL.TJRNLID).FirstOrDefault();

                int _JrnlNumber = await _dataservice.Finance.Finance_ReverseInvoiceGeneralLedgerr(tJRNL, usr.SYSTEM_UNAME);
                tRMBL.ISTATUSID = _general.General.GetConfigStatus("TINVOICE", 3);
                tRMBL.TJRNLID = _JrnlNumber;

                _context.SaveChanges();

                return Ok();

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
        [AllowAnonymous]
        [HttpPost("Invoice/Generate")]
        public async Task<IActionResult> InvoiceGenerate(string InvoiceGuid)
        {


            ApplicationUser usr = await _general.General.GetCurrentUserAsync(HttpContext);
            try
            {


                var tRMBL = _context.TINVOICE.Where(a => a.HGUID == InvoiceGuid).FirstOrDefault();
                var tJRNL = _context.TTJRN.Where(a => a.TJRNLID == tRMBL.TJRNLID).FirstOrDefault();

                int _JrnlNumber = await _dataservice.Finance.Finance_ReverseInvoiceGeneralLedgerr(tJRNL, usr.SYSTEM_UNAME);
                tRMBL.ISTATUSID = _general.General.GetConfigStatus("TINVOICE", 2);
                tRMBL.TJRNLID = _JrnlNumber;

                _context.SaveChanges();

                return Ok();

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

    }
}
