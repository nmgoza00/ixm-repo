using IXM.Common;
using IXM.Models;
using IXM.Models.Core;
using System.Data;
using static IXM.Common.Generics.Generics4API;

namespace IXM.DB
{
    public interface ICompany
    {
        List<MMEMBER_READ> GetCompanyMembersByGuid(ApplicationUser au, string _Guid);
        List<TPAYMENT_READ> GetCompanyPaymentByGuid(ApplicationUser au, string _Guid);
        List<MCOMPANY> GetCompanyList(ApplicationUser au, string _Guid);
        Task<MCOMPANY> GetCompanyBySystem(ApplicationUser au, string System, string Company);


    }
}
