using IXM.DB;
using IXM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using IXM.Models.Core;
using System.Globalization;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;
using IXM.Constants;
using IXM.API.Services;
using Microsoft.AspNetCore.Authorization;
using static IXM.DB.QueryRepository;
using IXM.Common;

namespace IXM.API.Controllers

{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class OrganizerController : ControllerBase
    {
        private readonly IXMDBContext _context;
        private readonly IDataService _dataservice;
        private readonly IIXMDBRepo _dbrepo;
        private readonly IIXMDocumentRepo _docrepo;
        private readonly IIXMCommonRepo _commonRepo;
        private readonly IXMDBIdentity _identitycontext;
        private readonly ILogger <OrganizerController>_logger;

        public OrganizerController( IXMDBContext context,
                                    IXMDBIdentity identitycontext,
                                    ILogger<OrganizerController> logger,
                                    IIXMDBRepo dbrepo,
                                    IIXMCommonRepo commonRepo,
                                    IIXMDocumentRepo docrepo,
                                    IDataService dataservice)
        { 
            
            _context = context;
            _logger = logger;
            _dbrepo = dbrepo;
            _docrepo = docrepo;
            _dataservice = dataservice;
            _commonRepo = commonRepo;
            _identitycontext = identitycontext;

        }
        //public override OrganizerController(IXMDBContext context) => _context = context;


        [HttpGet("Companies")]
        //[Route("filter")]
        public IActionResult GetOrganizerCompanies(string pUsername)
        {
            string _pUsername = pUsername;
            try
            {
                var singleuser = _identitycontext.Users.Where(a => a.Email.ToLower() == pUsername.ToLower()).ToList();
                if (singleuser.Count > 0)
                {
                    var prCompany = _dbrepo.Organisor.GetOrganisorCompanies(singleuser.First().Id);
                        //_context.ORGANISER_COMPANY.FromSqlRaw<ORGANISER_COMPANY>(_dataservice.GetOrganiser_Companies(singleuser.First().Id));
                    return Ok(prCompany);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpGet("Companies/Unmapped")]
        //[Route("filter")]
        public IActionResult GetOrganizerUnmappedCompanies(string pGuid)
        {
            try
            {
                var singleuser = _identitycontext.Users.Where(a => a.Id.ToLower() == pGuid.ToLower()).ToList();
                if (singleuser.Count > 0)
                {
                    var pCompany = _dbrepo.Organisor.GetOrganisorUnmappedCompanies();
                    return Ok(pCompany);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("Branches/Guid")]
        //[Route("filter")]
        public IActionResult GetOrganizerBranches(string pGuid)
        {

            try
            {
                var singleuser = _identitycontext.Users.Where(a => a.Id.ToLower() == pGuid.ToLower()).ToList();
                if (singleuser.Count > 0)
                {

                    var prCompany = _dbrepo.Organisor.GetOrganisorBranches(pGuid);
                    //var prCompany = _context.ORGANISER_COMPANY.FromSqlRaw<ORGANISER_COMPANY>(_dataservice.GetOrganiser_Branches(singleuser.First().Id));
                    return Ok(prCompany);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("Branches/Unmapped")]
        //[Route("filter")]
        public IActionResult GetOrganizerUnmappedBranches(string pGuid)
        {

            try
            {
                var singleuser = _identitycontext.Users.Where(a => a.Id.ToLower() == pGuid.ToLower()).ToList();
                if (singleuser.Count > 0)
                {

                    var prCompany = _dbrepo.Organisor.GetOrganisorUnmappedBranches();
                    return Ok(prCompany);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpGet("Companies/Guid")]
        //[Route("filter")]
        public IActionResult GetOrganizerCompaniesByGuid(string pUsername)
        {
            string _pUsername = pUsername;
            try
            {
                var singleuser = _identitycontext.Users.Where(a => a.Id.ToLower() == pUsername.ToLower()).ToList();
                if (singleuser.Count > 0)
                {
                    var prCompany = _dbrepo.Organisor.GetOrganisorCompanies(singleuser.First().Id);

                    //var prCompany = _context.ORGANISER_COMPANY.FromSqlRaw<ORGANISER_COMPANY>(_dataservice.GetOrganiser_Companies(singleuser.First().Id));
                    return Ok(prCompany);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("Companies")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(CCUR company)
        {
            await _context.CCUR.AddAsync(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { pid = company.USERID }, company);

        }


        [HttpDelete("Companies")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> DeleteCCUR(CCUR company)
        {

            _context.CCUR.Remove(company);
            await _context.SaveChangesAsync();

            return Ok(0);
            //DeletedAtAction(nameof(GetById), new { pid = company.USERID }, company);

        }

        [HttpGet("StatsBranch")]
        public IActionResult GetStatsBranch(string pUName)
        {
            string _pRegion = pUName;

            var singleuser = _identitycontext.Users.Where(a => a.Email.ToLower() == pUName.ToLower()).ToList();
            if (singleuser.Count > 0)
            {

                var prCompany = _context.STATSBRANCH.FromSqlRaw<STATSBRANCH>(_dataservice.GetOrganiser_BranchStats(singleuser.First().Id));
                return prCompany == null ? NotFound() : Ok(prCompany);
            } else
            { return NotFound(); }

        }

        [HttpGet("StatsRegion")]
        //[Route("filter")]
        public IActionResult GetStatsRegion(string pUName)
        {
            string _pRegion = pUName;
            //var prCompany = _context.Database.SqlQuery<MCOMPANY>($"SELECT MCOMPANY.* FROM MCOMPANY INNER JOIN CCUR ON CCUR.SRCID = MCOMPANY.COMPANYID AND CCUR.SRCOBJ = {"MUSER"} INNER JOIN MUSER ON MUSER.USERID = CCUR.USERID AND MUSER.UNAME = {pUsername}");

            var prCompany = _context.STATSREGION.FromSqlRaw<STATSREGION>($"SELECT COUNT(*) MEMBERS" +
                ", ml.LOCALITYID, MAX(ml.DESCRIPTION) REGIONAME, MIN(m2.PERIODID) MIN_PERIODID " +
                ", MIN(mpi.FYEARMONTH) MIN_FYEARMONTH, MAX(m2.PERIODID) MAX_PERIODID, MAX(mpa.FYEARMONTH) MAX_FYEARMONTH " +
                " FROM MMEMBER m " +
                " INNER JOIN MMLP m2 ON m2.MEMBERID = m.MEMBERID " +
                " INNER JOIN MPERIOD mpi ON m2.PERIODID = mpi.PRID " +
                " INNER JOIN MPERIOD mpa ON m2.PERIODID = mpa.PRID " +
                " INNER JOIN CLOCALITY clo ON clo.CITYID = m.CITYID " +
                " INNER JOIN MLOCALITY ml ON ml.LOCALITYID = clo.LOCALITYID " +
                " WHERE m2.PERIODID >= (SELECT MAX(PRID) - 2 FROM MPERIOD WHERE PSTATUSID = 42) " +
                " AND ml.LOCALITYID = CASE WHEN @p0 IS NULL THEN ml.LOCALITYID ELSE @p0 END " +
                " GROUP BY ml.LOCALITYID", _pRegion);
            return prCompany == null ? NotFound() : Ok(prCompany);

        }

        [HttpGet("pid")]
        [ProducesResponseType(typeof(MUSER),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int pid) 
        {
            var company = await _context.MUSER.FindAsync(pid);
            return company == null ? NotFound() : Ok(company);
        }

        [AllowAnonymous]
        [HttpGet("MyPhoto")]
        [ProducesResponseType(typeof(MUSER), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrganizerImage(int pObjectId)
        {
            var _fileinfo = _dataservice.GetObjectFileName(IXMFileType.Image, pObjectId);
            var _oimage = _commonRepo.FileService.GetDocumentData(_fileinfo.Item2);

            return _oimage.Item1 == -1 ? 
                    NotFound(_oimage.Item2) : 
                    Ok(_oimage.Item2);
        }


        [HttpGet("MyPhotoURI")]
        [ProducesResponseType(typeof(MUSER), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrganizerImageURI(int pObjectId)
        {
            var _fileinfo = _dataservice.GetObjectFileName(IXMFileType.Image, pObjectId);
            return _fileinfo == null ? NotFound() : Ok(_fileinfo.Item2);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(MUSER company) 
        {
            await _context.MUSER.AddAsync(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new {pid = company.USERID },company);

        }

        [AllowAnonymous]
        [HttpPut("MyPhoto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task <IActionResult> Update([FromForm] MOBJECT_DOC model) 
        {
            var status = new IxmReturnStatus();
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.StatusMessage = "Please pass a valid dataset with your request.";
                _logger.LogTrace(status.StatusMessage);
                return Ok(status);

            }

            if (model.ORGIMAGEFILE != null)
            {
                var _fileresult = _commonRepo.FileService.SaveImageFile(model, model.ORGIMAGEFILE);
                if (_fileresult.Item1 == 1) 
                {
                    //model.DOCUMENTNAME = _fileresult.Item2; //name of image
                    //model.SFOLDERNAME = _fileresult.Item3;
                    //model.FILESIZE = _fileresult.Item4;

                }
                else
                {
                    status.StatusCode = -1;
                    status.StatusMessage = "Error on updating Organizer " + _fileresult.Item2;
                    _logger.LogTrace(status.StatusMessage);
                    return Ok(status);

                }

                Tuple<int, string> _organizerresult = new Tuple<int,string>(-1,"");
                
                if (model.OBJECTID == -1)
                {                    
                    _organizerresult = _docrepo.Document.AddEditDocumentToDB(_logger, model);
                    //if (_organizerresult == null) _logger.LogTrace("Could not add Organizer Image");

                } else if (model.OBJECTID > 0)
                {
                    var eresult = _docrepo.Document.EditOrganizerImage(model);
                }

                if (_organizerresult.Item1 > 0)
                {
                    status.StatusCode = 1;
                    status.StatusMessage = "Updated Successfully";

                }
                else
                { 
                    status.StatusCode = -1;
                    status.StatusMessage = "Error on updating Organizer";
                    _logger.LogTrace(status.StatusMessage);

                }

                return Ok(status);

            }

            
            return NoContent();
        }

        [HttpDelete("{pid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> DeleteOrg(int pid) 
        { 
            var companyToDelete = await _context.MCOMPANY.FindAsync(pid);
            if (companyToDelete == null) return NotFound();

            _context.MCOMPANY.Remove(companyToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        
        }
    }
}
