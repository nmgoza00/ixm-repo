using DocumentFormat.OpenXml.Vml.Spreadsheet;
using IXM.Common.Generics;
using IXM.Constants;
using IXM.DB;
using IXM.Models;
using IXM.Models.Events;
using IXM.Models.Write;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using static IXM.Common.Generics.Generics4API;
//using MassTransit;

namespace IXM.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MasterDataController : ControllerBase
    {
        private readonly IXMDBContext _context;
        private readonly IIXMDBRepo _dbrepo;
        private readonly IXMSystemDBContext _systemcontext;


        public MasterDataController(IXMDBContext context, IXMSystemDBContext systemcontext, IIXMDBRepo dbrepo)
        {

            _context = context;
            _systemcontext = systemcontext;
            _dbrepo = dbrepo;

        }
        

        [HttpGet("Sector")]
        [EnableQuery]
        public IActionResult GetSectors()
        {

            var prData = _context.MSECTOR.FromSqlRaw<MSECTOR>($"SELECT v.* " +
                                        $" FROM MSECTOR v ");
            return prData == null ? NotFound() : Ok(prData);

        }

        [AllowAnonymous]
        [HttpPost("Sector")]
        [EnableQuery]
        public async Task<IActionResult> PostSectors([FromBody] MSECTORWr mSECTOR)
        {

            var prData =  await _dbrepo.MasterData.PostSector(mSECTOR);
            return prData == -1 ? NotFound() : Ok(prData);

        }

        [AllowAnonymous]
        [HttpGet("Union")]
        [EnableQuery]
        public async Task<IActionResult> GetUnions()
        {

            //var prData = _context.MUNION.FromSqlRaw<MUNION>($"SELECT v.* " +
            //$" FROM MUNION v ");
            var prData = await _dbrepo.MasterData.GetUnion();
            return prData == null ? NotFound() : Ok(prData);

        }

        [AllowAnonymous]
        [HttpPost("Union")]
        [EnableQuery]
        public async Task<IActionResult> PostUnion([FromBody] MUNIONWr mUNION)
        {

            var prData = await _dbrepo.MasterData.PostUnion(mUNION);
            return prData == null ? NotFound() : Ok(prData);

        }

        [AllowAnonymous]
        [HttpGet("UnionSys")]
        [EnableQuery]
        public async Task<IActionResult> GetUnionSystem()
        {

            //var prData = _context.MUNION.FromSqlRaw<MUNION>($"SELECT v.* " +
            //$" FROM MUNION v ");
            var prData = await _dbrepo.MasterData.GetUnionSystem();
            return prData == null ? NotFound() : Ok(prData);

        }

        [AllowAnonymous]
        [HttpGet("Locality/CityMap")]
        [EnableQuery]

        public async Task<IActionResult> GetLocalityCictyMap()
        {

            var prData = await _dbrepo.MasterData.GetLocalityCityMap();
            return prData == null ? NotFound() : Ok(prData);

        }

        [AllowAnonymous]
        [HttpPost("Locality/CityMap")]
        [EnableQuery]

        public async Task<IActionResult> PostLocalityCictyMap(List<CLOCALITY> cLOCALITies)
        {

            var prData = await _dbrepo.MasterData.PostLocalityCityMap(cLOCALITies);
            return Ok(prData);

        }

        [AllowAnonymous]
        [HttpGet("Locality")]
        [EnableQuery]

        public async Task<IActionResult> GetLocality()
        {

            var prData = await _dbrepo.MasterData.GetLocality();
            //var prData = _context.MLOCALITY.FromSqlRaw<MLOCALITY>($"SELECT v.* " +
                                        //$" FROM MLOCALITY v ");
            return prData == null ? NotFound() : Ok(prData);

        }

        [AllowAnonymous]
        [HttpPost("Locality")]
        [EnableQuery]

        public async Task<IActionResult> PostLocality(MLOCALITYWr mLOCALITY)
        {

            var prData = await _dbrepo.MasterData.PostLocality(mLOCALITY);
            //var prData = _context.MLOCALITY.FromSqlRaw<MLOCALITY>($"SELECT v.* " +
            //$" FROM MLOCALITY v ");
            return prData == null ? NotFound() : Ok(prData);

        }

        [AllowAnonymous]
        [HttpGet("Period")]
        [EnableQuery]

        public async Task <IActionResult> GetPeriod(int? PeriodId)
        {

            CancellationToken cancellationToken = default;

            /*await _messagePublish.PublishAsync(new XLS_REMITTANCE
            {
                COMPANYID = 73
            }, cancellationToken
            );*/
            

            var prData = await _dbrepo.MasterData.GetPeriod(PeriodId);
            return prData == null ? NotFound() : Ok(prData);

        }

        [AllowAnonymous]
        [HttpPost("Period")]
        [EnableQuery]

        public async Task<IActionResult> PostPeriod(MPERIODWr mPERIOD)
        {

            CancellationToken cancellationToken = default;

            var prData = await _dbrepo.MasterData.PostPeriod(mPERIOD);
            return Ok(prData);

        }

        [HttpGet("StatusText")]
        [EnableQuery]

        public async Task<IActionResult> GetStatusText()
        {
            //var prData = _context.MSTATUS_TEXT.FromSqlRaw<MSTATUS_TEXT>($"SELECT v.* " +
              //                          $" FROM MSTATUS_TEXT v ");

            var prData = await _dbrepo.MasterData.GetStatusText();
            return prData == null ? NotFound() : Ok(prData);

        }

        [AllowAnonymous]
        [HttpGet("City")]
        [EnableQuery]
        public async Task<IActionResult> GetCity()
        {

            var prData = await _dbrepo.MasterData.GetCities();
            return prData == null ? NotFound() : Ok(prData);

        }

        [AllowAnonymous]
        [HttpPost("City")]
        [EnableQuery]
        public async Task<ActionResult<ApiResponse<MCITY>>> PostCity([FromBody] MCITY mCITY)
        {

            var result = await _dbrepo.MasterData.PostCity(mCITY);

            return Ok(ApiResponse<MCITY>.Ok(mCITY, result));

            /*
            return result switch
            {
                "created" => Ok(new
                {
                    success = true,
                    message = $"City '{mCITY.DESCRIPTION}' created successfully.",
                    data = new
                    {
                        cityId = mCITY.CITYID,
                        description = mCITY.DESCRIPTION
                    }
                }),

                "updated" => Ok(new
                {
                    success = true,
                    message = $"City '{mCITY.DESCRIPTION}' updated successfully.",
                    data = new
                    {
                        cityId = mCITY.CITYID,
                        description = mCITY.DESCRIPTION
                    }
                }),

                "nochange" => Ok(new
                {
                    success = true,
                    message = "No changes detected. City details are already up to date.",
                    data = new
                    {
                        cityId = mCITY.CITYID,
                        description = mCITY.DESCRIPTION
                    }
                }),

                "duplicate_id" => BadRequest(new
                {
                    success = false,
                    message = "City ID already exists."
                }),

                "duplicate_name" => BadRequest(new
                {
                    success = false,
                    message = "City name already exists."
                }),

                _ => BadRequest(new
                {
                    success = false,
                    message = "City save failed."
                })
            };
            */




            ////rn prData == "nil" ? NotFound() : Ok(prData);

        }
        [AllowAnonymous]
        [HttpGet("CityType")]
        [EnableQuery]
        public async Task<IActionResult> GetCityType()
        {

            var prData = await _dbrepo.MasterData.GetCityType();
            return prData == null ? NotFound() : Ok(prData);

        }

        [AllowAnonymous]
        [HttpGet("Gender")]
        [EnableQuery]
        public async Task<IActionResult> GetGender()
        {

            var prData = await _dbrepo.MasterData.GetGender();
            return prData == null ? NotFound() : Ok(prData);

        }

        [AllowAnonymous]
        [HttpGet("Language")]
        [EnableQuery]
        public async Task<IActionResult> GetLanguage()
        {

            var prData = await _dbrepo.MasterData.GetLanguage();
            return prData == null ? NotFound() : Ok(prData);

        }

        [AllowAnonymous]
        [HttpGet("MaritalStatus")]
        [EnableQuery]
        public async Task<IActionResult> GetMaritalStatus()
        {

            var prData = await _dbrepo.MasterData.GetMaritalStatus();
            return prData == null ? NotFound() : Ok(prData);

        }
        [HttpGet("CaseType")]
        [EnableQuery]
        public async Task<IActionResult> GetCaseType()
        {

            List<MCASETYPE> prData = await _dbrepo.MasterData.GetCaseType();
            return prData == null ? NotFound() : Ok(prData);

        }

        [HttpPost("CaseType")]
        [EnableQuery]
        public async Task<IActionResult> PostCaseType(MCASETYPEWr mCASETYPE)
        {

            int prData = await _dbrepo.MasterData.PostCaseType(mCASETYPE);
            return Ok(prData);

        }

        [HttpGet("ExtPosition")]
        [EnableQuery]
        public async Task<IActionResult> GetExtPosition()
        {

            List<MEXTPOSITION> prData = await _dbrepo.MasterData.GetExtPosition();
            return prData == null ? NotFound() : Ok(prData);

        }

        [HttpGet("IntPosition")]
        [EnableQuery]
        public async Task<IActionResult> GetIntPosition()
        {

            List<MINTPOSITION> prData = await _dbrepo.MasterData.GetIntPosition();
            return prData == null ? NotFound() : Ok(prData);

        }

        [HttpPost("IntPosition")]
        [EnableQuery]
        public async Task<IActionResult> PostIntPosition(MINTPOSITIONWr mMINTPOSITION)
        {

            int prData = await _dbrepo.MasterData.PostIntPosition(mMINTPOSITION);
            return Ok(prData);

        }

        [AllowAnonymous]
        [HttpGet("Province")]
        [EnableQuery]

        public async Task<IActionResult> GetProvince()
        {

            //var prData = _context.MPROVINCE.FromSqlRaw<MPROVINCE>($"SELECT v.* " +
            //             $" FROM MPROVINCE v ");
            var prData = await _dbrepo.MasterData.GetProvince();
            return prData == null ? NotFound() : Ok(prData);

        }

        [AllowAnonymous]
        [HttpGet("Status")]
        [EnableQuery]
        public IActionResult GetStatus(string? STATUS_TYPE)
        {
            string lWhere = "";
            if (STATUS_TYPE is not null)
            {
                lWhere = "WHERE STATUS_TYPE = '" + STATUS_TYPE + "'";

            }
            var prData = _context.VMSTATUS.FromSqlRaw<VMSTATUS>($"SELECT v.* FROM VMSTATUS v " + lWhere);
            return prData == null ? NotFound() : Ok(prData);

        }

        [AllowAnonymous]
        [HttpGet("DataValueMapping")]
        [EnableQuery]
        public IActionResult GetDataValueMapping()
        {

            var prData = _context.DATA_VALUEMAPPING.FromSqlRaw<DATA_VALUEMAPPING>($"SELECT v.* FROM DATA_VALUEMAPPING v ");
            return prData == null ? NotFound() : Ok(prData);

        }

        [HttpGet("BankName")]
        [EnableQuery]
        public IActionResult GetBankName()
        {

            var prData = _context.MBANKNAME.FromSqlRaw<MBANKNAME>($"SELECT v.* FROM MBANKNAME v ");
            return prData == null ? NotFound() : Ok(prData);

        }

        [HttpGet("UserGroup")]
        [EnableQuery]
        public async Task<IActionResult> GetUserGroup()
        {

            var prData = await _dbrepo.MasterData.GetUserGroup();
            return prData == null ? NotFound() : Ok(prData);

        }

        [AllowAnonymous]
        [HttpGet("CompanyType")]
        [EnableQuery]
        public async Task<IActionResult> GetCompanyType()
        {

            var prData = await _context.MCOMPTYPE.ToListAsync();
            return prData == null ? NotFound() : Ok(prData);

        }


        [AllowAnonymous]
        [HttpGet("Event/Component")]
        [EnableQuery]

        public IActionResult GetEventComponent(string pEventId)
        {

            var prData = _systemcontext.MEVENT_COMPONENT.FromSqlRaw<MEVENT_COMPONENT>($"SELECT v.* FROM MEVENT_COMPONENT v ");
            return prData == null ? NotFound() : Ok(prData);

        }

        /*
        [AllowAnonymous]
        [HttpGet("Event/ComponentConfig")]

        public IActionResult GetEventComponentConfig(string pEventId, string pComponentId)
        {

            var prData = _context.CEVT_COMPONENT.FromSqlRaw<CEVT_COMPONENT>($"SELECT v.* " +
                                        $" FROM CEVT_COMPONENT v ");
            return prData == null ? NotFound() : Ok(prData);

        }*/

        [HttpGet("CompanyTypeForCompany")]
        [EnableQuery]
        public IActionResult GetCompanyTypeById(int pCompTypID)
        {

            var prData = _context.MCOMPTYPE.FromSqlRaw<MCOMPTYPE>($"SELECT v.* " +
                                        $" FROM MCOMPTYPE v " +
                                        $" WHERE v.LEVEL_N IN (SELECT ALVL FROM MCOMPTYPE WHERE COMPTYPID = @p0)", pCompTypID);
            return prData == null ? NotFound() : Ok(prData);

        }

        [AllowAnonymous]
        [HttpGet("Caption")]
        [EnableQuery]
        public IActionResult GetCaption(IxmAppCaptionObject CaptionObject)
        {

            string lSql = "SELECT * FROM APP_OBJECTSETUP WHERE OBJECTNAME = '" + CaptionObject.ToString() + "' ORDER BY FIELD_POSITION";
            var DataCaption = _context.TCAPTIONS.FromSqlRaw<TCAPTIONS>(lSql).ToList();
            return DataCaption == null ? NotFound() : Ok(DataCaption);

        }
    }
}
