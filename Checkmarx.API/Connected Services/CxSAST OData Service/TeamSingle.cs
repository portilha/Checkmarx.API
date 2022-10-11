namespace CxDataRepository
{
/// <summary>
    /// There are no comments for TeamSingle in the schema.
    /// </summary>
    public partial class TeamSingle : global::Microsoft.OData.Client.DataServiceQuerySingle<Team>
    {
        /// <summary>
        /// Initialize a new TeamSingle object.
        /// </summary>
        public TeamSingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new TeamSingle object.
        /// </summary>
        public TeamSingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new TeamSingle object.
        /// </summary>
        public TeamSingle(global::Microsoft.OData.Client.DataServiceQuerySingle<Team> query)
            : base(query) {}

        /// <summary>
        /// There are no comments for Users in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.User> Users
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._Users == null))
                {
                    this._Users = Context.CreateQuery<global::CxDataRepository.User>(GetPath("Users"));
                }
                return this._Users;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.User> _Users;
    }
}