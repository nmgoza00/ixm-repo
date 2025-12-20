using IXM.Common;
using IXM.Models;
using IXM.Models.Core;

namespace IXM.DB
{
    public interface IProcessor
    {
        List<REP_PROCESSOR_LOADEDSCHEDULES> GetProcessorSchedules(string _Guid, string CompanyId, string Period);
        List<TRMBL> GetPendingRemittances(string _Guid, string CompanyId, string Period);


    }
}
