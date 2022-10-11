namespace CxDataRepository
{
/// <summary>
    /// There are no comments for ProjectCustomFieldSingle in the schema.
    /// </summary>
    public partial class ProjectCustomFieldSingle : global::Microsoft.OData.Client.DataServiceQuerySingle<ProjectCustomField>
    {
        /// <summary>
        /// Initialize a new ProjectCustomFieldSingle object.
        /// </summary>
        public ProjectCustomFieldSingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new ProjectCustomFieldSingle object.
        /// </summary>
        public ProjectCustomFieldSingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new ProjectCustomFieldSingle object.
        /// </summary>
        public ProjectCustomFieldSingle(global::Microsoft.OData.Client.DataServiceQuerySingle<ProjectCustomField> query)
            : base(query) {}

        /// <summary>
        /// There are no comments for Project in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.ProjectSingle Project
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._Project == null))
                {
                    this._Project = new global::CxDataRepository.ProjectSingle(this.Context, GetPath("Project"));
                }
                return this._Project;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.ProjectSingle _Project;
    }
}