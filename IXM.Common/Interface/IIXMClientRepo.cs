using IXM.Common;
using IXM.Common.Client;
using IXM.Common.Interfaces;
using IXM.Common.Notification;

namespace IXM.Common
{


    public interface IIXMClientRepo
    {
        IDataServicesClient DataServicesClient { get; }
        IIXMNotification Notification { get; }
    }
}
