using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Checkmarx.API
{
    public class SCAClient
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private string _username;
        private string _password;
        private string _tenant;
        private DateTime _bearerValidTo;

        public SCA.Client ClientSCA { get; private set; }
        private const string _baseURL = "https://api-sca.checkmarx.net";
        public SCAClient(string tenant, string username, string password)
        {
            if (string.IsNullOrEmpty(tenant)) throw new ArgumentNullException(nameof(tenant));
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
            _username = username;
            _password = password;
            _tenant = tenant;
            _bearerValidTo = DateTime.UtcNow.AddHours(1);
            CreateClient();
    }

    private void CreateClient()
        {
            var token = Autenticate(_tenant, _username, _password);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ClientSCA = new SCA.Client(_httpClient);
            ClientSCA.BaseUrl = _baseURL;
        }

        public bool Connected
        {
            get
            {
                if (_httpClient == null || (_bearerValidTo - DateTime.UtcNow).TotalMinutes < 5)
                {
                    CreateClient();
                    _bearerValidTo = DateTime.UtcNow.AddHours(1);
                }

                return true;
            }
        }

        private string Autenticate(string tenant, string username, string password)
        {
            var identityURL = "https://platform.checkmarx.net/identity/connect/token";
            var kv = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "client_id", "sca_resource_owner" },
                { "scope", "sca_api" },
                { "username", username },
                { "password", password },
                { "acr_values", "Tenant:" + tenant }
            };
            var req = new HttpRequestMessage(HttpMethod.Post, identityURL) { Content = new FormUrlEncodedContent(kv) };
            var response = _httpClient.SendAsync(req).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject accessToken = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                string authToken = ((JProperty)accessToken.First).Value.ToString();
                return authToken;
            }
            throw new Exception(response.Content.ReadAsStringAsync().Result);
        }
    }
}
