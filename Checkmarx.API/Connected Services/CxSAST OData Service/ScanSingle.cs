namespace CxDataRepository
{
/// <summary>
    /// There are no comments for ScanSingle in the schema.
    /// </summary>
    public partial class ScanSingle : global::Microsoft.OData.Client.DataServiceQuerySingle<Scan>
    {
        /// <summary>
        /// Initialize a new ScanSingle object.
        /// </summary>
        public ScanSingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new ScanSingle object.
        /// </summary>
        public ScanSingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new ScanSingle object.
        /// </summary>
        public ScanSingle(global::Microsoft.OData.Client.DataServiceQuerySingle<Scan> query)
            : base(query) {}

        /// <summary>
        /// There are no comments for Results in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.Result> Results
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._Results == null))
                {
                    this._Results = Context.CreateQuery<global::CxDataRepository.Result>(GetPath("Results"));
                }
                return this._Results;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.Result> _Results;
        /// <summary>
        /// There are no comments for TopScanVulnerabilities in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.TopScanVulnerability> TopScanVulnerabilities
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._TopScanVulnerabilities == null))
                {
                    this._TopScanVulnerabilities = Context.CreateQuery<global::CxDataRepository.TopScanVulnerability>(GetPath("TopScanVulnerabilities"));
                }
                return this._TopScanVulnerabilities;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.TopScanVulnerability> _TopScanVulnerabilities;
        /// <summary>
        /// There are no comments for ScannedLanguages in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QueryLanguage> ScannedLanguages
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._ScannedLanguages == null))
                {
                    this._ScannedLanguages = Context.CreateQuery<global::CxDataRepository.QueryLanguage>(GetPath("ScannedLanguages"));
                }
                return this._ScannedLanguages;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QueryLanguage> _ScannedLanguages;
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
        /// There are no comments for EngineServer in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.EngineServerSingle EngineServer
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._EngineServer == null))
                {
                    this._EngineServer = new global::CxDataRepository.EngineServerSingle(this.Context, GetPath("EngineServer"));
                }
                return this._EngineServer;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.EngineServerSingle _EngineServer;
        /// <summary>
        /// There are no comments for Preset in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.PresetSingle Preset
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._Preset == null))
                {
                    this._Preset = new global::CxDataRepository.PresetSingle(this.Context, GetPath("Preset"));
                }
                return this._Preset;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.PresetSingle _Preset;
        /// <summary>
        /// There are no comments for OwningTeam in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.TeamSingle OwningTeam
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._OwningTeam == null))
                {
                    this._OwningTeam = new global::CxDataRepository.TeamSingle(this.Context, GetPath("OwningTeam"));
                }
                return this._OwningTeam;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.TeamSingle _OwningTeam;
        /// <summary>
        /// There are no comments for OwningUser in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.UserSingle OwningUser
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._OwningUser == null))
                {
                    this._OwningUser = new global::CxDataRepository.UserSingle(this.Context, GetPath("OwningUser"));
                }
                return this._OwningUser;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.UserSingle _OwningUser;
        /// <summary>
        /// There are no comments for ResultSummary in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.ScanResultSummaryDataSingle ResultSummary
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._ResultSummary == null))
                {
                    this._ResultSummary = new global::CxDataRepository.ScanResultSummaryDataSingle(this.Context, GetPath("ResultSummary"));
                }
                return this._ResultSummary;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.ScanResultSummaryDataSingle _ResultSummary;
    }
}