using IXM.Models;

namespace IXM.DB.Finance
{
    public interface IFinance
    {

        Task<int> Finance_GeneratePaymentGeneralLedger(TPAYMENT pPAYMENT, string UserName);

        Task<int> Finance_ReversePaymentGeneralLedger(TTJRN pTJRN, string UserName);
        Task<int> Finance_GenerateInvoiceGeneralLedger(TINVOICE pINVOICE, string UserName);
        Task<int> Finance_ReverseInvoiceGeneralLedgerr(TTJRN pTJRN, string UserName);




    }
}
