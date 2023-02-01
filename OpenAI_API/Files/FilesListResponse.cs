using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenAI_API
{
    /// <summary>
    /// Response from OpenAI API for List Files functionality
    /// Returns a list of files that belong to the user's organization.
    /// https://platform.openai.com/docs/api-reference/files/list
    /// </summary>
    public class FilesListResponse
    {
        /// <summary>
        /// Array including the list of files
        /// </summary>
        [JsonProperty("data")]
        public List<DataFile> Files{ get; set; } = new List<DataFile>();

        /// <summary>
        /// Description of the type of information included in <see cref="DataFile"/>
        /// Usually "list" in this usage
        /// </summary>
        [JsonProperty("object")]
        public string Object { get; set; } = "list";

    }

}
