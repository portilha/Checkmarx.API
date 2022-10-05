namespace CxDataRepository
{
/// <summary>
    /// There are no comments for Preset in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::Microsoft.OData.Client.Key("Id")]
    public partial class Preset : global::Microsoft.OData.Client.BaseEntityType
    {
        /// <summary>
        /// Create a new Preset object.
        /// </summary>
        /// <param name="ID">Initial value of Id.</param>
        /// <param name="isSystemPreset">Initial value of IsSystemPreset.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public static Preset CreatePreset(long ID, bool isSystemPreset)
        {
            Preset preset = new Preset();
            preset.Id = ID;
            preset.IsSystemPreset = isSystemPreset;
            return preset;
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
        /// There are no comments for Property IsSystemPreset in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual bool IsSystemPreset
        {
            get
            {
                return this._IsSystemPreset;
            }
            set
            {
                this.OnIsSystemPresetChanging(value);
                this._IsSystemPreset = value;
                this.OnIsSystemPresetChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private bool _IsSystemPreset;
        partial void OnIsSystemPresetChanging(bool value);
        partial void OnIsSystemPresetChanged();
    }
}