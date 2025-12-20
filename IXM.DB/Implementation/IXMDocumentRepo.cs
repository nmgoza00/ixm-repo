using IXM.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using IXM.DB.Services;
using IXM.GeneralSQL;
using IXM.DB.Implementation.B2B;
using IXM.DB.Interface.B2B;

namespace IXM.DB
{
    public class IXMDocumentRepo : IIXMDocumentRepo
    {
        private HttpClient _httpClient;
        private List<MSYSTEMS> mSYSTEMS = new List<MSYSTEMS>();
        public IXMDBContext _context;
        private readonly IXMDBContextFactory _dbfactory;




        public IXMDocumentRepo( IXMDBContext context,
                                IXMDBIdentity idtcontext,
                                IXMDBContextFactory dbfactory,
                                IConfiguration configuration,
                                IDataValidator dataValidator,
                                UserManager<ApplicationUser> usermanager,
                                IIXMDBRepo dbrepo,
                                IGeneralSQL generalSQL,
                                ILogger<Transaction> loggerT,
                                ILogger<Document> loggerD)
        {

            Document = new Document(context, idtcontext, dbfactory, usermanager, dbrepo, generalSQL, configuration, loggerD);
            FileTransfer = new SftpFileTransferService(configuration);
            _context = context;

        }

        public IDocument Document { get; private set; }
        public IFileTransferService FileTransfer { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }



    }
}
