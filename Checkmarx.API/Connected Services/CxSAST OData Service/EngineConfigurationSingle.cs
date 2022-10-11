namespace CxDataRepository
{
/// <summary>
    /// There are no comments for EngineConfigurationSingle in the schema.
    /// </summary>
    public partial class EngineConfigurationSingle : global::Microsoft.OData.Client.DataServiceQuerySingle<EngineConfiguration>
    {
        /// <summary>
        /// Initialize a new EngineConfigurationSingle object.
        /// </summary>
        public EngineConfigurationSingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new EngineConfigurationSingle object.
        /// </summary>
        public EngineConfigurationSingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new EngineConfigurationSingle object.
        /// </summary>
        public EngineConfigurationSingle(global::Microsoft.OData.Client.DataServiceQuerySingle<EngineConfiguration> query)
            : base(query) {}

    }
}