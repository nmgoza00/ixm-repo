using IXM.Models;
using IXM.Models.Core;
using Microsoft.EntityFrameworkCore;
using IXM.DB;
using Microsoft.Extensions.Logging;
using IXM.GeneralSQL;
using IXM.Common;
using System.Data;
using IXM.DB.Server;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

namespace IXM.Ingest
{
    public class FileIngest : IFileIngest
    {

        //private string _baseurl = App.aAppInfo.BaseAPIURL;
        private string _baseurl = "";
        //private IWebHostEnvironment _environment;
        private IXMDBContext _context;
        private IXMJobDBContext _jobcontext;
        private IXMDBIdentity _identitycontext;
        private readonly IDataImport _dataimport;
        private readonly IIXMCommonRepo _commonrepo;
        private IGeneralSQL _generalSQL;
        private readonly ILogger<FileIngest> _logger;
        private List<MSYSTEMS> mSYSTEMS = new List<MSYSTEMS>();


        public FileIngest(IXMDBContext context, IXMDBIdentity identitycontext, ILogger<FileIngest> logger, IGeneralSQL generalSQL,
                                    IXMJobDBContext jobcontext,
                                    IIXMCommonRepo commonrepo,
                                    IDataImport dataimport)
        {
            //this._environment = environment;
            _context = context;
            _commonrepo = commonrepo;
            _identitycontext = identitycontext;
            _generalSQL = generalSQL;
            _dataimport = dataimport;
            _logger = logger;
            _jobcontext = jobcontext;
           // mSYSTEMS = customData.GetSystems();

        }


