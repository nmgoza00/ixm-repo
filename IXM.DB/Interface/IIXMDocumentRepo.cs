using IXM.DB;
using IXM.DB.Interface.B2B;

namespace IXM.DB
{


    public interface IIXMDocumentRepo : IDisposable
    {
        IDocument Document { get; }
        IFileTransferService FileTransfer { get; }

    }
}
