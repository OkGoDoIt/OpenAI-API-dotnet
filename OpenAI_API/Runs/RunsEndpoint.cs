using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenAI_API.Common;

namespace OpenAI_API.Runs
{
    /// <summary>
    /// The endpoint for the Runs API.
    /// </summary>
    public class RunsEndpoint : EndpointBase, IRunsEndpoint
    {
        /// <summary>
        /// The name of the endpoint, which is the final path segment in the API URL.
        /// </summary>
        protected override string Endpoint => "threads";

        /// <summary>
        /// Constructs a new <see cref="RunsEndpoint"/>.
        /// </summary>
        /// 
        /// <param name="api">
        /// The <see cref="OpenAIAPI"/> instance to use for making requests.
        /// </param>
        public RunsEndpoint(OpenAIAPI api) : base(api) { }

        /// <inheritdoc />
        public async Task<RunResult> CreateRun(string threadId, RunRequest request)
        {
            var url = $"{Url}/{threadId}/runs";

            return await HttpPost<RunResult>(url, request);
        }

        /// <inheritdoc />
        public async Task<RunResult> CreateThreadAndRun(ThreadAndRunRequest request)
        {
            var url = $"{Url}/runs";

            return await HttpPost<RunResult>(url, request);
        }

        /// <inheritdoc />
        public async Task<ResultsList<RunResult>> ListRuns(string threadId, QueryParams queryParams = null)
        {
            queryParams ??= new QueryParams();

            var url = $"{Url}/{threadId}/runs{queryParams}";

            var content = await HttpGetContent(url);

            return JsonConvert.DeserializeObject<ResultsList<RunResult>>(content);
        }

        /// <inheritdoc />
        public async Task<ResultsList<RunStepResult>> ListRunSteps(
            string threadId,
            string runId,
            QueryParams queryParams = null
        )
        {
            queryParams ??= new QueryParams();

            var url = $"{Url}/{threadId}/runs/{runId}/steps{queryParams}";

            var content = await HttpGetContent(url);

            return JsonConvert.DeserializeObject<ResultsList<RunStepResult>>(content);
        }

        /// <inheritdoc />
        public async Task<RunResult> RetrieveRun(string threadId, string runId)
        {
            var url = $"{Url}/{threadId}/runs/{runId}";

            return await HttpGet<RunResult>(url);
        }

        /// <inheritdoc />
        public async Task<RunStepResult> RetrieveRunStep(string threadId, string runId, string stepId)
        {
            var url = $"{Url}/{threadId}/runs/{runId}/steps/{stepId}";

            return await HttpGet<RunStepResult>(url);
        }

        /// <inheritdoc />
        public async Task<RunResult> ModifyRun(string threadId, string runId, MetadataRequest request)
        {
            var url = $"{Url}/{threadId}/runs/{runId}";

            return await HttpPost<RunResult>(url, request);
        }

        /// <inheritdoc />
        public Task<RunResult> SubmitToolOutputsToRun(string threadId, string runId, ToolOutputsRequest request)
        {
            var url = $"{Url}/{threadId}/runs/{runId}/submit_tool_outputs";

            return HttpPost<RunResult>(url, request);
        }

        /// <inheritdoc />
        public async Task<RunResult> CancelRun(string threadId, string runId)
        {
            var url = $"{Url}/{threadId}/runs/{runId}/cancel";

            return await HttpPost<RunResult>(url);
        }
    }
}