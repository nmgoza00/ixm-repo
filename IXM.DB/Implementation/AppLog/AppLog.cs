using IXM.Constants;
using IXM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;


namespace IXM.DB.Log
{

    public class AppLog : IAppLog
    {

        private IXMAppLogContext _context;
        private IXMDBIdentity _identitycontext;
        private readonly ILogger<AppLog> _logger;

        public AppLog(IXMAppLogContext context,
                              IXMDBIdentity identitycontext,
                              ILogger<AppLog> logger)
        {
            _context = context;
            _identitycontext = identitycontext;
            _logger = logger;
        }


        public async Task LogTotableE(IxmAppLogType LogType, IxmAppSourceObjects SourceObj, string SystemId, string SourceId, string Message, string User)
        {

            var DT = new APP_LOGTABLE()
            {
                TableName = "APP_LOGTABLEE",
                SYSTEMID = SystemId,
                LOGTIME = DateTime.UtcNow,
                LOGTYPE = LogType.ToString(),
                DESCRIPTION = Message,
                SOURCEOBJ = SourceObj.ToString(),
                SOURCEID = SourceId,
                USERNAME = User
            };

            await SaveValues(DT);

        }

        public async Task LogTotableF(IxmAppLogType LogType, IxmAppSourceObjects SourceObj, string SystemId, string SourceId, string Message, string User)
        {

            var FT = new APP_LOGTABLE()
            {
                TableName = "APP_LOGTABLEF",
                SYSTEMID = SystemId,
                LOGTIME = DateTime.UtcNow,
                LOGTYPE = LogType.ToString(),
                DESCRIPTION = Message,
                SOURCEOBJ = SourceObj.ToString(),
                SOURCEID = SourceId,
                USERNAME = User
            };

            await SaveValues(FT);

        }


        public async Task LogTotableR(IxmAppLogType LogType, IxmAppSourceObjects SourceObj, string SystemId, string SourceId, string Message, string User)
        {

            var RT = new APP_LOGTABLE()
            {
                TableName = "APP_LOGTABLER",
                SYSTEMID = SystemId,
                LOGTIME = DateTime.UtcNow,
                LOGTYPE = LogType.ToString(),
                DESCRIPTION = Message,
                SOURCEOBJ = SourceObj.ToString(),
                SOURCEID = SourceId,
                USERNAME = User
            };

            await SaveValues(RT);

        }
        public async Task LogTotableI(IxmAppLogType LogType, IxmAppSourceObjects SourceObj, string SystemId, string SourceId, string Message, string User)
        {

            var RT = new APP_LOGTABLE()
            {
                TableName = "APP_LOGTABLEI",
                SYSTEMID = SystemId,
                LOGTIME = DateTime.UtcNow,
                LOGTYPE = LogType.ToString(),
                DESCRIPTION = Message,
                SOURCEOBJ = SourceObj.ToString(),
                SOURCEID = SourceId,
                USERNAME = User
            };

            await SaveValues(RT);

        }
        private async Task SaveValues(APP_LOGTABLE FT)
        {
            Regex reg = new Regex("[*'\",&#^@{}%$!+]");
            //var lVal = reg.Replace(string.Concat(FT.DESCRIPTION.Where(Char.IsLetterOrDigit)), string.Empty);
            var lVal = reg.Replace(FT.DESCRIPTION, string.Empty);
            //str1 = reg.Replace(str1, string.Empty);
            //lVal = lVal.Replace("0", " ");

            try
            {
                string lSql = "INSERT INTO " + FT.TableName + " VALUES ( " + FT.SYSTEMID + ", current_timestamp,'" + FT.LOGTYPE + "','" + FT.SOURCEOBJ + "'" + ",'" + FT.SOURCEID + "','" +
                    lVal + "','" + FT.USERNAME + "',NULL,NULL,NULL)";

                await _context.Database.ExecuteSqlRawAsync(lSql);

                _logger.LogInformation(FT.LOGTYPE + " | " + FT.SOURCEOBJ + " | " + FT.USERNAME + " | " + FT.DESCRIPTION);


            }
            catch (Exception e)
            {
                _logger.LogInformation(FT.LOGTYPE + " | " + FT.SOURCEOBJ + " | " + FT.USERNAME + " | Error :: " + e.Message);
            }

        }


        public async Task<List<APP_LOGTABLEE>> GetFromLogTableE(IxmAppSourceObjects SourceObj, string SystemId, string SourceId)
        {

            var value = await _context.APP_LOGTABLEE.Where(a => a.SOURCEOBJ == SourceObj.ToString())
                                                   .Where(b => b.SOURCEID == SourceId)
                                                   .ToListAsync();
            return value;
        }

        public async Task<List<APP_LOGTABLEI>> GetFromLogTableI(IxmAppSourceObjects SourceObj, string SystemId, string SourceId)
        {

            var value = await _context.APP_LOGTABLEI.Where(a => a.SOURCEOBJ == SourceObj.ToString())
                                                   .Where(b => b.SOURCEID == SourceId)
                                                   .ToListAsync();
            return value;
        }

        public async Task<List<APP_LOGTABLEF>> GetFromLogTableF(IxmAppSourceObjects SourceObj, string SystemId, string SourceId)
        {

            var value = await _context.APP_LOGTABLEF.Where(a => a.SOURCEOBJ == SourceObj.ToString())
                                                   .Where(b => b.SOURCEID == SourceId)
                                                   .ToListAsync();
            return value;
        }

        public async Task<List<APP_LOGTABLER>> GetFromLogTableR(IxmAppSourceObjects SourceObj, string SystemId, string SourceId)
        {

            var value = await _context.APP_LOGTABLER.Where(a => a.SOURCEOBJ == SourceObj.ToString())
                                                   .Where(b => b.SOURCEID == SourceId)
                                                   .ToListAsync();
            return value;
        }
    }

}
