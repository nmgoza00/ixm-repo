using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using IXM.Common;
using IXM.Common.Constant;
using IXM.Common.Generics;
using IXM.Common.Implementation.ExcelExporter;
using IXM.DB.Services;
using IXM.GeneralSQL;
using IXM.Models;
using IXM.Models.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Entity;
using Microsoft.AspNetCore.OData.Query;
using static IXM.Common.Generics.Generics4API;

namespace IXM.DB
{
    public class Company : ICompany
    {

        private readonly IXMDBContext _dbcontext;
        private readonly IXMDBIdentity _identitycontext;
        private readonly IXMDBContextFactory _dbfactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<Member> _logger;
        private readonly IGeneralSQL _generalSQL;

        public GenFunctions genFunctions = new GenFunctions();

        public Company( 
                            IXMDBIdentity idtcontext,
                            IXMDBContext dbcontext,
                            IXMDBContextFactory dbfactory,
                            IConfiguration configuration,
                            IGeneralSQL generalSQL,
                            ILogger<Member> logger)
        {
            _dbcontext = dbcontext;
            _identitycontext = idtcontext;
            _configuration = configuration;
            _logger = logger;
            _generalSQL = generalSQL;
            _dbfactory = dbfactory;
        }


        public List<MCOMPANY> GetCompanyList(ApplicationUser au, string _Guid)
        {
            try
            {

                //IQueryable<MCOMPANY> prCompany = _dbcontext.MCOMPANY.FromSqlRaw<MCOMPANY>(_generalSQL.GetCompaniesList(_Guid)).AsQueryable();
                //IQueryable<MCOMPANY> prCompany = _dbcontext.MCOMPANY.AsQueryable();
                //var result = await Generics4API.PageList<MCOMPANY>.CreateAsync(prCompany, PAGENO, PAGESIZE);

                //List<MCOMPANY> prCompany = _dbcontext.MCOMPANY.FromSqlRaw<MCOMPANY>(_generalSQL.GetCompaniesList(_Guid)).Skip((PAGENO -1) * PAGESIZE).ToList();
                List<MCOMPANY> prCompany = _dbcontext.MCOMPANY.FromSqlRaw<MCOMPANY>(_generalSQL.GetCompaniesList(_Guid)).ToList();


                //query.Skip((page - 1) * pagesize).Take(pagesize).ToListAsync();
                return prCompany;
                //.Skip((PAGENO - 1) * PAGESIZE).ToList();
            }
            catch (Exception)
            {

                return null;
            }

        }

        public async Task <MCOMPANY> GetCompanyBySystem(ApplicationUser au, string System, string Company)
        {
            try
            {

                var singlesys = _identitycontext.MSYSTEM.Where(a => a.SYSTEMID == System).FirstOrDefault();

                var _ctx = _dbfactory.Create("Firebird" + singlesys.SYSTEMNAME);

                _logger.LogInformation("DB Factory Connected - Processing values for System {@1} - DB {@musr}", singlesys.SYSTEMNAME, _ctx.Database);

                var prCompany = _ctx.MCOMPANY.FromSqlRaw<MCOMPANY>(_generalSQL.GetCompanyById(Company)).SingleOrDefault();
                return prCompany;
            }
            catch (Exception e)
            {

                _logger.LogError("DB Factory Error - {@1} ", e.Message);
                return null;
            }

        }

       

        public List<MMEMBER_READ> GetCompanyMembersByGuid(ApplicationUser au, string _Guid)
        {
            try
            {
                var _user = _dbcontext.MCOMPANY.ForBasicData().Where(a => a.HGUID == _Guid).First();

                if (_user.COMPANYID != null)
                {
                    var prMembers = _dbcontext.MMEMBER_READ.FromSqlRaw<MMEMBER_READ>(_generalSQL.GetCompanyMembers(_Guid)).ToList();
                    return prMembers;
                } return null;


            }
            catch (Exception)
            {

                return null;
            }

        }

        public List<TPAYMENT_READ> GetCompanyPaymentByGuid(ApplicationUser au, string _Guid)
        {
            try
            {
                //var _user = _dbcontext.MCOMPANY.ForBasicData().Where(a => a.HGUID == _Guid).First();

                //if (_user.COMPANYID != null)
                //{
                //For now the _Guid is Company ID


                //

                var prCompany = _dbcontext.TPAYMENT_READ.FromSqlRaw(_generalSQL.GetCompanyPayments(_Guid))
                                    .ForCompanyPaymentRead()
                                    .ToList();

                //DataTable dtb = prCompany.ToDataTable();
                /*$"SELECT m.COMPANYID,m.CNAME, m.BUILDINGNAME, m.COMPANYNUM,t.PAYMENTID,t.IAMOUNT PAMOUNT,t.PAYMENTNUM, " +
                                            $" p.FYEARMONTH PYEARMONTH, t.PERIODID, " +
                                            $" (SELECT COUNT(DISTINCT MEMBERID) FROM TPAYMENT_DET td WHERE td.PAYMENTID = t.PAYMENTID AND td.LT = 1) MEMCOUNT " +
                                            $" FROM TPAYMENT t " +
                                            $" LEFT JOIN MCOMPANY m ON t.CUSTOMERID = m.COMPANYID " +
                                            $" INNER JOIN MPERIOD p ON p.PRID = t.PERIODID " +
                                            $" WHERE t.CUSTOMERID = @p0" +
                                            $" ORDER BY P.FYEARMONTH ", _Guid);*/

                //prCompany.Fill

                return prCompany;
                //}
                //return null;


            }
            catch (Exception)
            {

                return null;
            }

        }


    }
}
