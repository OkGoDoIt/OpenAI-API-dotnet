using System.Threading.Tasks;
using OpenAI_API.Common;

namespace OpenAI_API.Threads
{
    /// <summary>
    /// The endpoint for the Threads API.
    /// </summary>
    public class ThreadsEndpoint : EndpointBase, IThreadsEndpoint
    {
        /// <summary>
        /// The name of the endpoint, which is the final path segment in the API URL.
        /// </summary>
        protected override string Endpoint => "threads";

        /// <summary>
        /// Constructs a new <see cref="ThreadsEndpoint"/>.
        /// </summary>
        /// 
        /// <param name="api">
        /// The <see cref="OpenAIAPI"/> instance to use for making requests.
        /// </param>
        public ThreadsEndpoint(OpenAIAPI api) : base(api) { }

        /// <inheritdoc />
        public async Task<ThreadResult> CreateThread(ThreadRequest request)
        {
            return await HttpPost<ThreadResult>(Url, request);
        }

        /// <inheritdoc />
        public async Task<ThreadResult> RetrieveThread(string threadId)
        {
            var url = $"{Url}/{threadId}";

            return await HttpGet<ThreadResult>(url);
        }

        /// <inheritdoc />
        public async Task<ThreadResult> ModifyThread(string threadId, MetadataRequest request)
        {
            var url = $"{Url}/{threadId}";

            return await HttpPut<ThreadResult>(url, request);
        }

        /// <inheritdoc />
        public async Task<DeletionStatus> DeleteThread(string threadId)
        {
            var url = $"{Url}/{threadId}";

            return await HttpDelete<DeletionStatus>(url);
        }
    }
}