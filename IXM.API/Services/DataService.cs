//using System.Data.Entity.Validation;
using IXM.Constants;
using IXM.DB;
using IXM.DB.Services;
using IXM.Models;
using IXM.Models.Core;
using IXM.Models.Write;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Validation;
using System.Reflection;
using static IXM.DB.QueryRepository;


namespace IXM.API.Services
{
    public class DataService : IDataService
    {
        private readonly IXMDBContext _context;
        private readonly IXMWriteDBContext _wrcontext;
        private readonly IXMDBIdentity _identitycontext;
        private readonly IConfiguration _configuration;
        private readonly IIXMDBRepo _general;
        private readonly IDataValidator _datavalidator;
        private readonly ILogger<DataService> _logger;
        private readonly IIXMTransactionRepo _transactrepo;
        //private readonly IQueryRepository _queryRepository;

        public DataService(IXMDBContext context, IXMWriteDBContext wrcontext,
        IXMDBIdentity idtcontext, 
            IConfiguration configuration, 
            IDataValidator dataValidator,
            ILogger<DataService> logger,
        IIXMDBRepo general,
        IIXMTransactionRepo transactrepo)
        {
            _context = context;
            _wrcontext = wrcontext;
            _datavalidator = dataValidator;
            _identitycontext = idtcontext;
            _general = general;
            _configuration = configuration;
            _logger = logger;
            _transactrepo = transactrepo;

        }


        public string GetDepartmentLoadedSchedules(string pYear)
        {

            var lSql = "SELECT GEN_UUID() UUID, TPAYMENTSC.INSBY, MPERIOD.myear, MAX(MPERIOD.myearmonth) MYEARMONTH, MP2.myearmonth LOADYEARMONTH, max(VMUSER.NAME ||' '||VMUSER.SURNAME) fullname, max(vmuser.localoffice) localoffice,COUNT(TPAYMENTSC.trancount) TRANCOUNT, " +
                " SUM(TPAYMENTSC.memcount) MEMCOUNT, SUM(TPAYMENTSC.iamount) IAMOUNT,SUM(1) SCHEDULES  FROM TPAYMENTSC INNER JOIN MPERIOD ON TPAYMENTSC.PERIODID = MPERIOD.PRID " +
                " INNER JOIN MPERIOD MP2 ON TPAYMENTSC.INSDAT BETWEEN MP2.SDATE AND MP2.EDATE " +
                " LEFT OUTER JOIN VMUSER ON TPAYMENTSC.insby = VMUSER.UNAME WHERE TPAYMENTSC.DETYPE = 'HD' " +
                " AND MP2.MYEAR = " + pYear + "" +
                " GROUP BY TPAYMENTSC.INSBY,MPERIOD.myear, MP2.myearmonth";

            return lSql;

        }



        public string GetLinkedUserToEmployee(string pUserId)
        {

            var lSql = " SELECT e.EMPID, e.EMPTYPID,  e.MNAME, e.MSURNAME, e.FULLNAME, u.CELLNUMBER, e.EMAIL, e.MEMBERID, u.AUTHCODE ID_USERID, e.USERID" +
            " FROM MEMPLOYEE e, MUSER u WHERE e.USERID = u.USERID AND u.AUTHCODE = CASE WHEN '" + pUserId + "' = '' THEN u.AUTHCODE ELSE '" + pUserId + "' END ";

            return lSql;

        }

