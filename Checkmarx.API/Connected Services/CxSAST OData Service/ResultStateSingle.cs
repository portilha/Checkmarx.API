namespace CxDataRepository
{
/// <summary>
    /// There are no comments for ResultStateSingle in the schema.
    /// </summary>
    public partial class ResultStateSingle : global::Microsoft.OData.Client.DataServiceQuerySingle<ResultState>
    {
        /// <summary>
        /// Initialize a new ResultStateSingle object.
        /// </summary>
        public ResultStateSingle(global::Microsoft.OData.Client.DataServiceContext context, string path)
            : base(context, path) {}

        /// <summary>
        /// Initialize a new ResultStateSingle object.
        /// </summary>
        public ResultStateSingle(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isComposable)
            : base(context, path, isComposable) {}

        /// <summary>
        /// Initialize a new ResultStateSingle object.
        /// </summary>
        public ResultStateSingle(global::Microsoft.OData.Client.DataServiceQuerySingle<ResultState> query)
            : base(query) {}

    }
}