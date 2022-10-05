namespace CxDataRepository
{
/// <summary>
    /// There are no comments for EngineServer in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::Microsoft.OData.Client.Key("Id")]
    public partial class EngineServer : global::Microsoft.OData.Client.BaseEntityType
    {
        /// <summary>
        /// Create a new EngineServer object.
        /// </summary>
        /// <param name="ID">Initial value of Id.</param>
        /// <param name="isEnabled">Initial value of IsEnabled.</param>
        /// <param name="isAlive">Initial value of IsAlive.</param>
        /// <param name="maxConcurrentScans">Initial value of MaxConcurrentScans.</param>
        /// <param name="scanMinLOC">Initial value of ScanMinLOC.</param>
        /// <param name="scanMaxLOC">Initial value of ScanMaxLOC.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public static EngineServer CreateEngineServer(long ID, 
                    bool isEnabled, 
                    bool isAlive, 
                    int maxConcurrentScans, 
                    int scanMinLOC, 
                    int scanMaxLOC)
        {
            EngineServer engineServer = new EngineServer();
            engineServer.Id = ID;
            engineServer.IsEnabled = isEnabled;
            engineServer.IsAlive = isAlive;
            engineServer.MaxConcurrentScans = maxConcurrentScans;
            engineServer.ScanMinLOC = scanMinLOC;
            engineServer.ScanMaxLOC = scanMaxLOC;
            return engineServer;
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
        /// There are no comments for Property Url in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string Url
        {
            get
            {
                return this._Url;
            }
            set
            {
                this.OnUrlChanging(value);
                this._Url = value;
                this.OnUrlChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _Url;
        partial void OnUrlChanging(string value);
        partial void OnUrlChanged();
        /// <summary>
        /// There are no comments for Property IsEnabled in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual bool IsEnabled
        {
            get
            {
                return this._IsEnabled;
            }
            set
            {
                this.OnIsEnabledChanging(value);
                this._IsEnabled = value;
                this.OnIsEnabledChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private bool _IsEnabled;
        partial void OnIsEnabledChanging(bool value);
        partial void OnIsEnabledChanged();
        /// <summary>
        /// There are no comments for Property IsAlive in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual bool IsAlive
        {
            get
            {
                return this._IsAlive;
            }
            set
            {
                this.OnIsAliveChanging(value);
                this._IsAlive = value;
                this.OnIsAliveChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private bool _IsAlive;
        partial void OnIsAliveChanging(bool value);
        partial void OnIsAliveChanged();
        /// <summary>
        /// There are no comments for Property MaxConcurrentScans in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual int MaxConcurrentScans
        {
            get
            {
                return this._MaxConcurrentScans;
            }
            set
            {
                this.OnMaxConcurrentScansChanging(value);
                this._MaxConcurrentScans = value;
                this.OnMaxConcurrentScansChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private int _MaxConcurrentScans;
        partial void OnMaxConcurrentScansChanging(int value);
        partial void OnMaxConcurrentScansChanged();
        /// <summary>
        /// There are no comments for Property ScanMinLOC in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual int ScanMinLOC
        {
            get
            {
                return this._ScanMinLOC;
            }
            set
            {
                this.OnScanMinLOCChanging(value);
                this._ScanMinLOC = value;
                this.OnScanMinLOCChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private int _ScanMinLOC;
        partial void OnScanMinLOCChanging(int value);
        partial void OnScanMinLOCChanged();
        /// <summary>
        /// There are no comments for Property ScanMaxLOC in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual int ScanMaxLOC
        {
            get
            {
                return this._ScanMaxLOC;
            }
            set
            {
                this.OnScanMaxLOCChanging(value);
                this._ScanMaxLOC = value;
                this.OnScanMaxLOCChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private int _ScanMaxLOC;
        partial void OnScanMaxLOCChanging(int value);
        partial void OnScanMaxLOCChanged();
    }
}