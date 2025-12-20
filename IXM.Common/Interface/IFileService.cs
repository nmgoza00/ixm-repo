
using Microsoft.AspNetCore.Http;
using IXM.Models.Core;

namespace IXM.Common
{
    public interface IFileService
    {

        Task<Tuple<int, string, string, int>> SaveDocumentToFileSystem(MOBJECT_DOC model, IFormFile orgimagefile);
        Tuple<int, string, string, int> SaveImageFile(MOBJECT_DOC model, IFormFile orgimagefile);
        public Tuple<int, byte[], string> GetDocumentData(string pFilename);
        //public Tuple<int, string> GetDocPath(String pSystemName, IxmAppDocumentType pDoctype, String lSystemType);

    }

}
