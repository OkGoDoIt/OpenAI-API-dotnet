using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenAI_API.Assistants;

namespace OpenAI_API.Runs
{
    /// <summary>
    /// Represents a run step.
    /// </summary>
    public class RunStepResult : ApiResultBase
    {
        /// <summary>
        /// The identifier, which can be referenced in API endpoints.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The ID of the assistant associated with this run step.
        /// </summary>
        [JsonProperty("assistant_id")]
        public string AssistantId { get; set; }

        /// <summary>
        /// The ID of the thread that was run.
        /// </summary>
        [JsonProperty("thread_id")]
        public string ThreadId { get; set; }

        /// <summary>
        /// The ID of the run that this run step is part of.
        /// </summary>
        [JsonProperty("run_id")]
        public string RunId { get; set; }

        /// <summary>
        /// The type of the run step.
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RunStepType Type { get; set; }

        /// <summary>
        /// The status of the run step.
        /// </summary>
        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RunStepStatus Status { get; set; }

        /// <summary>
        /// The details of the run step.
        /// </summary>
        [JsonProperty("step_details")]
        public RunStepDetails StepDetails { get; set; }

        /// <summary>
        /// The last error associated with the run step. Will be <c>null</c> if there are no errors.
        /// </summary>
        [JsonProperty("last_error")]
        public RunError LastError { get; set; }

        #region Timestamps

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run step was created.
        /// </summary>
        [JsonProperty("created_at")]
        public long? CreatedAtUnixTime { get; set; }

        /// <summary>
        /// The timestamp for when the run step was created.
        /// </summary>
        [JsonIgnore]
        public DateTime? CreatedAt => ConvertUnixTime(CreatedAtUnixTime);

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run step expired.
        /// </summary>
        [JsonProperty("expired_at")]
        public long? ExpiredAtUnixTime { get; set; }

        /// <summary>
        /// The timestamp for when the run step expired.
        /// </summary>
        [JsonIgnore]
        public DateTime? ExpiredAt => ConvertUnixTime(ExpiredAtUnixTime);

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run step was cancelled.
        /// </summary>
        [JsonProperty("cancelled_at")]
        public long? CancelledAtUnixTime { get; set; }

        /// <summary>
        /// The timestamp for when the run step was cancelled.
        /// </summary>
        [JsonIgnore]
        public DateTime? CancelledAt => ConvertUnixTime(CancelledAtUnixTime);

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run step failed.
        /// </summary>
        [JsonProperty("failed_at")]
        public long? FailedAtUnixTime { get; set; }

        /// <summary>
        /// The timestamp for when the run step failed.
        /// </summary>
        [JsonIgnore]
        public DateTime? FailedAt => ConvertUnixTime(FailedAtUnixTime);

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run step was completed.
        /// </summary>
        [JsonProperty("completed_at")]
        public long? CompletedAtUnixTime { get; set; }

        /// <summary>
        /// The timestamp for when the run step was completed.
        /// </summary>
        [JsonIgnore]
        public DateTime? CompletedAt => ConvertUnixTime(CompletedAtUnixTime);

        #endregion

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
    /// Represents the details of a run step.
    /// </summary>
    public class RunStepDetails
    {
        /// <summary>
        /// The type of step details.
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RunStepDetailsType Type { get; set; }

        /// <summary>
        /// Details of the message creation step. Only present if <see cref="Type"/> is <see cref="RunStepType.MessageCreation"/>.
        /// </summary>
        [JsonProperty("message_creation")]
        public RunStepMessageCreation MessageCreation { get; set; }

        /// <summary>
        /// An array of tool calls the run step was involved in. Only present if <see cref="Type"/> is <see cref="RunStepType.ToolCalls"/>.
        /// </summary>
        [JsonProperty("tool_calls")]
        public IReadOnlyList<RunStepToolCall> ToolCalls { get; set; }
    }

    /// <summary>
    /// Represents the creation of a message by a run step.
    /// </summary>
    public class RunStepMessageCreation
    {
        /// <summary>
        /// The ID of the message that was created by this run step.
        /// </summary>
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }

    /// <summary>
    /// Represents a tool call by a run step.
    /// </summary>
    public class RunStepToolCall
    {
        /// <summary>
        /// The ID of the tool call.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        
        /// <summary>
        /// The type of tool call.
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AssistantToolType Type { get; set; }
        
        /// <summary>
        /// The code interpreter tool call definition. Only present if <see cref="Type"/> is <see cref="AssistantToolType.CodeInterpreter"/>.
        /// </summary>
        [JsonProperty("code_interpreter")]
        public object CodeInterpreter { get; set; }
        
        /// <summary>
        /// The retrieval tool call definition. For now this is going to be an empty object.
        /// </summary>
        [JsonProperty("retrieval")]
        public object Retrieval { get; set; }
        
        /// <summary>
        /// The definition of the function that was called. Only present if <see cref="Type"/> is <see cref="AssistantToolType.Function"/>.
        /// </summary>
        [JsonProperty("function")]
        public object Function { get; set; }
    }

    #region Enums

    /// <summary>
    /// Enumerates the possible types of a run step.
    /// </summary>
    public enum RunStepType
    {
        [EnumMember(Value = "message_creation")] MessageCreation,
        [EnumMember(Value = "tool_calls")] ToolCalls
    }

    /// <summary>
    /// Enumerates the possible statuses of a run step.
    /// </summary>
    public enum RunStepStatus
    {
        [EnumMember(Value = "in_progress")] InProgress,
        [EnumMember(Value = "cancelled")] Cancelled,
        [EnumMember(Value = "failed")] Failed,
        [EnumMember(Value = "completed")] Completed,
        [EnumMember(Value = "expired")] Expired
    }

    /// <summary>
    /// Enumerates the possible types of step details.
    /// </summary>
    public enum RunStepDetailsType
    {
        [EnumMember(Value = "message_creation")] MessageCreation,
        [EnumMember(Value = "tool_calls")] ToolCalls
    }

    #endregion
}