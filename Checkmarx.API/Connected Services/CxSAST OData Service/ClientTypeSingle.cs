namespace CxDataRepository
{
/// <summary>
    /// There are no comments for ClientTypeSingle in the schema.
    /// </summary>
    public partial class ClientTypeSingle : global::Microsoft.OData.Client.DataServiceQuerySingle<ClientType>
    {
        /// <summary>
        /// Initialize a new ClientTypeSingle object.
        /// </summary>
        public ClientTypeSingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new ClientTypeSingle object.
        /// </summary>
        public ClientTypeSingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new ClientTypeSingle object.
        /// </summary>
        public ClientTypeSingle(global::Microsoft.OData.Client.DataServiceQuerySingle<ClientType> query)
            : base(query) {}

    }
}