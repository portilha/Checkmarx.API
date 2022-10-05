namespace CxDataRepository
{
/// <summary>
    /// Class containing all extension methods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Get an entity of type global::CxDataRepository.Project as global::CxDataRepository.ProjectSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.ProjectSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.Project> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.ProjectSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.Project as global::CxDataRepository.ProjectSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static global::CxDataRepository.ProjectSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.Project> source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new global::CxDataRepository.ProjectSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.Scan as global::CxDataRepository.ScanSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.ScanSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.Scan> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.ScanSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.Scan as global::CxDataRepository.ScanSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static global::CxDataRepository.ScanSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.Scan> source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new global::CxDataRepository.ScanSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.ProjectCustomField as global::CxDataRepository.ProjectCustomFieldSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.ProjectCustomFieldSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.ProjectCustomField> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.ProjectCustomFieldSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.ProjectCustomField as global::CxDataRepository.ProjectCustomFieldSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="projectId">The value of projectId</param>
        /// <param name="fieldName">The value of fieldName</param>
        public static global::CxDataRepository.ProjectCustomFieldSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.ProjectCustomField> source,
            global::System.Nullable<long> projectId, 
            string fieldName)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "ProjectId", projectId }, 
                { "FieldName", fieldName }
            };
            return new global::CxDataRepository.ProjectCustomFieldSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.EngineConfiguration as global::CxDataRepository.EngineConfigurationSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.EngineConfigurationSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.EngineConfiguration> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.EngineConfigurationSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.EngineConfiguration as global::CxDataRepository.EngineConfigurationSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static global::CxDataRepository.EngineConfigurationSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.EngineConfiguration> source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new global::CxDataRepository.EngineConfigurationSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.ClientType as global::CxDataRepository.ClientTypeSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.ClientTypeSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.ClientType> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.ClientTypeSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.ClientType as global::CxDataRepository.ClientTypeSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static global::CxDataRepository.ClientTypeSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.ClientType> source,
            int id)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new global::CxDataRepository.ClientTypeSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.User as global::CxDataRepository.UserSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.UserSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.User> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.UserSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.User as global::CxDataRepository.UserSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static global::CxDataRepository.UserSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.User> source,
            int id)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new global::CxDataRepository.UserSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.Team as global::CxDataRepository.TeamSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.TeamSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.Team> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.TeamSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.Team as global::CxDataRepository.TeamSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static global::CxDataRepository.TeamSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.Team> source,
            string id)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new global::CxDataRepository.TeamSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.Preset as global::CxDataRepository.PresetSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.PresetSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.Preset> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.PresetSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.Preset as global::CxDataRepository.PresetSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static global::CxDataRepository.PresetSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.Preset> source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new global::CxDataRepository.PresetSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.Result as global::CxDataRepository.ResultSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.ResultSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.Result> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.ResultSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.Result as global::CxDataRepository.ResultSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="id">The value of id</param>
        /// <param name="scanId">The value of scanId</param>
        public static global::CxDataRepository.ResultSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.Result> source,
            int id, 
            global::System.Nullable<long> scanId)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }, 
                { "ScanId", scanId }
            };
            return new global::CxDataRepository.ResultSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.TopScanVulnerability as global::CxDataRepository.TopScanVulnerabilitySingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.TopScanVulnerabilitySingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.TopScanVulnerability> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.TopScanVulnerabilitySingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.TopScanVulnerability as global::CxDataRepository.TopScanVulnerabilitySingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="scanId">The value of scanId</param>
        /// <param name="rank">The value of rank</param>
        public static global::CxDataRepository.TopScanVulnerabilitySingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.TopScanVulnerability> source,
            long scanId, 
            int rank)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "ScanId", scanId }, 
                { "Rank", rank }
            };
            return new global::CxDataRepository.TopScanVulnerabilitySingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.QueryLanguage as global::CxDataRepository.QueryLanguageSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.QueryLanguageSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QueryLanguage> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.QueryLanguageSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.QueryLanguage as global::CxDataRepository.QueryLanguageSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="versionId">The value of versionId</param>
        /// <param name="languageId">The value of languageId</param>
        public static global::CxDataRepository.QueryLanguageSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QueryLanguage> source,
            int versionId, 
            int languageId)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "VersionId", versionId }, 
                { "LanguageId", languageId }
            };
            return new global::CxDataRepository.QueryLanguageSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.EngineServer as global::CxDataRepository.EngineServerSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.EngineServerSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.EngineServer> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.EngineServerSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.EngineServer as global::CxDataRepository.EngineServerSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static global::CxDataRepository.EngineServerSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.EngineServer> source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new global::CxDataRepository.EngineServerSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.ScanResultSummaryData as global::CxDataRepository.ScanResultSummaryDataSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.ScanResultSummaryDataSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.ScanResultSummaryData> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.ScanResultSummaryDataSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.ScanResultSummaryData as global::CxDataRepository.ScanResultSummaryDataSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="scanId">The value of scanId</param>
        public static global::CxDataRepository.ScanResultSummaryDataSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.ScanResultSummaryData> source,
            global::System.Nullable<long> scanId)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "ScanId", scanId }
            };
            return new global::CxDataRepository.ScanResultSummaryDataSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.Query as global::CxDataRepository.QuerySingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.QuerySingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.Query> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.QuerySingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.Query as global::CxDataRepository.QuerySingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static global::CxDataRepository.QuerySingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.Query> source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new global::CxDataRepository.QuerySingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.ResultState as global::CxDataRepository.ResultStateSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.ResultStateSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.ResultState> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.ResultStateSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.ResultState as global::CxDataRepository.ResultStateSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static global::CxDataRepository.ResultStateSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.ResultState> source,
            int id)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new global::CxDataRepository.ResultStateSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.QueryCxDescription as global::CxDataRepository.QueryCxDescriptionSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.QueryCxDescriptionSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QueryCxDescription> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.QueryCxDescriptionSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.QueryCxDescription as global::CxDataRepository.QueryCxDescriptionSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static global::CxDataRepository.QueryCxDescriptionSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QueryCxDescription> source,
            int id)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new global::CxDataRepository.QueryCxDescriptionSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.QueryCweDescription as global::CxDataRepository.QueryCweDescriptionSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.QueryCweDescriptionSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QueryCweDescription> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.QueryCweDescriptionSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.QueryCweDescription as global::CxDataRepository.QueryCweDescriptionSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static global::CxDataRepository.QueryCweDescriptionSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QueryCweDescription> source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new global::CxDataRepository.QueryCweDescriptionSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.QuerySource as global::CxDataRepository.QuerySourceSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.QuerySourceSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QuerySource> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.QuerySourceSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.QuerySource as global::CxDataRepository.QuerySourceSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static global::CxDataRepository.QuerySourceSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QuerySource> source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new global::CxDataRepository.QuerySourceSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.QueryGroup as global::CxDataRepository.QueryGroupSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.QueryGroupSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QueryGroup> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.QueryGroupSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.QueryGroup as global::CxDataRepository.QueryGroupSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static global::CxDataRepository.QueryGroupSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QueryGroup> source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new global::CxDataRepository.QueryGroupSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.QueryGroupType as global::CxDataRepository.QueryGroupTypeSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.QueryGroupTypeSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QueryGroupType> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.QueryGroupTypeSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.QueryGroupType as global::CxDataRepository.QueryGroupTypeSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static global::CxDataRepository.QueryGroupTypeSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QueryGroupType> source,
            int id)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new global::CxDataRepository.QueryGroupTypeSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.QueryCategory as global::CxDataRepository.QueryCategorySingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.QueryCategorySingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QueryCategory> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.QueryCategorySingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.QueryCategory as global::CxDataRepository.QueryCategorySingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static global::CxDataRepository.QueryCategorySingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QueryCategory> source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new global::CxDataRepository.QueryCategorySingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.QueryCategoryType as global::CxDataRepository.QueryCategoryTypeSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="keys">dictionary with the names and values of keys</param>
        public static global::CxDataRepository.QueryCategoryTypeSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QueryCategoryType> source, global::System.Collections.Generic.IDictionary<string, object> keys)
        {
            return new global::CxDataRepository.QueryCategoryTypeSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
        /// <summary>
        /// Get an entity of type global::CxDataRepository.QueryCategoryType as global::CxDataRepository.QueryCategoryTypeSingle specified by key from an entity set
        /// </summary>
        /// <param name="source">source entity set</param>
        /// <param name="id">The value of id</param>
        public static global::CxDataRepository.QueryCategoryTypeSingle ByKey(this global::Microsoft.OData.Client.DataServiceQuery<global::CxDataRepository.QueryCategoryType> source,
            long id)
        {
            global::System.Collections.Generic.IDictionary<string, object> keys = new global::System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", id }
            };
            return new global::CxDataRepository.QueryCategoryTypeSingle(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetKeyString(source.Context, keys)));
        }
    }
}