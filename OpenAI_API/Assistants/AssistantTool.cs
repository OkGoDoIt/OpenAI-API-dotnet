using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace OpenAI_API.Assistants
{
    /// <summary>
    /// Represents a tool that an assistant can use.
    /// </summary>
    public class AssistantTool
    {
        /// <summary>
        /// The type of tool being defined.
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AssistantToolType Type { get; set; }

        /// <summary>
        /// The function of the tool being defined. Only present if <see cref="Type"/> is <see cref="AssistantToolType.Function"/>.
        /// </summary>
        [JsonProperty("function")]
        public AssistantToolFunction Function { get; set; }
    }

    /// <summary>
    /// Represents a function of a tool that an assistant can use.
    /// </summary>
    public class AssistantToolFunction
    {
        /// <summary>
        /// The name of the function to be called. Must be a-z, A-Z, 0-9, or contain underscores and dashes, with a
        /// minimum length of 64.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// A description of what the function does. Used by the model to choose when and how to call the function.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// The parameters the functions accepts, described as a JSON Schema object. See the guide for examples, and the
        /// JSON Schema reference for documentation about the format.
        /// </summary>
        /// <remarks>
        /// Omitting parameters defines a function with an empty parameter list.
        /// </remarks>
        [JsonProperty("parameters")]
        public JObject Parameters { get; set; }
    }

    /// <summary>
    /// Enumerates the types of assistant tools.
    /// </summary>
    public enum AssistantToolType
    {
        [EnumMember(Value = "code_interpreter")] CodeInterpreter,
        [EnumMember(Value = "retrieval")] Retrieval,
        [EnumMember(Value = "function")] Function
    }
}