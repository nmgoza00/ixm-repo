using IXM.Models;
using IXM.GeneralSQL;
using IXM.DB;
using Microsoft.EntityFrameworkCore;

namespace IXM.DB.Services
{
    public class DataValidator : IDataValidator
    {

        private IGeneralSQL _generalSQL; 
        private IXMDBContext _dbcontext;


        public DataValidator(IGeneralSQL generalSQL,
            IXMDBContext context)
        {
            _generalSQL = generalSQL;
            _dbcontext = context;
        }
        public Tuple<int,int,string> HasRemmittanceLoaded(string pPeriod, string pCompanyId)
        {
            try
            {
                var lSql = _generalSQL.GetPeriodByValue(pPeriod);
                Tuple<int, int, string> myResult;
                //= new(Tuple<int, int, string>);

                var prSeq = _dbcontext.MPERIOD.FromSqlRaw<MPERIOD>(lSql);
                if (prSeq != null)
                {
                    if (prSeq.Count() == 0)
                    {
                        return new Tuple<int, int, string>(-1, -1, "Selected Period {" + pPeriod + "} does not exist");
                    }
                }
                else
                {
                    return new Tuple<int, int, string>(-1, -1, "Selected Period {" + pPeriod + "} does not exist");
                }


                var prSeq1 = _dbcontext.TPAYMENT.FromSqlRaw<TPAYMENT>(_generalSQL.GetPeriodPayment(pCompanyId, prSeq.First().PRID.ToString()));

                if (prSeq1 != null)
                {
                    if (prSeq1.Count() > 0)
                    {
                        return new Tuple<int, int, string>(-2, -2, "Payment Transaction has already been processed for this Company {" + pCompanyId + "} and Period {" + pPeriod + "}");

                    }
                    else if (prSeq1.Count() == 0)
                    {
                        return new Tuple<int, int, string>(0, prSeq.First().PRID, "Not Yet Loaded for this Selected Period {" + pPeriod + "}");
                    }
                }
                else
                {
                    return new Tuple<int, int, string>(-3, -3, "Undetermined Remittance result for this Company {" + pCompanyId + "} and Period {" + pPeriod + "}");
                }

                return new Tuple<int, int, string>(-1, -1, "Selected Period {" + pPeriod + "} does not exist");

            }
            catch (Exception e)
            {

                return new Tuple<int, int, string>(-3, -3, "Error | encountered {" + e.Message );

            }
        }

        public Tuple<int, int, string> HasB2BRemmittanceLoaded(int pPeriod, int pCompanyId)
        {
            try
            {

                var prSeq1 = _dbcontext.TRMBL.Where(a => a.COMPANYID == pCompanyId)
                                             .Where(b => b.PERIODID == pPeriod)
                                             .Select(c => new { RMBLID = c.RMBLID })
                                             .ToList();

                if (prSeq1 != null)
                {
                    if (prSeq1.Count() > 0)
                    {
                        return new Tuple<int, int, string>(-2, -2, "Remmittance Transaction has already been loaded for this Company {" + pCompanyId + "} and Period {" + pPeriod + "}");

                    }
                    else
                    {
                        return new Tuple<int, int, string>(0, 0, "Remmittance has Not Yet Loaded for this Selected Period {" + pPeriod + "}");
                    }
                }
                else
                {
                    return new Tuple<int, int, string>(-3, -3, "Undetermined Remmittance result for this Company {" + pCompanyId + "} and Period {" + pPeriod + "}");
                }

            }
            catch (Exception e)
            {

                return new Tuple<int, int, string>(-4, -4, "Error | encountered {" + e.Message);

            }
        }

    }
}
