namespace CxDataRepository
{
/// <summary>
    /// There are no comments for EngineServerSingle in the schema.
    /// </summary>
    public partial class EngineServerSingle : global::Microsoft.OData.Client.DataServiceQuerySingle<EngineServer>
    {
        /// <summary>
        /// Initialize a new EngineServerSingle object.
        /// </summary>
        public EngineServerSingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new EngineServerSingle object.
        /// </summary>
        public EngineServerSingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new EngineServerSingle object.
        /// </summary>
        public EngineServerSingle(global::Microsoft.OData.Client.DataServiceQuerySingle<EngineServer> query)
            : base(query) {}

    }
}