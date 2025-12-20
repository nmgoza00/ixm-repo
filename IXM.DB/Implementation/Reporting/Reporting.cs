using IXM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using IXM.GeneralSQL;
using FirebirdSql.Data.FirebirdClient;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office.Word;
using System.Data;
using DevExpress.Emf;
using DevExpress.XtraPrinting;
using DocumentFormat.OpenXml.Drawing.Charts;
using IXM.Common;
using Microsoft.AspNetCore.StaticFiles;
using IXM.Common.Implementation.ExcelExporter;
using Microsoft.AspNetCore.Rewrite;
using System;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.Intrinsics.Arm;
using System.Data.Entity;

namespace IXM.DB
{
    public class Reporting : IReporting
    {

        private readonly IXMDBContext _context;
        private readonly IXMDBIdentity _identitycontext;
        private readonly ILogger<DBTasks> _logger;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly IGeneralSQL _generalsql;

        public Reporting(IXMDBContext context, IXMDBIdentity idtcontext,
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

        public async Task Payment_UpdateLatestPayment(int PeriodIdFrom, int PeriodIdTo, string UserName)
        {
            try
            {

                PeriodIdTo = PeriodIdFrom + 3;
                var lSql = _generalsql.Payment_UpdateLatestPayment(PeriodIdFrom, PeriodIdTo);
                _context.Database.ExecuteSqlRaw(lSql);
                //, PeriodIdFrom, PeriodIdTo);
               await _context.SaveChangesAsync();

            }
            catch (Exception E)
            {

                _logger.LogError("Error encountered :: {@err}",E.Message);
            }

        }

        public async Task<List<LIST_MONTHLYREPORTS>> ListMonthlyReports()
        {
            var lSql = "SELECT GEN_UUID() GUID, NULL SYSTEMNAME, CALCPERIOD PERIOD, CALCPERIODID PERIODID, MAX(RUNDATE) RUNDATE,'Finance Monthly Report' REPORTNAME, NULL FILENAME, NULL RESULTTYPE, COUNT(1) MEMBERS ,COUNT(DISTINCT COMPANYID) COMPANIES, SUM(CASE WHEN GENDER='M' THEN 1 ELSE 0 END) GENDER_MALE, SUM(CASE WHEN GENDER='F' THEN 1 ELSE 0 END) GENDER_FEMALE, SUM(CASE WHEN GENDER not in ('M','F') THEN 1 ELSE 0 END) GENDER_UNKNOWN FROM REP_MONTHLYREPORT GROUP BY CALCPERIOD, CALCPERIODID";

            var result = _context.LIST_MONTHLYREPORTS.FromSqlRaw<LIST_MONTHLYREPORTS>(lSql).ToList();
            return result;
        }
        public async Task<List<REP_FINANCE_MONTHLY>>FinanceReport_GenerateMonthly(int PeriodIdFrom, int PeriodIdTo, string UserName)
        {


            List<REP_FINANCE_MONTHLY> records = new List<REP_FINANCE_MONTHLY>();

            string lSql = _generalsql.FinanceReport_GenerateMonthly(PeriodIdFrom, PeriodIdTo);
            _logger.LogInformation("rEP sql :: {@0}", lSql);

            FbConnection _connection = new FbConnection(_context.Database.GetConnectionString());
            _connection.Open();

            await using (var transaction = await _connection.BeginTransactionAsync())
            {

                try
                {
                    var command = new FbCommand(lSql, _connection);
                    command.Transaction = transaction;
                    //command.Parameters.AddWithValue("@FPERIODID", PeriodIdFrom);
                    //command.Parameters.AddWithValue("@TPERIODID", PeriodIdTo);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        REP_FINANCE_MONTHLY record = new REP_FINANCE_MONTHLY
                        {      
                            COMPANYID = reader.xmGetOrdInteger("COMPANYID"),
                            COMPANYNAME = reader.xmGetOrdString("COMPANYNAME"),
                            MNAME = reader.xmGetOrdString("MNAME"),
                            MSURNAME = reader.xmGetOrdString("MSURNAME"),
                            GENDER = reader.xmGetOrdString("GENDER"),
                            BCOMPANYID = reader.xmGetOrdInteger("BCOMPANYID"),
                            LOCALITYID = reader.xmGetOrdInteger("LOCALITYID"),
                            CITYID = reader.xmGetOrdString( "CITYID"),
                            PAYMENTID = reader.xmGetOrdInteger("PAYMENTID"),
                            PERIODID = reader.xmGetOrdInteger("PERIODID"),
                            MEMBERID = reader.xmGetOrdInteger("MEMBERID"),
                            NOFPAYMENTS = reader.xmGetOrdInteger("NOFPAYMENTS"),
                            NOOFENTRIES = reader.xmGetOrdInteger("NOOFENTRIES"),
                            COMPANYNUM = reader.xmGetOrdString("COMPANYNUM"),
                            BCOMPANYNAME = reader.xmGetOrdString("BCOMPANYNAME"),
                            SECTORNAME = reader.xmGetOrdString("SECTORNAME"),
                            REGIONAME = reader.xmGetOrdString("REGIONAME"),
                            MAGE = reader.xmGetOrdInteger("MAGE"),
                            AGEGROUPING = reader.xmGetOrdString("AGEGROUPING"),
                            IDNUMBER = reader.xmGetOrdString("IDNUMBER"),
                            //EMPNUMBER = reader.GetString(reader.GetOrdinal("EMPNUMBER")),
                            MYEARMONTH = reader.xmGetOrdString("MYEARMONTH"),
                            IAMOUNT = reader.xmGetOrdDouble("IAMOUNT"),
                            LTAMOUNT = reader.xmGetOrdDouble("LTAMOUNT"),
                            LT = reader.xmGetOrdInteger("LT"),
                            IDVALID = reader.xmGetOrdString("IDVALID"),

                            // Map other fields
                        };
                        records.Add(record);
                    }

                    await transaction.CommitAsync();
                    return records;
                    //var adapter = new FbDataAdapter(command.CommandText, _connection);
                    //var datatable = new DataTable();
                    //adapter.Fill(datatable);
                    //IEnumerable<REP_FINANCE_MONTHLY> empDetails = _context.Database. .SqlQuery<REP_FINANCE_MONTHLY>("lSql").ToList();
                    //IEnumerable<REP_FINANCE_MONTHLY> empDetails = _context.Translate<REP_FINANCE_MONTHLY>(reader).ToList();
                    // _context.Database.ExecuteSqlRaw(lSql);
                    //_context.SaveChangesAsync();

                }
                catch (Exception E)
                {

                    _logger.LogError("Error encountered :: {@err}", E.Message);
                    transaction.RollbackAsync(); //transaction.Rollback;
                    return null;
                }
            }

        }


