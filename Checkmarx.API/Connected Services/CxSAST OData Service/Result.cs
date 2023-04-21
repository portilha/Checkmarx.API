namespace CxDataRepository
{
    /// <summary>
    /// There are no comments for Result in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// ScanId
    /// </KeyProperties>
    [global::Microsoft.OData.Client.Key("Id", "ScanId")]
    public partial class Result : global::Microsoft.OData.Client.BaseEntityType
    {
        /// <summary>
        /// Create a new Result object.
        /// </summary>
        /// <param name="ID">Initial value of Id.</param>
        /// <param name="similarityId">Initial value of SimilarityId.</param>
        /// <param name="pathId">Initial value of PathId.</param>
        /// <param name="date">Initial value of Date.</param>
        /// <param name="severity">Initial value of Severity.</param>
        /// <param name="stateId">Initial value of StateId.</param>
        /// <param name="queryVersionId">Initial value of QueryVersionId.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public static Result CreateResult(int ID,
                    long similarityId,
                    long pathId,
                    global::System.DateTimeOffset date,
                    global::CxDataRepository.Severity severity,
                    int stateId,
                    long queryVersionId)
        {
            Result result = new Result();
            result.Id = ID;
            result.SimilarityId = similarityId;
            result.PathId = pathId;
            result.Date = date;
            result.Severity = severity;
            result.StateId = stateId;
            result.QueryVersionId = queryVersionId;
            return result;
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
        /// There are no comments for Property ResultId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string ResultId
        {
            get
            {
                return this._ResultId;
            }
            set
            {
                this.OnResultIdChanging(value);
                this._ResultId = value;
                this.OnResultIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _ResultId;
        partial void OnResultIdChanging(string value);
        partial void OnResultIdChanged();
        /// <summary>
        /// There are no comments for Property ScanId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<long> ScanId
        {
            get
            {
                return this._ScanId;
            }
            set
            {
                this.OnScanIdChanging(value);
                this._ScanId = value;
                this.OnScanIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<long> _ScanId;
        partial void OnScanIdChanging(global::System.Nullable<long> value);
        partial void OnScanIdChanged();
        /// <summary>
        /// There are no comments for Property SimilarityId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual long SimilarityId
        {
            get
            {
                return this._SimilarityId;
            }
            set
            {
                this.OnSimilarityIdChanging(value);
                this._SimilarityId = value;
                this.OnSimilarityIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private long _SimilarityId;
        partial void OnSimilarityIdChanging(long value);
        partial void OnSimilarityIdChanged();
        /// <summary>
        /// There are no comments for Property RawPriority in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<long> RawPriority
        {
            get
            {
                return this._RawPriority;
            }
            set
            {
                this.OnRawPriorityChanging(value);
                this._RawPriority = value;
                this.OnRawPriorityChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<long> _RawPriority;
        partial void OnRawPriorityChanging(global::System.Nullable<long> value);
        partial void OnRawPriorityChanged();
        /// <summary>
        /// There are no comments for Property PathId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual long PathId
        {
            get
            {
                return this._PathId;
            }
            set
            {
                this.OnPathIdChanging(value);
                this._PathId = value;
                this.OnPathIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private long _PathId;
        partial void OnPathIdChanging(long value);
        partial void OnPathIdChanged();
        /// <summary>
        /// There are no comments for Property ConfidenceLevel in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<int> ConfidenceLevel
        {
            get
            {
                return this._ConfidenceLevel;
            }
            set
            {
                this.OnConfidenceLevelChanging(value);
                this._ConfidenceLevel = value;
                this.OnConfidenceLevelChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<int> _ConfidenceLevel;
        partial void OnConfidenceLevelChanging(global::System.Nullable<int> value);
        partial void OnConfidenceLevelChanged();
        /// <summary>
        /// There are no comments for Property Date in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.DateTimeOffset Date
        {
            get
            {
                return this._Date;
            }
            set
            {
                this.OnDateChanging(value);
                this._Date = value;
                this.OnDateChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.DateTimeOffset _Date;
        partial void OnDateChanging(global::System.DateTimeOffset value);
        partial void OnDateChanged();
        /// <summary>
        /// There are no comments for Property Severity in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.Severity Severity
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
        private global::CxDataRepository.Severity _Severity;
        partial void OnSeverityChanging(global::CxDataRepository.Severity value);
        partial void OnSeverityChanged();
        /// <summary>
        /// There are no comments for Property StateId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual int StateId
        {
            get
            {
                return this._StateId;
            }
            set
            {
                this.OnStateIdChanging(value);
                this._StateId = value;
                this.OnStateIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private int _StateId;
        partial void OnStateIdChanging(int value);
        partial void OnStateIdChanged();
        /// <summary>
        /// There are no comments for Property AssignedToUserId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<int> AssignedToUserId
        {
            get
            {
                return this._AssignedToUserId;
            }
            set
            {
                this.OnAssignedToUserIdChanging(value);
                this._AssignedToUserId = value;
                this.OnAssignedToUserIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<int> _AssignedToUserId;
        partial void OnAssignedToUserIdChanging(global::System.Nullable<int> value);
        partial void OnAssignedToUserIdChanged();
        /// <summary>
        /// There are no comments for Property AssignedTo in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string AssignedTo
        {
            get
            {
                return this._AssignedTo;
            }
            set
            {
                this.OnAssignedToChanging(value);
                this._AssignedTo = value;
                this.OnAssignedToChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _AssignedTo;
        partial void OnAssignedToChanging(string value);
        partial void OnAssignedToChanged();
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
        /// There are no comments for Property QueryId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<long> QueryId
        {
            get
            {
                return this._QueryId;
            }
            set
            {
                this.OnQueryIdChanging(value);
                this._QueryId = value;
                this.OnQueryIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<long> _QueryId;
        partial void OnQueryIdChanging(global::System.Nullable<long> value);
        partial void OnQueryIdChanged();
        /// <summary>
        /// There are no comments for Property QueryVersionId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual long QueryVersionId
        {
            get
            {
                return this._QueryVersionId;
            }
            set
            {
                this.OnQueryVersionIdChanging(value);
                this._QueryVersionId = value;
                this.OnQueryVersionIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private long _QueryVersionId;
        partial void OnQueryVersionIdChanging(long value);
        partial void OnQueryVersionIdChanged();
        /// <summary>
        /// There are no comments for Property Query in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.Query Query
        {
            get
            {
                return this._Query;
            }
            set
            {
                this.OnQueryChanging(value);
                this._Query = value;
                this.OnQueryChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.Query _Query;
        partial void OnQueryChanging(global::CxDataRepository.Query value);
        partial void OnQueryChanged();
        /// <summary>
        /// There are no comments for Property Scan in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.Scan Scan
        {
            get
            {
                return this._Scan;
            }
            set
            {
                this.OnScanChanging(value);
                this._Scan = value;
                this.OnScanChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.Scan _Scan;
        partial void OnScanChanging(global::CxDataRepository.Scan value);
        partial void OnScanChanged();
        /// <summary>
        /// There are no comments for Property AssignedToUser in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.User AssignedToUser
        {
            get
            {
                return this._AssignedToUser;
            }
            set
            {
                this.OnAssignedToUserChanging(value);
                this._AssignedToUser = value;
                this.OnAssignedToUserChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.User _AssignedToUser;
        partial void OnAssignedToUserChanging(global::CxDataRepository.User value);
        partial void OnAssignedToUserChanged();
        /// <summary>
        /// There are no comments for Property State in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.ResultState State
        {
            get
            {
                return this._State;
            }
            set
            {
                this.OnStateChanging(value);
                this._State = value;
                this.OnStateChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.ResultState _State;
        partial void OnStateChanging(global::CxDataRepository.ResultState value);
        partial void OnStateChanged();
    }
}