namespace CxDataRepository
{
/// <summary>
    /// There are no comments for QueryLanguage in the schema.
    /// </summary>
    /// <KeyProperties>
    /// VersionId
    /// LanguageId
    /// </KeyProperties>
    [global::Microsoft.OData.Client.Key("VersionId", "LanguageId")]
    public partial class QueryLanguage : global::Microsoft.OData.Client.BaseEntityType
    {
        /// <summary>
        /// Create a new QueryLanguage object.
        /// </summary>
        /// <param name="versionId">Initial value of VersionId.</param>
        /// <param name="languageId">Initial value of LanguageId.</param>
        /// <param name="versionDate">Initial value of VersionDate.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public static QueryLanguage CreateQueryLanguage(int versionId, int languageId, global::System.DateTimeOffset versionDate)
        {
            QueryLanguage queryLanguage = new QueryLanguage();
            queryLanguage.VersionId = versionId;
            queryLanguage.LanguageId = languageId;
            queryLanguage.VersionDate = versionDate;
            return queryLanguage;
        }
        /// <summary>
        /// There are no comments for Property VersionId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual int VersionId
        {
            get
            {
                return this._VersionId;
            }
            set
            {
                this.OnVersionIdChanging(value);
                this._VersionId = value;
                this.OnVersionIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private int _VersionId;
        partial void OnVersionIdChanging(int value);
        partial void OnVersionIdChanged();
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
        /// There are no comments for Property VersionHash in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string VersionHash
        {
            get
            {
                return this._VersionHash;
            }
            set
            {
                this.OnVersionHashChanging(value);
                this._VersionHash = value;
                this.OnVersionHashChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _VersionHash;
        partial void OnVersionHashChanging(string value);
        partial void OnVersionHashChanged();
        /// <summary>
        /// There are no comments for Property VersionDate in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.DateTimeOffset VersionDate
        {
            get
            {
                return this._VersionDate;
            }
            set
            {
                this.OnVersionDateChanging(value);
                this._VersionDate = value;
                this.OnVersionDateChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.DateTimeOffset _VersionDate;
        partial void OnVersionDateChanging(global::System.DateTimeOffset value);
        partial void OnVersionDateChanged();
    }
}