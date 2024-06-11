using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OpenAI_API.Common;

namespace OpenAI_API.Assistants
{
    /// <summary>
    /// Represents a request to create an assistant.
    /// </summary>
    public class AssistantRequest : MetadataRequest
    {
        /// <summary>
        /// The ID of the model to use.
        /// </summary>
        [JsonProperty("model")]
        public string Model { get; set; }

        /// <summary>
        /// The name of the assistant. The maximum length is 256 characters.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The description of the assistant. The maximum length is 512 characters.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// The system instructions that the assistant uses. The maximum length is 32768 characters.
        /// </summary>
        [JsonProperty("instructions")]
        public string Instructions { get; set; }

        /// <summary>
        /// A list of tools enabled on the assistant. There can be a maximum of 128 tools per assistant. Tools can be
        /// be of types <b>code_interpreter</b>, <b>retrieval</b> or <b>function</b>.
        /// </summary>
        [JsonProperty("tools")]
        public IList<AssistantTool> Tools { get; set; } = Array.Empty<AssistantTool>();

        /// <summary>
        /// A list of file IDs attached to the assistant. This can be useful for storing additional information about
        /// the object in a structured format. Keys can be a maximum of 64 characters long and values can be a maximum
        /// of 512 characters long.
        /// </summary>
        [JsonProperty("file_ids")]
        public IList<string> FileIds { get; set; } = Array.Empty<string>();
    }
}