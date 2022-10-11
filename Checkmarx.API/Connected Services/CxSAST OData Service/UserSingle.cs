namespace CxDataRepository
{
/// <summary>
    /// There are no comments for UserSingle in the schema.
    /// </summary>
    public partial class UserSingle : global::Microsoft.OData.Client.DataServiceQuerySingle<User>
    {
        /// <summary>
        /// Initialize a new UserSingle object.
        /// </summary>
        public UserSingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new UserSingle object.
        /// </summary>
        public UserSingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new UserSingle object.
        /// </summary>
        public UserSingle(global::Microsoft.OData.Client.DataServiceQuerySingle<User> query)
            : base(query) {}

        /// <summary>
        /// There are no comments for Team in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.TeamSingle Team
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._Team == null))
                {
                    this._Team = new global::CxDataRepository.TeamSingle(this.Context, GetPath("Team"));
                }
                return this._Team;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.TeamSingle _Team;
    }
}