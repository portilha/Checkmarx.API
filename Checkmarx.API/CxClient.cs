using Capri;
using Checkmarx.API.Exceptions;
using Checkmarx.API.Interfaces;
using Checkmarx.API.Models;
using Checkmarx.API.OSA;
using Checkmarx.API.SAST;
using Checkmarx.API.SAST.OData;
using Checkmarx.API.SASTV2;
using Checkmarx.API.SASTV2_1;
using Checkmarx.API.SASTV3;
using Checkmarx.API.SASTV4;
using Checkmarx.API.SASTV5;
using Checkmarx.API.SASTV5_1;
using Checkmarx.API.SASTV6;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using Polly.Timeout;
using PortalSoap;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Result = CxDataRepository.Result;
using Scan = Checkmarx.API.SAST.Scan;

namespace Checkmarx.API
{
    /// <summary>
    /// Wrapper for accessing the Checkmarx Products
    /// </summary>
    public class CxClient : IDisposable
    {
        public string Origin { get; set; } = "Checkmarx.API";

        private readonly int _defaultRetries = 10;
        internal static SOAPRetryPolicyProvider _soapRetryPolicyProvider;
        internal static IAsyncPolicy<HttpResponseMessage> _retryPolicy;

        public async Task<(HttpClient client, HttpResponseMessage response)> ExecuteWithRetryAsync(HttpClient client, HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await RESTRetryPolicyProvider.ExecuteWithPolicyAsync(_retryPolicy, client, request, cancellationToken);
                return (client, response);
            }
            catch (TimeoutRejectedException)
            {
                Console.WriteLine($"Timeout Rejected Exception: Refreshing connection...");

                client = Login(SASTServerURL.ToString(), Username, Password);

                var response = await RESTRetryPolicyProvider.ExecuteWithPolicyAsync(_retryPolicy, client, request, cancellationToken);
                return (client, response);
            }
        }

        #region Clients

        /// <summary>
        /// REST API client
        /// </summary>
        private HttpClient httpClient;

        /// <summary>
        /// OData client
        /// </summary>
        private Default.ODataClient8 _oData;

        private DefaultV9.Container _oDataV9;

        private ODataClient95 _oDataV95;

        private CxDataRepository97.Container _oDataV97;

        /// <summary>
        /// Returns the interface for the OData of SAST starting at V9.4 or higher
        /// </summary>
        public ODataClient95 ODataV95
        {
            get
            {
                checkConnection();

                if (_oDataV95 == null)
                    throw new NotSupportedException($"The SAST version  should be 9.4 or higher to support this OData interface, it is {Version.ToString()}");

                return _oDataV95;
            }
        }

        /// <summary>
        /// Returns the interface for the OData of SAST starting at V9.7 or higher
        /// </summary>
        public CxDataRepository97.Container ODataV97
        {
            get
            {
                checkConnection();

                if (_oDataV97 == null)
                    throw new NotSupportedException($"The SAST version  should be 9.4 or higher to support this OData interface, it is {Version.ToString()}");

                return _oDataV97;
            }
        }

        private List<CxDataRepository.Project> _odp;
        public List<CxDataRepository.Project> ProjectsOData
        {
            get
            {
                checkConnection();

                if (_odp == null)
                {
                    _odp = _oDataProjects.ToList();
                }
                return _odp;
            }
        }

        private TimeSpan? _serverDateTimeOffSet;
        public TimeSpan ServerDateTimeOffSet
        {
            get
            {
                if (_serverDateTimeOffSet == null)
                {
                    checkConnection();

                    var project = _oDataProjects.FirstOrDefault();
                    if (project == null)
                        throw new Exception("There are no conditions to get client DateTime Offset.");

                    _serverDateTimeOffSet = project.CreatedDate.Offset;
                }

                return _serverDateTimeOffSet.Value;
            }
        }

        private RetryableDataServiceQuery<global::CxDataRepository.Project> _oDataProjects => _isV9 ? _oDataV9.Projects : _oData.Projects;

        private RetryableDataServiceQuery<global::CxDataRepository.Scan> _oDataScans => _isV9 ? _oDataV9.Scans.Expand(x => x.ScannedLanguages) : _oData.Scans.Expand(x => x.ScannedLanguages);

        private RetryableDataServiceQuery<global::CxDataRepository97.Scan> _oData97Scans => _isV97 ? _oDataV97.Scans.Expand(x => x.ScannedLanguages) : null;

        private RetryableDataServiceQuery<global::CxDataRepository.Result> _oDataResults => _isV9 ? _oDataV9.Results : _oData.Results;

        private RetryableDataServiceQuery<Checkmarx.API.SAST.OData.Result> _oDataV95Results => _isV95 ? _oDataV95.Results : null;

        private RetryableDataServiceQuery<global::CxDataRepository97.Result> _oDataV97Results => _isV97 ? _oDataV97.Results : null;

        public IEnumerable<CxDataRepository97.Result> GetODataResults(long scanId, bool includeScan = false, bool includeQuery = false)
        {
            checkConnection();

            if (_isV97)
            {
                var results = _oDataV97Results;

                if (includeScan)
                    results = results.Expand(x => x.Scan);

                if (includeQuery)
                    results = results.Expand(x => x.Query).Expand(x => x.Query.QueryCxDescription);

                return results.Where(x => x.ScanId == scanId);
            }
            else if (_isV95)
            {
                var results = _oDataV95Results;

                if (includeScan)
                    results = results.Expand(x => x.Scan);

                if (includeQuery)
                    results = results.Expand(x => x.Query).Expand(x => x.Query.QueryCxDescription);

                return Mapper.MapOdataResults(results.Where(x => x.ScanId == scanId));
            }
            else
            {
                var results = _oDataResults;

                if (includeScan)
                    results = results.Expand(x => x.Scan);

                if (includeQuery)
                    results = results.Expand(x => x.Query).Expand(x => x.Query.QueryCxDescription);

                return Mapper.MapOdataResults(results.Where(x => x.ScanId == scanId));
            }
        }

        public IEnumerable<CxDataRepository97.Scan> GetScansFromOData(long projectId)
        {
            checkConnection();

            if (_isV97)
                return _oData97Scans.Where(x => x.ProjectId == projectId);
            else
                return Mapper.MapOdataScans(_oDataScans.Where(x => x.ProjectId == projectId));
        }

        /// <summary>
        /// SOAP client
        /// </summary>
        private PortalSoap.CxPortalWebServiceSoapClient _cxPortalWebServiceSoapClient;

        private cxPortalWebService93.CxPortalWebServiceSoapClient _cxPortalWebServiceSoapClientV9;

        private cxPriorityWebService.CxPriorityServiceSoapClient _cxPriorityServiceSoapClient;


        public cxPortalWebService93.CxPortalWebServiceSoapClient PortalSOAP
        {
            get
            {
                checkConnection();
                return _cxPortalWebServiceSoapClientV9;
            }
        }


        #region CxAudit

        private CxAuditWebServiceV9.CxAuditWebServiceSoapClient _cxAuditWebServiceSoapClientV9 = null;


        public CxAuditWebServiceV9.CxAuditWebServiceSoapClient CxAuditV9
        {
            get
            {
                checkConnection();

                if (_cxAuditWebServiceSoapClientV9 == null)
                {
                    _cxAuditWebServiceSoapClientV9 = new CxAuditWebServiceV9.CxAuditWebServiceSoapClient(SASTServerURL, TimeSpan.FromSeconds(360), Username, Password);

                    _cxAuditWebServiceSoapClientV9.ClientCredentials.ServiceCertificate.SslCertificateAuthentication =
                                new System.ServiceModel.Security.X509ServiceCertificateAuthentication
                                {
                                    CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None,
                                    RevocationMode = X509RevocationMode.NoCheck
                                };

                    var portalChannelFactory = _cxAuditWebServiceSoapClientV9.ChannelFactory;
                    portalChannelFactory.UseMessageInspector(async (request, channel, next) =>
                    {


                        HttpRequestMessageProperty reqProps = new HttpRequestMessageProperty();
                        reqProps.Headers.Add("Authorization", $"Bearer {AuthenticationToken.Parameter}");
                        request.Properties.Add(HttpRequestMessageProperty.Name, reqProps);
                        var response = await next(request);
                        return response;
                    });
                }

                return _cxAuditWebServiceSoapClientV9;
            }
        }


        #endregion

        private SASTRestClient _sastClient;

        /// <summary>
        /// Interface to all SAST/OSA REST methods.
        /// </summary>
        /// <remarks>Supports only V1</remarks>
        public SASTRestClient SASTClient
        {
            get
            {
                checkConnection();

                if (_sastClient == null)
                    _sastClient = new SASTRestClient(httpClient.BaseAddress.AbsoluteUri, httpClient);

                return _sastClient;
            }
        }

        private bool? _supportsV1_1 = null;

        public bool SupportsV1_1
        {
            get
            {
                if (_supportsV1_1 == null)
                {
                    checkConnection();
                    _supportsV1_1 = SupportsRESTAPIVersion("1.1");
                }
                return _supportsV1_1.Value;
            }
        }

        private SASTV1_1 _sastClientV1_1;

        /// <summary>
        /// Interface to all SAST/OSA REST methods.
        /// </summary>
        /// <remarks>Supports only V1.1</remarks>
        public SASTV1_1 SASTClientV1_1
        {
            get
            {
                if (!SupportsV1_1)
                    return null;

                if (_sastClientV1_1 == null)
                    _sastClientV1_1 = new SASTV1_1(this, httpClient);

                return _sastClientV1_1;
            }
        }


        private bool? _supportsV2_2 = null;

        public bool SupportsV2_2
        {
            get
            {
                if (_supportsV2_2 == null)
                {
                    checkConnection();
                    _supportsV2_2 = SupportsRESTAPIVersion("2.2");
                }
                return _supportsV2_2.Value;
            }
        }

        private bool? _supportsV2_1 = null;

        public bool SupportsV2_1
        {
            get
            {
                if (_supportsV2_1 == null)
                {
                    checkConnection();
                    _supportsV2_1 = SupportsRESTAPIVersion("2.1");
                }
                return _supportsV2_1.Value;
            }
        }

        private SASTV2_1Client _sastClientV2_1;

        /// <summary>
        /// Interface to all SAST/OSA REST methods.
        /// </summary>
        /// <remarks>Supports only V2.1</remarks>
        public SASTV2_1Client SASTClientV2_1
        {
            get
            {
                if (!SupportsV2_1)
                    return null;

                if (_sastClientV2_1 == null)
                    _sastClientV2_1 = new SASTV2_1Client(this, httpClient);

                return _sastClientV2_1;
            }
        }

        private bool? _supportsV2 = null;

        public bool SupportsV2
        {
            get
            {
                if (_supportsV2 == null)
                {
                    checkConnection();
                    _supportsV2 = SupportsRESTAPIVersion("2");
                }
                return _supportsV2.Value;
            }
        }

        private SASTV2Client _sastClientV2;

        /// <summary>
        /// Interface to all SAST/OSA REST methods.
        /// </summary>
        /// <remarks>Supports only V2</remarks>
        public SASTV2Client SASTClientV2
        {
            get
            {
                if (!SupportsV2)
                    return null;

                if (_sastClientV2 == null)
                    _sastClientV2 = new SASTV2Client(this, httpClient);

                return _sastClientV2;
            }
        }

        private bool? _supportsV3 = null;
        public bool SupportsV3
        {
            get
            {
                if (_supportsV3 == null)
                {
                    checkConnection();
                    _supportsV3 = SupportsRESTAPIVersion("3");
                }
                return _supportsV3.Value;
            }
        }

        private SASTV3Client _sastClientV3;

        /// <summary>
        /// Interface to SAST V4 Client.
        /// </summary>
        /// <remarks>Supports only V4</remarks>
        public SASTV3Client SASTClientV3
        {
            get
            {
                if (!SupportsV3)
                    return null;

                if (_sastClientV3 == null)
                    _sastClientV3 = new SASTV3Client(this, httpClient);

                return _sastClientV3;
            }
        }

        private bool? _supportsV4 = null;

        public bool SupportsV4
        {
            get
            {
                if (_supportsV4 == null)
                {
                    checkConnection();
                    _supportsV4 = SupportsRESTAPIVersion("4");
                }
                return _supportsV4.Value;
            }
        }

        private SASTV4Client _sastClientV4;

        /// <summary>
        /// Interface to SAST V4 Client.
        /// </summary>
        /// <remarks>Supports only V4</remarks>
        public SASTV4Client SASTClientV4
        {
            get
            {
                if (!SupportsV4)
                    return null;

                if (_sastClientV4 == null)
                    _sastClientV4 = new SASTV4Client(this, httpClient);

                return _sastClientV4;
            }
        }

        private bool? _supportsV5 = null;

        public bool SupportsV5
        {
            get
            {
                if (_supportsV5 == null)
                {
                    checkConnection();
                    _supportsV5 = SupportsRESTAPIVersion("5");
                }
                return _supportsV5.Value;
            }
        }

        private SASTV5Client _sastClientV5;

        /// <summary>
        /// Interface to SAST V5 Client.
        /// </summary>
        /// <remarks>Supports only V5</remarks>
        public SASTV5Client SASTClientV5
        {
            get
            {
                if (!SupportsV5)
                    return null;

                if (_sastClientV5 == null)
                    _sastClientV5 = new SASTV5Client(this, httpClient);

                return _sastClientV5;
            }
        }

        private bool? _supportsV5_1 = null;

        public bool SupportsV5_1
        {
            get
            {
                if (_supportsV5_1 == null)
                {
                    checkConnection();
                    _supportsV5_1 = SupportsRESTAPIVersion("5.1");
                }
                return _supportsV5_1.Value;
            }
        }

        private SASTV5_1Client _sastClientV5_1;

        /// <summary>
        /// Interface to SAST V5.1 Client.
        /// </summary>
        /// <remarks>Supports only V5.1</remarks>
        public SASTV5_1Client SASTClientV5_1
        {
            get
            {
                if (!SupportsV5_1)
                    return null;

                if (_sastClientV5_1 == null)
                    _sastClientV5_1 = new SASTV5_1Client(this, httpClient);

                return _sastClientV5_1;
            }
        }

        private bool? _supportsV6 = null;

        public bool SupportsV6
        {
            get
            {
                if (_supportsV6 == null)
                {
                    checkConnection();
                    _supportsV6 = SupportsRESTAPIVersion("6");
                }
                return _supportsV6.Value;
            }
        }

        private SASTV6Client _sastClientV6;

