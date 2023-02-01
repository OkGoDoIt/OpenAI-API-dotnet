using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API
{
    /// <summary>
    /// API request class to delete a file
    /// https://platform.openai.com/docs/api-reference/files/delete
    /// Returns <see cref="DataFile"/>
    /// </summary>
    public class FileDeleteRequest
    {
        /// <summary>
        /// ID of the file to delete
        /// https://platform.openai.com/docs/api-reference/files/delete#files/delete-file_id
        /// </summary>
        [JsonProperty("file_id")]
        public string FileId { get; set; }

    }

}
