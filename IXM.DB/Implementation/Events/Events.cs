using IXM.Common.Notification;
using IXM.Constants;
using IXM.Models;
using IXM.Models.Events;
using IXM.SQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Math.EC.Endo;


namespace IXM.DB
{
    public class Events : IEvents
    {

        private readonly IXMDBContext _context;
        private readonly IXMSystemDBContext _systemcontext;
        private readonly IXMSystemWrDBContext _systemwrcontext;
        private readonly IXMDBIdentity _identitycontext;
        private readonly IGeneral _general;
        private readonly ILogger<Events> _logger;
        private readonly EventSQL _eventSql;
        private readonly IMemoryCache _memCache;
        private readonly UserManager<ApplicationUser> _usermanager;

        public Events(IXMDBContext context, IXMDBIdentity idtcontext,
                                    IXMSystemDBContext systemcontext,
                                    IXMSystemWrDBContext systemwrcontext,
                            IMemoryCache memoryCache,
                                    UserManager<ApplicationUser> usermanager,
                                    ILogger<Events> logger)
        {

            _context = context;
            _systemcontext = systemcontext;
            _systemwrcontext = systemwrcontext;
            _identitycontext = idtcontext;
            _usermanager = usermanager;
            _logger = logger;
            _memCache = memoryCache;
            _general = new General(context, null, null, null);
            _eventSql = new EventSQL();

        }



        public async Task<List<TPROJECT>> GetEvtProject()
        {
            try
            {

                var result = await _systemcontext.TPROJECT.ToListAsync();

                return result ?? new List<TPROJECT>();


            }
            catch (Exception ex)            
            {

                _logger.LogError("Error Encountered while retrieving entries : {@model}", ex.Message);
                return new List<TPROJECT>();
            }

        }

        public async Task<List<TEVENT>> GetEvtEvent()
        {
            try
            {

                var result = await _systemcontext.TEVENT.ToListAsync();

                return result ?? new List<TEVENT>();


            }
            catch (Exception ex)
            {

                _logger.LogError("Error Encountered while retrieving entries : {@model}", ex.Message);
                return new List<TEVENT>();
            }

        }
        public async Task<int> EventAttend(IIXMNotification notification, int pECOMPID, TPRJEVTD tPRJEVTD, int SystemId)
        {

            var StatusId = _context.MSTATUS.Where(a => a.STATUS_TYPE == "TPRJEVTD" && a.STATUS_SEQ == 4).FirstOrDefault();
            var Employee = _context.MEMPLOYEE.Where(a => a.EMPID == tPRJEVTD.SOURCEID && tPRJEVTD.SOURCEFLD == "EMPID").FirstOrDefault();

            if (StatusId != null)
            {
                var mMEMBERl = _systemcontext.TPRJEVTD.Where(b => b.PEVTDID == tPRJEVTD.PEVTDID)
                               .ExecuteUpdate(up => up.SetProperty(upd => upd.HATD, upd => upd.HATD == null ? 1 : upd.HATD + 1)
                                                      .SetProperty(upd => upd.STATUSID, upd => StatusId.STATUSID)
                                                      .SetProperty(upd => upd.MODBY, upd => tPRJEVTD.MODBY));


                if (mMEMBERl > 0)
                {
                    var prjevtdd = new TPRJEVTDD()
                    {

                        PEVTDID = tPRJEVTD.PEVTDID,
                        ECOMPID = pECOMPID,
                        STATUSID = StatusId.STATUSID,
                        INSDAT = DateTime.Now,
                        INSBY = tPRJEVTD.MODBY

                    };

                    _systemcontext.TPRJEVTDD.Add(prjevtdd);
                    _context.SaveChanges();

                }


                var Msg = _systemcontext.CEVT_COMPONENT.Where(a => a.ECOMPID == pECOMPID && a.PEVTID == tPRJEVTD.PEVTID && a.MSGACTIVE == "Y").FirstOrDefault();

                if (Msg != null)
                {
                    var msms = new SSMS_DATA()
                    {

                        SSID = _general.GetSEQUENCE(nameof(IxmDBSequence.SEQ_SSMSDATA)),
                        CELLNUMBER = "27" + Employee.CELLNUMBER.Substring(Employee.CELLNUMBER.Length - 9, 9),
                        SOURCEOBJECT = "TPRJEVTD",
                        SOURCEID = tPRJEVTD.SOURCEID,
                        INSERT_DATE = DateTime.Now,
                        INSERTED_BY = tPRJEVTD.INSBY
                    };



                    EMAIL_TEMPLATE email_Template = new EMAIL_TEMPLATE();

                    var Msgh = _context.MMSG.Where(a => a.MSGID == Msg.MSGID).FirstOrDefault();

                    //_logger.LogInformation("Data working on PRJEVT : {@model}", tPRJEVTD);
                    //_logger.LogInformation("Data working on EMPLOYEE : {@model}", Employee);
                    //_logger.LogInformation("Data working on MSGH : {@model}", Msgh);
                    //_logger.LogInformation("Data working on SMS : {@model}", msms);

                    if (Msgh != null)
                    {
                        email_Template.Message = Msgh.SMESSAGE;
                        email_Template.Name = Employee.MNAME;
                        email_Template.Surname = Employee.MSURNAME;
                        email_Template.CellNumber = msms.CELLNUMBER;
                        msms.SMESSAGE = Msgh.SMESSAGE;

                        _logger.LogInformation("Attempting to Save SMS : {@model}", msms);
                        await _context.SSMS_DATA.AddAsync(msms);
                        _context.SaveChanges();

                        _logger.LogInformation("Saved");

                        _logger.LogInformation("Sending SMS");
                        await notification.SendSMSAsync(email_Template);

                    }

                    _logger.LogInformation("Thank You for attending : {@model}", tPRJEVTD);


                }

            }


            return 0;
        }


