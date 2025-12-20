using IXM.Ingest;
using IXM.DB.B2B;
using IXM.DB.Log;
using IXM.DB.Server;


namespace IXM.DB
{


    public interface IIXMDBRepo : IDisposable
    {

        IGeneral General { get; }
        ICustomUpdates CustomUpdates { get; }
        IDBTasks DBTasks { get; }
        IEvents Events { get; }
        IFileIngest FileIngest { get; }
        IGetB2BDataSets GetB2BDataSets {get; }
        IAppLog AppLog { get; }
        IMember Member { get; }
        ICompany Company { get; }
        IOrganisor Organisor { get; }
        IProcessor Processor { get; }
        IUsers Users { get; }
        ILegal Legal { get; }
        IReporting Reporting { get; }
        IMasterData MasterData { get; }
        IDataServicesServer DataServices { get; }


    }
}
