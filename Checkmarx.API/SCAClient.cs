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
        private Uri _acUrl;
        private DateTime _bearerValidTo;

        private SCA.Client _clientSCA = null;

        public SCA.Client ClientSCA
        {
            get
            {
                if (_clientSCA == null || (_bearerValidTo - DateTime.UtcNow).TotalMinutes < 5)
                {
                    var token = Autenticate(_tenant, _username, _password);
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    _clientSCA = new SCA.Client(_httpClient)
                    {
                        BaseUrl = _baseURL.AbsoluteUri
                    };

                    _bearerValidTo = DateTime.UtcNow.AddHours(1);
                }
                return _clientSCA;
            }
        }

        private Uri _baseURL;

        public SCAClient(
            string tenant,
            string username,
            string password,
            string acUrl = "https://platform.checkmarx.net",
            string apiUrl = "https://api-sca.checkmarx.net")
        {
            if (string.IsNullOrEmpty(tenant)) throw new ArgumentNullException(nameof(tenant));
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrEmpty(acUrl)) throw new ArgumentNullException(nameof(acUrl));
            if (string.IsNullOrEmpty(apiUrl)) throw new ArgumentNullException(nameof(apiUrl));

            _username = username;
            _password = password;
            _tenant = tenant;
            _acUrl = new Uri(acUrl);
            _baseURL = new Uri(apiUrl);
        }

        public bool Connected
        {
            get
            {
                return ClientSCA != null;
            }
        }

        private string Autenticate(string tenant, string username, string password)
        {
            var identityURL = $"{_acUrl}identity/connect/token";
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

        public void EnableExploitablePathForAllProjects()
        {
            foreach (var project in ClientSCA.GetProjectsAsync().Result)
            {
                ClientSCA.UpdateProjectsSettingsAsync(project.Id,
                            new API.SCA.ProjectSettings { EnableExploitablePath = true }).Wait();
            }
        }
    }
}
