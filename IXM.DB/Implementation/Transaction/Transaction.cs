using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using IXM.DB.Services;
using IXM.Models;
using IXM.GeneralSQL;
using IXM.Common;
using IXM.Constants;
using IXM.Models.Core;
using IXM.Common.Constant;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;
using IXM.DB.Finance;



namespace IXM.DB
{
    public class Transaction : ITransaction
    {

        private readonly IXMDBContext _context;
        private readonly IXMWriteDBContext _writecontext;
        private readonly IXMDBIdentity _identitycontext;
        private readonly IConfiguration _configuration;
        private readonly IDataValidator _datavalidator;
        private readonly IIXMDBRepo _general;
        private readonly IIXMCommonRepo _commonRepo;
        private readonly IIXMDocumentRepo _docrepo;
        private readonly IGeneralSQL _generalSQL;
        private readonly ILogger<Transaction> _logger;
        private ILogger<Document> _loggerDoc;
        private readonly IFinance _finance;

        public GenFunctions genFunctions = new GenFunctions();
        //public Finance(IFinance finance) => _finance = finance; // Dependency injection

        public Transaction( IXMDBContext context, 
                            IXMWriteDBContext writecontext,
                            IXMDBIdentity idtcontext, 
                            IConfiguration configuration, 
                            IDataValidator dataValidator,
                            IFinance finance,
                            IIXMDBRepo general,
                            IIXMCommonRepo commonRepo,
                            IIXMDocumentRepo docrepo,
                            IGeneralSQL generalSQL,
                            ILogger<Transaction> logger)
        {
            _context = context;
            _general = general;
            _commonRepo = commonRepo;
            _generalSQL = generalSQL;
            _datavalidator = dataValidator;
            _identitycontext = idtcontext;
            _configuration = configuration;
            _logger = logger;
            _finance = finance;
            _docrepo = docrepo;
            _writecontext = writecontext;
            //_queryRepository = queryRepository;
        }

        public async Task UpdateMembers(string Username, int RMBLID, List<IxmRemittanceMatchTypes> MatchTypes)
        {
            var trmblStatus = _general.General.GetConfigStatus("TRMBL", 3);
            var memberStatus = _general.General.GetConfigStatus("MMEMBER", 4);

            var prHeader = _context.TRMBL.Where(a => a.RMBLID == RMBLID)
                                          .Where(b => b.STATUSID == trmblStatus)
                                          .ToList();
            var prDetail = _context.TRMBLD.Where(a => a.RMBLID == RMBLID)
                                          .Where(b => b.MEMEXISTS == 1)
                                          .Where(b => b.DTF_IDNUMBER != null)
                                          .ToList();
            if (prDetail.Count() > 0)
            {

                try
                {

                    await using (var transaction = await _context.Database.BeginTransactionAsync())
                    {

                        foreach (var member in prDetail)
                        {
                            var mMemmber = _context.MMEMBER.First(a => a.MEMBERID == member.DB_MEMBERID);
                            foreach (var mType in MatchTypes)
                            {
                                if ((mType == IxmRemittanceMatchTypes.MATCH_NAME) && (member.DTF_MNAME != null))
                                {
                                    mMemmber.MNAME = member.DTF_MNAME;
                                }
                                if ((mType == IxmRemittanceMatchTypes.MATCH_SURNAME) && (member.DTF_MSURNAME != null))
                                {
                                    mMemmber.MSURNAME = member.DTF_MSURNAME;
                                }
                                if ((mType == IxmRemittanceMatchTypes.MATCH_GENDER) && (member.TRL_GENDER != null))
                                {
                                    mMemmber.GENDER = member.DTF_GENDER;
                                }
                                if ((mType == IxmRemittanceMatchTypes.MATCH_COMPANYID) && (member.DTF_COMPANYID != null))
                                {
                                    mMemmber.COMPANYID = prHeader.First().COMPANYID;
                                }
                                if ((mType == IxmRemittanceMatchTypes.MATCH_BCOMPANYID) && (member.TRL_BCOMPANYID != null))
                                {
                                    mMemmber.BCOMPANYID = member.TRL_BCOMPANYID;
                                }
                                if ((mType == IxmRemittanceMatchTypes.MATCH_MEMSTATUSID) && (member.TRL_MSTATUSID != null))
                                {
                                    mMemmber.MEMSTATUSID = member.TRL_MSTATUSID;
                                }
                                if ((mType == IxmRemittanceMatchTypes.MATCH_EMPNUMBER) && (member.DTF_EMPNUMBER != null))
                                {
                                    mMemmber.EMPNUMBER = member.DTF_EMPNUMBER;
                                }
                                if ((mType == IxmRemittanceMatchTypes.MATCH_IDNUMBER) && (member.DTF_IDNUMBER != null))
                                {
                                    mMemmber.IDNUMBER = member.DTF_IDNUMBER;
                                }
                                if ((mType == IxmRemittanceMatchTypes.MATCH_CITYID) && (member.TRL_CITYID != null))
                                {
                                    mMemmber.CITYID = member.TRL_CITYID;
                                }

                            }
                            _context.SaveChanges();

                        }

                        transaction.Commit();
                    }

                }
                catch (Exception e)
                {
                }


            }

        }