        public string GetLinkedUserDeviceInfo(string pUserId)
        {

            var lSql = " SELECT e.EMPID, e.MNAME, e.MSURNAME, e.FULLNAME, u.AUTHCODE ID_UserId, u.EMAILADDRESS, u.CELLNUMBER " +
            " FROM MEMPLOYEE e, MUSER u WHERE e.USERID = u.USERID AND u.AUTHCODE = CASE WHEN '" + pUserId + "' = '' THEN u.AUTHCODE ELSE '" + pUserId + "' END ";

            return lSql;

        }
        public string GetOrganiser_BranchStats(string pUserId)
        {

            var lSql = " SELECT MPERIOD.PRID PERIODID, BCOMPANY.COMPANYID, MPERIOD.MYEARMONTH, BCOMPANY.COMPANYNUM, BCOMPANY.CNAME, BCOMPANY.USERID, " +
                       " BCOMPANY.MMEMCOUNT, IIF(TPAYMENT.PAYMENTNUM IS NULL,'', TPAYMENT.PAYMENTNUM) PAYMENTNUM, " +
                       " IIF(TPAYMENTSB.IAMOUNT IS NULL, 0, TPAYMENTSB.IAMOUNT) IAMOUNT, " +
                       " IIF(TPAYMENTSB.MEMCOUNT IS NULL, 0, TPAYMENTSB.MEMCOUNT) PMEMCOUNT " +
                       "  FROM " +
                       " (SELECT BCOMPANY.*, (SELECT MAX(PERIODID) FROM TPAYMENTSB " +
                       " WHERE TPAYMENTSB.BCOMPANYID = BCOMPANY.COMPANYID) PERIODID, " +
                       " (SELECT COUNT(1) FROM MMEMBER WHERE MMEMBER.BCOMPANYID = BCOMPANY.COMPANYID " +
                       " AND MMEMBER.MEMSTATUSID NOT IN (3,12,18)) MMEMCOUNT FROM (SELECT b.COMPANYID, " +
                       " b.COMPANYNUM, b.CNAME, u.USERID FROM CCUR c, MCOMPANY b, MUSER u, MEMPLOYEE e " +
                       " WHERE C.SRCOBJ='MEMPLYEE' AND c.COMPANYID = b.COMPANYID AND b.COMPTYPID = 4 " +
                       " AND c.SRCID = e.EMPID " +
                       " AND e.USERID = u.USERID AND u.AUTHCODE = '" + pUserId + "') BCOMPANY ) BCOMPANY " +
                       " INNER JOIN MPERIOD ON MPERIOD.PRID BETWEEN BCOMPANY.PERIODID-1 AND BCOMPANY.PERIODID+1 " +
                       " LEFT JOIN TPAYMENTSB ON TPAYMENTSB.PERIODID = MPERIOD.PRID AND TPAYMENTSB.BCOMPANYID = BCOMPANY.COMPANYID " +
                       " LEFT JOIN TPAYMENT ON TPAYMENT.PAYMENTID = TPAYMENTSB.PAYMENTID " +
                       " ORDER BY MPERIOD.PRID";

            return lSql;

        }


        public string GetMember_Details(string pid)
        {

            var lSql = " SELECT m.*, mlp.PYEAR||' '||mlp.PMONTH PYEARMONTH, CASE WHEN mcd.MEMBERID IS NULL THEN 'N' ELSE 'Y' END MemCardReceived " +
                       " FROM MMEMBER m" +
                       " INNER JOIN MCOMPANY b ON b.COMPANYID = m.COMPANYID " +
                       " LEFT JOIN MMCDD mcd ON mcd.MEMBERID = m.MEMBERID AND mcd.LT=1 " +
                       " LEFT JOIN MMLP mlp ON mlp.MEMBERID = m.MEMBERID " +
                       " WHERE m.MEMBERID = " + pid;

            return lSql;

        }


        public string GetBranch_NonPaymentMembers(string pPeriod, string pBCompany)
        {

            var lSql = " SELECT m.MEMBERID, m.MNAME,m.MSURNAME, mp.MYEAR || ' - ' || mp.MMONTH PYEARMONTH, CASE WHEN mcd.MEMBERID IS NULL THEN 'N' ELSE 'Y' END MemCardReceived  " +
                        " FROM MMEMBER m INNER JOIN TPAYMENTSB " +
                        " ON TPAYMENTSB.PERIODID = " + pPeriod + " AND TPAYMENTSB.BCOMPANYID = m.BCOMPANYID " +
                        " AND TPAYMENTSB.BCOMPANYID = " + pBCompany +
                        " LEFT JOIN MMCDD mcd ON m.MEMBERID = mcd.MEMBERID AND mcd.LT=1" +
                        " INNER JOIN MPERIOD mp ON mp.PRID = TPAYMENTSB.PERIODID " +
                        " LEFT JOIN TPAYMENT_DET td ON td.PAYMENTID = TPAYMENTSB.PAYMENTID " +
                        " AND td.MEMBERID = m.MEMBERID " +
                        " WHERE m.MEMSTATUSID NOT IN(3, 12, 18) AND td.MEMBERID IS NULL ";

            return lSql;

        }

        public string GetIdentity_UserList(string pUsrName)
        {

            var lSql = "SELECT CASE WHEN ms.\"SystemId\" IS NOT NULL AND mro.\"RoleId\" IS NOT NULL THEN 'Verified' " +
                " WHEN ((ms.\"SystemId\" IS NULL AND mro.\"RoleId\" IS NOT NULL) OR (ms.\"SystemId\" IS NULL AND mro.\"RoleId\" IS NOT NULL)) THEN 'Partial' " +
                " WHEN ms.\"SystemId\" IS NULL AND mro.\"RoleId\" IS NULL THEN 'Guest' END UserStatus, " +
                " CASE WHEN ms.\"SystemId\" IS NOT NULL THEN 'Granted' END SystemName, r.\"Name\" ROLENAME, " +
                " usr.\"UserName\" USERNAME, usr.\"Email\" EMAIL, usr.\"PhoneNumber\" CELLNUMBER " +
                " FROM \"Users\" usr " +
                " LEFT JOIN MUSER_ROLE mro ON mro.\"UserId\" = usr.\"Id\" " +
                " LEFT JOIN MUSER_SYSTEM ms ON ms.\"UserId\" = usr.\"Id\"  " +
                " LEFT JOIN \"Roles\" r ON r.\"Id\" = mro.\"RoleId\" ";

            return lSql;
        }


