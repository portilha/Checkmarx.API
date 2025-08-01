using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Checkmarx.API.Models
{
    public static class RESTRetryPolicyProvider
    {
        internal static IAsyncPolicy<HttpResponseMessage> CreateRetryPolicy(int retries = 10)
        {
            if (retries < 1)
                throw new Exception("Number of REST policy number of retries must be greater than 0");

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(response => response.StatusCode == System.Net.HttpStatusCode.TooManyRequests) // Specifically handle 429
                .WaitAndRetryAsync(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, retryCount, context) =>
                {
                    // Optional: Log the retry attempt
                    Console.WriteLine($"Retry {retryCount} after {timeSpan.TotalSeconds} seconds due to: " +
                        $"{(exception.Exception != null ? exception.Exception.Message : $"{(int?)exception.Result?.StatusCode} {exception.Result?.ReasonPhrase}")}");
                });
        }

        public static HttpRequestMessage CloneHttpRequestMessage(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri);

            // Clone request content (if any)
            if (request.Content != null)
            {
                clone.Content = getHttpContentClone(request.Content);
                clone.Content.Headers.Clear();
                foreach (var header in request.Content.Headers)
                    clone.Content.Headers.Add(header.Key, header.Value);
            }

            // Clone the request headers
            foreach (var header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            // Copy other properties (e.g., Version)
            clone.Version = request.Version;

            return clone;
        }

        private static HttpContent getHttpContentClone(HttpContent content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            if (content is StreamContent)
            {
                return new StreamContent(content.ReadAsStreamAsync().Result);
            }
            else if (content is StringContent)
            {
                return new StringContent(content.ReadAsStringAsync().Result);
            }
            else if (content is ByteArrayContent)
            {
                return new ByteArrayContent(content.ReadAsByteArrayAsync().Result);
            }
            else if (content is FormUrlEncodedContent)
            {
                return new FormUrlEncodedContent(content.ReadAsStringAsync().Result.Split('&').Select(pair =>
                {
                    var kv = pair.Split('=');
                    return new KeyValuePair<string, string>(kv[0], kv.Length > 1 ? kv[1] : "");
                }));
            }
            else if (content is MultipartFormDataContent)
            {
                var multiPartContent = (MultipartFormDataContent)content;
                var newMultipartContent = new MultipartFormDataContent();
                foreach (var partContent in multiPartContent)
                {
                    newMultipartContent.Add(partContent, partContent.Headers.ContentDisposition.Name, partContent.Headers.ContentDisposition.FileName);
                }
                return newMultipartContent;
            }
            else
            {
                throw new NotSupportedException($"Unsupported content type: {content.GetType()}");
            }
        }
    }
}