        public async Task ProcessMembers(ApplicationUser au, int RMBLID)
        {
            var trmblStatus = _general.General.GetConfigStatus("TRMBL", 1);
            var memberStatus = _general.General.GetConfigStatus("MMEMBER", 4);

            var prHeader = _context.TRMBL.Where(a => a.RMBLID == RMBLID)
                                          .Where(b => b.STATUSID == trmblStatus)
                                          .ToList();

            if (prHeader.Count() > 0)
            {


                var prCompany = _context.MCOMPANY.Where(a => a.COMPANYID == prHeader.First().COMPANYID)
                                                 .Select(b => new
                                                 {
                                                     b.COMPANYID,
                                                     b.BCITYID,
                                                     b.RCOMPANYID
                                                 })
                                                 .ToList();

                _logger.LogInformation("Processing Members for Remittance :: {@1}", RMBLID.ToString());
                // Get Definitions
                var toUpdate = new[] { "MemCardReceived", "SALARY" };

                var prDetail = _context.TRMBLD.Where(a => a.RMBLID == RMBLID)
                                              .Where(b => b.MEMEXISTS == 0)
                                              .Where(b => b.DTF_IDNUMBER != null)
                                              .ToList();

                _logger.LogInformation("Members to Process :: {@1}", prDetail.Count().ToString());
                if (prDetail.Count() > 0)
                {
                    MMEMBER memberAdd = new();
                    foreach (var member in prDetail)
                    {

                        var nextMemberID = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQMEMBER));

                        memberAdd = new MMEMBER
                        //memberAdd.Add(new MMEMBER
                        {
                            MEMBERID = nextMemberID,
                            MEMBERNUM = _general.General.GetConfigPrefix("MMEMBER","MEMBERNUM",nextMemberID),
                            MNAME = member.DTF_MNAME,
                            MEMBERTYPE = 1, // 1 = Member, 2 = Potential
                            FILENAME = "REMITTANCE",
                            MSURNAME = member.DTF_MSURNAME,
                            IDNUMBER = member.DTF_IDNUMBER,
                            IDTYPEID = member.TRL_IDTYPEID,
                            EMPNUMBER = member.DTF_EMPNUMBER,
                            SALARY = member.TRL_SALARY,
                            ICONTRIBUTION = member.TRL_IAMOUNT,
                            COMPANYID = prHeader.Single().COMPANYID,
                            CITYID = member.TRL_CITYID == null ? prCompany.Single().BCITYID : member.TRL_CITYID,
                            CELLNUMBER = member.DTF_CELLNUMBER,
                            RMBLDID = member.RMBLDID,
                            INSERT_DATE = DateTime.UtcNow,
                            MODIFIED_DATE = DateTime.UtcNow,
                            INSERTED_BY = au.SYSTEM_UNAME,
                            MODIFIED_BY = au.SYSTEM_UNAME,
                            GENDER = member.DTF_GENDER,
                            MEMSTATUSID = memberStatus,
                            PROSTATUSID = memberStatus,
                            MAGE = member.TRL_MAGE
                        };
                        member.MEMEXISTS = 1;

                        //
                        //_context.MMEMBER.AddRange(memberAdd);
                        await _context.MMEMBER.AddAsync(memberAdd);
                        /*foreach (var pname in toUpdate)
                        {
                            memberAdd.Property(pname).IsModified = false;
                        }*/
                        await _context.SaveChangesAsync();

                        //_logger.LogInformation("Member loaded to DB :: {@1}", memberAdd);

                    }

                    try
                    {

                        _logger.LogInformation("Perform Member load to DB");

                       
                    }
                    catch (Exception e)
                    {
                        await _context.Database.RollbackTransactionAsync();

                        await _general.AppLog.LogTotableI(IxmAppLogType.LogB2B, IxmAppSourceObjects.TRMBL, au.SYSTEM_UNAME, RMBLID.ToString(),
                            "Remittance "+ RMBLID+" : Error Encountered when loading Members to Database :: "+ e.Message+ " - User "+ au.SYSTEM_UNAME, au.SYSTEM_UNAME);
                        _logger.LogError("Remittance {@RemId} : Error Encountered when loading Members to Database :: {@Error} ' - User {@user}", RMBLID, e.Message, au.SYSTEM_UNAME);
                    }



                    }


            } else
            {
                _logger.LogError("Remittance {@RemId} : Not Yet Authorised or not in State to 'Process Members' - User {@user}", RMBLID, au.SYSTEM_UNAME);

            }

