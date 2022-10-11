namespace CxDataRepository
{
/// <summary>
    /// There are no comments for Project in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::Microsoft.OData.Client.Key("Id")]
    public partial class Project : global::Microsoft.OData.Client.BaseEntityType
    {
        /// <summary>
        /// Create a new Project object.
        /// </summary>
        /// <param name="ID">Initial value of Id.</param>
        /// <param name="isPublic">Initial value of IsPublic.</param>
        /// <param name="createdDate">Initial value of CreatedDate.</param>
        /// <param name="originClientTypeId">Initial value of OriginClientTypeId.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public static Project CreateProject(long ID, bool isPublic, global::System.DateTimeOffset createdDate, int originClientTypeId)
        {
            Project project = new Project();
            project.Id = ID;
            project.IsPublic = isPublic;
            project.CreatedDate = createdDate;
            project.OriginClientTypeId = originClientTypeId;
            return project;
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
        /// There are no comments for Property IsPublic in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual bool IsPublic
        {
            get
            {
                return this._IsPublic;
            }
            set
            {
                this.OnIsPublicChanging(value);
                this._IsPublic = value;
                this.OnIsPublicChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private bool _IsPublic;
        partial void OnIsPublicChanging(bool value);
        partial void OnIsPublicChanged();
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
        /// There are no comments for Property CreatedDate in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.DateTimeOffset CreatedDate
        {
            get
            {
                return this._CreatedDate;
            }
            set
            {
                this.OnCreatedDateChanging(value);
                this._CreatedDate = value;
                this.OnCreatedDateChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.DateTimeOffset _CreatedDate;
        partial void OnCreatedDateChanging(global::System.DateTimeOffset value);
        partial void OnCreatedDateChanged();
        /// <summary>
        /// There are no comments for Property OwnerId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<int> OwnerId
        {
            get
            {
                return this._OwnerId;
            }
            set
            {
                this.OnOwnerIdChanging(value);
                this._OwnerId = value;
                this.OnOwnerIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<int> _OwnerId;
        partial void OnOwnerIdChanging(global::System.Nullable<int> value);
        partial void OnOwnerIdChanged();
        /// <summary>
        /// There are no comments for Property OwningTeamId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string OwningTeamId
        {
            get
            {
                return this._OwningTeamId;
            }
            set
            {
                this.OnOwningTeamIdChanging(value);
                this._OwningTeamId = value;
                this.OnOwningTeamIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _OwningTeamId;
        partial void OnOwningTeamIdChanging(string value);
        partial void OnOwningTeamIdChanged();
        /// <summary>
        /// There are no comments for Property EngineConfigurationId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<long> EngineConfigurationId
        {
            get
            {
                return this._EngineConfigurationId;
            }
            set
            {
                this.OnEngineConfigurationIdChanging(value);
                this._EngineConfigurationId = value;
                this.OnEngineConfigurationIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<long> _EngineConfigurationId;
        partial void OnEngineConfigurationIdChanging(global::System.Nullable<long> value);
        partial void OnEngineConfigurationIdChanged();
        /// <summary>
        /// There are no comments for Property IssueTrackingSettings in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string IssueTrackingSettings
        {
            get
            {
                return this._IssueTrackingSettings;
            }
            set
            {
                this.OnIssueTrackingSettingsChanging(value);
                this._IssueTrackingSettings = value;
                this.OnIssueTrackingSettingsChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _IssueTrackingSettings;
        partial void OnIssueTrackingSettingsChanging(string value);
        partial void OnIssueTrackingSettingsChanged();
        /// <summary>
        /// There are no comments for Property SourcePath in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string SourcePath
        {
            get
            {
                return this._SourcePath;
            }
            set
            {
                this.OnSourcePathChanging(value);
                this._SourcePath = value;
                this.OnSourcePathChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _SourcePath;
        partial void OnSourcePathChanging(string value);
        partial void OnSourcePathChanged();
        /// <summary>
        /// There are no comments for Property SourceProviderCredentials in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string SourceProviderCredentials
        {
            get
            {
                return this._SourceProviderCredentials;
            }
            set
            {
                this.OnSourceProviderCredentialsChanging(value);
                this._SourceProviderCredentials = value;
                this.OnSourceProviderCredentialsChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _SourceProviderCredentials;
        partial void OnSourceProviderCredentialsChanging(string value);
        partial void OnSourceProviderCredentialsChanged();
        /// <summary>
        /// There are no comments for Property ExcludedFiles in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string ExcludedFiles
        {
            get
            {
                return this._ExcludedFiles;
            }
            set
            {
                this.OnExcludedFilesChanging(value);
                this._ExcludedFiles = value;
                this.OnExcludedFilesChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _ExcludedFiles;
        partial void OnExcludedFilesChanging(string value);
        partial void OnExcludedFilesChanged();
        /// <summary>
        /// There are no comments for Property ExcludedFolders in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string ExcludedFolders
        {
            get
            {
                return this._ExcludedFolders;
            }
            set
            {
                this.OnExcludedFoldersChanging(value);
                this._ExcludedFolders = value;
                this.OnExcludedFoldersChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _ExcludedFolders;
        partial void OnExcludedFoldersChanging(string value);
        partial void OnExcludedFoldersChanged();
        /// <summary>
        /// There are no comments for Property OriginClientTypeId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual int OriginClientTypeId
        {
            get
            {
                return this._OriginClientTypeId;
            }
            set
            {
                this.OnOriginClientTypeIdChanging(value);
                this._OriginClientTypeId = value;
                this.OnOriginClientTypeIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private int _OriginClientTypeId;
        partial void OnOriginClientTypeIdChanging(int value);
        partial void OnOriginClientTypeIdChanged();
        /// <summary>
        /// There are no comments for Property PresetId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<long> PresetId
        {
            get
            {
                return this._PresetId;
            }
            set
            {
                this.OnPresetIdChanging(value);
                this._PresetId = value;
                this.OnPresetIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<long> _PresetId;
        partial void OnPresetIdChanging(global::System.Nullable<long> value);
        partial void OnPresetIdChanged();
        /// <summary>
        /// There are no comments for Property LastScanId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<long> LastScanId
        {
            get
            {
                return this._LastScanId;
            }
            set
            {
                this.OnLastScanIdChanging(value);
                this._LastScanId = value;
                this.OnLastScanIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<long> _LastScanId;
        partial void OnLastScanIdChanging(global::System.Nullable<long> value);
        partial void OnLastScanIdChanged();
        /// <summary>
        /// There are no comments for Property TotalProjectScanCount in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<int> TotalProjectScanCount
        {
            get
            {
                return this._TotalProjectScanCount;
            }
            set
            {
                this.OnTotalProjectScanCountChanging(value);
                this._TotalProjectScanCount = value;
                this.OnTotalProjectScanCountChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<int> _TotalProjectScanCount;
        partial void OnTotalProjectScanCountChanging(global::System.Nullable<int> value);
        partial void OnTotalProjectScanCountChanged();
        /// <summary>
        /// There are no comments for Property SchedulingExpression in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string SchedulingExpression
        {
            get
            {
                return this._SchedulingExpression;
            }
            set
            {
                this.OnSchedulingExpressionChanging(value);
                this._SchedulingExpression = value;
                this.OnSchedulingExpressionChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _SchedulingExpression;
        partial void OnSchedulingExpressionChanging(string value);
        partial void OnSchedulingExpressionChanged();
        /// <summary>
        /// There are no comments for Property Scans in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Collections.ObjectModel.Collection<global::CxDataRepository.Scan> Scans
        {
            get
            {
                return this._Scans;
            }
            set
            {
                this.OnScansChanging(value);
                this._Scans = value;
                this.OnScansChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Collections.ObjectModel.Collection<global::CxDataRepository.Scan> _Scans = new global::System.Collections.ObjectModel.Collection<global::CxDataRepository.Scan>();
        partial void OnScansChanging(global::System.Collections.ObjectModel.Collection<global::CxDataRepository.Scan> value);
        partial void OnScansChanged();
        /// <summary>
        /// There are no comments for Property CustomFields in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Collections.ObjectModel.Collection<global::CxDataRepository.ProjectCustomField> CustomFields
        {
            get
            {
                return this._CustomFields;
            }
            set
            {
                this.OnCustomFieldsChanging(value);
                this._CustomFields = value;
                this.OnCustomFieldsChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Collections.ObjectModel.Collection<global::CxDataRepository.ProjectCustomField> _CustomFields = new global::System.Collections.ObjectModel.Collection<global::CxDataRepository.ProjectCustomField>();
        partial void OnCustomFieldsChanging(global::System.Collections.ObjectModel.Collection<global::CxDataRepository.ProjectCustomField> value);
        partial void OnCustomFieldsChanged();
        /// <summary>
        /// There are no comments for Property EngineConfiguration in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.EngineConfiguration EngineConfiguration
        {
            get
            {
                return this._EngineConfiguration;
            }
            set
            {
                this.OnEngineConfigurationChanging(value);
                this._EngineConfiguration = value;
                this.OnEngineConfigurationChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.EngineConfiguration _EngineConfiguration;
        partial void OnEngineConfigurationChanging(global::CxDataRepository.EngineConfiguration value);
        partial void OnEngineConfigurationChanged();
        /// <summary>
        /// There are no comments for Property ClientType in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.ClientType ClientType
        {
            get
            {
                return this._ClientType;
            }
            set
            {
                this.OnClientTypeChanging(value);
                this._ClientType = value;
                this.OnClientTypeChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.ClientType _ClientType;
        partial void OnClientTypeChanging(global::CxDataRepository.ClientType value);
        partial void OnClientTypeChanged();
        /// <summary>
        /// There are no comments for Property OwningUser in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.User OwningUser
        {
            get
            {
                return this._OwningUser;
            }
            set
            {
                this.OnOwningUserChanging(value);
                this._OwningUser = value;
                this.OnOwningUserChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.User _OwningUser;
        partial void OnOwningUserChanging(global::CxDataRepository.User value);
        partial void OnOwningUserChanged();
        /// <summary>
        /// There are no comments for Property OwningTeam in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.Team OwningTeam
        {
            get
            {
                return this._OwningTeam;
            }
            set
            {
                this.OnOwningTeamChanging(value);
                this._OwningTeam = value;
                this.OnOwningTeamChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.Team _OwningTeam;
        partial void OnOwningTeamChanging(global::CxDataRepository.Team value);
        partial void OnOwningTeamChanged();
        /// <summary>
        /// There are no comments for Property Preset in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.Preset Preset
        {
            get
            {
                return this._Preset;
            }
            set
            {
                this.OnPresetChanging(value);
                this._Preset = value;
                this.OnPresetChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.Preset _Preset;
        partial void OnPresetChanging(global::CxDataRepository.Preset value);
        partial void OnPresetChanged();
        /// <summary>
        /// There are no comments for Property LastScan in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.Scan LastScan
        {
            get
            {
                return this._LastScan;
            }
            set
            {
                this.OnLastScanChanging(value);
                this._LastScan = value;
                this.OnLastScanChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.Scan _LastScan;
        partial void OnLastScanChanging(global::CxDataRepository.Scan value);
        partial void OnLastScanChanged();
    }
}