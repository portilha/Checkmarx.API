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
    /// There are no comments for QueryCategorySingle in the schema.
    /// </summary>
    [global::Microsoft.OData.Client.OriginalNameAttribute("QueryCategorySingle")]
    public partial class QueryCategorySingle : global::Microsoft.OData.Client.DataServiceQuerySingle<QueryCategory>
    {
        /// <summary>
        /// Initialize a new QueryCategorySingle object.
        /// </summary>
        public QueryCategorySingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new QueryCategorySingle object.
        /// </summary>
        public QueryCategorySingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new QueryCategorySingle object.
        /// </summary>
        public QueryCategorySingle(global::Microsoft.OData.Client.DataServiceQuerySingle<QueryCategory> query)
            : base(query) {}

        /// <summary>
        /// There are no comments for QueryCategoryType in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        [global::Microsoft.OData.Client.OriginalNameAttribute("QueryCategoryType")]
        public virtual Checkmarx.API.SAST.OData.QueryCategoryTypeSingle QueryCategoryType
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._QueryCategoryType == null))
                {
                    this._QueryCategoryType = new Checkmarx.API.SAST.OData.QueryCategoryTypeSingle(this.Context, GetPath("QueryCategoryType"));
                }
                return this._QueryCategoryType;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private Checkmarx.API.SAST.OData.QueryCategoryTypeSingle _QueryCategoryType;
        /// <summary>
        /// There are no comments for Queries in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        [global::Microsoft.OData.Client.OriginalNameAttribute("Queries")]
        public virtual global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.Query> Queries
        {
            get
            {
                if (!this.IsComposable)
                {
                    throw new global::System.NotSupportedException("The previous function is not composable.");
                }
                if ((this._Queries == null))
                {
                    this._Queries = Context.CreateQuery<Checkmarx.API.SAST.OData.Query>(GetPath("Queries"));
                }
                return this._Queries;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.Query> _Queries;
    }
    /// <summary>
    /// There are no comments for QueryCategory in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::Microsoft.OData.Client.Key("Id")]
    [global::Microsoft.OData.Client.OriginalNameAttribute("QueryCategory")]
    public partial class QueryCategory : global::Microsoft.OData.Client.BaseEntityType
    {
        /// <summary>
        /// Create a new QueryCategory object.
        /// </summary>
        /// <param name="ID">Initial value of Id.</param>
        /// <param name="typeId">Initial value of TypeId.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        public static QueryCategory CreateQueryCategory(long ID, long typeId)
        {
            QueryCategory queryCategory = new QueryCategory();
            queryCategory.Id = ID;
            queryCategory.TypeId = typeId;
            return queryCategory;
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
        /// There are no comments for Property Name in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]

        [global::Microsoft.OData.Client.OriginalNameAttribute("Name")]
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
        /// There are no comments for Property TypeId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]

        [global::Microsoft.OData.Client.OriginalNameAttribute("TypeId")]
        [global::System.ComponentModel.DataAnnotations.RequiredAttribute(ErrorMessage = "TypeId is required.")]
        public virtual long TypeId
        {
            get
            {
                return this._TypeId;
            }
            set
            {
                this.OnTypeIdChanging(value);
                this._TypeId = value;
                this.OnTypeIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private long _TypeId;
        partial void OnTypeIdChanging(long value);
        partial void OnTypeIdChanged();
        /// <summary>
        /// There are no comments for Property QueryCategoryType in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]

        [global::Microsoft.OData.Client.OriginalNameAttribute("QueryCategoryType")]
        public virtual Checkmarx.API.SAST.OData.QueryCategoryType QueryCategoryType
        {
            get
            {
                return this._QueryCategoryType;
            }
            set
            {
                this.OnQueryCategoryTypeChanging(value);
                this._QueryCategoryType = value;
                this.OnQueryCategoryTypeChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private Checkmarx.API.SAST.OData.QueryCategoryType _QueryCategoryType;
        partial void OnQueryCategoryTypeChanging(Checkmarx.API.SAST.OData.QueryCategoryType value);
        partial void OnQueryCategoryTypeChanged();
        /// <summary>
        /// There are no comments for Property Queries in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]

        [global::Microsoft.OData.Client.OriginalNameAttribute("Queries")]
        public virtual global::System.Collections.ObjectModel.Collection<Checkmarx.API.SAST.OData.Query> Queries
        {
            get
            {
                return this._Queries;
            }
            set
            {
                this.OnQueriesChanging(value);
                this._Queries = value;
                this.OnQueriesChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.OData.Client.Design.T4", "#VersionNumber#")]
        private global::System.Collections.ObjectModel.Collection<Checkmarx.API.SAST.OData.Query> _Queries = new global::System.Collections.ObjectModel.Collection<Checkmarx.API.SAST.OData.Query>();
        partial void OnQueriesChanging(global::System.Collections.ObjectModel.Collection<Checkmarx.API.SAST.OData.Query> value);
        partial void OnQueriesChanged();
    }
}
