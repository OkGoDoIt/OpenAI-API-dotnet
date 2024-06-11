using System.Threading.Tasks;
using OpenAI_API.Common;

namespace OpenAI_API.Runs
{
    /// <summary>
    /// An interface for the Runs API endpoint.
    /// </summary>
    public interface IRunsEndpoint
    {
        /// <summary>
        /// Creates a run.
        /// </summary>
        /// 
        /// <param name="threadId">
        /// The ID of the thread in which to create the run.
        /// </param>
        /// <param name="request">
        /// The request object.
        /// </param>
        /// 
        /// <returns>
        /// The created run object.
        /// </returns>
        public Task<RunResult> CreateRun(string threadId, RunRequest request);

        /// <summary>
        /// Creates a thread and runs it.
        /// </summary>
        /// 
        /// <param name="request">
        /// The request object.
        /// </param>
        /// 
        /// <returns>
        /// The created run object.
        /// </returns>
        public Task<RunResult> CreateThreadAndRun(ThreadAndRunRequest request);

        /// <summary>
        /// Lists runs belonging to a thread.
        /// </summary>
        /// 
        /// <param name="threadId">
        /// The ID of the thread to which the runs belong to.
        /// </param>
        /// <param name="queryParams">
        /// The query parameters to use for the request. If unspecified, the default parameters are used.
        /// </param>
        /// 
        /// <returns>
        /// A list of runs objects belonging to the thread with the specified ID.
        /// </returns>
        public Task<ResultsList<RunResult>> ListRuns(string threadId, QueryParams queryParams = null);

        /// <summary>
        /// Lists run steps belonging to a run.
        /// </summary>
        /// 
        /// <param name="threadId">
        /// The ID of the thread to which the run and run steps belong to.
        /// </param>
        /// <param name="runId">
        /// The ID of the run the run steps belong to.
        /// </param>
        /// <param name="queryParams">
        /// The query parameters to use for the request. If unspecified, the default parameters are used.
        /// </param>
        /// 
        /// <returns>
        /// A list of run step objects belonging to the run with the specified ID.
        /// </returns>
        public Task<ResultsList<RunStepResult>> ListRunSteps(
            string threadId,
            string runId,
            QueryParams queryParams = null
        );

        /// <summary>
        /// Retrieves a run.
        /// </summary>
        /// 
        /// <param name="threadId">
        /// The ID of the thread to which the run belongs to.
        /// </param>
        /// <param name="runId">
        /// The ID of the run to retrieve.
        /// </param>
        /// 
        /// <returns>
        /// The run object matching the specified ID.
        /// </returns>
        public Task<RunResult> RetrieveRun(string threadId, string runId);

        /// <summary>
        /// Retrieves a run step.
        /// </summary>
        /// 
        /// <param name="threadId">
        /// The ID of the thread to which the run and run step belong to.
        /// </param>
        /// <param name="runId">
        /// The ID of the run to which the run step belongs to.
        /// </param>
        /// <param name="stepId">
        /// The ID of the run step to retrieve.
        /// </param>
        /// 
        /// <returns>
        /// The run step object matching the specified ID.
        /// </returns>
        public Task<RunStepResult> RetrieveRunStep(string threadId, string runId, string stepId);

        /// <summary>
        /// Modifies a run.
        /// </summary>
        /// 
        /// <param name="threadId">
        /// The ID of the thread to which the run belongs to.
        /// </param>
        /// <param name="runId">
        /// The ID of the run to modify.
        /// </param>
        /// <param name="request">
        /// The request object.
        /// </param>
        /// 
        /// <returns>
        /// The modified run object matching the specified ID.
        /// </returns>
        public Task<RunResult> ModifyRun(string threadId, string runId, MetadataRequest request);

        /// <summary>
        /// When a run has the <c>status: "requires_action"</c> and <c>required_action.type</c> is submit_tool_outputs,
        /// this endpoint can be used to submit the outputs from the tool calls once they're all completed. All outputs
        /// must be submitted in a single request.
        /// </summary>
        /// 
        /// <param name="threadId">
        /// The ID of the thread to which the run belongs to.
        /// </param>
        /// <param name="runId">
        /// The ID of the run that requires the tool output submission.
        /// </param>
        /// <param name="request">
        /// The request object.
        /// </param>
        /// 
        /// <returns>
        /// The modified run object matching the specified ID.
        /// </returns>
        public Task<RunResult> SubmitToolOutputsToRun(string threadId, string runId, ToolOutputsRequest request);

        /// <summary>
        /// Cancels a run.
        /// </summary>
        /// 
        /// <param name="threadId">
        /// The ID of the thread to which the run belongs to.
        /// </param>
        /// <param name="runId">
        /// The ID of the run to cancel.
        /// </param>
        /// 
        /// <returns>
        /// The cancelled run object matching the specified ID.
        /// </returns>
        public Task<RunResult> CancelRun(string threadId, string runId);
    }
}