using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using IXM.DB.Services;
using IXM.Models;
using IXM.GeneralSQL;
using IXM.Common;
using IXM.Models.Core;
using IXM.Common.Constant;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Spreadsheet;
using DevExpress.Utils.About;
using static DevExpress.Utils.Filtering.ExcelFilterOptions;
using System.ComponentModel.Design;
using System.Data.Entity;



namespace IXM.DB
{
    public class Remittance : IRemittance
    {

        private readonly IXMDBContext _context;
        private readonly IXMWriteDBContext _writecontext;
        private readonly IXMDBIdentity _identitycontext;
        private readonly IConfiguration _configuration;
        private readonly IDataValidator _datavalidator;
        private readonly IIXMDBRepo _general;
        private readonly IGeneralSQL _generalSQL;
        private readonly ILogger<Transaction> _logger;

        public GenFunctions genFunctions = new GenFunctions();

        public Remittance( IXMDBContext context, 
                            IXMWriteDBContext writecontext,
                            IXMDBIdentity idtcontext, 
                            IConfiguration configuration, 
                            IDataValidator dataValidator,
                            IIXMDBRepo general, 
                            IGeneralSQL generalSQL,
                            ILogger<Transaction> logger)
        {
            _context = context;
            _general = general;
            _generalSQL = generalSQL;
            _datavalidator = dataValidator;
            _identitycontext = idtcontext;
            _configuration = configuration;
            _logger = logger;
            _writecontext = writecontext;
            //_queryRepository = queryRepository;
        }

        public async Task <List<TRMBLE>> GetRemittanceError(int CompanyId)
        {
            try
            {
                List<TRMBLE> result = _context.TRMBLE.FromSqlRaw<TRMBLE>(_generalSQL.GetRemittanceError(CompanyId.ToString())).ToList();
                return result;
            }
            catch (Exception)
            {

                return null;
            }

        }


    }
}
