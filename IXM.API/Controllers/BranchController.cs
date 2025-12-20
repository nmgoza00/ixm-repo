using IXM.DB;
using IXM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using FirebirdSql.EntityFrameworkCore;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Collections.Generic;
using System.Xml.Linq;
using IXM.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace IXM.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BranchController : ControllerBase
           {
        private readonly IXMDBContext _context;
        private readonly IDataService _dataservice;

        public BranchController(IXMDBContext context,
                                    IDataService dataservice)
        { _context = context;
            _dataservice = dataservice;
        }


        [HttpGet]
        //public IEnumerable<MCOMPANY> Get()
        public IActionResult Get()
        {

            var prData = _context.MCOMPANY.FromSqlRaw<MCOMPANY>($"SELECT FIRST 500 m.*" +
                $" FROM VMCOMPANY m WHERE m.COMPTYPID = 4 AND m.RCOMPANYID > 0");                //.Take(1000).ToList();
            return prData == null ? NotFound() : Ok(prData);

        }

        [HttpGet("Organizer")]
        public IActionResult GetOrganizerFilter(string pCompanyID)
        {
            string _pCompanyID = pCompanyID;
     
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


        [HttpGet("Members")]
        public IActionResult GetCompanyMembers(string pBCompany)
        {

            var prMembers = _context.MMEMBER_READ.FromSqlRaw<MMEMBER_READ>($"SELECT FIRST 200 v.*, mc.DESCRIPTION CITYNAME, m.PYEAR||' '||m.PMONTH PYEARMONTH, CASE WHEN mcd.MEMBERID IS NULL THEN 'N' ELSE 'Y' END MemCardReceived  " +
                                        $" FROM MMEMBER v "+
                                        $" INNER JOIN MCOMPANY b ON b.COMPANYID = v.BCOMPANYID " +
                                        $" LEFT JOIN MCITY mc ON v.CITYID = mc.CITYID " +
                                        $" LEFT JOIN MMCDD mcd ON v.MEMBERID = mcd.MEMBERID " +
                                        $" LEFT JOIN MMLP m ON m.MEMBERID = v.MEMBERID WHERE (v.BCOMPANYID =  @p0 OR v.COMPANYID = @p0) ", pBCompany);
            return prMembers == null ? NotFound() : Ok(prMembers);

        }


        [HttpGet("Payments")]
        public IActionResult GetBranchPayments(string pBCompany)
        {

            var prMembers = _context.MMEMBER.FromSqlRaw<MMEMBER>($"SELECT FIRST 200 v.*, mc.DESCRIPTION CITYNAME, m.PYEAR||' '||m.PMONTH PYEARMONTH " +
                                        $" FROM MMEMBER v " +
                                        $" INNER JOIN MCOMPANY b ON b.COMPANYID = v.BCOMPANYID " +
                                        $" LEFT JOIN MCITY mc ON v.CITYID = mc.CITYID " +
                                        $" LEFT JOIN MMLP m ON m.MEMBERID = v.MEMBERID WHERE v.BCOMPANYID =  @p0 AND b.COMPTYPID = 4", pBCompany);
            return prMembers == null ? NotFound() : Ok(prMembers);

        }


        [HttpGet("NonPayment")]
        public IActionResult GetBranchNonPayments(string pBCompany, string pPeriod)
        {

            var prMembers = _context.MMEMBER_NONPAYMENT.FromSqlRaw<MMEMBER_NONPAYMENT>(_dataservice.GetBranch_NonPaymentMembers(pPeriod,pBCompany));
            return prMembers == null ? NotFound() : Ok(prMembers);

        }

        [HttpGet("pid")]
        [ProducesResponseType(typeof(MCOMPANY),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int pid) 
        {
            var company = await _context.MCOMPANY.FindAsync(pid);
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

        public async Task<IActionResult> Update(int pid, MCOMPANY company) 
        {
            if (pid != company.COMPANYID) return BadRequest();
            
            _context.Entry(company).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            //var companyId = company.CompanyId;

            return NoContent();
        }


    }
}


