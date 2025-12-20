using DevExpress.XtraPrinting;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using IXM.Common;
using IXM.Models;
using Microsoft.Extensions.Logging;
using System.IO;

namespace IXM.DB
{
    public interface ICustomUpdates
    {
        Task GetData<TEntity>(string lsql);
        Task StatusUpdate(string Username, string TBNAME, string KEYFIELD, string KEYVALUE, string UPDFIELD, int UPDVALUE);
        Task RemittanceMembersUpdate(string Username, string RMBLID, int MEMBERS);

        Task RemittanceUpdateDBDetails(string MEMEXISTS, string Username, string RMBLID);
        //int AddEditDocumentToDB(ILogger logger, MOBJECT_DOC model);
    }
}
