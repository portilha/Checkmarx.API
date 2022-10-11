namespace CxDataRepository
{
/// <summary>
    /// There are no comments for QuerySingle in the schema.
    /// </summary>
    public partial class QuerySingle : global::Microsoft.OData.Client.DataServiceQuerySingle<Query>
    {
        /// <summary>
        /// Initialize a new QuerySingle object.
        /// </summary>
        public QuerySingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new QuerySingle object.
        /// </summary>
        public QuerySingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new QuerySingle object.
        /// </summary>
        public QuerySingle(global::Microsoft.OData.Client.DataServiceQuerySingle<Query> query)
            : base(query) {}

        /// <summary>
        /// There are no comments for QueryCxDescription in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.QueryCxDescriptionSingle QueryCxDescription
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._QueryCxDescription == null))
                {
                    this._QueryCxDescription = new global::CxDataRepository.QueryCxDescriptionSingle(this.Context, GetPath("QueryCxDescription"));
                }
                return this._QueryCxDescription;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.QueryCxDescriptionSingle _QueryCxDescription;
        /// <summary>
        /// There are no comments for QueryCweDescription in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.QueryCweDescriptionSingle QueryCweDescription
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._QueryCweDescription == null))
                {
                    this._QueryCweDescription = new global::CxDataRepository.QueryCweDescriptionSingle(this.Context, GetPath("QueryCweDescription"));
                }
                return this._QueryCweDescription;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.QueryCweDescriptionSingle _QueryCweDescription;
        /// <summary>
        /// There are no comments for QuerySource in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.QuerySourceSingle QuerySource
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._QuerySource == null))
                {
                    this._QuerySource = new global::CxDataRepository.QuerySourceSingle(this.Context, GetPath("QuerySource"));
                }
                return this._QuerySource;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.QuerySourceSingle _QuerySource;
        /// <summary>
        /// There are no comments for QueryGroup in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.QueryGroupSingle QueryGroup
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._QueryGroup == null))
                {
                    this._QueryGroup = new global::CxDataRepository.QueryGroupSingle(this.Context, GetPath("QueryGroup"));
                }
                return this._QueryGroup;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.QueryGroupSingle _QueryGroup;
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