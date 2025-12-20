using IXM.DB;
using IXM.Models;
using IXM.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using FirebirdSql.EntityFrameworkCore;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;
using IXM.API.Resources;
using IXM.API.Services;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.PortableExecutable;

namespace IXM.API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly IXMDBContext _context;
        private readonly IDataService _dataservice;
        private readonly ILogger <OrganizerController>_logger;

        public NotesController( IXMDBContext context, 
                                    ILogger<OrganizerController> logger,
                                    IDataService dataservice)
        { 
            
            _context = context;
            _logger = logger;
            _dataservice = dataservice;

        }


        [HttpGet("BCOMPANYID")]
        [ProducesResponseType(typeof(MOBJECT_NOT), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByBCompanyId(int BCOMPANYID)
        {
            var notes = _context.MOBJECT_NOT.Where(a => a.SOURCEOBJ == "MCOMPANY" && a.SOURCEID == BCOMPANYID).ToList();
            return notes == null ? NotFound() : Ok(notes);
        }


        [HttpGet("NOTEID")]
        [ProducesResponseType(typeof(MOBJECT_NOT), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByNoteId(int NOTEID)
        {
            var notes = await _context.MOBJECT_NOT.FindAsync(NOTEID);
            return notes == null ? NotFound() : Ok(notes);
        }



        [HttpGet("Notice")]
        [ProducesResponseType(typeof(MOBJECT_NOT), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByNoticeId(int NOTICEID)
        {
            var notes = await _context.TMB_NOTICE.FindAsync(NOTICEID);
            return notes == null ? NotFound() : Ok(notes);
        }

        [AllowAnonymous]
        [HttpGet("NoticesAll")]
        [ProducesResponseType(typeof(MOBJECT_NOT), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetNotices()
        {
            var notes = await _context.TMB_NOTICE.Where(a => a.ISACTIVE == "Y").Where(b => b.EXPIRATIONDATE >= DateTime.Now).ToListAsync();
            return notes == null ? NotFound() : Ok(notes);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> UpdateNotes(MOBJECT_NOT mobject_note)
        {

           var result = await _dataservice.InsertNote(_logger, mobject_note);
            
            return CreatedAtAction(nameof(mobject_note), new { pid = mobject_note.NOTEID }, mobject_note);

        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<IActionResult> Update(MOBJECT_NOT mobject_note)
        {

            _context.Entry(mobject_note).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }



        /*
        [HttpPost]
        [HttpGet("Mail/CompanyRegCode")]
        public async Task SendMail()
        {

            USER_PASSWORDRESET preset = new USER_PASSWORDRESET();
            /*
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            string returnUrl = Url.Content("~/");

            var EmailConfirmationUrl = Url.Action("VerifyReset", "ServiceCheck", new { code = token, email = pemail }, Request.Scheme);
            //var EmailConfirmationUrl = Url.Page("/", pageHandler: null,
            //values: new { area = "ResetPasswordConfirm", code = token, email = pemail },
            //protocol: Request.Scheme);

            preset.email = pemail;
            preset.message = "Your password Request has been sent to your email.";
            //preset.message = preset.message + EmailConfirmationUrl;
            preset.token = token;

            UserPasswordResets upassreset = new UserPasswordResets
            {
                UserId = user.Id,
                LoginProvider = "Custom",
                PasswordResetToken = token,
                PasswordSalt = token,
                PasswordResetVerifiedDate = null,
                PasswordResetExpiryDate = DateTime.Now.AddDays(1)
            };

            _identitycontext.UserPasswordResets.Add(upassreset);
            await _identitycontext.SaveChangesAsync();

            EMAIL_TEMPLATE email_Template = new EMAIL_TEMPLATE();

            email_Template.ToEmail = pemail;
            email_Template.Subject = "IXM : Forgot Password Request.";
            //email_Template.Message = preset.message;
            email_Template.VerifyLink = EmailConfirmationUrl;
            
            await _notification.SendEmailAsync(IXMMailType.ForgotPassword, ref email_Template);
            

            return Ok();

        }*/




    }
}