        public string GetDistribution_CardStats(string pBCompanyId, string pUsrName)
        {

            var lSql = " SELECT gen_uuid() ChartKey, max(CASE WHEN mcd.MEMBERID IS NULL THEN 'N' ELSE 'Y' END) Label, " +
                " m.BCOMPANYID LabelGroupId,'BCOMPANYID' LabelGroupName, MAX(mc.CNAME) LabelGroupDescription, " +
                " COUNT(1) LabelValue, '' Color FROM MMEMBER m " +
                " INNER JOIN CCUR ld ON ld.COMPANYID = m.BCOMPANYID AND ld.SRCOBJ = 'MEMPLOYEE' " +
                " AND ld.COMPANYID = CASE WHEN "+pBCompanyId+" = -99 THEN ld.COMPANYID ELSE "+pBCompanyId+" END " +
                " INNER JOIN MEMPLOYEE emp ON emp.EMPID = ld.SRCID " +
                " INNER JOIN MUSER mld ON mld.USERID = emp.USERID  AND mld.AUTHCODE = '" +pUsrName+"' " +
                " INNER JOIN MCOMPANY mc ON mc.COMPANYID = ld.COMPANYID " +
                " LEFT JOIN MMCDD mcd ON m.MEMBERID = mcd.MEMBERID AND mcd.LT = 1 AND mcd.WI = 'Y' " +
                " LEFT JOIN TSURVEYMEMBERDATA ts ON ts.MEMBERID = m.MEMBERID " +
                " LEFT JOIN TSURVEY_C t ON t.SURVEYID = ts.SURVEYID AND t.ISACTIVE = 'Y' " +
                " WHERE m.MEMSTATUSID NOT IN (3, 12, 18) GROUP BY m.BCOMPANYID, CASE WHEN mcd.MEMBERID IS NULL THEN 'N' ELSE 'Y' END "; 


            return lSql;

        }

        public string GetDistribution_SurveyStats(string pBCompanyId, string pUsrName)
        {

            var lSql = " SELECT gen_uuid() ChartKey, max(CASE WHEN ts.MEMBERID IS NULL THEN 'N' ELSE 'Y' END) Label, " +
                " m.BCOMPANYID LabelGroupId,'BCOMPANYID' LabelGroupName, MAX(mc.CNAME) LabelGroupDescription, " +
                " COUNT(1) LabelValue, '' Color FROM MMEMBER m " +
                " INNER JOIN CCUR ld ON ld.COMPANYID = m.BCOMPANYID AND ld.SRCOBJ = 'MUSER' " +
                " AND ld.COMPANYID = CASE WHEN " + pBCompanyId + " = -99 THEN ld.COMPANYID ELSE " + pBCompanyId + " END " +
                " INNER JOIN MUSER mld ON mld.USERID = ld.USERID  AND mld.UNAME = '" + pUsrName + "' " +
                " INNER JOIN MCOMPANY mc ON mc.COMPANYID = ld.COMPANYID " +
                " LEFT JOIN TSURVEYMEMBERDATA ts ON ts.MEMBERID = m.MEMBERID " +
                " LEFT JOIN TSURVEY_C t ON t.SURVEYID = ts.SURVEYID AND t.ISACTIVE = 'Y' " +
                " WHERE m.MEMSTATUSID NOT IN (3, 12, 18) GROUP BY m.BCOMPANYID, CASE WHEN ts.MEMBERID IS NULL THEN 'N' ELSE 'Y' END ";

            return lSql;

        }


        public string GetSurvey_Member(string pMemberId)
        {

            var lSql = " SELECT tmc.MEMBERID , tmc.LOCALITYID, tmc.SURVEYDATE, tmc.SRVY_QU01, tmc.SRVY_QU02 " +
                ", tmc.SRVY_QU03, tmc.SRVY_QU04, tmc.SRVY_QU05, tmc.SRVY_QU06, tmc.SRVY_QU07, tmc.SRVY_QU08 " +
                ", tmc.SRVY_QU09, tmc.SRVY_QU10 " +
                "  FROM TSURVEY_MEMBER_CARDS tmc " +
                "  INNER JOIN MMEMBER m ON m.MEMBERID = tmc.MEMBERID " +
                "  WHERE m.MEMBERID = " + pMemberId;


            return lSql;

        }



