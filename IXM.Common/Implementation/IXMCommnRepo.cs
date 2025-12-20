using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using IXM.GeneralSQL;
using IXM.Common.Data;
using Microsoft.AspNetCore.Hosting;
using IXM.Common.Notification;

namespace IXM.Common
{
    public class IXMCommonRepo : IIXMCommonRepo
    {       

        public IXMCommonRepo(
                            IWebHostEnvironment environment,
                            IConfiguration configuration,
                            CustomData customData,
                            IGeneralSQL generalSQL,
                            ILogger<DataImport> loggerD,
                            ILogger<IXMNotification> loggerN)
        {

            FileService = new FileService(environment, configuration, customData);
            DataImport = new DataImport(loggerD);
            GenValues = new GenValues();
            Notification = new IXMNotification(environment, configuration, loggerN);

        }

        public IFileService FileService { get; }
        public IDataImport DataImport { get; }
        public IGenValues GenValues { get; }
        public IIXMNotification Notification { get; }



    }
}
