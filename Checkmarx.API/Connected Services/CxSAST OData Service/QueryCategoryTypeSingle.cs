namespace CxDataRepository
{
/// <summary>
    /// There are no comments for QueryCategoryTypeSingle in the schema.
    /// </summary>
    public partial class QueryCategoryTypeSingle : global::Microsoft.OData.Client.DataServiceQuerySingle<QueryCategoryType>
    {
        /// <summary>
        /// Initialize a new QueryCategoryTypeSingle object.
        /// </summary>
        public QueryCategoryTypeSingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new QueryCategoryTypeSingle object.
        /// </summary>
        public QueryCategoryTypeSingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new QueryCategoryTypeSingle object.
        /// </summary>
        public QueryCategoryTypeSingle(global::Microsoft.OData.Client.DataServiceQuerySingle<QueryCategoryType> query)
            : base(query) {}

        /// <summary>
        /// There are no comments for QueryCategories in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QueryCategory> QueryCategories
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._QueryCategories == null))
                {
                    this._QueryCategories = Context.CreateQuery<global::CxDataRepository.QueryCategory>(GetPath("QueryCategories"));
                }
                return this._QueryCategories;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QueryCategory> _QueryCategories;
    }
}