using System.Collections.Generic;
using Newtonsoft.Json;
using OpenAI_API.Common;
using OpenAI_API.Messages;

namespace OpenAI_API.Threads
{
    /// <summary>
    /// Represents a request to create a thread.
    /// </summary>
    public class ThreadRequest : MetadataRequest
    {
        /// <summary>
        /// A list of messages to start the thread with.
        /// </summary>
        [JsonProperty("messages")]
        public IList<MessageRequest> Messages { get; set; }
    }
}