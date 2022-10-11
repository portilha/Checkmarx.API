namespace CxDataRepository
{
/// <summary>
    /// There are no comments for QueryCxDescription in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::Microsoft.OData.Client.Key("Id")]
    public partial class QueryCxDescription : global::Microsoft.OData.Client.BaseEntityType
    {
        /// <summary>
        /// Create a new QueryCxDescription object.
        /// </summary>
        /// <param name="ID">Initial value of Id.</param>
        /// <param name="lCID">Initial value of LCID.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public static QueryCxDescription CreateQueryCxDescription(int ID, int lCID)
        {
            QueryCxDescription queryCxDescription = new QueryCxDescription();
            queryCxDescription.Id = ID;
            queryCxDescription.LCID = lCID;
            return queryCxDescription;
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
        /// There are no comments for Property LCID in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual int LCID
        {
            get
            {
                return this._LCID;
            }
            set
            {
                this.OnLCIDChanging(value);
                this._LCID = value;
                this.OnLCIDChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private int _LCID;
        partial void OnLCIDChanging(int value);
        partial void OnLCIDChanged();
        /// <summary>
        /// There are no comments for Property ResultDescription in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string ResultDescription
        {
            get
            {
                return this._ResultDescription;
            }
            set
            {
                this.OnResultDescriptionChanging(value);
                this._ResultDescription = value;
                this.OnResultDescriptionChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _ResultDescription;
        partial void OnResultDescriptionChanging(string value);
        partial void OnResultDescriptionChanged();
        /// <summary>
        /// There are no comments for Property BestFixLocation in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string BestFixLocation
        {
            get
            {
                return this._BestFixLocation;
            }
            set
            {
                this.OnBestFixLocationChanging(value);
                this._BestFixLocation = value;
                this.OnBestFixLocationChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _BestFixLocation;
        partial void OnBestFixLocationChanging(string value);
        partial void OnBestFixLocationChanged();
        /// <summary>
        /// There are no comments for Property Risk in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string Risk
        {
            get
            {
                return this._Risk;
            }
            set
            {
                this.OnRiskChanging(value);
                this._Risk = value;
                this.OnRiskChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _Risk;
        partial void OnRiskChanging(string value);
        partial void OnRiskChanged();
        /// <summary>
        /// There are no comments for Property Cause in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string Cause
        {
            get
            {
                return this._Cause;
            }
            set
            {
                this.OnCauseChanging(value);
                this._Cause = value;
                this.OnCauseChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _Cause;
        partial void OnCauseChanging(string value);
        partial void OnCauseChanged();
        /// <summary>
        /// There are no comments for Property GeneralRecommendations in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string GeneralRecommendations
        {
            get
            {
                return this._GeneralRecommendations;
            }
            set
            {
                this.OnGeneralRecommendationsChanging(value);
                this._GeneralRecommendations = value;
                this.OnGeneralRecommendationsChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _GeneralRecommendations;
        partial void OnGeneralRecommendationsChanging(string value);
        partial void OnGeneralRecommendationsChanged();
    }
}