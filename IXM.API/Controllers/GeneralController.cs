using IXM.Common;
using IXM.DB.Services;
using IXM.DB;
using IXM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FirebirdSql.EntityFrameworkCore.Firebird;

namespace IXM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private readonly IXMDBContext _context;
        private readonly IIXMTransactionRepo _dataservice;
        private readonly IIXMDBRepo _general;
        private readonly IDataValidator _datavalidator;
        private readonly IConfiguration _configuration;
        private readonly IXMDBIdentity _identitycontext;
        private readonly ILogger<TransactionController> _logger;
        private readonly UserManager<ApplicationUser> _usermanager;
        public GeneralController(IXMDBContext context, IXMDBIdentity identity,
                                    ILogger<TransactionController> logger,
                                    UserManager<ApplicationUser> usermanager,
                                    IIXMTransactionRepo dataservice,
                                    IIXMDBRepo general,
                                    IDataValidator datavalidator,
                                    IConfiguration configuration)
        {
            _context = context;
            _identitycontext = identity;
            _usermanager = usermanager;
            _logger = logger;
            _general = general;
            _datavalidator = datavalidator;
            _dataservice = dataservice;
            _configuration = configuration;
        }

        [HttpGet("PrivacyNotice")]
        public IActionResult Get()
        {

            var prData = "Privacy Notice";
            return prData == null ? NotFound() : Ok(prData);

        }


        [AllowAnonymous]
        [HttpPost("StatusUpdate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> StatusUpdate(string TBNAME, string KEYFIELD, string KEYVALUE, string UPDFIELD, int UPDVALUE)
        {
            if (UPDFIELD.Contains("STATUS"))
            {

                ApplicationUser usr = await _general.General.GetCurrentUserAsync(HttpContext);

                if (usr != null)
                {

                    var lScript = _general.CustomUpdates.StatusUpdate(usr.SYSTEM_UNAME, TBNAME, KEYFIELD, KEYVALUE, UPDFIELD, UPDVALUE);
                } else
                {
                    _logger.LogError("Incorrect User Access for this function 'StatusUpdate'. Aborting");
                }

            } else
            {
                _logger.LogInformation("Status Update :: Call is only allowed for Status related fields");
            }



            return Ok();

        }

    }



}