        public string GetSurvey_CurrentList(string pMemberId)
        {

            var lSql = " SELECT tmb.MEMBERID, tdc.*, CASE WHEN tdc.FIELDNAME='SRVY_QU01' THEN tmb.SRVY_QU01 " +
                       " WHEN tdc.FIELDNAME='SRVY_QU02' THEN tmb.SRVY_QU02 " +
                       " WHEN tdc.FIELDNAME='SRVY_QU03' THEN tmb.SRVY_QU03 " +
                       " WHEN tdc.FIELDNAME='SRVY_QU04' THEN tmb.SRVY_QU04 " +
                       " WHEN tdc.FIELDNAME='SRVY_QU05' THEN tmb.SRVY_QU05 " +
                       " WHEN tdc.FIELDNAME='SRVY_QU06' THEN tmb.SRVY_QU06 " +
                       " WHEN tdc.FIELDNAME='SRVY_QU07' THEN tmb.SRVY_QU07 " +
                       " WHEN tdc.FIELDNAME='SRVY_QU08' THEN tmb.SRVY_QU08 " +
                       " WHEN tdc.FIELDNAME='SRVY_QU09' THEN tmb.SRVY_QU09 " +
                       " WHEN tdc.FIELDNAME='SRVY_QU10' THEN tmb.SRVY_QU10 END FIELDVALUE " +
                       " FROM TSURVEY_C tc INNER JOIN TDSURVEY_C tdc ON tdc.SURVEYID = tc.SURVEYID " +
                       " AND tc.ISACTIVE = 'Y' LEFT JOIN TSURVEYMEMBERDATA tmb ON tmb.MEMBERID = " + pMemberId;

            return lSql;

        }


        public string GetPureDocuments(string pCompanyId)
        {

            var lSql = " SELECT MD.*,m.CODE_TEXT, M.CODE_FPATH FROM MOBJECT_DOC md LEFT JOIN MCODE m ON m.CODE_VALUE = md.DOCTYPE WHERE md.SOURCEOBJ NOT IN('TPAYMENT') " +
                        " AND md.SOURCEFLD = 'COMPANYID' AND md.SOURCEID = " + pCompanyId +
                        " AND md.LT = 1";

            return lSql;

        }


        public string GetEmployeePhotos(string pEmployeeId)
        {

            var lSql = " SELECT MD.*,m.CODE_TEXT, M.CODE_FPATH FROM MOBJECT_DOC md LEFT JOIN MCODE m ON m.CODE_VALUE = md.DOCTYPE WHERE md.SOURCEOBJ = 'MEMPLOYEE' " +
                        " AND md.SOURCEFLD = 'EMPID' AND md.SOURCEID = " + pEmployeeId +
                        " AND md.LT = 1";

            return lSql;

        }

        public int GetMobileCCDID()
        {

            string lSql = "SELECT CDID SEQVALUE FROM MMCD WHERE REQTYPE = 'MBA'";
            var prSeq = _context.SEQUENCE.FromSqlRaw<SEQUENCE>(lSql);
            return prSeq == null ? -1 : prSeq.First().SEQVALUE;

        }

        public Tuple<int, string> GetObjectFileName(IXMFileType  ixmFileType, int pObjectId)
        {

            var prSeq = _context.MOBJECT_DOCFILE.FromSqlRaw<MOBJECT_DOCFILE>($"SELECT OBJECTID,DOCTYPE, DOCUMENTNAME, LDOCUMENTNAME, SFOLDERNAME, LT,m.CODE_FPATH,(SELECT HVALUE FROM APP_HARDCODES WHERE HARDC_TYPE='IMAGESTORE') IMAGEURI, " +
                " (SELECT HVALUE FROM APP_HARDCODES WHERE HARDC_TYPE='BASEFLDR') BASEFLDR " +
                " FROM MOBJECT_DOC " +
                " LEFT JOIN MCODE m ON m.CODE_VALUE = MOBJECT_DOC.DOCTYPE WHERE MOBJECT_DOC.OBJECTID = @p0", pObjectId);
            var BaseDocFolder = _configuration.GetConnectionString("BaseDocFolder");
            var Baseurl = _configuration.GetConnectionString("BaseURL");
            string sFilename = "";

            if (prSeq != null)
            {
                if (prSeq.First().DOCTYPE == "EMPHOTO")
                {
                    sFilename = Baseurl.ToString() + "APPFILES/" + prSeq.First().CODE_FPATH + "/" + prSeq.First().DOCUMENTNAME;
                    //"/APPFILES/" + prMembers.First().CODE_FPATH + "/" + prMembers.First().PDOCUMENTNAME;

                }
                else if (ixmFileType == IXMFileType.Image)
                {

                    sFilename = prSeq.First().IMAGEURI + "//" + prSeq.First().DOCUMENTNAME;

                }
                else if (ixmFileType == IXMFileType.Image)
                {

                    //var prDocType = _context.MCODE.FromSqlRaw<MCODE>($"SELECT MCODE.*,'' DESCRIPTION FROM MCODE WHERE CODE_VALUE = @p0", prSeqDOCTYPE).ToList();
                    //sFilename = BaseDocFolder + prDocType.First().CODE_FPATH.ToString() + "\\" + b.DOCUMENTNAME.ToString();
                    sFilename = prSeq.First().IMAGEURI + "//" + prSeq.First().DOCUMENTNAME;

                }
            }




            return prSeq == null ? new Tuple<int, string>(-1, "FileNotFound") : new Tuple<int, string>(pObjectId, sFilename);


        }

