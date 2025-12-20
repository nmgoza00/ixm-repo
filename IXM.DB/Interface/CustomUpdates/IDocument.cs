using IXM.Models.Core;
using Microsoft.Extensions.Logging;

namespace IXM.DB
{
    public interface IDocument
    {
        Tuple<int, string> AddEditDocumentToDB(ILogger logger, MOBJECT_DOC model);
        Task<Tuple<int, string>> DocumentTransfer(ILogger logger, MOBJECT_DOC model, int FromID, string pSystemName);
        public int EditOrganizerImage(MOBJECT_DOC model);
    }
}
