using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenAI_API.Runs
{
    /// <summary>
    /// Represents a request to submit the outputs of a tool call to continue a run.
    /// </summary>
    public class ToolOutputsRequest
    {
        /// <summary>
        /// A list of tools for which the outputs are being submitted.
        /// </summary>
        [JsonProperty("tool_outputs")]
        public IList<ToolOutput> ToolOutputs { get; set; }
    }

    /// <summary>
    /// Represents the output of a tool call to be submitted to continue a run.
    /// </summary>
    public class ToolOutput
    {
        /// <summary>
        /// The ID of the tool call in the <c>required_action</c> object within the run object the output is being
        /// submitted for.
        /// </summary>
        [JsonProperty("tool_call_id")]
        public string ToolCallId { get; set; }

        /// <summary>
        /// The output of the tool call to be submitted to continue the run.
        /// </summary>
        [JsonProperty("output")]
        public string Output { get; set; }
    }
}