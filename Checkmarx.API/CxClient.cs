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
using System.Threading.Tasks;
using System.Diagnostics;

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


        private global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.Scan> soapScans => _isV9 ? _oDataV9.Scans : _oData.Scans;

        /// <summary>
        /// SOAP client
        /// </summary>
        private PortalSoap.CxPortalWebServiceSoapClient _cxPortalWebServiceSoapClient;



        private Dictionary<long, CxDataRepository.Scan> _scanCache;

        /// <summary>
        /// Get a preset for a specific scan.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetScanPreset(long id)
        {
            checkConnection();

            if (id <= 0)
            {
                throw new ArgumentException(nameof(id));
            }

            if (_scanCache == null)
            {
#if DEBUG
                var watch = new Stopwatch();
                watch.Start();
#endif

                _scanCache = soapScans.Where(x => x.IsLocked).ToDictionary(x => x.Id);

#if DEBUG
                watch.Stop();
                Console.WriteLine($"Found {_scanCache.Keys.Count} scans in {watch.Elapsed.TotalMinutes} minutes");
                Console.SetCursorPosition(0, Console.CursorTop);
#endif
            }

            if (!_scanCache.ContainsKey(id))
            {
                var scan = soapScans.Where(x => x.Id == id).FirstOrDefault();
                if (scan != null)
                {
                    _scanCache.Add(scan.Id, scan);
                    return scan.PresetName;
                }

                throw new KeyNotFoundException($"Scan with Id {id} does not exist.");
            }

            return _scanCache[id].PresetName;
        }

        /// <summary>
        /// Check if the wrapper is connected.
        /// </summary>
        public bool Connected
        {
            get
            {
                if (httpClient == null || (_jwtSecurityToken.ValidTo - DateTime.UtcNow).TotalMinutes < 5) // login and re-logins...
                {
                    httpClient = Login(SASTServerURL.ToString(), Username, Password);
                }

                return true;
            }
        }

        /// <summary>
        /// URI of the server.
        /// </summary>
        public Uri SASTServerURL { get; }

        public string Username { get; }

        public string Password { get; }

        /// <summary>
        /// Localization Id
        /// </summary>
        public int LcId { get; private set; } = 1033;

        /// <summary>
        /// Constructor of the Checkmarx Client
        /// </summary>
        /// <param name="sastServerAddress">Server URL, e.g. http://localhost/</param>
        /// <param name="username">The username of the user, if it is an LDAP user please put the DOMAIN\Username</param>
        /// <param name="password">The password of the user</param>
        /// <param name="lcid">Localization of the user</param>
        public CxClient(Uri sastServerAddress, string username, string password, int lcid = 1033)
        {
            Username = username;
            Password = password;
            SASTServerURL = sastServerAddress;
            LcId = lcid;
        }

        //public CxClient(Uri webServerAddress, AuthenticationHeaderValue authenticationHeaderValue)
        //{
        //    WebServerURL = webServerAddress;
        //    AuthenticationToken = authenticationHeaderValue;
        //}

        private JwtSecurityToken _jwtSecurityToken;

        private AuthenticationHeaderValue _authenticationHeaderValue = null;

        private string _soapSessionId = "";

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
                        var webServer = SASTServerURL;
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
            get
            {
                checkConnection();
                return _version;
            }
        }

        private bool _isV9 = false;

        #region Access Control 

        private HttpClient Login(string baseURL = "http://localhost/cxrestapi/",
            string userName = "", string password = "")
        {
            var webServer = new Uri(baseURL);

            Uri baseServer = new Uri(webServer.AbsoluteUri);

            _cxPortalWebServiceSoapClient = new PortalSoap.CxPortalWebServiceSoapClient(
                baseServer, TimeSpan.FromSeconds(60), userName, password);

            _version = _cxPortalWebServiceSoapClient.GetVersionNumber().Version;

            _isV9 = _version.StartsWith("V 9.");

            var httpClient = new HttpClient
            {
                BaseAddress = webServer
            };

            if (httpClient.BaseAddress.LocalPath != "cxrestapi")
            {
                httpClient.BaseAddress = new Uri(webServer, "/cxrestapi/");
            }

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
                        _cxPortalWebServiceSoapClient = new PortalSoap.CxPortalWebServiceSoapClient(baseServer, TimeSpan.FromSeconds(60), userName, password);

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
                        _soapSessionId = _cxPortalWebServiceSoapClient.Login(new PortalSoap.Credentials
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

        public string GetProjectTeamName(string teamId)
        {
            return GetTeams()[teamId];
        }

        public PortalSoap.CxWSResponseServerLicenseData GetLicense()
        {
            if (!Connected)
                throw new NotSupportedException();

            return _cxPortalWebServiceSoapClient.GetServerLicenseData(_soapSessionId);
        }

        // Cache
        private Dictionary<string, string> _teamsCache;

        public string GetProjectTeamName(int projectId)
        {
            return GetProjectTeamName(GetProjectTeamId(projectId));
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
                        if (!_teamsCache.ContainsKey(item.SelectToken("id").ToString()))
                        {
                            _teamsCache.Add(
                               item.SelectToken("id").ToString(),
                               (string)item.SelectToken("fullName"));
                        }
                    }

                    return _teamsCache;
                }

                throw new NotSupportedException(request.ToString());
            }
        }

      

        public IQueryable<CxDataRepository.Scan> GetScansFromOData(long projectId)
        {
            checkConnection();
            return soapScans.Expand(x => x.ScannedLanguages).Where(x => x.ProjectId == projectId);
        }

        #endregion

        private void checkConnection()
        {
            if (!Connected)
                throw new NotSupportedException();
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
            checkConnection();

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
            checkConnection();

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

        /// <summary>
        /// Creates Manage Fields in SAST.
        /// </summary>
        /// <param name="fieldNames">Name of the Fields</param>
        public void CreateCustomField(params string[] fieldNames)
        {
            if (fieldNames == null)
                throw new ArgumentNullException(nameof(fieldNames));

            checkConnection();

            var existingFields = GetSASTCustomFields();
            if (existingFields.Count >= 10 || (existingFields.Count + fieldNames.Count()) > 10)
            {
                throw new Exception("Only 10 custom fields can be defined.");
            }

            var fields = new List<PortalSoap.CxWSCustomField>();
            foreach (var name in fieldNames)
            {
                fields.Add(new PortalSoap.CxWSCustomField { Name = name });
            }

            var result = _cxPortalWebServiceSoapClient.SaveCustomFields(_soapSessionId, fields.ToArray());
            if (!result.IsSuccesfull)
            {
                throw new Exception(result.ErrorMessage);
            }
        }

        public void SetCustomFields(ProjectDetails projDetails, IEnumerable<CustomField> customFields)
        {
            checkConnection();

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
            checkConnection();

            return _oDataProjs.Where(x => x.Id == projectID).First().CreatedDate;
        }

        public ProjectDetails GetProjectSettings(int projectId)
        {
            checkConnection();

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
            checkConnection();

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

        public Dictionary<string, int> GetSASTCustomFields()
        {
            checkConnection();

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
            checkConnection();

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
            checkConnection();

            var projectResponse = _cxPortalWebServiceSoapClient.GetProjectConfiguration(_soapSessionId, projectId);

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
            checkConnection();

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
            checkConnection();

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
            checkConnection();

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

        private enum ScanRetrieveKind
        {
            First,
            Last,
            Locked,
            All
        }

        public List<Scan> GetAllSASTScans(long projectId)
        {
            var scans = GetScans(projectId, true, ScanRetrieveKind.All);
            return scans;
        }
        public Scan GetFirstScan(long projectId)
        {
            var scan = GetScans(projectId, true, ScanRetrieveKind.First);
            return scan.FirstOrDefault();
        }

        public Scan GetLastScan(long projectId)
        {
            var scan = GetScans(projectId, true, ScanRetrieveKind.Last);
            return scan.FirstOrDefault();
        }

        public Scan GetLockedScan(long projectId)
        {
            var scan = GetScans(projectId, true, ScanRetrieveKind.First);
            return scan.FirstOrDefault();
        }

        public int GetScanCount()
        {
            checkConnection();
            return soapScans.Count();
        }

        private List<Scan> GetScans(long projectId, bool finished,
            ScanRetrieveKind scanKind = ScanRetrieveKind.All)
        {
            IQueryable<CxDataRepository.Scan> scans = soapScans.Where(x => x.ProjectId == projectId);
            switch (scanKind)
            {
                case ScanRetrieveKind.First:
                    scans = scans.Take(1);
                    break;
                case ScanRetrieveKind.Last:
                    scans = scans.Skip(Math.Max(0, scans.Count() - 1));
                    break;

                case ScanRetrieveKind.Locked:
                    scans = scans.Where(x => x.IsLocked);
                    break;
                case ScanRetrieveKind.All:
                    break;
            }

            var ret = new List<Scan>();
            foreach (var scan in scans)
            {
                // 1 - finished
                // 3 - unfinished

                if (finished && scan.ScanType == 3)
                {
                    continue;
                }
                var currentScan = ConvertScanFromOData(scan);

                ret.Add(currentScan);
            }

            return ret;
        }

        private static Scan ConvertScanFromOData(CxDataRepository.Scan scan)
        {
            return new Scan
            {
                Comment = scan.Comment,
                Id = scan.Id,
                IsLocked = scan.IsLocked,
                ScanState = new ScanState
                {
                    LanguageStateCollection = scan.ScannedLanguages.Select(language => new LanguageStateCollection
                    {
                        LanguageName = language.LanguageName
                    }).ToList(),

                    LinesOfCode = scan.LOC.GetValueOrDefault()
                },
                Origin = scan.Origin,
                ScanRisk = scan.RiskScore,
                DateAndTime = new DateAndTime
                {
                    EngineFinishedOn = scan.EngineFinishedOn,
                    EngineStartedOn = scan.EngineStartedOn
                },
                Results = new SASTResults
                {
                    High = (uint)scan.High,
                    Medium = (uint)scan.Medium,
                    Low = (uint)scan.Low,
                    FailedLoC = (int)scan.FailedLOC.GetValueOrDefault(),
                    LoC = (int)scan.LOC.GetValueOrDefault()
                }
            };
        }

        public Scan GetScanById(long scanId)
        {
            checkConnection();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"sast/scans/{scanId}"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var objRes = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<Scan>(objRes);
                }

                throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
            }
        }

        // get preset /sast/scanSettings/{projectId}
        public string GetSASTPreset(int projectId)
        {
            checkConnection();

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
            checkConnection();

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
            checkConnection();

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


        private Dictionary<int, string> _presetsCache;
        public Dictionary<int, string> GetPresets()
        {
            checkConnection();

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

#region Reports

        /// <summary>
        /// Returns the ScanId of a finished scan.
        /// </summary>
        /// <param name="scanId"></param>
        /// <returns></returns>
        public byte[] GetScanLogs(long scanId)
        {
            checkConnection();

            if (scanId <= 0)
                throw new ArgumentOutOfRangeException(nameof(scanId));


            var objectName = new CxWSRequestScanLogFinishedScan
            {
                SessionID = _soapSessionId,
                ScanId = scanId
            };

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(objectName.GetType());

            StringBuilder sb = new StringBuilder();
            using (StringWriter xmlWriter = new StringWriter(sb))
            {
                x.Serialize(xmlWriter, objectName);
            }

            Console.WriteLine(sb.ToString());

            var result = _cxPortalWebServiceSoapClient.GetScanLogs(objectName);



            if (!result.IsSuccesfull)
                throw new ActionNotSupportedException(result.ErrorMessage);

            return result.ScanLog;
        }

        /// <summary>
        /// Returns a stream of the scan report.
        /// </summary>
        /// <param name="scanId"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public Stream GetScanReport(long scanId, ReportType reportType)
        {
            checkConnection();

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
            var res = _cxPortalWebServiceSoapClient.GetScansDisplayData(_soapSessionId, projectId);
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

#region Results

        public CxWSSingleResultData[] GetResultsForScan(long scanId)
        {
            checkConnection();

            var result = _cxPortalWebServiceSoapClient.GetResultsForScan(_soapSessionId, scanId);

            if (!result.IsSuccesfull)
                throw new ApplicationException(result.ErrorMessage);

            return result.Results;
        }

        /// <summary>
        /// Get the all query groups of the CxSAST server.
        /// </summary>
        /// <returns></returns>
        public CxWSQueryGroup[] GetQueries()
        {
            checkConnection();

            var result = _cxPortalWebServiceSoapClient.GetQueryCollection(_soapSessionId);

            if (!result.IsSuccesfull)
                throw new ApplicationException(result.ErrorMessage);

            return result.QueryGroups;
           
        }

        /// <summary>
        /// Comment Separator.
        /// </summary>
        public static char CommentSeparator
        {
            get
            {
                return Convert.ToChar(255);
            }
        }


        public void GetCommentsHistoryTest(long scanId)
        {
            checkConnection();

            var response = _cxPortalWebServiceSoapClient.GetResultsForScan(_soapSessionId, scanId);

            // Assert.IsTrue(response.IsSuccesfull);

            foreach (var item in response.Results)
            {
                //Console.WriteLine(item.State);

                if (!string.IsNullOrWhiteSpace(item.Comment))
                {
                    var commentsResults = _cxPortalWebServiceSoapClient.GetPathCommentsHistory(_soapSessionId, scanId, item.PathId,
                         ResultLabelTypeEnum.Remark);

                    foreach (var comment in commentsResults.Path.Comment.Split(new[] { CommentSeparator },
                        StringSplitOptions.RemoveEmptyEntries))
                    {
                        string commentText = comment.Substring(comment.IndexOf(']') + 3);

                        Console.WriteLine(string.Join(";",
                            toSeverityToString(item.Severity), toResultStateToString((ResultState)item.State), commentText));
                    }
                }
            }
        }

        public enum ResultState : int
        {
            ToVerify = 0,
            NonExploitable = 1,
            Confirmed = 2,
            Urgent = 3,
            ProposedNotExploitable = 4
        }


        /// <summary>
        /// Get the HTML Query Description (?)
        /// </summary>
        /// <param name="cwe">CWE Id</param>
        /// <returns>For 0 returns empty, for less than 0 throws an exception, for the other an HTML Query Description</returns>
        public string GetCWEDescription(long cwe)
        {
            if (cwe < 0)
                throw new ArgumentOutOfRangeException(nameof(cwe));

            if (cwe == 0)
                return string.Empty;

            checkConnection();

            var result = _cxPortalWebServiceSoapClient.GetQueryDescription(_soapSessionId, (int)cwe);
            if (!result.IsSuccesfull)
                throw new Exception(result.ErrorMessage);

            return result.QueryDescription;
        }


        private static string toResultStateToString(ResultState state)
        {
            switch (state)
            {
                case ResultState.ToVerify:
                    return "To Verify";
                case ResultState.NonExploitable:
                    return "Non Exploitable";
                case ResultState.Confirmed:
                    return "Confirmed";
                case ResultState.Urgent:
                    return "Urgent";
                case ResultState.ProposedNotExploitable:
                    return "Proposed Not Exploitable";
                default:
                    throw new NotImplementedException();
            }
        }

        private static string toSeverityToString(int severity)
        {
            switch (severity)
            {
                case 0:
                    return "Low";
                case 1:
                    return "Low";
                case 2:
                    return "Medium";
                case 3:
                    return "High";
                default:
                    throw new NotImplementedException();
            }
        }

#endregion


        public void Dispose()
        {
            // There is logoff...

        }
    }


}
