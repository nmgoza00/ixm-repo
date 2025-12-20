using IXM.Constants;
using IXM.Models;


namespace IXM.DB.Log
{


    public interface IAppLog
    {

        Task LogTotableE(IxmAppLogType LogType, IxmAppSourceObjects SourceObj, string SystemId, string SourceId,string Message, string User);
        Task LogTotableI(IxmAppLogType LogType, IxmAppSourceObjects SourceObj, string SystemId, string SourceId, string Message, string User);
        Task LogTotableF(IxmAppLogType LogType, IxmAppSourceObjects SourceObj, string SystemId, string SourceId, string Message, string User);
        Task LogTotableR(IxmAppLogType LogType, IxmAppSourceObjects SourceObj, string SystemId, string SourceId, string Message, string User);

        Task<List<APP_LOGTABLEE>>GetFromLogTableE(IxmAppSourceObjects SourceObj, string SystemId, string SourceId);
        Task<List<APP_LOGTABLEI>> GetFromLogTableI(IxmAppSourceObjects SourceObj, string SystemId, string SourceId);
        Task<List<APP_LOGTABLEF>> GetFromLogTableF(IxmAppSourceObjects SourceObj, string SystemId, string SourceId);
        Task<List<APP_LOGTABLER>> GetFromLogTableR(IxmAppSourceObjects SourceObj, string SystemId, string SourceId);


    }
}
