namespace CxDataRepository
{
/// <summary>
    /// There are no comments for QueryGroupTypeSingle in the schema.
    /// </summary>
    public partial class QueryGroupTypeSingle : global::Microsoft.OData.Client.DataServiceQuerySingle<QueryGroupType>
    {
        /// <summary>
        /// Initialize a new QueryGroupTypeSingle object.
        /// </summary>
        public QueryGroupTypeSingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new QueryGroupTypeSingle object.
        /// </summary>
        public QueryGroupTypeSingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new QueryGroupTypeSingle object.
        /// </summary>
        public QueryGroupTypeSingle(global::Microsoft.OData.Client.DataServiceQuerySingle<QueryGroupType> query)
            : base(query) {}

        /// <summary>
        /// There are no comments for QueryGroups in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QueryGroup> QueryGroups
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._QueryGroups == null))
                {
                    this._QueryGroups = Context.CreateQuery<global::CxDataRepository.QueryGroup>(GetPath("QueryGroups"));
                }
                return this._QueryGroups;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QueryGroup> _QueryGroups;
    }
}