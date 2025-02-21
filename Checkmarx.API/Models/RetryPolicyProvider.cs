using Polly;
using Polly.Retry;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Checkmarx.API.Models
{
    public class RetryPolicyProvider
    {
        // Thread-safe caches for storing policies by type
        private readonly ConcurrentDictionary<Type, object> _syncPolicyCache = new();
        private readonly ConcurrentDictionary<Type, object> _asyncPolicyCache = new();

        private readonly int _defaultRetries;

        public RetryPolicyProvider(int defaultRetries = 10)
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

        private RetryPolicy<T> GetOrCreateSyncPolicy<T>(int retries)
        {
            int effectiveRetries = retries > 0 ? retries : _defaultRetries;
            return (RetryPolicy<T>)_syncPolicyCache.GetOrAdd(
                typeof(T),
                _ => BuildRetryPolicy<T>(effectiveRetries)
            );
        }

        private AsyncRetryPolicy<T> GetOrCreateAsyncPolicy<T>(int retries)
        {
            int effectiveRetries = retries > 0 ? retries : _defaultRetries;
            return (AsyncRetryPolicy<T>)_asyncPolicyCache.GetOrAdd(
                typeof(T),
                _ => BuildAsyncRetryPolicy<T>(effectiveRetries)
            );
        }

        #endregion

        #region Policy Builders

        private static RetryPolicy<T> BuildRetryPolicy<T>(int retries)
        {
            return Policy<T>
                .Handle<FaultException>()
                .Or<WebException>(IsTransientError)
                .WaitAndRetry(
                    retries,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (outcome, timeSpan, retryCount, context) =>
                        OnRetry(outcome, timeSpan, retryCount, context));
        }

        private static AsyncRetryPolicy<T> BuildAsyncRetryPolicy<T>(int retries)
        {
            return Policy<T>
                .Handle<FaultException>()
                .Or<WebException>(IsTransientError)
                .WaitAndRetryAsync(
                    retries,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    async (outcome, timeSpan, retryCount, context) =>
                        await OnRetryAsync(outcome, timeSpan, retryCount, context));
        }

        #endregion

        #region Support Methods

        private static bool IsTransientError(WebException ex)
        {
            if (ex.Response is HttpWebResponse response)
            {
                if ((int)response.StatusCode >= 500 || response.StatusCode == HttpStatusCode.RequestTimeout)
                {
                    return true;
                }
            }
            return ex.Status == WebExceptionStatus.Timeout;
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
