using IXM.Common;
using IXM.Common.Constant;
using IXM.Constants;
using IXM.GeneralSQL;
using IXM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Drawing.Text;
using static IXM.Common.Notification.IXMNotification;

namespace IXM.DB
{
    public class Users : IUsers
    {

        private readonly IXMDBContext _dbcontext;
        private readonly IXMDBIdentity _identitycontext;
        private readonly IIXMCommonRepo _commonRepo;
        private readonly ILogger<Users> _logger;
        private readonly IMemoryCache _memCache;
        private readonly IGeneralSQL _generalSQL;
        private readonly IMasterData _masterData;
        private readonly IConfiguration _config;
        private readonly IXMWriteDBContextFactory _dbfactory;
        private IGeneral _general;

        public GenFunctions genFunctions = new GenFunctions();

        public Users(
                            IXMDBContext dbcontext,
                            IXMDBIdentity identitycontext,
                            IIXMCommonRepo commonRepo,
                            IMemoryCache memoryCache,
                            IXMWriteDBContextFactory dbfactory,
                            IGeneralSQL generalSQL,
                            ILogger<Users> logger,
                            IConfiguration config)
        {
            _dbcontext = dbcontext;
            _general = new General(dbcontext, null, null, null);
            _identitycontext = identitycontext;
            _commonRepo = commonRepo;
            _logger = logger;
            _generalSQL = generalSQL;
            _config = config;
            _dbfactory = dbfactory;
            _masterData = new MasterData(null,null, dbcontext, memoryCache, null);
        }

        
        public List<USER_COMPANY> GetUserLinkedCompanies(ApplicationUser au, Guid _Guid)
        {
            try
            {


                var _user = _dbcontext.USER_COMPANY.FromSqlRaw(_generalSQL.GetUserLinkedCompanies(_Guid)).ReadUserCompanies().ToList();

                if (_user != null)
                {
                    return _user;

                } return null;


            }
            catch (Exception)
            {

                return null;
            }

        }


        private string GetUserCode(IxmRegisterUserType registerUserType)
        {
            if (registerUserType == IxmRegisterUserType.NormalUser)
                return "IXU";
            else if (registerUserType == IxmRegisterUserType.BusinessUser)
                return "IXB";
            else
                return "IXG";

        }


        public async Task<USER_MASTERDATA> GetUser_MASTERDATA()
        {


            var musr = new USER_MASTERDATA();

            musr.MLOCALITY = await _masterData.GetLocality();
            musr.MUSER_GROUP = await _masterData.GetUserGroup();
            musr.MINTPOSITION = await _masterData.GetIntPosition();
            //musr.MEMPLOYEE = await _masterData.GetE();
            //musr.VMSTATUS = await _masterData.GetStatusText();

            return musr ?? new USER_MASTERDATA();

        }


