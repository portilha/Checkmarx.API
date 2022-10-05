namespace CxDataRepository
{
/// <summary>
    /// There are no comments for QuerySourceSingle in the schema.
    /// </summary>
    public partial class QuerySourceSingle : global::Microsoft.OData.Client.DataServiceQuerySingle<QuerySource>
    {
        /// <summary>
        /// Initialize a new QuerySourceSingle object.
        /// </summary>
        public QuerySourceSingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new QuerySourceSingle object.
        /// </summary>
        public QuerySourceSingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new QuerySourceSingle object.
        /// </summary>
        public QuerySourceSingle(global::Microsoft.OData.Client.DataServiceQuerySingle<QuerySource> query)
            : base(query) {}

    }
}