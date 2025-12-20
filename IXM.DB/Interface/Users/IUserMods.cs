using IXM.Common;
using IXM.GeneralSQL;
using IXM.Models;
using IXM.Models.Core;

namespace IXM.DB
{
    public interface IUserMods
    {
        List<USER_COMPANY> RegisterUserToSystem(ApplicationUser au, Guid _Guid);

    }
}
