namespace CxDataRepository
{
/// <summary>
    /// There are no comments for QueryCxDescriptionSingle in the schema.
    /// </summary>
    public partial class QueryCxDescriptionSingle : global::Microsoft.OData.Client.DataServiceQuerySingle<QueryCxDescription>
    {
        /// <summary>
        /// Initialize a new QueryCxDescriptionSingle object.
        /// </summary>
        public QueryCxDescriptionSingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new QueryCxDescriptionSingle object.
        /// </summary>
        public QueryCxDescriptionSingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new QueryCxDescriptionSingle object.
        /// </summary>
        public QueryCxDescriptionSingle(global::Microsoft.OData.Client.DataServiceQuerySingle<QueryCxDescription> query)
            : base(query) {}

    }
}