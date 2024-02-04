using System;
using Newtonsoft.Json;

namespace OpenAI_API.Assistants
{
    /// <summary>
    /// Represents a file that is attached to an assistant.
    /// </summary>
    public class AssistantFileResult : ApiResultBase
    {
        /// <summary>
        /// The identifier, which can be referenced in API endpoints.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the assistant file was created.
        /// </summary>
        [JsonProperty("created_at")]
        public long? CreatedAtUnixTime { get; set; }

        /// <summary>
        /// The timestamp for when the assistant file was created.
        /// </summary>
        [JsonIgnore]
        public DateTime? CreatedAt => ConvertUnixTime(CreatedAtUnixTime);

        /// <summary>
        /// The ID of the assistant this file is attached to.
        /// </summary>
        [JsonProperty("assistant_id")]
        public string AssistantId { get; set; }
    }
}