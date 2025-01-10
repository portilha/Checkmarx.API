using Polly;
using Polly.Retry;
using System;
using System.Threading.Tasks;

namespace Checkmarx.API.Models
{
    public static class RetryPolicyProvider
    {
        // Synchronous Retry Policy
        public static T ExecuteWithRetry<T>(Func<T> action, int retries = 10)
        {
            var retryPolicy = RetryPolicyProvider.getRetryPolicy<T>(retries);
            return retryPolicy.Execute(action);
        }

        // Asynchronous Retry Policy
        public static async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action, int retries = 10)
        {
            var retryPolicy = RetryPolicyProvider.getAsyncRetryPolicy<T>(retries);
            return await retryPolicy.ExecuteAsync(action);
        }

        #region Private Methods

        private static RetryPolicy<T> getRetryPolicy<T>(int retries = 10)
        {
            return Policy<T>
                .Handle<Exception>()
                .WaitAndRetry(retries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static AsyncRetryPolicy<T> getAsyncRetryPolicy<T>(int retries = 10)
        {
            return Policy<T>
                .Handle<Exception>()
                .WaitAndRetryAsync(retries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        #endregion
    }
}