        /*public int AddEditDocumentToDB(ILogger logger, MOBJECT_DOC model)
        {

            try
            {
                logger.LogInformation("Data to Load {@model}", model);

                model.INSERT_DATE = DateTime.Now;
                model.LT = 1;
                var mOBJECT_DOC = _context.MOBJECT_DOC.Where(b => b.SOURCEOBJ == model.SOURCEOBJ)
                                                      .Where(b => b.SOURCEFLD == model.SOURCEFLD)
                                                      .Where(b => b.SOURCEID == model.SOURCEID)
                                                      .Where(b => b.DOCTYPE == model.DOCTYPE).ToList();



                logger.LogInformation("Data extracted from Context {@model}", mOBJECT_DOC);

                mOBJECT_DOC.ForEach(b =>
                {
                    b.LT = 0;
                    b.EFFDATE = DateTime.Now;
                    b.INSERTED_BY = model.UNAME;
                    _context.Update(b);
                });


                model.OBJECTID = _general.General.GetSEQUENCE("SEQOBJECT_STA");
                _context.MOBJECT_DOC.Add(model);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return -1;

            }

            return model.OBJECTID;
        }*/


        public Tuple<int, string> RegisterBusinessUploadToRemmittance(ILogger logger, MOBJECT_DOC model, int pPeriodId, int pRSTYPEID)
        {

            try
            {


                var _result = _datavalidator.HasB2BRemmittanceLoaded(pPeriodId, model.SOURCEID);
                if (_result.Item1 != 0 )
                {

                    _general.AppLog.LogTotableE(IxmAppLogType.LogB2B, IxmAppSourceObjects.B2BLOADS, model.SYSTEMID, model.SOURCEID.ToString(), _result.Item3.ToString(), model.INSERTED_BY);
                    _logger.LogInformation("Record to be Updaetd : {@model}", _result);

                    var tRMBL = _context.TRMBL.Where(a => a.COMPANYID == model.SOURCEID).Where(b => b.PERIODID == pPeriodId).ToList();
                    if (tRMBL.Count() > 0)
                    {
                        tRMBL.Single().OBJECTID = model.OBJECTID;
                        tRMBL.Single().FLOADS = ++tRMBL.Single().FLOADS;

                        _context.Entry(tRMBL.Single()).State = EntityState.Modified;
                        _context.SaveChanges();
                    }

                    if (_result.Item1 == -2)
                    {
                        return new Tuple<int, string>(tRMBL.Single().RMBLID, _result.Item3);
                    } else
                    {
                        return new Tuple<int, string>(-1, _result.Item3);
                    }

                } else
                {

                    _logger.LogInformation("New Remittance Record to be added");

                    TRMBL_POST ldt = new TRMBL_POST
                    {

                        COMPANYID = model.SOURCEID,
                        OBJECTID = model.OBJECTID,
                        PERIODID = pPeriodId,
                        RSTYPEID = pRSTYPEID,
                        INSBY = model.INSERTED_BY,
                        MODBY = model.INSERTED_BY

                    };

                    var dt1 = _transactrepo.Transaction.GenerateRemittanceHeader(ldt, model.INSERTED_BY);

                    _context.TRMBL.Add(dt1);
                    _context.SaveChanges();


                    return new Tuple<int, string>(dt1.RMBLID, "File loaded successfully.");

                };


            }
            catch (Exception ex)
            {

                  _general.AppLog.LogTotableE(IxmAppLogType.LogB2B, IxmAppSourceObjects.B2BLOADS, model.SYSTEMID, model.SOURCEID.ToString(), ex.Message + " :: File : " + model.ORGIMAGEFILE.FileName, model.INSERTED_BY);
    
                return new Tuple<int, string>(-1, ex.Message);
            }
        }

