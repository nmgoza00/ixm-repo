using IXM.Common;
using IXM.Models;
using IXM.Models.Core;

namespace IXM.DB
{
    public interface IRemittance
    {


        //****************************************************
        Task <List<TRMBLE>> GetRemittanceError(int CompanyId);
        
        //Task<int> CaptureRemittanceToDB(TRMBL_POST DataHeader, List<TRMBLD> DataTable);


    }
}
