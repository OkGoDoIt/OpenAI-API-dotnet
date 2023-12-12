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
		public IReadOnlyList<ChatChoice> Choices { get; set; }

		/// <summary>
		/// The usage statistics for the chat interaction
		/// </summary>
		[JsonProperty("usage")]
		public ChatUsage Usage { get; set; }

		/// <summary>
		/// A convenience method to return the content of the message in the first choice of this response
		/// </summary>
		/// <returns>The content of the message, not including <see cref="ChatMessageRole"/>.</returns>
		public override string ToString()
		{
			if (Choices != null && Choices.Count > 0)
				return Choices[0].ToString();
			else
				return null;
		}
	}

	/// <summary>
	/// A message received from the API, including the message text, index, and reason why the message finished.
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

		/// <summary>
		/// Partial message "delta" from a stream. For example, the result from <see cref="ChatEndpoint.StreamChatEnumerableAsync(ChatRequest)">StreamChatEnumerableAsync.</see>
		/// If this result object is not from a stream, this will be null
		/// </summary>
		[JsonProperty("delta")]
		public ChatMessage Delta { get; set; }

		/// <summary>
		/// A convenience method to return the content of the message in this response
		/// </summary>
		/// <returns>The content of the message in this response, not including <see cref="ChatMessageRole"/>.</returns>
		public override string ToString()
		{
			if (Message == null && Delta != null)
				return Delta.TextContent;
			else
				return Message.TextContent;
		}
	}

	/// <summary>
	/// How many tokens were used in this chat message.
	/// </summary>
	public class ChatUsage : Usage
	{
		/// <summary>
		/// The number of completion tokens used during the chat interaction
		/// </summary>
		[JsonProperty("completion_tokens")]
		public int CompletionTokens { get; set; }
	}
}
