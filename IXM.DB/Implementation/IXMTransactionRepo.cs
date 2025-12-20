using IXM.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using IXM.DB.Services;
using IXM.GeneralSQL;
using IXM.Common;
using IXM.DB.Finance;
using Serilog.Core;

namespace IXM.DB
{
    public class IXMTransactionRepo : IIXMTransactionRepo
    {
        private HttpClient _httpClient;
        private List<MSYSTEMS> mSYSTEMS = new List<MSYSTEMS>();
        public IXMDBContext _context;


        

        public IXMTransactionRepo(
                            IXMDBContext context,
                            IXMWriteDBContext writecontext,
                            IXMAppLogContext logcontext,
                            IXMDBIdentity idtcontext,
                            IConfiguration configuration,
                            IDataValidator dataValidator,
                            UserManager<ApplicationUser> usermanager,
                            IIXMDBRepo dbrepo,
                            IIXMCommonRepo commonrepo,
                            IIXMDocumentRepo docrepo,
                            IGeneralSQL generalSQL,
                            IDataImport dataimport,
                            IFinance finance,
                            ILogger<Transaction> loggerT)
        {

            Remittance = new Remittance(context, writecontext, idtcontext, configuration, dataValidator, dbrepo, generalSQL, loggerT);
            Finance = new Finance.Finance(context, writecontext, idtcontext, configuration, dataValidator, dbrepo, generalSQL, loggerT);
            Transaction = new Transaction(context, writecontext, idtcontext, configuration, dataValidator, finance, dbrepo, commonrepo, docrepo, generalSQL, loggerT);




            _context = context;

        }

        public IRemittance Remittance { get; private set; }
        public IFinance Finance { get; private set; }
        public ITransaction Transaction { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }



    }
}
