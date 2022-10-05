namespace CxDataRepository
{
/// <summary>
    /// There are no comments for ResultSingle in the schema.
    /// </summary>
    public partial class ResultSingle : global::Microsoft.OData.Client.DataServiceQuerySingle<Result>
    {
        /// <summary>
        /// Initialize a new ResultSingle object.
        /// </summary>
        public ResultSingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new ResultSingle object.
        /// </summary>
        public ResultSingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new ResultSingle object.
        /// </summary>
        public ResultSingle(global::Microsoft.OData.Client.DataServiceQuerySingle<Result> query)
            : base(query) {}

        /// <summary>
        /// There are no comments for Query in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.QuerySingle Query
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._Query == null))
                {
                    this._Query = new global::CxDataRepository.QuerySingle(this.Context, GetPath("Query"));
                }
                return this._Query;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.QuerySingle _Query;
        /// <summary>
        /// There are no comments for Scan in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.ScanSingle Scan
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._Scan == null))
                {
                    this._Scan = new global::CxDataRepository.ScanSingle(this.Context, GetPath("Scan"));
                }
                return this._Scan;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.ScanSingle _Scan;
        /// <summary>
        /// There are no comments for AssignedToUser in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.UserSingle AssignedToUser
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._AssignedToUser == null))
                {
                    this._AssignedToUser = new global::CxDataRepository.UserSingle(this.Context, GetPath("AssignedToUser"));
                }
                return this._AssignedToUser;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.UserSingle _AssignedToUser;
        /// <summary>
        /// There are no comments for State in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.ResultStateSingle State
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._State == null))
                {
                    this._State = new global::CxDataRepository.ResultStateSingle(this.Context, GetPath("State"));
                }
                return this._State;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.ResultStateSingle _State;
    }
}