using DevExpress.XtraPrinting.Native.Lines;
using IXM.DB;
using IXM.Models;
using IXM.Models.Core;


namespace IXM.Ingest
{


    public interface IFileIngest
    {


        Task<List<XLS_REMITTANCE>> XlsRemittanceInjest(string Username ,string FileID, string SystemID, char Reload);
        Task<int> XlsRemittanceToDB(List<XLS_REMITTANCE> DataTable, string lFName);
        Task<int> XlsRemittanceErrorToDB(List<XLS_REMITTANCE> ErrorDataTable, int pRMBLID);
        Task <Tuple<int,int>> XlsRemittanceErrorExport(List<XLS_REMITTANCE> DataTable, string FullFIleName);
        Task <MemoryStream> XlsExportRemittanceContentInfo(string Username, string FileName, int PeriodId, int CompanyId, string SystemID);
        Task <MOBJECT_DOC> GetFileDocumentInfo(int pFIleId);

    }
}
