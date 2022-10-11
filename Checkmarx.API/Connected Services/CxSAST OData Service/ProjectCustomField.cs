namespace CxDataRepository
{
/// <summary>
    /// There are no comments for ProjectCustomField in the schema.
    /// </summary>
    /// <KeyProperties>
    /// ProjectId
    /// FieldName
    /// </KeyProperties>
    [global::Microsoft.OData.Client.Key("ProjectId", "FieldName")]
    public partial class ProjectCustomField : global::Microsoft.OData.Client.BaseEntityType
    {
        /// <summary>
        /// Create a new ProjectCustomField object.
        /// </summary>
        /// <param name="fieldName">Initial value of FieldName.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public static ProjectCustomField CreateProjectCustomField(string fieldName)
        {
            ProjectCustomField projectCustomField = new ProjectCustomField();
            projectCustomField.FieldName = fieldName;
            return projectCustomField;
        }
        /// <summary>
        /// There are no comments for Property ProjectId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::System.Nullable<long> ProjectId
        {
            get
            {
                return this._ProjectId;
            }
            set
            {
                this.OnProjectIdChanging(value);
                this._ProjectId = value;
                this.OnProjectIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Nullable<long> _ProjectId;
        partial void OnProjectIdChanging(global::System.Nullable<long> value);
        partial void OnProjectIdChanged();
        /// <summary>
        /// There are no comments for Property FieldName in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string FieldName
        {
            get
            {
                return this._FieldName;
            }
            set
            {
                this.OnFieldNameChanging(value);
                this._FieldName = value;
                this.OnFieldNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _FieldName;
        partial void OnFieldNameChanging(string value);
        partial void OnFieldNameChanged();
        /// <summary>
        /// There are no comments for Property FieldValue in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual string FieldValue
        {
            get
            {
                return this._FieldValue;
            }
            set
            {
                this.OnFieldValueChanging(value);
                this._FieldValue = value;
                this.OnFieldValueChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _FieldValue;
        partial void OnFieldValueChanging(string value);
        partial void OnFieldValueChanged();
        /// <summary>
        /// There are no comments for Property Project in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public virtual global::CxDataRepository.Project Project
        {
            get
            {
                return this._Project;
            }
            set
            {
                this.OnProjectChanging(value);
                this._Project = value;
                this.OnProjectChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::CxDataRepository.Project _Project;
        partial void OnProjectChanging(global::CxDataRepository.Project value);
        partial void OnProjectChanged();
    }
}