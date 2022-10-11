namespace CxDataRepository
{
/// <summary>
    /// There are no comments for ProjectSingle in the schema.
    /// </summary>
    public partial class ProjectSingle : global::Microsoft.OData.Client.DataServiceQuerySingle<Project>
    {
        /// <summary>
        /// Initialize a new ProjectSingle object.
        /// </summary>
        public ProjectSingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new ProjectSingle object.
        /// </summary>
        public ProjectSingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new ProjectSingle object.
        /// </summary>
        public ProjectSingle(global::Microsoft.OData.Client.DataServiceQuerySingle<Project> query)
            : base(query) {}

        /// <summary>
        /// There are no comments for Scans in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.Scan> Scans
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._Scans == null))
                {
                    this._Scans = Context.CreateQuery<global::CxDataRepository.Scan>(GetPath("Scans"));
                }
                return this._Scans;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.Scan> _Scans;
        /// <summary>
        /// There are no comments for CustomFields in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.ProjectCustomField> CustomFields
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._CustomFields == null))
                {
                    this._CustomFields = Context.CreateQuery<global::CxDataRepository.ProjectCustomField>(GetPath("CustomFields"));
                }
                return this._CustomFields;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.ProjectCustomField> _CustomFields;
        /// <summary>
        /// There are no comments for EngineConfiguration in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.EngineConfigurationSingle EngineConfiguration
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._EngineConfiguration == null))
                {
                    this._EngineConfiguration = new global::CxDataRepository.EngineConfigurationSingle(this.Context, GetPath("EngineConfiguration"));
                }
                return this._EngineConfiguration;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.EngineConfigurationSingle _EngineConfiguration;
        /// <summary>
        /// There are no comments for ClientType in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.ClientTypeSingle ClientType
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._ClientType == null))
                {
                    this._ClientType = new global::CxDataRepository.ClientTypeSingle(this.Context, GetPath("ClientType"));
                }
                return this._ClientType;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.ClientTypeSingle _ClientType;
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
        /// There are no comments for LastScan in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.ScanSingle LastScan
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._LastScan == null))
                {
                    this._LastScan = new global::CxDataRepository.ScanSingle(this.Context, GetPath("LastScan"));
                }
                return this._LastScan;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.ScanSingle _LastScan;
    }
}