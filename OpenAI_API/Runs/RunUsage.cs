using Newtonsoft.Json;

namespace OpenAI_API.Runs
{
    /// <summary>
    /// Represents the usage statistics related to a run.
    /// </summary>
    public class RunUsage
    {
        /// <summary>
        /// Number of completion tokens used over the course of the run.
        /// </summary>
        [JsonProperty("completion_tokens")]
        public int CompletionTokens { get; set; }

        /// <summary>
        /// Number of prompt tokens used over the course of the run.
        /// </summary>
        [JsonProperty("prompt_tokens")]
        public int PromptTokens { get; set; }

        /// <summary>
        /// Number of tokens used (prompt + completion) over the course of the run.
        /// </summary>
        [JsonProperty("total_tokens")]
        public int TotalTokens { get; set; }
    }
}