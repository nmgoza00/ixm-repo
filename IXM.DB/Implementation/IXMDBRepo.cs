using IXM.Common;
using IXM.DB.B2B;
using IXM.DB.Log;
using IXM.DB.Server;
using IXM.DB.Services;
using IXM.GeneralSQL;
using IXM.Ingest;
using IXM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace IXM.DB
{
    public class IXMDBRepo : IIXMDBRepo
    {
        public IXMDBContext _context;

        public IXMDBRepo(IXMDBContext context,
                         IXMSystemDBContext systemcontext,
                         IXMSystemWrDBContext systemwrcontext,
                         IXMJobDBContext jobcontext,
                         IXMAppLogContext logcontext,
                         IXMWriteDBContext writecontext,
                         IXMDBIdentity idtcontext,
                         IXMDBContextFactory dbfactory,
                         IXMWriteDBContextFactory wrdbfactory,
                         IMemoryCache jobmemcache,
                         IIXMCommonRepo commonrepo,
                         IMemoryCache memcache,
                         IConfiguration configuration,
                         IDataValidator dataValidator,
                         IDataServicesServer dataServices,
                         UserManager<ApplicationUser> usermanager,
                         IGeneralSQL generalSQL, 
                         IDataImport dataimport,
                         ILogger<General> loggerG,
                         ILogger<Transaction> loggerT,
                         ILogger<CustomUpdates> loggerCU,
                         ILogger<FileIngest> loggerI,
                         ILogger<AppLog> loggerAL,
                         ILogger<Users> loggerU,
                         ILogger<Events> loggerE,
                         ILogger<Member> loggerM,
                         ILogger<Organisor> loggerO,
                         ILogger<Legal> loggerL,
                         ILogger<MasterData> loggerMa,
                         ILogger<DBTasks> loggerD)
        {

            General = new General(context, idtcontext, usermanager, loggerG);
            AppLog = new AppLog(logcontext, idtcontext, loggerAL);
            CustomUpdates = new CustomUpdates(context, idtcontext, usermanager, generalSQL, configuration, loggerCU);
            DBTasks = new DBTasks(context, idtcontext, usermanager, generalSQL, loggerD);
            Events = new Events(context, idtcontext, systemcontext, systemwrcontext, jobmemcache, usermanager, loggerE);
            FileIngest = new FileIngest(context, idtcontext, loggerI,generalSQL, jobcontext, commonrepo, dataimport);
            GetB2BDataSets = new GetB2BDataSets(context, idtcontext, loggerI);
            GenValues = new GenValues();
            Member = new Member(idtcontext,context, memcache,configuration, loggerM);
            Company = new Company(idtcontext, context, dbfactory, configuration, generalSQL, loggerM);
            Organisor = new Organisor(idtcontext, context, configuration, generalSQL, loggerO);
            Processor = new Processor(idtcontext, context, configuration, generalSQL, loggerO);
            Users = new Users(context, idtcontext, commonrepo, jobmemcache, wrdbfactory, generalSQL, loggerU, configuration);
            Legal = new Legal(idtcontext, context, writecontext, configuration, loggerL);
            Reporting = new Reporting(context, idtcontext, usermanager, generalSQL, loggerD);
            MasterData = new MasterData(idtcontext,dbfactory, context, memcache, loggerMa);
            DataServices = new DataServicesServer(configuration, memcache, jobcontext, context);

            _context = context;
        }

        public IGeneral General { get; private set; }
        public IAppLog AppLog { get; private set; }
        public ICustomUpdates CustomUpdates { get; private set; }
        public IDBTasks DBTasks { get; private set; }
        public IEvents Events { get; private set; }
        public IFileIngest FileIngest { get; private set; }
        public IGetB2BDataSets GetB2BDataSets { get; private set; }
        public IGenValues GenValues { get; private set; }
        public IMember Member { get; private set; }
        public ICompany Company { get; private set; }
        public IOrganisor Organisor { get; private set; }
        public IProcessor Processor { get; private set; }
        public IUsers Users { get; private set; }
        public ILegal Legal { get; private set; }
        public IReporting Reporting { get; private set; }
        public IMasterData MasterData { get; private set; }
        public IDataServicesServer DataServices { get; private set; }


        public void Dispose()
        {
            _context.Dispose();
        }



    }
}
