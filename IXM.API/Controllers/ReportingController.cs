using IXM.Constants;
using IXM.DB.Services;
using IXM.DB;
using IXM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IXM.Common.Implementation.ExcelExporter;
using System.Data;
using IXM.Common.Constant;
using Microsoft.EntityFrameworkCore;

namespace IXM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportingController : ControllerBase
    {
        private readonly IXMDBContext _context;
        private readonly IDataValidator _datavalidator;
        private readonly IConfiguration _configuration;
        private readonly IIXMDBRepo _ixmdb;
        private readonly IIXMDBRepo _general;
        private readonly IXMDBIdentity _identitycontext;
        private readonly ILogger<TransactionController> _logger;
        private readonly UserManager<ApplicationUser> _usermanager;
        public GenFunctions genFunctions = new GenFunctions();
        public ReportingController(IXMDBContext context, IXMDBIdentity identity,
                                    ILogger<TransactionController> logger,
                                    UserManager<ApplicationUser> usermanager,
                                    IIXMDBRepo general,
                                    IIXMDBRepo ixmdb,
                                    IDataValidator datavalidator,
                                    IConfiguration configuration)
        {
            _context = context;
            _identitycontext = identity;
            _usermanager = usermanager;
            _logger = logger;
            _ixmdb = ixmdb;
            _general = general;
            _datavalidator = datavalidator;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet("List/MonthlyReports")]
        public async Task<IActionResult> ListMonthlyReports()
        {

            var lPayments = await _general.Reporting.ListMonthlyReports();

            return Ok(lPayments);

        }

        [AllowAnonymous]
        [HttpPost("FinanceMonthly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> SFinanceMonthlyrCreate(int SystemId, int PeriodId)
        {

            ApplicationUser usr = await _general.General.GetCurrentUserAsync(HttpContext);

            await _ixmdb.Reporting.Payment_UpdateLatestPayment(PeriodId-2, PeriodId, "");

            await _ixmdb.Reporting.FinanceReport_GenerateMonthlyToTable(PeriodId, "");

            //List<REP_FINANCE_MONTHLY> lrepresult = await _ixmdb.Reporting.FinanceReport_GenerateMonthly(PeriodId-2, PeriodId, "");
            List<REP_FINANCE_MONTHLY> lrepresult = await _ixmdb.Reporting.FinanceReport_FromTable(PeriodId, "");
            DataTable _data = lrepresult.ToDataTable();


            var BaseDocFolder = _configuration.GetConnectionString("BaseDocFolder");
            var lSystemType = "Live";

            string contentType = "";

            var pSystem = _identitycontext.MSYSTEM.Where(a => a.SYSTEMID == SystemId.ToString()).First();

            if (pSystem == null)
            {
                return BadRequest("System Improper");
            }

            string p2 = nameof(IxmStaticReports.REP_FINCOM);

            var pReport = await _context.MREPORT.Where(a => a.TECHNICALNAME == p2).FirstAsync();

            var path = genFunctions.GetTemplateDocument(pSystem.SYSTEMNAME, IxmAppDocumentType.XLS_REPORT, IxmAppTemplateFileType.Report, lSystemType, BaseDocFolder);
            string PhysicalFileName = path.Item2;

            var pPeriod = await _context.MPERIOD.Where(a => a.PRID == PeriodId).FirstAsync();

            if (pReport.CRT_PERIOD == 1)
            {
                PhysicalFileName = PhysicalFileName + pReport.FILENAME;
                PhysicalFileName = PhysicalFileName.Replace("[PERIOD]", pPeriod.MYEARMONTH);
            }


            _ixmdb.Reporting.ExportReportToFile(_data, PhysicalFileName, "Data");

            return Ok();
        }


    }



}
