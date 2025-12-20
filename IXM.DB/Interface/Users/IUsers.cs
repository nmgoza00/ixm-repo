using IXM.Constants;
using IXM.Models;


namespace IXM.DB
{
    public interface IUsers
    {
        List<USER_COMPANY> GetUserLinkedCompanies(ApplicationUser au, Guid _Guid);
        Task <int> RegisterUserToSystem(REGISTER_DATA registerData);
        Task<USER_MASTERDATA> GetUser_MASTERDATA();

    }
}
