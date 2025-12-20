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
    public class DistributionController : ControllerBase
           {
        private readonly IXMDBContext _context;
        private readonly IDataService _dataservice;
        private readonly IXMDBIdentity _identitycontext;
        private readonly ILogger<DistributionController> _logger;

        public DistributionController(IXMDBContext context,
                                    IXMDBIdentity identitycontext,
                                    ILogger<DistributionController> logger,
                                    IDataService dataservice)
        { 
            _context = context;
            _dataservice = dataservice;
            _logger = logger;
            _identitycontext = identitycontext;
        }
        
      

        [HttpGet]
        //public IEnumerable<MCOMPANY> Get()
        public IActionResult Get()
        {

            var prData = _context.MCOMPANY.FromSqlRaw<MCOMPANY>($"SELECT FIRST 250 m.*" +
                $" FROM VMCOMPANY m ");                //.Take(1000).ToList();
            return prData == null ? NotFound() : Ok(prData);

            //return await _context.Company.ToListAsync();
        }


        [HttpGet("CardStats")]
        public IActionResult GetDiscributionCardStats(string pBCompanyID, string pUserName)
        {
            string _pBCompanyID = pBCompanyID;
            string _pUserName = pUserName;
            var singleuser = _identitycontext.Users.Where(a => a.Email.ToLower() == pUserName.ToLower()).ToList();
            if (singleuser.Count > 0)
            {
                var prMembers = _context.IXMChartData.FromSqlRaw<IXMChartData>(_dataservice.GetDistribution_CardStats(_pBCompanyID, singleuser.First().Id));
                return Ok(prMembers);
            } else 
            {
                return NotFound();
            }

        }

        [HttpGet("SurveyStats")]
        public IActionResult GetDiscributionSurveyStats(string pBCompanyID, string pUserName)
        {
            string _pBCompanyID = pBCompanyID;
            string _pUserName = pUserName;
            //var prCompany = _context.Database.SqlQuery<MCOMPANY>($"SELECT MCOMPANY.* FROM MCOMPANY INNER JOIN CCUR ON CCUR.SRCID = MCOMPANY.COMPANYID AND CCUR.SRCOBJ = {"MUSER"} INNER JOIN MUSER ON MUSER.USERID = CCUR.USERID AND MUSER.UNAME = {pUsername}");

            var prMembers = _context.IXMChartData.FromSqlRaw<IXMChartData>(_dataservice.GetDistribution_SurveyStats(_pBCompanyID, _pUserName));
            return prMembers == null ? NotFound() : Ok(prMembers);

        }


        [HttpGet("Survey")]
        public IActionResult GetDistributionSurvery(string pMemberId)
        {

            var prMembers = _context.TSURVEYMEMBERDATA.FromSqlRaw<TSURVEYMEMBERDATA>(_dataservice.GetSurvey_Member(pMemberId));
            return prMembers == null ? NotFound() : Ok(prMembers);

        }


        [HttpPost("Survey")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(TSURVEYMEMBERDATA surveydata)
        {
            await _dataservice.UpdateSurveyData(_logger, surveydata);
            //_context.Entry(company).State = EntityState.Modified;
            //await _context.SaveChangesAsync();


            return CreatedAtAction(nameof(Create), new { pid = surveydata.MEMBERID }, surveydata);

        }


        [HttpGet("CurrentSurveyList")]
        public IActionResult GetCurrentSurveyList (string pMemberId)
        {

            var prMembers = _context.TDSURVEY_C.FromSqlRaw<TDSURVEY_C>(_dataservice.GetSurvey_CurrentList(pMemberId));
            return prMembers == null ? NotFound() : Ok(prMembers);

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


