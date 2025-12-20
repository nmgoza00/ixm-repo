using IXM.Models;
using IXM.Models.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IXM.DB
{
    public class General : IGeneral
    {

        private readonly IXMDBContext _context;
        private readonly IXMDBIdentity _identitycontext;
        private readonly ILogger<General> _logger;
        private readonly UserManager<ApplicationUser> _usermanager;

        public General(IXMDBContext context, IXMDBIdentity idtcontext,
                                    UserManager<ApplicationUser> usermanager,
                                    ILogger<General> logger)
        {
            _context = context;
            _identitycontext = idtcontext;
            _usermanager = usermanager;
            _logger = logger;
        }
        public int GetSEQUENCE(string SEQNAME)
        {

            string lSql = "SELECT GEN_ID(" + SEQNAME + ",1) SEQVALUE FROM RDB$DATABASE";
            var prSeq = _context.SEQUENCE.FromSqlRaw<SEQUENCE>(lSql);
            return prSeq == null ? -1 : prSeq.First().SEQVALUE;

        }


        public int GetSEQUENCE(string SEQNAME, IXMJobDBContext jobcontext)
        {

            string lSql = "SELECT GEN_ID(" + SEQNAME + ",1) SEQVALUE FROM RDB$DATABASE";
            var prSeq = jobcontext.SEQUENCE.FromSqlRaw<SEQUENCE>(lSql);
            return prSeq == null ? -1 : prSeq.First().SEQVALUE;

        }
        public int GetSEQUENCE(string SEQNAME, IXMSystemDBContext systemcontext)
        {

            string lSql = "SELECT GEN_ID(" + SEQNAME + ",1) SEQVALUE FROM RDB$DATABASE";
            var prSeq = systemcontext.SEQUENCE.FromSqlRaw<SEQUENCE>(lSql);
            return prSeq == null ? -1 : prSeq.First().SEQVALUE;

        }
        public int GetSEQUENCE(string SEQNAME, IXMWriteDBContext context)
        {

            string lSql = "SELECT GEN_ID(" + SEQNAME + ",1) SEQVALUE FROM RDB$DATABASE";
            var prSeq = context.SEQUENCE.FromSqlRaw<SEQUENCE>(lSql);
            return prSeq == null ? -1 : prSeq.First().SEQVALUE;

        }
        public async Task<int> GetSEQUENCE(string SEQNAME, IXMDBContext context)
        {

            string lSql = "SELECT GEN_ID(" + SEQNAME + ",1) SEQVALUE FROM RDB$DATABASE";
            var prSeq = context.SEQUENCE.FromSqlRaw<SEQUENCE>(lSql);
            return prSeq == null ? -1 : prSeq.First().SEQVALUE;

        }

        public string GetConfigPrefix(string OBJECTNAME, string FIELDNAME, int lVal)
        {

            string lSql = "SELECT MCODE.* FROM MCODE WHERE CODE_TEXT = '" + OBJECTNAME + "~" + FIELDNAME + "' AND CODETYPEID = 13";
            var prSeq = _context.MCODE.FromSqlRaw<MCODE>(lSql);
            string lReturnVal = "";

            if ((prSeq != null) && (prSeq.Count() > 0))
            {

                if ((prSeq.First().CODE_LEN != null) & (prSeq.First().CODE_LEN > 0))
                {
                    int lLen = prSeq.First().CODE_LEN == null ? 0 : prSeq.First().CODE_LEN.Value;
                    char p = '0';
                    lReturnVal = lVal.ToString().PadLeft(lLen, p);
                }
                if ((prSeq.First().CODE_PREFIX != null) & (prSeq.First().CODE_PREFIX.Length > 0))
                {

                    int lLen = prSeq.First().CODE_LEN == null ? 10 : prSeq.First().CODE_LEN.Value;
                    string lSub = prSeq.First().CODE_PREFIX == null ? "" : prSeq.First().CODE_PREFIX.ToString();
                    lReturnVal = lSub + lReturnVal.ToString();

                }
                return lReturnVal;
            }
            else { return lVal.ToString(); }

        }

        public int GetConfigStatus(string OBJECTNAME, int SEQUENCE)
        {

            string lSql = "SELECT MSTATUS.* FROM VMSTATUS MSTATUS WHERE STATUS_TYPE = '" + OBJECTNAME + "' AND STATUS_SEQ = " + SEQUENCE.ToString() + " AND ISACTIVE = 'Y'";
            var prSeq = _context.VMSTATUS.FromSqlRaw<VMSTATUS>(lSql);
            return prSeq == null ? -1 : prSeq.First().STATUSID;

        }

        public int GetConfigMStatus(string OBJECTNAME, int SEQUENCE)
        {

            string lSql = "SELECT MSTATUS.* FROM VMSTATUS MSTATUS WHERE STATUS_TYPE = '" + OBJECTNAME + "' AND STATUS_SEQ = " + SEQUENCE.ToString() + " AND ISACTIVE = 'Y'";
            var prSeq = _context.VMSTATUS.FromSqlRaw<VMSTATUS>(lSql);
            return prSeq == null ? -1 : prSeq.First().MSTATUSID;

        }

        public int GetConfigStatus(string OBJECTNAME, int SEQUENCE, IXMWriteDBContext dbcontext)
        {

            string lSql = "SELECT MSTATUS.* FROM VMSTATUS MSTATUS WHERE STATUS_TYPE = '" + OBJECTNAME + "' AND STATUS_SEQ = " + SEQUENCE.ToString() + " AND ISACTIVE = 'Y'";
            var prSeq = dbcontext.VMSTATUS.FromSqlRaw<VMSTATUS>(lSql);
            return prSeq == null ? -1 : prSeq.First().STATUSID;

        }
        public int GetConfigMStatus(string OBJECTNAME, int SEQUENCE, IXMWriteDBContext dbcontext)
        {

            string lSql = "SELECT MSTATUS.* FROM VMSTATUS MSTATUS WHERE STATUS_TYPE = '" + OBJECTNAME + "' AND STATUS_SEQ = " + SEQUENCE.ToString() + " AND ISACTIVE = 'Y'";
            var prSeq = dbcontext.VMSTATUS.FromSqlRaw<VMSTATUS>(lSql);
            return prSeq == null ? -1 : prSeq.First().MSTATUSID;

        }
        public int GetUserGroupId(string GROUPOBJECT, IXMWriteDBContext dbcontext)
        {

            string lSql = "SELECT UGID, DESCRIPTION, UGCODE FROM MUSER_GROUP WHERE UGCODE = '" + GROUPOBJECT + "' AND ISACTIVE = 'Y'";
            var prSeq = dbcontext.MUSER_GROUP.FromSqlRaw<MUSER_GROUP>(lSql);
            return prSeq == null ? -1 : prSeq.First().UGID;

        }

        public MOBJECT_DOC GetObjectDoc(DOCUMENT_UPLOAD docUpload)
        {
            MOBJECT_DOC objDoc = new MOBJECT_DOC();

            objDoc.OBJECTID = docUpload.OBJECTID;
            objDoc.DOCTYPE = docUpload.DOCTYPE.ToString();
            objDoc.DOCUMENTNAME = docUpload.DOCUMENTNAME;  
            objDoc.SOURCEID = docUpload.SOURCEID;

            return objDoc;
        }
        public async Task<ApplicationUser> GetCurrentUserAsync(HttpContext httpContext)
        {
            try
            {
                ApplicationUser usr = await _usermanager.GetUserAsync(httpContext.User);
                if (usr != null)
                {
                    var sysu = _context.MUSER.Where(a => a.AUTHCODE == usr.Id).Select(u => new { u.USERID, u.UNAME }).Single();
                    if (sysu != null)
                    {
                        usr.SYSTEM_USERID = sysu.USERID;
                        usr.SYSTEM_UNAME = sysu.UNAME;
                    }
                    {

                    }
                }

                return usr;

            }
            catch (Exception)
            {

                return null;

            }

        }


    }
}
