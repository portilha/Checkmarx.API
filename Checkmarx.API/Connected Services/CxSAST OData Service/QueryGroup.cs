namespace CxDataRepository
{
/// <summary>
    /// There are no comments for QueryGroup in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::Microsoft.OData.Client.Key("Id")]
    public partial class QueryGroup : global::Microsoft.OData.Client.BaseEntityType
    {
        /// <summary>
        /// Create a new QueryGroup object.
        /// </summary>
        /// <param name="ID">Initial value of Id.</param>
        /// <param name="languageId">Initial value of LanguageId.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public static QueryGroup CreateQueryGroup(long ID, int languageId)
        {
            QueryGroup queryGroup = new QueryGroup();
            queryGroup.Id = ID;
            queryGroup.LanguageId = languageId;
            return queryGroup;
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
        /// There are no comments for Property Description in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                this.OnDescriptionChanging(value);
                this._Description = value;
                this.OnDescriptionChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _Description;
        partial void OnDescriptionChanging(string value);
        partial void OnDescriptionChanged();
        /// <summary>
        /// There are no comments for Property ProjectId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<long> ProjectId
        {
            get
            {
                return this._ProjectId;
            }
            set
            {
                this.OnProjectIdChanging(value);
                this._ProjectId = value;
                this.OnProjectIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<long> _ProjectId;
        partial void OnProjectIdChanging(global::System.Nullable<long> value);
        partial void OnProjectIdChanged();
        /// <summary>
        /// There are no comments for Property TeamId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string TeamId
        {
            get
            {
                return this._TeamId;
            }
            set
            {
                this.OnTeamIdChanging(value);
                this._TeamId = value;
                this.OnTeamIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _TeamId;
        partial void OnTeamIdChanging(string value);
        partial void OnTeamIdChanged();
        /// <summary>
        /// There are no comments for Property QueryGroupTypeId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<int> QueryGroupTypeId
        {
            get
            {
                return this._QueryGroupTypeId;
            }
            set
            {
                this.OnQueryGroupTypeIdChanging(value);
                this._QueryGroupTypeId = value;
                this.OnQueryGroupTypeIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<int> _QueryGroupTypeId;
        partial void OnQueryGroupTypeIdChanging(global::System.Nullable<int> value);
        partial void OnQueryGroupTypeIdChanged();
        /// <summary>
        /// There are no comments for Property LanguageId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual int LanguageId
        {
            get
            {
                return this._LanguageId;
            }
            set
            {
                this.OnLanguageIdChanging(value);
                this._LanguageId = value;
                this.OnLanguageIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private int _LanguageId;
        partial void OnLanguageIdChanging(int value);
        partial void OnLanguageIdChanged();
        /// <summary>
        /// There are no comments for Property LanguageName in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string LanguageName
        {
            get
            {
                return this._LanguageName;
            }
            set
            {
                this.OnLanguageNameChanging(value);
                this._LanguageName = value;
                this.OnLanguageNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _LanguageName;
        partial void OnLanguageNameChanging(string value);
        partial void OnLanguageNameChanged();
        /// <summary>
        /// There are no comments for Property Project in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.Project Project
        {
            get
            {
                return this._Project;
            }
            set
            {
                this.OnProjectChanging(value);
                this._Project = value;
                this.OnProjectChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.Project _Project;
        partial void OnProjectChanging(global::CxDataRepository.Project value);
        partial void OnProjectChanged();
        /// <summary>
        /// There are no comments for Property Team in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.Team Team
        {
            get
            {
                return this._Team;
            }
            set
            {
                this.OnTeamChanging(value);
                this._Team = value;
                this.OnTeamChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.Team _Team;
        partial void OnTeamChanging(global::CxDataRepository.Team value);
        partial void OnTeamChanged();
        /// <summary>
        /// There are no comments for Property QueryGroupType in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.QueryGroupType QueryGroupType
        {
            get
            {
                return this._QueryGroupType;
            }
            set
            {
                this.OnQueryGroupTypeChanging(value);
                this._QueryGroupType = value;
                this.OnQueryGroupTypeChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.QueryGroupType _QueryGroupType;
        partial void OnQueryGroupTypeChanging(global::CxDataRepository.QueryGroupType value);
        partial void OnQueryGroupTypeChanged();
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