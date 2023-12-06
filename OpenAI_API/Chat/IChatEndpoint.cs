﻿using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenAI_API.Chat
{
	/// <summary>
	/// An interface for <see cref="ChatEndpoint"/>, the ChatGPT API endpoint. Use this endpoint to send multiple messages and carry on a conversation.
	/// </summary>
	public interface IChatEndpoint
	{
		/// <summary>
		/// This allows you to set default parameters for every request, for example to set a default temperature or max tokens.  For every request, if you do not have a parameter set on the request but do have it set here as a default, the request will automatically pick up the default value.
		/// </summary>
		ChatRequest DefaultChatRequestArgs { get; set; }

		/// <summary>
		/// Creates an ongoing chat which can easily encapsulate the conversation.  This is the simplest way to use the Chat endpoint.
		/// </summary>
		/// <param name="defaultChatRequestArgs">Allows setting the parameters to use when calling the ChatGPT API.  Can be useful for setting temperature, presence_penalty, and more.  See <see href="https://platform.openai.com/docs/api-reference/chat/create">OpenAI documentation for a list of possible parameters to tweak.</see></param>
		/// <returns>A <see cref="Conversation"/> which encapsulates a back and forth chat between a user and an assistant.</returns>
		Conversation CreateConversation(ChatRequest defaultChatRequestArgs = null);


		/// <summary>
		/// Ask the API to complete the request using the specified parameters. This is non-streaming, so it will wait until the API returns the full result. Any non-specified parameters will fall back to default values specified in <see cref="DefaultChatRequestArgs"/> if present.
		/// </summary>
		/// <param name="request">The request to send to the API.</param>
		/// <returns>Asynchronously returns the completion result. Look in its <see cref="ChatResult.Choices"/> property for the results.</returns>
		Task<ChatResult> CreateChatCompletionAsync(ChatRequest request);

		/// <summary>
		/// Ask the API to complete the request using the specified parameters. This is non-streaming, so it will wait until the API returns the full result. Any non-specified parameters will fall back to default values specified in <see cref="DefaultChatRequestArgs"/> if present.
		/// </summary>
		/// <param name="request">The request to send to the API.</param>
		/// <param name="numOutputs">Overrides <see cref="ChatRequest.NumChoicesPerMessage"/> as a convenience.</param>
		/// <returns>Asynchronously returns the completion result. Look in its <see cref="ChatResult.Choices"/> property for the results.</returns>
		Task<ChatResult> CreateChatCompletionAsync(ChatRequest request, int numOutputs = 5);

		/// <summary>
		/// Ask the API to complete the request using the specified parameters. This is non-streaming, so it will wait until the API returns the full result. Any non-specified parameters will fall back to default values specified in <see cref="DefaultChatRequestArgs"/> if present.
		/// </summary>
		/// <param name="messages">The array of messages to send to the API</param>
		/// <param name="model">The model to use. See the ChatGPT models available from <see cref="ModelsEndpoint.GetModelsAsync()"/></param>
		/// <param name="temperature">What sampling temperature to use. Higher values means the model will take more risks. Try 0.9 for more creative applications, and 0 (argmax sampling) for ones with a well-defined answer. It is generally recommend to use this or <paramref name="top_p"/> but not both.</param>
		/// <param name="top_p">An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are considered. It is generally recommend to use this or <paramref name="temperature"/> but not both.</param>
		/// <param name="numOutputs">How many different choices to request for each prompt.</param>
		/// <param name="max_tokens">How many tokens to complete to. Can return fewer if a stop sequence is hit.</param>
		/// <param name="frequencyPenalty">The scale of the penalty for how often a token is used.  Should generally be between 0 and 1, although negative numbers are allowed to encourage token reuse.</param>
		/// <param name="presencePenalty">The scale of the penalty applied if a token is already present at all.  Should generally be between 0 and 1, although negative numbers are allowed to encourage token reuse.</param>
		/// <param name="logitBias">Maps tokens (specified by their token ID in the tokenizer) to an associated bias value from -100 to 100. Mathematically, the bias is added to the logits generated by the model prior to sampling. The exact effect will vary per model, but values between -1 and 1 should decrease or increase likelihood of selection; values like -100 or 100 should result in a ban or exclusive selection of the relevant token.</param>
		/// <param name="stopSequences">One or more sequences where the API will stop generating further tokens. The returned text will not contain the stop sequence.</param>
		/// <returns>Asynchronously returns the completion result. Look in its <see cref="ChatResult.Choices"/> property for the results.</returns>
		Task<ChatResult> CreateChatCompletionAsync(IList<ChatMessage> messages, Model model = null, double? temperature = null, double? top_p = null, int? numOutputs = null, int? max_tokens = null, double? frequencyPenalty = null, double? presencePenalty = null, IReadOnlyDictionary<string, float> logitBias = null, params string[] stopSequences);

		/// <summary>
		/// Ask the API to complete the request using the specified message(s).  Any parameters will fall back to default values specified in <see cref="DefaultChatRequestArgs"/> if present.
		/// </summary>
		/// <param name="messages">The messages to use in the generation.</param>
		/// <returns>The <see cref="ChatResult"/> with the API response.</returns>
		Task<ChatResult> CreateChatCompletionAsync(params ChatMessage[] messages);

		/// <summary>
		/// Ask the API to complete the request using the specified message(s).  Any parameters will fall back to default values specified in <see cref="DefaultChatRequestArgs"/> if present.
		/// </summary>
		/// <param name="userMessages">The user message or messages to use in the generation.  All strings are assumed to be of Role <see cref="ChatMessageRole.User"/></param>
		/// <returns>The <see cref="ChatResult"/> with the API response.</returns>
		Task<ChatResult> CreateChatCompletionAsync(params string[] userMessages);


		/// <summary>
		/// Ask the API to complete the message(s) using the specified request, and stream the results to the <paramref name="resultHandler"/> as they come in.
		/// If you are on the latest C# supporting async enumerables, you may prefer the cleaner syntax of <see cref="StreamChatEnumerableAsync(ChatRequest)"/> instead.
		/// </summary>
		/// <param name="request">The request to send to the API. This does not fall back to default values specified in <see cref="DefaultChatRequestArgs"/>.</param>
		/// <param name="resultHandler">An action to be called as each new result arrives, which includes the index of the result in the overall result set.</param>
		Task StreamChatAsync(ChatRequest request, Action<ChatResult> resultHandler);

		/// <summary>
		/// Ask the API to complete the message(s) using the specified request, and stream the results as they come in.
		/// If you are not using C# 8 supporting async enumerables or if you are using the .NET Framework, you may need to use <see cref="StreamChatAsync(ChatRequest, Action{ChatResult})"/> instead.
		/// </summary>
		/// <param name="request">The request to send to the API.  This does not fall back to default values specified in <see cref="DefaultChatRequestArgs"/>.</param>
		/// <returns>An async enumerable with each of the results as they come in.  See <see href="https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#asynchronous-streams"/> for more details on how to consume an async enumerable.</returns>
		IAsyncEnumerable<ChatResult> StreamChatEnumerableAsync(ChatRequest request);

		/// <summary>
		/// Ask the API to complete the message(s) using the specified request, and stream the results as they come in.
		/// If you are not using C# 8 supporting async enumerables or if you are using the .NET Framework, you may need to use <see cref="StreamChatAsync(ChatRequest, Action{ChatResult})"/> instead.
		/// </summary>
		/// <param name="messages">The array of messages to send to the API</param>
		/// <param name="model">The model to use. See the ChatGPT models available from <see cref="ModelsEndpoint.GetModelsAsync()"/></param>
		/// <param name="temperature">What sampling temperature to use. Higher values means the model will take more risks. Try 0.9 for more creative applications, and 0 (argmax sampling) for ones with a well-defined answer. It is generally recommend to use this or <paramref name="top_p"/> but not both.</param>
		/// <param name="top_p">An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are considered. It is generally recommend to use this or <paramref name="temperature"/> but not both.</param>
		/// <param name="numOutputs">How many different choices to request for each prompt.</param>
		/// <param name="max_tokens">How many tokens to complete to. Can return fewer if a stop sequence is hit.</param>
		/// <param name="frequencyPenalty">The scale of the penalty for how often a token is used.  Should generally be between 0 and 1, although negative numbers are allowed to encourage token reuse.</param>
		/// <param name="presencePenalty">The scale of the penalty applied if a token is already present at all.  Should generally be between 0 and 1, although negative numbers are allowed to encourage token reuse.</param>
		/// <param name="logitBias">Maps tokens (specified by their token ID in the tokenizer) to an associated bias value from -100 to 100. Mathematically, the bias is added to the logits generated by the model prior to sampling. The exact effect will vary per model, but values between -1 and 1 should decrease or increase likelihood of selection; values like -100 or 100 should result in a ban or exclusive selection of the relevant token.</param>
		/// <param name="stopSequences">One or more sequences where the API will stop generating further tokens. The returned text will not contain the stop sequence.</param>
		/// <returns>An async enumerable with each of the results as they come in. See <see href="https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#asynchronous-streams">the C# docs</see> for more details on how to consume an async enumerable.</returns>
		IAsyncEnumerable<ChatResult> StreamChatEnumerableAsync(IList<ChatMessage> messages, Model model = null, double? temperature = null, double? top_p = null, int? numOutputs = null, int? max_tokens = null, double? frequencyPenalty = null, double? presencePenalty = null, IReadOnlyDictionary<string, float> logitBias = null, params string[] stopSequences);

		/// <summary>
		/// Ask the API to complete the message(s) using the specified request, and stream the results to the <paramref name="resultHandler"/> as they come in.
		/// If you are on the latest C# supporting async enumerables, you may prefer the cleaner syntax of <see cref="StreamChatEnumerableAsync(ChatRequest)"/> instead.
		/// </summary>
		/// <param name="request">The request to send to the API. This does not fall back to default values specified in <see cref="DefaultChatRequestArgs"/>.</param>
		/// <param name="resultHandler">An action to be called as each new result arrives, which includes the index of the result in the overall result set.</param>
		Task StreamCompletionAsync(ChatRequest request, Action<int, ChatResult> resultHandler);
	}
}