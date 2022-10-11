namespace CxDataRepository
{
/// <summary>
    /// There are no comments for QueryCweDescriptionSingle in the schema.
    /// </summary>
    public partial class QueryCweDescriptionSingle : global::Microsoft.OData.Client.DataServiceQuerySingle<QueryCweDescription>
    {
        /// <summary>
        /// Initialize a new QueryCweDescriptionSingle object.
        /// </summary>
        public QueryCweDescriptionSingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new QueryCweDescriptionSingle object.
        /// </summary>
        public QueryCweDescriptionSingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new QueryCweDescriptionSingle object.
        /// </summary>
        public QueryCweDescriptionSingle(global::Microsoft.OData.Client.DataServiceQuerySingle<QueryCweDescription> query)
            : base(query) {}

    }
}