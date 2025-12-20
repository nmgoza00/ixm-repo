using DevExpress.XtraPrinting;
using FirebirdSql.EntityFrameworkCore;
using IXM.API.Services;
using IXM.DB;
using IXM.Models;
using IXM.Models.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace IXM.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IXMDBContext _context;
        private readonly IDataService _dataservice;
        private readonly ILogger<CompanyController> _logger;

        private readonly IIXMDBRepo _dbrepo;

        public CompanyController(ILogger<CompanyController> logger,IXMDBContext context, IIXMDBRepo dbrepo, IDataService dataService)
        { 
             _context = context;
            _dataservice = dataService;
            _logger = logger;
            _dbrepo = dbrepo;
        }



        [AllowAnonymous]
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {

            ApplicationUser usr = await _dbrepo.General.GetCurrentUserAsync(HttpContext);
            //var prData = _dbrepo.Company.GetCompanyList(usr, null, PAGENO, PAGESIZE);
            var prData = _dbrepo.Company.GetCompanyList(usr, null);
            _logger.LogInformation("Retrieved Company List");
            return prData == null ? NotFound() : Ok(prData);

        }

        [HttpGet("Tester")]
        public String GetDateTime()
        {

            return DateTime.Now.ToString();

        }
        [AllowAnonymous]
        [HttpGet("System")]
        public async Task<IActionResult> GetCompanyBySystem(string SystemId, string Company)
        {

            ApplicationUser usr = await _dbrepo.General.GetCurrentUserAsync(HttpContext);
            var prData = await _dbrepo.Company.GetCompanyBySystem(usr, SystemId, Company);
            _logger.LogInformation("Retrieved Company List");
            return prData == null ? NotFound() : Ok(prData);

        }


        [HttpGet("Organizer")]
        //[Route("filter")]
        public IActionResult GetOrganizerFilter(string pCompanyID)
        {
            string _pCompanyID = pCompanyID;
            //var prCompany = _context.Database.SqlQuery<MCOMPANY>($"SELECT MCOMPANY.* FROM MCOMPANY INNER JOIN CCUR ON CCUR.SRCID = MCOMPANY.COMPANYID AND CCUR.SRCOBJ = {"MUSER"} INNER JOIN MUSER ON MUSER.USERID = CCUR.USERID AND MUSER.UNAME = {pUsername}");
 
            var prCompany = _context.MCOMPANY_CCUR.FromSqlRaw<MCOMPANY_CCUR>($"SELECT m.*, mld.NAME ||' '||mld.SURNAME PROCESSOR_NAME, " +
                $" tp.PAYMENTNUM, tp.MYEAR||' - '||tp.MMONTH FYEARMONTH, "+
                $" mld.EMAILADDRESS PROCESSOR_EMAIL, mld.CELLNUMBER PROCESSOR_CELLNUMBER " +
                $" FROM VMCOMPANY m "+
                $" INNER JOIN CCUR ON CCUR.COMPANYID = m.COMPANYID AND CCUR.SRCOBJ = 'MUSER' " +
                $" LEFT JOIN CCUR ld ON ld.COMPANYID = m.COMPANYID AND ld.SRCOBJ = 'MUSER'" +
                $" LEFT JOIN VTPAYMENTL tp ON tp.CUSTOMERID = m.COMPANYID " +
                $" LEFT JOIN MUSER mld ON mld.USERID = ld.USERID "+
                $" INNER JOIN MUSER ON MUSER.USERID = CCUR.USERID AND m.COMPANYID = @p0", _pCompanyID);
            return prCompany == null ? NotFound() : Ok(prCompany);

        }


        [Authorize]
        [HttpGet("Members")]
        [EnableQuery]
        public async Task<IActionResult> GetCompanyMembers(string Company)
        {

            string _param = "1";
            ApplicationUser usr = await _dbrepo.General.GetCurrentUserAsync(HttpContext);
            if (usr == null)
            {
                return NotFound("File Not Found ");
            }

            var prMembers = _dbrepo.Company.GetCompanyMembersByGuid(usr,Company);
            return prMembers == null ? NotFound() : Ok(prMembers);

        }


        [HttpGet("Payments")]
        [EnableQuery]
        public async Task<IActionResult> GetCompanyPaymentFilter(string pCompanyId)
        {
            string _pCompanyId = pCompanyId;
            ApplicationUser usr = await _dbrepo.General.GetCurrentUserAsync(HttpContext);

            var prCompany = _dbrepo.Company.GetCompanyPaymentByGuid(usr, pCompanyId);

            return prCompany == null ? NotFound() : Ok(prCompany);

        }




        [HttpGet("pCompany")]
        //[Route("filter")]
        public IActionResult GetFilter(string pCompany)
        {
            var prCompany = _context.MCOMPANY.Where(b => b.CNAME.Contains(pCompany)).ToList();
            return prCompany == null ? NotFound() : Ok(prCompany);

        }


        [HttpGet("pid")]
        [ProducesResponseType(typeof(MCOMPANY),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int pid) 
        {
            var company = await _context.MCOMPANY.FindAsync(pid);
            return company == null ? NotFound() : Ok(company);
        }



        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MCOMPANY), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var company = _context.MCOMPANY.FromSqlRaw<MCOMPANY>($"SELECT FIRST 250 m.* FROM VMCOMPANY m WHERE m.COMPANYID = @p0",id).First();

            //_context.MCOMPANY.FromSqlRaw<MCOMPANY>($"SELECT FIRST 250 m.* FROM VMCOMPANY m WHERE COMPANYID = @p0",id); 
            return company == null ? NotFound() : Ok(company);
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(MCOMPANY company) 
        {
            await _context.MCOMPANY.AddAsync(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new {pid = company.COMPANYID },company);

        }


        [HttpPut("{pid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Update(MCOMPANY_WRITE company)
        {

            _dataservice.ModifyCompany(_logger, company);

            return Ok();
        }

        [HttpDelete("{pid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Delete(int pid) 
        { 
            var companyToDelete = await _context.MCOMPANY.FindAsync(pid);
            if (companyToDelete == null) return NotFound();

            _context.MCOMPANY.Remove(companyToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        
        }
    }
}
