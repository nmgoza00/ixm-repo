using IXM.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IXM.Models;
using IXM.Constants;

namespace IXM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppLogController : ControllerBase
    {

        private readonly IIXMDBRepo _dbrepo;
        private readonly UserManager<ApplicationUser> _userManager;
        public AppLogController(
            IIXMDBRepo dbrepo,
            UserManager<ApplicationUser> userManager)
        {

            _dbrepo = dbrepo;
            _userManager = userManager;
        }

        [HttpGet, Route("")]
        public IActionResult HeartBeat()
        {
            return Ok($"Heartbeat @ {DateTime.Now}");
        }


        [AllowAnonymous]
        [HttpGet, Route("AppDBLogE")]
        public async Task<IActionResult> GetAppDBLog(string SystemId, IxmAppSourceObjects SourceObj, string SourceId)
        {

            var lSys = await _dbrepo.AppLog.GetFromLogTableE(SourceObj, SystemId, SourceId);

            if (lSys == null)
            {
                return BadRequest("Invalid Token");
            }

            return Ok(lSys);


        }
        [AllowAnonymous]
        [HttpGet, Route("AppDBLogF")]
        public async Task<IActionResult> GetAppDBLogF(string SystemId, IxmAppSourceObjects SourceObj, string SourceId)
        {

            var lSys = await _dbrepo.AppLog.GetFromLogTableF(SourceObj, SystemId, SourceId);

            if (lSys == null)
            {
                return BadRequest("Invalid Token");
            }

            return Ok(lSys);


        }
        [AllowAnonymous]
        [HttpGet, Route("AppDBLogR")]
        public async Task<IActionResult> GetAppDBLogR(string SystemId, IxmAppSourceObjects SourceObj, string SourceId)
        {

            var lSys = await _dbrepo.AppLog.GetFromLogTableR(SourceObj, SystemId, SourceId);

            if (lSys == null)
            {
                return BadRequest("Invalid Token");
            }

            return Ok(lSys);


        }
    }
}