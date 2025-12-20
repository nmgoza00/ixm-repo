using DocumentFormat.OpenXml.InkML;
using IXM.API.Services;
using IXM.Common;
using IXM.Constants;
using IXM.DB;
using IXM.GeneralSQL;
using IXM.Models;
using IXM.Models.Core;
using IXM.Models.Write;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace IXM.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IXMDBContext _context;
        private readonly IXMDBIdentity _identitycontext;
        private readonly IXMAppDBContext _appctrlcontext;
        private readonly IConfiguration _configuration;
        private readonly IDataService _dataservice;
        private readonly IIXMDBRepo _general;
        private readonly ILogger<UserController> _logger;
        private readonly IIXMCommonRepo _commonrepo;
        private readonly IQueryRepository _queryRepository;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        
        private readonly SignInManager<ApplicationUser> _signInManager;


        public UserController(IXMDBContext context,
            IXMDBIdentity identitycontext,
            IXMAppDBContext appctrlcontext,
                                    ILogger<UserController> logger,
                                    IConfiguration configuration,
                                    IIXMDBRepo general,
                                    IDataService dataservice,
                                    IIXMCommonRepo commonrepo,
                                    IQueryRepository queryRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _general = general;
            _identitycontext = identitycontext;
            _appctrlcontext = appctrlcontext;
            _configuration = configuration;
            _logger = logger;
            _roleManager = roleManager;
            _dataservice = dataservice;
            _userManager = userManager;
            _signInManager = signInManager;
            _commonrepo = commonrepo;
            _queryRepository = queryRepository;

        }

        [AllowAnonymous]
        [HttpGet("Tester")]
        public IActionResult Tester()
        {

            EMAIL_TEMPLATE email_Template = new EMAIL_TEMPLATE();

            email_Template.ToEmail = "nmgoza@inchsolution.co.za";
            var result = _commonrepo.Notification.SendEmailAsync(IXMMailType.Register, ref email_Template );

            if (result.Status == TaskStatus.RanToCompletion)
            {

                return Ok();
            } else return BadRequest(result.ToString());

        }

        /*[HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Update(MUSER mMUSER)
        {

            _dataservice.ModifyUser(_logger, mMUSER);

            return NoContent();
        }*/


        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> InsertUser(MUSERWr mMUSER)
        {
            var lret = _dataservice.InsertUser(_logger, mMUSER);

            return Ok(lret);
            //CreatedAtAction(nameof(mMUSER), new { pid = mMUSER.USERID }, mMUSER);

        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MUSER), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {

            var usert = await _context.MUSER.Where(l => l.USERID == id).
                                                   Take(1).
                                                   FirstAsync();

            return usert == null ? NotFound() : Ok(usert);
        }


        [HttpGet]
        [Route("/Identity/Users")]
        [Route("/Identity/UsersL/{uid}")]
        public async Task<IActionResult> GetIdentityList(String? uid)
        {

            try
            {

                var userlist = await _identitycontext.Users.ToListAsync();
                var roles = await _identitycontext.Roles.ToListAsync();
                var userroles = await _identitycontext.MUSER_ROLE.ToListAsync();
                var systemusers = await _context.MUSER.ToListAsync();
                var usersystem = await _identitycontext.MUSER_SYSTEM.ToListAsync();
                var msystem = await _identitycontext.MSYSTEM.ToListAsync();
                // var system = await _identitycontext.MSYSTEM.ToListAsync();
                var uemp = await _context.MEMPLOYEE_DISPLAY.FromSqlRaw<MEMPLOYEE_DISPLAY>(_dataservice.GetLinkedUserToEmployee("")).ToListAsync();
                var udev = _identitycontext.MUSER_DEVICE.Select(a => new { a.INSERT_DATE, a.LOGINS, a.EMAIL }).Distinct().ToList();

                var uRole = (from ur in userlist

                             join lsy in usersystem on ur.Id equals lsy.UserId
                             into lsyjoiner
                             from urlsy in lsyjoiner.DefaultIfEmpty()

                             join msy in msystem on urlsy.SystemId equals msy.SYSTEMID
                             into msyjoiner
                             from umsy in msyjoiner.DefaultIfEmpty()

                             join uem in uemp on ur.Id equals uem.ID_USERID
                             into uemjoiner
                             from uruem in uemjoiner.DefaultIfEmpty()

                             join su in systemusers on ur.Id equals su.AUTHCODE
                             into sujoiner
                             from ursu in sujoiner.DefaultIfEmpty()

                             join ude in udev on ur.Email equals ude.EMAIL
                             into udejoiner
                             from urude in udejoiner.DefaultIfEmpty()

                             join lro in userroles on ur.Id equals lro.UserId
                             into lrojoiner
                             from urlro in lrojoiner.DefaultIfEmpty()

                             join sro in roles on urlro != null ? urlro.RoleId : "" equals sro.Id
                             into srojoiner
                             from ursro in srojoiner.DefaultIfEmpty()

                             select new
                             {
                                 UserStatus = urlsy != null ? "Verified" : urlro != null ? "Partial" : "Guest",
                                 ur.Id,
                                 CellNumber = ur.PhoneNumber,
                                 ur.UserName,
                                 Email = ur.Email,
                                 SystemId = urlsy != null ? urlsy.SystemId : string.Empty,
                                 SystemName = umsy != null ? umsy.SYSTEMNAME : string.Empty,
                                 RoleId = ursro != null ? ursro.Id : string.Empty,
                                 RoleName = ursro != null ? ursro.Name : string.Empty,
                                 EmployeeId = uruem != null ? uruem.EMPID : -99,
                                 Employee_FullName = uruem != null ? uruem.FULLNAME : string.Empty,
                                 InsertDate = urude != null ? urude.INSERT_DATE : null,
                                 Logins = urude != null ? urude.LOGINS : null,
                                 SYSTEM_USERID = ursu != null ? ursu.USERID : -99

                             }).DistinctBy(a => (a.Id));


                if ((uid != null) && (uRole != null))
                {

                    var uRole1 = uRole.Where(a => a.Email.ToLower() == uid.ToLower()).ToList();

                    return Ok(uRole1);

                }
                else return Ok(uRole);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }


        [HttpGet("/Identity/Users/{uid}")]
        public async Task<IActionResult> GetIdentityByIdList(String uid)
        {
            var singleuser = await _identitycontext.Users.Where(a => a.Id == uid).FirstAsync();
            return Ok(singleuser);

        }

        [HttpGet("/Identity/UserLogins")]
        public async Task<IActionResult> GetIdentityUserLoginsList()
        {

            var userlist = await _identitycontext.UserLogins.ToListAsync();
            return Ok(userlist);

        }

        [HttpGet("/Identity/UserTokens")]
        public async Task<IActionResult> GetIdentityTokenList()
        {

            var userlist = await _identitycontext.UserTokens.ToListAsync();
            return Ok(userlist);

        }

        [AllowAnonymous]
        [HttpGet("/Identity/UserRoles/{uid}")]
        public async Task<IActionResult> GetIdentityUserRolesByUserList(string uid, IGeneralSQL _generalSql)
        {

            var userrole = await _identitycontext.MUSER_ROLE.Where(a => a.UserId == uid).ToListAsync();
            var sysuserrole = await _context.CUSER_GROUP.FromSqlRaw(_generalSql.GetSystemUserGroup(uid)).ToListAsync();
            var roles = await _identitycontext.Roles.ToListAsync();

            var uRole = (from ur in userrole

                         join sro in roles on ur != null ? ur.RoleId : "" equals sro.Id
                         into srojoiner
                         from ursro in srojoiner.DefaultIfEmpty()

                         join sur in sysuserrole on ur != null ? ur.UserId : "" equals sur.AUTHCODE
                         into surjoiner
                         from suro in surjoiner.DefaultIfEmpty()

                         select new
                         {
                             ur.UserId,
                             RoleId = ursro != null ? ursro.Id : string.Empty,
                             RoleName = ursro != null ? ursro.Name : string.Empty,
                             UGCode = suro != null ? suro.UGCODE : string.Empty

                         }).DistinctBy(a => (a.UGCode));

            return Ok(uRole);

        }


        [HttpPost("/Identity/UserRoles/{puser}/{prole}")]
        public async Task<IActionResult> PutIdentityRolesList(string puser, string prole)
        {

            var user = await _identitycontext.MUSER_ROLE.Where(a => a.UserId == puser)
                .Where(b => b.RoleId == prole).ToListAsync();

            if (user.Count == 0 ) 
            {
                MUSER_ROLE ur = new MUSER_ROLE() { UserId = puser, RoleId = prole };
                _identitycontext.MUSER_ROLE.Add(ur);
                _identitycontext.SaveChanges();
            }
            
            return Ok();
        }


        [HttpDelete("/Identity/UserRoles/{puser}/{prole}")]
        public async Task<IActionResult> DeleteIdentityUserRolesList(string puser, string prole)
        {
            var user = await _identitycontext.MUSER_ROLE.Where(a => a.UserId == puser)
                .Where(b => b.RoleId == prole).ToListAsync();

            int v = _identitycontext.MUSER_ROLE.Where(a => a.UserId == puser && a.RoleId == prole ).ExecuteDelete();
            _identitycontext.SaveChanges();
            return Ok();
        }



        [HttpGet("Companies")]
        public async Task<IActionResult> GetUserLinkedToCompany(Guid Guid)
        {


            ApplicationUser usr = await _general.General.GetCurrentUserAsync(HttpContext);
            if (usr != null)
            {
                var user = _general.Users.GetUserLinkedCompanies(usr, Guid).ToList();
                return Ok(user);

            } else
            {

                return Ok();

            }
        }


        [HttpGet("/Identity/UserSystems/{uid}")]
        public async Task<IActionResult> GetIdentityUserSystemsByUserList(string uid)
        {

            var userrole = await _identitycontext.MUSER_SYSTEM.Where(a => a.UserId == uid).ToListAsync();
            return Ok(userrole);

        }


        [HttpPost("/Identity/UserSystems/{puser}/{psystem}")]
        public async Task<IActionResult> PutIdentityUserSystemsList(string puser, string psystem)
        {

            try
            {

                var user = _identitycontext.Users.Where(a => a.Id == puser).First();

                var userSYSTEM = _identitycontext.MUSER_SYSTEM.Where(a => a.UserId == puser)
                    .Where(b => b.SystemId == psystem).ToList();

                if (user != null)
                {
                    var userAPPM = _context.MUSER.Where(a => a.EMAILADDRESS.ToLower() == user.NormalizedEmail.ToLower()).ToList();

                    if (userAPPM.Count == 0)
                    {

                        int seq = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQ_MUSER));
                        var musr = new MUSER()
                        {
                            USERID = seq,
                            UNAME = "SWA-" + seq.ToString(),
                            EMAILADDRESS = user.Email,
                            LOCALITYID = -99,
                            AUTHCODE = user.Id,
                            UPASSWORD = user.PasswordHash,
                            INSERT_DATE = DateTime.UtcNow
                        };

                        _logger.LogInformation("User Generation Info. Processing values {@musr}", musr);

                        _context.MUSER.Add(musr);
                        _context.SaveChanges();

                        MUSER_SYSTEM ur = new MUSER_SYSTEM() { UserId = puser, SystemId = psystem, MUSER_USERID = seq, INSERT_DATE = DateTime.Now, INSERTED_BY = user.Id, ISDOMAINUSER = "N" };
                        _identitycontext.MUSER_SYSTEM.Add(ur);
                        _identitycontext.SaveChanges();

                    }
                    else if ((userAPPM != null) && (userSYSTEM != null))
                    {
                        //MUSER_SYSTEM ur = new MUSER_SYSTEM() { UserId = puser, SystemId = psystem, MUSER_USERID = userAPPM.First().USERID };

                        //_identitycontext.MUSER_SYSTEM.Add(ur);
                        //_identitycontext.SaveChanges();

                        userAPPM.Single().AUTHCODE = user.Id;
                        _context.Attach(userAPPM.Single());
                        _context.Entry(userAPPM.Single()).Property("AUTHCODE").IsModified = true;
                        _context.SaveChanges();

                    }
                }
            } catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpDelete("/Identity/UserSystems/{puser}/{psystem}")]
        public async Task<IActionResult> DeleteIdentityUserSystemsList(string puser, string psystem)
        {
            try
            {
                var user = _identitycontext.MUSER_SYSTEM.Where(a => a.UserId == puser)
                    .Where(b => b.SystemId == psystem).ToList();

                _identitycontext.MUSER_SYSTEM.Where(a => a.UserId == puser && a.SystemId == psystem).ExecuteDeleteAsync();
                _identitycontext.SaveChanges();
            } catch (Exception ex) 
            { 
              return BadRequest(ex.Message);
            }
            return Ok();
        }



        [HttpGet("/Identity/Roles")]
        public async Task<IActionResult> GetIdentityRolesList()
        {

            var userlist = await _identitycontext.Roles.ToListAsync();
            return Ok(userlist);
        }

        [HttpGet("/Identity/Roles/{id}")]
        public async Task<IActionResult> GetIdentityRoleByIdList(string id)
        {

            var singlerole = await _identitycontext.Roles.Where(a => a.Id == id).FirstAsync();
            return Ok(singlerole);
        }

        [HttpPost("/Identity/Roles")]
        public async Task<IActionResult> PostIdentityRolesList(IdentityRole cstmRole)
        {

            //CstmRoles rol = new() { Name = cstmRole.Name, ROLE_CODE=cstmRole.ROLE_CODE };
            //rol = cstmRole;

            var uresult = await _roleManager.CreateAsync(new IdentityRole { Name = cstmRole.Name });

            //var uresult = await _roleManager.CreateAsync(rol);
            if (uresult.Succeeded)
            {

            }
            //await _identitycontext.Roles.Add(role);
            //await _identitycontext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("/Identity/Roles")]
        public async Task<IActionResult> PutIdentityRolesList(IdentityRole cstmRole)
        {

            var uresult = await _roleManager.UpdateAsync(cstmRole);

            if (uresult.Succeeded)
            {

            }
            return Ok();
        }


        [HttpGet("UserList")]
        public async Task<IActionResult> GetUserList()
        {

            var userlist = await _context.MUSER.Where(l => l.LOCALITYID != null)
                                               .Where(l => l.UGID != null).
                                                          Take(50).
                                                          ToListAsync();
            return Ok(userlist);
        }


        [AllowAnonymous]
        [HttpGet("MASTERDATA_User")]
        public async Task<IActionResult> GetUserMASTERDATA()
        {

            var userlist = await _general.Users.GetUser_MASTERDATA();
            return Ok(userlist);
        }

        [HttpGet("UserSearch")]
        public async Task<IActionResult> GetUserSearch(string SearchUser)
        {

            var userlist = await _context.MUSER.Where(o => o.UNAME.Contains(SearchUser) ||
                                                           o.NAME.Contains(SearchUser) ||
                                                           o.SURNAME.Contains(SearchUser))
                                                .Where(l => l.LOCALITYID != null)
                                                .Where(l => l.UGID != null).
                                                           Select(p => new {
                                                               p.USERID,
                                                               p.UNAME,
                                                               p.CELLNUMBER,
                                                               p.NAME,
                                                               p.SURNAME
                                                           }).
                                                           Take(20).
                                                           ToListAsync();
            return Ok(userlist);

        }


        [AllowAnonymous]
        [HttpGet("Removeaccount")]
        public IEnumerable<MUSER> RemoveUserAccount()
        {

            return _context.MUSER.ToList();

        }


        [AllowAnonymous]
        [HttpPost("/Identity/Register")]
        public async Task<IActionResult> Register(REGISTER_DATA model)
        {
            if (!string.IsNullOrEmpty(model.password))
            {

                CreatePasswordHash(model.password,
                    out byte[] passwordHash,
                    out byte[] passwordSalt);

                var user = new ApplicationUser()
                {
                    UserName = model.userName,
                    Email = model.email,
                    PhoneNumber = model.phoneNumber,
                    PasswordHash = model.password,
                    SYSTEM_USERID = model.NU_System,
                    REGISTRATIONTYPE = nameof(model.RegistrationType)
                    
                };
                var mDevice = new MUSER_DEVICE()
                {
                    UNAME = model.userName,
                    EMAIL = model.email,
                    DEVICETYPE = model.deviceInfo.DeviceType,
                    MODEL = model.deviceInfo.Model,
                    NAME = model.deviceInfo.Name,
                    MANUFACTURER = model.deviceInfo.Manufacturer,
                    OSVERSION = model.deviceInfo.OSVersion,
                    IDIOM = model.deviceInfo.Idiom,
                    PLATFORM = model.deviceInfo.Platform,
                    INSERT_DATE = DateTime.Now,
                    INSERTED_BY = model.userName,
                    MODIFIED_DATE = DateTime.Now,
                    MODIFIED_BY = model.userName

                };
                try
                {

                    //await using var txA = await _identitycontext.Database.BeginTransactionAsync();


                _logger.LogInformation("User Creation");
                var result = await _userManager.CreateAsync(user, user.PasswordHash!);
                if (result.Succeeded)
                {

                    _logger.LogInformation("User Device Info");
                    var result1 = await _identitycontext.MUSER_DEVICE.AddAsync(mDevice);
                    await _identitycontext.SaveChangesAsync();


                    _logger.LogInformation("User Registration");
                    int lReturn = await _general.Users.RegisterUserToSystem(model);

                    if (lReturn == 0)
                    {
                            EMAIL_TEMPLATE email_Template = new EMAIL_TEMPLATE();

                            email_Template.ToEmail = model.email;
                            email_Template.Subject = "IXM : Registration completed successfully.";
                            email_Template.userName = model.userName;
                            email_Template.passWord = model.password;
                            email_Template.Name = model.name;
                            email_Template.Surname = model.surname;

                            _logger.LogInformation("User Notify :: {@1}", email_Template);
                            await _commonrepo.Notification.SendEmailAsync(IXMMailType.Register, ref email_Template);

                    }



                }
                else
                {
                    return NotFound(result.ToString());
                }

            }
            catch (Exception ex)
            {
                    _logger.LogError("User Creation Error :: @1", ex.Message);
                    return BadRequest(ex.Message);
            }

            }
            else return BadRequest("Please enter a password!");

            //return BadRequest(result.Errors + "Error occurred");

            return Ok("Registration made successfully");
        }

        [AllowAnonymous]
        [HttpPost("/Account/Activate")]
        public async Task<IActionResult> AccountActivate(REGISTER_DATA model)
        {
            if (!string.IsNullOrEmpty(model.password))
            {

                CreatePasswordHash(model.password,
                    out byte[] passwordHash,
                    out byte[] passwordSalt);

                var user = new ApplicationUser()
                {
                    UserName = model.userName,
                    Email = model.email,
                    PhoneNumber = model.phoneNumber,
                    PasswordHash = model.password,
                    REGISTRATIONTYPE = nameof(model.RegistrationType)

                };
                var mDevice = new MUSER_DEVICE()
                {
                    UNAME = model.userName,
                    EMAIL = model.email,
                    DEVICETYPE = model.deviceInfo.DeviceType,
                    MODEL = model.deviceInfo.Model,
                    NAME = model.deviceInfo.Name,
                    MANUFACTURER = model.deviceInfo.Manufacturer,
                    OSVERSION = model.deviceInfo.OSVersion,
                    IDIOM = model.deviceInfo.Idiom,
                    PLATFORM = model.deviceInfo.Platform,
                    INSERT_DATE = DateTime.Now,
                    INSERTED_BY = model.userName,
                    MODIFIED_DATE = DateTime.Now,
                    MODIFIED_BY = model.userName

                };
                try
                {

                    //await using var txA = await _identitycontext.Database.BeginTransactionAsync();


                    _logger.LogInformation("User Creation");
                    var result = await _userManager.CreateAsync(user, user.PasswordHash!);
                    if (result.Succeeded)
                    {

                        _logger.LogInformation("User Device Info");
                        var result1 = await _identitycontext.MUSER_DEVICE.AddAsync(mDevice);
                        await _identitycontext.SaveChangesAsync();


                        _logger.LogInformation("User Registration");
                        int lReturn = await _general.Users.RegisterUserToSystem(model);

                        if (lReturn == 0)
                        {
                            EMAIL_TEMPLATE email_Template = new EMAIL_TEMPLATE();

                            email_Template.ToEmail = model.email;
                            email_Template.Subject = "IXM : Registration completed successfully.";
                            email_Template.userName = model.userName;
                            email_Template.passWord = model.password;
                            email_Template.Name = model.name;
                            email_Template.Surname = model.surname;

                            var lMailType = IXMMailType.Register;
                            if (model.RegistrationType == IxmRegisterUserType.BusinessUser)
                            {
                                email_Template.Subject = "IXM : Account Registered & Activated successfully.";
                                lMailType = IXMMailType.AccountActivated;
                            }
                            _logger.LogInformation("User Notify :: {@1}", email_Template);
                            await _commonrepo.Notification.SendEmailAsync(lMailType, ref email_Template);

                        }



                    }
                    else
                    {
                        return NotFound(result.ToString());
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError("User Creation Error :: @1", ex.Message);
                    return BadRequest(ex.Message);
                }

            }
            else return BadRequest("Please enter a password!");

            //return BadRequest(result.Errors + "Error occurred");

            return Ok("Registration made successfully");
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {

            var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

        }

        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        [HttpGet]
        public IActionResult ResetPassword(string code, string email)
        {
            var model = new USER_PASSWORDRESET { token = code, email = email };
            return Ok(model);
        }



        [AllowAnonymous]
        [HttpPost("/Identity/ResetPassword")]
        public async Task<IActionResult> ResetPassword(USER_PASSWORDRESET resetinfo)
        {

            var user = await _userManager.FindByEmailAsync(resetinfo.email);
            if (user == null)
            {
                return BadRequest("Could not find User, please try again.");
            }

            var result = await _userManager.ResetPasswordAsync(user, resetinfo.token, resetinfo.password);

            if (result.Succeeded)
            {

                //var user1 = await _identitycontext.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == resetinfo.token);
                //var user1 = await _identitycontext.Users.FirstOrDefaultAsync();
                //user1.PasswordResetToken = null;
                //user1.PasswordResetExpiryDate = null;
                await _identitycontext.SaveChangesAsync();


                EMAIL_TEMPLATE email_Template = new EMAIL_TEMPLATE();

                email_Template.ToEmail = resetinfo.email;
                email_Template.Subject = "IXM : Password changed successfully.";
                email_Template.userName = user.UserName;
                email_Template.passWord = resetinfo.password;
                //email_Template.Name = user1.name;
                //email_Template.Surname = user1.surname;
                await _commonrepo.Notification.SendEmailAsync(IXMMailType.PasswordChanged, ref email_Template);

                return Ok("You have reset your password. Welcome back.");
                //return RedirectToAction("ResetPasswordConfirmation", "Account");

            }
            return BadRequest("Password change Failed." + result.Errors.ToString());



        }

        [AllowAnonymous]
        [HttpPost("/Identity/ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string pemail)
        {
            //var user = await _identitycontext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            try
            {

                var user = await _userManager.FindByEmailAsync(pemail);
                if (user == null)
                {
                    return BadRequest("Invalid Token");
                }
                else
                {

                    USER_PASSWORDRESET preset = new USER_PASSWORDRESET();

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

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

                    await _commonrepo.Notification.SendEmailAsync(IXMMailType.ForgotPassword, ref email_Template);


                    return Ok(preset);

                }
            }
            catch (Exception ex)
            {                
                return BadRequest(ex.Message);
            }           


        }

        [AllowAnonymous]
        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser(string UserName, string email, string password)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(
                userName: UserName,
                  password: password!,
                  isPersistent: false,
                  lockoutOnFailure: false
                  );
            if (signInResult.Succeeded)
            {
                return Ok("You are successfully logged in");
            }
            return BadRequest("Error occurred");
        }


        [AllowAnonymous]
        [HttpPost("/User/Environment")]
        public async Task<IActionResult> GetUserSettings(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user != null)
                {
                    var usersystem = _identitycontext.MUSER_SYSTEM.Where(a => a.UserId == user.Id).First();
                    if (usersystem != null)
                    {
                        var domainlicense = _appctrlcontext.APPDOMAINLICENSE.Where(a => a.SYSTEMID.ToString() == usersystem.SystemId)
                                                                            .Select(b => new { b.SYSTEMID, b.APPSERVERNAME, b.APPSERVERPORTNO, b.APPSERVERDEVPORTNO, b.DOMAINREQUEST, IsDomainAccount = usersystem.ISDOMAINUSER, id = user.Id, SYSTEMUSERID = usersystem.MUSER_USERID })
                                                                            .FirstOrDefault();
                         // domainlicense.

                        if (domainlicense != null)
                        {
                            return Ok(domainlicense);
                        }
                        return BadRequest("LicenseNotAvailable");

                    }
                    return BadRequest("SystemAllocationNotAvailable");

                }
                return BadRequest("Error occurred");
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UserAuth")]
        public IActionResult GetLogin(String pUsername, String pPassword, MUSER_DEVICE pDevice)
        {
            var muser = new MUSER();

            try
            {
                var singleuser = _identitycontext.Users.Where(a => a.Email.ToLower() == pUsername.ToLower()).ToList();
                if (singleuser.Count == 0)
                {
                    return BadRequest(new Tuple<int, string>(-1, "User Not Found"));
                    //return BadRequest(new Tuple<int, string>(-1, "ID User Not Found"));
                }

                //.Where(b => b.UPASSWORD == pPassword)
                var prUser = _context.MUSER
                            .Where(b => b.EMAILADDRESS.ToLower().Equals(pUsername.ToLower()))
                            .Where(b => b.AUTHCODE.Equals(singleuser.First().Id))
                            .ToList();


                if (prUser.Count == 0)
                {
                   // return BadRequest(new Tuple<int, string>(-2, "System User Not Found"));
                    return BadRequest(new Tuple<int, string>(-2, "User Not Found"));
                }

                var prObjectId = _context.MOBJECT_DOC.FromSqlRaw<MOBJECT_DOC>($"SELECT (SELECT HVALUE FROM APP_HARDCODES WHERE HARDC_TYPE='IMAGESTORE') IMAGEURI, m.* " +
                " FROM MOBJECT_DOC m WHERE m.SOURCEOBJ = 'MUSER' AND m.SOURCEFLD = 'USERID' " +
                " AND m.SOURCEID = @p0 AND m.DOCTYPE = 'USRPHOTO' AND m.LT=1", prUser.First().USERID);

                var prURI = _context.APP_HARDCODES.FromSqlRaw<APP_HARDCODES>($"SELECT HARDC_TYPE, HARDCODEID, HVALUE FROM APP_HARDCODES WHERE HARDC_TYPE='IMAGESTORE'");

                if (prObjectId.Count() > 0)
                {
                    prUser.ForEach(b =>
                    {
                        b.OBJECTID = prObjectId.First().OBJECTID.ToString();
                        b.DOCUMENTNAME = prObjectId.First().DOCUMENTNAME;
                        b.SFOLDERNAME = prObjectId.First().SFOLDERNAME;
                        b.IMAGEURI = prURI.First().HVALUE + "//" + prObjectId.First().DOCUMENTNAME;
                    });
                }


                var userRole = _identitycontext.MUSER_ROLE.Where(a => a.UserId == singleuser.First().Id.ToString()).First();

                if (userRole == null)
                {
                    return BadRequest(new Tuple<int, string>(-3, "ID User Role Not Found"));
                }

                var userRoleName = _identitycontext.Roles.Where(a => a.Id == userRole.RoleId).ToList();


                if (userRoleName.Count() == 0)
                {
                    return BadRequest(new Tuple<int, string>(-4, "ID RoleName not found.Data sequencing"));
                }

                //var prMUG = _context.MUSER_GROUP.FromSqlRaw<MUSER_GROUP>($"SELECT MUSER_GROUP.* FROM MUSER_GROUP, CUSER_GROUP WHERE CUSER_GROUP.UGID = MUSER_GROUP.UGID AND CUSER_GROUP.USERID = @p0" +
                //    " AND MUSER_GROUP.UGCODE LIKE 'MB_G_%'", prUser.First().USERID);

                var prMUG = _context.MUSER_GROUP.FromSqlRaw<MUSER_GROUP>($"SELECT MUSER_GROUP.* FROM MUSER_GROUP WHERE Upper(LINK_DESCRIPTION) = @p0", userRoleName.First().NormalizedName );

                if (prMUG == null)
                {
                    return BadRequest(new Tuple<int, string>(-4, "System User Role Not Found"));
                }

                if (prMUG.Count() > 0)
                {
                    prUser.First().UGCODE = prMUG.First().UGCODE;
                }

                muser = prUser.First();
                muser.UPASSWORD = "";

                var mDevice = new MUSER_DEVICE()
                {
                        UNAME = muser.UNAME,
                        EMAIL = muser.EMAILADDRESS,
                        DEVICETYPE = pDevice.DEVICETYPE,
                        MODEL = pDevice.MODEL,
                        NAME = pDevice.NAME,
                        MANUFACTURER = pDevice.MANUFACTURER,
                        OSVERSION = pDevice.OSVERSION,
                        IDIOM = pDevice.IDIOM,
                        PLATFORM = pDevice.PLATFORM,
                        INSERT_DATE = DateTime.Now,
                        INSERTED_BY = pDevice.UNAME,
                        MODIFIED_DATE = DateTime.Now,
                        MODIFIED_BY = pDevice.UNAME
                };
                _dataservice.UpdateDeviceInfo(_logger, mDevice);

                return Ok(muser);
            
             } catch (Exception ex) 
            {
                
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);

            }

        }

        [HttpGet("/System/MenuRights")]
        public async Task<IActionResult> GetMenuRights(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user != null)
                {
                    var usersystem = _identitycontext.MUSER_SYSTEM.Where(a => a.UserId == user.Id).First();
                    if (usersystem != null)
                    {
                        var prUser = _context.MUSER
                                    .Where(b => b.AUTHCODE.Equals(user.Id))
                                    .ToList();


                        if (prUser.Count == 0)
                        {
                            return BadRequest(new Tuple<int, string>(-2, "System User Not Found"));
                        }

                        var result = _dataservice.GetSystemMenuRights(user.Id, prUser.First().USERID);
                        return Ok(result);

                    }
                    return BadRequest("SystemAllocationNotAvailable");

                }
                return BadRequest("Error occurred");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }

}