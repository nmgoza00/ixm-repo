using IXM.Common;
using IXM.Models;
using IXM.Models.Core;

namespace IXM.DB
{
    public interface ILegal
    {
        Task<List<TCASE>> GetCases();
        Task<int> PostCase(TCASE_WRITE value, string pUsername);

    }
}