        public async Task<TPROJECTWr> Modify_EvtProject(IIXMNotification notification, TPROJECTWr tPROJECT, int SystemId)
        {

            try
            {
                var StatusId = _systemcontext.MSTATUS.Where(a => a.STATUS_TYPE == "TPROJECT" && a.STATUS_SEQ == 1).FirstOrDefault();
                if (tPROJECT.PSTDAT >= tPROJECT.PENDAT) throw new ArgumentException("Start must be before End");


                if (StatusId != null)
                {
                    var tPROJECTu = _systemcontext.TPROJECT.Where(b => b.PRJID == tPROJECT.PRJID)
                                   .ExecuteUpdate(up => up.SetProperty(upd => upd.PENDAT, upd => upd.PENDAT == null ? DateTime.Now : tPROJECT.PENDAT)
                                                          .SetProperty(upd => upd.DESCRIPTION, tPROJECT.DESCRIPTION)
                                                          .SetProperty(upd => upd.RESPONSIBLEUNIT, tPROJECT.RESPONSIBLEUNIT)
                                                          .SetProperty(upd => upd.SCOPE, tPROJECT.SCOPE)
                                                          .SetProperty(upd => upd.PRJTYPID, tPROJECT.PRJTYPID)
                                                          .SetProperty(upd => upd.LONGITUDE, tPROJECT.LONGITUDE)
                                                          .SetProperty(upd => upd.LATITUDE, tPROJECT.LATITUDE)
                                                          .SetProperty(upd => upd.VENUE, tPROJECT.VENUE)
                                                          .SetProperty(upd => upd.STATUSID, StatusId.STATUSID));


                    if (tPROJECTu == 0)
                    {

                        tPROJECT.PRJID = _general.GetSEQUENCE(nameof(IxmDBSequence.SEQTPROJECT), _systemcontext);
                        var prjevt = new TPROJECTWr()
                        {

                            PRJID = tPROJECT.PRJID,
                            PRJCODE = tPROJECT.PRJCODE,
                            PRJTYPID = tPROJECT.PRJTYPID,
                            EHCPP = tPROJECT.EHCPP,
                            ENOR = tPROJECT.ENOR,
                            DESCRIPTION = tPROJECT.DESCRIPTION,
                            VENUE = tPROJECT.VENUE,
                            LATITUDE = tPROJECT.LATITUDE,
                            LONGITUDE = tPROJECT.LONGITUDE,
                            RESPONSIBLEUNIT = tPROJECT.RESPONSIBLEUNIT,
                            ISACTIVE = tPROJECT.ISACTIVE,
                            SCOPE = tPROJECT.SCOPE,
                            STATUSID = StatusId.STATUSID,
                            INSDAT = DateTime.Now,
                            INSBY = tPROJECT.INSBY,
                            MODAT = DateTime.Now,
                            MODBY = tPROJECT.MODBY

                        };

                        _systemwrcontext.TPROJECT.Add(tPROJECT);
                        var lresult = await _systemwrcontext.SaveChangesAsync();
                        if (lresult >= 0)
                        {

                            _logger.LogInformation("Thank You for creating Event : {@model}", tPROJECT);

                        }


                    }
                    else
                    {
                        return null;
                    }
                    
                }

                return tPROJECT;
            }
            catch (Exception ex)
            {

                _logger.LogError("Error Encountered :: {@model}", ex.Message);
                return null;
            }


        }

