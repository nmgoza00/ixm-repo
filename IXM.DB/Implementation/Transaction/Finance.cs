using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using IXM.DB.Services;
using IXM.Models;
using IXM.GeneralSQL;
using IXM.Constants;
using IXM.Common.Constant;



namespace IXM.DB.Finance
{
    public class Finance : IFinance
    {

        private readonly IXMDBContext _context;
        private readonly IXMWriteDBContext _writecontext;
        private readonly IXMDBIdentity _identitycontext;
        private readonly IConfiguration _configuration;
        private readonly IDataValidator _datavalidator;
        private readonly IIXMDBRepo _general;
        private readonly IGeneralSQL _generalSQL;
        private readonly ILogger<Transaction> _logger;

        public GenFunctions genFunctions = new GenFunctions();

        public Finance( IXMDBContext context, 
                            IXMWriteDBContext writecontext,
                            IXMDBIdentity idtcontext, 
                            IConfiguration configuration, 
                            IDataValidator dataValidator,
                            IIXMDBRepo general, 
                            IGeneralSQL generalSQL,
                            ILogger<Transaction> logger)
        {
            _context = context;
            _general = general;
            _generalSQL = generalSQL;
            _datavalidator = dataValidator;
            _identitycontext = idtcontext;
            _configuration = configuration;
            _logger = logger;
            _writecontext = writecontext;
        }


        public async Task<int> Finance_GeneratePaymentGeneralLedger(TPAYMENT pPAYMENT, string UserName)
        {

            if (pPAYMENT != null)
            {
                var nextTTJRN = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQTTJRN));

                var memberAdd = new TTJRN
                {
                    TJRNLID = nextTTJRN,
                    TJRNLNUM = _general.General.GetConfigPrefix("TTJRN", "TJRNLNUM", nextTTJRN),
                    TJRNLTYP = "PA",
                    REFERENCEKEY = pPAYMENT.PAYMENTNUM,
                    PERIODID = pPAYMENT.PERIODID,
                    TJSTATUSID = 99, // Get Reversal Status from Config
                    DESCRIPTION = "Membership Subscription for " + pPAYMENT.PAYMENTNUM,
                    INSDT = DateTime.UtcNow,
                    INSBY = UserName,
                    HGUID = Guid.NewGuid().ToString()

                };


                await _context.TTJRN.AddAsync(memberAdd);

