namespace CxDataRepository
{
/// <summary>
    /// There are no comments for QueryCategoryType in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::Microsoft.OData.Client.Key("Id")]
    public partial class QueryCategoryType : global::Microsoft.OData.Client.BaseEntityType
    {
        /// <summary>
        /// Create a new QueryCategoryType object.
        /// </summary>
        /// <param name="ID">Initial value of Id.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public static QueryCategoryType CreateQueryCategoryType(long ID)
        {
            QueryCategoryType queryCategoryType = new QueryCategoryType();
            queryCategoryType.Id = ID;
            return queryCategoryType;
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
        /// There are no comments for Property QueryCategories in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Collections.ObjectModel.Collection<global::CxDataRepository.QueryCategory> QueryCategories
        {
            get
            {
                return this._QueryCategories;
            }
            set
            {
                this.OnQueryCategoriesChanging(value);
                this._QueryCategories = value;
                this.OnQueryCategoriesChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Collections.ObjectModel.Collection<global::CxDataRepository.QueryCategory> _QueryCategories = new global::System.Collections.ObjectModel.Collection<global::CxDataRepository.QueryCategory>();
        partial void OnQueryCategoriesChanging(global::System.Collections.ObjectModel.Collection<global::CxDataRepository.QueryCategory> value);
        partial void OnQueryCategoriesChanged();
    }
}