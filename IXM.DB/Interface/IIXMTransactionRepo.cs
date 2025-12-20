using IXM.DB;
using IXM.DB.Finance;

namespace IXM.DB
{


    public interface IIXMTransactionRepo : IDisposable
    {

        IRemittance Remittance { get; }
        IFinance Finance { get; }
        //IIXMDBRepo DBRepo { get; }
        ITransaction Transaction { get; }

    }
}