                var nextTTRAN1 = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQTRANID));

                List<TTRAN> memberAdd1 =

                [
                new(){
                    TJRNLID = nextTTJRN,
                    TRANID = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQTRANID)),
                    TRANTYPE = "DB",
                    TAMOUNT = pPAYMENT.IAMOUNT,
                    CUSTOMERID = pPAYMENT.CUSTOMERID,
                    //MEMBERID = null,
                    PRODUCTID = 1,
                    LINENUM = 1,
                    PERIODID = pPAYMENT.PERIODID,
                    DESCRIPTION = "",
                    INSERT_DATE = DateTime.UtcNow,
                    INSERTED_BY = UserName,
                    FACCNO = "010400",  //Accounts Receivable

                },
                new() {
                    TJRNLID = nextTTJRN,
                    TRANID = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQTRANID)),
                    TRANTYPE = "CR",
                    TAMOUNT = pPAYMENT.IAMOUNT,
                    CUSTOMERID = pPAYMENT.CUSTOMERID,
                    //MEMBERID = null,
                    PRODUCTID = 1,
                    LINENUM = 2,
                    PERIODID = pPAYMENT.PERIODID,
                    DESCRIPTION = "",
                    INSERT_DATE = DateTime.UtcNow,
                    INSERTED_BY = UserName,
                    FACCNO = "400100", // Membership Subscription

                }];

                await _context.TTRAN.AddRangeAsync(memberAdd1);
                return nextTTJRN;
            }
            else return -1;

        }

        public async Task<int> Finance_ReversePaymentGeneralLedger(TTJRN pTJRN, string UserName)
        {

            if (pTJRN == null)
            {
                return -1;

            }

            var tTPAYMENT = _context.TPAYMENT.Where(a => a.TJRNLID == pTJRN.TJRNLID).AsNoTracking().FirstOrDefault();

            if (pTJRN == null)
            {
                return -2;

            }

            var nextTTJRN = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQTTJRN));

            var memberAdd = new TTJRN
            {
                    TJRNLID = nextTTJRN,
                    TJRNLNUM = _general.General.GetConfigPrefix("TTJRN", "TJRNLNUM", nextTTJRN),
                    TJRNLTYP = "PA",
                    REFERENCEKEY = pTJRN.REFERENCEKEY,
                    PERIODID = pTJRN.PERIODID,
                    DESCRIPTION = "Membership Subscription Reversal for " + tTPAYMENT.PAYMENTNUM,
                    TJSTATUSID = 99, // Get Reversal Status from Config
                    PTJRNLID = pTJRN.TJRNLID,
                    INSDT = DateTime.UtcNow,
                    INSBY = UserName,
                    HGUID = Guid.NewGuid().ToString()

            };

            _logger.LogInformation("Finance Journal Item to Add to DB ::{@1}", memberAdd);
            await _context.TTJRN.AddAsync(memberAdd);

                var nextTTRAN1 = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQTRANID));

                List<TTRAN> memberAdd1 =

                [
                new(){
                    TJRNLID = nextTTJRN,
                    TRANID = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQTRANID)),
                    TRANTYPE = "DB",
                    TAMOUNT = tTPAYMENT.IAMOUNT,
                    CUSTOMERID = tTPAYMENT.CUSTOMERID,
                    //MEMBERID = null,
                    PRODUCTID = 1,
                    LINENUM = 1,
                    PERIODID = tTPAYMENT.PERIODID,
                    DESCRIPTION = "",
                    INSERT_DATE = DateTime.UtcNow,
                    INSERTED_BY = UserName,
                    FACCNO = "400100", // Membership Subscription

                },
                new() {
                    TJRNLID = nextTTJRN,
                    TRANID = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQTRANID)),
                    TRANTYPE = "CR",
                    TAMOUNT = tTPAYMENT.IAMOUNT,
                    CUSTOMERID = tTPAYMENT.CUSTOMERID,
                    //MEMBERID = null,
                    PRODUCTID = 1,
                    LINENUM = 2,
                    PERIODID = tTPAYMENT.PERIODID,
                    DESCRIPTION = "",
                    INSERT_DATE = DateTime.UtcNow,
                    INSERTED_BY = UserName,
                    FACCNO = "010400",  //Accounts Receivable

                }];

            //
            //_context.MMEMBER.AddRange(memberAdd);

            _logger.LogInformation("Finance Transaction Item to Add to DB ::{@1}", memberAdd1);
            await _context.TTRAN.AddRangeAsync(memberAdd1);

                return nextTTJRN;
                //await _context.SaveChangesAsync();

        }

        public async Task<int> Finance_GenerateInvoiceGeneralLedger(TINVOICE pINVOICE, string UserName)
        {
            if (pINVOICE != null)
            {
                var nextTTJRN = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQTTJRN));

                var memberAdd = new TTJRN
                {
                    TJRNLID = nextTTJRN,
                    TJRNLNUM = _general.General.GetConfigPrefix("TTJRN", "TJRNLNUM", nextTTJRN),
                    TJRNLTYP = "IV",
                    REFERENCEKEY = pINVOICE.INVOICENUM,
                    PERIODID = pINVOICE.PERIODID,
                    TJSTATUSID = 99, // Get Reversal Status from Config
                    DESCRIPTION = "Membership Invoice for " + pINVOICE.INVOICENUM,
                    INSDT = DateTime.UtcNow,
                    INSBY = UserName,
                    HGUID = Guid.NewGuid().ToString()

                };


                await _context.TTJRN.AddAsync(memberAdd);

                var nextTTRAN1 = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQTRANID));

                List<TTRAN> memberAdd1 =

                [
                new(){
                    TJRNLID = nextTTJRN,
                    TRANID = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQTRANID)),
                    TRANTYPE = "DB",
                    TAMOUNT = pINVOICE.IAMOUNT,
                    CUSTOMERID = pINVOICE.CUSTOMERID,
                    //MEMBERID = null,
                    PRODUCTID = 1,
                    LINENUM = 1,
                    PERIODID = pINVOICE.PERIODID,
                    DESCRIPTION = "",
                    INSERT_DATE = DateTime.UtcNow,
                    INSERTED_BY = UserName,
                    FACCNO = "010400",  //Accounts Receivable

                },
                new() {
                    TJRNLID = nextTTJRN,
                    TRANID = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQTRANID)),
                    TRANTYPE = "CR",
                    TAMOUNT = pINVOICE.IAMOUNT,
                    CUSTOMERID = pINVOICE.CUSTOMERID,
                    //MEMBERID = null,
                    PRODUCTID = 1,
                    LINENUM = 2,
                    PERIODID = pINVOICE.PERIODID,
                    DESCRIPTION = "",
                    INSERT_DATE = DateTime.UtcNow,
                    INSERTED_BY = UserName,
                    FACCNO = "400100", // Membership Subscription

                }];

                return nextTTJRN;
            }
            else return -1;

        }
        public async Task<int> Finance_ReverseInvoiceGeneralLedgerr(TTJRN pTJRN, string UserName)
        {
            if (pTJRN == null)
            {
                return -1;

            }

            var tTINVOICE = _context.TINVOICE.Where(a => a.TJRNLID == pTJRN.TJRNLID).AsNoTracking().FirstOrDefault();

            if (pTJRN == null)
            {
                return -2;

            }

            var nextTTJRN = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQTTJRN));

            var memberAdd = new TTJRN
            {
                TJRNLID = nextTTJRN,
                TJRNLNUM = _general.General.GetConfigPrefix("TTJRN", "TJRNLNUM", nextTTJRN),
                TJRNLTYP = "IV",
                REFERENCEKEY = pTJRN.REFERENCEKEY,
                PERIODID = pTJRN.PERIODID,
                DESCRIPTION = "Membership Invoice Reversal for " + tTINVOICE.INVOICENUM,
                TJSTATUSID = 99, // Get Reversal Status from Config
                PTJRNLID = pTJRN.TJRNLID,
                INSDT = DateTime.UtcNow,
                INSBY = UserName,
                HGUID = Guid.NewGuid().ToString()

            };


            await _context.TTJRN.AddAsync(memberAdd);

            var nextTTRAN1 = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQTRANID));

            List<TTRAN> memberAdd1 =

            [
            new(){
                    TJRNLID = nextTTJRN,
                    TRANID = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQTRANID)),
                    TRANTYPE = "DB",
                    TAMOUNT = tTINVOICE.IAMOUNT,
                    CUSTOMERID = tTINVOICE.CUSTOMERID,
                    //MEMBERID = null,
                    PRODUCTID = 1,
                    LINENUM = 1,
                    PERIODID = tTINVOICE.PERIODID,
                    DESCRIPTION = "",
                    INSERT_DATE = DateTime.UtcNow,
                    INSERTED_BY = UserName,
                    FACCNO = "400100", // Membership Subscription

                },
                new() {
                    TJRNLID = nextTTJRN,
                    TRANID = _general.General.GetSEQUENCE(nameof(IxmDBSequence.SEQTRANID)),
                    TRANTYPE = "CR",
                    TAMOUNT = tTINVOICE.IAMOUNT,
                    CUSTOMERID = tTINVOICE.CUSTOMERID,
                    //MEMBERID = null,
                    PRODUCTID = 1,
                    LINENUM = 2,
                    PERIODID = tTINVOICE.PERIODID,
                    DESCRIPTION = "",
                    INSERT_DATE = DateTime.UtcNow,
                    INSERTED_BY = UserName,
                    FACCNO = "010400",  //Accounts Receivable

                }];

            //
            //_context.MMEMBER.AddRange(memberAdd);
            await _context.TTRAN.AddRangeAsync(memberAdd1);

            return nextTTJRN;
            //await _context.SaveChangesAsync();

        }


    }
}
