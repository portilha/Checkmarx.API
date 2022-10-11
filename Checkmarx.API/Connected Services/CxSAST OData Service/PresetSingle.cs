namespace CxDataRepository
{
/// <summary>
    /// There are no comments for PresetSingle in the schema.
    /// </summary>
    public partial class PresetSingle : global::Microsoft.OData.Client.DataServiceQuerySingle<Preset>
    {
        /// <summary>
        /// Initialize a new PresetSingle object.
        /// </summary>
        public PresetSingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new PresetSingle object.
        /// </summary>
        public PresetSingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new PresetSingle object.
        /// </summary>
        public PresetSingle(global::Microsoft.OData.Client.DataServiceQuerySingle<Preset> query)
            : base(query) {}

    }
}