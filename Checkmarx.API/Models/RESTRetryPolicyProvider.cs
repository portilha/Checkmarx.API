using Polly;
using Polly.Timeout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Checkmarx.API.Models
{
    public static class RESTRetryPolicyProvider
    {
        internal static IAsyncPolicy<HttpResponseMessage> CreateRetryPolicy(int retries = 10)
        {
            if (retries < 1)
                throw new Exception("Number of REST policy number of retries must be greater than 0");

            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(
                TimeSpan.FromSeconds(300),
                TimeoutStrategy.Pessimistic,
                onTimeoutAsync: (context, timespan, task, exception) =>
                {
                    Console.WriteLine($"Request timed out after {timespan.TotalSeconds} seconds.");
                    return Task.CompletedTask;
                });

            var retryPolicy = Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                //.Or<TimeoutRejectedException>()
                .OrResult(response =>
                    response.StatusCode == HttpStatusCode.TooManyRequests ||
                    ((int)response.StatusCode >= 500 && (int)response.StatusCode <= 599))
                .WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: (retryAttempt, outcome, context) =>
                    {
                        //const string TooManyRequestsKey = "HadTooManyRequests";

                        //if (outcome.Result?.StatusCode == HttpStatusCode.TooManyRequests)
                        //{
                        //    context[TooManyRequestsKey] = true;
                        //    return TimeSpan.FromSeconds(10);
                        //}

                        //if (context.TryGetValue(TooManyRequestsKey, out var had429) && had429 is bool b && b)
                        //{
                        //    return TimeSpan.FromSeconds(10);
                        //}

                        return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                    },
                    onRetryAsync: async (outcome, timespan, retryCount, context) =>
                    {
                        string message;
                        if (outcome.Exception != null)
                        {
                            message = outcome.Exception.Message;
                        }
                        else if (outcome.Result != null)
                        {
                            var statusCode = outcome.Result.StatusCode;
                            var statusCodeInt = (int)statusCode;
                            var reason = string.IsNullOrWhiteSpace(outcome.Result.ReasonPhrase)
                                ? statusCode.ToString()
                                : outcome.Result.ReasonPhrase;

                            message = $"{statusCodeInt} {reason}";
                        }
                        else
                        {
                            message = "Unknown error";
                        }

                        Console.WriteLine($"Retry {retryCount} after {timespan.TotalSeconds} seconds due to: {message}");
                        await Task.CompletedTask;
                    });

            var fallbackPolicy = Policy<HttpResponseMessage>
                .Handle<Exception>(ex => !(ex is TimeoutRejectedException))  // Exclude timeout here
                .OrResult(response => !response.IsSuccessStatusCode)
                .FallbackAsync(
                    fallbackValue: new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                    {
                        ReasonPhrase = "Fallback response"
                    },
                    onFallbackAsync: (outcome, context) =>
                    {
                        Console.WriteLine($"Fallback triggered due to: {outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()}");
                        return Task.CompletedTask;
                    });

            return fallbackPolicy.WrapAsync(retryPolicy.WrapAsync(timeoutPolicy));
        }

        public static Task<HttpResponseMessage> ExecuteWithPolicyAsync(IAsyncPolicy<HttpResponseMessage> policy, HttpClient client, HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            return policy.ExecuteAsync(
                (context, token) =>
                    client.SendAsync(
                        CloneHttpRequestMessage(request),
                        HttpCompletionOption.ResponseHeadersRead,
                        token),
                new Context(),  // or pass one if needed
                cancellationToken);
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
