namespace CxDataRepository
{
/// <summary>
    /// There are no comments for QueryGroupType in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::Microsoft.OData.Client.Key("Id")]
    public partial class QueryGroupType : global::Microsoft.OData.Client.BaseEntityType
    {
        /// <summary>
        /// Create a new QueryGroupType object.
        /// </summary>
        /// <param name="ID">Initial value of Id.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public static QueryGroupType CreateQueryGroupType(int ID)
        {
            QueryGroupType queryGroupType = new QueryGroupType();
            queryGroupType.Id = ID;
            return queryGroupType;
        }
        /// <summary>
        /// There are no comments for Property Id in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual int Id
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
        private int _Id;
        partial void OnIdChanging(int value);
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
        /// There are no comments for Property QueryGroups in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Collections.ObjectModel.Collection<global::CxDataRepository.QueryGroup> QueryGroups
        {
            get
            {
                return this._QueryGroups;
            }
            set
            {
                this.OnQueryGroupsChanging(value);
                this._QueryGroups = value;
                this.OnQueryGroupsChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Collections.ObjectModel.Collection<global::CxDataRepository.QueryGroup> _QueryGroups = new global::System.Collections.ObjectModel.Collection<global::CxDataRepository.QueryGroup>();
        partial void OnQueryGroupsChanging(global::System.Collections.ObjectModel.Collection<global::CxDataRepository.QueryGroup> value);
        partial void OnQueryGroupsChanged();
    }
}