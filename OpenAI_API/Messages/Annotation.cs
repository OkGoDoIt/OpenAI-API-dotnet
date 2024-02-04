using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace OpenAI_API.Messages
{
    /// <summary>
    /// Represents an annotation in a message content.
    /// </summary>
    public class Annotation
    {
        /// <summary>
        /// The type of annotation.
        /// </summary>
        [JsonProperty("type")]
        public AnnotationType Type { get; set; }

        /// <summary>
        /// The text in the message content that needs to be replaced.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// The file citation.
        /// </summary>
        /// <remarks>
        /// Only present if <see cref="Type"/> is <see cref="AnnotationType.FileCitation"/>.
        /// </remarks>
        [JsonProperty("file_annotation")]
        public FileCitation FileCitation { get; set; }

        /// <summary>
        /// The file path.
        /// </summary>
        /// <remarks>
        /// Only present if <see cref="Type"/> is <see cref="AnnotationType.FilePath"/>.
        /// </remarks>
        [JsonProperty("file_path")]
        public FilePath FilePath { get; set; }

        /// <summary>
        /// The start index.
        /// </summary>
        [JsonProperty("start_index")]
        public int StartIndex { get; set; }

        /// <summary>
        /// The end index.
        /// </summary>
        [JsonProperty("end_index")]
        public int EndIndex { get; set; }
    }

    /// <summary>
    /// Represents a citation to a file.
    /// </summary>
    public class FileCitation
    {
        /// <summary>
        /// The ID of the specified file the citation is from.
        /// </summary>
        [JsonProperty("file_id")]
        public string FileId { get; set; }

        /// <summary>
        /// The specific quote in the file.
        /// </summary>
        [JsonProperty("quote")]
        public string Quote { get; set; }
    }

    /// <summary>
    /// Represents a file path.
    /// </summary>
    public class FilePath
    {
        /// <summary>
        /// The ID of the file that was generated.
        /// </summary>
        [JsonProperty("file_id")]
        public string FileId { get; set; }
    }

    public enum AnnotationType
    {
        [EnumMember(Value = "file_citation")] FileCitation,
        [EnumMember(Value = "file_path")] FilePath
    }
}