using Microsoft.OData.Client;
using Polly;
using Polly.Timeout;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Checkmarx.API.Models
{
    public class RetryableDataServiceQuery<T> : IQueryable<T>
    {
        private readonly IQueryable<T> _innerQuery;
        private readonly int _retryCount;
        private readonly ISyncPolicy<IEnumerable<T>> _retryPolicy;
        private readonly ISyncPolicy<object> _genericRetryPolicy;
        private readonly RetryableQueryProvider<T> _retryableProvider;

        public RetryableDataServiceQuery(IQueryable<T> innerQuery, int retryCount = 10)
        {
            _innerQuery = innerQuery;
            _retryCount = retryCount;
            _retryPolicy = buildRetryPolicy(_retryCount);
            _genericRetryPolicy = buildGenericRetryPolicy(_retryCount);

            _retryableProvider = new RetryableQueryProvider<T>(_innerQuery.Provider, _retryPolicy, _genericRetryPolicy, retryCount);
        }

        public IEnumerator<T> GetEnumerator()
        {
            var result = (IEnumerable<T>)_retryableProvider.Execute(Expression);
            return result.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType => _innerQuery.ElementType;
        public Expression Expression => _innerQuery.Expression;
        public IQueryProvider Provider => _retryableProvider;

        public IEnumerable<T> Execute()
        {
            return _retryPolicy.Execute(() => _innerQuery.ToList());
        }

        public RetryableDataServiceQuery<T> Expand(Expression<Func<T, object>> expandExpression)
        {
            var expandedQuery = (_innerQuery as DataServiceQuery<T>)?.Expand(expandExpression);

            if (expandedQuery == null)
                throw new InvalidCastException("The inner query is not of type DataServiceQuery<T>");

            return new RetryableDataServiceQuery<T>(expandedQuery, _retryCount);
        }

        public RetryableDataServiceQuery<T> Expand(string expandQuery)
        {
            var expandedQuery = (_innerQuery as DataServiceQuery<T>)?.Expand(expandQuery);

            if (expandedQuery == null)
                throw new InvalidCastException("The inner query is not of type DataServiceQuery<T>");

            return new RetryableDataServiceQuery<T>(expandedQuery, _retryCount);
        }

        public RetryableDataServiceQuery<T> AddQueryOption(string name, string value)
        {
            if (_innerQuery is DataServiceQuery<T> dataServiceQuery)
            {
                var updatedQuery = dataServiceQuery.AddQueryOption(name, value);

                return new RetryableDataServiceQuery<T>(updatedQuery, _retryCount);
            }
            else
            {
                throw new InvalidOperationException("AddQueryOption is not supported for the current query type.");
            }
        }

        public static explicit operator DataServiceQuery<T>(RetryableDataServiceQuery<T> retryableQuery)
        {
            return retryableQuery._innerQuery as DataServiceQuery<T> ?? throw new InvalidCastException("Cannot cast to DataServiceQuery<T>");
        }

        #region Policy Builders

        private static TimeSpan getRetryDelay(int retryAttempt)
        {
            var delay = TimeSpan.FromSeconds(Math.Min(Math.Pow(2, retryAttempt), 30));
            var jitter = TimeSpan.FromMilliseconds(Random.Shared.Next(0, 1000));
            return delay + jitter;
        }

        private static void onRetry<TResult>(DelegateResult<TResult> outcome, TimeSpan timespan, int retryAttempt, Context context)
        {
            Exception ex = outcome.Exception;
            string message = ex?.Message ?? "Unknown error";

            if (ex is HttpRequestException httpEx && httpEx.InnerException != null)
                message = httpEx.InnerException.Message;
            else if (ex is WebException webEx)
                message = webEx.Message;

            Console.WriteLine($"Retry {retryAttempt} after {timespan.TotalSeconds:F1} seconds due to: {message}");
        }

        private static ISyncPolicy<IEnumerable<T>> buildRetryPolicy(int retries)
        {
            var timeoutPolicy = Policy.Timeout<IEnumerable<T>>(
                TimeSpan.FromHours(1),
                TimeoutStrategy.Pessimistic,
                onTimeout: (context, timespan, task) =>
                    Console.WriteLine($"OData request timed out after {timespan.TotalSeconds} seconds."));

            var retryPolicy = Policy<IEnumerable<T>>
                .Handle<DataServiceQueryException>(isTransientError)
                .Or<HttpRequestException>(isTransientError)
                .Or<WebException>(isTransientWebError)
                .Or<TaskCanceledException>(isRetryableTaskCanceled)
                .WaitAndRetry(retries, getRetryDelay, onRetry);

            return retryPolicy.Wrap(timeoutPolicy);
        }

        private static ISyncPolicy<object> buildGenericRetryPolicy(int retries)
        {
            var timeoutPolicy = Policy.Timeout<object>(
                TimeSpan.FromHours(1),
                TimeoutStrategy.Pessimistic,
                onTimeout: (context, timespan, task) =>
                    Console.WriteLine($"OData request timed out after {timespan.TotalSeconds} seconds."));

            var retryPolicy = Policy<object>
                .Handle<DataServiceQueryException>(isTransientError)
                .Or<HttpRequestException>(isTransientError)
                .Or<WebException>(isTransientWebError)
                .Or<TaskCanceledException>(isRetryableTaskCanceled)
                .WaitAndRetry(retries, getRetryDelay, onRetry);

            return retryPolicy.Wrap(timeoutPolicy);
        }

        private static bool isRetryableTaskCanceled(TaskCanceledException ex)
        {
            // Retry only on HTTP-layer timeouts, not on explicit user/application cancellations
            return !ex.CancellationToken.IsCancellationRequested;
        }

        private static bool isTransientError(DataServiceQueryException ex)
        {
            return isTransientCode(ex.Response?.StatusCode);
        }

        private static bool isTransientError(HttpRequestException ex)
        {
            if (ex.InnerException is WebException webEx && webEx.Response is HttpWebResponse response)
            {
                int statusCode = (int)response.StatusCode;
                return isTransientCode(statusCode);
            }
            return false;
        }

        private static bool isTransientWebError(WebException ex)
        {
            if (ex.Response is HttpWebResponse response)
            {
                int statusCode = (int)response.StatusCode;
                return isTransientCode(statusCode);
            }
            return ex.Status == WebExceptionStatus.Timeout;
        }

        private static bool isTransientCode(int? statusCode)
        {
            if (statusCode != null)
                return statusCode >= 500 || statusCode == 408;

            return false;
        }

        #endregion
    }


    public class RetryableQueryProvider<T> : IQueryProvider
    {
        private readonly IQueryProvider _innerProvider;
        private readonly ISyncPolicy<IEnumerable<T>> _retryPolicy;
        private readonly ISyncPolicy<object> _genericRetryPolicy;
        private readonly int _retryCount;

        public RetryableQueryProvider(IQueryProvider innerProvider, ISyncPolicy<IEnumerable<T>> retryPolicy, ISyncPolicy<object> genericRetryPolicy, int retryCount = 10)
        {
            _innerProvider = innerProvider;
            _retryPolicy = retryPolicy;
            _genericRetryPolicy = genericRetryPolicy;
            _retryCount = retryCount;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            var query = _innerProvider.CreateQuery<T>(expression);
            return new RetryableDataServiceQuery<T>(query, _retryCount);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            var query = _innerProvider.CreateQuery<TElement>(expression);
            return new RetryableDataServiceQuery<TElement>(query, _retryCount);
        }

        public object Execute(Expression expression)
        {
            var queryable = _innerProvider.CreateQuery<T>(expression);
            return executeWithExceptionHandling(() => _retryPolicy.Execute(() => queryable.ToList()));
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var queryable = _innerProvider.CreateQuery<T>(expression);

            return executeWithExceptionHandling(() =>
            {
                if (typeof(TResult).IsGenericType && typeof(TResult).GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    return (TResult)(object)_retryPolicy.Execute(() => queryable.ToList());
                else
                    return (TResult)_genericRetryPolicy.Execute(() => queryable.Provider.Execute(expression));
            });
        }

        private static TResult executeWithExceptionHandling<TResult>(Func<TResult> action)
        {
            try
            {
                return action();
            }
            catch (TargetInvocationException ex) when (ex.InnerException is DataServiceQueryException queryEx)
            {
                throw queryEx;
            }
            catch (TargetInvocationException ex) when (ex.InnerException is DataServiceClientException clientEx)
            {
                throw clientEx;
            }
        }
    }
}