        /// <summary>
        /// Interface to SAST V6 Client.
        /// </summary>
        /// <remarks>Supports only V6</remarks>
        public SASTV6Client SASTClientV6
        {
            get
            {
                if (!SupportsV6)
                    return null;

                if (_sastClientV6 == null)
                    _sastClientV6 = new SASTV6Client(this, httpClient);

                return _sastClientV6;
            }
        }

        private Dictionary<long, CxDataRepository.Scan> _scanCache;

        public cxPortalWebService93.CxWSResponceScanCompareResults GetCompareScanResultsAsync(long previousScanId, long newScanId)
        {
            return PortalSOAP.GetCompareScanResultsAsync(_soapSessionId, previousScanId, newScanId).Result;
        }

        public cxPortalWebService93.CxWSResponseScanCompareSummary GetScanCompareSummaryAsync(long previousScanId, long newScanId)
        {
            return PortalSOAP.GetScanCompareSummaryAsync(_soapSessionId, previousScanId, newScanId).Result;
        }

        public cxPortalWebService93.CxWSResponceResultPath GetScanResultPath(long scanId, long pathId)
        {
            return PortalSOAP.GetResultPathAsync(_soapSessionId, scanId, pathId).Result;
        }

        /// <summary>
        /// GET Generic Request... ODATA/REST/SOAP
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public T GetRequest<T>(string requestUri)
        {
            checkConnection();

            try
            {
                var uri = new Uri(SASTServerURL, requestUri);
                if (!_isV9 && uri.LocalPath.ToLowerInvariant().StartsWith("/cxwebinterface"))
                {
                    var byteArray = Encoding.ASCII.GetBytes($"{Username}:{Password}");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                }

                using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
                {
                    var result = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                    HttpResponseMessage response = result.response;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                    }

                    throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
                }
            }
            finally
            {
                httpClient.DefaultRequestHeaders.Authorization = AuthenticationToken;
            }
        }

        #endregion

        #region Credentials

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

        public Uri GetProjectSummaryLink(long projectId)
        {
            return new Uri($"{SASTServerURL}CxWebClient/portal#/projectState/{projectId}/Summary");
        }

        public Uri GetProjectScansLink(long projectId)
        {
            return new Uri($"{SASTServerURL}CxWebClient/projectscans.aspx?id={projectId}");
        }


        public Uri GetScanLink(long projectId, long scanId)
        {
            return new Uri($"{SASTServerURL}CxWebClient/ViewerMain.aspx?scanId={scanId}&ProjectID={projectId}");
        }

        public Uri GetVulnerabilityLink(long projectId, long scanId, long pathId)
        {
            return new Uri($"{SASTServerURL}CxWebClient/ViewerMain.aspx?scanId={scanId}&ProjectID={projectId}&pathid={pathId}");
        }

        #endregion

        /// <summary>
        /// Constructor of the Checkmarx Client
        /// </summary>
        /// <param name="sastServerAddress">Server URL, e.g. http://localhost/</param>
        /// <param name="username">The username of the user, if it is an LDAP user please put the DOMAIN\Username</param>
        /// <param name="password">The password of the user</param>
        /// <param name="lcid">Localization of the user</param>
        /// <param name="defaultRetries">Max number of retries on API calls</param>
        public CxClient(Uri sastServerAddress, string username, string password, int lcid = 1033, int defaultRetries = 10)
        {
            Username = username;
            Password = password;
            SASTServerURL = sastServerAddress;
            LcId = lcid;

            if (defaultRetries <= 0)
                throw new ArgumentOutOfRangeException(nameof(defaultRetries));

            _defaultRetries = defaultRetries;
            _soapRetryPolicyProvider = new SOAPRetryPolicyProvider(_defaultRetries);
            _retryPolicy = RESTRetryPolicyProvider.CreateRetryPolicy(_defaultRetries);
        }

        private JwtSecurityToken _jwtSecurityToken;

        private AuthenticationHeaderValue _authenticationHeaderValue = null;

        private string _soapSessionId = "";

        /// <summary>
        /// Authentication Token for the Version REST 8.9, and all service 9.X
        /// </summary>
        public AuthenticationHeaderValue AuthenticationToken
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
                        _jwtSecurityToken = new JwtSecurityToken(_authenticationHeaderValue.Parameter);
                    }
                }
            }
        }

        private Version _version;
        /// <summary>
        /// Get SAST Version
        /// </summary>
        public Version Version
        {
            get
            {
                checkConnection();
                return _version;
            }
        }

        private bool _isV9 = false;
        private bool _isV94 = false;
        private bool _isV95 = false;
        private bool _isV97 = false;

        public bool IsV94 { get { return _isV94; } }
        public bool IsV95 { get { return _isV95; } }
        public bool IsV97 { get { return _isV97; } }

        #region Access Control 

        private AccessControlClient _ac = null;
        public AccessControlClient AC
        {
            get
            {
                if (_ac == null)
                {
                    checkConnection();

                    _ac = new AccessControlClient(httpClient);
                }
                return _ac;
            }
        }

        /// <summary>
        /// Returns the version of the Checkmarx.
        /// </summary>
        /// <param name="baseURL"></param>
        /// <returns></returns>
        public static string GetVersionWithoutConnecting(string baseURL)
        {
            var webServer = new Uri(baseURL);

            Uri baseServer = new Uri(webServer.AbsoluteUri);

            var cxPortalWebServiceSoapClient = new PortalSoap.CxPortalWebServiceSoapClient(
              baseServer, TimeSpan.FromSeconds(60), "dummy", "dummy");

            return cxPortalWebServiceSoapClient.GetVersionNumber().Version;
        }

        private HttpClient Login(string baseURL = "http://localhost/cxrestapi/",
            string userName = "", string password = "", bool ignoreCertificate = true)
        {
            string portalVersion = null;
            if (!ignoreCertificate)
                portalVersion = GetVersionWithoutConnecting(baseURL);

            var webServer = new Uri(baseURL);

            Uri baseServer = new Uri(webServer.AbsoluteUri);

            _cxPortalWebServiceSoapClient = new PortalSoap.CxPortalWebServiceSoapClient(
                baseServer, TimeSpan.FromSeconds(60), userName, password);

            if (ignoreCertificate)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                _cxPortalWebServiceSoapClient.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new System.ServiceModel.Security.X509ServiceCertificateAuthentication()
                {
                    CertificateValidationMode = X509CertificateValidationMode.None,
                    RevocationMode = X509RevocationMode.NoCheck
                };
            }

            // Get version number with regex
            if (string.IsNullOrWhiteSpace(portalVersion))
                portalVersion = _cxPortalWebServiceSoapClient.GetVersionNumber().Version;

            if (string.IsNullOrWhiteSpace(portalVersion))
                throw new Exception("Error fetching CxSAST portal version");

            string pattern = @"\d+(\.\d+)+";
            Regex rg = new Regex(pattern);
            Match m = rg.Match(portalVersion);
            string version = m.Value;

            _version = new Version(version);

            _isV9 = _version.Major >= 9;

            if (_isV9)
            {
                _isV94 = _version.Minor >= 4;
                _isV95 = _version.Minor >= 5;
                _isV97 = _version.Minor >= 7;
            }

            Console.WriteLine("Checkmarx " + _version.ToString());

            HttpClientHandler httpClientHandler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                MaxConnectionsPerServer = 10
            };

            // Ignore certificate for http client
            if (ignoreCertificate)
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            }

            httpClient = new HttpClient(httpClientHandler)
            {
                BaseAddress = webServer,
                Timeout = TimeSpan.FromMinutes(60)
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
                    _isV9 ? "offline_access sast_api access_control_api" : "sast_rest_api"),
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

                    _cxAuditWebServiceSoapClientV9 = null;

                    if (_isV9)
                    {
                        #region V8 SOAP

                        _cxPortalWebServiceSoapClient = new PortalSoap.CxPortalWebServiceSoapClient(baseServer, TimeSpan.FromSeconds(360), userName, password);

                        if (ignoreCertificate)
                        {
                            _cxPortalWebServiceSoapClient.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new System.ServiceModel.Security.X509ServiceCertificateAuthentication()
                            {
                                CertificateValidationMode = X509CertificateValidationMode.None,
                                RevocationMode = X509RevocationMode.NoCheck
                            };
                        }

                        var portalChannelFactory = _cxPortalWebServiceSoapClient.ChannelFactory;
                        portalChannelFactory.UseMessageInspector(async (request, channel, next) =>
                        {
                            HttpRequestMessageProperty reqProps = new HttpRequestMessageProperty();
                            reqProps.Headers.Add("Authorization", $"Bearer {authToken}");
                            request.Properties.Add(HttpRequestMessageProperty.Name, reqProps);
                            var response = await next(request);
                            return response;
                        });

                        #endregion

                        #region V9 SOAP

                        _cxPortalWebServiceSoapClientV9 = new cxPortalWebService93.CxPortalWebServiceSoapClient(baseServer, TimeSpan.FromSeconds(360), userName, password);

                        if (ignoreCertificate)
                        {
                            _cxPortalWebServiceSoapClientV9.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new System.ServiceModel.Security.X509ServiceCertificateAuthentication()
                            {
                                CertificateValidationMode = X509CertificateValidationMode.None,
                                RevocationMode = X509RevocationMode.NoCheck
                            };
                        }

                        var portalChannelFactoryV9 = _cxPortalWebServiceSoapClientV9.ChannelFactory;
                        portalChannelFactoryV9.UseMessageInspector(async (request, channel, next) =>
                        {
                            HttpRequestMessageProperty reqProps = new HttpRequestMessageProperty();
                            reqProps.Headers.Add("Authorization", $"Bearer {authToken}");
                            request.Properties.Add(HttpRequestMessageProperty.Name, reqProps);
                            var response = await next(request);
                            return response;
                        });

                        #endregion

                        #region V9 Priority SOAP

                        _cxPriorityServiceSoapClient = new cxPriorityWebService.CxPriorityServiceSoapClient(baseServer, TimeSpan.FromSeconds(360), userName, password);

                        if (ignoreCertificate)
                        {
                            _cxPriorityServiceSoapClient.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new System.ServiceModel.Security.X509ServiceCertificateAuthentication()
                            {
                                CertificateValidationMode = X509CertificateValidationMode.None,
                                RevocationMode = X509RevocationMode.NoCheck
                            };
                        }

                        _cxPriorityServiceSoapClient.ChannelFactory.UseMessageInspector(async (request, channel, next) =>
                        {
                            HttpRequestMessageProperty reqProps = new HttpRequestMessageProperty();
                            reqProps.Headers.Add("Authorization", $"Bearer {authToken}");
                            request.Properties.Add(HttpRequestMessageProperty.Name, reqProps);
                            var response = await next(request);
                            return response;
                        });

                        #endregion

                        // This object is to maintain compatibility with the current code.
                        _oDataV9 = CxOData.ConnectToODataV9(webServer, authToken, _defaultRetries);

                        if (_isV95)
                            _oDataV95 = CxOData.ConnectToODataV95(webServer, authToken, _defaultRetries);

                        if (_isV97)
                            _oDataV97 = CxOData.ConnectToODataV97(webServer, authToken, _defaultRetries);
                    }
                    else
                    {
                        _soapSessionId = _cxPortalWebServiceSoapClient.Login(new PortalSoap.Credentials
                        {
                            User = userName,
                            Pass = password
                        }, LcId).SessionId;

                        // ODATA V8
                        _oData = CxOData.ConnectToOData(webServer, userName, password, _defaultRetries);
                    }

                    AuthenticationToken = new AuthenticationHeaderValue("Bearer", authToken);

                    httpClient.DefaultRequestHeaders.Authorization = AuthenticationToken;
                    httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");

                    //httpClient.DefaultRequestHeaders.Add("Keep-Alive", "timeout=600000");

                    return httpClient;
                }

                throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
            }
        }

        public static HttpResponseMessage TestConnection(string baseURL,
            string userName, string password, bool ignoreCertificate = true)
        {
            if (string.IsNullOrWhiteSpace(baseURL))
                throw new ArgumentNullException(nameof(baseURL));

            if (!Uri.TryCreate(baseURL, UriKind.Absolute, out Uri outAstServer))
                throw new ArgumentException($"{baseURL} is not a valid uri");

            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException(nameof(userName));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));

            if (_soapRetryPolicyProvider == null)
                _soapRetryPolicyProvider = new SOAPRetryPolicyProvider();

            if (_retryPolicy == null)
                _retryPolicy = RESTRetryPolicyProvider.CreateRetryPolicy();

            string portalVersion = null;
            if (!ignoreCertificate)
                portalVersion = GetVersionWithoutConnecting(baseURL);

            var webServer = new Uri(baseURL);
            Uri baseServer = new Uri(webServer.AbsoluteUri);

            var cxPortalWebServiceSoapClient = new PortalSoap.CxPortalWebServiceSoapClient(
                baseServer, TimeSpan.FromSeconds(60), userName, password);

            if (ignoreCertificate)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                cxPortalWebServiceSoapClient.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new System.ServiceModel.Security.X509ServiceCertificateAuthentication()
                {
                    CertificateValidationMode = X509CertificateValidationMode.None,
                    RevocationMode = X509RevocationMode.NoCheck
                };
            }

            // Get version number with regex
            if (string.IsNullOrWhiteSpace(portalVersion))
                portalVersion = cxPortalWebServiceSoapClient.GetVersionNumber().Version;

            if (string.IsNullOrWhiteSpace(portalVersion))
                throw new Exception("Error fetching CxSAST portal version");

            string pattern = @"\d+(\.\d+)+";
            Regex rg = new Regex(pattern);
            Match m = rg.Match(portalVersion);
            string version = m.Value;

            var isV9 = new Version(version).Major >= 9;

            HttpClient client = null;

            if (ignoreCertificate)
            {
                // Ignore certificate for http client
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                client = new HttpClient(clientHandler)
                {
                    BaseAddress = webServer,
                    Timeout = TimeSpan.FromMinutes(20),
                };
            }
            else
            {
                client = new HttpClient()
                {
                    BaseAddress = webServer,
                    Timeout = TimeSpan.FromMinutes(20),
                };
            }

            if (client.BaseAddress.LocalPath != "cxrestapi")
            {
                client.BaseAddress = new Uri(webServer, "/cxrestapi/");
            }

            using (var request = new HttpRequestMessage(HttpMethod.Post, "auth/identity/connect/token"))
            {
                request.Headers.Add("Accept", "application/json");

                var values = new List<KeyValuePair<string, string>> {
                    new KeyValuePair<string, string>("username", userName),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("scope",
                    isV9 ? "offline_access sast_api access_control_api" : "sast_rest_api"),
                    new KeyValuePair<string, string>("client_id",
                    isV9 ? "resource_owner_sast_client" : "resource_owner_client"),
                    new KeyValuePair<string, string>("client_secret", "014DF517-39D1-4453-B7B3-9930C563627C")
                };

                request.Content = new FormUrlEncodedContent(values);

                HttpResponseMessage response = client.SendAsync(request).Result;

                return response;
            }
        }

        public string GetProjectTeamName(string teamId)
        {
            if (string.IsNullOrWhiteSpace(teamId))
                throw new ArgumentNullException(nameof(teamId));

            var teams = GetTeams();
            if (teams.ContainsKey(teamId))
                return teams[teamId];

            return null;
        }

        public PortalSoap.CxWSResponseServerLicenseData GetLicense()
        {
            checkConnection();

            return _cxPortalWebServiceSoapClient.GetServerLicenseData(_soapSessionId);
        }

        // Cache
        private Dictionary<string, string> _teamsCache;

        /// <summary>
        /// Full Team Name
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public string GetProjectTeamName(int projectId)
        {
            return GetProjectTeamName(GetProjectTeamId(projectId));
        }

        /// <summary>
        /// Id -> Full Team Name
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetTeams()
        {
            checkConnection();

            if (_teamsCache != null)
                return _teamsCache;

            if (_isV9)
            {
                _teamsCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                foreach (var item in AC.TeamsAllAsync().Result)
                {
                    if (!_teamsCache.ContainsKey(item.Id.ToString()))
                        _teamsCache.Add(item.Id.ToString(), item.FullName);
                }

                return _teamsCache;
            }

            using (var request = new HttpRequestMessage(HttpMethod.Get, "auth/teams"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                var result = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                HttpResponseMessage response = result.response;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _teamsCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

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

        public IEnumerable<CxDataRepository.Project> GetProjectsWithLastScan()
        {
            checkConnection();

            return _oDataProjects.Expand(x => x.LastScan);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FailedScansDisplayData[] GetFailingScans()
        {
            checkConnection();

            var result = _cxPortalWebServiceSoapClient.GetFailedScansDisplayData(_soapSessionId);

            if (!result.IsSuccesfull)
                throw new ActionNotSupportedException(result.ErrorMessage);

            return result.FailedScansList;
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
        public Tuple<string, string> GetExcludedSettings(long projectId)
        {
            checkConnection();

            if (Version.Major > 9 || (Version.Major == 9 && Version.Minor >= 6))
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, $"projects/{projectId}/sourceCode/pathFilter"))
                {
                    request.Headers.Add("Accept", "application/json");

                    var result = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                    HttpResponseMessage response = result.response;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        JObject excludeSettings = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);

                        return new Tuple<string, string>(
                           (string)excludeSettings.SelectToken("pathFilter"),
                           (string)excludeSettings.SelectToken("pathFilter"));
                    }
                }
            }
            else
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, $"projects/{projectId}/sourceCode/excludeSettings"))
                {
                    request.Headers.Add("Accept", "application/json");

                    var result = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                    HttpResponseMessage response = result.response;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        JObject excludeSettings = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);

                        return new Tuple<string, string>(
                           (string)excludeSettings.SelectToken("excludeFoldersPattern"),
                           (string)excludeSettings.SelectToken("excludeFilesPattern"));
                    }
                }
            }

            throw new NotSupportedException();
        }

        public void SetExcludedSettings(int projectId, string excludeFoldersPattern, string excludeFilesPattern)
        {
            checkConnection();

            //SASTClient.ExcludeSettings_PutByidexcludeSettingsAsync(projectId, new ExcludeSettingsDto() { ExcludeFilesPattern = "", ExcludeFoldersPattern = ""}).Result

            using (var request = new HttpRequestMessage(HttpMethod.Put, $"projects/{projectId}/sourceCode/excludeSettings"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                JObject settings = new JObject
                {
                    { "excludeFoldersPattern", excludeFoldersPattern },
                    { "excludeFilesPattern", excludeFilesPattern },
                };

                request.Content = new StringContent(JsonConvert.SerializeObject(settings));
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                HttpResponseMessage response = result.response;
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"Error updating excluded settings for project {projectId} ({response.StatusCode})");
            }
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

                var result = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                HttpResponseMessage response = result.response;

                if (response.StatusCode == HttpStatusCode.OK)
                {

                }
            }

        }

        #region Custom Fields
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
            SetCustomFields(projDetails.Id, projDetails.Name, projDetails.TeamId.ToString(), customFields);
        }

        public void SetCustomFields(long projId, string projName, string teamId, IEnumerable<CustomField> customFields)
        {
            checkConnection();

            using (var request = new HttpRequestMessage(HttpMethod.Put, $"projects/{projId}"))
            {
                request.Headers.Add("Accept", "application/json;v=2.0");
                JObject settings = new JObject
                {
                    { "name",  projName },
                    { "owningTeam", teamId },
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

                var result = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                HttpResponseMessage response = result.response;
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
                }
            }
        }
        #endregion

        public void SetProjectConfiguration(long projId, string projName = null, string teamId = null)
        {
            if (projId <= 0)
                throw new ArgumentOutOfRangeException(nameof(projId));

            if (string.IsNullOrWhiteSpace(projName) && string.IsNullOrWhiteSpace(teamId))
            {
                throw new NotSupportedException(nameof(projName) + " and " + nameof(teamId) + " cannot be null in the same call");
            }

            checkConnection();

            var projectProperties = GetProjectSettings((int)projId);

            var projectName = projName ?? projectProperties.Name;
            var projectTeamId = teamId ?? projectProperties.TeamId;
            var customFields = projectProperties.CustomFields != null ? new JArray(
                        projectProperties.CustomFields.Select(x => new JObject
                        {
                            {  "id", x.Id },
                            {  "value", x.Value }
                        })) : null;

            using (var request = new HttpRequestMessage(HttpMethod.Put, $"projects/{projId}"))
            {
                request.Headers.Add("Accept", "application/json;v=2.0");
                JObject settings = new JObject
                {
                    { "name",  projName },
                    { "owningTeam", teamId },
                    { "customFields", customFields }
                };

                request.Content = new StringContent(JsonConvert.SerializeObject(settings));

                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                HttpResponseMessage response = result.response;
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        public DateTimeOffset GetProjectCreationDate(long projectID)
        {
            checkConnection();

            return ProjectsOData.Where(x => x.Id == projectID).First().CreatedDate;
        }

        public ProjectDetails GetProjectSettings(int projectId)
        {
            checkConnection();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"projects/{projectId}"))
            {
                request.Headers.Add("Accept", "application/json;v=2.0");

                var result = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                HttpResponseMessage response = result.response;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    ProjectDetails projDetails = JsonConvert.DeserializeObject<ProjectDetails>(response.Content.ReadAsStringAsync().Result);
                    return projDetails;
                }

                throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
            }
        }

        public cxPortalWebService93.CxWSSingleResultCompareData[] GetScansDiff(int oldScanId, int newScanId)
        {
            checkConnection();

            var result = _cxPortalWebServiceSoapClientV9.GetCompareScanResultsAsync(_soapSessionId, oldScanId, newScanId).Result;

            checkSoapResponse(result);

            return result.Results;
        }

        /// <summary>
        /// Deletes an existing project with all related scans
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="deleteRunningScans"></param>
        public void DeleteProject(int projectId, bool deleteRunningScans = true)
        {
            checkConnection();

            using (var request = new HttpRequestMessage(HttpMethod.Delete, $"projects/{projectId}"))
            {
                request.Headers.Add("Accept", "application/json;v=2.0");

                JObject settings = new JObject
                {
                    { "deleteRunningScans",  deleteRunningScans },
                };

                request.Content = new StringContent(JsonConvert.SerializeObject(settings));
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                HttpResponseMessage response = result.response;
                if (response.StatusCode != HttpStatusCode.Accepted)
                {
                    throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        public void DeleteProjects(params long[] ids)
        {
            var result = PortalSOAP.DeleteProjectsAsync(new cxPortalWebService93.CxWSRequestDeleteProjects
            {
                ProjectIDs = ids,
                Flags = cxPortalWebService93.DeleteFlags.RunningScans,
                SessionID = _soapSessionId
            }).Result;

            checkSoapResponse(result);
        }

        /// <summary>
        /// Returns true if the SAST version supports a given version.s
        /// </summary>
        /// <param name="version">1.1, 0.1, 1, ...</param>
        /// <returns>true if the api supports a given version of the REST API, otherwise false.</returns>
        public bool SupportsRESTAPIVersion(string version)
        {
            var _ = double.Parse(version);

            try
            {
                GetRequest<JObject>($"CxRestAPI/help/swagger/docs/v{version}");
                return true;
            }
            catch
            {
                return false;
            }
        }

        private FailedScansDisplayData[] _failedScans = null;

        public FailedScansDisplayData[] FailedScans
        {
            get
            {
                if (_failedScans == null)
                {
                    checkConnection();
                    var response = _cxPortalWebServiceSoapClient.GetFailedScansDisplayData(_soapSessionId);
                    checkSoapResponse(response);
                    _failedScans = response.FailedScansList;
                }
                return _failedScans;
            }
        }

        public IEnumerable<FailedScansDisplayData> GetFailedScans(long projectId)
        {
            return FailedScans.Where(x => x.ProjectId == projectId);
        }

        public DateTime GetLastDataRetention()
        {
            checkConnection();

            dynamic result = null;

            if (_isV9)
            {
                result = _cxPortalWebServiceSoapClientV9.GetLatestFinishedDataRetentionRequestAsync(_soapSessionId).Result;
            }
            else
            {
                result = _cxPortalWebServiceSoapClient.GetLatestFinishedDataRetentionRequest(_soapSessionId);
            }

            checkSoapResponse(result);
            return result.DataRetentionRequest.RequestDate;
        }

        public int GetResultStateIdByName(string name)
        {
            return (int)GetResultStateList().First(x => x.Value == name).Key;
        }

        public Dictionary<long, string> GetResultStateList()
        {
            checkConnection();

            Dictionary<long, string> result = null;

            if (_isV9)
            {
                var resultV9 = _cxPortalWebServiceSoapClientV9.GetResultStateListAsync(_soapSessionId).Result;
                checkSoapResponse(resultV9);
                result = resultV9.ResultStateList.ToDictionary(x => x.ResultID, y => y.ResultName);
            }
            else
            {
                var resultV8 = _cxPortalWebServiceSoapClient.GetResultStateListAsync(_soapSessionId).Result;
                checkSoapResponse(resultV8);
                result = resultV8.ResultStateList.ToDictionary(x => x.ResultID, y => y.ResultName);
            }

            return result;
        }

        public cxPortalWebService93.CxWSResponceResultPath GetPathCommentsHistory(long scanId, long pathId)
        {
            checkConnection();

            dynamic result = null;

            if (_isV9)
            {
                result = _cxPortalWebServiceSoapClientV9.GetPathCommentsHistoryAsync(_soapSessionId, scanId, pathId, cxPortalWebService93.ResultLabelTypeEnum.State).Result;
                result = _cxPortalWebServiceSoapClientV9.GetPathCommentsHistoryAsync(_soapSessionId, scanId, pathId, cxPortalWebService93.ResultLabelTypeEnum.Assign).Result;
                result = _cxPortalWebServiceSoapClientV9.GetPathCommentsHistoryAsync(_soapSessionId, scanId, pathId, cxPortalWebService93.ResultLabelTypeEnum.IgnorePath).Result;
                result = _cxPortalWebServiceSoapClientV9.GetPathCommentsHistoryAsync(_soapSessionId, scanId, pathId, cxPortalWebService93.ResultLabelTypeEnum.Remark).Result;
                result = _cxPortalWebServiceSoapClientV9.GetPathCommentsHistoryAsync(_soapSessionId, scanId, pathId, cxPortalWebService93.ResultLabelTypeEnum.Severity).Result;
                result = _cxPortalWebServiceSoapClientV9.GetPathCommentsHistoryAsync(_soapSessionId, scanId, pathId, cxPortalWebService93.ResultLabelTypeEnum.IssueTracking).Result;
            }
            else
            {
                throw new NotImplementedException();
                // result = _cxPortalWebServiceSoapClient.GetPathCommentsHistoryAsync(_soapSessionId, scanId, pathId);
            }

            checkSoapResponse(result);
            return result;
        }

        /// <summary>
        /// For V8.9 is UID, for 9.X or higher is Int.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public string GetProjectTeamId(int projectId)
        {
            return GetProjectSettings(projectId).TeamId;
        }

        public Dictionary<string, int> GetSASTCustomFields()
        {
            checkConnection();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"customFields"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                var result = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                HttpResponseMessage response = result.response;

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
            return GetProjectSettings(projectId).CustomFields.ToDictionary(x => x.Name);
        }

        #region OSA

        public IEnumerable<Guid> GetOSAScansIds(int projectId)
        {
            return GetOSAScans(projectId).Select(x => x.Id);
        }

        public ICollection<OSAScanDto> GetOSAScans(int projectId)
        {
            checkConnection();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"osa/scans?projectId={projectId}"))
            {
                request.Headers.Add("Accept", "application/json;v=2.0");

                var result = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                HttpResponseMessage response = result.response;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<OSAScanDto[]>(response.Content.ReadAsStringAsync().Result);
                }

                throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
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
            checkConnection();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"osa/reports?scanId={osaScanId}"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                var resultResponse = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                HttpResponseMessage response = resultResponse.response;
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

                throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
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

                var result = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                HttpResponseMessage response = result.response;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<List<OSALibraryDto>>(response.Content.ReadAsStringAsync().Result);
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

                var result = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                HttpResponseMessage response = result.response;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<List<OSALicenseDto>>(response.Content.ReadAsStringAsync().Result);
                }

                throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
            }
        }

        #endregion

        #region SAST

        public byte[] GetSourceCode(long scanId)
        {
            checkConnection();

            dynamic result = null;

            if (_isV9)
            {
                result = CxAuditV9.GetSourceCodeForScanAsync(_soapSessionId, scanId).Result;
            }
            else
            {
                result = _cxPortalWebServiceSoapClient.GetSourceCodeForScan(_soapSessionId, scanId);
            }

            checkSoapResponse(result);
            return result.sourceCodeContainer.ZippedFile;
        }


        public void CancelScan(string runId)
        {
            checkConnection();

            dynamic result = null;

            if (_isV9)
            {
                result = _cxPortalWebServiceSoapClientV9.CancelScanAsync(_soapSessionId, runId).Result;
            }
            else
            {
                result = _cxPortalWebServiceSoapClient.CancelScanAsync(_soapSessionId, runId).Result;
            }

            checkSoapResponse(result);
        }

        public void DeleteScan(long scanId)
        {
            checkConnection();

            dynamic result = null;

            var scan = GetScanById(scanId);
            if (scan != null)
            {
                if (scan.Status.Name == "Scanning" || scan.Status.Name == "Running" || scan.Status.Name == "Queued")
                    CancelScan(scanId.ToString());

                result = _cxPortalWebServiceSoapClient.DeleteScanAsync(_soapSessionId, scanId).Result;
            }

            checkSoapResponse(result);
        }

        public ProjectConfiguration GetProjectConfigurations(long projectId)
        {
            checkConnection();

            var response = _cxPortalWebServiceSoapClient.GetProjectConfiguration(_soapSessionId, projectId);

            checkSoapResponse(response);

            return response.ProjectConfig;
        }

        public ConfigurationSet[] GetConfigurationSetList()
        {
            checkConnection();

            var response = _cxPortalWebServiceSoapClient.GetConfigurationSetList(_soapSessionId);

            checkSoapResponse(response);

            return response.ConfigSetList;
        }

        public void SetPreset(long projectId, string presetName)
        {
            SetPreset(projectId, GetPresets().First(x => string.Compare(x.Value, presetName) == 0).Key);
        }

        public void SetPreset(long projectId, long presetId)
        {
            var projecTconfigu = SASTClient.ScanSettings_GetByprojectIdAsync(projectId).Result;

            SASTClient.ScanSettings_PutByscanSettingsAsync(new ScanSettingsRequestDto
            {
                ProjectId = projectId,
                PresetId = presetId,
                EngineConfigurationId = projecTconfigu.EngineConfiguration.Id.Value,
                EmailNotifications = projecTconfigu.EmailNotifications,
                PostScanActionId = projecTconfigu.PostScanAction?.Id
            }).Wait();
        }

        /// <summary>
        /// Runs a SAST Scan or re-runs it with the last source code.
        /// </summary>
        /// <param name="projectId">Project ID in SAST</param>
        /// <param name="preset">What preset to use</param>
        /// <param name="comment"></param>
        /// <param name="forceScan"></param>
        /// <param name="sourceCodeZipContent">Zipped source code to scan</param>
        public long? RunSASTScan(long projectId, string comment = "", bool forceScan = true, byte[] sourceCodeZipContent = null,
            bool useLastScanPreset = false, int? presetId = null, int? configurationId = null, bool runPublicScan = true, bool forceLocal = false, CxClient cxClient2 = null)
        {
            checkConnection();

            var projectConfig = GetProjectConfigurations(projectId);

            if (forceLocal || projectConfig.SourceCodeSettings.SourceOrigin == PortalSoap.SourceLocationType.Local)
            {
                if (sourceCodeZipContent == null || !sourceCodeZipContent.Any())
                {
                    if (!forceScan)
                        throw new NotSupportedException("If the scan is not being forced, then you should pass the source code in the parameters.");

                    var scan = GetLastScan(projectId);
                    if (scan == null)
                        throw new NotSupportedException("There is no last scan in the project to download the source code from, please pass it on the parameters");

                    sourceCodeZipContent = GetSourceCode(scan.Id);

                    if (cxClient2 == null)
                    {
                        //Scan without overriding anything
                        if (Version.Major > 9 || (Version.Major == 9 && Version.Minor >= 5))
                        {
                            if (SASTClientV4 != null && presetId.HasValue)
                            {
                                SASTV4.FileParameter file = new SASTV4.FileParameter(new MemoryStream(sourceCodeZipContent));

                                var result = SASTClientV4.ScanWithSettings_4_StartScanByscanSettings((int)projectId, false, false, true, true, comment, presetId.Value, configurationId, null, null, file).Result;

                                return result.Id;
                            }
                        }
                        else
                        {
                            if (SASTClientV1_1 != null && presetId.HasValue)
                            {
                                SAST.FileParameter file = new SAST.FileParameter(new MemoryStream(sourceCodeZipContent));

                                var result = SASTClientV1_1.ScanWithSettings1_1_StartScanByscanSettings((int)projectId, false, false, true, true, comment, presetId.Value, configurationId, null, file).Result;

                                return result.Id;
                            }
                        }

                        // Update SAST Project Config to match CI/CD
                        if (useLastScanPreset)
                        {
                            var scanPreset = GetScanPreset(scan.Id);
                            if (scanPreset.Id != projectConfig.ProjectSettings.PresetID)
                                SetPreset(projectId, scanPreset.Name);
                        }

                        UploadSourceCode(projectId, sourceCodeZipContent);
                    }
                    else
                    {
                        //Scan without overriding anything
                        if (cxClient2.Version.Major > 9 || (cxClient2.Version.Major == 9 && cxClient2.Version.Minor >= 5))
                        {
                            if (cxClient2.SASTClientV4 != null && presetId.HasValue)
                            {
                                SASTV4.FileParameter file = new SASTV4.FileParameter(new MemoryStream(sourceCodeZipContent));

                                var result = cxClient2.SASTClientV4.ScanWithSettings_4_StartScanByscanSettings((int)projectId, false, false, true, true, comment, presetId.Value, configurationId, null, null, file).Result;

                                return result.Id;
                            }
                        }
                        else
                        {
                            if (cxClient2.SASTClientV1_1 != null && presetId.HasValue)
                            {
                                SAST.FileParameter file = new SAST.FileParameter(new MemoryStream(sourceCodeZipContent));

                                var result = cxClient2.SASTClientV1_1.ScanWithSettings1_1_StartScanByscanSettings((int)projectId, false, false, true, true, comment, presetId.Value, configurationId, null, file).Result;

                                return result.Id;
                            }
                        }

                        // Update SAST Project Config to match CI/CD
                        if (useLastScanPreset)
                        {
                            var scanPreset = GetScanPreset(scan.Id);
                            if (scanPreset.Id != projectConfig.ProjectSettings.PresetID)
                                cxClient2.SetPreset(projectId, scanPreset.Name);
                        }

                        cxClient2.UploadSourceCode(projectId, sourceCodeZipContent);
                    }
                }
            }
            else
            {
                if (cxClient2 == null)
                {
                    if (Version.Major > 9 || (Version.Major == 9 && Version.Minor >= 5))
                    {
                        if (SASTClientV4 != null && presetId.HasValue)
                        {
                            var result = SASTClientV4.ScanWithSettings_4_StartScanByscanSettings(Convert.ToInt32(projectId), false, false, true, true, comment, presetId.Value, configurationId, null, null, null).Result;

                            return result.Id;
                        }
                    }
                    else
                    {
                        if (SASTClientV1_1 != null && presetId.HasValue)
                        {
                            var result = SASTClientV1_1.ScanWithSettings1_1_StartScanByscanSettings(Convert.ToInt32(projectId), false, false, true, true, comment, presetId.Value, configurationId, null, null).Result;

                            return result.Id;
                        }
                    }
                }
                else
                {
                    if (cxClient2.Version.Major > 9 || (cxClient2.Version.Major == 9 && cxClient2.Version.Minor >= 5))
                    {
                        if (cxClient2.SASTClientV4 != null && presetId.HasValue)
                        {
                            var result = cxClient2.SASTClientV4.ScanWithSettings_4_StartScanByscanSettings(Convert.ToInt32(projectId), false, false, true, true, comment, presetId.Value, configurationId, null, null, null).Result;

                            return result.Id;
                        }
                    }
                    else
                    {
                        if (cxClient2.SASTClientV1_1 != null && presetId.HasValue)
                        {
                            var result = cxClient2.SASTClientV1_1.ScanWithSettings1_1_StartScanByscanSettings(Convert.ToInt32(projectId), false, false, true, true, comment, presetId.Value, configurationId, null, null).Result;

                            return result.Id;
                        }
                    }
                }
            }

            if (cxClient2 == null)
                return triggerNewScan(projectId, forceScan, runPublicScan, comment);
            else
                return cxClient2.triggerNewScan(projectId, forceScan, runPublicScan, comment);


        }

        private void UploadSourceCode(long projectId, byte[] sourceCodeZipContent)
        {
            using (var content = new MultipartFormDataContent())
            {
                var fileContent = new ByteArrayContent(sourceCodeZipContent);
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "filename.zip",
                    Name = "zippedSource"
                };

                content.Add(fileContent);

                string requestUri = $"projects/{projectId}/sourceCode/attachments";

                HttpResponseMessage attachCodeResponse = httpClient.PostAsync(requestUri, content).Result;

                if (attachCodeResponse.StatusCode != HttpStatusCode.OK && attachCodeResponse.StatusCode != HttpStatusCode.NoContent)
                {
                    throw new NotSupportedException(attachCodeResponse.Content.ReadAsStringAsync().Result);
                }
            }
        }

        /// <summary>
        /// Creates a new scan
        /// </summary>
        /// <param name="projectId">Specifies the Unique Id of the specific project to be scanned</param>
        /// <param name="forceScan">Specifies whether the code should be scanned regardless of unchanged code ,</param>
        /// <param name="publicScan">Specifies whether the requested scan is public or private</param>
        /// <param name="comment">Specifies the scan comment</param>
        /// <returns>the Id of the scan</returns>
        private long? triggerNewScan(long projectId, bool forceScan, bool publicScan, string comment)
        {
            return SASTClient.SastScans_PostByscanAsync(new SastScanRequestWriteDTO
            {
                ProjectId = projectId,
                Comment = comment ?? string.Empty,
                ForceScan = forceScan,
                IsIncremental = false,
                IsPublic = publicScan
            }).Result.Id;
        }

        private void checkSoapResponse(cxPortalWebService93.CxWSBasicRepsonse result)
        {
            if (!result.IsSuccesfull)
                throw new ApplicationException(result.ErrorMessage);
        }

        private void checkSoapResponse(CxAuditWebServiceV9.CxWSBasicRepsonse result)
        {
            if (!result.IsSuccesfull)
                throw new ApplicationException(result.ErrorMessage);
        }

        private void checkSoapResponse(CxWSBasicRepsonse result)
        {
            if (!result.IsSuccesfull)
                throw new ApplicationException(result.ErrorMessage);
        }


        public SASTResults GetSASTResults(long sastScanId)
        {
            checkConnection();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"sast/scans/{sastScanId}/resultsStatistics"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                var result = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                HttpResponseMessage response = result.response;

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

                var result = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                HttpResponseMessage response = result.response;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var scans = (JArray)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                    return scans;
                }

                throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
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

                var result = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                HttpResponseMessage response = result.response;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var objRes = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<Scan>>(objRes);
                }

                throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
            }
        }

        public void LockScan(long scanId, string comment = null)
        {
            checkConnection();

            if (_isV9)
            {
                var response = _cxPortalWebServiceSoapClientV9.LockScanAsync(_soapSessionId, scanId).Result;

                if (!string.IsNullOrWhiteSpace(comment))
                    checkSoapResponse(_cxPortalWebServiceSoapClientV9.UpdateScanCommentAsync(_soapSessionId, scanId, comment).Result);

                checkSoapResponse(response);
            }
            else
            {
                var response = _cxPortalWebServiceSoapClient.LockScanAsync(_soapSessionId, scanId).Result;

                if (!string.IsNullOrWhiteSpace(comment))
                    throw new NotImplementedException("Adding comment is not support for this version");

                checkSoapResponse(response);
            }
        }

        public void UnlockScan(long scanId)
        {
            checkConnection();

            if (_isV9)
            {
                checkSoapResponse(_cxPortalWebServiceSoapClientV9.UnlockScanAsync(_soapSessionId, scanId).Result);
            }
            else
            {
                checkSoapResponse(_cxPortalWebServiceSoapClient.UnlockScanAsync(_soapSessionId, scanId).Result);
            }
        }

        public enum ScanRetrieveKind
        {
            First,
            Last,
            Locked,
            All
        }

        public IEnumerable<Scan> GetAllSASTScans(long projectId)
        {
            return GetScans(projectId, true, ScanRetrieveKind.All);
        }

        public Scan GetFirstScan(long projectId)
        {
            return GetScans(projectId, true).FirstOrDefault();
        }

        public Scan GetLastScan(long projectId, bool fullScanOnly = false, bool onlyPublic = false, DateTime? maxScanDate = null, bool finished = true, bool useServerTime = true)
        {
            var scans = GetScans(projectId, finished, onlyPublic: onlyPublic, maxScanDate: maxScanDate, useServerTime: useServerTime);

            // if there is no scans
            if (!scans.Any())
                return null;

            if (fullScanOnly)
                scans = scans.Where(x => !x.IsIncremental);

            if ((Version.Major == 9 && Version.Minor >= 5) || Version.Major > 9)
            {
                if (!finished)
                    return scans.OrderByDescending(x => x.DateAndTime.StartedOn).FirstOrDefault();

                DateTimeOffset? filterMaxDate = null;
                if (maxScanDate.HasValue)
                    filterMaxDate = useServerTime ? new DateTimeOffset(maxScanDate.Value, ServerDateTimeOffSet) : new DateTimeOffset(maxScanDate.Value);

                // Prevent cases where the Id's counters of the scans where reinitiated.
                long? scanId = _oDataV95.Projects.Expand(x => x.Scans)
                    .Where(p => p.Id == projectId).FirstOrDefault()?.Scans
                    .Where(x => (!fullScanOnly || !x.IsIncremental.Value)
                             && (!onlyPublic || x.IsPublic)
                             && (!finished || x.ScanType == 1)
                             && (filterMaxDate == null || x.ScanCompletedOn <= filterMaxDate)
                          )
                    .OrderByDescending(x => x.ScanRequestedOn)
                    .FirstOrDefault()?.Id;

                // There needs to be a scan in the scans list that match the value returned by the ODATA query.
                return scanId != null ? scans.Single(x => x.Id == scanId.Value) : null;
            }

            return scans.LastOrDefault();
        }

        public Scan GetLastScanByVersion(long projectId, string version, bool onlyPublic = false, DateTime? maxScanDate = null, bool useServerTime = true)
        {
            return GetScans(projectId, true, version: version, onlyPublic: onlyPublic, maxScanDate: maxScanDate, useServerTime: useServerTime).LastOrDefault();
        }

        public Scan GetLastScanFinishOrFailed(long projectId)
        {
            return GetScans(projectId, false).LastOrDefault();
        }

        public Scan GetLockedScan(long projectId)
        {
            return GetScans(projectId, true, ScanRetrieveKind.Locked).LastOrDefault();
        }

        public int GetScanCount()
        {
            checkConnection();
            return _oDataScans.Count();
        }

        public IEnumerable<Scan> GetScans(long projectId, bool finished, ScanRetrieveKind scanKind = ScanRetrieveKind.All, string version = null, bool onlyPublic = false, DateTime? minScanDate = null, DateTime? maxScanDate = null, bool includeGhostScans = true, bool useServerTime = true)
        {
            DateTimeOffset? filterMaxDate = null;
            if (maxScanDate.HasValue)
                filterMaxDate = useServerTime ? new DateTimeOffset(maxScanDate.Value, ServerDateTimeOffSet) : new DateTimeOffset(maxScanDate.Value);

            DateTimeOffset? filterMinDate = null;
            if (minScanDate.HasValue)
                filterMinDate = useServerTime ? new DateTimeOffset(minScanDate.Value, ServerDateTimeOffSet) : new DateTimeOffset(minScanDate.Value);

            return getODataScans(projectId, finished, scanKind, version, onlyPublic, filterMinDate, filterMaxDate, includeGhostScans);
        }

        private IEnumerable<Scan> getODataScans(long projectId, bool finished, ScanRetrieveKind scanKind = ScanRetrieveKind.All, string version = null, bool onlyPublic = false, DateTimeOffset? minScanDateOffset = null, DateTimeOffset? maxScanDateOffset = null, bool includeGhostScans = true)
        {
            checkConnection();

            IQueryable<IODataScan> scans = null;
            if (_isV97)
                scans = _oData97Scans.Where(x => x.ProjectId == projectId);
            else
                scans = _oDataScans.Where(x => x.ProjectId == projectId);

            if (onlyPublic)
                scans = scans.Where(x => x.IsPublic);

            if (version != null)
                scans = scans.Where(x => version.StartsWith(x.ProductVersion));

            if (minScanDateOffset != null)
                scans = scans.Where(x => x.ScanRequestedOn >= minScanDateOffset);

            if (maxScanDateOffset != null)
                scans = scans.Where(x => x.ScanRequestedOn <= maxScanDateOffset);

            if (!includeGhostScans)
                scans = scans.Where(x => !(x.ScanType == 1 && x.EngineFinishedOn == null));

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

            foreach (var scan in scans)
            {
                if (finished && scan.ScanType == 3)
                    continue;

                if (_isV97)
                    yield return Mapper.ConvertScanFromOData((CxDataRepository97.Scan)scan);
                else
                    yield return Mapper.ConvertScanFromOData((CxDataRepository.Scan)scan);
            }
        }

        public Scan GetScanById(long scanId)
        {
            checkConnection();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"sast/scans/{scanId}"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                var result = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                HttpResponseMessage response = result.response;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var objRes = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<Scan>(objRes);
                }

                throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
            }
        }

        public bool ProjectHasScanRunning(long projectId)
        {
            return GetScansQueue(projectId).Any(x => x.Stage.Value != "Finished" &&
                                                     x.Stage.Value != "Failed" &&
                                                     x.Stage.Value != "Canceled");
        }

        public ICollection<ScanQueue> GetScansQueue(long? projectId = null)
        {
            return SASTClient.ScansQueueV1_GetScansQueueByprojectIdAsync(projectId).Result;
        }

        /// <summary>
        /// get preset /sast/scanSettings/{projectId}
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public string GetSASTPreset(int projectId)
        {
            checkConnection();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"sast/scanSettings/{projectId}"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                var resultResponse = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                HttpResponseMessage response = resultResponse.response;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject<ScanSettings>(response.Content.ReadAsStringAsync().Result);

                    if (!GetPresets().ContainsKey(result.Preset.Id))
                        return "Checkmarx Default";

                    return GetPresets()[result.Preset.Id];
                }

                throw new NotSupportedException(response.ToString());
            }
        }

        public string GetProjectConfiguration(int projectId)
        {
            checkConnection();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"sast/scanSettings/{projectId}"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                var resultResponse = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                HttpResponseMessage response = resultResponse.response;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject<ScanSettings>(response.Content.ReadAsStringAsync().Result);

                    var config = GetConfigurationSetList().Where(x => x.ID == result.EngineConfiguration.Id).FirstOrDefault();

                    if (config == null)
                        return "Default Configuration";

                    return config.ConfigSetName;
                }

                throw new NotSupportedException(response.ToString());
            }
        }

        #endregion

        #region Checkmarx Configurations

        public List<ComponentConfiguration> GetConfigurations(SAST.Group group)
        {
            checkConnection();

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"configurationsExtended/{ConvertToString(group)}"))
            {
                request.Headers.Add("Accept", "application/json;v=1.0");

                var result = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                var response = result.response;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<List<ComponentConfiguration>>(response.Content.ReadAsStringAsync().Result);
                }

                throw new NotSupportedException(response.Content.ReadAsStringAsync().Result);
            }
        }

        #endregion

        #region Projects

        /// <summary>
        /// Gets the projects.
        /// Id - Name
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Dictionary<int, string> GetProjects()
        {
            return GetAllProjectsDetails().ToDictionary(x => (int)x.Id, x => x.Name);
        }

        public List<ProjectDetails> GetAllProjectsDetails(bool showAlsoDeletedProjects = false)
        {
            checkConnection();

            string version = "2.0";
            string link = "projects";
            if (SupportsRESTAPIVersion("2.2"))
            {
                if (showAlsoDeletedProjects)
                    link += "?showAlsoDeletedProjects=true";

                version = "2.2";
            }

            using (var projects = new HttpRequestMessage(HttpMethod.Get, link))
            {
                try
                {
                    projects.Headers.Add("Accept", $"application/json;v={version}");
                    projects.Headers.Add("Connection", "keep-alive");

                    var result = ExecuteWithRetryAsync(httpClient, projects, CancellationToken.None).GetAwaiter().GetResult();
                    var projectListResponse = result.response;

                    if (projectListResponse.StatusCode == HttpStatusCode.OK)
                    {
                        string plResult = projectListResponse.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<List<ProjectDetails>>(plResult);
                    }

                    if (projectListResponse.StatusCode == HttpStatusCode.Unauthorized)
                        throw new UnauthorizedAccessException();

                    throw new NotSupportedException(projectListResponse.Content.ReadAsStringAsync().Result);
                }
                catch (AggregateException)
                {
                    throw;
                }
            }
        }

        #endregion

        #region Users and Teams

        private IEnumerable<UserViewModel> _users { get; set; }
        public IEnumerable<UserViewModel> Users
        {
            get
            {
                if (_users == null)
                    _users = AC.GetAllUsersDetailsAsync().Result;

                return _users;
            }
        }

        private IEnumerable<TeamViewModel> _teams { get; set; }
        public IEnumerable<TeamViewModel> Teams
        {
            get
            {
                if (_teams == null)
                    _teams = AC.TeamsAllAsync().Result;

                return _teams;
            }
        }

        public TeamViewModel GetTeamByFullName(string fullName)
        {
            return Teams.Where(x => x.FullName == fullName).FirstOrDefault();
        }

        public IEnumerable<UserViewModel> GetUsersByTeamId(long id)
        {
            return Users.Where(x => x.TeamIds.Any(x => x == id));
        }

        public UserViewModel GetScanInitiator(long scanId)
        {
            var scan = GetScanById(scanId);
            if (scan != null)
                return GetUserByFullName(scan.InitiatorName);

            return null;
        }

        public UserViewModel GetUserByFullName(string fullName)
        {
            return Users.Where(x => string.Format("{0} {1}", x.FirstName, x.LastName) == fullName).FirstOrDefault();
        }

        public void DisableUserByEmail(string email)
        {
            var user = Users.FirstOrDefault(x => x.Email == email);

            if (user == null)
                throw new UserNotFoundException("User not found");

            UpdateUserModel updateUserModel = new UpdateUserModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ExpirationDate = user.ExpirationDate,
                PhoneNumber = user.PhoneNumber,
                AllowedIpList = user.AllowedIpList,
                CellPhoneNumber = user.CellPhoneNumber,
                Country = user.Country,
                JobTitle = user.JobTitle,
                LocaleId = user.LocaleId,
                Other = user.Other,
                TeamIds = user.TeamIds,
                RoleIds = user.RoleIds,
                Active = false
            };

            AC.UpdateUserDetails(user.Id, updateUserModel);
        }

        #endregion

        #region Presets & Queries

        private Dictionary<int, string> _presetsCache;
        public Dictionary<int, string> GetPresets()
        {
            if (_presetsCache != null)
                return _presetsCache;

            checkConnection();

            using (var presets = new HttpRequestMessage(HttpMethod.Get, $"sast/presets"))
            {
                presets.Headers.Add("Accept", "application/json;v=1.0");

                var resultResponse = ExecuteWithRetryAsync(httpClient, presets, CancellationToken.None).GetAwaiter().GetResult();
                HttpResponseMessage presetResponse = resultResponse.response;

                if (presetResponse.StatusCode == HttpStatusCode.OK)
                {
                    var presetArray = JsonConvert.DeserializeObject<JArray>(presetResponse.Content.ReadAsStringAsync().Result);

                    _presetsCache = presetArray.ToDictionary(x => (int)x.SelectToken("id"), x => (string)x.SelectToken("name"));

                    return _presetsCache;
                }

                throw new NotSupportedException(presetResponse.ToString());
            }
        }

        /// <summary>
        /// Get the preset name for a specific scan.
        /// </summary>
        /// <param name="scanId"></param>
        /// <returns></returns>
        public string GetScanPresetName(long scanId)
        {
            return GetScanPreset(scanId).Name;
        }

        /// <summary>
        /// Get a preset for a specific scan.
        /// </summary>
        /// <param name="scanId"></param>
        /// <returns></returns>
        public CxDataRepository.Preset GetScanPreset(long scanId)
        {
            checkConnection();

            if (scanId <= 0)
            {
                throw new ArgumentException(nameof(scanId));
            }

            if (_scanCache == null)
            {
#if DEBUG
                //var watch = new Stopwatch();
                //watch.Start();
#endif

                _scanCache = _oDataScans.Expand(x => x.Preset).Where(x => x.IsLocked).ToDictionary(x => x.Id);

#if DEBUG
                //watch.Stop();
                //Console.WriteLine($"Found {_scanCache.Keys.Count} scans in {watch.Elapsed.TotalMinutes} minutes");
                //Console.SetCursorPosition(0, Console.CursorTop);
#endif
            }

            if (!_scanCache.ContainsKey(scanId))
            {
                var scan = _oDataScans.Expand(x => x.Preset).Where(x => x.Id == scanId).FirstOrDefault();
                if (scan != null)
                {
                    _scanCache.Add(scan.Id, scan);
                    return scan.Preset;
                }

                throw new KeyNotFoundException($"Scan with Id {scanId} does not exist.");
            }

            return _scanCache[scanId].Preset;
        }

        public Dictionary<string, List<long>> GetPresetCWEByLanguage(long presetId)
        {
            var listOfCWEByLanguage = new Dictionary<string, List<long>>();
            var queryCWE = new Dictionary<long, Tuple<string, CxWSQuery>>();

            foreach (var item in QueryGroups)
            {
                foreach (var query in item.Queries)
                {
                    queryCWE.Add(query.QueryId, new Tuple<string, CxWSQuery>(item.LanguageName, query));
                }
            }

            // Language -> CWE
            foreach (var queryId in _cxPortalWebServiceSoapClient.GetPresetDetailsAsync(_soapSessionId, presetId).Result.preset.queryIds)
            {
                var result = queryCWE[queryId];
                if (result.Item2.Cwe != 0)
                {
                    if (!listOfCWEByLanguage.ContainsKey(result.Item1))
                    {
                        listOfCWEByLanguage.Add(result.Item1, new List<long> { result.Item2.Cwe });
                    }
                    else
                    {
                        listOfCWEByLanguage[result.Item1].Add(result.Item2.Cwe);
                    }
                }
            }

            return listOfCWEByLanguage;
        }

        public string GetPresetCWE(string presetName)
        {
            checkConnection();

            var presets = _cxPortalWebServiceSoapClient.GetPresetListAsync(_soapSessionId).Result;

            checkSoapResponse(presets);

            presetName = presetName.ToLowerInvariant();
            var preset = presets.PresetList.SingleOrDefault(x => x.PresetName.ToLowerInvariant() == presetName);

            var stringBuilder = new StringBuilder();

            if (preset == null)
                throw new NotSupportedException($"The preset {presetName} was not found");

            var languageAndCweIds = GetPresetCWEByLanguage(preset.ID);
            foreach (var item in languageAndCweIds)
            {
                stringBuilder.AppendLine($"{item.Key} - {string.Join(",", item.Value.Distinct().OrderBy(x => x))}");
            }

            return stringBuilder.ToString();
        }

        public void ImportQueryGroupQuery93(IEnumerable<CxAuditWebServiceV9.CxWSQueryGroup> queryGroup)
        {
            checkConnection();
            dynamic response = null;
            if (_isV9)
            {

                response = CxAuditV9
                      .UploadQueriesAsync(_soapSessionId, queryGroup.ToArray()).Result;
            }
            checkSoapResponse(response);
            _queryGroupsCache = null;
        }

        public IEnumerable<long> GetPresetQueryIds(int presetId)
        {
            checkConnection();

            return _cxPortalWebServiceSoapClientV9.GetPresetDetailsAsync(_soapSessionId, presetId).Result.preset.queryIds;
        }

        public cxPortalWebService93.CxPresetDetails GetPresetDetails(int presetId)
        {

            return _cxPortalWebServiceSoapClientV9.GetPresetDetailsAsync(_soapSessionId, presetId).Result.preset;
        }


        // structure the name of the queries -> id of the CX queries
        private Dictionary<string, long> _cxQueryId = null;
        public long GetPresetQueryId(cxPortalWebService93.CxWSQueryGroup queryGroup, cxPortalWebService93.CxWSQuery query)
        {
            if (queryGroup == null)
                throw new ArgumentNullException(nameof(queryGroup));

            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (queryGroup.PackageType == cxPortalWebService93.CxWSPackageTypeEnum.Cx)
                return query.QueryId;

            if (_cxQueryId == null)
            {
                var queryGroups = GetQueries();

                _cxQueryId = new Dictionary<string, long>();

                foreach (var queryGroupCx in queryGroups.Where(x => x.PackageType == cxPortalWebService93.CxWSPackageTypeEnum.Cx))
                {
                    foreach (var queryCx in queryGroupCx.Queries)
                    {
                        _cxQueryId.Add(queryGroupCx.Language + "_" + queryGroupCx.Name + "_" + queryCx.Name, queryCx.QueryId);
                    }
                }

                foreach (var queryGroupCorporate in queryGroups.Where(x => x.PackageType == cxPortalWebService93.CxWSPackageTypeEnum.Corporate))
                {
                    foreach (var queryCorporate in queryGroupCorporate.Queries)
                    {
                        string queryPAth = queryGroupCorporate.Language + "_" + queryGroupCorporate.Name + "_" + queryCorporate.Name;

                        if (!_cxQueryId.ContainsKey(queryPAth))
                            _cxQueryId.Add(queryPAth, query.QueryId);
                    }
                }
            }

            string queryFullName = queryGroup.Language + "_" + queryGroup.Name + "_" + query.Name;
            if (_cxQueryId.ContainsKey(queryFullName))
                return _cxQueryId[queryFullName];

            return query.QueryId;
        }

        private Dictionary<long, Tuple<cxPortalWebService93.CxWSQueryGroup, cxPortalWebService93.CxWSQuery>> _queryVersionCache = null;
        public long GetPresetQueryId(long queryVersionCode)
        {
            if (_queryVersionCache == null)
            {
                _queryVersionCache = new Dictionary<long, Tuple<cxPortalWebService93.CxWSQueryGroup, cxPortalWebService93.CxWSQuery>>();
                foreach (var queryGroup in QueryGroupsByVersion)
                {
                    foreach (var query in queryGroup.Queries)
                    {
                        _queryVersionCache.Add(query.QueryVersionCode, new Tuple<cxPortalWebService93.CxWSQueryGroup, cxPortalWebService93.CxWSQuery>(queryGroup, query));
                    }
                }
            }

            var pair = _queryVersionCache[queryVersionCode];
            return GetPresetQueryId(pair.Item1, pair.Item2);
        }

        private cxPortalWebService93.CxWSQueryGroup[] _queryGroupCache = null;
        public Dictionary<cxPortalWebService93.CxWSQuery, cxPortalWebService93.CxWSQueryGroup> GetQueriesByLanguageAndOrName(string language, string queryName, bool onlyExecutable = false)
        {
            if (string.IsNullOrWhiteSpace(language) && string.IsNullOrWhiteSpace(queryName))
                throw new NullReferenceException("Between language and query name, at least one must have a value.");

            Dictionary<cxPortalWebService93.CxWSQuery, cxPortalWebService93.CxWSQueryGroup> foundQueries = new Dictionary<cxPortalWebService93.CxWSQuery, cxPortalWebService93.CxWSQueryGroup>();

            if (_queryGroupCache == null)
                _queryGroupCache = GetQueries().ToArray();

            if (!string.IsNullOrWhiteSpace(language))
            {
                var selectedGroups = _queryGroupCache.Where(x => x.LanguageName.ToLower() == language.Trim().ToLower());
                if (selectedGroups.Any())
                {
                    if (!string.IsNullOrWhiteSpace(queryName))
                    {
                        foreach (var g in selectedGroups)
                        {
                            foreach (var q in g.Queries.Where(x => !onlyExecutable || x.IsExecutable))
                            {
                                if (q.Name.ToLower() == queryName.Trim().ToLower())
                                    foundQueries.Add(q, g);
                            }
                        }
                    }
                    else
                    {
                        foreach (var g in selectedGroups)
                        {
                            foreach (var q in g.Queries.Where(x => !onlyExecutable || x.IsExecutable))
                                foundQueries.Add(q, g);
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    foreach (var g in _queryGroupCache)
                    {
                        foreach (var q in g.Queries.Where(x => !onlyExecutable || x.IsExecutable))
                        {
                            if (q.Name.ToLower() == queryName.Trim().ToLower())
                                foundQueries.Add(q, g);
                        }
                    }
                }
            }

            return foundQueries;
        }


        /// <summary>
        /// Get Query Information about the CWE of the Checkmarx Queries and other information.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<QueryInformationDTO> GetQueryInformation(bool detailedCategories = false, bool exportNonExecutableQueries = false, bool exportOnlyCx = false, params string[] presetNames)
        {
            Dictionary<string, HashSet<string>> standards = new Dictionary<string, HashSet<string>>();

            var queryGroups = this.GetQueries();

            var corpGroups = queryGroups.Where(x => x.PackageType == cxPortalWebService93.CxWSPackageTypeEnum.Corporate)
                          .ToDictionary(y => $"{y.LanguageName}:{y.Name}");

            // we need order
            Dictionary<string, HashSet<long>> presetQueries = new Dictionary<string, HashSet<long>>();

            if (presetNames != null)
            {
                var presets = GetPresets();

                foreach (var preset in presetNames)
                {
                    var presetId = presets.First(x => x.Value == preset);
                    presetQueries.Add(presetId.Value, GetPresetQueryIds(presetId.Key).ToHashSet());
                }
            }

            foreach (var queryGroup in queryGroups)
            {
                if (exportOnlyCx && queryGroup.PackageType != cxPortalWebService93.CxWSPackageTypeEnum.Cx)
                    continue;

                foreach (var query in queryGroup.Queries)
                {
                    if (!exportNonExecutableQueries && !query.IsExecutable)
                        continue;

                    List<string> categories = new List<string>();

                    foreach (var item in query.Categories)
                    {
                        if (!standards.ContainsKey(item.CategoryType.Name))
                            standards.Add(item.CategoryType.Name, new HashSet<string>(StringComparer.OrdinalIgnoreCase));

                        var subTopic = standards[item.CategoryType.Name];

                        if (!subTopic.Contains(item.CategoryName))
                            subTopic.Add(item.CategoryName);

                        categories.Add($"{item.CategoryName} [{item.CategoryType.Name}]");
                    }

                    long presetQueryId = GetPresetQueryId(queryGroup, query);

                    var queryInfo = new QueryInformationDTO
                    {
                        QueryId = query.QueryId,
                        PresetQueryId = presetQueryId,
                        Language = queryGroup.LanguageName,
                        PackageType = queryGroup.PackageTypeName,
                        QueryGroup = queryGroup.PackageFullName,
                        QueryName = query.Name,
                        IsExecutable = query.IsExecutable,
                        CWE = query.Cwe,
                        Severity = toSeverityToString(query.Severity),
                        SystemSeverity = toSeverityToString(query.Severity)
                    };

                    if (queryGroup.PackageType == cxPortalWebService93.CxWSPackageTypeEnum.Cx)
                    {
                        var queryGroupName = $"{queryGroup.LanguageName}:{queryGroup.Name}";

                        // check if the query is already overwritten in CORP
                        if (corpGroups.ContainsKey(queryGroupName))
                        {
                            var overriden = corpGroups[queryGroupName].Queries.SingleOrDefault(x => x.Name == query.Name);
                            if (overriden != null)
                                queryInfo.SystemSeverity = toSeverityToString(overriden.Severity);
                        }
                    }

                    if (detailedCategories)
                        queryInfo.Categories = string.Join(";", categories);

                    // add the presence of the queries in the presets.
                    queryInfo.PresetsPresence = new Dictionary<string, bool>();

                    foreach (var preset in presetQueries)
                    {
                        queryInfo.PresetsPresence.Add(preset.Key, preset.Value.Contains(presetQueryId));
                    }

                    yield return queryInfo;
                }
            }
        }

        public Dictionary<long, List<Tuple<long, string>>> GetQueryForCWE(ICollection<long> cwes)
        {
            // CWE Query Name
            var listOfQueriesForCWE = new Dictionary<long, List<Tuple<long, string>>>();

            foreach (var item in QueryGroups)
            {
                foreach (var query in item.Queries)
                {
                    if (cwes.Contains(query.Cwe))
                    {
                        string fullQueryName = item.PackageFullName + "::" + query.Name;

                        if (!listOfQueriesForCWE.ContainsKey(query.Cwe))
                            listOfQueriesForCWE.Add(query.Cwe, new List<Tuple<long, string>>() { new Tuple<long, string>(query.QueryId, fullQueryName) });
                        else
                            listOfQueriesForCWE[query.Cwe].Add(new Tuple<long, string>(query.QueryId, fullQueryName));
                    }
                }
            }

            return listOfQueriesForCWE;
        }

        public void AddCWESupportToExistentPreset(
            string presetFullFileName,
            Dictionary<long, List<Tuple<long, string>>> results,
            string outputFullFileName = null)
        {
            XDocument doc = XDocument.Load(presetFullFileName);

            List<long> queryIdInPreset = new List<long>();

            foreach (var xElement in doc.Descendants(XName.Get("OtherQueryId", doc.Root.GetDefaultNamespace().ToString())))
            {
                queryIdInPreset.Add(long.Parse(xElement.Value));
            }

            var queriesNode = doc.Descendants(XName.Get("OtherQueryIds")).First();

            foreach (var cweQuery in results)
            {
                foreach (var queryId in cweQuery.Value)
                {
                    if (!queryIdInPreset.Contains(queryId.Item1))
                    {
                        queriesNode.Add(new XElement("OtherQueryId", queryId.Item1));
                        queryIdInPreset.Add(queryId.Item1);
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(outputFullFileName))
                doc.Save(presetFullFileName);
            else
                doc.Save(outputFullFileName);
        }

        public void AddQueriesToPreset(string presetName, params long[] cxQueryIds)
        {
            if (string.IsNullOrWhiteSpace(presetName))
                throw new ArgumentNullException(nameof(presetName));

            if (cxQueryIds == null)
                throw new ArgumentNullException(nameof(cxQueryIds));

            checkConnection();

            int presetId = GetPresets().Where(x => x.Value == presetName).First().Key;

            var presetxmlResponse = _cxPortalWebServiceSoapClientV9.ExportPresetAsync(_soapSessionId, presetId).Result;

            checkSoapResponse(presetxmlResponse);

            using (MemoryStream ms = new MemoryStream(presetxmlResponse.Preset))
            {
                XDocument doc = XDocument.Load(ms);

                List<long> queryIdInPreset = new List<long>();

                foreach (var xElement in doc.Descendants(XName.Get("OtherQueryId", doc.Root.GetDefaultNamespace().ToString())))
                {
                    queryIdInPreset.Add(long.Parse(xElement.Value));
                }

                var queriesNode = doc.Descendants(XName.Get("OtherQueryIds")).First();

                foreach (var queryId in cxQueryIds)
                {
                    if (!queryIdInPreset.Contains(queryId))
                    {
                        queriesNode.Add(new XElement("OtherQueryId", queryId));
                        queryIdInPreset.Add(queryId);
                    }
                }

                using (MemoryStream newPreset = new MemoryStream())
                {
                    doc.Save(newPreset);
                    checkSoapResponse(_cxPortalWebServiceSoapClientV9.ImportPresetAsync(_soapSessionId, newPreset.ToArray()).Result.ImportPresetResult);
                }
            }
        }

        private IEnumerable<dynamic> _queryGroupsCache = null;
        public IEnumerable<dynamic> QueryGroups
        {
            get
            {
                if (_queryGroupsCache == null)
                {
                    checkConnection();
                    dynamic response = null;
                    if (_isV9)
                    {
                        response = _cxPortalWebServiceSoapClientV9
                              .GetQueryCollectionAsync(_soapSessionId).Result;
                    }
                    else
                    {
                        response = _cxPortalWebServiceSoapClient
                              .GetQueryCollectionAsync(_soapSessionId).Result;

                    }
                    checkSoapResponse(response);
                    _queryGroupsCache = response.QueryGroups;
                }
                return _queryGroupsCache;
            }
        }

        private IEnumerable<dynamic> _queryGroupsByVersionCache = null;
        public IEnumerable<dynamic> QueryGroupsByVersion
        {
            get
            {
                if (_queryGroupsByVersionCache == null)
                {
                    checkConnection();
                    dynamic response = null;
                    if (_isV9)
                    {
                        response = _cxPortalWebServiceSoapClientV9
                              .GetQueryCollectionWithInactiveAsync(_soapSessionId).Result;
                    }
                    else
                    {
                        response = _cxPortalWebServiceSoapClient
                              .GetQueryCollectionWithInactiveAsync(_soapSessionId).Result;

                    }
                    checkSoapResponse(response);
                    _queryGroupsByVersionCache = response.QueryGroups;
                }
                return _queryGroupsByVersionCache;
            }
        }

        public IEnumerable<dynamic> GetProjectLevelQueries(long projectId)
        {
            if (_isV9)
            {
                return QueryGroups
                .Where(x => x.PackageType == cxPortalWebService93.CxWSPackageTypeEnum.Project && x.ProjectId == projectId)
                .ToArray();
            }

            return QueryGroups
            .Where(x => x.PackageType == CxWSPackageTypeEnum.Project && x.ProjectId == projectId)
            .ToArray();
        }

        public IEnumerable<dynamic> GetTeamCorpLevelQueries(string teamId)
        {
            if (_isV9)
            {
                return QueryGroups
                .Where(x => (x.PackageType == cxPortalWebService93.CxWSPackageTypeEnum.Team && x.OwningTeam.ToString() == teamId.ToString()) || x.PackageType == cxPortalWebService93.CxWSPackageTypeEnum.Corporate)
                .ToArray();
            }

            return QueryGroups
            .Where(x => (x.PackageType == CxWSPackageTypeEnum.Team && x.OwningTeam.ToString() == teamId.ToString()) || x.PackageType == CxWSPackageTypeEnum.Corporate)
            .ToArray();
        }

        public IEnumerable<dynamic> GetCustomizedQueriesTeam(string teamid)
        {
            if (_isV9)
            {
                return QueryGroups
                    .Where(group => group.PackageType == cxPortalWebService93.CxWSPackageTypeEnum.Team)
                    .Where(group => group.OwningTeam.ToString() == teamid).ToArray();
            }

            return QueryGroups
                .Where(group => group.PackageType == CxWSPackageTypeEnum.Team)
                .Where(group => group.OwningTeam.ToString() == teamid).ToArray();
        }

        #endregion

        #region Engines

        /// <summary>
        /// Giving an LoC please look up
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="teamId"></param>
        /// <param name="loc"></param>
        /// <returns></returns>
        private int GetAvaliableSlot(ProjectDetails project, long loc)
        {
            int capacity = 0;

            var engines = SASTClientV5.EngineServers_GetV5Async().Result;

            foreach (var engine in engines)
            {
                // If the LoC is higher than the engine's max loc, skip it
                if (loc > engine.MaxLoc)
                    continue;

                var team = GetProjectTeamName(project.TeamId);

                // If the engine is dedicated to other projects or teams , skip it
                if (engine.Dedications.Any())
                {
                    if (!engine.Dedications
                        .Where(d =>
                        (d.ItemType == "Project" && d.ItemId == project.Id)
                     || (d.ItemType == "Team" && team.StartsWith(d.ItemName))).Any())
                        continue;
                }

                capacity += engine.MaxScans;
            }

            return 0;
        }

        #endregion

        #region Reports

        public IEnumerable<string> GetScannedLanguages(long scanId)
        {
            var log = GetScanLog(scanId);

            var langsTmp = new List<string>();

            //Languages that will be scanned: Java=3, CPP=1, JavaScript=1, Groovy=6, Kotlin=361
            Regex regexLang = new Regex("^Languages\\sthat\\swill\\sbe\\sscanned:\\s+(?:(\\w+)\\=\\d+\\,?\\s?)+", RegexOptions.Multiline);
            MatchCollection mcLang = regexLang.Matches(log);

            foreach (Match m in mcLang)
            {
                System.Text.RegularExpressions.GroupCollection groups = m.Groups;
                foreach (System.Text.RegularExpressions.Group g in groups)
                {
                    foreach (Capture c in g.Captures)
                    {
                        if (c.Value != "" && !c.Value.StartsWith("Languages that will be scanned:"))
                        {
                            langsTmp.Add(c.Value);
                        }
                    }
                }
            }

            return langsTmp;
        }

        public double GetScanCoverage(long scanId)
        {
            string log = GetScanLog(scanId);

            double firstFinalScanAccuracy = 0;

            Regex regex = new Regex("^Scan\\scoverage:\\s+(?<pc>[\\d\\.]+)\\%", RegexOptions.Multiline);
            MatchCollection mc = regex.Matches(log);
            foreach (Match m in mc)
            {
                GroupCollection groups = m.Groups;
                double.TryParse(groups["pc"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out firstFinalScanAccuracy);
            }

            return firstFinalScanAccuracy;
        }

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

            //Console.WriteLine(sb.ToString());

            var result = _cxPortalWebServiceSoapClient.GetScanLogs(objectName);

            if (!result.IsSuccesfull)
                throw new ActionNotSupportedException(result.ErrorMessage);

            return result.ScanLog;
        }

        public string GetScanLog(long scanId)
        {
            if (scanId < 1)
                throw new ArgumentOutOfRangeException(nameof(scanId));

            var zipFileBytes = GetScanLogs(scanId);

            // Create a MemoryStream over the byte array
            using (MemoryStream zipMemoryStream = new MemoryStream(zipFileBytes))
            {
                using (ZipArchive archive = new ZipArchive(zipMemoryStream, ZipArchiveMode.Read))
                {
                    // Iterate through the files in the archive
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.FullName.Equals($"Scan_{scanId}.zip", StringComparison.OrdinalIgnoreCase))
                        {
                            // Open a stream to the file within the ZIP
                            using (MemoryStream reader = new MemoryStream())
                            {
                                entry.Open().CopyTo(reader);
                                reader.Position = 0;

                                using (ZipArchive scanZipFile = new ZipArchive(reader, ZipArchiveMode.Read))
                                {
                                    var txtLogFile = scanZipFile.Entries.First();

                                    using (StreamReader txtReader = new StreamReader(txtLogFile.Open(), Encoding.UTF8))
                                    {
                                        // Read the contents of the file
                                        return txtReader.ReadToEnd();
                                    }
                                }

                            }
                        }
                    }
                }
            }

            throw new FileNotFoundException($"Log entry for the scan {scanId} was not found!");
        }

        public byte[] GetSystemLogs()
        {
            checkConnection();

            var result = _cxPortalWebServiceSoapClientV9.GetSystemLogsAsync(_soapSessionId).Result;

            checkSoapResponse(result);

            return result.SystemLogs;
        }

        /// <summary>
        /// Get the time it took for each executable query to run.
        /// </summary>
        /// <param name="scanId">Id of the scan</param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, TimeSpan>> GetQueriesRuntimeDuration(long scanId)
        {
            var log = GetScanLog(scanId);

            var reg = new Regex(@"Query\s+\-\s+(?<language>[^\.]+)\.([^\.]+)\.(?<queryGroup>[^\.]+)\.(?<queryName>[^\s]+)\s+Severity:\s(?<severity>[^\s]+)\s+([^\s]+)\s+([^\s]+)\s+\d+\s+Duration\s\=\s(?<duration>[^\s]+)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            var queryTimespans = new Dictionary<string, Dictionary<string, TimeSpan>>(StringComparer.OrdinalIgnoreCase);

            foreach (Match entry in reg.Matches(log))
            {
                string language = entry.Groups["language"].Value;

                if (!queryTimespans.ContainsKey(language))
                    queryTimespans.Add(language, new Dictionary<string, TimeSpan>(StringComparer.OrdinalIgnoreCase));

                string queryName = entry.Groups["queryName"].Value;

                queryTimespans[language].Add(queryName, TimeSpan.Parse(entry.Groups["duration"].Value));
            }

            return queryTimespans;
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

                var result = ExecuteWithRetryAsync(httpClient, request, CancellationToken.None).GetAwaiter().GetResult();
                var response = result.response;
                if (response.StatusCode == HttpStatusCode.Accepted)
                {
                    JObject createReport = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);

                    long reportId = (long)createReport["reportId"];

                    while (!isReportReady(reportId))
                    {
                        // wait 
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }

                    // Get Report
                    using (var getReport = new HttpRequestMessage(HttpMethod.Get, $"reports/sastScan/{reportId}"))
                    {
                        getReport.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue($"application/{reportTypeString}"));

                        var reportResult = ExecuteWithRetryAsync(httpClient, getReport, CancellationToken.None).GetAwaiter().GetResult();
                        var getReportResponse = reportResult.response;
                        if (getReportResponse.StatusCode == HttpStatusCode.OK)
                        {
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

        private bool isReportReady(long reportId)
        {
            if (!Connected)
                throw new NotSupportedException();

            using (var getReportStatus = new HttpRequestMessage(HttpMethod.Get, $"reports/sastScan/{reportId}/status"))
            {
                var result = ExecuteWithRetryAsync(httpClient, getReportStatus, CancellationToken.None).GetAwaiter().GetResult();
                var response = result.response;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    JObject reportStatus = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);

                    return (string)reportStatus.SelectToken("status").SelectToken("value") == "Created";
                }
            }

            return false;
        }

        #endregion

        #region Results

        public CxAuditWebServiceV9.AuditScanResult[] GetResult(long scanId)
        {
            return CxAuditV9.GetResultsAsync(_soapSessionId, scanId).Result.ResultCollection.Results;
        }

        public IEnumerable<SoapSingleResultData> GetResultsForScanByState(long scanId, ResultState state, bool includeInfoSeverityResults = true)
        {
            return GetResultsForScan(scanId, includeInfoSeverityResults).Where(x => x.State == (int)state);
        }

        public IEnumerable<SoapSingleResultData> GetResultsForScan(long scanId, bool includeInfoSeverityResults = true, bool includeNonExploitables = true, bool includeProposedNonExploitables = true, bool includeConfirmedResults = true)
        {
            return getScanResults(scanId, includeInfoSeverityResults, includeNonExploitables, includeProposedNonExploitables, includeConfirmedResults);
        }

        private IEnumerable<SoapSingleResultData> getScanResults(long scanId, bool includeInfoSeverityResults = true, bool includeNonExploitables = true, bool includeProposedNonExploitables = true, bool includeConfirmedResults = true)
        {
            checkConnection();

            IEnumerable<SoapSingleResultData> results = null;
            if (_isV94)
            {
                var response = _cxPriorityServiceSoapClient.GetResultsForScan(_soapSessionId, scanId);

                if (!response.IsSuccesfull)
                    throw new ApplicationException(response.ErrorMessage);

                results = response.Results?.Select(x => Mapper.MapPrioritySingleResultData(x));
            }
            else
            {
                var response = _cxPortalWebServiceSoapClient.GetResultsForScan(_soapSessionId, scanId);

                if (!response.IsSuccesfull)
                    throw new ApplicationException(response.ErrorMessage);

                results = response.Results?.Select(x => Mapper.MapSoapSingleResultData(x));
            }

            if (!includeInfoSeverityResults)
            {
                results = results.Where(x => x.Severity != (int)Severity.Info).ToArray();
            }

            if (!includeNonExploitables)
            {
                results = results.Where(x => x.State != (int)ResultState.NonExploitable).ToArray();
            }

            if (!includeProposedNonExploitables)
            {
                results = results.Where(x => x.State != (int)ResultState.ProposedNotExploitable).ToArray();
            }

            if (!includeConfirmedResults)
            {
                results = results.Where(x => x.State != (int)ResultState.Confirmed).ToArray();
            }

            return results;
        }

        public CxWSQueryVulnerabilityData[] GetQueriesForScan(long scanId)
        {
            checkConnection();

            var result = _cxPortalWebServiceSoapClient.GetQueriesForScan(_soapSessionId, scanId);

            if (!result.IsSuccesfull)
                throw new ApplicationException(result.ErrorMessage);

            return result.Queries;
        }

        public CxWSResponseScanProperties GetScanProperties(long scanId)
        {
            checkConnection();

            var result = _cxPortalWebServiceSoapClient.GetScanProperties(_soapSessionId, scanId);

            if (!result.IsSuccesfull)
                throw new ApplicationException(result.ErrorMessage);

            return result;
        }

        public int GetTotalConfirmedResultsForScan(long scanId)
        {
            return GetResultsForScan(scanId, false, false).Where(x => x.State == (int)ResultState.Confirmed || x.State == (int)ResultState.Urgent).Count();
        }

        /// <summary>
        /// Get the all query groups of the CxSAST server.
        /// </summary>
        /// <returns></returns>
        public cxPortalWebService93.CxWSQueryGroup[] GetQueries(bool includeAllQueryVersions = false)
        {
            checkConnection();

            cxPortalWebService93.CxQueryCollectionResponse result = null;
            if (includeAllQueryVersions)
                result = _cxPortalWebServiceSoapClientV9.GetQueryCollectionWithInactiveAsync(_soapSessionId).Result;
            else
                result = _cxPortalWebServiceSoapClientV9.GetQueryCollectionAsync(_soapSessionId).Result;

            if (!result.IsSuccesfull)
                throw new ApplicationException(result.ErrorMessage);

            return result.QueryGroups;
        }

        public CxAuditWebServiceV9.CxWSQueryGroup[] GetAuditQueries()
        {
            var result = CxAuditV9.GetQueryCollectionAsync(_soapSessionId).Result;

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

        private bool? isOsaAvailable;
        public bool IsOsaAvailable
        {
            get
            {


                if (isOsaAvailable == null)
                {
                    try
                    {
                        checkConnection();

                        isOsaAvailable = GetLicense().IsOsaEnabled;
                    }
                    catch
                    {
                        isOsaAvailable = false;
                    }
                }
                return isOsaAvailable.Value;
            }
        }

        /// <summary>
        /// Returns a list of tuples with comment remarks list per path id
        /// </summary>
        /// <param name="scanId"></param>
        /// <returns></returns>
        public List<Tuple<List<string>, long>> GetAllCommentRemarksForScan(long scanId)
        {
            var results = GetResultsForScan(scanId);
            var commentList = new List<Tuple<List<string>, long>>();
            foreach (var item in results)
            {
                var pathCommentList = new List<string>();
                if (!string.IsNullOrEmpty(item.Comment) && !string.IsNullOrWhiteSpace(item.Comment))
                {
                    var commentHistory = _cxPortalWebServiceSoapClient.GetPathCommentsHistory(_soapSessionId, scanId, item.PathId,
                        ResultLabelTypeEnum.Remark);
                    VerifyAndAddComment(commentHistory, pathCommentList);
                }
                if (pathCommentList.Any())
                {
                    commentList.Add(new Tuple<List<string>, long>(pathCommentList, item.PathId));
                }
            }

            return commentList;
        }

        public bool AllToVerifyScanResultsHaveComments(long scanId)
        {
            checkConnection();

            var results = GetODataResults(scanId).Where(x => x.StateId == (int)ResultState.ToVerify);

            return !results.Any(x => string.IsNullOrEmpty(x.Comment) || string.IsNullOrWhiteSpace(x.Comment));
        }

        public bool ScanHasResultsFlagedAsProposedNoExploitable(long scanId)
        {
            checkConnection();

            var results = GetODataResults(scanId).Where(x => x.StateId == (int)ResultState.ProposedNotExploitable);

            return results.Any();
        }

        public IEnumerable<SoapSingleResultData> GetScanResultsWithStateExclusions(long scanId, List<long> customStatesToExclude)
        {
            var results = GetResultsForScan(scanId);
            foreach (var result in results)
            {
                if (!customStatesToExclude.Any(y => y == result.State))
                    yield return result;
            }
        }

        public int GetTotalToVerifyFromScan(long scanId)
        {
            return GetODataResults(scanId).Where(x => x.StateId == (int)ResultState.ToVerify && x.Severity != CxDataRepository97.Severity.Info).Count();
        }

        public IEnumerable<CxDataRepository97.Result> GetNotExploitableFromScan(long scanId)
        {
            return GetODataResults(scanId).Where(x => x.StateId == (int)ResultState.NonExploitable).ToList();
        }

        /// <summary>
        /// Return a comment remark list for a specific scan and path id
        /// </summary>
        /// <param name="scanId"></param>
        /// <param name="pathId"></param>
        /// <returns></returns>
        public List<string> GetAllCommentRemarksForScanAndPath(long scanId, long pathId)
        {
            checkConnection();

            var commentList = new List<string>();

            VerifyAndAddComment(_cxPortalWebServiceSoapClient.GetPathCommentsHistory(_soapSessionId, scanId, pathId,
                    ResultLabelTypeEnum.Remark), commentList);

            return commentList;
        }

        private void VerifyAndAddComment(CxWSResponceResultPath cxWSResponceResultPath, List<string> pathCommentList)
        {
            if (cxWSResponceResultPath != null)
            {
                if (cxWSResponceResultPath.Path != null)
                {
                    if (cxWSResponceResultPath.Path.Comment != null)
                    {
                        var comments = cxWSResponceResultPath.Path.Comment.Split(new char[] { CommentSeparator }, StringSplitOptions.RemoveEmptyEntries);
                        pathCommentList.AddRange(comments);
                    }
                }
            }
        }


        public void UpdateResultState(long projectId, long scanId, long pathId, ResultState resultState, string remarks = null)
        {
            UpdateResultState(scanId, pathId, projectId, (int)resultState, remarks);
        }


        public void UpdateSetOfResultState(params ResultStateData[] results)
        {
            checkConnection();

            var response = _cxPortalWebServiceSoapClient.UpdateSetOfResultState(_soapSessionId, results);

            checkSoapResponse(response);
        }

        public void UpdateSetOfResultStateAndComment(ResultStateData[] results, string remark)
        {
            UpdateSetOfResultState(results.ToArray());

            // Workaround: Currently is not possible to change both Status and add a comment in the same call
            if (!string.IsNullOrWhiteSpace(remark))
            {
                foreach (var res in results)
                    res.ResultLabelType = (int)PortalSoap.ResultLabelTypeEnum.Remark;

                UpdateSetOfResultState(results.ToArray());
            }
        }

        public void UpdateResultState(long projectId, long scanId, long pathId, int result, string remarks = null)
        {
            if (projectId < 0)
                throw new ArgumentNullException(nameof(projectId));

            if (scanId < 0)
                throw new ArgumentNullException(nameof(scanId));

            if (pathId < 0)
                throw new ArgumentNullException(nameof(pathId));

            if (result < 0)
                throw new ArgumentNullException(nameof(result));

            checkConnection();

            var data = ((int)result).ToString();
            var response = _cxPortalWebServiceSoapClient.UpdateResultState(_soapSessionId, scanId, pathId, projectId, remarks, (int)ResultLabelTypeEnum.State, data);

            checkSoapResponse(response);
        }

        public void AddResultComment(long projectId, long scanId, long pathId, string comment)
        {
            checkConnection();

            var response = _cxPortalWebServiceSoapClient.UpdateResultState(_soapSessionId, scanId, pathId, projectId, comment, (int)ResultLabelTypeEnum.Remark, null);
        }

        public enum ResultState : int
        {
            ToVerify = 0,
            NonExploitable = 1,
            Confirmed = 2,
            Urgent = 3,
            ProposedNotExploitable = 4
        }

        public enum Severity
        {
            Info = 0,
            Low = 1,
            Medium = 2,
            High = 3,
            Critical = 4
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

            checkSoapResponse(result);

            return result.QueryDescription;
        }


        public static string toResultStateToString(ResultState state)
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

        public static string toSeverityToString(int severity)
        {
            switch (severity)
            {
                case 0:
                    return "Info";
                case 1:
                    return "Low";
                case 2:
                    return "Medium";
                case 3:
                    return "High";
                case 4:
                    return "Critical";
                default:
                    throw new NotImplementedException();
            }
        }


        public CxAuditWebServiceV9.FileExtension[] GetSupportedFileFormats()
        {
            var result = CxAuditV9.GetFilesExtensionsAsync(_soapSessionId).Result;

            checkSoapResponse(result);

            return result.fileExtensionsSetList;
        }

        #endregion

        #region Queries

        public Dictionary<CxDataRepository97.Severity, int> GetODataScanResultsQuerySeverityCounters(long scanId)
        {
            checkConnection();

            IEnumerable<CxDataRepository97.Result> results = null;
            string query = $"ScanId eq {scanId} and State ne null and QueryId ne null and StateId ne 1";

            if (_isV97)
            {
                var instruction = _oDataV97Results.AddQueryOption("$filter", query);
                results = instruction.Execute();
            }
            else
            {
                var instruction = _oDataResults.AddQueryOption("$filter", query);
                results = Mapper.MapOdataResults(instruction.Execute());
            }

            var dic = results
                .GroupBy(entity => entity.Severity)
                .ToDictionary(x => x.Key, y => y.Select(x => x.QueryId).Distinct().Count());

            if (!dic.ContainsKey(CxDataRepository97.Severity.Critical))
                dic.Add(CxDataRepository97.Severity.Critical, 0);

            if (!dic.ContainsKey(CxDataRepository97.Severity.High))
                dic.Add(CxDataRepository97.Severity.High, 0);

            if (!dic.ContainsKey(CxDataRepository97.Severity.Medium))
                dic.Add(CxDataRepository97.Severity.Medium, 0);

            if (!dic.ContainsKey(CxDataRepository97.Severity.Low))
                dic.Add(CxDataRepository97.Severity.Low, 0);

            return dic;
        }

        public Dictionary<CxDataRepository97.Severity, int> GetScanResultsQuerySeverityCounters(long scanId)
        {
            checkConnection();

            var severityCounters = GetResultsForScan(scanId).Where(x => x.QueryId != 0 && x.State != 1);

            //var dic = severityCounters
            //    .GroupBy(entity => entity.Severity)
            //    .ToDictionary(x => (CxDataRepository.Severity)x.Key, y => y.Select(x => x.QueryId).Distinct().Count());

            // The soap client is returning the vuln with the wrong severity in cases when a client changes the severity of a query.
            // The odata results is returning the right severity but it takes a long time to filter the results -> does not suport select/distinct operator directly
            // The solution for now is to check the severity of the query directly
            //var scanQueries = _cxPortalWebServiceSoapClient.GetQueriesForScan(_soapSessionId, scanId);
            var scanQueries = GetQueriesForScan(scanId);

            Dictionary<long, CxDataRepository97.Severity> queryDic = new Dictionary<long, CxDataRepository97.Severity>();
            var distinctQueries = severityCounters.Select(x => x.QueryId).Distinct();
            foreach (var item in distinctQueries)
            {
                var qry = scanQueries.Where(x => x.QueryId == item).FirstOrDefault();
                if (qry != null)
                    queryDic.Add(item, (CxDataRepository97.Severity)qry.Severity);
                else
                    queryDic.Add(item, (CxDataRepository97.Severity)severityCounters.FirstOrDefault(x => x.QueryId == item).Severity);
            }

            Dictionary<CxDataRepository97.Severity, int> dic = queryDic.GroupBy(x => x.Value).ToDictionary(x => x.Key, y => y.Count());

            if (!dic.ContainsKey(CxDataRepository97.Severity.Critical))
                dic.Add(CxDataRepository97.Severity.Critical, 0);

            if (!dic.ContainsKey(CxDataRepository97.Severity.High))
                dic.Add(CxDataRepository97.Severity.High, 0);

            if (!dic.ContainsKey(CxDataRepository97.Severity.Medium))
                dic.Add(CxDataRepository97.Severity.Medium, 0);

            if (!dic.ContainsKey(CxDataRepository97.Severity.Low))
                dic.Add(CxDataRepository97.Severity.Low, 0);

            if (!dic.ContainsKey(CxDataRepository97.Severity.Info))
                dic.Add(CxDataRepository97.Severity.Info, 0);

            return dic;
        }


        public void OverrideQueryInCorporate(CxAuditWebServiceV9.CxWSQueryGroup queryGroupSource,
            CxAuditWebServiceV9.CxWSQuery queryGroupSourceQuery, string customQuery, Severity severity, string description, bool alreadyExist = false)
        {
            if (queryGroupSource == null)
                throw new ArgumentNullException(nameof(queryGroupSource));

            if (queryGroupSourceQuery == null)
                throw new ArgumentNullException(nameof(queryGroupSourceQuery));

            if (string.IsNullOrWhiteSpace(customQuery))
                throw new ArgumentNullException(nameof(customQuery));

            // Create query objects and insert new
            CxAuditWebServiceV9.CxWSQuery queryToUp = new CxAuditWebServiceV9.CxWSQuery()
            {
                Cwe = queryGroupSourceQuery.Cwe,
                EngineMetadata = " ",
                Name = queryGroupSourceQuery.Name,
                Severity = (int)severity,
                Source = customQuery,
                Status = alreadyExist ? CxAuditWebServiceV9.QueryStatus.Edited : CxAuditWebServiceV9.QueryStatus.New,
                Type = CxAuditWebServiceV9.CxWSQueryType.Regular,
                IsExecutable = queryGroupSourceQuery.IsExecutable,
                IsEncrypted = queryGroupSourceQuery.IsEncrypted
            };

            string packageTypeName = $"Corp";
            string packageFullName = $"{queryGroupSource.LanguageName}:Corp:{queryGroupSource.Name}";
            var newQuerieGroup = new CxAuditWebServiceV9.CxWSQueryGroup()
            {
                Description = description,
                IsEncrypted = queryGroupSource.IsEncrypted,
                IsReadOnly = queryGroupSource.IsReadOnly,
                Language = queryGroupSource.Language,
                LanguageName = queryGroupSource.LanguageName,
                Name = queryGroupSource.Name,
                PackageType = CxAuditWebServiceV9.CxWSPackageTypeEnum.Corporate,
                PackageTypeName = packageTypeName,
                PackageFullName = packageFullName,
                Status = alreadyExist ? CxAuditWebServiceV9.QueryStatus.Edited : CxAuditWebServiceV9.QueryStatus.New,
                Queries = [queryToUp]
            };

            UploadQueries([newQuerieGroup]);
        }

        /// <summary>
        /// not tested.
        /// </summary>
        /// <param name="queryGroupSource"></param>
        /// <param name="queryGroupSourceQuery"></param>
        /// <param name="projectId"></param>
        /// <param name="teamId"></param>
        /// <param name="customQuery"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public CxAuditWebServiceV9.CxWSQueryGroup InsertQueryForProject(CxAuditWebServiceV9.CxWSQueryGroup queryGroupSource, CxAuditWebServiceV9.CxWSQuery queryGroupSourceQuery, long projectId, int teamId, string customQuery, string description)
        {
            if (queryGroupSource == null)
                throw new ArgumentNullException(nameof(queryGroupSource));

            if (queryGroupSourceQuery == null)
                throw new ArgumentNullException(nameof(queryGroupSourceQuery));

            if (string.IsNullOrWhiteSpace(customQuery))
                throw new ArgumentNullException(nameof(customQuery));

            // Create query objects and insert new
            CxAuditWebServiceV9.CxWSQuery queryToUp = new CxAuditWebServiceV9.CxWSQuery()
            {
                Cwe = queryGroupSourceQuery.Cwe,
                EngineMetadata = " ",
                Name = queryGroupSourceQuery.Name,
                Severity = queryGroupSourceQuery.Severity,
                Source = customQuery,
                Status = CxAuditWebServiceV9.QueryStatus.New,
                Type = CxAuditWebServiceV9.CxWSQueryType.Regular,
                IsExecutable = queryGroupSourceQuery.IsExecutable,
                IsEncrypted = queryGroupSourceQuery.IsEncrypted
            };

            string packageTypeName = $"CxProject_{projectId}";
            string packageFullName = $"{queryGroupSource.LanguageName}:CxProject_{projectId}:{queryGroupSource.Name}";
            var newQuerieGroup = new CxAuditWebServiceV9.CxWSQueryGroup()
            {
                Description = description,
                IsEncrypted = queryGroupSource.IsEncrypted,
                IsReadOnly = queryGroupSource.IsReadOnly,
                Language = queryGroupSource.Language,
                LanguageName = queryGroupSource.LanguageName,
                Name = queryGroupSource.Name,
                OwningTeam = teamId,
                PackageType = CxAuditWebServiceV9.CxWSPackageTypeEnum.Project,
                PackageTypeName = packageTypeName,
                PackageFullName = packageFullName,
                ProjectId = projectId,
                Status = CxAuditWebServiceV9.QueryStatus.New,
                Queries = [queryToUp]
            };

            UploadQueries([newQuerieGroup]);

            // For some reason, the description is not added when creating. We need to update again with the description
            var querieGroupsRefresh = GetAuditQueries();
            var createdQueryGroup = querieGroupsRefresh.Single(x => x.PackageType == CxAuditWebServiceV9.CxWSPackageTypeEnum.Project && x.ProjectId == projectId && x.PackageFullName == packageFullName);

            createdQueryGroup.Description = description;
            UploadQueries([createdQueryGroup]);

            return createdQueryGroup;
        }

        public void UploadQueries(CxAuditWebServiceV9.CxWSQueryGroup[] queries)
        {
            var uploadResponse = CxAuditV9.UploadQueriesAsync(_soapSessionId, queries).Result;

            if (!uploadResponse.IsSuccesfull)
                throw new ApplicationException(uploadResponse.ErrorMessage);
        }

        public void DeleteQueryGroup(CxAuditWebServiceV9.CxWSQueryGroup queryGroupToDelete)
        {
            if (queryGroupToDelete == null)
                throw new ArgumentNullException(nameof(queryGroupToDelete));

            foreach (var query in queryGroupToDelete.Queries)
                query.Status = CxAuditWebServiceV9.QueryStatus.Deleted;

            if (queryGroupToDelete.Queries.All(x => x.Status == CxAuditWebServiceV9.QueryStatus.Deleted))
                queryGroupToDelete.Status = CxAuditWebServiceV9.QueryStatus.Deleted;
            else
                throw new Exception("something didn't work well deleting this query group");

            UploadQueries([queryGroupToDelete]);
        }

        public void DeleteAnyQueryGroupWithTheDescription(string queryDescription)
        {
            if (string.IsNullOrWhiteSpace(queryDescription))
                throw new ArgumentNullException(nameof(queryDescription));

            var querieGroups = GetAuditQueries();
            var queryGroupSource = querieGroups.Where(x => x.Description == queryDescription);

            // Delete Query Group
            if (queryGroupSource.Any())
            {
                foreach (var createdQueryGroup in queryGroupSource)
                    DeleteQueryGroup(createdQueryGroup);
            }
        }

        #endregion

        #region Utils

        private string ConvertToString(object value, CultureInfo cultureInfo = null)
        {
            if (value == null)
            {
                return "";
            }

            if (cultureInfo == null)
                cultureInfo = CultureInfo.InvariantCulture;

            if (value is System.Enum)
            {
                var name = System.Enum.GetName(value.GetType(), value);
                if (name != null)
                {
                    var field = System.Reflection.IntrospectionExtensions.GetTypeInfo(value.GetType()).GetDeclaredField(name);
                    if (field != null)
                    {
                        var attribute = System.Reflection.CustomAttributeExtensions.GetCustomAttribute(field, typeof(System.Runtime.Serialization.EnumMemberAttribute))
                            as System.Runtime.Serialization.EnumMemberAttribute;
                        if (attribute != null)
                        {
                            return attribute.Value != null ? attribute.Value : name;
                        }
                    }

                    var converted = System.Convert.ToString(System.Convert.ChangeType(value, System.Enum.GetUnderlyingType(value.GetType()), cultureInfo));
                    return converted == null ? string.Empty : converted;
                }
            }
            else if (value is bool)
            {
                return System.Convert.ToString((bool)value, cultureInfo).ToLowerInvariant();
            }
            else if (value is byte[])
            {
                return System.Convert.ToBase64String((byte[])value);
            }
            else if (value.GetType().IsArray)
            {
                var array = System.Linq.Enumerable.OfType<object>((System.Array)value);
                return string.Join(",", System.Linq.Enumerable.Select(array, o => ConvertToString(o, cultureInfo)));
            }

            var result = System.Convert.ToString(value, cultureInfo);
            return result == null ? "" : result;
        }

        #endregion

        public void Dispose()
        {
            // There is logoff...
        }
    }
}