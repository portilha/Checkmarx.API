namespace CxDataRepository
{
/// <summary>
    /// There are no comments for QueryGroupSingle in the schema.
    /// </summary>
    public partial class QueryGroupSingle : global::Microsoft.OData.Client.DataServiceQuerySingle<QueryGroup>
    {
        /// <summary>
        /// Initialize a new QueryGroupSingle object.
        /// </summary>
        public QueryGroupSingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new QueryGroupSingle object.
        /// </summary>
        public QueryGroupSingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new QueryGroupSingle object.
        /// </summary>
        public QueryGroupSingle(global::Microsoft.OData.Client.DataServiceQuerySingle<QueryGroup> query)
            : base(query) {}

        /// <summary>
        /// There are no comments for Project in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.ProjectSingle Project
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._Project == null))
                {
                    this._Project = new global::CxDataRepository.ProjectSingle(this.Context, GetPath("Project"));
                }
                return this._Project;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.ProjectSingle _Project;
        /// <summary>
        /// There are no comments for Team in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.TeamSingle Team
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._Team == null))
                {
                    this._Team = new global::CxDataRepository.TeamSingle(this.Context, GetPath("Team"));
                }
                return this._Team;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.TeamSingle _Team;
        /// <summary>
        /// There are no comments for QueryGroupType in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.QueryGroupTypeSingle QueryGroupType
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._QueryGroupType == null))
                {
                    this._QueryGroupType = new global::CxDataRepository.QueryGroupTypeSingle(this.Context, GetPath("QueryGroupType"));
                }
                return this._QueryGroupType;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.QueryGroupTypeSingle _QueryGroupType;
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