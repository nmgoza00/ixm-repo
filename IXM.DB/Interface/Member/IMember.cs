using IXM.Common;
using IXM.Models;
using IXM.Models.Core;

namespace IXM.DB
{
    public interface IMember
    {
        MMEMBER GetMemberByGuid(ApplicationUser au, string _Guid);
        List<MMEMBER> GetMemberByCompanyGuid(ApplicationUser au, string _Guid);
        Task <MEMBER_MASTERDATA> GetMember_MASTERDATA();


        Tuple<int, string> MemberUpsert(MMEMBER mMEMBER);
        bool ModifyMemberCard(MMCDD mMMCDD);

    }
}
