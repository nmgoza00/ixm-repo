using IXM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using IXM.GeneralSQL;
using IXM.Common;
using IXM.Models.Core;

namespace IXM.DB
{
    public class CustomUpdates : ICustomUpdates
    {

        private readonly IXMDBContext _context;
        private readonly IXMDBIdentity _identitycontext;
        private readonly IGeneralSQL _generalSQL;
        private readonly ILogger<CustomUpdates> _logger;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _usermanager;

        public CustomUpdates(IXMDBContext context, 
                             IXMDBIdentity idtcontext, 
                             UserManager<ApplicationUser> usermanager,
                             IGeneralSQL generalSQL,
                             IConfiguration configuration,
                             ILogger<CustomUpdates> logger)
        {
            _context = context;
            _identitycontext = idtcontext;
            _usermanager = usermanager;
            _configuration = configuration;
            _generalSQL = generalSQL;
            _logger = logger;
        }
        

       
        public Task GetData<TEntity>(string lsql)
        {

           var lResult = _context.Database.ExecuteSqlRaw(lsql);            
            return Task.FromResult(0);

        }
        public Task StatusUpdate(string Username, string TBNAME, string KEYFIELD, string KEYVALUE, string UPDFIELD, int UPDVALUE)
        {
            try
            {
                var lSql = "UPDATE " + TBNAME +
                    "  SET " + UPDFIELD + " = " + UPDVALUE.ToString() +
                    ", MODBY = '" + Username + "'" +
                    ", MODAT = current_timestamp" +
                    "  WHERE " + KEYFIELD + " = " + KEYVALUE;

                _context.Database.ExecuteSqlRaw(lSql);
                var lresult = _context.SaveChanges();
                
                _logger.LogTrace("Custom Update Successfully made {@user}", Username);


            }
            catch (Exception E)
            {
                _logger.LogError("Error encountered :: {@Message}", E.Message);
            }
            return Task.FromResult(0);
        }


        public Task RemittanceUpdateDBDetails(string MEMEXISTS, string Username, string RMBLID)
        {

            _logger.LogInformation("Starting Remittance Member Update Process {@user}", Username);

            _context.Database.ExecuteSqlRaw("EXECUTE PROCEDURE SP_TRMBLD_DBVALUES ({0}, {1}, '{2}')", RMBLID, MEMEXISTS, Username);

            _logger.LogInformation("Completed Remittance Member Update Process {@user}", Username);

            return Task.FromResult(0);

        }

        public Task RemittanceErrorsToDB(ApplicationUser au, List<TRMBLD> RMBLID)
        {

            //var lScriptFile = "SCH_UPD_VALUES.block";

            //string lpath = _configuration.GetConnectionString("ScripBlockFolder");
            //lpath = lpath + lScriptFile;
            //var lScript = _generalSQL.GetBlockExecution(lpath);
            //lScript = lScript.Replace(":RMBLID", RMBLID);

            _context.Database.ExecuteSqlRaw("EXECUTE PROCEDURE SP_TRMBLD_DBVALUES ({0}, '{1}')", RMBLID, au.SYSTEM_UNAME.ToString());

            return Task.FromResult(0);

        }



        public Task RemittanceMembersUpdate(string Username, string RMBLID, int MEMBERS)
        {
            string lSql = "UPDATE TRMBL SET STATUSID = (SELECT STATUSID FROM MSTATUS WHERE STATUS_TYPE = 'TRMBL' AND STATUS_SEQ = 2), IAMOUNT = (SELECT SUM(IAMOUNT) FROM TRMBLD WHERE RMBLID = " + RMBLID + " AND VERSIONID=1), MEMBERS = " + MEMBERS.ToString() +", MODBY = '" + Username + "' WHERE RMBLID = " + RMBLID;

            _context.Database.ExecuteSqlRaw(lSql);

            return Task.FromResult(0);

        }

        /*
        public int AddEditDocumentToDB(ILogger logger, MOBJECT_DOC model)
        {

            try
            {
                _logger.LogInformation("Data to Load {@model}", model);

                model.INSERT_DATE = DateTime.Now;
                model.LT = 1;
                var mOBJECT_DOC = _context.MOBJECT_DOC.Where(b => b.SOURCEOBJ == model.SOURCEOBJ)
                                                      .Where(b => b.SOURCEFLD == model.SOURCEFLD)
                                                      .Where(b => b.SOURCEID == model.SOURCEID)
                                                      .Where(b => b.DOCTYPE == model.DOCTYPE).ToList();



                _logger.LogInformation("Data extracted from Context {@model}", mOBJECT_DOC);

                mOBJECT_DOC.ForEach(b =>
                {
                    b.LT = 0;
                    b.EFFDATE = DateTime.Now;
                    b.INSERTED_BY = model.UNAME;
                    _context.Update(b);
                });


                model.OBJECTID = _general.GetSEQUENCE("SEQOBJECT_STA");
                _context.MOBJECT_DOC.Add(model);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return -1;

            }

            return model.OBJECTID;
        }*/



    }
}
