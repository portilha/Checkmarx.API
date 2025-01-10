using Microsoft.OData.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Checkmarx.API.Models
{
    public class RetryableDataServiceQuery<T> : IQueryable<T>
    {
        private readonly DataServiceQuery<T> _innerQuery;

        public RetryableDataServiceQuery(DataServiceQuery<T> innerQuery)
        {
            _innerQuery = innerQuery;
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

        // Add retry logic when executing the query
        public IEnumerable<T> Execute()
        {
            return RetryPolicyProvider.ExecuteWithRetry(() => _innerQuery.Execute());
        }

        public DataServiceQuery<T> Expand(Expression<Func<T, object>> expandExpression)
        {
            var expandedQuery = _innerQuery.Expand(expandExpression);
            return new RetryableDataServiceQuery<T>(expandedQuery);
        }

        // Implicit conversion operator
        public static implicit operator DataServiceQuery<T>(RetryableDataServiceQuery<T> retryableQuery)
        {
            return retryableQuery._innerQuery;
        }
    }
}
