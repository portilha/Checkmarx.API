using Microsoft.OData.Client;
using Polly;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;

namespace Checkmarx.API.Models
{
    public class RetryableDataServiceQuery<T> : IQueryable<T>
    {
        private readonly DataServiceQuery<T> _innerQuery;
        private readonly int _retryCount;
        private readonly Policy<IEnumerable<T>> _retryPolicy;

        public RetryableDataServiceQuery(DataServiceQuery<T> innerQuery, int retryCount = 10)
        {
            _innerQuery = innerQuery;
            _retryCount = retryCount;
            _retryPolicy = BuildRetryPolicy(_retryCount);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _innerQuery.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _innerQuery.GetEnumerator();
        }

        public Type ElementType => _innerQuery.ElementType;
        public Expression Expression => _innerQuery.Expression;
        public IQueryProvider Provider => _innerQuery.Provider;

        // Execute query with retry logic
        public IEnumerable<T> Execute()
        {
            return _retryPolicy.Execute(() => _innerQuery.Execute());
        }

        public DataServiceQuery<T> Expand(Expression<Func<T, object>> expandExpression)
        {
            var expandedQuery = _innerQuery.Expand(expandExpression);
            return new RetryableDataServiceQuery<T>(expandedQuery, _retryCount);
        }

        public static implicit operator DataServiceQuery<T>(RetryableDataServiceQuery<T> retryableQuery)
        {
            return retryableQuery._innerQuery;
        }

        #region Policy Builders

        private static Policy<IEnumerable<T>> BuildRetryPolicy(int retries)
        {
            return Policy<IEnumerable<T>>
                .Handle<HttpRequestException>(ex => IsTransientError(ex))
                .Or<WebException>(ex => IsTransientWebError(ex))
                .WaitAndRetry(
                    retries,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (outcome, timespan, retryAttempt, context) =>
                    {
                        Exception ex = outcome.Exception;
                        string message = ex?.Message ?? "Unknown error";

                        if (ex is HttpRequestException httpEx && httpEx.InnerException != null)
                            message = httpEx.InnerException.Message;
                        else if (ex is WebException webEx)
                            message = webEx.Message;

                        Console.WriteLine($"Retry {retryAttempt} after {timespan.TotalSeconds} seconds due to: {message}");
                    });
        }

        private static bool IsTransientError(HttpRequestException ex)
        {
            if (ex.InnerException is WebException webEx && webEx.Response is HttpWebResponse response)
            {
                int statusCode = (int)response.StatusCode;
                return statusCode >= 500 || statusCode == 408;
            }
            return false;
        }

        private static bool IsTransientWebError(WebException ex)
        {
            if (ex.Response is HttpWebResponse response)
            {
                int statusCode = (int)response.StatusCode;
                return statusCode >= 500 || statusCode == 408;
            }
            return ex.Status == WebExceptionStatus.Timeout;
        }

        #endregion
    }

}
