namespace CxDataRepository
{
/// <summary>
    /// There are no comments for QueryCategorySingle in the schema.
    /// </summary>
    public partial class QueryCategorySingle : global::Microsoft.OData.Client.DataServiceQuerySingle<QueryCategory>
    {
        /// <summary>
        /// Initialize a new QueryCategorySingle object.
        /// </summary>
        public QueryCategorySingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new QueryCategorySingle object.
        /// </summary>
        public QueryCategorySingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new QueryCategorySingle object.
        /// </summary>
        public QueryCategorySingle(global::Microsoft.OData.Client.DataServiceQuerySingle<QueryCategory> query)
            : base(query) {}

        /// <summary>
        /// There are no comments for QueryCategoryType in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.QueryCategoryTypeSingle QueryCategoryType
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._QueryCategoryType == null))
                {
                    this._QueryCategoryType = new global::CxDataRepository.QueryCategoryTypeSingle(this.Context, GetPath("QueryCategoryType"));
                }
                return this._QueryCategoryType;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.QueryCategoryTypeSingle _QueryCategoryType;
        /// <summary>
        /// There are no comments for Queries in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.Query> Queries
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._Queries == null))
                {
                    this._Queries = Context.CreateQuery<global::CxDataRepository.Query>(GetPath("Queries"));
                }
                return this._Queries;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.Query> _Queries;
    }
}