using IXM.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IXM.DB.Interface.B2B
{
    public interface IFileTransferService
    {
        /// <summary>
        /// Uploads a file stream to the remote server and returns the remote path.
        /// </summary>
        Task<Tuple<int, string>> UploadAsync(Stream fileStream, string fileName, string targetFilepath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads a file from the remote server to a stream.
        /// </summary>
        Task<Stream> DownloadAsync(string remotePath, CancellationToken cancellationToken = default);
    }
}
