namespace CxDataRepository
{
/// <summary>
    /// There are no comments for ScanResultSummaryDataSingle in the schema.
    /// </summary>
    public partial class ScanResultSummaryDataSingle : global::Microsoft.OData.Client.DataServiceQuerySingle<ScanResultSummaryData>
    {
        /// <summary>
        /// Initialize a new ScanResultSummaryDataSingle object.
        /// </summary>
        public ScanResultSummaryDataSingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new ScanResultSummaryDataSingle object.
        /// </summary>
        public ScanResultSummaryDataSingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new ScanResultSummaryDataSingle object.
        /// </summary>
        public ScanResultSummaryDataSingle(global::Microsoft.OData.Client.DataServiceQuerySingle<ScanResultSummaryData> query)
            : base(query) {}

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
        /// There are no comments for PreviousScan in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.ScanSingle PreviousScan
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._PreviousScan == null))
                {
                    this._PreviousScan = new global::CxDataRepository.ScanSingle(this.Context, GetPath("PreviousScan"));
                }
                return this._PreviousScan;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.ScanSingle _PreviousScan;
    }
}