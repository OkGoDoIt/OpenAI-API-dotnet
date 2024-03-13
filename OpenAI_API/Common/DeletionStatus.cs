using Newtonsoft.Json;

namespace OpenAI_API.Common
{
    /// <summary>
    /// Represents the status of a deletion request.
    /// </summary>
    public class DeletionStatus : ApiResultBase
    {
        /// <summary>
        /// The ID of the object that was deleted.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The type of object that was deleted.
        /// </summary>
        [JsonProperty("object")]
        public new string Object { get; set; }

        /// <summary>
        /// Whether the object was deleted.
        /// </summary>
        [JsonProperty("deleted")]
        public bool Deleted { get; set; }
    }
}