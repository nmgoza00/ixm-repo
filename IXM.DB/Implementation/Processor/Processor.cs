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
    public class Processor : IProcessor
    {

        private readonly IXMDBContext _dbcontext;
        private readonly IXMDBIdentity _identitycontext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<Organisor> _logger;
        private readonly IGeneralSQL _generalSQL;

        public GenFunctions genFunctions = new GenFunctions();

        public Processor( 
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

        public List<REP_PROCESSOR_LOADEDSCHEDULES> GetProcessorSchedules(string _Guid, string CompanyId, string Period)
        {
            try
            {

                var result = _dbcontext.REP_PROCESSOR_LOADEDSCHEDULES.FromSqlRaw<REP_PROCESSOR_LOADEDSCHEDULES>(_generalSQL.GetProcessorLoadedSchedules(_Guid, CompanyId, Period)).ToList();

                return result;


            }
            catch (Exception)
            {

                return null;
            }

        }

        public List<TRMBL> GetPendingRemittances(string _Guid, string CompanyId, string Period)
        {

            try
            {

                var prData = _dbcontext.TRMBL.ReadModel().Where(a => a.STATUSID == 79).ToList();

                return prData;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }

}
