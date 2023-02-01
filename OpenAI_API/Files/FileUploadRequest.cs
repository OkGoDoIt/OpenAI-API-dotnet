using Newtonsoft.Json;

namespace OpenAI_API
{
    /// <summary>
    /// API request class to upload a file
    /// https://platform.openai.com/docs/api-reference/files/upload
    /// Returns <see cref="DataFile"/>
    /// </summary>
    public class FileUploadRequest
    {
        /// <summary>
        /// Name of the JSON Lines file to be uploaded. (https://jsonlines.readthedocs.io/en/latest/)
        /// If the purpose is set to "fine-tune", each line is a JSON record with "prompt" and "completion" fields representing your training examples.
        /// https://platform.openai.com/docs/api-reference/files/upload#files/upload-file
        /// Info on preparing training examples --> https://platform.openai.com/docs/guides/fine-tuning/prepare-training-data
        /// </summary>
        [JsonProperty("file")]
        public string Filename { get; set; }

        /// <summary>
        /// The intended purpose of the uploaded documents.
        /// Use "fine-tune" for Fine-tuning. This allows us to validate the format of the uploaded file.
        /// https://platform.openai.com/docs/api-reference/files/upload#files/upload-purpose
        /// More on fine-tuning here --> https://platform.openai.com/docs/api-reference/fine-tunes
        /// </summary>
        [JsonProperty("purpose")]
        public string Purpose { get; set; } = "fine-tune";

    }
}