        public Tuple<int, string> ModifyEmployee(ILogger logger, MEMPLOYEE_WRITE mEMPLOYEE)
        {

            // Get Definitions
            var toUpdate = new[] { "MNAME", "MSURNAME", "CELLNUMBER", "USERID", "MEMBERID","MODAT", "MODBY" };
            Type type = typeof(MEMPLOYEE_WRITE);
            PropertyInfo[] properties = type.GetProperties();

            // Arrange objects to modify
            _wrcontext.Entry(mEMPLOYEE).State = EntityState.Modified;
            var entry = _wrcontext.Entry(mEMPLOYEE);

            //Set Update Properties
            foreach (PropertyInfo property in properties)
            {

                entry.Property(property.Name).IsModified = false;

            }
            foreach (var pname in toUpdate)
            {
                if (entry.Property(pname).CurrentValue != null) { entry.Property(pname).IsModified = true; }
            }

            try
            {

                _wrcontext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                return new Tuple<int, string>(-1, ex.Message);
            }
            return new Tuple<int, string>(0, "Update Processed");

        }
        public bool ModifyUser(ILogger logger, MUSER mMUSER)
        {

            // Get Definitions
            var toUpdate = new[] { "NAME", "SURNAME", "CELLNUMBER", "MSTATUSID","TELNUMBER","IPOSITIONID","EXPDAT","APPSERVICE", "UNAME","UGID","LOCALITYID","EMAILADDRESS","MEMBERID" };
            Type type = typeof(MUSER);
            PropertyInfo[] properties = type.GetProperties();

            // Arrange objects to modify
            _context.Entry(mMUSER).State = EntityState.Modified;
            var entry = _context.Entry(mMUSER);

            //Set Update Properties
            foreach (PropertyInfo property in properties)
            {
                try
                {

                    entry.Property(property.Name).IsModified = false;
                }
                catch 
                { 

                }

            }
            foreach (var pname in toUpdate)
            {
                entry.Property(pname).IsModified = true;
            }

            _context.SaveChanges();

            return false;
        }



        public bool ModifyCompany(ILogger logger, MCOMPANY_WRITE mCOMPANY)
        {

            // Get Definitions
            var toUpdate = new[] { "CNAME", "COMPANYNUM", "COMPTYPID", "CELL_NUMBER", "TEL_NUMBER","REG_NUM", "WEBADDRESS", "E_MAIL", "WREGCODE", "CONTACT_PERSON", "BANK_REFNR", "HMEMCNT", "HMEMAVG", "EXT_COCODE","COMPANYINFO"};
            Type type = typeof(MCOMPANY_WRITE);
            PropertyInfo[] properties = type.GetProperties();

            // Arrange objects to modify
            _wrcontext.Entry(mCOMPANY).State = EntityState.Modified;
            var entry = _wrcontext.Entry(mCOMPANY);

            //Set Update Properties
            foreach (PropertyInfo property in properties)
            {

                entry.Property(property.Name).IsModified = false;

            }
            foreach (var pname in toUpdate)
            {
                entry.Property(pname).IsModified = true;
            }

            _wrcontext.SaveChanges();

            return false;
        }

        public async Task<bool> InsertNote(ILogger logger, MOBJECT_NOT mMOBJECT_NOT)
        {

            mMOBJECT_NOT.NOTEID = _general.General.GetSEQUENCE("SEQ_"+nameof(MOBJECT_NOT));
            _context.Entry(mMOBJECT_NOT).State = EntityState.Added;
            await _context.SaveChangesAsync();

            return true;
        }


        public IEnumerable<MUSER_MENU> GetSystemMenuRights(string pIdentityUserID, int pSystemUserId)
        {
            /// Check if System Db is Firebird or SQL Server
           // string lSql = "SELECT * FROM SP_REPORTMENU2(" + pSystemUserId + ")";

            var prSeq = _context.MUSER_MENU.FromSqlRaw<MUSER_MENU>($"SELECT mu.UNAME SYSTEMUSERNAME, TRIM(m.MENUID) MENUID, TRIM(m.PARENTID) PARENTID, m.LEVEL_L, cg.USERID, m.FRIENDLYNAME, m.FORMHEADER, m.TECHNICALNAME, m.FOBJECT, m.FTYPE, " +
                " cr.AR_ADD, cr.AR_EDIT, cr.AR_DELETE, '' IMAGEURL, mu.ISACTIVE, mu.MSTATUSID, mu.EXPDAT FROM CUSER_GROUP cg " +
                " INNER JOIN CGROUP_RIGHTS cr ON CG.UGID = CR.UGID "+
                " INNER JOIN MMAINMENU m ON m.MENUID = cr.MENUID " +
                " INNER JOIN MUSER mu ON mu.USERID = cg.USERID " +
                " WHERE cg.USERID = " + pSystemUserId.ToString() +
                " ORDER BY m.PARENTID, m.MENUORDER");

            //return prSeq == null ? new [] MUSER_MENU() : (MUSER_MENU);
            return (IEnumerable<MUSER_MENU>)prSeq;

        }



