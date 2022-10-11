namespace CxDataRepository
{
/// <summary>
    /// There are no comments for QueryCategory in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::Microsoft.OData.Client.Key("Id")]
    public partial class QueryCategory : global::Microsoft.OData.Client.BaseEntityType
    {
        /// <summary>
        /// Create a new QueryCategory object.
        /// </summary>
        /// <param name="ID">Initial value of Id.</param>
        /// <param name="typeId">Initial value of TypeId.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public static QueryCategory CreateQueryCategory(long ID, long typeId)
        {
            QueryCategory queryCategory = new QueryCategory();
            queryCategory.Id = ID;
            queryCategory.TypeId = typeId;
            return queryCategory;
        }
        /// <summary>
        /// There are no comments for Property Id in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private long _Id;
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        /// <summary>
        /// There are no comments for Property Name in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this.OnNameChanging(value);
                this._Name = value;
                this.OnNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _Name;
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        /// <summary>
        /// There are no comments for Property TypeId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual long TypeId
        {
            get
            {
                return this._TypeId;
            }
            set
            {
                this.OnTypeIdChanging(value);
                this._TypeId = value;
                this.OnTypeIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private long _TypeId;
        partial void OnTypeIdChanging(long value);
        partial void OnTypeIdChanged();
        /// <summary>
        /// There are no comments for Property QueryCategoryType in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.QueryCategoryType QueryCategoryType
        {
            get
            {
                return this._QueryCategoryType;
            }
            set
            {
                this.OnQueryCategoryTypeChanging(value);
                this._QueryCategoryType = value;
                this.OnQueryCategoryTypeChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.QueryCategoryType _QueryCategoryType;
        partial void OnQueryCategoryTypeChanging(global::CxDataRepository.QueryCategoryType value);
        partial void OnQueryCategoryTypeChanged();
        /// <summary>
        /// There are no comments for Property Queries in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Collections.ObjectModel.Collection<global::CxDataRepository.Query> Queries
        {
            get
            {
                return this._Queries;
            }
            set
            {
                this.OnQueriesChanging(value);
                this._Queries = value;
                this.OnQueriesChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Collections.ObjectModel.Collection<global::CxDataRepository.Query> _Queries = new global::System.Collections.ObjectModel.Collection<global::CxDataRepository.Query>();
        partial void OnQueriesChanging(global::System.Collections.ObjectModel.Collection<global::CxDataRepository.Query> value);
        partial void OnQueriesChanged();
    }
}