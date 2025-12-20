using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using IXM.DB.Services;
using IXM.Models;
using IXM.GeneralSQL;
using IXM.Common;
using System.IO;
using System.Data.Entity;
using DocumentFormat.OpenXml.InkML;
using IXM.Models.Core;
using IXM.Common.Constant;

namespace IXM.DB
{
    public class Organisor : IOrganisor
    {

        private readonly IXMDBContext _dbcontext;
        private readonly IXMDBIdentity _identitycontext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<Organisor> _logger;
        private readonly IGeneralSQL _generalSQL;

        public GenFunctions genFunctions = new GenFunctions();

        public Organisor( 
                            IXMDBIdentity idtcontext,
                            IXMDBContext dbcontext,
                            IConfiguration configuration,
                            IGeneralSQL generalSQL,
                            ILogger<Organisor> logger)
        {
            _dbcontext = dbcontext;
            _identitycontext = idtcontext;
            _configuration = configuration;
            _logger = logger;
            _generalSQL = generalSQL;
        }

        public List<ORGANISER_COMPANY> GetOrganisorCompanies(string _Guid)
        {
            try
            {

              var prCompany = _dbcontext.ORGANISER_COMPANY.FromSqlRaw<ORGANISER_COMPANY>(_generalSQL.GetOrganiser_Companies(_Guid)).ToList();

              return prCompany;


            }
            catch (Exception)
            {

                return null;
            }

        }

        public List<ORGANISER_COMPANY> GetOrganisorBranches(string _Guid)
        {
            try
            {

                var prCompany = _dbcontext.ORGANISER_COMPANY.FromSqlRaw<ORGANISER_COMPANY>(_generalSQL.GetOrganiser_Branches(_Guid)).ToList();

                return prCompany;


            }
            catch (Exception)
            {

                return null;
            }

        }

        public List<MCOMPANY> GetOrganisorUnmappedCompanies()
        {
            try
            {


                var prCompany = _dbcontext.MCOMPANY.FromSqlRaw<MCOMPANY>(_generalSQL.GetOrganiser_UnmappedCompanies()).ToList();
                return prCompany;


            }
            catch (Exception)
            {

                return null;
            }

        }
        public List<MCOMPANY> GetOrganisorUnmappedBranches()
        {
            try
            {


                var prCompany = _dbcontext.MCOMPANY.FromSqlRaw<MCOMPANY>(_generalSQL.GetOrganiser_UnmappedBranches()).ToList();
                return prCompany;


            }
            catch (Exception)
            {

                return null;
            }

        }


    }
}
