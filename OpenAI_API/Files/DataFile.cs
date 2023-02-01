using Newtonsoft.Json;

namespace OpenAI_API
{
    /// <summary>
    /// Information on an individual file
    /// </summary>
    public class DataFile
    {
        /// <summary>
        /// ID of the file to identify when using in creating fine-tunes
        /// </summary>
        [JsonProperty("id")]
        public string FileId { get; set; }

        /// <summary>
        /// Object refers to the datum type
        /// Usually "file" in this usage
        /// </summary>
        [JsonProperty("object")]
        public string Object { get; set; } = "file";

        /// <summary>
        /// Size of the file
        /// </summary>
        [JsonProperty("bytes")]
        public long Bytes { get; set; }

        /// <summary>
        /// Unix timestamp for creation date/time
        /// </summary>
        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }

        /// <summary>
        /// Name of file (not to be confused with <see cref="FileId"/>)
        /// </summary>
        [JsonProperty("filename")]
        public string Filename { get; set; }

        /// <summary>
        /// Purpose of the file
        /// Usually "fine-tune"
        /// </summary>
        [JsonProperty("purpose")]
        public string Purpose { get; set; } = "fine-tune";

        /// <summary>
        /// Used with <see cref="FilesDeleteRequest"/>
        /// </summary>
        [JsonProperty("deleted")]
        public bool Deleted { get; set; }
    }
}