            //return await Task.FromResult(0);
        }


        public async Task WrapperUpdateMemberDetails(string Username, int RMBLID, List<IxmRemittanceMatchTypes> MatchTypes)
        {
            _logger.LogTrace("Starting Member Update from Remittance number {@rmblid} - User {@user}", RMBLID, Username);

            await UpdateMembers(Username, RMBLID, MatchTypes);

            _logger.LogTrace("Completed Member Update from Remittance number {@rmblid} - User {@user}", RMBLID, Username);


            //return Task.FromResult(0);
        }

        public List<TPAYMENT_DET> GetPayment(string PaymentGuid)
        {

            var pPaymentDetail = _context.TPAYMENT_DET.FromSqlRaw(_generalSQL.GetPayment(PaymentGuid))
                                          .ToList();
            return pPaymentDetail;

        }

        public List<TPAYMENT> GetPaymentConfirmation(string UserGuidD)
        {

            var pPaymentConfirm = _context.TPAYMENT.FromSqlRaw(_generalSQL.GetPaymentConfirmation(UserGuidD))
                                          .PaymentConfirm()
                                          .ToList();
            return pPaymentConfirm;

        }
        public async Task<int> PaymentConfirmation(string PaymentGuid, string UserName)
        {

            int lConfirmStatus = _general.General.GetConfigStatus("TPAYMENT", 2);
            int lCurrentStatus = _general.General.GetConfigStatus("TPAYMENT", 1);

            var tRMBL = _context.TPAYMENT.Where(a => a.HGUID == PaymentGuid).FirstOrDefault();
            int PeriodId = tRMBL.PERIODID;


            if (tRMBL.PSTATUSID == lCurrentStatus)
            {
                int _JrnlNumber = await _finance.Finance_GeneratePaymentGeneralLedger(tRMBL, UserName);
                tRMBL.PSTATUSID = lConfirmStatus;
                tRMBL.TJRNLID = _JrnlNumber;

                _context.SaveChanges();

                _logger.LogInformation("Performing Post Payment Confirmation Tasks :: {@0}", tRMBL.PAYMENTNUM);
                //string ls = _generalSQL.PostPaymentConfirmExecution();
                var lret = _context.Database.ExecuteSqlRaw(_generalSQL.PostPaymentConfirmExecution(), PeriodId);
                _logger.LogInformation("Performing Post Payment Confirmation Result :: {@0}", lret);

                return 0;

            }
            else
            {
                _logger.LogInformation("Pament Confirmation cannot be processed. Payment not in Status ready for Confirmation. {@0}", tRMBL);
                return 1;
            }



        }

        public async Task PaymentGenerate(int PERIODID, int COMPANYID, int SystemID, int Source, string UserName)
        {
            try
            {
                if (Source == 0) // IXM
                {

                    _logger.LogInformation("Remittance Payment transaction initiated :: User {@0}, Company {@1}", UserName, COMPANYID.ToString());
                    await using (var transaction = await _context.Database.BeginTransactionAsync())
                    {

                        var checkresult = _datavalidator.HasRemmittanceLoaded(PERIODID.ToString(), COMPANYID.ToString());


                        if (checkresult.Item1 != 0)
                        {
                            _logger.LogTrace(checkresult.Item3);
                        }

                        var prHeader = _context.TRMBL.ReadModel().Where(a => a.COMPANYID == COMPANYID)
                                                               .Where(a => a.PERIODID == PERIODID)
                                                               .Single();
                        if (prHeader != null)
                        {
                            var lSql = _generalSQL.GetRemittanceForPaymentDetail(prHeader.RMBLID.ToString());
                            var prDetail = _context.TRMBLD.FromSqlRaw(_generalSQL.GetRemittanceForPaymentDetail(prHeader.RMBLID.ToString()))
                                                          .ForPayment()
                                                          .ToList();
                            if (prDetail.Count() == 0)
                            {
                                _logger.LogError("Remittance Payment transaction :: {@0} - There were no Detail Transactions found to process. Member Count = {1}", prHeader.RMBLNUM, prDetail.Count());
                                return;
                            }

                        TPAYMENT PaymentHeader = PaymentHeaderGenerateFromRemittance(prHeader, UserName);

                        _logger.LogInformation("Remittance Payment transaction loading :: {@0}, {@1}", prHeader, PaymentHeader);

                        if (PaymentHeader != null)
                            {
                                var trmblStatus = _general.General.GetConfigStatus("TRMBL", 3);
                                _logger.LogInformation("Remittance Payment Status :: {@0}", trmblStatus);
                                List<TPAYMENT_DET> PaymentDetails = PaymentDetailGenerateFromRemittance(prDetail, PaymentHeader);


                            _logger.LogInformation("Remittance Payment detail transaction loading :: Header ({@0})", PaymentHeader);
                            _logger.LogInformation("Remittance Payment detail transaction loading :: Detail Payment Items ({@0} , {@1})", PaymentDetails.Count(), prDetail.Count());



                            trmblStatus = _general.General.GetConfigStatus("TRMBL", 5);

                            prHeader.PAYMENTID = PaymentHeader.PAYMENTID;
                            prHeader.STATUSID = trmblStatus;
                            _context.Attach(prHeader);
                            _context.Entry(prHeader).Property("PAYMENTID").IsModified = true;
                            _context.Entry(prHeader).Property("STATUSID").IsModified = true;

                            await _context.AddAsync(PaymentHeader);
                            await _context.AddRangeAsync(PaymentDetails);

                            await _context.SaveChangesAsync();

                            string lpath = _configuration.GetConnectionString("ScripBlockFolder");
                            lpath = lpath + genFunctions.GetBlockScriptName(IxmBlockScriptType.BS_UpdatePostPayment);

                            var lScript = _generalSQL.GetBlockExecution(lpath);
                            lScript = lScript.Replace(":PPAYMENTID", PaymentHeader.PAYMENTID.ToString());

                            _logger.LogInformation("Remittance Post load Cleanup Tasks :: {@0} - File execution {@1}", prHeader.PAYMENTID, lpath);

                            _context.Database.ExecuteSqlRaw("EXECUTE PROCEDURE SP_TPAYMENT_LT_DBVALUES ({0})", prHeader.PAYMENTID);

                            _logger.LogInformation("Remittance Post load Cleanup Tasks :: {@0} - Transfer file to Server {@1}", prHeader.PAYMENTID, lpath);

                            await TransferRemittanceToServer(SystemID.ToString(), prHeader.OBJECTID.ToString(), prHeader.PERIODID, prHeader, prHeader.MODBY, lpath);



                            }

                        }

                        await transaction.CommitAsync();

                    }


                }

            }
            catch (Exception e)
            {

                _logger.LogError("'PaymentGeneration::Error Encountered - {@Message}",e.Message);

            }

            //else { lPeriodId = checkresult.Item2; } //Pass the Period to be loaded.


            //return 0;
        }

        public Task PaymentSimulate(int PERIODID, int COMPANYID)
        {
            return Task.FromResult(0);
        }
        public Task PaymentDelete(int PAYMENTID)
        {
            return Task.FromResult(0);
        }

        public Task InvoiceGenerate(int PERIODID, int COMPANYID)
        {
            return Task.FromResult(0);
        }
        public Task InvoiceSimulate(int PERIODID, int COMPANYID)
        {
            return Task.FromResult(0);
        }
        public Task InvoiceDelete(int INVOICEID)
        {
            return Task.FromResult(0);
        }

        public Task CreditNoteGenerate(int PERIODID, int COMPANYID)
        {
            return Task.FromResult(0);
        }
        public Task CreditNoteSimulate(int PERIODID, int COMPANYID)
        {
            return Task.FromResult(0);
        }
        public Task CreditNoteDelete(int CREDITNOTEID)
        {
            return Task.FromResult(0);
        }

        public List<MMEMBER> GetMemberNotInRemittance(string RMBLID)
        {
            var result = new List<MMEMBER>();
            return result;
        }

        public TPAYMENT PaymentHeaderGenerateFromRemittance(TRMBL value, string pUserName)
        {

            var cp = _context.MCOMPANY.Where(a => a.COMPANYID == value.COMPANYID)
                                      .ForBasicData()
                                      .Single();

            int lp = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQTPAYMENT));

            var payment = new TPAYMENT()
            {
                HGUID = Guid.NewGuid().ToString(),
                PAYMENTID = lp,
                PAYMENTNUM = _general.General.GetConfigPrefix("TPAYMENT","PAYMENTNUM", lp),
                CUSTOMERID = value.COMPANYID,
                CUSTOMER_CODE = cp.COMPANYNUM,
                PERIODID = value.PERIODID,
                PSTATUSID = _general.General.GetConfigStatus("TPAYMENT",1),
                IAMOUNT = value.IAMOUNT,
                IADMINFEE = value.ADMINFEE,
                SOURCEID = value.RMBLID,
                SOURCE_TRANTYPE = "REMITTANCE",
                DEBTORTYPE = "ENTY",
                DEBTORNAME = cp.CNAME,
                STREETNO = cp.STREETNO,
                STREETNAME = cp.STREETNAME,
                BUILDINGNO = cp.BUILDINGNO,
                BUILDINGNAME = cp.BUILDINGNAME,
                POSTALCODE = cp.POSTALCODE,
                DBFACCTID = cp.DBFACCTID,
                CRFACCTID = cp.CRFACCTID,
                POBOXNO = cp.POBOXNO,
                POBOXNAME = cp.BUILDINGNAME,
                POBOXPREFIX = cp.POBOXPREFIX,
                INSERTED_BY = pUserName,
                INSERT_DATE = DateTime.Now,
                MODIFIED_BY = pUserName,
                MODIFIED_DATE = DateTime.Now

            };

            return payment;

        }

        public List<TPAYMENT_DET> PaymentDetailGenerateFromRemittance(List<TRMBLD> value, TPAYMENT header)
        {
            var Result = new List<TPAYMENT_DET>();
            int lr = 1;


            foreach (var payment in value)
            {
                var p1 = new TPAYMENT_DET()
                {
                    PAYMENTID = header.PAYMENTID,
                    PAYMENTDID = lr++,
                    PERIODID = header.PERIODID,
                    MEMBERID = (int)payment.DB_MEMBERID,
                    CUSTOMERID = (int)header.CUSTOMERID,
                    //== null ? payment.DB_COMPANYID : header.CUSTOMERID,
                    CITYID = payment.DB_CITYID,
                    LOCALITYID = payment.DB_LOCALITYID,
                    BCOMPANYID = payment.DB_BCOMPANYID,
                    RMBLDID = (int)payment.RMBLDID,
                    ERAMOUNT = payment.TRL_EAMOUNT,
                    PROVINCEID = payment.DB_PROVINCEID,
                    DESCRIPTION = payment.DTF_MNAME + ", " + payment.DTF_MSURNAME + " - " + payment.DTF_IDNUMBER + " : " + payment.DB_MEMBERID,
                    MEMSTATUSID = payment.DB_MEMSTATUSID,
                    SALARY = payment.TRL_SALARY,
                    IAMOUNT = payment.TRL_IAMOUNT,
                    INSDT = DateTime.Now,
                    INSBY = header.INSERTED_BY,
                    LT = payment.LT,
                    LTAMOUNT = 0,
                    LTCNT = payment.LTCNT

                };

                Result.Add(p1);
            }
            return Result;
        }

        public TRMBL GenerateRemittanceHeader(TRMBL_POST value, string pUserName)
        {

            var cp = _context.MCOMPANY.Where(a => a.COMPANYID == value.COMPANYID)
                                      .ForBasicData()
                                      .Single();

            int lSeq = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQTRMBL));
            if (lSeq < 0)
            {
                //_general.AppLog.LogTotableE(IxmAppLogType.LogB2B, IxmAppSourceObjects.B2BLOADS, model.SYSTEMID, model.SOURCEID.ToString(), "Sequence error encountered for TRMBL.", model.INSERTED_BY);
            }
            string lSeqVal = _general.General.GetConfigPrefix("TRMBL", "RMBLNUM", lSeq);
            int lStatusid = _general.General.GetConfigStatus("TRMBL", 1);
            var dt = new TRMBL()
            {
                RMBLID = lSeq,
                RMBLNUM = lSeqVal,
                COMPANYID = cp.COMPANYID,
                PERIODID = value.PERIODID,
                OBJECTID = value.OBJECTID,
                RSTYPEID = (int)value.RSTYPEID,
                STATUSID = lStatusid,
                IAMOUNT = value.IAMOUNT,                 
                FLOADS = 1,
                MEMBERS = value.MEMBERS,
                INSBY = pUserName,
                INSDAT = DateTime.Now,
                MODBY = pUserName,
                MODAT = DateTime.Now
            };

            _logger.LogInformation("Remmittance - Payment generated :: {@model}", dt);
            //_context.TRMBL.Add(dt);
            //_context.SaveChanges();

            return dt;

        }


        public async Task<int> RemmittanceCaptureGenerate(TRMBL_POST DataHeader, List<TRMBLD> DataTable, string pUsername)
        {

            await using (var transaction = await _writecontext.Database.BeginTransactionAsync())
            {

                try
                {
                    List<TRMBLD_WRITE> _DetailAdd = new List<TRMBLD_WRITE>();
                    List<TRMBLD_WRITE> _DetailUpd = new List<TRMBLD_WRITE>();
                    TRMBL tRMBL = new();
                    int lr = 1; int lids = 1;
                    int rmblid = DataTable.First().RMBLID == null ? 0 : DataHeader.RMBLID;
                    string lIDNumber = "";

                    var _result = _datavalidator.HasB2BRemmittanceLoaded(DataHeader.PERIODID, DataHeader.COMPANYID);
                    if (_result.Item1 == -4)
                    {

                        _logger.LogError("Remmiitance ID Check viz(HasB2BRemmittanceLoaded) could be not be established. Kindly resolve :: {@1}", _result.Item3);
                        return -1;
                    }
                    else if (_result.Item1 == 0)
                    {
                        var _Header = GenerateRemittanceHeader(DataHeader, pUsername);
                        DataHeader.RMBLID = _Header.RMBLID;
                        _context.TRMBL.Add(_Header);
                        _context.SaveChangesAsync();
                        //Retrieve - we will update later
                        tRMBL = _context.TRMBL.Where(a => a.RMBLID == _Header.RMBLID).FirstOrDefault();

                    }
                    else
                    {
                        tRMBL = _context.TRMBL.Where(a => a.COMPANYID == DataHeader.COMPANYID).Where(b => b.PERIODID == DataHeader.PERIODID).FirstOrDefault();
                        if (tRMBL.RMBLID > 0)
                        {
                            tRMBL.OBJECTID = DataHeader.OBJECTID;
                            tRMBL.FLOADS = ++tRMBL.FLOADS;
                            tRMBL.IAMOUNT = DataHeader.IAMOUNT;
                            DataHeader.RMBLID = tRMBL.RMBLID;
                        }

                    }

                    if (DataHeader.RMBLID == -1)
                    {
                        _logger.LogError("Remmiitance ID could be not be established. Kindly resolve :: {@1}", DataHeader);
                        transaction.RollbackAsync(); //transaction.Rollback;
                        return -1;

                    }

                    _logger.LogInformation("First Record Data :: {@1}", DataTable.FirstOrDefault());

                    var lExist = _writecontext.TRMBLD.Where(a => a.RMBLID == DataHeader.RMBLID)
                                   .Where(b => b.VERSIONID == 1)
                                   .AsNoTracking()
                                   .ToList();


                    foreach (var row in DataTable)
                    {
                        if (row.DTF_IDNUMBER != lIDNumber)
                        {
                            lIDNumber = row.DTF_IDNUMBER;
                            lids = 0;
                        }

                        
                        var _Upsert = lExist.Where(a => a.DTF_IDNUMBER == row.DTF_IDNUMBER).ToList();
          

                        if (_Upsert.Count() > 0) { _logger.LogError("Record for possible updating :: {@1}", _Upsert); };

                        if ((_Upsert.Count() > 0) && (row.RMBLDID < 0))
                        {
                            // There might be a need to capture the Same Member more than once in the same Schedule. This may need to be accommodated. At this stage, this rule will prevent this from happening. Once things are robust, this can be altered and added.
                            //For now, this avoids a mass inserts due to an error.
                            _logger.LogError("There is a Data Conflict. Update record exists in DB, but trying to insert new record :: {@1}", row);
                            continue;
                        }

                        //When this is a New Record, we need to assigned a sequencial number value, to allow proper savig to DB.
                        if (row.RMBLDID == -1) { lr++; row.RMBLDID = lr; };


                        var DT = new TRMBLD_WRITE();
                        DT = new TRMBLD_WRITE()
                        {
                            RMBLID = DataHeader.RMBLID,
                            RMBLDID = row.RMBLDID,
                            DTF_IDSEQ = lids++,
                            VERSIONID = 1,
                            MEMEXISTS = row.MEMEXISTS,

                            DTF_MNAME = row.DTF_MNAME,
                            DTF_MSURNAME = row.DTF_MSURNAME,
                            DTF_EMPNUMBER = row.DTF_EMPNUMBER,
                            DTF_IDNUMBER = row.DTF_IDNUMBER,
                            DTF_IDTYPE = row.DTF_IDTYPE,
                            DTF_GENDER = row.DTF_GENDER,
                            DTF_CITYID = row.DTF_CITYID,
                            DTF_COMPANYID = row.DTF_COMPANYID,
                            DTF_BCOMPANYID = row.DTF_BCOMPANYID,
                            DTF_RCOMPANYID = row.DTF_RCOMPANYID,
                            DTF_ECONTRIBUTION = row.DTF_ECONTRIBUTION,
                            DTF_ICONTRIBUTION = row.DTF_ICONTRIBUTION,
                            DTF_MEMSTATUSID = row.DTF_MEMSTATUSID,
                            DTF_DOB = row.DTF_DOB,
                            DTF_CELLNUMBER = row.DTF_CELLNUMBER,
                            DTF_O1ID = row.DTF_O1ID,
                            DTF_SALARY = row.DTF_SALARY,

                            MEMBERID = row.DB_MEMBERID.ToString(),
                            DB_MEMBERID = row.DB_MEMBERID,
                            DB_MNAME = row.DB_MNAME,
                            DB_MSURNAME = row.DB_MSURNAME,
                            DB_EMPNUMBER = row.DB_EMPNUMBER,
                            DB_IDNUMBER = row.DB_IDNUMBER,
                            DB_IDTYPEID = row.DB_IDTYPEID,
                            DB_GENDER = row.DB_GENDER,
                            DB_CITYID = row.DB_CITYID,
                            DB_COMPANYID = row.DB_COMPANYID,
                            DB_BCOMPANYID = row.DB_BCOMPANYID,
                            DB_RCOMPANYID = row.DB_RCOMPANYID,
                            DB_MEMSTATUSID = row.DB_MEMSTATUSID,
                            DB_ICONTRIBUTION = row.DB_ICONTRIBUTION,
                            DB_ECONTRIBUTION = row.DB_ECONTRIBUTION,

                            INSDAT = row.INSDAT,
                            INSBY = row.INSBY,
                            MODAT = DateTime.UtcNow,
                            MODBY = row.MODBY,

                            TRL_EAMOUNT = row.TRL_EAMOUNT,
                            TRL_IAMOUNT = row.TRL_IAMOUNT,
                            TRL_SALARY = row.TRL_SALARY,
                            //Directly push easy Translations (TRL_)
                            TRL_IDTYPEID = row.TRL_IDTYPEID,
                            TRL_GENDER = row.TRL_GENDER,

                            ERRORNUM = row.ERRORNUM,
                            ERRORDESCRIPTION = row.ERRORDESCRIPTION,
                            LT = 1,
                            LTCNT = lids
                        };

                        if (_Upsert.Count() == 0)
                        {
                            _DetailAdd.Add(DT);



                        }
                        else if (_Upsert.Count() > 0)
                        {
                            //lExist.Where(a => a.DTF_IDNUMBER == row.DTF_IDNUMBER).Single() = DT;

                            _DetailUpd.Add(DT);
                            //
                            var ist = _writecontext.TRMBLD.Local.Where(a => a.RMBLDID == DT.RMBLDID).SingleOrDefault();
                            if (ist == null)
                            {

                                _logger.LogInformation("Not tracked...call Updeta and Track...Ready to Save to DB. Individual Member in Transactions {@0}", _DetailUpd.Count());

                                //_writecontext.Entry(lExist.Where(a => a.RMBLDID == DT.RMBLDID).Single()).CurrentValues.SetValues(DT);
                                _writecontext.Update(DT);

                            } else
                            {
                                _logger.LogError("Already tracked...copy across values. Updating Record :: {@1}", DT);
                                _writecontext.Entry(_Upsert.Single()).CurrentValues.SetValues(DT);

                                

                            }
                            _writecontext.SaveChanges();
                            //_writecontext.Entry(DT).State = EntityState.Modified;

                        }

                    }

                    //_logger.LogInformation("First Revcord Data :: {@1}", DataImport.FirstOrDefault());
                    if (_DetailAdd.Count() > 0)
                    {
                        _writecontext.TRMBLD.AddRange(_DetailAdd);
                        _logger.LogInformation("Ready to Save to DB. Members in Transactions {@0}", _DetailAdd.Count());

                        int recs1 = await _writecontext.SaveChangesAsync();

                        _logger.LogInformation("Captured Remmittance Imported to Data Table. Records imported {@Recs}", recs1);
                    } else if (_DetailUpd.Count() > 0)
                    {
                        _logger.LogInformation("Ready to Save to DB. All Members in Transactions {@0}", _DetailUpd.Count());
                        //_writecontext.UpdateRange(_DetailUpd);
                        _writecontext.SaveChangesAsync();
                    }


                    int lMembers = _writecontext.TRMBLD.Where(a => a.RMBLID == DataHeader.RMBLID)
                                                       .Where(b => b.VERSIONID == 1).Count();
                    var lAmount = _writecontext.TRMBLD.Where(a => a.RMBLID == DataHeader.RMBLID)
                                                       .Where(b => b.VERSIONID == 1).Sum(d => d.DB_ICONTRIBUTION);

                    tRMBL.MEMBERS = lMembers;
                    tRMBL.IAMOUNT = lAmount.Value;
                    tRMBL.STATUSID = _general.General.GetConfigStatus("TRMBL", 2);
                    _context.Entry(tRMBL).State = EntityState.Modified;
                    _context.SaveChanges();

                    transaction.Commit();
                    _logger.LogInformation("Logging All Record Updates :: {@0}", _DetailUpd);



                }
                catch (Exception e)
                {

                    _logger.LogInformation("Issue Encountered :: Reference RMBLID - {@Recs}. Error :: {@Err}", DataHeader.RMBLID, e.Message);
                    transaction.RollbackAsync(); //transaction.Rollback;
                }



            }


            //int recs = _writecontext.SaveChanges();

            return 0;


        }


        public async Task <Tuple<int, string>> GenerateRemittanceFromDataFile(MOBJECT_DOC model , string pRMBLID, string pUsername, string pBaseFolder, string lNewFile)
        {
            string lFileObjectId = "";
            var lresult = await _general.FileIngest.XlsRemittanceInjest(model.INSERTED_BY, model.OBJECTID.ToString(), model.SYSTEMID.ToString(), Convert.ToChar('Y'));
            _logger.LogInformation("Result : XLsRemittanceInject :: Records loaded to Memory {@1}", lresult.Count);

            if (lresult != null)
            {
                var DV = await _general.DataServices.GetDataValueMapping();
                _commonRepo.DataImport.DataValueMap = DV;
                var MC = await _general.DataServices.GetBranches();
                _commonRepo.DataImport.Branches = MC;
                var CITY = await _general.DataServices.GetCitites();
                _commonRepo.DataImport.Location = CITY;
                int HasIssues = _commonRepo.DataImport.IxmExcelValidateImport(ref lresult);



                if (HasIssues > 0)
                {

                    int CompanyId = (int)lresult.First().COMPANYID;
                    string bFolder = pBaseFolder;
                    var lFName = "ERR_" + lNewFile;

                    var mDoc = _commonRepo.GenValues.GenDocumentValues(Convert.ToInt32(pRMBLID), IxmAppSourceObjects.TRMBL,
                        IxmAppDocumentType.XLS_REMITTANCEERR, "RMBLID", lFName, bFolder);


                    mDoc.SFOLDERNAME = mDoc.SFOLDERNAME + model.INSERTED_BY + "\\";
                    Directory.CreateDirectory(mDoc.SFOLDERNAME);
                    await _general.FileIngest.XlsRemittanceErrorExport(lresult.Where(a => a.ERRORNUM > 0).ToList(), mDoc.SFOLDERNAME + mDoc.DOCUMENTNAME);
                    var _organizerresult = _docrepo.Document.AddEditDocumentToDB(_logger, mDoc);
                    lFileObjectId = _organizerresult.Item1.ToString();

                    lresult.ForEach(s => s.ERROR_OBJECTID = _organizerresult.Item1);
                    await _general.FileIngest.XlsRemittanceErrorToDB(lresult.Where(a => a.ERRORNUM > 0).ToList(), Convert.ToInt32(pRMBLID));

                    return new Tuple<int, string>(-1, "Remittance has Errors");


                }
                else
                {


                    var tracker = new List<TPROCESSTASK>
                    {
                        new TPROCESSTASK {
                        PARAMOBJ = "REMITLOAD",
                        PARAMFLD = "OBJECTID",
                        PARAMVAL = model.OBJECTID.ToString() },

                        new TPROCESSTASK {
                        PARAMOBJ = "REMITLOAD",
                        PARAMFLD = "RMBLID",
                        PARAMVAL = pRMBLID },

                        new TPROCESSTASK {
                        PARAMOBJ = "REMITLOAD",
                        PARAMFLD = "UNAME",
                        PARAMVAL = pUsername },

                        new TPROCESSTASK {
                        PARAMOBJ = "REMITLOAD",
                        PARAMFLD = "FILEID",
                        PARAMVAL = model.OBJECTID.ToString() },

                        new TPROCESSTASK {
                        PARAMOBJ = "REMITLOAD",
                        PARAMFLD = "SYSTEMID",
                        PARAMVAL = model.SYSTEMID.ToString() },
                    };


                    var lret = await  _general.DataServices.ProcessQueueAsync(tracker);

                    //---Queue the rrquest
                    /// For now - we still need to Upload to the DB in the Same thread until the Background Service works.
                    ///  For now - it will re-import the data file again
                    GenerateRemittanceToDataBase(model, pRMBLID, pUsername, pBaseFolder);


                    await _general.DataServices.ProcessCompleteAsync(lret);


                    return new Tuple<int, string>(0, lret);

                }
            } else return new Tuple<int, string>(2, "No Values Found");
        }

        public async Task<int> GenerateRemittanceToDataBase(MOBJECT_DOC model, string pRMBLID, string pUsername, string pBaseFolder)
        {


            var lresult = await _general.FileIngest.XlsRemittanceInjest(model.INSERTED_BY, model.OBJECTID.ToString(), model.SYSTEMID.ToString(), Convert.ToChar('Y'));

            var DV = await _general.DataServices.GetDataValueMapping();
            _commonRepo.DataImport.DataValueMap = DV;
            var MC = await _general.DataServices.GetBranches();
            _commonRepo.DataImport.Branches = MC;
            var CITY = await _general.DataServices.GetCitites();
            _commonRepo.DataImport.Location = CITY;
            _commonRepo.DataImport.IxmExcelValidateImport(ref lresult);

            _logger.LogInformation("Result : XLsRemittanceInject :: Records loaded to Memory {@1}", lresult.Count);
            _logger.LogInformation("Result : XLsRemittanceInject :: Before sending to DB {@1}", lresult.LastOrDefault());

            var lresult2 = await _general.FileIngest.XlsRemittanceToDB(lresult, pRMBLID);
            if (lresult2 == 0)
            {
                await _general.AppLog.LogTotableI(IxmAppLogType.LogB2B, IxmAppSourceObjects.TRMBL, model.SYSTEMID, pRMBLID, "File Member List registered to DB Server", model.INSERTED_BY);
                await _general.CustomUpdates.RemittanceUpdateDBDetails("0", model.INSERTED_BY, pRMBLID);
                await _general.CustomUpdates.RemittanceMembersUpdate(model.INSERTED_BY, pRMBLID, lresult.Count());
                await _general.AppLog.LogTotableI(IxmAppLogType.LogB2B, IxmAppSourceObjects.TRMBL, model.SYSTEMID, pRMBLID, "Member List details updated", model.INSERTED_BY);

                _logger.LogInformation("File Loaded Successfully : User : {@user}, System : {@system} allocated Remittance ID : {@id}", model.INSERTED_BY, model.SYSTEMID, pRMBLID);

                try
                {
                    ///Get Processor and Business User Email Addresses
                    EMAIL_TEMPLATE email_Template = new EMAIL_TEMPLATE();


                    //                      email_Template.ToEmail = model.email;
                    email_Template.Subject = "IXM : Remmittance Received - Reference Number :: RM0~" + pRMBLID;

                    //                      email_Template.Name = model.name;
                    //                      email_Template.Surname = model.surname;  
                    await _commonRepo.Notification.SendEmailAsync(IXMMailType.RemmittanceReceived, ref email_Template);

                    //                      email_Template.ToEmail = model.email;
                    email_Template.Subject = "IXM : Remmittance Submission - Reference Number :: RM0~" + pRMBLID;

                    //                      email_Template.Name = model.name;
                    //                      email_Template.Surname = model.surname;  
                    await _commonRepo.Notification.SendEmailAsync(IXMMailType.RemmittanceSubmitted, ref email_Template);

                }
                catch (Exception)
                {

                    return 0;
                }

            }

            return 0;

        }


        public async Task<int> TransferRemittanceToServer(string pSystemId, string pOBJECTID, int? pPERIODID, TRMBL pTRMBL, string pUsername, string pBaseFolder)
        {

            try
            {

                CancellationToken stoppingToken = new CancellationToken();

                var prData = _context.MOBJECT_DOC.FromSqlRaw<MOBJECT_DOC>(_generalSQL.GetDocumentByObjectId(pOBJECTID)).SingleOrDefault();

                var prPeriod = _context.MPERIOD.Where(a => a.PRID == pPERIODID)
                                                .Select(x => new
                                                {
                                                    PERIODID = x.PRID,
                                                    PERIOD = string.Concat(x.FYEAR,x.MMONTH)
                                                }).SingleOrDefault();
                var lFName = prData.SFOLDERNAME;

                var prSystem = await _general.MasterData.GetSystem();
                //.Ge_ _context.MPERIOD.Where(a => a.PRID == pPERIODID).Select(b => b.PRID).SingleOrDefault();

                FileStream fileStream = new FileStream(lFName, FileMode.Open, FileAccess.Read);
                string folderPath = Path.GetDirectoryName(lFName);
                var SysName = prSystem.Where(a => a.SYSTEMID == pSystemId).Single();

                string filename = FileNamerForTarget(IxmAppDocumentType.XLS_REMITTANCE, SysName.SYSTEMNAME, pTRMBL.COMPANYID.ToString(), prPeriod.PERIOD.ToString());
                string targetFilepath = "Upload/infinIT/" + SysName.SYSTEMNAME + "/Live/DOC/Schedules/" + prPeriod.PERIOD.ToString();
                ////infinIT//" + SysName.SYSTEMNAME + "//Live//DOC//SCHEDULES//" + prPeriod.PERIOD.ToString();   
                //filename = folderPath + "\\" + filename;

                MOBJECT_DOC prData2 = new MOBJECT_DOC()
                {
                    OBJECTID = -1,
                    SYSTEMID = pSystemId,
                    HIERID = prData.OBJECTID,
                    DOCTYPE = nameof(IxmAppDocumentType.PAYXLS),
                    SOURCEOBJ = nameof(IxmAppSourceObjects.TPAYMENT),
                    SOURCEFLD = "PAYMENTID",
                    SOURCEID = (int)pTRMBL.PAYMENTID,
                    DOCUMENTNAME = filename,
                    SFOLDERNAME = targetFilepath + "/",
                    //DESCRIPTION = "Remittance Schedule for " + SysName.SYSTEMNAME + " Period " + prPeriod.PERIOD.ToString(),
                    FILESIZE = (int)fileStream.Length,
                    FILEXTENSION = ".xlsx",
                    LT = 1,
                    DFE = 1,
                    INSERTED_BY = pUsername,
                    INSERT_DATE = DateTime.Now,
                    UNAME = pUsername
                };

                var lRet = await _docrepo.FileTransfer.UploadAsync(fileStream, filename, targetFilepath, stoppingToken);
                if ( lRet.Item1 == 0)
                {
                    prData2.SFOLDERNAME = lRet.Item2;

                    var lReturn2 = await _docrepo.Document.DocumentTransfer(_loggerDoc, prData2, prData.OBJECTID, SysName.SYSTEMNAME);

                    return 0;

                }


            }
            catch (Exception e)
            {
                ///Log Error
                return -1;
            }
            return 0;

        }


        public string FileNamerForTarget(IxmAppDocumentType appDocumentType, string SystemName, string SourceId, string Period)
        {
            if (IxmAppDocumentType.XLS_REMITTANCE == appDocumentType)
            {
                return "PAYMENT_" + SystemName + "_" + SourceId + "_" + Period + "~1.xlsx";
            }
            else if (IxmAppDocumentType.XLS_INVOICE == appDocumentType)
            {
                return "Invoice";
            }
            else if (IxmAppDocumentType.PDF_STATEMENT == appDocumentType)
            {
                return "Statement";
            }
            else
            {
                return "Document";
            }

        }


    }
}
