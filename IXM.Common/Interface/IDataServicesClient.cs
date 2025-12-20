using IXM.Models;
using IXM.Models.Core;
using Microsoft.AspNetCore.Components.Forms;
using IXM.Constants;

namespace IXM.Common.Client
{


    public interface IDataServicesClient
    {

        Task<Tuple<byte[], string>> GetDocumentByFileName(IxmAppTemplateFileType FileType, String pSystemName, IxmAppDocumentType DocType, HttpClient httpclient, String JWToken, string PeriodSel, string CompanyId);

        Task<Tuple<byte[], string>> GetDocumentByFileID(String FileID, IxmAppDocumentType documentType, String SystemID, HttpClient httpclient, String JWToken);
        Task<Tuple<byte[], string>> GetDownloadByReportId(String pSystemName, HttpClient httpclient, String JWToken, string PeriodSel, int ReportId, IxmStaticReports tReport);

        Task<IxmReturnStatus> BusinessFileUpload(IxmAppDocumentType documentType, IBrowserFile pimg, IMAGEUPLOAD pmyImage, String pPeriod, string pSourceId);
        Task<IxmReturnStatus> FileUpload(IxmAppDocumentType documentType, IBrowserFile content, String Valu1, string Valu2);
        //Task<int> RemittanceNewMembers(int RMBLID, List<IxmRemittanceMatchTypes> MatchTypes);
        //Task<int> RemittanceUpdateNewMembers(int RMBLID, List<IxmRemittanceMatchTypes> MatchTypes);
        Task<int> TransactionPaymentProcessMembers(int RMBLID, string SystemID);
        Task<int> TransactionPaymentUpdateMembers(int RMBLID, string SystemID, List<IxmRemittanceMatchTypes> MatchTypes);
        Task<int> TransactionPaymentGenerate(int PERIODID, int COMPANYID, string SystemID, int Source);
        Task<int> TransactionPaymentDelete(int RMBLID, string SystemID);
        Task<int> MemberUpdateFromRemitttance(int RMBLID, string pProcessId);
        Task<int> MemberUpdateAge();
        Task<int> MemberUpdateGender();
        Task<int> MemberUpdateStatus();
        Task<Remmittance_POST> GetValuesForPosting(TRMBL rMBL, List<TRMBLD> rMBLD);
        Task<TRMBL_POST> RemmittanceTranslateForPosting(TRMBL rMBL);

    }
}
