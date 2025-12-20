using DocumentFormat.OpenXml.Spreadsheet;
using IXM.API.Services;
using IXM.Common;
using IXM.DB;
using IXM.GeneralSQL;
using IXM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ixm_api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IXMDBContext _context;
        private readonly IDataService _dataservice;
        private readonly IGeneralSQL _generalSQL;
        private readonly ILogger<MemberController> _logger;
        private readonly IIXMDBRepo _dbrepo;

        public MemberController(IXMDBContext context,
                                    ILogger<MemberController> logger,
                                    IIXMDBRepo dbrepo,
                                    IGeneralSQL generalSQL,
                                    IDataService dataservice)
        {

            _context = context;
            _logger = logger;
            _generalSQL = generalSQL;
            _dataservice = dataservice;
            _dbrepo = dbrepo;
        }

        [AllowAnonymous]
        [HttpGet("Payments")]
        [EnableQuery]
        public IActionResult GetCompanyPaymentFilter(string pMemberId)
        {
            string _pMemberId = pMemberId;

            var prCompany = _context.TMEMPAYMENT.FromSqlRaw<TMEMPAYMENT>(_generalSQL.GetMemberPayments(pMemberId));

            return prCompany == null ? NotFound() : Ok(prCompany);

        }


        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MMEMBER), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var psid = id.ToString();
            var company = _context.MMEMBER.FromSqlRaw<MMEMBER>(_dataservice.GetMember_Details(psid)).First();
            //.FindAsync(pid);
            return company == null ? NotFound() : Ok(company);
        }

        [AllowAnonymous]
        [HttpGet("Member_MASTERDATA")]
        [ProducesResponseType(typeof(MMEMBER), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMemberMasterData(int id)
        {
            var psid = id.ToString();
            var company = await _dbrepo.Member.GetMember_MASTERDATA();
            //.FindAsync(pid);
            return company == null ? NotFound() : Ok(company);
        }


        [HttpGet("Guid")]
        [ProducesResponseType(typeof(MMEMBER), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByGuId(string uid)
        {
            var psid = uid;

            ApplicationUser usr = await _dbrepo.General.GetCurrentUserAsync(HttpContext);
            if (usr == null)
            {
                return NotFound("File Not Found ");
            }


            var member = _dbrepo.Member.GetMemberByGuid(usr, uid);
            return member == null ? NotFound() : Ok(member);
        }


        [HttpGet("Details")]
        [EnableQuery]
        [ProducesResponseType(typeof(MCOMPANY),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByParamId(int pid) 
        {
            var psid = pid.ToString();
            var company = _context.MMEMBER.FromSqlRaw<MMEMBER>(_dataservice.GetMember_Details(psid));
                //.FindAsync(pid);
            return company == null ? NotFound() : Ok(company);
        }



        [HttpGet("Notes")]
        [EnableQuery]
        [ProducesResponseType(typeof(MCOMPANY), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMemberNoteById(int pMemberid)
        {
            var company = await _context.MMEMBER_NOT.Where(o => o.MEMBERID == pMemberid).ToListAsync();
            return company == null ? NotFound() : Ok(company);
        }


        [HttpGet("Bank")]
        [ProducesResponseType(typeof(MCOMPANY), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMemberBankById(int pMemberid)
        {
            var company = await _context.MMEMBER_BANK.Where(o => o.MEMBERID == pMemberid).ToListAsync();
            return company == null ? NotFound() : Ok(company);
        }



        [HttpGet("Address")]
        [ProducesResponseType(typeof(MCOMPANY), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMemberAddressById(int pMemberid)
        {
            var company = await _context.MMEMBER_ADD.Where(o => o.MEMBERID == pMemberid).ToListAsync();
            return company == null ? NotFound() : Ok(company);
        }

        [HttpGet("FindMember")]
        [EnableQuery]
        [ProducesResponseType(typeof(MMEMBER), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> FindMember(int pCompanyId, string IDSearch)
        {

            var member = await _context.MMEMBER.Where(o => o.IDNUMBER.Contains(IDSearch)).Take(10).ToListAsync();
            return member == null ? NotFound() : Ok(member);

        }

        [HttpGet("Company")]
        [ProducesResponseType(typeof(MMEMBER), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> FindMember(string CompanyId)
        {


            ApplicationUser usr = await _dbrepo.General.GetCurrentUserAsync(HttpContext);
            if (usr != null)
            {
                var mmember = _dbrepo.Member.GetMemberByCompanyGuid(usr, CompanyId);

                return mmember == null ? NotFound() : Ok(mmember);


            }
            else return null;

        }

        [AllowAnonymous]
        [HttpGet("GlobalFind")]
        [EnableQuery]
        [ProducesResponseType(typeof(MMEMBER), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GlobalFind(string pSearchText)
        {

            var member = await _context.MMEMBER.Where(o => o.IDNUMBER.Contains(pSearchText) || 
                                                           o.MNAME.Contains(pSearchText) ||
                                                           o.MSURNAME.Contains(pSearchText) ||
                                                           o.EMPNUMBER.Contains(pSearchText)).
                                                           Select(p => new{ p.MEMBERID,p.MNAME, 
                                                                            p.MSURNAME, 
                                                                            p.IDNUMBER, p.EMPNUMBER }).
                                                           Take(20).OrderBy(a=>a.IDNUMBER).
                                                           ThenBy(a=>a.EMPNUMBER).
                                                           ToListAsync();

            return member == null ? NotFound() : Ok(member);

        }


        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        //[HttpPut("{pid}")] -- This should be  a create
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Update(MMEMBER mMEMBER)
        {

            var _regresult = _dbrepo.Member.MemberUpsert(mMEMBER);
            if (_regresult.Item1 == -1)
            {
                return BadRequest(_regresult.Item2);
            }
            else if ((_regresult.Item1 > 0) && (mMEMBER.MEMBERID == -1))
            {

                return CreatedAtAction(nameof(GetById), new { pid = _regresult.Item1 }, null);

            }
            else if (mMEMBER.MEMBERID > 0)
            {
                 return AcceptedAtAction(nameof(GetById), new { id = mMEMBER.MEMBERID }, _regresult);
               
            }
            return NoContent();
        }

        /*[AllowAnonymous]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> InsertMember(MMEMBER member)
        {
            if (member.MEMBERID > 0)
            {

                _dataservice.ModifyMember(_logger, member);
                return NoContent();

            } else
            {
                _dataservice.InsertMember(_logger, member);
                return CreatedAtAction(nameof(GetById), new { pid = member.MEMBERID }, member);
            }



        }*/

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
