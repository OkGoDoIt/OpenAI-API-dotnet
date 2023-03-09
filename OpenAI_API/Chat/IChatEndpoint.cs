using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenAI_API.Chat
{
	/// <summary>
	/// An interface for <see cref="ChatEndpoint"/>, for ease of mock testing, etc
	/// </summary>
	public interface IChatEndpoint
	{
		ChatRequest DefaultChatRequestArgs { get; set; }

		Task<ChatResult> CreateChatCompletionAsync(ChatRequest request);
		Task<ChatResult> CreateChatCompletionAsync(ChatRequest request, int numOutputs = 5);
		Task<ChatResult> CreateChatCompletionAsync(IList<ChatMessage> messages, Model model = null, double? temperature = null, double? top_p = null, int? numOutputs = null, int? max_tokens = null, double? frequencyPenalty = null, double? presencePenalty = null, IReadOnlyDictionary<string, float> logitBias = null, params string[] stopSequences);
		Task<ChatResult> CreateChatCompletionAsync(params ChatMessage[] messages);
		Task<ChatResult> CreateChatCompletionAsync(params string[] userMessages);
		Conversation CreateConversation();
		Task StreamChatAsync(ChatRequest request, Action<ChatResult> resultHandler);
		IAsyncEnumerable<ChatResult> StreamChatEnumerableAsync(ChatRequest request);
		IAsyncEnumerable<ChatResult> StreamChatEnumerableAsync(IList<ChatMessage> messages, Model model = null, double? temperature = null, double? top_p = null, int? numOutputs = null, int? max_tokens = null, double? frequencyPenalty = null, double? presencePenalty = null, IReadOnlyDictionary<string, float> logitBias = null, params string[] stopSequences);
		Task StreamCompletionAsync(ChatRequest request, Action<int, ChatResult> resultHandler);
	}
}