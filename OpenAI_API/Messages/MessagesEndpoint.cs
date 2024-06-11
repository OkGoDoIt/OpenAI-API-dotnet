using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenAI_API.Common;

namespace OpenAI_API.Messages
{
    /// <summary>
    /// The endpoint for the Messages API.
    /// </summary>
    public class MessagesEndpoint : EndpointBase, IMessagesEndpoint
    {
        /// <summary>
        /// The name of the endpoint, which is the final path segment in the API URL.
        /// </summary>
        protected override string Endpoint => "threads";

        /// <summary>
        /// Constructs a new <see cref="MessagesEndpoint"/>.
        /// </summary>
        /// 
        /// <param name="api">
        /// The <see cref="OpenAIAPI"/> instance to use for making requests.
        /// </param>
        public MessagesEndpoint(OpenAIAPI api) : base(api) { }

        /// <inheritdoc />
        public async Task<MessageResult> CreateMessage(string threadId, MessageRequest request)
        {
            var url = $"{Url}/{threadId}/messages";
            
            return await HttpPost<MessageResult>(url, request);
        }

        /// <inheritdoc />
        public async Task<ResultsList<MessageResult>> ListMessages(string threadId, QueryParams queryParams = null)
        {
            queryParams ??= new QueryParams();
            
            var url = $"{Url}/{threadId}/messages{queryParams}";

            var content = await HttpGetContent(url);

            return JsonConvert.DeserializeObject<ResultsList<MessageResult>>(content);
        }

        /// <inheritdoc />
        public async Task<ResultsList<MessageFileResult>> ListMessageFiles(
            string threadId,
            string messageId,
            QueryParams queryParams = null
        )
        {
            queryParams ??= new QueryParams();

            var url = $"{Url}/{threadId}/messages/{messageId}/files{queryParams}";

            var content = await HttpGetContent(url);

            return JsonConvert.DeserializeObject<ResultsList<MessageFileResult>>(content);
        }

        /// <inheritdoc />
        public async Task<MessageResult> RetrieveMessage(string threadId, string messageId)
        {
            var url = $"{Url}/{threadId}/messages/{messageId}";

            return await HttpGet<MessageResult>(url);
        }

        /// <inheritdoc />
        public async Task<MessageFileResult> RetrieveMessageFile(string threadId, string messageId, string fileId)
        {
            var url = $"{Url}/{threadId}/messages/{messageId}/files/{fileId}";

            return await HttpGet<MessageFileResult>(url);
        }

        /// <inheritdoc />
        public async Task<MessageResult> ModifyMessage(string threadId, string messageId, MetadataRequest request)
        {
            var url = $"{Url}/{threadId}/messages/{messageId}";

            return await HttpPost<MessageResult>(url, request);
        }
    }
}