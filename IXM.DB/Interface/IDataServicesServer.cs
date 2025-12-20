using IXM.Models;
using IXM.Models.Core;
using Microsoft.AspNetCore.Components.Forms;


namespace IXM.DB.Server
{


    public interface IDataServicesServer
    {
        /*
        Task<Stream> GetDocumentByFileName(String pFileNme, String pURL, String pSystemName, String pDocType, HttpClient httpclient, String JWToken);

        Task<byte[]> GetDocumentByFileID(String FileID, String SystemID, HttpClient httpclient, String JWToken);

        Task<int> BusinessFileUpload(IBrowserFile pimg, IMAGEUPLOAD pmyImage, String pPeriod);*/

        Task<List<DATA_VALUEMAPPING>> GetDataValueMapping();
        Task<List<MCOMPANY>> GetBranches();
        Task<List<MCITY>> GetCitites();

        Task <string> ProcessQueueAsync(List<TPROCESSTASK> tPROCESSTASK);
        Task<List<PROCESS_RUN>> GetProcessQueueAsync();
        Task<List<PROCESS_RUN>> GetProcessId(List<TPROCESSTASK> tPROCESSTASK);
        Task<List<TPROCESSTASK>> GetProcessTaskParams(int PROCESSID);
        Task<string> ProcessStartAsync(string pProcessId);
        Task<string> ProcessFailAsync(string pMessage, string pProcessId);
        Task<string> ProcessCompleteAsync(string pProcessId);
        Task<string> ProcessDeleteAsync(string pProcessId);
        Task<PROCESS_RUN> ProcessCheckStatus(string pProcessId);

        Task<PROCESS_RUN> ExecuteQueueAsync(int PROCESSID);
        // Done at Client side
        //Task<GeocodeResult?> GeocodeAsync(string address);
        //Task<ReverseGeocodeResult?> ReverseGeocodeAsync(double lat, double lon);


    }
}