        public async Task<List<REP_FINANCE_MONTHLY>> FinanceReport_FromTable(int PeriodIdTo, string UserName)
        {


            List<REP_FINANCE_MONTHLY> records = new List<REP_FINANCE_MONTHLY>();

            string lSql = "SELECT * FROM REP_MONTHLYREPORT WHERE CALCPERIODID = " + PeriodIdTo.ToString() + " AND LT = 1";
            _logger.LogInformation("rEP sql :: {@0}", lSql);

            FbConnection _connection = new FbConnection(_context.Database.GetConnectionString());
            _connection.Open();

            await using (var transaction = await _connection.BeginTransactionAsync())
            {

                try
                {
                    var command = new FbCommand(lSql, _connection);
                    command.Transaction = transaction;
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        REP_FINANCE_MONTHLY record = new REP_FINANCE_MONTHLY
                        {
                            COMPANYID = reader.xmGetOrdInteger("COMPANYID"),
                            COMPANYNAME = reader.xmGetOrdString("COMPANYNAME"),
                            MNAME = reader.xmGetOrdString("MNAME"),
                            MSURNAME = reader.xmGetOrdString("MSURNAME"),
                            GENDER = reader.xmGetOrdString("GENDER"),
                            BCOMPANYID = reader.xmGetOrdInteger("BCOMPANYID"),
                            LOCALITYID = reader.xmGetOrdInteger("LOCALITYID"),
                            CITYID = reader.xmGetOrdString("CITYID"),
                            PAYMENTID = reader.xmGetOrdInteger("PAYMENTID"),
                            PERIODID = reader.xmGetOrdInteger("PERIODID"),
                            MEMBERID = reader.xmGetOrdInteger("MEMBERID"),
                            NOFPAYMENTS = reader.xmGetOrdInteger("NOOFPAYMENTS"),
                            NOOFENTRIES = reader.xmGetOrdInteger("NOOFENTRIES"),
                            COMPANYNUM = reader.xmGetOrdString("COMPANYNUM"),
                            BCOMPANYNAME = reader.xmGetOrdString("BCOMPANYNAME"),
                            SECTORNAME = reader.xmGetOrdString("SECTORNAME"),
                            REGIONAME = reader.xmGetOrdString("REGIONAME"),
                            MAGE = reader.xmGetOrdInteger("MAGE"),
                            AGEGROUPING = reader.xmGetOrdString("AGEGROUPING"),
                            IDNUMBER = reader.xmGetOrdString("IDNUMBER"),
                            //EMPNUMBER = reader.GetString(reader.GetOrdinal("EMPNUMBER")),
                            MYEARMONTH = reader.xmGetOrdString("MYEARMONTH"),
                            IAMOUNT = reader.xmGetOrdDouble("IAMOUNT"),
                            LTAMOUNT = reader.xmGetOrdDouble("LTAMOUNT"),
                            LT = reader.xmGetOrdInteger("LT"),
                            IDVALID = reader.xmGetOrdString("IDVALID"),
                            CALCPERIOD = reader.xmGetOrdString("CALCPERIOD"),
                            CALCPERIODID = reader.xmGetOrdInteger("CALCPERIODID"),
                            RUNDATE = reader.xmGetOrdDateTime("RUNDATE"),

                            // Map other fields
                        };
                        records.Add(record);
                    }

                    await transaction.CommitAsync();
                    return records;

                }
                catch (Exception E)
                {

                    _logger.LogError("Error encountered :: {@err}", E.Message);
                    transaction.RollbackAsync(); //transaction.Rollback;
                    return null;
                }
            }

        }


