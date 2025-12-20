using IXM.Models;
using IXM.Models.Core;
using static IXM.Common.Generics.Generics4API;


namespace IXM.DB.B2B
{


    public interface IGetB2BDataSets
    {

        Task<PageList<TRMBLD>> RemittanceDetailHandle(IXM.Models.Core.Remittance request);



    }
}
