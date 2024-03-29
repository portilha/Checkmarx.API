﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generation date: 10/5/2022 1:06:25 PM
namespace Checkmarx.API.SAST.OData
{
    /// <summary>
    /// There are no comments for ProjectCustomFieldSingle in the schema.
    /// </summary>
    [global::Microsoft.OData.Client.OriginalNameAttribute("ProjectCustomFieldSingle")]
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
        [global::Microsoft.OData.Client.OriginalNameAttribute("Project")]
        public virtual Checkmarx.API.SAST.OData.ProjectSingle Project
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._Project == null))
                {
                    this._Project = new Checkmarx.API.SAST.OData.ProjectSingle(this.Context, GetPath("Project"));
                }
                return this._Project;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private Checkmarx.API.SAST.OData.ProjectSingle _Project;
    }
    /// <summary>
    /// There are no comments for ProjectCustomField in the schema.
    /// </summary>
    /// <KeyProperties>
    /// FieldName
    /// ProjectId
    /// </KeyProperties>
    [global::Microsoft.OData.Client.Key("FieldName", "ProjectId")]
    [global::Microsoft.OData.Client.OriginalNameAttribute("ProjectCustomField")]
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

        [global::Microsoft.OData.Client.OriginalNameAttribute("ProjectId")]
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

        [global::Microsoft.OData.Client.OriginalNameAttribute("FieldName")]
        [global::System.ComponentModel.DataAnnotations.RequiredAttribute(ErrorMessage = "FieldName is required.")]
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

        [global::Microsoft.OData.Client.OriginalNameAttribute("FieldValue")]
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

        [global::Microsoft.OData.Client.OriginalNameAttribute("Project")]
        public virtual Checkmarx.API.SAST.OData.Project Project
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
        private Checkmarx.API.SAST.OData.Project _Project;
        partial void OnProjectChanging(Checkmarx.API.SAST.OData.Project value);
        partial void OnProjectChanged();
    }
}
