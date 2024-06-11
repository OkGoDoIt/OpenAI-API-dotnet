using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenAI_API.Common;

namespace OpenAI_API.Assistants
{
    /// <summary>
    /// The endpoint for the Assistants API.
    /// </summary>
    public class AssistantsEndpoint : EndpointBase, IAssistantsEndpoint
    {
        /// <summary>
        /// The name of the endpoint, which is the final path segment in the API URL.
        /// </summary>
        protected override string Endpoint => "assistants";

        /// <summary>
        /// Constructs a new <see cref="AssistantsEndpoint"/>.
        /// </summary>
        /// 
        /// <param name="api">
        /// The <see cref="OpenAIAPI"/> instance to use for making requests.
        /// </param>
        public AssistantsEndpoint(OpenAIAPI api) : base(api) { }

        /// <inheritdoc />
        public async Task<AssistantResult> CreateAssistant(AssistantRequest request)
        {
            return await HttpPost<AssistantResult>(Url, request);
        }

        /// <inheritdoc />
        public async Task<AssistantFileResult> CreateAssistantFile(string assistantId, AssistantFileRequest request)
        {
            var url = $"{Url}/{assistantId}/files";

            return await HttpPost<AssistantFileResult>(url, request);
        }

        /// <inheritdoc />
        public async Task<ResultsList<AssistantResult>> ListAssistants(QueryParams queryParams = null)
        {
            queryParams ??= new QueryParams();

            var url = $"{Url}{queryParams}";

            var content = await HttpGetContent(url);

            return JsonConvert.DeserializeObject<ResultsList<AssistantResult>>(content);
        }

        /// <inheritdoc />
        public async Task<ResultsList<AssistantFileResult>> ListAssistantFiles(
            string assistantId,
            QueryParams queryParams = null
        )
        {
            queryParams ??= new QueryParams();

            var url = $"{Url}/{assistantId}/files{queryParams}";

            var content = await HttpGetContent(url);

            return JsonConvert.DeserializeObject<ResultsList<AssistantFileResult>>(content);
        }

        /// <inheritdoc />
        public Task<AssistantResult> RetrieveAssistant(string assistantId)
        {
            var url = $"{Url}/{assistantId}";

            return HttpGet<AssistantResult>(url);
        }

        /// <inheritdoc />
        public Task<AssistantFileResult> RetrieveAssistantFile(string assistantId, string fileId)
        {
            var url = $"{Url}/{assistantId}/files/{fileId}";

            return HttpGet<AssistantFileResult>(url);
        }

        /// <inheritdoc />
        public Task<AssistantResult> ModifyAssistant(string assistantId, AssistantRequest request)
        {
            var url = $"{Url}/{assistantId}";

            return HttpPost<AssistantResult>(url, request);
        }

        /// <inheritdoc />
        public Task<DeletionStatus> DeleteAssistant(string assistantId)
        {
            var url = $"{Url}/{assistantId}";

            return HttpDelete<DeletionStatus>(url);
        }

        /// <inheritdoc />
        public Task<DeletionStatus> DeleteAssistantFile(string assistantId, string fileId)
        {
            var url = $"{Url}/{assistantId}/files/{fileId}";

            return HttpDelete<DeletionStatus>(url);
        }
    }
}