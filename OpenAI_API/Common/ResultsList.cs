using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenAI_API.Common
{
    /// <summary>
    /// Represents a list of results.
    /// </summary>
    public class ResultsList<TResult> where TResult : ApiResultBase
    {
        /// <summary>
        /// The object type. Always <c>"list"</c>.
        /// </summary>
        [JsonProperty("object")]
        public string Object { get; set; }

        /// <summary>
        /// The list of results.
        /// </summary>
        [JsonProperty("data")]
        public IReadOnlyList<TResult> Data { get; set; }

        /// <summary>
        /// The ID of the first result in the list.
        /// </summary>
        [JsonProperty("first_id")]
        public string FirstId { get; set; }

        /// <summary>
        /// The ID of the last result in the list.
        /// </summary>
        [JsonProperty("last_id")]
        public string LastId { get; set; }

        /// <summary>
        /// Whether there are more results available.
        /// </summary>
        [JsonProperty("has_more")]
        public bool HasMore { get; set; }
    }
}