        public bool InsertEmployee(ILogger logger, MEMPLOYEE mEMPLOYEE)
        {
            //if (pid != company.MEMBERID) return BadRequest();
            mEMPLOYEE.EMPID = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQEMPLOYEE));

            // Get Definitions
            var toUpdate = new[] { "MNAME", "MSURNAME", "CELLNUMBER", "EMPNUMBER", "SALARY" };
            //Type type = typeof(MMEMBER_C);
            //PropertyInfo[] properties = type.GetProperties();

            _context.Database.ExecuteSqlRaw("EXECUTE PROCEDURE SP_APP_INSERT_MEMBER({0},{1},{2},{3},{4},{5},{6},{7},{8})",
                mEMPLOYEE.EMPID,
                mEMPLOYEE.EMPTYPID,
                mEMPLOYEE.MNAME,
                mEMPLOYEE.MSURNAME,
                mEMPLOYEE.IDNUMBER,
                mEMPLOYEE.CELLNUMBER,
                mEMPLOYEE.EMAIL,
                mEMPLOYEE.IPOSITIONID,
                mEMPLOYEE.LOCALITYID);
            // Arrange objects to modify
            //_context.Entry(mMEMBER).State = EntityState.Added;
            //var entry = _context.Entry(mMEMBER);
            //var entry = _context.MMEMBER.Add(mMEMBER);


            _context.SaveChanges();
            //ModifyMemberCard(logger, mMEMBER);


            //var companyId = company.CompanyId;

