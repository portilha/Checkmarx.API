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
    /// Class containing all extension methods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.Project as Checkmarx.API.SAST.OData.ProjectSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.ProjectSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.Project> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.ProjectSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.Project as Checkmarx.API.SAST.OData.ProjectSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static Checkmarx.API.SAST.OData.ProjectSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.Project> _source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new Checkmarx.API.SAST.OData.ProjectSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.Scan as Checkmarx.API.SAST.OData.ScanSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.ScanSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.Scan> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.ScanSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.Scan as Checkmarx.API.SAST.OData.ScanSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static Checkmarx.API.SAST.OData.ScanSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.Scan> _source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new Checkmarx.API.SAST.OData.ScanSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.ProjectCustomField as Checkmarx.API.SAST.OData.ProjectCustomFieldSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.ProjectCustomFieldSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.ProjectCustomField> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.ProjectCustomFieldSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.ProjectCustomField as Checkmarx.API.SAST.OData.ProjectCustomFieldSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="fieldName">The value of fieldName</param>
        /// <param name="projectId">The value of projectId</param>
        public static Checkmarx.API.SAST.OData.ProjectCustomFieldSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.ProjectCustomField> _source,
            string fieldName, 
            global::System.Nullable<long> projectId)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "FieldName", fieldName }, 
                { "ProjectId", projectId }
            };
            return new Checkmarx.API.SAST.OData.ProjectCustomFieldSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.EngineConfiguration as Checkmarx.API.SAST.OData.EngineConfigurationSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.EngineConfigurationSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.EngineConfiguration> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.EngineConfigurationSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.EngineConfiguration as Checkmarx.API.SAST.OData.EngineConfigurationSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static Checkmarx.API.SAST.OData.EngineConfigurationSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.EngineConfiguration> _source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new Checkmarx.API.SAST.OData.EngineConfigurationSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.ClientType as Checkmarx.API.SAST.OData.ClientTypeSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.ClientTypeSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.ClientType> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.ClientTypeSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.ClientType as Checkmarx.API.SAST.OData.ClientTypeSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static Checkmarx.API.SAST.OData.ClientTypeSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.ClientType> _source,
            int id)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new Checkmarx.API.SAST.OData.ClientTypeSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.User as Checkmarx.API.SAST.OData.UserSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.UserSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.User> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.UserSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.User as Checkmarx.API.SAST.OData.UserSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static Checkmarx.API.SAST.OData.UserSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.User> _source,
            int id)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new Checkmarx.API.SAST.OData.UserSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.Team as Checkmarx.API.SAST.OData.TeamSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.TeamSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.Team> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.TeamSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.Team as Checkmarx.API.SAST.OData.TeamSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static Checkmarx.API.SAST.OData.TeamSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.Team> _source,
            int id)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new Checkmarx.API.SAST.OData.TeamSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.Preset as Checkmarx.API.SAST.OData.PresetSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.PresetSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.Preset> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.PresetSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.Preset as Checkmarx.API.SAST.OData.PresetSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static Checkmarx.API.SAST.OData.PresetSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.Preset> _source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new Checkmarx.API.SAST.OData.PresetSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.Result as Checkmarx.API.SAST.OData.ResultSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.ResultSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.Result> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.ResultSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.Result as Checkmarx.API.SAST.OData.ResultSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="id">The value of id</param>
        /// <param name="scanId">The value of scanId</param>
        public static Checkmarx.API.SAST.OData.ResultSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.Result> _source,
            int id, 
            global::System.Nullable<long> scanId)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }, 
                { "ScanId", scanId }
            };
            return new Checkmarx.API.SAST.OData.ResultSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.TopScanVulnerability as Checkmarx.API.SAST.OData.TopScanVulnerabilitySingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.TopScanVulnerabilitySingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.TopScanVulnerability> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.TopScanVulnerabilitySingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.TopScanVulnerability as Checkmarx.API.SAST.OData.TopScanVulnerabilitySingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="rank">The value of rank</param>
        /// <param name="scanId">The value of scanId</param>
        public static Checkmarx.API.SAST.OData.TopScanVulnerabilitySingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.TopScanVulnerability> _source,
            int rank, 
            long scanId)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Rank", rank }, 
                { "ScanId", scanId }
            };
            return new Checkmarx.API.SAST.OData.TopScanVulnerabilitySingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.QueryLanguage as Checkmarx.API.SAST.OData.QueryLanguageSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.QueryLanguageSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.QueryLanguage> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.QueryLanguageSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.QueryLanguage as Checkmarx.API.SAST.OData.QueryLanguageSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="languageId">The value of languageId</param>
        /// <param name="versionId">The value of versionId</param>
        public static Checkmarx.API.SAST.OData.QueryLanguageSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.QueryLanguage> _source,
            int languageId, 
            int versionId)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "LanguageId", languageId }, 
                { "VersionId", versionId }
            };
            return new Checkmarx.API.SAST.OData.QueryLanguageSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.EngineServer as Checkmarx.API.SAST.OData.EngineServerSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.EngineServerSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.EngineServer> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.EngineServerSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.EngineServer as Checkmarx.API.SAST.OData.EngineServerSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static Checkmarx.API.SAST.OData.EngineServerSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.EngineServer> _source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new Checkmarx.API.SAST.OData.EngineServerSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.ScanResultSummaryData as Checkmarx.API.SAST.OData.ScanResultSummaryDataSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.ScanResultSummaryDataSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.ScanResultSummaryData> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.ScanResultSummaryDataSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.ScanResultSummaryData as Checkmarx.API.SAST.OData.ScanResultSummaryDataSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="scanId">The value of scanId</param>
        public static Checkmarx.API.SAST.OData.ScanResultSummaryDataSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.ScanResultSummaryData> _source,
            global::System.Nullable<long> scanId)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "ScanId", scanId }
            };
            return new Checkmarx.API.SAST.OData.ScanResultSummaryDataSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.Query as Checkmarx.API.SAST.OData.QuerySingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.QuerySingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.Query> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.QuerySingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.Query as Checkmarx.API.SAST.OData.QuerySingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static Checkmarx.API.SAST.OData.QuerySingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.Query> _source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new Checkmarx.API.SAST.OData.QuerySingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.ResultState as Checkmarx.API.SAST.OData.ResultStateSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.ResultStateSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.ResultState> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.ResultStateSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.ResultState as Checkmarx.API.SAST.OData.ResultStateSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static Checkmarx.API.SAST.OData.ResultStateSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.ResultState> _source,
            int id)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new Checkmarx.API.SAST.OData.ResultStateSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.QueryCxDescription as Checkmarx.API.SAST.OData.QueryCxDescriptionSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.QueryCxDescriptionSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.QueryCxDescription> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.QueryCxDescriptionSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.QueryCxDescription as Checkmarx.API.SAST.OData.QueryCxDescriptionSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static Checkmarx.API.SAST.OData.QueryCxDescriptionSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.QueryCxDescription> _source,
            int id)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new Checkmarx.API.SAST.OData.QueryCxDescriptionSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.QueryCweDescription as Checkmarx.API.SAST.OData.QueryCweDescriptionSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.QueryCweDescriptionSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.QueryCweDescription> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.QueryCweDescriptionSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.QueryCweDescription as Checkmarx.API.SAST.OData.QueryCweDescriptionSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static Checkmarx.API.SAST.OData.QueryCweDescriptionSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.QueryCweDescription> _source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new Checkmarx.API.SAST.OData.QueryCweDescriptionSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.QuerySource as Checkmarx.API.SAST.OData.QuerySourceSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.QuerySourceSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.QuerySource> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.QuerySourceSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.QuerySource as Checkmarx.API.SAST.OData.QuerySourceSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static Checkmarx.API.SAST.OData.QuerySourceSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.QuerySource> _source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new Checkmarx.API.SAST.OData.QuerySourceSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.QueryGroup as Checkmarx.API.SAST.OData.QueryGroupSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.QueryGroupSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.QueryGroup> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.QueryGroupSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.QueryGroup as Checkmarx.API.SAST.OData.QueryGroupSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static Checkmarx.API.SAST.OData.QueryGroupSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.QueryGroup> _source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new Checkmarx.API.SAST.OData.QueryGroupSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.QueryGroupType as Checkmarx.API.SAST.OData.QueryGroupTypeSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.QueryGroupTypeSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.QueryGroupType> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.QueryGroupTypeSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.QueryGroupType as Checkmarx.API.SAST.OData.QueryGroupTypeSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static Checkmarx.API.SAST.OData.QueryGroupTypeSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.QueryGroupType> _source,
            int id)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new Checkmarx.API.SAST.OData.QueryGroupTypeSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.QueryCategory as Checkmarx.API.SAST.OData.QueryCategorySingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.QueryCategorySingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.QueryCategory> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.QueryCategorySingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.QueryCategory as Checkmarx.API.SAST.OData.QueryCategorySingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static Checkmarx.API.SAST.OData.QueryCategorySingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.QueryCategory> _source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new Checkmarx.API.SAST.OData.QueryCategorySingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.QueryCategoryType as Checkmarx.API.SAST.OData.QueryCategoryTypeSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="_keys">dictionary with the names and values of keys</param>
        public static Checkmarx.API.SAST.OData.QueryCategoryTypeSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.QueryCategoryType> _source, global::System.Collections.Generic.IDictionary<string, object> _keys)
        {
            return new Checkmarx.API.SAST.OData.QueryCategoryTypeSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
        /// <summary>
        /// Get an entity of type Checkmarx.API.SAST.OData.QueryCategoryType as Checkmarx.API.SAST.OData.QueryCategoryTypeSingle specified by key from an entity set
        /// </summary>
        /// <param name="_source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static Checkmarx.API.SAST.OData.QueryCategoryTypeSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<Checkmarx.API.SAST.OData.QueryCategoryType> _source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> _keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new Checkmarx.API.SAST.OData.QueryCategoryTypeSingle(_source.Context, _source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(_source.Context, _keys)));
        }
    }
}
