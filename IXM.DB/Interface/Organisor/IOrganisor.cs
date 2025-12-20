using IXM.Common;
using IXM.Models;
using IXM.Models.Core;

namespace IXM.DB
{
    public interface IOrganisor
    {
        List<ORGANISER_COMPANY> GetOrganisorCompanies(string _Guid);
        List<ORGANISER_COMPANY> GetOrganisorBranches(string _Guid);
        List<MCOMPANY> GetOrganisorUnmappedCompanies();
        List<MCOMPANY> GetOrganisorUnmappedBranches();


    }
}
