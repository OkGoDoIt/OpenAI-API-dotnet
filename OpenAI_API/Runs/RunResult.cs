using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenAI_API.Assistants;

namespace OpenAI_API.Runs
{
    /// <summary>
    /// Represents a run.
    /// </summary>
    public class RunResult : ApiResultBase
    {
        /// <summary>
        /// The identifier, which can be referenced in API endpoints.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The ID of the thread that was executed as part of this run.
        /// </summary>
        [JsonProperty("thread_id")]
        public string ThreadId { get; set; }

        /// <summary>
        /// The ID of the assistant used for execution of this run.
        /// </summary>
        [JsonProperty("assistant_id")]
        public string AssistantId { get; set; }

        /// <summary>
        /// The status of the run.
        /// </summary>
        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RunStatus Status { get; set; }

        /// <summary>
        /// Details on the action required to continue the run. Will be <c>null</c> if no action is required.
        /// </summary>
        [JsonProperty("required_action")]
        public RequiredAction RequiredAction { get; set; }

        /// <summary>
        /// The last error associated with the run step. Will be <c>null</c> if there are no errors.
        /// </summary>
        [JsonProperty("last_error")]
        public RunError LastError { get; set; }

        #region Timestamps

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run was created.
        /// </summary>
        [JsonProperty("created_at")]
        public long? CreatedAtUnixTime { get; set; }

        /// <summary>
        /// The timestamp for when the run was created.
        /// </summary>
        [JsonIgnore]
        public DateTime? CreatedAt => ConvertUnixTime(CreatedAtUnixTime);

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run will expire.
        /// </summary>
        [JsonProperty("expires_at")]
        public long? ExpiresAtUnixTime { get; set; }

        /// <summary>
        /// The timestamp for when the run will expire.
        /// </summary>
        [JsonIgnore]
        public DateTime? ExpiresAt => ConvertUnixTime(ExpiresAtUnixTime);

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run was started.
        /// </summary>
        [JsonProperty("started_at")]
        public long? StartedAtUnixTime { get; set; }

        /// <summary>
        /// The timestamp for when the run was started.
        /// </summary>
        [JsonIgnore]
        public DateTime? StartedAt => ConvertUnixTime(StartedAtUnixTime);

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run was cancelled.
        /// </summary>
        [JsonProperty("cancelled_at")]
        public long? CancelledAtUnixTime { get; set; }

        /// <summary>
        /// The timestamp for when the run was cancelled.
        /// </summary>
        [JsonIgnore]
        public DateTime? CancelledAt => ConvertUnixTime(CancelledAtUnixTime);

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run failed.
        /// </summary>
        [JsonProperty("failed_at")]
        public long? FailedAtUnixTime { get; set; }

        /// <summary>
        /// The timestamp for when the run failed.
        /// </summary>
        [JsonIgnore]
        public DateTime? FailedAt => ConvertUnixTime(FailedAtUnixTime);

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run was completed.
        /// </summary>
        [JsonProperty("completed_at")]
        public long? CompletedAtUnixTime { get; set; }

        /// <summary>
        /// The timestamp for when the run was completed.
        /// </summary>
        [JsonIgnore]
        public DateTime? CompletedAt => ConvertUnixTime(CompletedAtUnixTime);

        #endregion

        /// <summary>
        /// The model that the <b>assistant</b> used for this run.
        /// </summary>
        [JsonProperty("model")]
        public new string Model { get; set; }

        /// <summary>
        /// The instructions that the <b>assistant</b> used for this run.
        /// </summary>
        [JsonProperty("instructions")]
        public string Instructions { get; set; }

        /// <summary>
        /// The list of tools that the <b>assistant</b> used for this run.
        /// </summary>
        [JsonProperty("tools")]
        public IReadOnlyList<AssistantTool> Tools { get; set; }

        /// <summary>
        /// The list of file IDs the <b>assistant</b> used for this run.
        /// </summary>
        [JsonProperty("file_ids")]
        public IReadOnlyList<string> FileIds { get; set; }

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object. This can be useful for storing additional
        /// information about the object in a structured format. Keys can be a maximum of 64 characters long and values
        /// can be a maximum of 512 characters long.
        /// </summary>
        [JsonProperty("metadata")]
        public IReadOnlyDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// Usage statistics related to the run. Will be <c>null</c> if the run is not in a terminal state.
        /// </summary>
        [JsonProperty("usage")]
        public RunUsage Usage { get; set; }
    }

    /// <summary>
    /// Represents the details of the action required to continue a run.
    /// </summary>
    public class RequiredAction
    {
        /// <summary>
        /// For now, this is always <c>submit_tool_outputs</c>.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Details on the tool outputs needed for this run to continue.
        /// </summary>
        [JsonProperty("submit_tool_outputs")]
        public SubmitToolOutputs SubmitToolOutputs { get; set; }
    }

    /// <summary>
    /// Represents the tool outputs needed for a run to continue.
    /// </summary>
    public class SubmitToolOutputs
    {
        /// <summary>
        /// A list of the relevant tool calls.
        /// </summary>
        [JsonProperty("tool_calls")]
        public IReadOnlyList<ToolCall> ToolCalls { get; set; }
    }

    /// <summary>
    /// Represents a tool call.
    /// </summary>
    public class ToolCall
    {
        /// <summary>
        /// The ID of the tool call. This ID must be referenced when you submit the tool outputs in using the
        /// <see cref="IRunsEndpoint.SubmitToolOutputsToRun"/> endpoint.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The type of tool call the output is required for. For now, this is always <c>function</c>.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// The function definition.
        /// </summary>
        [JsonProperty("function")]
        public FunctionDefinition Function { get; set; }
    }

    /// <summary>
    /// Represents a function definition.
    /// </summary>
    public class FunctionDefinition
    {
        /// <summary>
        /// The name of the function.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The arguments that the model expects you to pass to the function.
        /// </summary>
        [JsonProperty("arguments")]
        public IReadOnlyList<string> Arguments { get; set; }
    }
}