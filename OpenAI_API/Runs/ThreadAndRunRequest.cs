using Newtonsoft.Json;
using OpenAI_API.Threads;

namespace OpenAI_API.Runs
{
    /// <summary>
    /// Represents a request to create a thread and run it.
    /// </summary>
    public class ThreadAndRunRequest : RunRequest
    {
        /// <summary>
        /// The thread to create.
        /// </summary>
        [JsonProperty("thread")]
        public ThreadRequest Thread { get; set; }
    }
}