        public async Task<MemoryStream> XlsExportRemittanceContentInfo(string Username, string FileName, int PeriodId, int CompanyId, string SystemID)
        {

            try
            {

                var prData = _context.MPERIOD.Where(a => a.PRID == PeriodId).Single();
                var prHeader = _context.MCOMPANY.Where(a => a.COMPANYID == CompanyId).ForRemitInfo().First();

                if (prHeader != null)
                {
                    try
                    {

                        if (FileName != null)
                        {
                            var DataTable = await _dataimport.IxmExportedRemittanceInfo(FileName, "Sheet1", prData.FYEARMONTH, prHeader);
                            if (DataTable != null)
                            {
                                return DataTable;

                            }
                            else return null;
                        }
                        else return null;

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error Encountered :: {@message}", ex.Message);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.LogError("Error Encountered :: {@message}", ex.Message);
                return null;

            }

            return null;
            
     }




        public async Task <List<XLS_REMITTANCE>> XlsRemittanceInjest(string Username, string FileID, string SystemID, char Reload)
        {

            try
            {

                /*var pSystem = _identitycontext.MSYSTEM.Where(a => a.SYSTEMID == SystemID).First();

                if (pSystem == null)
                {
                    _logger.LogError("Requested System Info not found {@system}", SystemID);
                }*/
                try
                {

                    var prData = _context.MOBJECT_DOC.FromSqlRaw<MOBJECT_DOC>(_generalSQL.GetDocumentByObjectId(FileID)).First();
                    var prHeader = _context.TRMBL.Where(a => a.OBJECTID == prData.OBJECTID).FirstOrDefault();

                    if (prHeader != null)
                    {

                        var prDetail = _context.TRMBLD.Where(a => a.RMBLID == prHeader.RMBLID)
                                                      .Where(b => b.LT == 1).ToList();
                        try
                        {
                            var RemittanceProcessed = _context.TCOUNT.FromSqlRaw<TCOUNT>(_generalSQL.CheckIfRemittanceFullyProcessed(prHeader.RMBLID.ToString())).Single();
                            if (RemittanceProcessed.COUNTER > 0)
                            {
                                _logger.LogInformation("This Remittance Number ({@1}) has already been finalised. Cannot performing any reloads.({@2})", RemittanceProcessed.COUNTER, Username);
                                //return null;
                            }
                            else if ((Reload == 'Y') && (prDetail.Count() > 0))
                            {
                                var lVal = _context.TCOUNT.FromSqlRaw<TCOUNT>("SELECT MAX(VERSIONID) COUNTER FROM TRMBLD WHERE RMBLID = " + prHeader.RMBLID.ToString());
                                int lnextVersion = lVal.Single().COUNTER ??= 0 + 1;
                                _context.Database.ExecuteSqlRaw("UPDATE TRMBLD SET VERSIONID = " + lnextVersion + " WHERE RMBLID = " + prHeader.RMBLID + "AND VERSIONID = " + lVal.Single().COUNTER);

                            }

                        }
                        catch (Exception)
                        {

                            _logger.LogError("Remittance Number ({@1}) not yet Processed.", prHeader.RMBLID.ToString());

                        }

                        var lFName = prData.SFOLDERNAME;

                        if ((lFName != null) && (Reload == 'Y'))
                        {
                            // List<XLS_REMITTANCE> DataTable = _dataimport.IxmExcelImporter<XLS_REMITTANCE>(lFName, SystemID).OrderBy(a => a.DTF_IDNUMBER).ToList();
                            List<XLS_REMITTANCE> DataTable = _dataimport.IxmExcelImporter<XLS_REMITTANCE>(lFName, SystemID).ToList();

                            DataTable.ForEach(s => s.RMBLID = prHeader.RMBLID);
                            DataTable.ForEach(s => s.COMPANYID = prHeader.COMPANYID);
                            DataTable.ForEach(s => s.INSBY = Username);


                            _logger.LogInformation("XLS DataTable :: ({@1}), {@2}", DataTable.FirstOrDefault(), lFName);

                            return DataTable;
                        }
                        else { return null; }

                    }
                    else
                    {

                        _logger.LogInformation("There is no Remittance record existing for this Document Number ({@1}) : User {@2} ", prData.OBJECTID.ToString(), Username);
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error Encountered :: {@message}", ex.Message);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Encountered :: {@message}", ex.Message);
                return null;

            }
        }

        public async Task<int> XlsRemittanceToDB(List<XLS_REMITTANCE> DataTable, string lFName)
        {

                    List<TRMBLD> DataImport = new List<TRMBLD>();
                    int lr = 1; int lids = 1;
                    int rmblid = DataTable.First().RMBLID == null ? 0 : DataTable.First().RMBLID.Value;
                    string lIDNumber = "";


            _logger.LogInformation("First Revcord Data :: {@1}", DataTable.FirstOrDefault());
            foreach (var row in DataTable)
                    {
                        if (row.DTF_IDNUMBER != lIDNumber)
                        {
                            lIDNumber = row.DTF_IDNUMBER;
                            lids = 0;
                        }
                        var DT = new TRMBLD()
                        {
                            RMBLID = rmblid,
                            RMBLDID = lr++,
                            DTF_IDSEQ = lids++,
                            VERSIONID = 1,
                            MEMEXISTS = 0,
                            DB_ICONTRIBUTION = row.TRL_IAMOUNT,
                            DTF_MNAME = row.DTF_MNAME,
                            DTF_MSURNAME = row.DTF_MSURNAME,
                            DTF_EMPNUMBER = row.DTF_EMPNUMBER,
                            DTF_IDNUMBER = row.DTF_IDNUMBER,
                            DTF_IDTYPE = row.DTF_IDTYPE,
                            DTF_GENDER = row.DTF_GENDER,
                            DTF_CITYID = row.DTF_CITYID,
                            DTF_COMPANYID = row.COMPANYID.ToString(),
                            DTF_BCOMPANYID = row.DTF_BCOMPANYID,
                            DTF_RCOMPANYID = row.DTF_RCOMPANYID,
                            DTF_ICONTRIBUTION = row.DTF_ICONTRIBUTION,
                            DTF_ECONTRIBUTION = row.DTF_ECONTRIBUTION,
                            DTF_MEMSTATUSID = row.DTF_MEMSTATUSID,
                            DTF_DOB = row.DTF_DOB,
                            DTF_CELLNUMBER = row.DTF_CELLNUMBER,
                            DTF_O1ID = row.DTF_O1ID,
                            DTF_SALARY = row.DTF_SALARY,
                            INSDAT = DateTime.UtcNow,
                            INSBY = row.INSBY,
                            MODAT = DateTime.UtcNow,
                            MODBY = row.INSBY,
                            TRL_IAMOUNT = row.TRL_IAMOUNT,
                            TRL_EAMOUNT = row.TRL_EAMOUNT,
                            TRL_SALARY = row.TRL_SALARY,
                            TRL_BCOMPANYID = row.TRL_BCOMPANYID,
                            TRL_CITYID = row.TRL_CITYID,
                            //Directly push easy Translations (TRL_)
                            TRL_IDTYPEID = row.TRL_IDTYPEID,
                            TRL_GENDER = row.TRL_GENDER == null ? null : row.TRL_GENDER.Substring(0,1),

                            ERRORNUM = row.ERRORNUM,
                            ERRORDESCRIPTION = row.ERRORDESCRIPTION,
                            LT = 1,
                            LTCNT = lids
                        };
                        DataImport.Add(DT);
                    }

                    //var lStat = _general.GetConfigStatus("TRMBL", 3);
                    string lSql = "SELECT MSTATUS.* FROM VMSTATUS MSTATUS WHERE STATUS_TYPE = 'TRMBL' AND STATUS_SEQ = 3 AND ISACTIVE = 'Y'";
                    var prSeq = _context.VMSTATUS.FromSqlRaw<VMSTATUS>(lSql);
                    var lStat = prSeq == null ? -1 : prSeq.First().STATUSID;



                    //_logger.LogInformation("Updating Header Values");
                    var tRMBL = _context.TRMBL.Where(a => a.RMBLID == rmblid).FirstOrDefault();
                    tRMBL.MEMBERS = DataImport.Count;

                    //Not Possible to update at this stage, if sourceing from file.
                    tRMBL.IAMOUNT = DataImport.Sum(a => a.DB_ICONTRIBUTION).Value;

                    //Register Remmittance to DB 'Registered - Remt. File'
                    tRMBL.STATUSID = lStat;
                    _logger.LogInformation("Remittance Header Data TO MODIFY {@Recs}", tRMBL);
                    _context.Entry(tRMBL).State = EntityState.Modified;

                    //IDataServicesServer ldss = new DataServicesServer(null, null, _jobcontext, null);

                    List<TPROCESSTASK> lpt = new List<TPROCESSTASK>
                    {
                        new TPROCESSTASK{ PARAMOBJ = "SCHEDULELOAD", PARAMFLD = "COMPANYID", PARAMVAL = tRMBL.COMPANYID.ToString()},
                        new TPROCESSTASK{ PARAMOBJ = "SCHEDULELOAD", PARAMFLD = "PERIODID", PARAMVAL = tRMBL.PERIODID.ToString()},
                        new TPROCESSTASK{ PARAMOBJ = "SCHEDULELOAD", PARAMFLD = "RMBLID", PARAMVAL = tRMBL.RMBLID.ToString()},
                    };

                    //var processid = await ldss.ProcessQueueAsync(lpt);

                    int batchSize = 50;
                    int total = DataImport.Count;
                    int processed = 0;

                    try
                    {
                        foreach (var batch in DataImport.Chunk(batchSize))
                        {

                            _context.TRMBLD.AddRange(batch);
                            _context.SaveChanges();

                            processed += batch.Count();
                            //Console.WriteLine($"Progress: {processed}/{total}");

                            _logger.LogInformation("Remittance Load Progress {@1} / {@2}", processed, total);

                            // Optionally, clear change tracker to reduce memory
                            _context.ChangeTracker.Clear();

                        }




                    }
                    catch (Exception ex)
                    {

                        _logger.LogError("::: Data Load Error Encountered ::: {@0}", ex);
                        return -1;

                    }


                    //await ldss.ProcessCompleteAsync(processid);


            int recs = _context.SaveChanges();
                    _logger.LogInformation("Data Imported to Data Table for data file {@file}. Records imported {@Recs}", lFName, recs);

            return 0;


        }


        public async Task<int> XlsRemittanceErrorToDB(List<XLS_REMITTANCE> ErrorDataTable, int pRMBLID)
        {
            try
            {

                List<TRMBLE> ErrorDataImport = new List<TRMBLE>();
                int lr = 0;

                var tTRMBLE = _context.TRMBLE.Where(b => b.RMBLID == pRMBLID).ToList();

                int VersionId = tTRMBLE.Count() > 0 ? tTRMBLE.Max(a => a.VERSIONID) : 0;
                

                List<TRMBLE> DataTable = ErrorDataTable.GroupBy(a => a.ERRORNUM)
                                          .Select(b => new TRMBLE
                                          {
                                              RMBLEID = lr++,
                                              OBJECTID = b.Max(c => c.ERROR_OBJECTID),
                                              ERRNUM = (int)b.Key,
                                              ERRCOUNT = b.Count(),
                                              RMBLID = pRMBLID,
                                              VERSIONID = ++VersionId,
                                              ERRNOTE = b.Max(c => c.ERRORDESCRIPTION),
                                              INSDAT = DateTime.UtcNow,
                                              INSBY = b.Max(c => c.INSBY),
                                              MODAT = DateTime.UtcNow,
                                              MODBY = b.Max(c => c.INSBY),
                                              LT = 1

                                          }).ToList()
                                          ;

                _logger.LogInformation("Data extracted from Context {@model}", tTRMBLE);

                tTRMBLE.ForEach(b =>
                {
                    b.LT = 0;
                    _context.Update(b);
                });

                var tRMBL = _context.TRMBL.Where(a => a.RMBLID == pRMBLID).FirstOrDefault();
                tRMBL.MEMBERS = -1;                
                _context.Entry(tRMBL).State = EntityState.Modified;


                _context.TRMBLE.AddRange(DataTable);

                int recs = await _context.SaveChangesAsync();
                _logger.LogInformation("Data Imported to Data Table for data file. Records imported {@Recs}", lr);


            }
            catch (Exception e)
            {

                return -1;
            }
            return 0;


        }

        public async Task <Tuple<int,int>> XlsRemittanceErrorExport(List<XLS_REMITTANCE> DataTable, string FullFileName)
        {
            try
            {
                var DataCaption = _context.TCAPTIONS.FromSqlRaw<TCAPTIONS>("SELECT * FROM APP_OBJECTSETUP WHERE OBJECTNAME = 'XLS_TEMPLATEERROR'").ToList();
                if (DataTable != null)
                {
                    _dataimport.IxmExcelExportDataIssues(DataTable, DataCaption, FullFileName);
                }

            }
            catch (Exception e)
            {

                _logger.LogError("Error Encountered :: {@a1}",e.Message);
            }


            return new Tuple<int,int>(0,0);
        }


        public async Task <MOBJECT_DOC> GetFileDocumentInfo(int pFIleId)
        {

            var tMOBJECT_DOC = _context.MOBJECT_DOC.Where(b => b.OBJECTID == pFIleId).SingleOrDefault();

            return tMOBJECT_DOC;

        }


    }
    }
