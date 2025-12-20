using IXM.DB;
using IXM.Models;
using IXM.Models.Events;
using IXM.Common.Notification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IXM.API.Services;
using Microsoft.AspNetCore.Authorization;
using IXM.Common;
using IXM.SQL;

namespace IXM.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EventsController : ControllerBase
           {
        private readonly IXMDBContext _context;
        private readonly IXMSystemDBContext _systemcontext;
        private readonly IDataService _dataservice;
        private readonly IEventSQL _eventSql;
        private readonly IIXMDBRepo _dbrepo;
        private readonly IIXMCommonRepo _commonrepo;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DistributionController> _logger;

        public EventsController(
            
            IXMDBContext context,
            IConfiguration configuration,
            IIXMCommonRepo commonrepo,
            IXMSystemDBContext systemcontext,
            IIXMDBRepo dbrepo,
            ILogger<DistributionController> logger,
            IDataService dataservice

            )

        { 
            _systemcontext = systemcontext;
            _context = context;
            _dataservice = dataservice;
            _commonrepo = commonrepo;
            _configuration = configuration;
            _dbrepo = dbrepo;
            _logger = logger;
            _eventSql = new EventSQL();

        }

        [AllowAnonymous]
        [HttpGet("EvtProject")]
        public async Task<IActionResult> GetEventProject()
        {

            var prMembers = await _dbrepo.Events.GetEvtProject();

            return prMembers == null ? NotFound() : Ok(prMembers);

        }

        [AllowAnonymous]
        [HttpPost("EvtProject")]
        public async Task<IActionResult> PostEventCreate([FromQuery] int pECOMPID, [FromQuery] int pSystemID, [FromBody] TPROJECTWr pPROJECT)
        {

            var prMembers = await _dbrepo.Events.Modify_EvtProject(_commonrepo.Notification, pPROJECT, pSystemID);

            return prMembers == null ? NotFound() : Ok(prMembers);

        }


        [AllowAnonymous]
        [HttpGet("EvtEvent")]
        public async Task<IActionResult> GetEvtEvent()
        {

            var prMembers = await _dbrepo.Events.GetEvtEvent();
            return prMembers == null ? NotFound() : Ok(prMembers);

        }


        [AllowAnonymous]
        [HttpPost("EvtEvent")]
        public async Task<IActionResult> PostEventCreate([FromQuery] int pSystemID, [FromBody] TEVENTWr pEVENT)
        {

            var prMembers = await _dbrepo.Events.Modify_EvtEvent(_commonrepo.Notification, pEVENT, pSystemID);

            return prMembers == null ? NotFound() : Ok(prMembers);

        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var prData = _context.MCOMPANY.FromSqlRaw<MCOMPANY>($"SELECT FIRST 500 m.*" +
                $" FROM MEVENT m ");                //.Take(1000).ToList();
            return prData == null ? NotFound() : Ok(prData);

        }

        [AllowAnonymous]
        [HttpPost("EventAttend")]
        public async Task<IActionResult> PostEventAttend(int pECOMPID, int pSystemID, TPRJEVTD pPRJEVTD)
        {

            var prMembers = _dbrepo.Events.EventAttend(_commonrepo.Notification, pECOMPID, pPRJEVTD, pSystemID);
            //var user = User?.Identity?.Name ?? "system";
            return CreatedAtAction(nameof(Get), new { id = prMembers }, prMembers);

        }


        [AllowAnonymous]
        [HttpPost("Event")]
        public IActionResult PostEventCreate([FromQuery] int pECOMPID, [FromQuery] int pSystemID, [FromBody] TPRJEVT pPRJEVT)
        {

            var prMembers = _dbrepo.Events.Event(_commonrepo.Notification, pPRJEVT, pSystemID);

            return prMembers == null ? NotFound() : Ok(prMembers);

        }

        [AllowAnonymous]
        [HttpGet("EventSearch")]
        public IActionResult GetEventByCriteria(string? pSearchString, string? pSearchField)
        {

            //var prMembers = _systemcontext.TPRJEVTD_DISPLAY.FromSqlRaw<TPRJEVT_DISPLAY>(_dataservice.GetEventsListing(pSearchString, pSearchField));
            var prMembers = _systemcontext.TPRJEVT_DISPLAY.FromSqlRaw<TPRJEVT_DISPLAY>(_eventSql.GetEventsListing(pSearchString, pSearchField));
            return prMembers == null ? NotFound() : Ok(prMembers);

        }

        [AllowAnonymous]
        [HttpGet("EventDetailSearch")]
        public IActionResult GetDiscributionSurveyStats(string? pSearchString, string? pSearchField)
        {

            var prMembers = _systemcontext.TPRJEVTD_DISPLAY.FromSqlRaw<TPRJEVTD_DISPLAY>(_eventSql.GetEventDetailListing(pSearchString, pSearchField));
            return prMembers == null ? NotFound() : Ok(prMembers);

        }


        [AllowAnonymous]
        [HttpGet("EventDetailScan")]
        public IActionResult GetEventScan(string? pEventId, string? pSearchValue, string? pSearchField)
        {

            var prMembers = _systemcontext.TPRJEVTD_DISPLAY.FromSqlRaw<TPRJEVTD_DISPLAY>(_eventSql.GetEventDetailScan(pEventId,pSearchValue, pSearchField));
            var Baseurl = _configuration.GetConnectionString("BaseURL");

            if (prMembers != null)
            {
                if (prMembers.Count() > 0)
                {
                    prMembers.First().DOCUMENT_URI = Baseurl.ToString() + "APPFILES/" + prMembers.First().CODE_FPATH + "/" + prMembers.First().PDOCUMENTNAME;
                    //+ "/" + prMembers.First().PDOCUMENTNAME;
                }

            }


            return prMembers == null ? NotFound() : Ok(prMembers);

        }

        [HttpGet("EventProgramme")]
        public IActionResult GetEventProgramme(string pEventId)
        {

            var prMembers = _systemcontext.TPRJEVTS.FromSqlRaw<TPRJEVTS>(_eventSql.GetEventProgramme(pEventId));
            return prMembers == null ? NotFound() : Ok(prMembers);

        }



        [HttpGet("EventStatsComponent")]
        public IActionResult GetEventComponent(string pEventId)
        {

            var prMembers = _systemcontext.REP_EVTSTATS_S.FromSqlRaw<REP_EVTSTATS_S>(_eventSql.GetEventStatsComponent(pEventId));
            return prMembers == null ? NotFound() : Ok(prMembers);

        }


        [HttpGet("EventComponents")]
        public IActionResult GetEventComponents(string pEventId)
        {

            var prCEVT = _systemcontext.VCEVT_COMPONENT.FromSqlRaw<VCEVT_COMPONENT>(_eventSql.GetEventComponents(pEventId)).ToList();


            var prPEVT = _systemcontext.TPRJEVT_DISPLAY.FromSqlRaw<TPRJEVT_DISPLAY>(_eventSql.GetEventsListing(pEventId, "PEVTID"));
            var prMEVT = _systemcontext.MEVENT_COMPONENT.ToList();
            var prMMSG = _context.MMSG.ToList();

            var uRole = (from cevt in prCEVT


                         join pevt in prPEVT on cevt.PEVTID equals pevt.PEVTID
                         into pevtjoiner
                         from pevtsy in pevtjoiner.DefaultIfEmpty()

                         join mevt in prMEVT on cevt.ECOMPID equals mevt.ECOMPID
                         into evtjoiner
                         from evtsy in evtjoiner.DefaultIfEmpty()

                         join mmsg in prMMSG on cevt.MSGID equals mmsg.MSGID
                         into msgjoiner
                         from msgsy in msgjoiner.DefaultIfEmpty()


                         select new
                         {
                             cevt.ECOMPID,
                             cevt.PEVTID,
                             cevt.MSGID,
                             cevt.MSGACTIVE,
                             cevt.ISACTIVE,
                             EVENT_DESCRIPTION = pevtsy != null ? pevtsy.DESCRIPTION : String.Empty,
                             evtsy.DESCRIPTION,
                             SMESSAGE = msgsy != null ? msgsy.SMESSAGE : String.Empty

                         });

            return uRole == null ? NotFound() : Ok(uRole);

        }


        [HttpGet("EventStatsEmpType")]
        public IActionResult GetEventEmpType(string pEventId)
        {

            var prMembers = _systemcontext.REP_EVTSTATS_S.FromSqlRaw<REP_EVTSTATS_S>(_eventSql.GetEventStatsEmpType(pEventId));
            return prMembers == null ? NotFound() : Ok(prMembers);

        }


        [HttpGet("EventDetailComponent")]
        public IActionResult GetEventDetailComponent(string pEventId)
        {

            var prMembers = _systemcontext.TPRJEVTS.FromSqlRaw<TPRJEVTS>(_eventSql.GetEventDetailComponent(pEventId));
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


