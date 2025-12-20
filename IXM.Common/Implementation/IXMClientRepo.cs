using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using IXM.Common.Client;
using IXM.Common.Data;
using Microsoft.AspNetCore.Hosting;
using IXM.Common.Notification;

namespace IXM.Common
{
    public class IXMClientRepo : IIXMClientRepo
    {       

        public IXMClientRepo(
                            IWebHostEnvironment environment,
                            IConfiguration configuration,
                            CustomData customData,
                            ILogger<IXMNotification> loggerN,
                            ILogger<DataServicesClient> loggerD)
        {

            DataServicesClient = new DataServicesClient(environment, configuration, customData);
            Notification = new IXMNotification(environment, configuration, loggerN);

        }

        public IDataServicesClient DataServicesClient { get; }
        public IIXMNotification Notification { get; }



    }
}
