using Capri;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using Checkmarx.API.OSA;
using Checkmarx.API.SAST;
using System.Xml.Linq;
using CxDataRepository;
using PortalSoap;
using Scan = Checkmarx.API.SAST.Scan;

namespace Checkmarx.API
{
    /// <summary>
    /// Wrapper for accessing the Checkmarx Products
    /// </summary>
    public class CxClient : IDisposable
    {
        /// <summary>
        /// REST API client
        /// </summary>
        private HttpClient httpClient;

        /// <summary>
        /// OData client
        /// </summary>
        private Default.Container _oData;
        private DefaultV9.Container _oDataV9;
        private List<CxDataRepository.Project> _oDataProjs;

        /// <summary>
        /// SOAP client
        /// </summary>
        private PortalSoap.CxPortalWebServiceSoapClient _cxPortalWebServiceSoapClient;

        /// <summary>
        /// Check if the wrapper is connected.
        /// </summary>
        private bool Connected
        {
            get
            {
                if (httpClient == null || (_jwtSecurityToken.ValidTo - DateTime.UtcNow).TotalMinutes < 5) // login and re-logins...
                {
                    httpClient = Login(WebServerURL.ToString(), Username, Password);
                }

                return true;
            }
        }

        /// <summary>
        /// URI of the server.
        /// </summary>
        public Uri WebServerURL { get; }

        public string Username { get; }

        public string Password { get; }

        /// <summary>
        /// Localization Id
        /// </summary>
        public int LcId { get; private set; } = 1033;

        /// <summary>
        /// Constructor of the Checkmarx Client
        /// </summary>
        /// <param name="webserverAddress">Server URL, e.g. http://localhost/</param>
        /// <param name="username">The username of the user, if it is an LDAP user please put the DOMAIN\Username</param>
        /// <param name="password">The password of the user</param>
        /// <param name="lcid">Localization of the user</param>
        public CxClient(Uri webserverAddress, string username, string password, int lcid = 1033)
        {
            Username = username;
            Password = password;
            WebServerURL = webserverAddress;
            LcId = lcid;
        }

        //public CxClient(Uri webServerAddress, AuthenticationHeaderValue authenticationHeaderValue)
        //{
        //    WebServerURL = webServerAddress;
        //    AuthenticationToken = authenticationHeaderValue;
        //}

        private JwtSecurityToken _jwtSecurityToken;

        private AuthenticationHeaderValue _authenticationHeaderValue = null;

        // Cache
        private Dictionary<int, string> _presetsCache;
        private Dictionary<string, string> _teamsCache;

        private string _sessionId = "";

        /// <summary>
        /// Authentication Token for the Version REST 8.9, and all service 9.X
        /// </summary>
        protected AuthenticationHeaderValue AuthenticationToken
        {
            get
            {
                if (_authenticationHeaderValue != null)
                    return _authenticationHeaderValue;

                // Connect and get the _authentication header
                if (Connected)
                    return _authenticationHeaderValue;

                return _authenticationHeaderValue;
            }
            private set
            {
                if (_authenticationHeaderValue != value)
                {
                    _authenticationHeaderValue = value;

                    if (_authenticationHeaderValue != null)
                    {
                        var webServer = WebServerURL;
                        if (webServer.LocalPath != "cxrestapi")
                        {
                            webServer = new Uri(webServer, "/cxrestapi/");
                        }

                        httpClient = new HttpClient
                        {
                            BaseAddress = webServer
                        };

                        _jwtSecurityToken = new JwtSecurityToken(_authenticationHeaderValue.Parameter);
                        httpClient.DefaultRequestHeaders.Authorization = _authenticationHeaderValue;
                    }
                }
            }
        }
 
        private string _version;
        /// <summary>
        /// Get SAST Version
        /// </summary>
        public string Version
        {
            get => _version;
        }

        private bool _isV9 = false;

