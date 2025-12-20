using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using IXM.Constants;
using System.Text;
using IXM.Common.Interfaces;
using IXM.Models;
using IXM.Common.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using IXM.Constants;

namespace IXM.Common.Implementations
{
    
    public class Calendar : ICalendar
    {

        private HttpClient _httpClient;
        private IGenValues _genValues;
        private ILogger<Calendar> _logger;
        public Calendar(
                        IWebHostEnvironment environment, 
                        IConfiguration configuration, 
                        ILogger<Calendar> logger,
                        IGenValues genValues,
                        CustomData customData)
        {

            GetClient();
            _genValues = genValues;
            _logger = logger;

        }

        public void GetClient()
        {

            if (_httpClient == null)
            {
                _httpClient = new HttpClient();

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", CommonConstants.JWToken);

            }
            else if (_httpClient.BaseAddress == null)
            {
                try
                {
                    _httpClient.BaseAddress = new Uri(CommonConstants.BaseAddress);

                }
                catch (Exception)
                {

                    _httpClient.BaseAddress = null;
                }
            }

        }

        public async Task<IEnumerable<MPERIOD>> GetPeriodsAll()
        {
            try
            {

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _genValues.GetToken());
                GetClient();

                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

                var _ub = new UriBuilder(CommonConstants.PeriodList);
                _ub.Query = queryString.ToString();

                HttpResponseMessage _response = await _httpClient.GetAsync(CommonConstants.PeriodList);


                if (_response.IsSuccessStatusCode)
                {

                    var stream = await _response.Content.ReadFromJsonAsync<IEnumerable<MPERIOD>>();
                    _logger.LogInformation("Data Access in IXMWebApp - 'GetPeriodsAll' processed with {@Count} records from User {@User}.", stream.Count(), UserDetails.CurrentUser );
                    return stream;
                }
                else return null;
            }
            catch (Exception ex)
            {
                //string _ex = ex.Message;
                _logger.LogError("Error in IXMWebApp - 'GetPeriodsAll' with message :{Message} ", ex.Message);
                var str1 = new MemoryStream(Encoding.Default.GetBytes(ex.Message));
                return null;

            }
        }

        public async Task<IEnumerable<MPERIOD>> GetPeriodsFrom(int? PeriodId)
        {
            try
            {

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _genValues.GetToken());
                GetClient();

                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
                queryString.Add("PeriodId", PeriodId.ToString());

                var _ub = new UriBuilder(CommonConstants.PeriodList);
                _ub.Query = queryString.ToString();


                HttpResponseMessage _response = await _httpClient.GetAsync(_ub.Uri);


                if (_response.IsSuccessStatusCode)
                {

                    var stream = await _response.Content.ReadFromJsonAsync<IEnumerable<MPERIOD>>();
                    _logger.LogInformation("Data Access in IXMWebApp - 'GetPeriodsAll' processed with {@Count} records from User {@User}.", stream.Count(), UserDetails.CurrentUser);
                    return stream;
                }
                else return null;
            }
            catch (Exception ex)
            {
                //string _ex = ex.Message;
                _logger.LogError("Error in IXMWebApp - 'GetPeriodsAll' with message :{Message} ", ex.Message);
                var str1 = new MemoryStream(Encoding.Default.GetBytes(ex.Message));
                return null;

            }
        }


    }
}
