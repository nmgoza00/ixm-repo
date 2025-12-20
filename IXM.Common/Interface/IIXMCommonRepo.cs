using IXM.Common;
using IXM.Common.Client;
using IXM.Common.Notification;
using IXM.Common.Interfaces;

namespace IXM.Common
{


    public interface IIXMCommonRepo
    {
        IFileService FileService { get; }
        IDataImport DataImport { get; }
        IGenValues GenValues { get; }
        IIXMNotification Notification { get; }
    }
}
