using System;
using IXM.Constants;
using System.Collections.Specialized;
using System.Linq;
using IXM.Models;
using System.Net.Http.Json;
using IXM.Security;

namespace IXM.Security
{
    public class IXMUserAuth
        (
        HttpClient httpClient
        )
    {

        public void GetClient()
        {

            if (httpClient == null)
            {
                httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", CommonConstants.JWToken);

            }

        }


        public async Task<List<ID_UserRoles>>GetID_UserRolesAsync(string uid)
        {
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", CommonConstants.JWToken);

            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString.Add("uid", uid);

            var _ub = new UriBuilder(CommonConstants.Identity_UserRoles + $"/{uid}");
            _ub.Query = queryString.ToString();

            var response = await httpClient.GetAsync(_ub.Uri) ?? throw new Exception("Could not find Roles for user");
            //var response = await httpClient.GetAsync(Constants.Identity_UserRoles);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // NOTE: THEN TOKEN HAS EXPIRED
            {

                return null;

            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return null;
            else
            {
                string _data1 = await response.Content.ReadAsStringAsync();
                if (_data1 != "")
                {
                    var _data = await response.Content.ReadFromJsonAsync<List<ID_UserRoles>>();
                    return _data;
                }
                else return null;

            }

        }
        public async Task<List<USER_COMPANY>> GetUserCompaniesAsync(string Guid)
        {
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", CommonConstants.JWToken);

            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString.Add("Guid", Guid);

            var _ub = new UriBuilder(CommonConstants.UserCompanies);
            _ub.Query = queryString.ToString();

            var response = await httpClient.GetAsync(_ub.Uri) ?? throw new Exception("Could not find linked Companies for user");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // NOTE: THEN TOKEN HAS EXPIRED
            {

                return null;

            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return null;
            else
            {
                string _data1 = await response.Content.ReadAsStringAsync();
                if (_data1 != "")
                {
                    var _data = await response.Content.ReadFromJsonAsync<List<USER_COMPANY>>();
                    return _data;
                }
                else return null;

            }

        }

    }
}
