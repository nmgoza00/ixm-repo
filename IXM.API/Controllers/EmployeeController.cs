using IXM.API.Controllers;
using IXM.API.Services;
using IXM.Common;
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
using System.Data;
using System.Globalization;
using System.Reflection;

namespace ixm_api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IXMDBContext _context;
        private readonly IXMDBIdentity _identitycontext;
        private readonly IDataService _dataservice;
        private readonly IIXMDBRepo _dbrepo;
        private readonly ILogger<OrganizerController> _logger;

        public EmployeeController(IXMDBContext context,
            IXMDBIdentity identitycontext,
                                    ILogger<OrganizerController> logger,
                                    IIXMDBRepo dbrepo,
                                    IDataService dataservice)
        {

            _context = context;
            _logger = logger;
            _dbrepo = dbrepo;
            _dataservice = dataservice;
            _identitycontext = identitycontext;
        }

        [HttpGet("Payments")]
        public IActionResult GetCompanyPaymentFilter(string pMemberId)
        {
            string _pMemberId = pMemberId;

            var prCompany = _context.TMEMPAYMENT.FromSqlRaw<TMEMPAYMENT>($"SELECT m.COMPANYID,m.CNAME,m.COMPANYNUM,t.PAYMENTID,td.IAMOUNT PAMOUNT,t.PAYMENTNUM, "+
                                        $" p.FYEARMONTH PYEARMONTH, t.PERIODID, td.MEMBERID "+
                                        $" FROM TPAYMENT t "+
                                        $" INNER JOIN TPAYMENT_DET td ON t.PAYMENTID = td.PAYMENTID " +
                                        $" LEFT JOIN MCOMPANY m ON t.CUSTOMERID = m.COMPANYID "+
                                        $" INNER JOIN MPERIOD p ON p.PRID = t.PERIODID "+
                                        $" WHERE td.MEMBERID = @p0"+
                                        $" ORDER BY P.FYEARMONTH ", pMemberId);

            return prCompany == null ? NotFound() : Ok(prCompany);

        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MEMPLOYEE), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var psid = id.ToString();
            var company = _context.MEMPLOYEE.FromSqlRaw<MEMPLOYEE>(_dataservice.GetMember_Details(psid)).First();
            //.FindAsync(pid);
            return company == null ? NotFound() : Ok(company);
        }


        [AllowAnonymous]
        [HttpGet("UserEmployee")]
        [ProducesResponseType(typeof(MCOMPANY),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByParamId(string pid) 
        {
            if (pid != null)
            {

                var psid = pid.ToString();
                var userlist = await _identitycontext.Users.Where(a => a.Id == pid).ToListAsync();
                var employee = await _context.MEMPLOYEE_DISPLAY.FromSqlRaw<MEMPLOYEE_DISPLAY>(_dataservice.GetLinkedUserToEmployee(psid)).ToListAsync();

                var ue = (from ur in userlist

                          join lro in employee on ur.Id equals lro.ID_USERID
                             into lrojoiner
                          from urlro in lrojoiner.DefaultIfEmpty()

                          select new
                          {
                              ur.Id,
                              ur.Email,
                              urlro.EMPID,
                              urlro.MNAME,
                              urlro.MSURNAME,
                              urlro.CELLNUMBER,
                              urlro.MEMBERID,
                              urlro.EMPTYPID

                          }).DistinctBy(a => (a.Id));
                return ue == null ? NotFound() : Ok(ue);

            } return NotFound();



        }


        [AllowAnonymous]
        [HttpGet("EmployeeType")]
        [EnableQuery]
        public async Task<IActionResult> GetEmployeeType()
        {

            List<MEMPTYPE> prData = await _dbrepo.MasterData.GetEmployeeType();
            return prData == null ? NotFound() : Ok(prData);

        }

        [HttpPost("Details")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Update(MEMPLOYEE_WRITE mEMPLOYEE)
        {

            var lretu = _dataservice.ModifyEmployee(_logger, mEMPLOYEE);

            if (lretu != null) 
            {
                return Ok(lretu);
            }
            else return NoContent();
        }

        [HttpGet("FindEmployee")]
        [ProducesResponseType(typeof(MEMPLOYEE), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> FindMember(int pCompanyId, string IDSearch)
        {

            var member = await _context.MEMPLOYEE.Where(o => o.IDNUMBER.Contains(IDSearch)).Take(10).ToListAsync();
            return member == null ? NotFound() : Ok(member);

        }


        [HttpGet("GlobalFind")]
        [ProducesResponseType(typeof(MEMPLOYEE), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GlobalFind(string pSearchText)
        {

            var employee = await _context.MEMPLOYEE.Where(o => o.IDNUMBER.Contains(pSearchText) || 
                                                           o.MNAME.Contains(pSearchText) ||
                                                           o.MSURNAME.Contains(pSearchText)).
                                                           Select(p => new{ p.EMPID,p.MNAME, 
                                                                            p.MSURNAME, 
                                                                            p.IDNUMBER,
                                                                            p.MEMBERID }).
                                                           Take(200).OrderBy(a=>a.IDNUMBER).
                                                           ToListAsync();

            return employee == null ? NotFound() : Ok(employee);

        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> InsertEmployee(MEMPLOYEE employee)
        {
            //member.MEMBERID = _dataservice.GetSEQUENCE("SEQ_MEMBERID");
            _dataservice.InsertEmployee(_logger, employee);
            //await _context.MMEMBER.AddAsync(member);
            //await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { pid = employee.MEMBERID }, employee);

        }

        [HttpDelete("{pid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Delete(int pid) 
        { 
            var companyToDelete = await _context.MEMPLOYEE.FindAsync(pid);
            if (companyToDelete == null) return NotFound();

            _context.MEMPLOYEE.Remove(companyToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        
        }
    }
}

