using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IXM.DB.Services;
using IXM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using IXM.GeneralSQL;

namespace IXM.DB
{
    public class DBTasks : IDBTasks
    {

        private readonly IXMDBContext _context;
        private readonly IXMDBIdentity _identitycontext;
        private readonly ILogger<DBTasks> _logger;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly IGeneralSQL _generalsql;

        public DBTasks(IXMDBContext context, IXMDBIdentity idtcontext,
                                    UserManager<ApplicationUser> usermanager,
                                    IGeneralSQL generalsql,
                                    ILogger<DBTasks> logger)
        {
            _context = context;
            _identitycontext = idtcontext;
            _usermanager = usermanager;
            _logger = logger;
            _generalsql = generalsql;
        }

        public async Task StatusTriggerCreate(string TBNAME, string CHKFIELD, string KEYFIELD)
        {
            try
            {
                var lSql = _generalsql.StatusTriggerCreate(TBNAME, CHKFIELD, KEYFIELD);

                _context.Database.ExecuteSqlRaw(lSql);
                _context.SaveChangesAsync();


            }
            catch (Exception E)
            {

                _logger.LogError("Error encountered :: {@err}",E.Message);
            }

        }
        public async Task StatusTriggerDrop(string TBNAME, string FLDNAME)
        {
            try
            {
                var lSql = _generalsql.StatusTriggerDrop(TBNAME, FLDNAME);

                _context.Database.ExecuteSqlRaw(lSql);
                _context.SaveChangesAsync();


            }
            catch (Exception E)
            {

                _logger.LogError("Error encountered :: {@err}", E.Message);
            }

        }

        public async Task DatabaseBackup(string TBNAME)
        {
            string lSql = _generalsql.DatabaseBackup(TBNAME,"");
        }
        public async Task DatabaseSweep(string TBNAME)
        {

        }


    }
}