            return false;
        }

        public async Task<int> InsertUser(ILogger logger, MUSERWr mMUSER)
        {


            try
            {
                var cUSER = _context.MUSER.Where(a => a.USERID == mMUSER.USERID).FirstOrDefault();


                if (cUSER != null)
                {
                    var tPROJECTu = _context.MUSER.Where(b => b.USERID == mMUSER.USERID)
                                   .ExecuteUpdate(up => up.SetProperty(upd => upd.INSERT_DATE, DateTime.Now)
                                                          .SetProperty(upd => upd.MODIFIED_BY, mMUSER.MODIFIED_BY)
                                                          .SetProperty(upd => upd.SURNAME, mMUSER.SURNAME)
                                                          .SetProperty(upd => upd.NAME, mMUSER.NAME)
                                                          .SetProperty(upd => upd.MSTATUSID, mMUSER.MSTATUSID)
                                                          .SetProperty(upd => upd.LOCALITYID, mMUSER.LOCALITYID)
                                                          .SetProperty(upd => upd.IPOSITIONID, mMUSER.IPOSITIONID)
                                                          .SetProperty(upd => upd.UGID, mMUSER.UGID)
                                                          .SetProperty(upd => upd.MEMBERID, mMUSER.MEMBERID)
                                                          .SetProperty(upd => upd.EMPID, mMUSER.EMPID)
                                                          .SetProperty(upd => upd.ISACTIVE, mMUSER.ISACTIVE)
                                                          .SetProperty(upd => upd.CELLNUMBER, mMUSER.CELLNUMBER)
                                                          .SetProperty(upd => upd.MCNAME, mMUSER.MCNAME)
                                                          .SetProperty(upd => upd.EXPDAT, mMUSER.EXPDAT)
                                                          .SetProperty(upd => upd.APPSERVICE, mMUSER.APPSERVICE)
                                                          .SetProperty(upd => upd.EMAILADDRESS, mMUSER.EMAILADDRESS)
                                                          .SetProperty(upd => upd.MODIFIED_DATE, DateTime.Now)
                                                          .SetProperty(upd => upd.INSERTED_BY, mMUSER.INSERTED_BY));

                    return cUSER.USERID;

                }
                else
                {

                    //var singlesys = _identitycontext.MUSER.Where(a => a.SYSTEMID == mMUSER.SYSTEMID.ToString()).FirstOrDefault();
                    //var _ctx = _dbfactory.Create("Firebird" + singlesys.SYSTEMNAME);
                    
                    var lUSERID = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQ_MUSER));

                    var sUSER = new MUSER()
                    {
                        USERID = lUSERID,
                        NAME = mMUSER.NAME,
                        SURNAME = mMUSER.SURNAME,
                        UPASSWORD = mMUSER.UPASSWORD,
                        AUTHCODE = mMUSER.AUTHCODE,
                        MEMBERID = mMUSER.MEMBERID,
                        EMPID = mMUSER.EMPID,
                        MSTATUSID = mMUSER.MSTATUSID,
                        LOCALITYID = mMUSER.LOCALITYID,
                        IPOSITIONID = mMUSER.IPOSITIONID,
                        UGID = mMUSER.UGID,
                        ISACTIVE = mMUSER.ISACTIVE,
                        CELLNUMBER = mMUSER.CELLNUMBER,
                        INSERT_DATE = DateTime.Now,
                        INSERTED_BY = mMUSER.INSERTED_BY,
                        MODIFIED_DATE = DateTime.Now,
                        MODIFIED_BY = mMUSER.MODIFIED_BY

                    };

                    _wrcontext.MUSER.Add(sUSER);
                    var lresult = _wrcontext.SaveChanges();
                    if (lresult >= 0)
                    {

                        _logger.LogInformation("User Created : {@model}", sUSER);
                        return lUSERID;

                    }
                    else return -1;

                    return 0;


                }



            }
            catch (Exception ex)
            {

                _logger.LogError("Error Encountered :: {@model}", ex.Message);
                return -2;
            }
        }






            /*

            mMUSER.USERID = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQ_MUSER));

            _context.Database.ExecuteSqlRaw("EXECUTE PROCEDURE SP_APP_INSERT_MUSER({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12})",
                mMUSER.USERID,
                mMUSER.UNAME,
                mMUSER.NAME,
                mMUSER.SURNAME,
                mMUSER.EMAILADDRESS,
                mMUSER.CELLNUMBER,
                mMUSER.MEMBERID,
                mMUSER.UGID,
                mMUSER.LOCALITYID,
                mMUSER.INSERTED_BY,
                mMUSER.INSERT_DATE,
                mMUSER.MODIFIED_DATE,
                mMUSER.MODIFIED_BY);
            // Arrange objects to modify
            //_context.Entry(mMEMBER).State = EntityState.Added;
            //var entry = _context.Entry(mMEMBER);
            //var entry = _context.MMEMBER.Add(mMEMBER);


            _context.SaveChanges();
            //ModifyMemberCard(logger, mMEMBER);


            //var companyId = company.CompanyId;

            return false;*/

        public async Task<bool> UpdateSurveyData(ILogger logger, TSURVEYMEMBERDATA surveyData)
        {

            var tSURVEY = _context.TSURVEYMEMBERDATA.Where(b => b.MEMBERID == surveyData.MEMBERID && b.SURVEYID == surveyData.SURVEYID)
                           .ExecuteUpdate(up => up.SetProperty(upd => upd.BCOMPANYID, surveyData.BCOMPANYID)
                                                  .SetProperty(upd => upd.SURVEYID, surveyData.SURVEYID)
                                                  .SetProperty(upd => upd.SRVY_QU01, surveyData.SRVY_QU01)
                                                  .SetProperty(upd => upd.SRVY_QU02, surveyData.SRVY_QU02)
                                                  .SetProperty(upd => upd.SRVY_QU03, surveyData.SRVY_QU03)
                                                  .SetProperty(upd => upd.SRVY_QU04, surveyData.SRVY_QU04)
                                                  .SetProperty(upd => upd.SRVY_QU05, surveyData.SRVY_QU05)
                                                  .SetProperty(upd => upd.SRVY_QU06, surveyData.SRVY_QU06)
                                                  .SetProperty(upd => upd.SRVY_QU07, surveyData.SRVY_QU07)
                                                  .SetProperty(upd => upd.SRVY_QU08, surveyData.SRVY_QU08)
                                                  .SetProperty(upd => upd.SRVY_QU09, surveyData.SRVY_QU09)
                                                  .SetProperty(upd => upd.SRVY_QU10, surveyData.SRVY_QU10));


            if (tSURVEY == 0)
            {
                await _context.TSURVEYMEMBERDATA.AddAsync(surveyData);
                await _context.SaveChangesAsync();                    

            };

            return false;
        }


        public async Task<bool> UpdateDeviceInfo(ILogger logger, MUSER_DEVICE pDevice)
        {
            try
            {

            var lpDEVICE = _identitycontext.MUSER_DEVICE.Where(b => b.UNAME == pDevice.UNAME &&
                                                                   b.NAME == pDevice.NAME &&
                                                                   b.EMAIL == pDevice.EMAIL &&
                                                                   b.DEVICETYPE == pDevice.DEVICETYPE &&
                                                                   b.MODEL == pDevice.MODEL )
                           .ExecuteUpdate(up => up.SetProperty(upd => upd.IDIOM, pDevice.IDIOM)
                                                  .SetProperty(upd => upd.MODIFIED_BY, pDevice.UNAME)
                                                  .SetProperty(upd => upd.LOGINS, upd => upd.LOGINS == null ? 1 : upd.LOGINS+1)
                                                  .SetProperty(upd => upd.MODIFIED_DATE, DateTime.Now)
                                                  .SetProperty(upd => upd.OSVERSION, pDevice.OSVERSION));


                if (lpDEVICE == 0)
                {
                    _identitycontext.MUSER_DEVICE.Add(pDevice);
                    _identitycontext.SaveChanges();

                };

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return false;

            }


            return false;
        }



    }
}
