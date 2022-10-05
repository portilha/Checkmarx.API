namespace CxDataRepository
{
/// <summary>
    /// There are no comments for Scan in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::Microsoft.OData.Client.Key("Id")]
    public partial class Scan : global::Microsoft.OData.Client.BaseEntityType
    {
        /// <summary>
        /// Create a new Scan object.
        /// </summary>
        /// <param name="ID">Initial value of Id.</param>
        /// <param name="scanType">Initial value of ScanType.</param>
        /// <param name="engineStartedOn">Initial value of EngineStartedOn.</param>
        /// <param name="scanCompletedOn">Initial value of ScanCompletedOn.</param>
        /// <param name="high">Initial value of High.</param>
        /// <param name="medium">Initial value of Medium.</param>
        /// <param name="low">Initial value of Low.</param>
        /// <param name="info">Initial value of Info.</param>
        /// <param name="riskScore">Initial value of RiskScore.</param>
        /// <param name="quantityLevel">Initial value of QuantityLevel.</param>
        /// <param name="statisticsUpToDate">Initial value of StatisticsUpToDate.</param>
        /// <param name="isPublic">Initial value of IsPublic.</param>
        /// <param name="isLocked">Initial value of IsLocked.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public static Scan CreateScan(long ID, 
                    int scanType, 
                    global::System.DateTimeOffset engineStartedOn, 
                    global::System.DateTimeOffset scanCompletedOn, 
                    int high, 
                    int medium, 
                    int low, 
                    int info, 
                    int riskScore, 
                    int quantityLevel, 
                    int statisticsUpToDate, 
                    bool isPublic, 
                    bool isLocked)
        {
            Scan scan = new Scan();
            scan.Id = ID;
            scan.ScanType = scanType;
            scan.EngineStartedOn = engineStartedOn;
            scan.ScanCompletedOn = scanCompletedOn;
            scan.High = high;
            scan.Medium = medium;
            scan.Low = low;
            scan.Info = info;
            scan.RiskScore = riskScore;
            scan.QuantityLevel = quantityLevel;
            scan.StatisticsUpToDate = statisticsUpToDate;
            scan.IsPublic = isPublic;
            scan.IsLocked = isLocked;
            return scan;
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
        /// There are no comments for Property SourceId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string SourceId
        {
            get
            {
                return this._SourceId;
            }
            set
            {
                this.OnSourceIdChanging(value);
                this._SourceId = value;
                this.OnSourceIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _SourceId;
        partial void OnSourceIdChanging(string value);
        partial void OnSourceIdChanged();
        /// <summary>
        /// There are no comments for Property Comment in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string Comment
        {
            get
            {
                return this._Comment;
            }
            set
            {
                this.OnCommentChanging(value);
                this._Comment = value;
                this.OnCommentChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _Comment;
        partial void OnCommentChanging(string value);
        partial void OnCommentChanged();
        /// <summary>
        /// There are no comments for Property IsIncremental in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<bool> IsIncremental
        {
            get
            {
                return this._IsIncremental;
            }
            set
            {
                this.OnIsIncrementalChanging(value);
                this._IsIncremental = value;
                this.OnIsIncrementalChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<bool> _IsIncremental;
        partial void OnIsIncrementalChanging(global::System.Nullable<bool> value);
        partial void OnIsIncrementalChanged();
        /// <summary>
        /// There are no comments for Property ScanType in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual int ScanType
        {
            get
            {
                return this._ScanType;
            }
            set
            {
                this.OnScanTypeChanging(value);
                this._ScanType = value;
                this.OnScanTypeChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private int _ScanType;
        partial void OnScanTypeChanging(int value);
        partial void OnScanTypeChanged();
        /// <summary>
        /// There are no comments for Property Origin in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string Origin
        {
            get
            {
                return this._Origin;
            }
            set
            {
                this.OnOriginChanging(value);
                this._Origin = value;
                this.OnOriginChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _Origin;
        partial void OnOriginChanging(string value);
        partial void OnOriginChanged();
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
        /// There are no comments for Property InitiatorName in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string InitiatorName
        {
            get
            {
                return this._InitiatorName;
            }
            set
            {
                this.OnInitiatorNameChanging(value);
                this._InitiatorName = value;
                this.OnInitiatorNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _InitiatorName;
        partial void OnInitiatorNameChanging(string value);
        partial void OnInitiatorNameChanged();
        /// <summary>
        /// There are no comments for Property ProjectName in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string ProjectName
        {
            get
            {
                return this._ProjectName;
            }
            set
            {
                this.OnProjectNameChanging(value);
                this._ProjectName = value;
                this.OnProjectNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _ProjectName;
        partial void OnProjectNameChanging(string value);
        partial void OnProjectNameChanged();
        /// <summary>
        /// There are no comments for Property PresetName in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string PresetName
        {
            get
            {
                return this._PresetName;
            }
            set
            {
                this.OnPresetNameChanging(value);
                this._PresetName = value;
                this.OnPresetNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _PresetName;
        partial void OnPresetNameChanging(string value);
        partial void OnPresetNameChanged();
        /// <summary>
        /// There are no comments for Property TeamName in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string TeamName
        {
            get
            {
                return this._TeamName;
            }
            set
            {
                this.OnTeamNameChanging(value);
                this._TeamName = value;
                this.OnTeamNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _TeamName;
        partial void OnTeamNameChanging(string value);
        partial void OnTeamNameChanged();
        /// <summary>
        /// There are no comments for Property Path in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string Path
        {
            get
            {
                return this._Path;
            }
            set
            {
                this.OnPathChanging(value);
                this._Path = value;
                this.OnPathChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _Path;
        partial void OnPathChanging(string value);
        partial void OnPathChanged();
        /// <summary>
        /// There are no comments for Property FileCount in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<long> FileCount
        {
            get
            {
                return this._FileCount;
            }
            set
            {
                this.OnFileCountChanging(value);
                this._FileCount = value;
                this.OnFileCountChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<long> _FileCount;
        partial void OnFileCountChanging(global::System.Nullable<long> value);
        partial void OnFileCountChanged();
        /// <summary>
        /// There are no comments for Property LOC in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<long> LOC
        {
            get
            {
                return this._LOC;
            }
            set
            {
                this.OnLOCChanging(value);
                this._LOC = value;
                this.OnLOCChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<long> _LOC;
        partial void OnLOCChanging(global::System.Nullable<long> value);
        partial void OnLOCChanged();
        /// <summary>
        /// There are no comments for Property FailedLOC in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<long> FailedLOC
        {
            get
            {
                return this._FailedLOC;
            }
            set
            {
                this.OnFailedLOCChanging(value);
                this._FailedLOC = value;
                this.OnFailedLOCChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<long> _FailedLOC;
        partial void OnFailedLOCChanging(global::System.Nullable<long> value);
        partial void OnFailedLOCChanged();
        /// <summary>
        /// There are no comments for Property ProductVersion in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string ProductVersion
        {
            get
            {
                return this._ProductVersion;
            }
            set
            {
                this.OnProductVersionChanging(value);
                this._ProductVersion = value;
                this.OnProductVersionChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _ProductVersion;
        partial void OnProductVersionChanging(string value);
        partial void OnProductVersionChanged();
        /// <summary>
        /// There are no comments for Property IsForcedScan in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<bool> IsForcedScan
        {
            get
            {
                return this._IsForcedScan;
            }
            set
            {
                this.OnIsForcedScanChanging(value);
                this._IsForcedScan = value;
                this.OnIsForcedScanChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<bool> _IsForcedScan;
        partial void OnIsForcedScanChanging(global::System.Nullable<bool> value);
        partial void OnIsForcedScanChanged();
        /// <summary>
        /// There are no comments for Property ScanRequestedOn in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<global::System.DateTimeOffset> ScanRequestedOn
        {
            get
            {
                return this._ScanRequestedOn;
            }
            set
            {
                this.OnScanRequestedOnChanging(value);
                this._ScanRequestedOn = value;
                this.OnScanRequestedOnChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<global::System.DateTimeOffset> _ScanRequestedOn;
        partial void OnScanRequestedOnChanging(global::System.Nullable<global::System.DateTimeOffset> value);
        partial void OnScanRequestedOnChanged();
        /// <summary>
        /// There are no comments for Property QueuedOn in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<global::System.DateTimeOffset> QueuedOn
        {
            get
            {
                return this._QueuedOn;
            }
            set
            {
                this.OnQueuedOnChanging(value);
                this._QueuedOn = value;
                this.OnQueuedOnChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<global::System.DateTimeOffset> _QueuedOn;
        partial void OnQueuedOnChanging(global::System.Nullable<global::System.DateTimeOffset> value);
        partial void OnQueuedOnChanged();
        /// <summary>
        /// There are no comments for Property EngineStartedOn in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.DateTimeOffset EngineStartedOn
        {
            get
            {
                return this._EngineStartedOn;
            }
            set
            {
                this.OnEngineStartedOnChanging(value);
                this._EngineStartedOn = value;
                this.OnEngineStartedOnChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.DateTimeOffset _EngineStartedOn;
        partial void OnEngineStartedOnChanging(global::System.DateTimeOffset value);
        partial void OnEngineStartedOnChanged();
        /// <summary>
        /// There are no comments for Property EngineFinishedOn in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<global::System.DateTimeOffset> EngineFinishedOn
        {
            get
            {
                return this._EngineFinishedOn;
            }
            set
            {
                this.OnEngineFinishedOnChanging(value);
                this._EngineFinishedOn = value;
                this.OnEngineFinishedOnChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<global::System.DateTimeOffset> _EngineFinishedOn;
        partial void OnEngineFinishedOnChanging(global::System.Nullable<global::System.DateTimeOffset> value);
        partial void OnEngineFinishedOnChanged();
        /// <summary>
        /// There are no comments for Property ScanCompletedOn in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.DateTimeOffset ScanCompletedOn
        {
            get
            {
                return this._ScanCompletedOn;
            }
            set
            {
                this.OnScanCompletedOnChanging(value);
                this._ScanCompletedOn = value;
                this.OnScanCompletedOnChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.DateTimeOffset _ScanCompletedOn;
        partial void OnScanCompletedOnChanging(global::System.DateTimeOffset value);
        partial void OnScanCompletedOnChanged();
        /// <summary>
        /// There are no comments for Property ScanDuration in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<global::System.DateTimeOffset> ScanDuration
        {
            get
            {
                return this._ScanDuration;
            }
            set
            {
                this.OnScanDurationChanging(value);
                this._ScanDuration = value;
                this.OnScanDurationChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<global::System.DateTimeOffset> _ScanDuration;
        partial void OnScanDurationChanging(global::System.Nullable<global::System.DateTimeOffset> value);
        partial void OnScanDurationChanged();
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
        /// There are no comments for Property EngineServerId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<long> EngineServerId
        {
            get
            {
                return this._EngineServerId;
            }
            set
            {
                this.OnEngineServerIdChanging(value);
                this._EngineServerId = value;
                this.OnEngineServerIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<long> _EngineServerId;
        partial void OnEngineServerIdChanging(global::System.Nullable<long> value);
        partial void OnEngineServerIdChanged();
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
        /// There are no comments for Property QueryLanguageVersionId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<int> QueryLanguageVersionId
        {
            get
            {
                return this._QueryLanguageVersionId;
            }
            set
            {
                this.OnQueryLanguageVersionIdChanging(value);
                this._QueryLanguageVersionId = value;
                this.OnQueryLanguageVersionIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<int> _QueryLanguageVersionId;
        partial void OnQueryLanguageVersionIdChanging(global::System.Nullable<int> value);
        partial void OnQueryLanguageVersionIdChanged();
        /// <summary>
        /// There are no comments for Property ScannedLanguageIds in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<int> ScannedLanguageIds
        {
            get
            {
                return this._ScannedLanguageIds;
            }
            set
            {
                this.OnScannedLanguageIdsChanging(value);
                this._ScannedLanguageIds = value;
                this.OnScannedLanguageIdsChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<int> _ScannedLanguageIds;
        partial void OnScannedLanguageIdsChanging(global::System.Nullable<int> value);
        partial void OnScannedLanguageIdsChanged();
        /// <summary>
        /// There are no comments for Property TotalVulnerabilities in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<int> TotalVulnerabilities
        {
            get
            {
                return this._TotalVulnerabilities;
            }
            set
            {
                this.OnTotalVulnerabilitiesChanging(value);
                this._TotalVulnerabilities = value;
                this.OnTotalVulnerabilitiesChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<int> _TotalVulnerabilities;
        partial void OnTotalVulnerabilitiesChanging(global::System.Nullable<int> value);
        partial void OnTotalVulnerabilitiesChanged();
        /// <summary>
        /// There are no comments for Property High in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual int High
        {
            get
            {
                return this._High;
            }
            set
            {
                this.OnHighChanging(value);
                this._High = value;
                this.OnHighChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private int _High;
        partial void OnHighChanging(int value);
        partial void OnHighChanged();
        /// <summary>
        /// There are no comments for Property Medium in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual int Medium
        {
            get
            {
                return this._Medium;
            }
            set
            {
                this.OnMediumChanging(value);
                this._Medium = value;
                this.OnMediumChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private int _Medium;
        partial void OnMediumChanging(int value);
        partial void OnMediumChanged();
        /// <summary>
        /// There are no comments for Property Low in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual int Low
        {
            get
            {
                return this._Low;
            }
            set
            {
                this.OnLowChanging(value);
                this._Low = value;
                this.OnLowChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private int _Low;
        partial void OnLowChanging(int value);
        partial void OnLowChanged();
        /// <summary>
        /// There are no comments for Property Info in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual int Info
        {
            get
            {
                return this._Info;
            }
            set
            {
                this.OnInfoChanging(value);
                this._Info = value;
                this.OnInfoChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private int _Info;
        partial void OnInfoChanging(int value);
        partial void OnInfoChanged();
        /// <summary>
        /// There are no comments for Property RiskScore in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual int RiskScore
        {
            get
            {
                return this._RiskScore;
            }
            set
            {
                this.OnRiskScoreChanging(value);
                this._RiskScore = value;
                this.OnRiskScoreChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private int _RiskScore;
        partial void OnRiskScoreChanging(int value);
        partial void OnRiskScoreChanged();
        /// <summary>
        /// There are no comments for Property QuantityLevel in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual int QuantityLevel
        {
            get
            {
                return this._QuantityLevel;
            }
            set
            {
                this.OnQuantityLevelChanging(value);
                this._QuantityLevel = value;
                this.OnQuantityLevelChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private int _QuantityLevel;
        partial void OnQuantityLevelChanging(int value);
        partial void OnQuantityLevelChanged();
        /// <summary>
        /// There are no comments for Property StatisticsUpdateDate in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<global::System.DateTimeOffset> StatisticsUpdateDate
        {
            get
            {
                return this._StatisticsUpdateDate;
            }
            set
            {
                this.OnStatisticsUpdateDateChanging(value);
                this._StatisticsUpdateDate = value;
                this.OnStatisticsUpdateDateChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<global::System.DateTimeOffset> _StatisticsUpdateDate;
        partial void OnStatisticsUpdateDateChanging(global::System.Nullable<global::System.DateTimeOffset> value);
        partial void OnStatisticsUpdateDateChanged();
        /// <summary>
        /// There are no comments for Property StatisticsUpToDate in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual int StatisticsUpToDate
        {
            get
            {
                return this._StatisticsUpToDate;
            }
            set
            {
                this.OnStatisticsUpToDateChanging(value);
                this._StatisticsUpToDate = value;
                this.OnStatisticsUpToDateChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private int _StatisticsUpToDate;
        partial void OnStatisticsUpToDateChanging(int value);
        partial void OnStatisticsUpToDateChanged();
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
        /// There are no comments for Property IsLocked in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual bool IsLocked
        {
            get
            {
                return this._IsLocked;
            }
            set
            {
                this.OnIsLockedChanging(value);
                this._IsLocked = value;
                this.OnIsLockedChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private bool _IsLocked;
        partial void OnIsLockedChanging(bool value);
        partial void OnIsLockedChanged();
        /// <summary>
        /// There are no comments for Property Results in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Collections.ObjectModel.Collection<global::CxDataRepository.Result> Results
        {
            get
            {
                return this._Results;
            }
            set
            {
                this.OnResultsChanging(value);
                this._Results = value;
                this.OnResultsChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Collections.ObjectModel.Collection<global::CxDataRepository.Result> _Results = new global::System.Collections.ObjectModel.Collection<global::CxDataRepository.Result>();
        partial void OnResultsChanging(global::System.Collections.ObjectModel.Collection<global::CxDataRepository.Result> value);
        partial void OnResultsChanged();
        /// <summary>
        /// There are no comments for Property TopScanVulnerabilities in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Collections.ObjectModel.Collection<global::CxDataRepository.TopScanVulnerability> TopScanVulnerabilities
        {
            get
            {
                return this._TopScanVulnerabilities;
            }
            set
            {
                this.OnTopScanVulnerabilitiesChanging(value);
                this._TopScanVulnerabilities = value;
                this.OnTopScanVulnerabilitiesChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Collections.ObjectModel.Collection<global::CxDataRepository.TopScanVulnerability> _TopScanVulnerabilities = new global::System.Collections.ObjectModel.Collection<global::CxDataRepository.TopScanVulnerability>();
        partial void OnTopScanVulnerabilitiesChanging(global::System.Collections.ObjectModel.Collection<global::CxDataRepository.TopScanVulnerability> value);
        partial void OnTopScanVulnerabilitiesChanged();
        /// <summary>
        /// There are no comments for Property ScannedLanguages in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Collections.ObjectModel.Collection<global::CxDataRepository.QueryLanguage> ScannedLanguages
        {
            get
            {
                return this._ScannedLanguages;
            }
            set
            {
                this.OnScannedLanguagesChanging(value);
                this._ScannedLanguages = value;
                this.OnScannedLanguagesChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Collections.ObjectModel.Collection<global::CxDataRepository.QueryLanguage> _ScannedLanguages = new global::System.Collections.ObjectModel.Collection<global::CxDataRepository.QueryLanguage>();
        partial void OnScannedLanguagesChanging(global::System.Collections.ObjectModel.Collection<global::CxDataRepository.QueryLanguage> value);
        partial void OnScannedLanguagesChanged();
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
        /// There are no comments for Property EngineServer in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.EngineServer EngineServer
        {
            get
            {
                return this._EngineServer;
            }
            set
            {
                this.OnEngineServerChanging(value);
                this._EngineServer = value;
                this.OnEngineServerChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.EngineServer _EngineServer;
        partial void OnEngineServerChanging(global::CxDataRepository.EngineServer value);
        partial void OnEngineServerChanged();
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
        /// There are no comments for Property ResultSummary in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.ScanResultSummaryData ResultSummary
        {
            get
            {
                return this._ResultSummary;
            }
            set
            {
                this.OnResultSummaryChanging(value);
                this._ResultSummary = value;
                this.OnResultSummaryChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.ScanResultSummaryData _ResultSummary;
        partial void OnResultSummaryChanging(global::CxDataRepository.ScanResultSummaryData value);
        partial void OnResultSummaryChanged();
    }
}