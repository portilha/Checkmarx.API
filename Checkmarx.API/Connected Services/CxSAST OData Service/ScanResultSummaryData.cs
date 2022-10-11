namespace CxDataRepository
{
/// <summary>
    /// There are no comments for ScanResultSummaryData in the schema.
    /// </summary>
    /// <KeyProperties>
    /// ScanId
    /// </KeyProperties>
    [global::Microsoft.OData.Client.Key("ScanId")]
    public partial class ScanResultSummaryData : global::Microsoft.OData.Client.BaseEntityType
    {
        /// <summary>
        /// Create a new ScanResultSummaryData object.
        /// </summary>
        /// <param name="new">Initial value of New.</param>
        /// <param name="recurrent">Initial value of Recurrent.</param>
        /// <param name="resolved">Initial value of Resolved.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public static ScanResultSummaryData CreateScanResultSummaryData(int @new, int recurrent, int resolved)
        {
            ScanResultSummaryData scanResultSummaryData = new ScanResultSummaryData();
            scanResultSummaryData.New = @new;
            scanResultSummaryData.Recurrent = recurrent;
            scanResultSummaryData.Resolved = resolved;
            return scanResultSummaryData;
        }
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
        /// There are no comments for Property PreviousScanId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<long> PreviousScanId
        {
            get
            {
                return this._PreviousScanId;
            }
            set
            {
                this.OnPreviousScanIdChanging(value);
                this._PreviousScanId = value;
                this.OnPreviousScanIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<long> _PreviousScanId;
        partial void OnPreviousScanIdChanging(global::System.Nullable<long> value);
        partial void OnPreviousScanIdChanged();
        /// <summary>
        /// There are no comments for Property New in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual int New
        {
            get
            {
                return this._New;
            }
            set
            {
                this.OnNewChanging(value);
                this._New = value;
                this.OnNewChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private int _New;
        partial void OnNewChanging(int value);
        partial void OnNewChanged();
        /// <summary>
        /// There are no comments for Property Recurrent in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual int Recurrent
        {
            get
            {
                return this._Recurrent;
            }
            set
            {
                this.OnRecurrentChanging(value);
                this._Recurrent = value;
                this.OnRecurrentChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private int _Recurrent;
        partial void OnRecurrentChanging(int value);
        partial void OnRecurrentChanged();
        /// <summary>
        /// There are no comments for Property Resolved in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual int Resolved
        {
            get
            {
                return this._Resolved;
            }
            set
            {
                this.OnResolvedChanging(value);
                this._Resolved = value;
                this.OnResolvedChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private int _Resolved;
        partial void OnResolvedChanging(int value);
        partial void OnResolvedChanged();
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
        /// There are no comments for Property PreviousScan in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.Scan PreviousScan
        {
            get
            {
                return this._PreviousScan;
            }
            set
            {
                this.OnPreviousScanChanging(value);
                this._PreviousScan = value;
                this.OnPreviousScanChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.Scan _PreviousScan;
        partial void OnPreviousScanChanging(global::CxDataRepository.Scan value);
        partial void OnPreviousScanChanged();
    }
}