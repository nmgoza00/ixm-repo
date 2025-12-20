using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using IXM.DB.Services;
using IXM.Models;
using IXM.GeneralSQL;
using IXM.Common;
using System.IO;
using System.Data.Entity;
using DocumentFormat.OpenXml.InkML;
using IXM.Models.Core;
using IXM.Common.Constant;
using DevExpress.Pdf.Drawing.DirectX;
using DevExpress.Blazor.DateEdit.Internal;
using Microsoft.AspNetCore.Rewrite;

namespace IXM.DB
{
    public class Legal : ILegal
    {

        private readonly IXMDBContext _dbcontext;
        private readonly IXMWriteDBContext _writecontext;
        private readonly IXMDBIdentity _identitycontext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<Legal> _logger;

        public GenFunctions genFunctions = new GenFunctions();

        public Legal(
                            IXMDBIdentity idtcontext,
                            IXMDBContext dbcontext,
                            IXMWriteDBContext writecontext,
                            IConfiguration configuration,
                            ILogger<Legal> logger)
        {
            _dbcontext = dbcontext;
            _writecontext = writecontext;
            _identitycontext = idtcontext;
            _configuration = configuration;
            _logger = logger;
        }


        public async Task<List<TCASE>> GetCases()
        {
            try
            {

                List<TCASE> _cases = _dbcontext.TCASE.ToList();

                return _cases;

            }
            catch (Exception)
            {

                return null;
            }

        }


        public async Task<int> PostCase(TCASE_WRITE value, string pUsername)
        {

            await using (var transaction = await _writecontext.Database.BeginTransactionAsync())
            {

                try
                {

                    TCASE_WRITE tCASE = new();


                    var _result = _writecontext.TCASE.Where(a => a.CASEID == value.CASEID).ToList();
                    if (_result.Count() == 0)
                    {

                        //var lvalue_write = GenerateCaseHeader(value);
                        value.INSDT = DateTime.UtcNow;
                        value.MODAT = DateTime.UtcNow;
                        _writecontext.TCASE.Add(value);
                        var lres = _writecontext.SaveChanges();

                    }
                    else
                    {
                        _result.First().MODAT = DateTime.Now;                        

                        _writecontext.Entry(_result.First()).CurrentValues.SetValues(value);
                        var lres = _writecontext.SaveChanges();


                    }

                }
                catch (Exception e)
                {

                    _logger.LogInformation("Issue Encountered :: Reference RMBLID - Error :: {@Err}", e.Message);
                    transaction.RollbackAsync(); //transaction.Rollback;
                    return -1;
                }


                await transaction.CommitAsync();
                return 0;

            }


        }


        public TCASE_WRITE GenerateCaseHeader(TCASE value)
        {

            //.string lSeqVal = _general.General.GetConfigPrefix("TRMBL", "RMBLNUM", lSeq);
            //int lStatusid = _general.General.GetConfigStatus("TRMBL", 1);
            var dt = new TCASE_WRITE
            {
                CASEID = value.CASEID,
                CASENUMBER = value.CASENUMBER,
                CASEPRIORITY = value.CASEPRIORITY,
                DESCRIPTION = value.DESCRIPTION,
                CASESTATUSID = value.CASESTATUSID,
                INSBY = value.INSBY,
                INSDT = DateTime.Now,
                MODBY = value.MODBY,
                MODAT = DateTime.Now
            };

            _logger.LogInformation("Remmittance - Payment generated :: {@model}", dt);

            return dt;

        }

    }
}
