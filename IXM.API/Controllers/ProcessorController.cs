using IXM.DB;
using IXM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using FirebirdSql.EntityFrameworkCore;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;
using IXM.Common;
using IXM.API.Services;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata.Ecma335;

namespace IXM.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProcessorController : ControllerBase
    {
        private readonly IXMDBContext _context;
        private readonly IDataService _dataservice;
        private readonly IIXMDBRepo _dbrepo;
        private readonly ILogger <OrganizerController>_logger;

        public ProcessorController( IXMDBContext context, 
                                    ILogger<OrganizerController> logger,
                                    IIXMDBRepo dbrepo,
                                    IDataService dataservice)
        { 
            
            _context = context;
            _logger = logger;
            _dataservice = dataservice;
            _dbrepo = dbrepo;

        }

        [HttpGet]
        public IActionResult Get()
        {

            var prData = _context.MPROCESSOR.FromSqlRaw<MPROCESSOR>($"SELECT DISTINCT MUSER.USERID, MUSER.UNAME, MUSER.NAME, MUSER.SURNAME FROM MUSER " +
                " INNER JOIN CUSER_GROUP ON MUSER.USERID = CUSER_GROUP.USERID " +
                " INNER JOIN CCUR c ON c.SRCID = MUSER.USERID AND c.SRCOBJ = 'MUSER'");
            return prData == null ? NotFound() : Ok(prData);

        }

        [HttpGet("Companies")]
        //[Route("filter")]
        public IActionResult GetOrganizerFilter(string pUsername)
        {
            string _pUsername = pUsername;
            //var prCompany = _context.Database.SqlQuery<MCOMPANY>($"SELECT MCOMPANY.* FROM MCOMPANY INNER JOIN CCUR ON CCUR.SRCID = MCOMPANY.COMPANYID AND CCUR.SRCOBJ = {"MUSER"} INNER JOIN MUSER ON MUSER.USERID = CCUR.USERID AND MUSER.UNAME = {pUsername}");
 
            var prCompany = _context.MCOMPANY_CCUR.FromSqlRaw<MCOMPANY_CCUR>($"SELECT m.*, mld.NAME ||' '||mld.SURNAME PROCESSOR_NAME, " +
                $" TPAYMENTSC.PAYMENTNUM, mp.MYEAR||' - '||mp.MMONTH FYEARMONTH, "+
                $" mld.EMAILADDRESS PROCESSOR_EMAIL, mld.CELLNUMBER PROCESSOR_CELLNUMBER " +
                $" FROM VMCOMPANY m "+
                $" LEFT JOIN CCUR ld ON ld.COMPANYID = m.COMPANYID AND ld.SRCOBJ = 'MUSER'" +
                $" LEFT JOIN(SELECT MAX(PAYMENTID) PAYMENTID, CUSTOMERID FROM TPAYMENTSC GROUP BY CUSTOMERID) tpl "+
                $" ON tpl.CUSTOMERID = m.COMPANYID "+
                $" LEFT JOIN TPAYMENTSC ON TPAYMENTSC.PAYMENTID = tpl.PAYMENTID AND TPAYMENTSC.CUSTOMERID = tpl.CUSTOMERID "+
                $" AND TPAYMENTSC.DETYPE = 'HD' "+
                $" LEFT JOIN MPERIOD mp ON mp.PRID = TPAYMENTSC.PERIODID " +
                $" INNER JOIN MUSER mld ON mld.USERID = ld.USERID  AND mld.UNAME = @p0 ", _pUsername);
            return prCompany == null ? NotFound() : Ok(prCompany);

        }


        [HttpGet("MyWork")]
        public IActionResult SelectMyWork(string pUserId, string pPeriodId)
        {
  
            var pQuery = _context.REP_PROCESSORWORK.FromSqlRaw<REP_PROCESSORWORK>($"SELECT ROW_NUMBER() OVER (ORDER BY VTPAYMENT.PERIODID),MUSER.NAME, MUSER.SURNAME, " +
                         "CASE WHEN VTPAYMENT.PERIODID IS NULL then '' ELSE 'X' END PAYMENT," +
                         "CCUR.COMPANYID, MCOMPANY.CNAME, MCOMPANY.EXT_COCODE, MUSER.PRID PERIODID," +
                         "MCOMPANY.COMPANYNUM,MCOMPTYPE.DESCRIPTION,VTPAYMENT.MYEAR,VTPAYMENT.MMONTH, CCUR.USERID || CCUR.COMPANYID USER_COMPANY," +
                         "CASE WHEN VTPAYMENT.PERIODID IS NULL THEN MUSER.MYEARMONTH ELSE VTPAYMENT.MYEARMONTH END MYEARMONTH, CCUR.userid,VTPAYMENT.PAYMENTID," +
                         "CASE WHEN VTPAYMENT.MYEAR IS NULL THEN 1 ELSE 0 END ISLOADED " +
                         "FROM(SELECT MUSER.*, MPERIOD.PRID, MPERIOD.FYEAR, MPERIOD.MYEARMONTH FROM MUSER, MPERIOD " +
                         "WHERE MPERIOD.MYEAR = CASE WHEN " + pPeriodId + " = -1 THEN MPERIOD.MYEAR ELSE " + pPeriodId + " END " +
                         "AND MPERIOD.FYEAR >= 2021 " +
                         "AND MUSER.USERID = CASE WHEN " + pUserId + " = -1 THEN MUSER.USERID ELSE " + pUserId + " END) MUSER " +
                         "INNER JOIN CCUR " +
                         "ON CCUR.USERID = MUSER.USERID AND CCUR.SRCOBJ = 'MUSER' " +
                         "INNER JOIN MCOMPANY ON MCOMPANY.COMPANYID = CCUR.COMPANYID " +
                         "LEFT JOIN MCOMPTYPE ON MCOMPTYPE.COMPTYPID = MCOMPANY.COMPTYPID " +
                         "LEFT JOIN VTPAYMENT ON VTPAYMENT.CUSTOMERID = MCOMPANY.COMPANYID " +
                         "AND VTPAYMENT.PERIODID = MUSER.PRID WHERE CCUR.COMPANYID = 413");

            return pQuery == null ? NotFound() : Ok(pQuery);

        }

        [AllowAnonymous]
        [HttpGet("Schedules")]
        public IActionResult SchedulesLoadedk(string pUserId, string pCompanyId, string? pYearMonth)
        {

            var pCompany = _dbrepo.Processor.GetProcessorSchedules(pUserId, pCompanyId, pYearMonth);

            return Ok(pCompany);

        }
        
        [AllowAnonymous]
        [HttpGet("PendingRemittances")]
        public IActionResult Remittance(string _Guid, string? CompanyId, string? YearMonth)
        {

            var pRemittances = _dbrepo.Processor.GetPendingRemittances(_Guid, CompanyId, YearMonth);

            return Ok(pRemittances);

        }


        [AllowAnonymous]
        [HttpGet("/api/Department/Schedules")]
        public IActionResult SchedulesByDepartment(string pYear)
        {
            var result = _context.REP_DEPT_LOADEDSCHEDULES.FromSqlRaw<REP_DEPT_LOADEDSCHEDULES>(_dataservice.GetDepartmentLoadedSchedules(pYear));
            return Ok(result);
        }


    }
    }
