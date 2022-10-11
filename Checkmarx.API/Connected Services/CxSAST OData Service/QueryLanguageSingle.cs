namespace CxDataRepository
{
/// <summary>
    /// There are no comments for QueryLanguageSingle in the schema.
    /// </summary>
    public partial class QueryLanguageSingle : global::Microsoft.OData.Client.DataServiceQuerySingle<QueryLanguage>
    {
        /// <summary>
        /// Initialize a new QueryLanguageSingle object.
        /// </summary>
        public QueryLanguageSingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new QueryLanguageSingle object.
        /// </summary>
        public QueryLanguageSingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new QueryLanguageSingle object.
        /// </summary>
        public QueryLanguageSingle(global::Microsoft.OData.Client.DataServiceQuerySingle<QueryLanguage> query)
            : base(query) {}

    }
}