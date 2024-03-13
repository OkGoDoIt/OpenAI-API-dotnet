using System;
using Newtonsoft.Json;

namespace OpenAI_API.Messages
{
    /// <summary>
    /// Represents a message file attached to a message.
    /// </summary>
    public class MessageFileResult : ApiResultBase
    {
        /// <summary>
        /// The identifier, which can be referenced in API endpoints. 
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the message file was created.
        /// </summary>
        [JsonProperty("created_at")]
        public long? CreatedAtUnixTime { get; set; }

        /// <summary>
        /// The timestamp for when the message file was created.
        /// </summary>
        [JsonIgnore]
        public DateTime? CreatedAt => ConvertUnixTime(CreatedAtUnixTime);
        
        /// <summary>
        /// The message ID this file belongs to.
        /// </summary>
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }
}