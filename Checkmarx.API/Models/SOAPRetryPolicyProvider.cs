using Polly;
using Polly.Timeout;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Checkmarx.API.Models
{
    public class SOAPRetryPolicyProvider
    {
        // Thread-safe caches for storing policies by (type, retryCount)
        private readonly ConcurrentDictionary<(Type, int), object> _syncPolicyCache = new();
        private readonly ConcurrentDictionary<(Type, int), object> _asyncPolicyCache = new();

        private readonly int _defaultRetries;

        public SOAPRetryPolicyProvider(int defaultRetries = 10)
        {
            _defaultRetries = defaultRetries;
        }

        // Synchronous Execution with Retry
        public T ExecuteWithRetry<T>(Func<T> action, int retries = -1)
        {
            var policy = GetOrCreateSyncPolicy<T>(retries);
            return policy.Execute(action);
        }

        // Asynchronous Execution with Retry
        public async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action, int retries = -1)
        {
            var policy = GetOrCreateAsyncPolicy<T>(retries);
            return await policy.ExecuteAsync(action);
        }

        #region Private Helpers

        private ISyncPolicy<T> GetOrCreateSyncPolicy<T>(int retries)
        {
            int effectiveRetries = retries > 0 ? retries : _defaultRetries;

            return (ISyncPolicy<T>)_syncPolicyCache.GetOrAdd(
                (typeof(T), effectiveRetries),
                _ => BuildRetryPolicy<T>(effectiveRetries)
            );
        }

        private IAsyncPolicy<T> GetOrCreateAsyncPolicy<T>(int retries)
        {
            int effectiveRetries = retries > 0 ? retries : _defaultRetries;

            return (IAsyncPolicy<T>)_asyncPolicyCache.GetOrAdd(
                (typeof(T), effectiveRetries),
                _ => BuildAsyncRetryPolicy<T>(effectiveRetries)
            );
        }

        #endregion

        #region Policy Builders

        private static ISyncPolicy<T> BuildRetryPolicy<T>(int retries)
        {
            var timeoutPolicy = Policy.Timeout<T>(
                TimeSpan.FromHours(1),
                TimeoutStrategy.Pessimistic,
                onTimeout: (context, timespan, task) =>
                    Console.WriteLine($"SOAP request timed out after {timespan.TotalSeconds} seconds."));

            var retryPolicy = Policy<T>
                .Handle<FaultException>()
                .Or<WebException>(IsTransientError)
                .Or<AggregateException>(IsTransientError)
                .Or<TimeoutException>(IsRetryableTimeout)
                .Or<TaskCanceledException>(IsRetryableTaskCanceled)
                .Or<HttpRequestException>(IsRetryableHttpRequest)
                .WaitAndRetry(
                    retryCount: retries,
                    sleepDurationProvider: (retryAttempt, outcome, context) => GetRetryDelayExponential<T>(outcome, retryAttempt),
                    onRetry: (outcome, timeSpan, retryCount, context) => OnRetry(outcome, timeSpan, retryCount, context)
                );

            return retryPolicy.Wrap(timeoutPolicy);
        }

        private static IAsyncPolicy<T> BuildAsyncRetryPolicy<T>(int retries)
        {
            var timeoutPolicy = Policy.TimeoutAsync<T>(
                TimeSpan.FromHours(1),
                TimeoutStrategy.Pessimistic,
                onTimeoutAsync: (context, timespan, task) =>
                {
                    Console.WriteLine($"SOAP request timed out after {timespan.TotalSeconds} seconds.");
                    return Task.CompletedTask;
                });

            var retryPolicy = Policy<T>
                .Handle<FaultException>()
                .Or<WebException>(IsTransientError)
                .Or<AggregateException>(IsTransientError)
                .Or<TimeoutException>(IsRetryableTimeout)
                .Or<TaskCanceledException>(IsRetryableTaskCanceled)
                .Or<HttpRequestException>(IsRetryableHttpRequest)
                .WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: (retryAttempt, outcome, context) => GetRetryDelayExponential<T>(outcome, retryAttempt),
                    onRetryAsync: async (outcome, timeSpan, retryCount, context) =>
                        await OnRetryAsync(outcome, timeSpan, retryCount, context)
                );

            return retryPolicy.WrapAsync(timeoutPolicy);
        }


        #endregion

        #region Support Methods

        private static bool IsTransientError(WebException ex)
        {
            if (ex.Response is HttpWebResponse response)
                return IsTransientStatusError((int)response.StatusCode);

            return ex.Status == WebExceptionStatus.Timeout;
        }

        private static bool IsRetryableHttpRequest(HttpRequestException ex)
        {
            if (ex.InnerException is WebException webEx)
                return IsTransientError(webEx);

            return false;
        }

        private static bool IsRetryableTaskCanceled(TaskCanceledException ex)
        {
            return !ex.CancellationToken.IsCancellationRequested; // Retry only if it's a timeout, not a user-cancel
        }

        private static bool IsRetryableTimeout(TimeoutException ex)
        {
            return true;
        }

        private static bool IsTransientError(AggregateException aggEx)
        {
            var flattened = aggEx.Flatten();

            return flattened.InnerExceptions.Any(inner =>
                inner is WebException webEx && IsTransientError(webEx) ||
                inner is HttpRequestException httpEx && IsRetryableHttpRequest(httpEx) ||
                inner is TimeoutException);
        }

        private static bool IsTransientStatusError(int statusCode)
        {
            return statusCode >= (int)HttpStatusCode.InternalServerError ||
                   statusCode == (int)HttpStatusCode.RequestTimeout ||
                   statusCode == (int)HttpStatusCode.TooManyRequests;
        }

        private static TimeSpan GetRetryDelayExponential<T>(DelegateResult<T> outcome, int retryAttempt)
        {
            var delay = TimeSpan.FromSeconds(Math.Min(Math.Pow(2, retryAttempt), 30));
            var jitter = TimeSpan.FromMilliseconds(Random.Shared.Next(0, 1000));
            return delay + jitter;
        }

        private static void OnRetry<T>(DelegateResult<T> outcome, TimeSpan timeSpan, int retryCount, Context context)
        {
            Console.WriteLine($"Retry {retryCount} after {timeSpan.TotalSeconds} seconds due to: " +
                              $"{(outcome.Exception != null ? outcome.Exception.Message : "SOAP Fault or Transient Error")}");
        }

        private static Task OnRetryAsync<T>(DelegateResult<T> outcome, TimeSpan timeSpan, int retryCount, Context context)
        {
            Console.WriteLine($"Retry {retryCount} after {timeSpan.TotalSeconds} seconds due to: " +
                              $"{(outcome.Exception != null ? outcome.Exception.Message : "SOAP Fault or Transient Error")}");
            return Task.CompletedTask;
        }

        #endregion
    }
}
