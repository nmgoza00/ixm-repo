using IXM.Models;
using IXM.Models.Core;

namespace IXM.Common {
    public interface IDataImport
    {

        public List<DATA_VALUEMAPPING> DataValueMap { get; set; }
        public List<MCOMPANY> Branches { get; set; }
        public List<MCITY> Location { get; set; }
        public List<T>? IxmExcelImporter<T>(string FilePathAndName, string SheetName);
        public int IxmExcelValidateImport(ref List<XLS_REMITTANCE> DataTable);
        Task<MemoryStream>  IxmExportedRemittanceInfo(string FilePathAndName, string SheetName, string PeriodName, MCOMPANY DataInfo);
        public int IxmExcelExportDataIssues<T>(List<T> DataTable, List<TCAPTIONS> Captions, string FilePathAndName);
        public List<TRMBLD> IxmRemittanceEntityImporter(IEnumerable<XLS_REMITTANCE> xlsEntity);
    }
}
