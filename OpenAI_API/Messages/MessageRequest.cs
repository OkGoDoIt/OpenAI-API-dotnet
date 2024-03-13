using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenAI_API.Common;

namespace OpenAI_API.Messages
{
    /// <summary>
    /// Represents a request to create a message.
    /// </summary>
    public class MessageRequest : MetadataRequest
    {

        /// <summary>
        /// The role of the message. Only <see cref="MessageRole.User"/> is currently supported.
        /// </summary>
        [JsonProperty("role")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageRole Role { get; set; }

        /// <summary>
        /// The content of the message
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// A list of File IDs that the message should use.
        /// </summary>
        [JsonProperty("file_ids")]
        public IList<string> FileIds { get; set; } = Array.Empty<string>();
    }
}