        public async Task<TEVENTWr> Modify_EvtEvent(IIXMNotification notification, TEVENTWr tEVENT, int SystemId)
        {

            try
            {
                var StatusId = _systemcontext.MSTATUS.Where(a => a.STATUS_TYPE == "TEVENT" && a.STATUS_SEQ == 1).FirstOrDefault();
                if (tEVENT.PSTDAT >= tEVENT.PENDAT) throw new ArgumentException("Start must be before End");


                if (StatusId != null)
                {
                    var tPROJECTu = _systemcontext.TEVENT.Where(b => b.PRJID == tEVENT.PRJID)
                                   .ExecuteUpdate(up => up.SetProperty(upd => upd.PENDAT, upd => upd.PENDAT == null ? DateTime.Now : tEVENT.PENDAT)
                                                          .SetProperty(upd => upd.DESCRIPTION, tEVENT.DESCRIPTION)
                                                          .SetProperty(upd => upd.LONGITUDE, tEVENT.LONGITUDE)
                                                          .SetProperty(upd => upd.LATITUDE, tEVENT.LATITUDE)
                                                          .SetProperty(upd => upd.LOCATIONAME, tEVENT.LOCATIONAME)
                                                          .SetProperty(upd => upd.STATUSID, StatusId.STATUSID));


                    if (tPROJECTu == 0)
                    {

                        tEVENT.EVTID = _general.GetSEQUENCE(nameof(IxmDBSequence.SEQTEVENT), _systemcontext);
                        var prjevt = new TEVENTWr()
                        {
                            EVTID = tEVENT.EVTID,
                            PRJID = tEVENT.PRJID,
                            EHCPP = tEVENT.EHCPP,
                            ENOR = tEVENT.ENOR,
                            DESCRIPTION = tEVENT.DESCRIPTION,
                            LOCATIONAME = tEVENT.LOCATIONAME,
                            LATITUDE = tEVENT.LATITUDE,
                            LONGITUDE = tEVENT.LONGITUDE,
                            ISACTIVE = tEVENT.ISACTIVE,
                            STATUSID = StatusId.STATUSID,
                            INSDAT = DateTime.Now,
                            INSBY = tEVENT.INSBY,
                            MODAT = DateTime.Now,
                            MODBY = tEVENT.MODBY

                        };

                        _systemwrcontext.TEVENT.Add(tEVENT);
                        var lresult = await _systemwrcontext.SaveChangesAsync();
                        if (lresult >= 0)
                        {

                            _logger.LogInformation("Thank You for creating Event : {@model}", tEVENT);

                        }


                    }
                    else
                    {
                        return null;
                    }

                }

                return tEVENT;
            }
            catch (Exception ex)
            {

                _logger.LogError("Error Encountered :: {@model}", ex.Message);
                return null;
            }


        }

        public async Task<TPRJEVT> Event(IIXMNotification notification, TPRJEVT tPRJEVT, int SystemId)
        {

            var StatusId = _context.MSTATUS.Where(a => a.STATUS_TYPE == "TPRJEVT" && a.STATUS_SEQ == 1).FirstOrDefault();
            if (tPRJEVT.PSTDAT >= tPRJEVT.PENDAT) throw new ArgumentException("Start must be before End");


            if (StatusId != null)
            {
                var mMEMBERl = _systemcontext.TPRJEVT.Where(b => b.PEVTID == tPRJEVT.PEVTID)
                               .ExecuteUpdate(up => up.SetProperty(upd => upd.PENDAT, upd => upd.PENDAT == null ? DateTime.Now : tPRJEVT.PENDAT)
                                                      .SetProperty(upd => upd.STATUSID, StatusId.STATUSID));


                if (mMEMBERl > 0)
                {

                    tPRJEVT.PEVTID = _general.GetSEQUENCE(nameof(IxmDBSequence.SEQTPRJEVT));
                    var prjevt = new TPRJEVT()
                    {

                        PEVTID = tPRJEVT.PEVTID,
                        EHCPP = tPRJEVT.EHCPP,
                        ENOR = tPRJEVT.ENOR,
                        DESCRIPTION = tPRJEVT.DESCRIPTION,
                        STATUSID = StatusId.STATUSID,
                        INSDAT = DateTime.Now,
                        INSBY = tPRJEVT.INSBY

                    };

                    _systemcontext.TPRJEVT.Add(prjevt);
                    _context.SaveChanges();
                    _logger.LogInformation("Thank You for creating Event : {@model}", prjevt);
                    return prjevt;

                }

                else return null;

            }

            else return null;


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
        public async Task<ApplicationUser> GetCurrentUserAsync(HttpContext httpContext)
        {
            ApplicationUser usr = await _usermanager.GetUserAsync(httpContext.User);
            if (usr != null)
            {
                var sysu = _context.MUSER.Where(a => a.AUTHCODE == usr.Id).Select(u => new { u.USERID, u.UNAME}).Single();
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


    }
}
