using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenAI_API.Assistants
{
    /// <summary>
    /// Represents an assistant that can call the model and use tools.
    /// </summary>
    public class AssistantResult : ApiResultBase
    {
        /// <summary>
        /// The identifier, which can be referenced in API endpoints.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the assistant was created.
        /// </summary>
        [JsonProperty("created_at")]
        public long? CreatedAtUnixTime { get; set; }

        /// <summary>
        /// The timestamp for when the assistant was created.
        /// </summary>
        [JsonIgnore]
        public DateTime? CreatedAt => ConvertUnixTime(CreatedAtUnixTime);

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
        /// A list of tools enabled on the assistant. There can be a maximum of 128 tools per assistant. Tools can be of
        /// types <c>code_interpreter</c>, <c>retrieval</c> or <c>function</c>.
        /// </summary>
        [JsonProperty("tools")]
        public IReadOnlyList<AssistantTool> Tools { get; set; }

        /// <summary>
        /// A list of file IDs attached to this assistant. There can be a maximum of 20 files attached to an assistant.
        /// Files are ordered by their creation date in ascending order.
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
    }
}