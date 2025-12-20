using DocumentFormat.OpenXml.Office.CoverPageProps;
using DocumentFormat.OpenXml.Office.Word;
using IXM.Constants;
using IXM.DB.Interface.B2B;
using IXM.Models.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Org.BouncyCastle.Utilities.Zlib;
using Renci.SshNet;
using System.Runtime.Intrinsics.X86;

namespace IXM.DB.Implementation.B2B
{
    public class SftpFileTransferService : IFileTransferService
    {
        private readonly string _host;
        private readonly string _username;
        private readonly string _password;
        private readonly int _port;
        private readonly string _remoteBasePath;

        public SftpFileTransferService(IConfiguration configuration
            )
        {
            // Get config from appsettings.json
            _host = configuration["Sftp:Host"];
            _username = configuration["Sftp:Username"];
            _password = configuration["Sftp:Password"];
            _port = int.Parse(configuration["Sftp:Port"] ?? "22");
            _remoteBasePath = configuration["Sftp:RemoteBasePath"] ?? "uploads";
        }

        public async Task<Tuple<int,string>> UploadAsync(Stream fileStream, string fileName, string targetFilepath, CancellationToken cancellationToken = default)
        {

            try
            {
                if (fileStream == null || fileStream.Length == 0)
                    throw new ArgumentException("File stream is empty", nameof(fileStream));

                //var remotePath = $"{_remoteBasePath}/{Guid.NewGuid()}_{fileName}";
                var remotePath = $"{targetFilepath}/{fileName}";

                //var remotePath = $"{targetFilepath}/{fileName}";
                var sourcePath = targetFilepath.Replace("Upload", "C:\\Users\\" + _username + "\\Upload");
                sourcePath = sourcePath.Replace("/", "\\");
                var targetPath = targetFilepath.Replace("Upload","C:\\APPS");
                targetPath = targetPath.Replace("/","\\");

                var remoteCmd = $@"C:\Users\{ _username}\Upload\movefile.cmd";   // remote script

                using (var sftp = new SftpClient(_host, _port, _username, _password))
                {
                    sftp.Connect();

                    if (!sftp.Exists(targetFilepath))
                    {
                        sftp.CreateDirectory(targetFilepath);
                    }

                    sftp.ChangeDirectory(targetFilepath);

                    // Write the stream to file
                    /*using (var fs = new FileStream("C:\\myData\\tester.xlsx", FileMode.Create, FileAccess.Write))
                    {
                        await fileStream.CopyToAsync(fs);
                    }*/

                    // Stream upload
                    fileStream.Position = 0; // rewind the stream before uploading
                    sftp.UploadFile(fileStream, fileName);

                    sftp.Disconnect();
                }




                // 2. Run CMD with parameters
                using (var ssh = new SshClient(_host, _port, _username, _password))
                {
                    ssh.Connect();

                    // pass file name and target folder
                    string command = $@"cmd /c ""{remoteCmd} {fileName} {sourcePath} {targetPath}""";
                    var cmd = ssh.CreateCommand(command);

                    var result = cmd.Execute();

                    Console.WriteLine("Output: " + result);
                    if (!string.IsNullOrEmpty(cmd.Error))
                    {

                        Console.WriteLine("Error: " + cmd.Error);
                        return new Tuple<int, string>(-1, cmd.Error.ToString());

                    }
                    ssh.Disconnect();
                }





                return  new Tuple<int, string>(0, targetPath);

            }
            catch (Exception e)
            {

                throw;
            }
        }




public async Task<Stream> DownloadAsync(string remotePath, CancellationToken cancellationToken = default)
        {
            var memoryStream = new MemoryStream();

            using var sftp = new SftpClient(_host, _port, _username, _password);
            sftp.Connect();
            sftp.DownloadFile(remotePath, memoryStream);
            sftp.Disconnect();

            memoryStream.Position = 0; // rewind before returning
            return memoryStream;
        }


    }
}