        private string GetUserGroupCode(IxmRegisterUserType registerUserType)
        {

            if (registerUserType == IxmRegisterUserType.NormalUser)
            
                return nameof(IxmUserRights.MB_G_MEM);
            
            else if (registerUserType == IxmRegisterUserType.BusinessUser)
            
                return nameof(IxmUserRights.MB_B_B2B);
            
            else
            
                return nameof(IxmUserRights.MB_G_GEN);

        }
        public async Task <int> RegisterUserToSystem(REGISTER_DATA registerData)
        {
            try
            {

                var usersel = await _identitycontext.Users.Where(a => a.Email == registerData.email).FirstAsync();

                var singlerole = await _identitycontext.Roles.Where(a => a.Name == "NOROLE").FirstAsync();


                try
                {

                var singlesys = await _identitycontext.MSYSTEM.Where(a => a.SYSTEMID == registerData.NU_System.ToString()).FirstOrDefaultAsync();
         
                var _ctx = _dbfactory.Create("Firebird" + singlesys.SYSTEMNAME);

                _logger.LogInformation("DB Factory Connected - Processing values for System {@1} - User {@musr}", singlesys.SYSTEMNAME, _ctx.Database);

                //bool exists = _ctx.MUSER.Any(x => x.EMAILADDRESS.ToLower() == usersel.NormalizedEmail.ToLower());
                var userAPPM = _ctx.MUSER.Where(a => a.EMAILADDRESS.ToLower() == usersel.NormalizedEmail.ToLower()).ToList();

                if ((userAPPM.Count > 0))
                {
                    bool val = userAPPM.First().UNAME.Contains(GetUserCode(registerData.RegistrationType));
                           
                    // Handle duplicate case
                    userAPPM.Single().AUTHCODE = usersel.Id;
                    _ctx.Attach(userAPPM.Single());
                    _ctx.Entry(userAPPM.Single()).Property("AUTHCODE").IsModified = true;
                    _ctx.SaveChanges();
                    return -6;
                    //throw new InvalidOperationException($"User {usersel.Id} is already linked to system {singlesys.SYSTEMID}.");
                }

                int seq = _general.GetSEQUENCE(nameof(IxmDBSequence.SEQ_MUSER), _ctx);
                int mstatusid = _general.GetConfigMStatus(nameof(IxmAppCaptionObject.MUSER), 1, _ctx);
                int UGID = _general.GetUserGroupId(GetUserGroupCode(registerData.RegistrationType), _ctx);
                var musr = new MUSER()
                {
                    USERID = seq,
                    UNAME = GetUserCode(registerData.RegistrationType) + "-" + seq.ToString(),
                    NAME = registerData.name,
                    SURNAME = registerData.surname,
                    EMAILADDRESS = usersel.Email,
                    LOCALITYID = -99,
                    AUTHCODE = usersel.Id,
                    MCNAME = registerData.deviceInfo.Name + " " + registerData.deviceInfo.Platform,
                    UPASSWORD = usersel.PasswordHash,
                    INSERT_DATE = DateTime.UtcNow,
                    MSTATUSID = mstatusid,
                    APPSERVICE = "IXM",
                    ISACTIVE = "P",
                };

                _logger.LogInformation("User Generation Info. Processing values for System {@1} - User {@musr}", singlesys.SYSTEMNAME,musr);

                _ctx.MUSER.Add(musr);

                _ctx.SaveChanges();

                    _logger.LogInformation("User Generation Info. - User added to Database - {@1}", musr);
                var cusr = new CUSER_GROUP_WRITE()
                {
                    USERID = seq,
                    UGID = UGID,
                };

                _ctx.CUSER_GROUP.Add(cusr);


                _logger.LogInformation("User Generation Info. - User Group added to Memory - {@1}", cusr);

                if (registerData.RegistrationType == IxmRegisterUserType.BusinessUser)
                {

                        var ccur = new CCUR()
                        {
                            COMPANYID = Convert.ToInt32(registerData.BU_Company),
                            SRCOBJ = nameof(IxmAppCaptionObject.MUSER),
                            USERID = seq,
                            SRCID = seq,
                            MIDX = 1,
                            INSDAT = DateTime.UtcNow,
                            INSBY = musr.UNAME,
                            MODAT = DateTime.UtcNow,
                            MODBY = musr.UNAME,
                            ISACTIVE = "Y"

                        };

                        _ctx.CCUR.Add(ccur);


                        _logger.LogInformation("User Generation Info. - User Company added to Memory - {@1}", ccur);

                }

                _ctx.SaveChanges();

                _logger.LogInformation("User Generation Info. User Group Saved to Database");

                    ///  Very crititcal step ///
                MUSER_SYSTEM ms = new MUSER_SYSTEM() { UserId = usersel.Id, SystemId = singlesys.SYSTEMID, INSERT_DATE = DateTime.Now, INSERTED_BY = registerData.userName, ISDOMAINUSER = "Y" , MUSER_USERID = seq };

                _identitycontext.MUSER_SYSTEM.Add(ms);
                await _identitycontext.SaveChangesAsync();



                    MUSER_ROLE url = new MUSER_ROLE() { UserId = usersel.Id, RoleId = singlerole.Id };
                    _identitycontext.MUSER_ROLE.Add(url);
                    _identitycontext.SaveChanges();

                    _logger.LogInformation("User Generation Info. Saved to Database");
                    return 0;

                }
                catch (UniqueConstraintViolationException ex)
                {
                    _logger.LogInformation("User Generation Info. Error on Db Save :: Unique Issue {@0}", ex.Message);
                    return -2;
                }
                catch (Exception ex)
                {

                    _logger.LogInformation("User Generation Info. Error on Saved to Database {@0}", ex.Message);
                    return -1;
                }


            }
            catch (Exception)
            {

                return -10;
            }

            return 0;

        }


    }
}
