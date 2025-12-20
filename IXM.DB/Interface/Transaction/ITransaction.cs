using IXM.Constants;
using IXM.Models;
using IXM.Models.Core;

namespace IXM.DB
{
    public interface ITransaction
    {



        Task ProcessMembers(ApplicationUser au, int RMBLID);
        Task UpdateMembers(string Username, int RMBLID, List<IxmRemittanceMatchTypes> MatchTypes);
        Task WrapperUpdateMemberDetails(string Username, int RMBLID, List<IxmRemittanceMatchTypes> MatchTypes);

        List<TPAYMENT_DET> GetPayment(string PaymentGuid);
        Task PaymentGenerate(int PERIODID, int COMPANYID, int SystemID, int Source, string UserName);
        Task PaymentSimulate(int PERIODID, int COMPANYID);
        Task PaymentDelete(int PAYMENTID);
        Task<int> PaymentConfirmation(string PaymentGuid, string UserName);
        List<TPAYMENT> GetPaymentConfirmation(string UserGuid);

        Task InvoiceGenerate(int PERIODID, int COMPANYID);
        Task InvoiceSimulate(int PERIODID, int COMPANYID);
        Task InvoiceDelete(int INVOICEID);

        Task CreditNoteGenerate(int PERIODID, int COMPANYID);
        Task CreditNoteSimulate(int PERIODID, int COMPANYID);
        Task CreditNoteDelete(int CREDITNOTEID);


        //****************************************************
        List<MMEMBER> GetMemberNotInRemittance(string RMBLID);
        TPAYMENT PaymentHeaderGenerateFromRemittance(TRMBL value, string pUserName);
        List<TPAYMENT_DET> PaymentDetailGenerateFromRemittance(List<TRMBLD> value, TPAYMENT header);
        TRMBL GenerateRemittanceHeader(TRMBL_POST value, string pUserName);
        Task<int>RemmittanceCaptureGenerate(TRMBL_POST pRMBL, List<TRMBLD> pRMBLD, string pUsername);
        Task<Tuple<int, string>> GenerateRemittanceFromDataFile(MOBJECT_DOC model, string pRMBLID, string pUsername, string pBaseFolder, string lNewFile);
        Task<int> GenerateRemittanceToDataBase(MOBJECT_DOC model, string pRMBLID, string pUsername, string pBaseFolder);
        Task<int> TransferRemittanceToServer(string pSystemId, string pOBJECTID, int? pPERIODID, TRMBL pTRMBL, string pUsername, string pBaseFolder);
        //Task<int> CaptureRemittanceToDB(TRMBL_POST DataHeader, List<TRMBLD> DataTable);


    }
}
