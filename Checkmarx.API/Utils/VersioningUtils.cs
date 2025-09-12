using Checkmarx.API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;

namespace Checkmarx.API.Utils
{
    public static class VersioningUtils
    {
        public static bool SupportsAPIMethod(HttpClient _httpClient, ApiVersionEnum apiVersion, string methodType, string method)
        {
            var version = apiVersion.ToString().Replace("v", "").Replace("_", ".");

            var result = supportsAPIMethod(_httpClient, version, methodType, method);
            if (!result && apiVersion != ApiVersionEnum.latest)
                result = supportsAPIMethod(_httpClient, ApiVersionEnum.latest.ToString(), methodType, method, $"application/json;v={version}");

            return result;
        }

        private static bool supportsAPIMethod(HttpClient _httpClient, string apiVersion, string methodType, string method, string contentType = null)
        {
            try
            {
                var version = apiVersion == ApiVersionEnum.latest.ToString() ? apiVersion : $"v{apiVersion}";

                // Get Swagger JSON
                using var request = new HttpRequestMessage(HttpMethod.Get, $"help/swagger/docs/{version}");
                var response = _httpClient.SendAsync(request).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                    return false;

                var swaggerJson = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var swagger = JObject.Parse(swaggerJson);

                // Look for paths
                var paths = swagger["paths"] as JObject;
                if (paths == null)
                    return false;

                // Find /projects path ignoring case
                var projectPath = paths.Properties()
                    .FirstOrDefault(p => string.Equals(p.Name, method, StringComparison.OrdinalIgnoreCase));

                if (projectPath == null)
                    return false;

                var methodNode = projectPath.Value[methodType];
                if (methodNode == null)
                    return false;

                var responses = methodNode["responses"];
                if (responses == null)
                    return false;

                if (!string.IsNullOrWhiteSpace(contentType))
                {
                    // Swagger 2.0 uses "produces" instead of responses.content
                    var produces = methodNode["produces"]?.Values<string>() ?? Enumerable.Empty<string>();

                    return produces.Any(p => p.Equals(contentType, StringComparison.OrdinalIgnoreCase));
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
