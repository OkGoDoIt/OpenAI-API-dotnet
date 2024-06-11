using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OpenAI_API.Messages
{
    /// <summary>
    /// Represents a message.
    /// </summary>
    public class MessageResult : ApiResultBase
    {
        /// <summary>
        /// The identifier, which can be referenced in API endpoints. 
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the message was created.
        /// </summary>
        [JsonProperty("created_at")]
        public long? CreatedAtUnixTime { get; set; }

        /// <summary>
        /// The timestamp for when the message was created.
        /// </summary>
        [JsonIgnore]
        public DateTime? CreatedAt => ConvertUnixTime(CreatedAtUnixTime);

        /// <summary>
        /// The thread ID this message belongs to.
        /// </summary>
        [JsonProperty("thread_id")]
        public string ThreadId { get; set; }

        /// <summary>
        /// The entity that produced the message.
        /// </summary>
        [JsonProperty("role")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageRole Role { get; set; }

        /// <summary>
        /// The content of the message in an array of text and/or images.
        /// </summary>
        [JsonProperty("content")]
        public IReadOnlyList<Content> Content { get; set; }

        /// <summary>
        /// If applicable, the ID of the assistant that authored the message.
        /// </summary>
        [JsonProperty("assistant_id")]
        public string AssistantId { get; set; }

        /// <summary>
        /// If applicable, the ID of the run associated with the authoring of this message.
        /// </summary>
        [JsonProperty("run_id")]
        public string RunId { get; set; }

        /// <summary>
        /// A list of file IDs that the assistant could use.
        /// </summary>
        [JsonProperty("file_ids")]
        public IReadOnlyList<string> FileIds { get; set; }

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object. This can be useful for storing additional
        /// information about the object in a structured format. Keys can be a maximum of 64 characters long and values
        /// can be a maximum of 512 characters long.
        /// </summary>
        [JsonProperty("metadata")]
        public IReadOnlyDictionary<string, string> Metadata { get; set; }
    }

    /// <summary>
    /// Represents the content of a message.
    /// </summary>
    public class Content
    {
        /// <summary>
        /// The type of content.
        /// </summary>
        [JsonProperty("type")]
        public MessageContentType Type { get; set; }

        /// <summary>
        /// The text content.
        /// </summary>
        /// <remarks>
        /// Only present if <see cref="Type"/> is <see cref="MessageContentType.Text"/>.
        /// </remarks>
        [JsonProperty("text")]
        public Text Text { get; set; }

        /// <summary>
        /// References an image file in the content of a message.
        /// </summary>
        /// <remarks>
        /// Only present if <see cref="Type"/> is <see cref="MessageContentType.ImageFile"/>.
        /// </remarks>
        [JsonProperty("image_file")]
        public ImageFile ImageFile { get; set; }
    }

    /// <summary>
    /// Represents a text object.
    /// </summary>
    public class Text
    {
        /// <summary>
        /// The value of the text.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// A list of annotations for the text.
        /// </summary>
        [JsonProperty("annotations")]
        public IReadOnlyList<Annotation> Annotations { get; set; }
    }

    /// <summary>
    /// Represents the reference to an image.
    /// </summary>
    public class ImageFile
    {
        /// <summary>
        /// The file ID of the image in the message content.
        /// </summary>
        [JsonProperty("file_id")]
        public string FileId { get; set; }
    }

    public enum MessageContentType
    {
        [EnumMember(Value = "test")] Text,
        [EnumMember(Value = "image_file")] ImageFile
    }
}