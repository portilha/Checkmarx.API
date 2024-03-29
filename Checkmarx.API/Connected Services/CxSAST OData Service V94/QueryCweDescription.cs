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
    /// There are no comments for QueryCweDescriptionSingle in the schema.
    /// </summary>
    [global::Microsoft.OData.Client.OriginalNameAttribute("QueryCweDescriptionSingle")]
    public partial class QueryCweDescriptionSingle : global::Microsoft.OData.Client.DataServiceQuerySingle<QueryCweDescription>
    {
        /// <summary>
        /// Initialize a new QueryCweDescriptionSingle object.
        /// </summary>
        public QueryCweDescriptionSingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new QueryCweDescriptionSingle object.
        /// </summary>
        public QueryCweDescriptionSingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new QueryCweDescriptionSingle object.
        /// </summary>
        public QueryCweDescriptionSingle(global::Microsoft.OData.Client.DataServiceQuerySingle<QueryCweDescription> query)
            : base(query) {}

    }
    /// <summary>
    /// There are no comments for QueryCweDescription in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::Microsoft.OData.Client.Key("Id")]
    [global::Microsoft.OData.Client.OriginalNameAttribute("QueryCweDescription")]
    public partial class QueryCweDescription : global::Microsoft.OData.Client.BaseEntityType
    {
        /// <summary>
        /// Create a new QueryCweDescription object.
        /// </summary>
        /// <param name="ID">Initial value of Id.</param>
        /// <param name="lCID">Initial value of LCID.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public static QueryCweDescription CreateQueryCweDescription(long ID, int lCID)
        {
            QueryCweDescription queryCweDescription = new QueryCweDescription();
            queryCweDescription.Id = ID;
            queryCweDescription.LCID = lCID;
            return queryCweDescription;
        }
        /// <summary>
        /// There are no comments for Property Id in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]

        [global::Microsoft.OData.Client.OriginalNameAttribute("Id")]
        [global::System.ComponentModel.DataAnnotations.RequiredAttribute(ErrorMessage = "Id is required.")]
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
        /// There are no comments for Property LCID in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]

        [global::Microsoft.OData.Client.OriginalNameAttribute("LCID")]
        [global::System.ComponentModel.DataAnnotations.RequiredAttribute(ErrorMessage = "LCID is required.")]
        public virtual int LCID
        {
            get
            {
                return this._LCID;
            }
            set
            {
                this.OnLCIDChanging(value);
                this._LCID = value;
                this.OnLCIDChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private int _LCID;
        partial void OnLCIDChanging(int value);
        partial void OnLCIDChanged();
        /// <summary>
        /// There are no comments for Property HtmlDescription in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]

        [global::Microsoft.OData.Client.OriginalNameAttribute("HtmlDescription")]
        public virtual string HtmlDescription
        {
            get
            {
                return this._HtmlDescription;
            }
            set
            {
                this.OnHtmlDescriptionChanging(value);
                this._HtmlDescription = value;
                this.OnHtmlDescriptionChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private string _HtmlDescription;
        partial void OnHtmlDescriptionChanging(string value);
        partial void OnHtmlDescriptionChanged();
    }
}
