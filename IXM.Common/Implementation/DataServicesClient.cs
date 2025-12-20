using IXM.Constants;
using IXM.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using IXM.Common.Data;
using Microsoft.AspNetCore.Components.Forms;
using IXM.Models.Core;
using System.Net.Http.Json;

namespace IXM.Common.Client
{
    public class DataServicesClient : IDataServicesClient
    {

        public enum IXMModType
        {
            Insert,
            Modify,
            Delete
        }

        //private string _baseurl = App.aAppInfo.BaseAPIURL;
        private string _baseurl = "";
        private IWebHostEnvironment _environment;
        private IConfiguration _configuration;
        private HttpClient _httpClient;
        private List<MSYSTEMS> mSYSTEMS = new List<MSYSTEMS>();

        public DataServicesClient(IWebHostEnvironment environment, IConfiguration configuration, CustomData customData)
        {
            this._environment = environment;
            this._configuration = configuration;
            GetClient();
            mSYSTEMS = customData.GetSystems();

        }

        public void GetClient()
        {

            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
                try
                {

                    _httpClient.BaseAddress = new Uri(CommonConstants.BaseAddress);
                    //_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", CommonConstants.JWToken);

                }
                catch (Exception)
                {

                }

            }

        }

        public async Task<Tuple<byte[], string>>GetDocumentByFileName(IxmAppTemplateFileType FileType, String pSystemName, IxmAppDocumentType DocType, HttpClient httpclient, String JWToken, string PeriodSel, string CompanyId)
        {
            try
            {
                var _client = httpclient;

                int en = (int)FileType;
                int dt = (int)DocType;
                var userToken = JWToken;
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userToken);

                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

                queryString.Add("SystemId", pSystemName);
                queryString.Add("DocType", dt.ToString());
                queryString.Add("FileType", en.ToString());
                queryString.Add("PeriodId", PeriodSel);
                queryString.Add("CompanyId", CompanyId);

                var _ub = new UriBuilder(CommonConstants.DocumentFileDownloadURL);
                _ub.Query = queryString.ToString();

                HttpResponseMessage _response = await _client.GetAsync(_ub.Uri);
                var stream = await _response.Content.ReadAsByteArrayAsync();
                //DialogService _dialogservice = new DialogService();

                if (_response.StatusCode == HttpStatusCode.NotFound)
                {
                    //await _dialogservice.DisplayActionSheet("Error", "Requested file currently not available in Server.", "Ok");
                    return new Tuple<byte[], string>(stream, "NotFound");
                }
                else if (_response.IsSuccessStatusCode)
                {

                    return new Tuple<byte[], string>(stream, _response.Content.Headers.ContentType.MediaType);
                    /*var fileSaverResult = await FileSaver.Default.SaveAsync("test.txt", stream, CancellationToken);
                    if (fileSaverResult.IsSuccessful)
                    {
                        await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}").Show(cancellationToken);
                    }
                    else
                    {
                        await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}").Show(cancellationToken);
                    }*/
                }
            }
            catch (Exception ex)
            {
                //string _ex = ex.Message;
                var str1 = new MemoryStream(Encoding.Default.GetBytes(ex.Message));
                return null;

            }
            var str = new MemoryStream(Encoding.Default.GetBytes("Unknown message"));
            return null;
        }


        public async Task<Tuple<byte[], string>> GetDocumentByFileID(String FileID, IxmAppDocumentType documentType, String SystemID, HttpClient httpclient, String JWToken)
        {
            try
            {

                var _client = httpclient;


                var userToken = JWToken;
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userToken);

                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

                int en = (int)documentType;
                queryString.Add("FileType", en.ToString());
                queryString.Add("FileID", FileID);
                queryString.Add("SystemID", SystemID);

                var _ub = new UriBuilder(CommonConstants.DocumentFileIDDownloadURL);
                _ub.Query = queryString.ToString();

                HttpResponseMessage _response = await _client.GetAsync(_ub.Uri);
                //DialogService _dialogservice = new DialogService();
                var stream = await _response.Content.ReadAsByteArrayAsync();

                if (_response.StatusCode == HttpStatusCode.NotFound)
                {
                    //await _dialogservice.DisplayActionSheet("Error", "Requested file currently not available in Server.", "Ok");
                    return new Tuple<byte[], string>(stream,"NotFound");
                }
                else if (_response.IsSuccessStatusCode)
                {
                    return new Tuple<byte[], string>(stream, _response.Content.Headers.ContentType.MediaType);


                    /*var fileSaverResult = await FileSaver.Default.SaveAsync("test.txt", stream, CancellationToken);
                    if (fileSaverResult.IsSuccessful)
                    {
                        await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}").Show(cancellationToken);
                    }
                    else
                    {
                        await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}").Show(cancellationToken);
                    }*/
                }
            }
            catch (Exception ex)
            {
                //string _ex = ex.Message;
                var str1 = new MemoryStream(Encoding.Default.GetBytes(ex.Message));
                return null;

            }
            var str = new MemoryStream(Encoding.Default.GetBytes("Unknown message"));
            return null;
        }

        public async Task<Tuple<byte[], string>> GetDownloadByReportId(String pSystemName, HttpClient httpclient, String JWToken, string PeriodSel, int ReportId, IxmStaticReports tReport)
        {
            try
            {
                var _client = httpclient;
                var userToken = JWToken;
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JWToken);

                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

                queryString.Add("SystemId", pSystemName);
                queryString.Add("PeriodId", PeriodSel);
                queryString.Add("ReportId", ReportId.ToString());

                if(tReport != null)
                {
                    int v = (int)tReport;
                    queryString.Add("TReport", v.ToString());
                }


                var _ub = new UriBuilder(CommonConstants.ReportDocumentDownloadURL);
                _ub.Query = queryString.ToString();

                HttpResponseMessage _response = await _client.GetAsync(_ub.Uri);
                var stream = await _response.Content.ReadAsByteArrayAsync();
                //DialogService _dialogservice = new DialogService();

                if (_response.StatusCode == HttpStatusCode.NotFound)
                {
                    //await _dialogservice.DisplayActionSheet("Error", "Requested file currently not available in Server.", "Ok");
                    return new Tuple<byte[], string>(stream, "NotFound");
                }
                else if (_response.IsSuccessStatusCode)
                {

                    return new Tuple<byte[], string>(stream, _response.Content.Headers.ContentType.MediaType);
                    /*var fileSaverResult = await FileSaver.Default.SaveAsync("test.txt", stream, CancellationToken);
                    if (fileSaverResult.IsSuccessful)
                    {
                        await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}").Show(cancellationToken);
                    }
                    else
                    {
                        await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}").Show(cancellationToken);
                    }*/
                }
            }
            catch (Exception ex)
            {
                //string _ex = ex.Message;
                var str1 = new MemoryStream(Encoding.Default.GetBytes(ex.Message));
                return null;

            }
            var str = new MemoryStream(Encoding.Default.GetBytes("Unknown message"));
            return null;
        }

        public async Task<IxmReturnStatus> BusinessFileUpload(IxmAppDocumentType documentType, IBrowserFile pimg, IMAGEUPLOAD pmyImage, String pPeriod, string pSourceId)
        {

            try
            {

                var hcontent = new MultipartFormDataContent();

                //hcontent.Headers.ContentType =  = "multipart/form-data";
                //hcontent.Add(new StringContent(pmyUser.OBJECTID.ToString()), "OBJECTID");
                hcontent.Add(new StringContent("-1"), "OBJECTID");

                hcontent.Add(new StringContent(documentType.ToString()), "SOURCEOBJ");
                // "BUSINESSUPLOAD"
                var ls = GetSOURCEFLD(documentType);
                hcontent.Add(new StringContent(ls), "SOURCEFLD");
               // hcontent.Add(new StringContent(nameof(documentType.XLS_REMITTANCE)), "DOCTYPE");
                hcontent.Add(new StringContent(documentType.ToString()), "DOCTYPE");
                hcontent.Add(new StringContent(UserDetails.SystemId), "SYSTEMID");
                hcontent.Add(new StringContent(UserDetails.CurrentUser), "UNAME");
                hcontent.Add(new StringContent(pSourceId), "SOURCEID");
                //hcontent.Add(new StringContent(pmyImage.FileName.ToString()), "LDOCUMENTNAME");
                //hcontent.Add(new StringContent(pmyImage.FileName.ToString()), "DOCUMENTNAME");
                hcontent.Add(new StringContent(UserDetails.CurrentUser), "INSERTED_BY");
                if (pPeriod != null) { hcontent.Add(new StringContent(pPeriod), "FYEARMONTH"); }

                //var filecontent = pimg.OpenReadStream(Constants.FileProperties.MaxFileSize);
                //hcontent.Headers.ContentType = MediaTypeHeaderValue.Parse(pimg.ContentType);
                //hcontent.Add(new StreamContent(pimg.OpenReadStream(Constants.FileProperties.MaxFileSize)), "ORGIMAGEFILE", pimg.Name);

                //var stream = new LazyBrowserFileStream(file, maxFileSize);

                var fileContent = new StreamContent(pimg.OpenReadStream(CommonConstants.FileProperties.MaxFileSize));
                //fileContent.Headers.ContentType = new MediaTypeHeaderValue(pimg.ContentType);
                hcontent.Add(fileContent, "ORGIMAGEFILE", pimg.Name);

                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
                //hcontent.Add(new StringContent(pmyImage.FileName.ToString()), "LDOCUMENTNAME");
                //hcontent.Add(new StringContent(pmyImage.FileName.ToString()), "DOCUMENTNAME");
                queryString.Add("INSERTED_BY", UserDetails.CurrentUser);
                //var _ub = new UriBuilder(Constants.DocumentFileUploadURL);
                //_ub.Query = queryString.ToString();
                //_httpClient.Timeout = 200;
                HttpResponseMessage _response = await _httpClient.PostAsync(CommonConstants.DocumentFileUploadURL, hcontent);
                //HttpResponseMessage _response = await _httpClient.GetAsync("/api/ServiceCheck");
                if (_response.IsSuccessStatusCode)
                {

                    var stream = await _response.Content.ReadFromJsonAsync<IxmReturnStatus>();
                    return stream;

                }

            }
            catch (Exception ex)
            {
                string _ex = ex.Message;

            }
            return null;

        }



        public string GetSOURCEFLD(IxmAppDocumentType docType)
        {

            if (docType == IxmAppDocumentType.EVENT) { return "EVENTID"; }
            else if (docType == IxmAppDocumentType.XLS_REMITTANCE) { return "COMPANYID"; }
            else if (docType == IxmAppDocumentType.REMITTANCE_POP) { return "COMPANYID"; }
            else if (docType == IxmAppDocumentType.REMITTANCE_EMAIL) { return "COMPANYID"; }
            else { return "COMPANYID"; }

        }

        public async Task<IxmReturnStatus> FileUpload(IxmAppDocumentType documentType, IBrowserFile pimg, string Valu1, string Valu2)
        {

            try
            {

                var hcontent = new MultipartFormDataContent();

                //hcontent.Headers.ContentType =  = "multipart/form-data";
                //hcontent.Add(new StringContent(pmyUser.OBJECTID.ToString()), "OBJECTID");
                hcontent.Add(new StringContent("-1"), "OBJECTID");
                hcontent.Add(new StringContent("BUSINESSUPLOAD"), "SOURCEOBJ");
                hcontent.Add(new StringContent("COMPANYID"), "SOURCEFLD");
                // hcontent.Add(new StringContent(nameof(documentType.XLS_REMITTANCE)), "DOCTYPE");
                hcontent.Add(new StringContent(documentType.ToString()), "DOCTYPE");
                hcontent.Add(new StringContent(UserDetails.SystemId), "SYSTEMID");
                hcontent.Add(new StringContent(UserDetails.CurrentUser), "UNAME");
                hcontent.Add(new StringContent(Valu1), "SOURCEID");
                //hcontent.Add(new StringContent(pmyImage.FileName.ToString()), "LDOCUMENTNAME");
                //hcontent.Add(new StringContent(pmyImage.FileName.ToString()), "DOCUMENTNAME");
                hcontent.Add(new StringContent(UserDetails.CurrentUser), "INSERTED_BY");
                //if (pPeriod != null) { hcontent.Add(new StringContent(pPeriod), "FYEARMONTH"); }

                //var filecontent = pimg.OpenReadStream(Constants.FileProperties.MaxFileSize);
                //hcontent.Headers.ContentType = MediaTypeHeaderValue.Parse(pimg.ContentType);
                //hcontent.Add(new StreamContent(pimg.OpenReadStream(Constants.FileProperties.MaxFileSize)), "ORGIMAGEFILE", pimg.Name);

                //var stream = new LazyBrowserFileStream(file, maxFileSize);

                var fileContent = new StreamContent(pimg.OpenReadStream(CommonConstants.FileProperties.MaxFileSize));
                //fileContent.Headers.ContentType = new MediaTypeHeaderValue(pimg.ContentType);
                hcontent.Add(fileContent, "ORGIMAGEFILE", pimg.Name);

                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
                //hcontent.Add(new StringContent(pmyImage.FileName.ToString()), "LDOCUMENTNAME");
                //hcontent.Add(new StringContent(pmyImage.FileName.ToString()), "DOCUMENTNAME");
                queryString.Add("INSERTED_BY", UserDetails.CurrentUser);
                //var _ub = new UriBuilder(Constants.DocumentFileUploadURL);
                //_ub.Query = queryString.ToString();
                //_httpClient.Timeout = 200;
                HttpResponseMessage _response = await _httpClient.PostAsync(CommonConstants.DocumentFileUploadURL, hcontent);
                //HttpResponseMessage _response = await _httpClient.GetAsync("/api/ServiceCheck");
                if (_response.IsSuccessStatusCode)
                {

                    var stream = await _response.Content.ReadFromJsonAsync<IxmReturnStatus>();
                    return stream;

                }

            }
            catch (Exception ex)
            {
                string _ex = ex.Message;

            }
            return null;

        }

        public async Task<int> TransactionPaymentProcessMembers(int RMBLID, string SystemID)
        {
            try
            {

                var _client = _httpClient;


                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", CommonConstants.JWToken);

                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

                queryString.Add("RMBLID", RMBLID.ToString());
                queryString.Add("SystemId", SystemID);

                var _ub = new UriBuilder(CommonConstants.TransactionPaymentProcessMembers);
                _ub.Query = queryString.ToString();

                HttpResponseMessage _response = await _client.PostAsync(_ub.Uri, null);
                var stream = await _response.Content.ReadAsByteArrayAsync();
                //DialogService _dialogservice = new DialogService();

                if (_response.StatusCode == HttpStatusCode.NotFound)
                {
                    //await _dialogservice.DisplayActionSheet("Error", "Requested file currently not available in Server.", "Ok");
                    return 0;
                }
                else if (_response.IsSuccessStatusCode)
                {

                    return 0;
                    /*var fileSaverResult = await FileSaver.Default.SaveAsync("test.txt", stream, CancellationToken);
                    if (fileSaverResult.IsSuccessful)
                    {
                        await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}").Show(cancellationToken);
                    }
                    else
                    {
                        await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}").Show(cancellationToken);
                    }*/
                }
            }
            catch (Exception ex)
            {
                //string _ex = ex.Message;
                var str1 = new MemoryStream(Encoding.Default.GetBytes(ex.Message));
                return 0;

            }
            var str = new MemoryStream(Encoding.Default.GetBytes("Unknown message"));
            return 1;

        }
        public async Task<int> TransactionPaymentUpdateMembers(int RMBLID, string SystemID, List<IxmRemittanceMatchTypes> MatchTypes)
        {
            try
            {

                var _client = _httpClient;


                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", CommonConstants.JWToken);

                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
                queryString.Add("RMBLID", RMBLID.ToString());

                var _ub = new UriBuilder(CommonConstants.TransactionPaymentUpdateMembers);
                _ub.Query = queryString.ToString();

                HttpResponseMessage _response = await _client.GetAsync(_ub.Uri);
                var stream = await _response.Content.ReadAsByteArrayAsync();
                //DialogService _dialogservice = new DialogService();

                if (_response.StatusCode == HttpStatusCode.NotFound)
                {
                    return 0;
                }
                else if (_response.IsSuccessStatusCode)
                {

                    return 0;
                }
            }
            catch (Exception ex)
            {
                //string _ex = ex.Message;
                var str1 = new MemoryStream(Encoding.Default.GetBytes(ex.Message));
                return 0;

            }
            var str = new MemoryStream(Encoding.Default.GetBytes("Unknown message"));
            return 1;


        }
        public async Task<int> TransactionPaymentGenerate(int PERIODID, int COMPANYID, string SystemID, int Source)
        {
            try
            {

                var _client = _httpClient;


                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", CommonConstants.JWToken);

                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

                queryString.Add("PERIODID", PERIODID.ToString());
                queryString.Add("COMPANYID", COMPANYID.ToString());
                queryString.Add("SystemId", UserDetails.SystemId);
                queryString.Add("Source", "0");

                var _ub = new UriBuilder(CommonConstants.TransactionPaymentGenerate);
                _ub.Query = queryString.ToString();

               // StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                //HttpResponseMessage _response = await _client.GetAsync(_ub.Uri);

                HttpResponseMessage _response = await _httpClient.PostAsync(_ub.Uri, null);
                var stream = await _response.Content.ReadAsByteArrayAsync();

                if (_response.StatusCode == HttpStatusCode.NotFound)
                {
                    //await _dialogservice.DisplayActionSheet("Error", "Requested file currently not available in Server.", "Ok");
                    return 0;
                }
                else if (_response.IsSuccessStatusCode)
                {

                    return 0;
                }
            }
            catch (Exception ex)
            {
                //string _ex = ex.Message;
                var str1 = new MemoryStream(Encoding.Default.GetBytes(ex.Message));
                return 0;

            }
            var str = new MemoryStream(Encoding.Default.GetBytes("Unknown message"));
            return 1;


        }
        public async Task<int> TransactionPaymentDelete(int PAYMENTID, string SystemID)
        {
            try
            {

                var _client = _httpClient;


                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", CommonConstants.JWToken);

                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

                queryString.Add("PAYMENTID", PAYMENTID.ToString());

                var _ub = new UriBuilder(CommonConstants.TransactionPaymentDelete);
                _ub.Query = queryString.ToString();

                HttpResponseMessage _response = await _client.GetAsync(_ub.Uri);
                var stream = await _response.Content.ReadAsByteArrayAsync();

                if (_response.StatusCode == HttpStatusCode.NotFound)
                {
                    return 0;
                }
                else if (_response.IsSuccessStatusCode)
                {

                    return 0;
                }
            }
            catch (Exception ex)
            {
                //string _ex = ex.Message;
                var str1 = new MemoryStream(Encoding.Default.GetBytes(ex.Message));
                return 0;

            }
            var str = new MemoryStream(Encoding.Default.GetBytes("Unknown message"));
            return 1;



        }

        public async Task<int> MemberUpdateFromRemitttance(int RMBLID, string pProcessId)
        {

            try
            {

                var _client = _httpClient;


                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", CommonConstants.JWToken);

                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

                queryString.Add("RMBLID", RMBLID.ToString());
                queryString.Add("PROCESSID", pProcessId);
                queryString.Add("SystemID", UserDetails.SystemId.ToString());

                var _ub = new UriBuilder(CommonConstants.B2B_RemmittanceDBDetailUpdate);
                _ub.Query = queryString.ToString();

                HttpResponseMessage _response = await _client.PostAsync(_ub.Uri, null);
                var stream = await _response.Content.ReadAsByteArrayAsync();
                //DialogService _dialogservice = new DialogService();

                if (_response.StatusCode == HttpStatusCode.NotFound)
                {
                    //await _dialogservice.DisplayActionSheet("Error", "Requested file currently not available in Server.", "Ok");
                    return 0;
                }
                else if (_response.IsSuccessStatusCode)
                {

                    return 0;
                    /*var fileSaverResult = await FileSaver.Default.SaveAsync("test.txt", stream, CancellationToken);
                    if (fileSaverResult.IsSuccessful)
                    {
                        await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}").Show(cancellationToken);
                    }
                    else
                    {
                        await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}").Show(cancellationToken);
                    }*/
                }
            }
            catch (Exception ex)
            {
                //string _ex = ex.Message;
                var str1 = new MemoryStream(Encoding.Default.GetBytes(ex.Message));
                return 0;

            }
            var str = new MemoryStream(Encoding.Default.GetBytes("Unknown message"));
            return 1;

        }

        public async Task<int> MemberUpdateAge()
        {

            try
            {

                var _client = _httpClient;


                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", CommonConstants.JWToken);

                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

                var _ub = new UriBuilder(CommonConstants.DBTask_MemberUpdateAge);
                _ub.Query = queryString.ToString();

                HttpResponseMessage _response = await _client.PostAsync(_ub.Uri, null);
                var stream = await _response.Content.ReadAsByteArrayAsync();

                if (_response.StatusCode == HttpStatusCode.NotFound)
                {
                    //await _dialogservice.DisplayActionSheet("Error", "Requested file currently not available in Server.", "Ok");
                    return 0;
                }
                else if (_response.IsSuccessStatusCode)
                {

                    return 0;

                }
            }
            catch (Exception ex)
            {
                //string _ex = ex.Message;
                var str1 = new MemoryStream(Encoding.Default.GetBytes(ex.Message));
                return 0;

            }
            var str = new MemoryStream(Encoding.Default.GetBytes("Unknown message"));
            return 1;

        }

        public async Task<int> MemberUpdateGender()
        {

            try
            {

                var _client = _httpClient;


                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", CommonConstants.JWToken);

                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

                var _ub = new UriBuilder(CommonConstants.DBTask_MemberUpdateGender);
                _ub.Query = queryString.ToString();

                HttpResponseMessage _response = await _client.PostAsync(_ub.Uri, null);
                var stream = await _response.Content.ReadAsByteArrayAsync();
                //DialogService _dialogservice = new DialogService();

                if (_response.StatusCode == HttpStatusCode.NotFound)
                {
                    //await _dialogservice.DisplayActionSheet("Error", "Requested file currently not available in Server.", "Ok");
                    return 0;
                }
                else if (_response.IsSuccessStatusCode)
                {

                    return 0;

                }
            }
            catch (Exception ex)
            {
                //string _ex = ex.Message;
                var str1 = new MemoryStream(Encoding.Default.GetBytes(ex.Message));
                return 0;

            }
            var str = new MemoryStream(Encoding.Default.GetBytes("Unknown message"));
            return 1;

        }

        public async Task<int> MemberUpdateStatus()
        {

            try
            {

                var _client = _httpClient;


                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", CommonConstants.JWToken);

                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

                var _ub = new UriBuilder(CommonConstants.DBTask_MemberUpdateStatus);
                _ub.Query = queryString.ToString();

                HttpResponseMessage _response = await _client.PostAsync(_ub.Uri, null);
                var stream = await _response.Content.ReadAsByteArrayAsync();
                //DialogService _dialogservice = new DialogService();

                if (_response.StatusCode == HttpStatusCode.NotFound)
                {
                    //await _dialogservice.DisplayActionSheet("Error", "Requested file currently not available in Server.", "Ok");
                    return 0;
                }
                else if (_response.IsSuccessStatusCode)
                {

                    return 0;

                }
            }
            catch (Exception ex)
            {
                //string _ex = ex.Message;
                var str1 = new MemoryStream(Encoding.Default.GetBytes(ex.Message));
                return 0;

            }
            var str = new MemoryStream(Encoding.Default.GetBytes("Unknown message"));
            return 1;

        }
        public async Task<TRMBL_POST> RemmittanceTranslateForPosting(TRMBL rMBL)
        {
            Remmittance_POST pRMBL_POST = new Remmittance_POST();

            TRMBL_POST lv = new TRMBL_POST
            {
                RMBLID = rMBL.RMBLID,
                RMBLNUM = rMBL.RMBLNUM,
                RSTYPEID = rMBL.RSTYPEID,
                COMPANYID = rMBL.COMPANYID,
                STATUSID = rMBL.STATUSID,
                OBJECTID = rMBL.OBJECTID,
                POPOBJECTID = rMBL.POPOBJECTID,
                EMLOBJECTID = rMBL.EMLOBJECTID,
                PAYMENTID = rMBL.PAYMENTID,
                PAYMENTREF = rMBL.PAYMENTREF,
                ADMINFEE = rMBL.ADMINFEE,
                ADMINFEETYPE = rMBL.ADMINFEETYPE,
                INSBY = rMBL.INSBY,
                INSDAT = rMBL.INSDAT,
                MODAT = DateTime.UtcNow,
                PERIODID = rMBL.PERIODID,
                MODBY = rMBL.MODBY,
                FLOADS = rMBL.FLOADS,
                MEMBERS = rMBL.MEMBERS,
                IAMOUNT = rMBL.IAMOUNT
            };

            return lv;

        }



        public async Task<Remmittance_POST> GetValuesForPosting(TRMBL rMBL, List<TRMBLD> rMBLD)
        {
            Remmittance_POST pRMBL_POST = new Remmittance_POST();

            TRMBL_POST lv = await RemmittanceTranslateForPosting(rMBL);


            if (pRMBL_POST.Remmittance == null)
            {
                pRMBL_POST.Remmittance = new TRMBL_POST();
            }


            if (pRMBL_POST.RemmittanceDetail == null)
            {
                pRMBL_POST.RemmittanceDetail = new List<TRMBLD>();
            }

            pRMBL_POST.Remmittance = lv;
            pRMBL_POST.RemmittanceDetail.AddRange(rMBLD);


            return pRMBL_POST;
        }


    }
}
