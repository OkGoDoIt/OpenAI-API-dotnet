using System.Runtime.Serialization;

namespace OpenAI_API.Messages
{
    /// <summary>
    /// Enumerates roles that a message can have.
    /// </summary>
    public enum MessageRole
    {
        /// <summary>
        /// The message was created by the user.
        /// </summary>
        [EnumMember(Value = "user")] User,

        /// <summary>
        /// The message was created by the assistant.
        /// </summary>
        [EnumMember(Value = "assistant")] Assistant
    }
}