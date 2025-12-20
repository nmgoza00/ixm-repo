using IXM.Common;
using IXM.DB.Services;
using IXM.DB;
using IXM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace IXM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DBTasksController : ControllerBase
    {
        private readonly IXMDBContext _context;
        private readonly IDataValidator _datavalidator;
        private readonly IConfiguration _configuration;
        private readonly IIXMDBRepo _ixmdb;
        private readonly IXMDBIdentity _identitycontext;
        private readonly ILogger<TransactionController> _logger;
        private readonly UserManager<ApplicationUser> _usermanager;
        public DBTasksController(IXMDBContext context, IXMDBIdentity identity,
                                    ILogger<TransactionController> logger,
                                    UserManager<ApplicationUser> usermanager,
                                    IIXMDBRepo ixmdb,
                                    IDataValidator datavalidator,
                                    IConfiguration configuration)
        {
            _context = context;
            _identitycontext = identity;
            _usermanager = usermanager;
            _logger = logger;
            _ixmdb = ixmdb;
            _datavalidator = datavalidator;
            _configuration = configuration;
        }

        [Authorize]
        [HttpPost("StatusTriggerCreate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> StatusTriggerCreate(string TBNAME, string CHKFIELD, string KEYFIELD)
        {
            _ixmdb.DBTasks.StatusTriggerCreate(TBNAME, CHKFIELD, KEYFIELD);
            return Ok();
        }

        [Authorize]
        [HttpPost("StatusTriggerDrop")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> StatusTriggerDrop(string TBNAME, string KEYFIELD)
        {
            _ixmdb.DBTasks.StatusTriggerDrop(TBNAME, KEYFIELD);
            return Ok();
        }

        [Authorize]
        [HttpPost("MassUpdate_Gender")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> MassUpdate_Gender()
        {
            //_ixmdb.DBTasks.StatusTriggerDrop(TBNAME, KEYFIELD);
            return Ok();

        }

        [Authorize]
        [HttpPost("MassUpdate_Age")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> MassUpdate_Age()
        {
            //_ixmdb.DBTasks.StatusTriggerDrop(TBNAME, KEYFIELD);
            return Ok();

        }

        [Authorize]
        [HttpPost("MassUpdate_MemberStatus")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> MassUpdate_MemberStatus()
        {
            //_ixmdb.DBTasks.StatusTriggerDrop(TBNAME, KEYFIELD);
            return Ok();

        }

        [Authorize]
        [HttpPost("DBTask_DatabaseSweep")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> DBTask_DatabaseSweep()
        {
            //_ixmdb.DBTasks.StatusTriggerDrop(TBNAME, KEYFIELD);
            return Ok();

        }

        [Authorize]
        [HttpPost("DBTask_DatabaseBackup")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> DBTask_DatabaseBackup()
        {
            //_ixmdb.DBTasks.StatusTriggerDrop(TBNAME, KEYFIELD);
            _ixmdb.DBTasks.DatabaseBackup("NUM/NBCCI/TIRISANO");
            return Ok();

        }


        [Authorize]
        [HttpPost("DBTask_DatabaseStats")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> DBTask_DatabaseStats()
        {
            //_ixmdb.DBTasks.StatusTriggerDrop(TBNAME, KEYFIELD);
            return Ok();

        }

        [Authorize]
        [HttpPost("Reporting_MonthlyFinance")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Reporting_MonthlyFinance()
        {
            //_ixmdb.DBTasks.StatusTriggerDrop(TBNAME, KEYFIELD);
            return Ok();

        }



        [AllowAnonymous]
        [HttpPost("/Task/GetTaskId")]
        public async Task<IActionResult> TaskGetTaskId(List<TPROCESSTASK> tPROCESSTASK)
        {
            var pProcessId = await _ixmdb.DataServices.GetProcessQueueAsync();
            return Ok(pProcessId);

        }

        
        [AllowAnonymous]
        [HttpPost("/Task/TaskCheckQueue")]
        public async Task<IActionResult> TaskGetQueue()
        {
            var pProcessId = await _ixmdb.DataServices.GetProcessQueueAsync();
            return Ok(pProcessId);

        }

        [AllowAnonymous]
        [HttpPost("/Task/TaskProcessQueue")]
        public async Task<IActionResult> TaskProcessStart(List<TPROCESSTASK>? tPROCESSTASK)
        {
            string pProcessId;
            if (tPROCESSTASK == null)
            {

                pProcessId = await _ixmdb.DataServices.ProcessQueueAsync(null);

            } else
            {

                pProcessId = await _ixmdb.DataServices.ProcessQueueAsync(tPROCESSTASK);
                
            }
            return Ok(pProcessId);

        }
        [AllowAnonymous]
        [HttpPost("/Task/TaskProcessStart")]
        public async Task<IActionResult> TaskProcessStart(string pProcessId)
        {

            pProcessId = await _ixmdb.DataServices.ProcessStartAsync(pProcessId);
            return Ok(pProcessId);

        }
        [AllowAnonymous]
        [HttpPost("/Task/TaskProcessComplete")]
        public async Task<IActionResult> TaskProcessComplete(string pProcessId)
        {

            pProcessId = await _ixmdb.DataServices.ProcessCompleteAsync(pProcessId);
            return Ok(pProcessId);

        }

        [AllowAnonymous]
        [HttpDelete("/Task/TaskProcessRemove")]
        public async Task<IActionResult> TaskProcessDelete(string pProcessId)
        {

            pProcessId = await _ixmdb.DataServices.ProcessCompleteAsync(pProcessId);
            return Ok(pProcessId);

        }


        [AllowAnonymous]
        [HttpGet("/Task/TaskProcessCheck")]
        public async Task<IActionResult> TaskProcessCheck(string ProcessId)
        {

            string _param = "1";
            var status = await _ixmdb.DataServices.ProcessCheckStatus(ProcessId);

            return status == null ? NotFound() : Ok(status);

        }




    }



}
