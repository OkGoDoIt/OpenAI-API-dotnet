using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OpenAI_API.Runs
{
    /// <summary>
    /// Represents an error that occurred during a run.
    /// </summary>
    public class RunError
    {
        /// <summary>
        /// The error code.
        /// </summary>
        [JsonProperty("code")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RunErrorCode Code { get; set; }

        /// <summary>
        /// A human-readable description of the error.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    /// <summary>
    /// Enumerates the possible error codes.
    /// </summary>
    public enum RunErrorCode
    {
        [EnumMember(Value = "server_error")] ServerError,
        [EnumMember(Value = "rate_limit_exceeded")] RateLimitExceeded,
    }
}