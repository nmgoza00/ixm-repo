using IXM.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IXM.Models;
using IXM.Models.Core;
//using MassTransit;

namespace IXM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceCheckController : ControllerBase
    {

        private readonly IXMDBIdentity _identitycontext;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IPublishEndpoint _publishEndpoint;
        private readonly IConfiguration _configuration;
        public ServiceCheckController(
            IXMDBIdentity identitycontext,
            //IPublishEndpoint publishEndpoint,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager)
        {

            _identitycontext = identitycontext;
            _userManager = userManager;
            //_publishEndpoint = publishEndpoint;
            _configuration = configuration;
        }

        [HttpGet, Route("")]
        public IActionResult HeartBeat()
        {
            return Ok($"Heartbeat @ {DateTime.Now}");
        }


        [AllowAnonymous]
        [HttpGet, Route("VerifyReset")]
        public IActionResult VerifyReset(string code, string email)

        {
            var user = _identitycontext.UserPasswordResets.Where(a => a.PasswordResetToken == code &&
                                                                        a.PasswordResetExpiryDate >= DateTime.Now).FirstOrDefault();
            if (user == null)
            {
                return BadRequest("Invalid Token");
            }

            user.PasswordResetVerifiedDate = DateTime.Now;

            //_identitycontext.UserPasswordResets.Entry(user);
            _identitycontext.SaveChanges();

            return Ok("User was Verified successfully. Thank You.");

        }


        [AllowAnonymous]
        [HttpGet, Route("VerifyCheck")]
        public async Task<IActionResult> VerifyCheck(string pcode, string pemail)
        {

            var userD = await _userManager.FindByEmailAsync(pemail);
            if (userD == null)
            {
                return BadRequest("Invalid Token");
            }
            var user = await _identitycontext.UserPasswordResets.Where(a => a.PasswordResetToken == pcode &&
                                                                      a.UserId == userD.Id.ToString() &&
                                                                      a.PasswordResetExpiryDate >= DateTime.Now &&
                                                                      a.PasswordResetVerifiedDate != null ).FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest("Expired or Not Verified");
            } else return Ok("Verified");


        }

        [AllowAnonymous]
        [HttpGet, Route("MessageQTest")]
        public async Task<IActionResult> MessageQCheck()
        {

            //string lFld = _configuration.GetConnectionString("BaseDocFolder");
            /*await _publishEndpoint.Publish(new MessageQTest
            {
                COMPANYID = -1
            });*/
            return Ok("Verified");

        }
    }
}