        public async Task<int> FinanceReport_GenerateMonthlyToTable(int PeriodIdTo, string UserName)
        {

            List<REP_FINANCE_MONTHLY> records = new List<REP_FINANCE_MONTHLY>();

            string lSql = _generalsql.FinanceReport_GenerateMonthlyToTable(PeriodIdTo);
            _logger.LogInformation("Gen Monthly Rep to Table :: {@0}", lSql);
            _context.Database.ExecuteSqlRaw(lSql);
            await _context.SaveChangesAsync();

            return 0;

        }


        public async Task<int>ExportReportToFile(System.Data.DataTable DataValues, string PhysicalFileName, string SheetName)
        {

            //Stream fileStream;
            /* MemoryStream fileStream = new MemoryStream();

                 var fileResult = await _dbrepo.FileIngest.XlsExportRemittanceContentInfo(usr.SYSTEM_UNAME, PhysicalFileName, PeriodId, CompanyId, SystemId);
                 if (fileResult != null)
                 {
                     //var fileS = new MemoryStream(fileResult.GetBuffer(), 0, (int)fileResult.Length);
                     fileResult.CopyTo(fileStream);
                 }


             else
             {
                 var fileS = new FileStream(PhysicalFileName, FileMode.Open, FileAccess.Read);
                 fileS.CopyTo(fileStream);

             }



             new FileExtensionContentTypeProvider().TryGetContentType(PhysicalFileName, out contentType);
             var _oimage = _commonrepo.FileService.GetDocumentData(PhysicalFileName);

             _logger.LogInformation("File Status : {@0}", _oimage);

             if (_oimage.Item1 == -1)
             {

                 _logger.LogInformation("File not found : {@0}", PhysicalFileName);
                 return NotFound("File Not Found 1 " + PhysicalFileName);

             }
             else if (_oimage.Item1 == 0)
             {

                 _logger.LogInformation("File found 0 : {@0}", PhysicalFileName);
                 //return new FileStreamResult(fileStream, contentType);
                 return File(fileStream.ToArray(), contentType);
             }
             */
            DataSet newD = new DataSet();
            newD.Tables.Add(DataValues);
            var exporter = new IxmExcelExporter();
            exporter.ExportDataSetToExcel(newD, PhysicalFileName, SheetName);

            return 1;

        }


    }
}
