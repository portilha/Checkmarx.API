namespace CxDataRepository
{
/// <summary>
    /// There are no comments for Query in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::Microsoft.OData.Client.Key("Id")]
    public partial class Query : global::Microsoft.OData.Client.BaseEntityType
    {
        /// <summary>
        /// Create a new Query object.
        /// </summary>
        /// <param name="ID">Initial value of Id.</param>
        /// <param name="version">Initial value of Version.</param>
        /// <param name="severity">Initial value of Severity.</param>
        /// <param name="cweId">Initial value of CweId.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public static Query CreateQuery(long ID, long version, int severity, long cweId)
        {
            Query query = new Query();
            query.Id = ID;
            query.Version = version;
            query.Severity = severity;
            query.CweId = cweId;
            return query;
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
        /// There are no comments for Property Version in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual long Version
        {
            get
            {
                return this._Version;
            }
            set
            {
                this.OnVersionChanging(value);
                this._Version = value;
                this.OnVersionChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private long _Version;
        partial void OnVersionChanging(long value);
        partial void OnVersionChanged();
        /// <summary>
        /// There are no comments for Property Severity in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual int Severity
        {
            get
            {
                return this._Severity;
            }
            set
            {
                this.OnSeverityChanging(value);
                this._Severity = value;
                this.OnSeverityChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private int _Severity;
        partial void OnSeverityChanging(int value);
        partial void OnSeverityChanged();
        /// <summary>
        /// There are no comments for Property Comments in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string Comments
        {
            get
            {
                return this._Comments;
            }
            set
            {
                this.OnCommentsChanging(value);
                this._Comments = value;
                this.OnCommentsChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _Comments;
        partial void OnCommentsChanging(string value);
        partial void OnCommentsChanged();
        /// <summary>
        /// There are no comments for Property CxDescriptionId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<int> CxDescriptionId
        {
            get
            {
                return this._CxDescriptionId;
            }
            set
            {
                this.OnCxDescriptionIdChanging(value);
                this._CxDescriptionId = value;
                this.OnCxDescriptionIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<int> _CxDescriptionId;
        partial void OnCxDescriptionIdChanging(global::System.Nullable<int> value);
        partial void OnCxDescriptionIdChanged();
        /// <summary>
        /// There are no comments for Property CweId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual long CweId
        {
            get
            {
                return this._CweId;
            }
            set
            {
                this.OnCweIdChanging(value);
                this._CweId = value;
                this.OnCweIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private long _CweId;
        partial void OnCweIdChanging(long value);
        partial void OnCweIdChanged();
        /// <summary>
        /// There are no comments for Property QuerySourceId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<long> QuerySourceId
        {
            get
            {
                return this._QuerySourceId;
            }
            set
            {
                this.OnQuerySourceIdChanging(value);
                this._QuerySourceId = value;
                this.OnQuerySourceIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<long> _QuerySourceId;
        partial void OnQuerySourceIdChanging(global::System.Nullable<long> value);
        partial void OnQuerySourceIdChanged();
        /// <summary>
        /// There are no comments for Property QueryGroupId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<long> QueryGroupId
        {
            get
            {
                return this._QueryGroupId;
            }
            set
            {
                this.OnQueryGroupIdChanging(value);
                this._QueryGroupId = value;
                this.OnQueryGroupIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<long> _QueryGroupId;
        partial void OnQueryGroupIdChanging(global::System.Nullable<long> value);
        partial void OnQueryGroupIdChanged();
        /// <summary>
        /// There are no comments for Property QueryCxDescription in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.QueryCxDescription QueryCxDescription
        {
            get
            {
                return this._QueryCxDescription;
            }
            set
            {
                this.OnQueryCxDescriptionChanging(value);
                this._QueryCxDescription = value;
                this.OnQueryCxDescriptionChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.QueryCxDescription _QueryCxDescription;
        partial void OnQueryCxDescriptionChanging(global::CxDataRepository.QueryCxDescription value);
        partial void OnQueryCxDescriptionChanged();
        /// <summary>
        /// There are no comments for Property QueryCweDescription in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.QueryCweDescription QueryCweDescription
        {
            get
            {
                return this._QueryCweDescription;
            }
            set
            {
                this.OnQueryCweDescriptionChanging(value);
                this._QueryCweDescription = value;
                this.OnQueryCweDescriptionChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.QueryCweDescription _QueryCweDescription;
        partial void OnQueryCweDescriptionChanging(global::CxDataRepository.QueryCweDescription value);
        partial void OnQueryCweDescriptionChanged();
        /// <summary>
        /// There are no comments for Property QuerySource in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.QuerySource QuerySource
        {
            get
            {
                return this._QuerySource;
            }
            set
            {
                this.OnQuerySourceChanging(value);
                this._QuerySource = value;
                this.OnQuerySourceChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.QuerySource _QuerySource;
        partial void OnQuerySourceChanging(global::CxDataRepository.QuerySource value);
        partial void OnQuerySourceChanged();
        /// <summary>
        /// There are no comments for Property QueryGroup in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.QueryGroup QueryGroup
        {
            get
            {
                return this._QueryGroup;
            }
            set
            {
                this.OnQueryGroupChanging(value);
                this._QueryGroup = value;
                this.OnQueryGroupChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.QueryGroup _QueryGroup;
        partial void OnQueryGroupChanging(global::CxDataRepository.QueryGroup value);
        partial void OnQueryGroupChanged();
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