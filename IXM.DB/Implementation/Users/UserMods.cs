using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using IXM.Models;
using IXM.GeneralSQL;
using IXM.Common.Constant;

namespace IXM.DB
{
    public class UserMods : IUserMods
    {

        private readonly IXMDBContext _dbcontext;
        private readonly ILogger<UserMods> _logger;
        private readonly IGeneralSQL _generalSQL;

        public GenFunctions genFunctions = new GenFunctions();

        public UserMods( 
                            IXMDBContext dbcontext,
                            IGeneralSQL generalSQL,
                            ILogger<UserMods> logger)
        {
            _dbcontext = dbcontext;
            _logger = logger;
            _generalSQL = generalSQL;
        }

        
        public List<USER_COMPANY> RegisterUserToSystem(ApplicationUser au, Guid _Guid)
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


    }
}
