using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenAI_API.Threads
{
    /// <summary>
    /// Represents a thread that contains messages.
    /// </summary>
    public class ThreadResult : ApiResultBase
    {
        /// <summary>
        /// The identifier which can be referenced in API endpoints.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the thread was created.
        /// </summary>
        [JsonProperty("created_at")]
        public long? CreatedAtUnixTime { get; set; }

        /// <summary>
        /// The timestamp for when the thread was created.
        /// </summary>
        [JsonIgnore]
        public DateTime? CreatedAt => ConvertUnixTime(CreatedAtUnixTime);

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object. This can be useful for storing additional
        /// information about the object in a structured format. Keys can be a maximum of 64 characters long and values
        /// can be a maximum of 512 characters long.
        /// </summary>
        [JsonProperty("metadata")]
        public IReadOnlyDictionary<string, string> Metadata { get; set; }
    }
}