        private HttpClient Login(string baseURL = "http://localhost/cxrestapi/",
            string userName = "", string password = "")
        {
            var webServer = new Uri(baseURL);

            _cxPortalWebServiceSoapClient = new PortalSoap.CxPortalWebServiceSoapClient(
                webServer.AbsoluteUri, TimeSpan.FromSeconds(60), userName, password);

            _version = _cxPortalWebServiceSoapClient.GetVersionNumber().Version;

            _isV9 = _version.StartsWith("V 9.");

            if (webServer.LocalPath != "cxrestapi")
            {
                webServer = new Uri(webServer, "/cxrestapi/");
            }

            var httpClient = new HttpClient
            {
                BaseAddress = webServer
            };

            using (var request = new HttpRequestMessage(HttpMethod.Post, "auth/identity/connect/token"))
            {
                request.Headers.Add("Accept", "application/json");

                var values = new List<KeyValuePair<string, string>> {
                    new KeyValuePair<string, string>("username", userName),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("scope",
                    _isV9 ? "offline_access sast_api" : "sast_rest_api"),
                    new KeyValuePair<string, string>("client_id",
                    _isV9 ? "resource_owner_sast_client" : "resource_owner_client"),
                    new KeyValuePair<string, string>("client_secret", "014DF517-39D1-4453-B7B3-9930C563627C")
                };

                request.Content = new FormUrlEncodedContent(values);

                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    JObject accessToken = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);

                    string authToken = ((JProperty)accessToken.First).Value.ToString();

                    if (_isV9)
                    {
                        _cxPortalWebServiceSoapClient = new PortalSoap.CxPortalWebServiceSoapClient(baseURL, TimeSpan.FromSeconds(60), userName, password);

                        var portalChannelFactory = _cxPortalWebServiceSoapClient.ChannelFactory;
                        portalChannelFactory.UseMessageInspector(async (request, channel, next) =>
                        {
                            HttpRequestMessageProperty reqProps = new HttpRequestMessageProperty();
                            reqProps.Headers.Add("Authorization", $"Bearer {authToken}");
                            request.Properties.Add(HttpRequestMessageProperty.Name, reqProps);
                            var response = await next(request);
                            return response;
                        });

                        // ODATA V9
                        _oDataV9 = CxOData.ConnectToODataV9(webServer, authToken);
                        _oDataProjs = _oDataV9.Projects.ToList();
                    }
                    else
                    {
                        _sessionId = _cxPortalWebServiceSoapClient.Login(new PortalSoap.Credentials
                        {
                            User = userName,
                            Pass = password
                        }, LcId).SessionId;

                        // ODATA V8
                        _oData = CxOData.ConnectToOData(webServer, userName, password);
                        _oDataProjs = _oData.Projects.ToList();
                    }

                    AuthenticationToken = new AuthenticationHeaderValue("Bearer", authToken);

                    httpClient.DefaultRequestHeaders.Authorization = AuthenticationToken;

                    return httpClient;
                }

                throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
            }
        }

        public PortalSoap.CxWSResponseServerLicenseData GetLicense()
        {
            if (!Connected)
                throw new NotSupportedException();

            return _cxPortalWebServiceSoapClient.GetServerLicenseData(_sessionId);
        }

        /// <summary>
        /// Gets the excluded settings.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns>excludedFoldersPattern, excludedFilesPattern</returns>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public Tuple<string, string> GetExcludedSettings(int projectId)
        {
            if (!Connected)
                throw new NotSupportedException();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"projects/{projectId}/sourceCode/excludeSettings"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    JObject excludeSettings = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);

                    return new Tuple<string, string>(
                       (string)excludeSettings.SelectToken("excludeFoldersPattern"),
                       (string)excludeSettings.SelectToken("excludeFilesPattern"));
                }
            }

            throw new NotSupportedException();
        }

        public void SetProjectSettings(int projectId, int presetId, int engineConfigurationId, int scanActionId)
        {
            if (!Connected)
                throw new NotSupportedException();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"sast/scanSettings"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                JObject settings = new JObject
                {
                    { "projectId", projectId },
                    { "presetId", presetId },
                    { "engineConfigurationId", engineConfigurationId },
                    { "postScanActionId", scanActionId },
                };

                JObject emailNotifications = new JObject
                {
                    {
                        "failedScan",
                        new JArray
                        {
                            "test@cx.com"
                        }
                    }
                };

                settings.Add("emailNotifications", emailNotifications);

                request.Content = new StringContent(JsonConvert.SerializeObject(settings));

                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {

                }
            }

        }

        public void SetCustomFields(ProjectDetails projDetails, IEnumerable<CustomField> customFields)
        {
            if (!Connected)
                throw new NotSupportedException();

            using (var request = new HttpRequestMessage(HttpMethod.Put, $"projects/{projDetails.Id}"))
            {
                request.Headers.Add("Accept", "application/json;v=2.0");
                JObject settings = new JObject
                {
                    { "name",  projDetails.Name },
                    { "owningTeam", projDetails.TeamId.ToString() },
                    { "customFields", new JArray(
                        customFields.Select(x => new JObject
                        {
                            {  "id", x.Id },
                            {  "value", x.Value }
                        }))
                    }
                };

                request.Content = new StringContent(JsonConvert.SerializeObject(settings));

                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        public DateTimeOffset GetProjectCreationDate(long projectID)
        {
            if (!Connected)
                throw new NotSupportedException();

            return _oDataProjs.Where(x => x.Id == projectID).FirstOrDefault().CreatedDate;
        }
        
        public ProjectDetails GetProjectSettings(int projectId)
        {
            if (!Connected)
                throw new NotSupportedException();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"projects/{projectId}"))
            {
                request.Headers.Add("Accept", "application/json;v=2.0");

                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    ProjectDetails projDetails = JsonConvert.DeserializeObject<ProjectDetails>(response.Content.ReadAsStringAsync().Result);

                    return projDetails;

                }

                throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
            }
        }

        /// <summary>
        /// For V8.9 is UID, for 9.X or higher is Int.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public string GetProjectTeamId(int projectId)
        {
            if (!Connected)
                throw new NotSupportedException();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"projects/{projectId}"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    JObject excludeSettings = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);

                    return (string)excludeSettings.SelectToken("teamId");

                }

                throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
            }
        }

        public string GetProjectTeamName(int projectId)
        {
            return GetTeams()[GetProjectTeamId(projectId)];
        }

        public Dictionary<string, int> GetSASTCustomFields()
        {
            if (!Connected)
                throw new NotSupportedException();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"customFields"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                Dictionary<string, int> customFields = new Dictionary<string, int>();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string customFieldsArray = response.Content.ReadAsStringAsync().Result;

                    var projectList = (JArray)JsonConvert.DeserializeObject(customFieldsArray);

                    foreach (var item in projectList)
                    {
                        customFields.Add((string)item.SelectToken("name"),
                            (int)item.SelectToken("id"));
                    }

                    return customFields;
                }
            }

            throw new NotSupportedException();
        }

        public Dictionary<string, CustomField> GetProjectCustomFields(int projectId)
        {
            if (!Connected)
                throw new NotSupportedException();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"projects/{projectId}"))
            {
                request.Headers.Add("Accept", "application/json;v=2.0");

                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    ProjectDetails projectSettings = JsonConvert.DeserializeObject<ProjectDetails>(response.Content.ReadAsStringAsync().Result);

                    return projectSettings.CustomFields.ToDictionary(x => x.Name);
                }

                throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
            }
        }

        #region OSA

        public ICollection<Guid> GetOSAScans(int projectId)
        {
            if (!Connected)
                throw new NotSupportedException();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"osa/scans?projectId={projectId}"))
            {
                request.Headers.Add("Accept", "application/json;v=2.0");

                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                var result = new List<Guid>();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var projectList = (JArray)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);

                    foreach (var item in projectList)
                    {
                        result.Add(Guid.Parse((string)item.SelectToken("id")));
                    }

                    return result;
                }

                throw new NotSupportedException(response.ToString());
            }


        }


        /// <summary>
        /// Gets the osa results.
        /// </summary>
        /// <param name="osaScanId">The osa scan identifier.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">
        /// </exception>
        /*
         * {
  "totalLibraries": 37,
  "highVulnerabilityLibraries": 6,
  "mediumVulnerabilityLibraries": 1,
  "lowVulnerabilityLibraries": 0,
  "nonVulnerableLibraries": 30,
  "vulnerableAndUpdated": 0,
  "vulnerableAndOutdated": 7,
  "vulnerabilityScore": "High",
  "totalHighVulnerabilities": 25,
  "totalMediumVulnerabilities": 12,
  "totalLowVulnerabilities": 1 
         */
        public OSAReportDto GetOSAResults(Guid osaScanId)
        {
            if (!Connected)
                throw new NotSupportedException();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"osa/reports?scanId={osaScanId}"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject<OSAReportDto>(response.Content.ReadAsStringAsync().Result);

                    var licenses = GetOSALicenses(osaScanId).ToDictionary(x => x.Id);

                    var libraries = GetOSALibraries(osaScanId);

                    foreach (var library in libraries)
                    {
                        foreach (var item in library.Licenses)
                        {
                            if (licenses.ContainsKey(item))
                            {
                                string riskLevel = licenses[item].RiskLevel;
                                if (riskLevel == "High" || riskLevel == "Medium")
                                {
                                    result.LibrariesAtLegalRick++;
                                }
                            }
                        }

                        if (library.Outdated)
                            result.TotalNumberOutdatedVersionLibraries++;
                    }

                    return result;
                }
            }

            throw new NotSupportedException();
        }

        public IList<OSALibraryDto> GetOSALibraries(Guid osaScanId)
        {
            if (!Connected)
                throw new NotSupportedException();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"osa/libraries?scanId={osaScanId}&itemsPerPage=20000"))
            {
                request.Headers.Add("Accept", "application/json;v=2.0");

                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject<List<OSALibraryDto>>(response.Content.ReadAsStringAsync().Result);

                    return result;
                }
            }

            throw new NotSupportedException();
        }

        public IList<OSALicenseDto> GetOSALicenses(Guid osaScanId)
        {
            if (!Connected)
                throw new NotSupportedException();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"osa/licenses?scanId={osaScanId}"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject<List<OSALicenseDto>>(response.Content.ReadAsStringAsync().Result);

                    return result;
                }

                throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
            }
        }

        #endregion

        #region SAST

        public void RunSASTScan(long projectId, string comment = "")
        {
            if (!Connected)
                throw new NotSupportedException();

            var projectResponse = _cxPortalWebServiceSoapClient.GetProjectConfiguration(_sessionId, projectId);

            if (!projectResponse.IsSuccesfull)
                throw new Exception(projectResponse.ErrorMessage);

            if (projectResponse.ProjectConfig.SourceCodeSettings.SourceOrigin == PortalSoap.SourceLocationType.Local)
                throw new NotSupportedException("The location is setup for Local, we don't support it");

            using (var request = new HttpRequestMessage(HttpMethod.Post, "sast/scans"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");
                request.Headers.Add("cxOrigin", "Checkmarx.API");

                var requestBody = JsonConvert.SerializeObject(new ScanDetails
                {
                    ProjectId = projectId,
                    Comment = comment ?? string.Empty,
                    ForceScan = true,
                    IsIncremental = false,
                    IsPublic = true
                });

                using (var stringContent = new StringContent(requestBody, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;
                    HttpResponseMessage response = httpClient.SendAsync(request).Result;

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
                    }
                }
            }

            throw new NotSupportedException();
        }

        public SASTResults GetSASTResults(long sastScanId)
        {
            if (!Connected)
                throw new NotSupportedException();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"sast/scans/{sastScanId}/resultsStatistics"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var sastRes = JsonConvert.DeserializeObject<SASTResults>(response.Content.ReadAsStringAsync().Result);
                    sastRes.Id = sastScanId;
                    return sastRes;
                }

                throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
            }
        }

        /*
         * 
         * sast/scans?last={numberOfScans}&scanStatus={state}&projectId={projectId} 
         * 
         */
        public IEnumerable<int> GetSASTScans(int projectId, string scanState = "Finished", int? numberOfScans = 1)
        {
            var l = GetSASTAllScans(projectId, scanState, numberOfScans);
            return l.Select(x => int.Parse((string)x.SelectToken("id")));
        }

        public JArray GetSASTAllScans(int projectId, string scanState = null, int? numberOfScans = null)
        {
            if (!Connected)
                throw new NotSupportedException();

            string sastFilter = $"projectId={projectId}";

            if (numberOfScans != null)
                sastFilter += $"&last={numberOfScans}";

            if (!string.IsNullOrWhiteSpace(scanState))
                sastFilter += $"&scanStatus={scanState}";

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"sast/scans?{sastFilter}"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var scans = (JArray)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                    return scans;
                }

                throw new NotSupportedException(request.ToString());
            }
        }

        /// <summary>
        /// sast/scans
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="scanState"></param>
        /// <param name="numberOfScans"></param>
        /// <returns></returns>
        public ICollection<Scan> GetSASTScanSummary(int projectId, string scanState = null, int? numberOfScans = null)
        {
            if (!Connected)
                throw new NotSupportedException();

            string sastFilter = $"projectId={projectId}";

            if (numberOfScans != null)
                sastFilter += $"&last={numberOfScans}";

            if (!string.IsNullOrWhiteSpace(scanState))
                sastFilter += $"&scanStatus={scanState}";

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"sast/scans?{sastFilter}"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var objRes = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<Scan>>(objRes);
                }

                throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
            }
        }

        // get preset /sast/scanSettings/{projectId}
        public string GetSASTPreset(int projectId)
        {
            if (!Connected)
                throw new NotSupportedException();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"sast/scanSettings/{projectId}"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject<ScanSettings>(response.Content.ReadAsStringAsync().Result);

                    return GetPresets()[result.Preset.Id];
                }

                throw new NotSupportedException(response.ToString());
            }
        }

        #endregion

        /// <summary>
        /// Gets the projects.
        /// Id - Name
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Dictionary<int, string> GetProjects()
        {
            if (!Connected)
                throw new NotSupportedException();

            // List Project Now..
            var projectListResponse = httpClient.GetAsync("projects").Result;

            var result = new Dictionary<int, string>();

            if (projectListResponse.StatusCode == HttpStatusCode.OK)
            {
                string plResult = projectListResponse.Content.ReadAsStringAsync().Result;

                var projectList = (JArray)JsonConvert.DeserializeObject(plResult);

                foreach (var item in projectList)
                {
                    result.Add((int)item.SelectToken("id"),
                        (string)item.SelectToken("name"));
                }

                return result;
            }

            if (projectListResponse.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            throw new NotSupportedException(projectListResponse.Content.ReadAsStringAsync().Result);
        }

        public List<ProjectDetails> GetAllProjectsDetails()
        {
            if (!Connected)
                throw new NotSupportedException();

            // List Project Now..
            var projectListResponse = httpClient.GetAsync("projects").Result;

            if (projectListResponse.StatusCode == HttpStatusCode.OK)
            {
                string plResult = projectListResponse.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<ProjectDetails>>(plResult);
            }

            if (projectListResponse.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            throw new NotSupportedException(projectListResponse.Content.ReadAsStringAsync().Result);
        }

        public Dictionary<int, string> GetPresets()
        {
            if (!Connected)
                throw new NotSupportedException();

            if (_presetsCache != null)
                return _presetsCache;

            using (var presets = new HttpRequestMessage(HttpMethod.Get, $"sast/presets"))
            {
                presets.Headers.Add("Accept", "application/json;v=1.0");

                HttpResponseMessage presetResponse = httpClient.SendAsync(presets).Result;

                if (presetResponse.StatusCode == HttpStatusCode.OK)
                {
                    var presetArray = JsonConvert.DeserializeObject<JArray>(presetResponse.Content.ReadAsStringAsync().Result);

                    _presetsCache = presetArray.ToDictionary(x => (int)x.SelectToken("id"), x => (string)x.SelectToken("name"));

                    return _presetsCache;
                }

                throw new NotSupportedException(presetResponse.ToString());
            }
        }

        public Dictionary<string, string> GetTeams()
        {
            if (!Connected)
                throw new NotSupportedException();

            if (_teamsCache != null)
                return _teamsCache;

            using (var request = new HttpRequestMessage(HttpMethod.Get, "auth/teams"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _teamsCache = new Dictionary<string, string>();

                    foreach (var item in (JArray)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result))
                    {
                        _teamsCache.Add(
                            item.SelectToken("id").ToString(),
                            (string)item.SelectToken("fullName"));
                    }

                    return _teamsCache;
                }

                throw new NotSupportedException(request.ToString());
            }
        }
        
        #region Reports

        /// <summary>
        /// Returns a stream of the scan report.
        /// </summary>
        /// <param name="scanId"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public Stream GetScanReport(long scanId, ReportType reportType)
        {
            if (!Connected)
                throw new NotSupportedException();

            // reportType = "XML"
            // scanId = scanId

            // POST reports/sastScan
            string reportTypeString = GetReportTypeString(reportType);

            using (var request = new HttpRequestMessage(HttpMethod.Post, "reports/sastScan"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                var values = new List<KeyValuePair<string, string>> {
                    new KeyValuePair<string, string>("reportType", reportTypeString ),
                    new KeyValuePair<string, string>("scanId", scanId.ToString()),
                };

                request.Content = new FormUrlEncodedContent(values);

                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                if (response.StatusCode == HttpStatusCode.Accepted)
                {
                    JObject createReport = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);

                    long reportId = (long)createReport["reportId"];

                    while (!IsReportReady(reportId))
                    {
                        // wait 
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }

                    // Get Report
                    using (var getReport = new HttpRequestMessage(HttpMethod.Get, $"reports/sastScan/{reportId}"))
                    {
                        getReport.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue($"application/{reportTypeString}"));

                        HttpResponseMessage getReportResponse = httpClient.SendAsync(getReport).Result;

                        if (getReportResponse.StatusCode == HttpStatusCode.OK)
                        {
                            //string fileName = $"{scanId} - {reportId}.{reportType}";
                            //while (File.Exists(fileName))
                            //    File.Delete(fileName);
                            return getReportResponse.Content.ReadAsStreamAsync().Result;
                        }
                    }
                }
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// Get the report and saves it inside a file in the FileSystem.
        /// </summary>
        /// <param name="scanId"></param>
        /// <param name="reportType"></param>
        /// <param name="fullOutputFileName"></param>
        /// <returns></returns>
        public string GetScanReportFile(long scanId, ReportType reportType, string fullOutputFileName)
        {
            if (string.IsNullOrWhiteSpace(fullOutputFileName))
                throw new NotSupportedException(nameof(fullOutputFileName));

            var memoryStream = GetScanReport(scanId, reportType);
            using (FileStream fs = File.Create(fullOutputFileName))
            {
                memoryStream.CopyTo(fs);
            }

            return Path.GetFullPath(fullOutputFileName);
        }

        /// <summary>
        /// Get the XML document for the reports.
        /// </summary>
        /// <param name="scanId">Id of the scan</param>
        /// <returns></returns>
        public XDocument GetScanReport(long scanId)
        {
            return XDocument.Load(GetScanReport(scanId, ReportType.XML));
        }

        private static string GetReportTypeString(ReportType reportType)
        {
            switch (reportType)
            {
                case ReportType.XML:
                    return "XML";
                case ReportType.PDF:
                    return "PDF";
                case ReportType.CSV:
                    return "CSV";
                case ReportType.RTF:
                    return "RTF";
                default:
                    throw new NotSupportedException();
            }
        }

        public CxWSResponseScansDisplayData GetScansDisplayData(long projectId)
        {
            if (!Connected) throw new NotSupportedException();
            var res = _cxPortalWebServiceSoapClient.GetScansDisplayData(_sessionId, projectId);
            if (res.IsSuccesfull)
            {
                return res;
            }
            throw new Exception(res.ErrorMessage);
        }


        private bool IsReportReady(long reportId)
        {
            if (!Connected)
                throw new NotSupportedException();

            using (var getReportStatus = new HttpRequestMessage(HttpMethod.Get, $"reports/sastScan/{reportId}/status"))
            {
                HttpResponseMessage getReportResponse = httpClient.SendAsync(getReportStatus).Result;

                if (getReportResponse.StatusCode == HttpStatusCode.OK)
                {
                    JObject reportStatus = JsonConvert.DeserializeObject<JObject>(getReportResponse.Content.ReadAsStringAsync().Result);

                    return (string)reportStatus.SelectToken("status").SelectToken("value") == "Created";
                }
            }

            return false;
        } 
        
        #endregion

        public void Dispose()
        {
            // There is logoff...
            
        }
    }

  
}
