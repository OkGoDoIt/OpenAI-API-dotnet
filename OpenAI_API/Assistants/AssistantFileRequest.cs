using Newtonsoft.Json;

namespace OpenAI_API.Assistants
{
    /// <summary>
    /// Represents a request to create an assistant file.
    /// </summary>
    public class AssistantFileRequest
    {
        /// <summary>
        /// A file ID (with <c>purpose="assistants"</c>) that the assistant should use. Useful for tools like
        /// <c>retrieval</c> and <c>code_interpreter</c> that can access files.
        /// </summary>
        [JsonProperty("file_id")]
        public string FileId { get; set; }
    }
}