using System.Collections.Generic;
using Newtonsoft.Json;
using OpenAI_API.Assistants;
using OpenAI_API.Common;

namespace OpenAI_API.Runs
{
    /// <summary>
    /// Represents a request to create a run.
    /// </summary>
    public class RunRequest : MetadataRequest
    {
        /// <summary>
        /// The ID of the assistant to execute this run.
        /// </summary>
        [JsonProperty("assistant_id")]
        public string AssistantId { get; set; }

        /// <summary>
        /// The ID of the Model to be used to execute this run. If a value is provided here, it will override the
        /// model associated with the assistant. If not, the model associated with the assistant will be used.
        /// </summary>
        [JsonProperty("model")]
        public string Model { get; set; }

        /// <summary>
        /// Overrides the instructions of the assistant. This is useful for modifying the behavior on a per-run basis.
        /// </summary>
        [JsonProperty("instructions")]
        public string Instructions { get; set; }

        /// <summary>
        /// Appends additional instructions at the end of the of the instructions for the run. This is useful for
        /// modifying the behavior on a per-run basis, without overriding other instructions.
        /// </summary>
        [JsonProperty("additional_instructions")]
        public string AdditionalInstructions { get; set; }

        /// <summary>
        /// Override the tools the assistant can use for this run. This is useful for modifying the behavior on a
        /// per-run basis.
        /// </summary>
        [JsonProperty("tools")]
        public IList<AssistantTool> Tools { get; set; }
    }
}