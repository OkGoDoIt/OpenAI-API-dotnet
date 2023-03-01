using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Chat
{
	/// <summary>
	/// Represents a result from calling the Chat API
	/// </summary>
	public class ChatResult : ApiResultBase
    {
        /// <summary>
        /// The identifier of the result, which may be used during troubleshooting
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The list of choices that the user was presented with during the chat interaction
        /// </summary>
        [JsonProperty("choices")]
        public List<ChatChoice> Choices { get; set; }

        /// <summary>
        /// The usage statistics for the chat interaction
        /// </summary>
        [JsonProperty("usage")]
        public ChatUsage Usage { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ChatChoice
    {
        /// <summary>
        /// The index of the choice in the list of choices
        /// </summary>
        [JsonProperty("index")]
        public int Index { get; set; }

        /// <summary>
        /// The message that was presented to the user as the choice
        /// </summary>
        [JsonProperty("message")]
        public ChatMessage Message { get; set; }

        /// <summary>
        /// The reason why the chat interaction ended after this choice was presented to the user
        /// </summary>
        [JsonProperty("finish_reason")]
        public string FinishReason { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ChatMessage
    {
        /// <summary>
        /// Constructor for a new Chat Message
        /// </summary>
        /// <param name="role">The role of the message, which can be "system", "assistant" or "user"</param>
        /// <param name="content">The text to send in the message</param>
        public ChatMessage(string role, string content)
        {
            this.Role = role;
            this.Content = content;
        }

        /// <summary>
        /// The role of the message, which can be "system", "assistant" or "user"
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }

        /// <summary>
        /// The content of the message
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ChatUsage
    {
        /// <summary>
        /// The number of prompt tokens used during the chat interaction
        /// </summary>
        [JsonProperty("prompt_tokens")]
        public int PromptTokens { get; set; }

        /// <summary>
        /// The number of completion tokens used during the chat interaction
        /// </summary>
        [JsonProperty("completion_tokens")]
        public int CompletionTokens { get; set; }

        /// <summary>
        /// The total number of tokens used during the chat interaction
        /// </summary>
        [JsonProperty("total_tokens")]
        public int TotalTokens { get; set; }
    }
}
