namespace CxDataRepository
{
/// <summary>
    /// There are no comments for Team in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::Microsoft.OData.Client.Key("Id")]
    public partial class Team : global::Microsoft.OData.Client.BaseEntityType
    {
        /// <summary>
        /// Create a new Team object.
        /// </summary>
        /// <param name="ID">Initial value of Id.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public static Team CreateTeam(string ID)
        {
            Team team = new Team();
            team.Id = ID;
            return team;
        }
        /// <summary>
        /// There are no comments for Property Id in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string Id
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
        private string _Id;
        partial void OnIdChanging(string value);
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
        /// There are no comments for Property FullName in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string FullName
        {
            get
            {
                return this._FullName;
            }
            set
            {
                this.OnFullNameChanging(value);
                this._FullName = value;
                this.OnFullNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _FullName;
        partial void OnFullNameChanging(string value);
        partial void OnFullNameChanged();
        /// <summary>
        /// There are no comments for Property Users in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Collections.ObjectModel.Collection<global::CxDataRepository.User> Users
        {
            get
            {
                return this._Users;
            }
            set
            {
                this.OnUsersChanging(value);
                this._Users = value;
                this.OnUsersChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Collections.ObjectModel.Collection<global::CxDataRepository.User> _Users = new global::System.Collections.ObjectModel.Collection<global::CxDataRepository.User>();
        partial void OnUsersChanging(global::System.Collections.ObjectModel.Collection<global::CxDataRepository.User> value);
        partial void OnUsersChanged();
    }
}