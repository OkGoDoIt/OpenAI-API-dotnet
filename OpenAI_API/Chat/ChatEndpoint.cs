using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_API.Chat
{
	/// <summary>
	/// 
	/// </summary>
    public class ChatEndpoint : EndpointBase
    {
		/// <summary>
		/// This allows you to set default parameters for every request, for example to set a default temperature or max tokens.  For every request, if you do not have a parameter set on the request but do have it set here as a default, the request will automatically pick up the default value.
		/// </summary>
		public ChatRequest DefaultCompletionRequestArgs { get; set; } = new ChatRequest() { Model = Model.ChatGPTTurbo };

		/// <summary>
		/// The name of the endpoint, which is the final path segment in the API URL.  For example, "completions".
		/// </summary>
		protected override string Endpoint { get { return "chat/completions"; } }

		/// <summary>
		/// Constructor of the api endpoint.  Rather than instantiating this yourself, access it through an instance of <see cref="OpenAIAPI"/> as <see cref="OpenAIAPI.Completions"/>.
		/// </summary>
		/// <param name="api"></param>
		internal ChatEndpoint(OpenAIAPI api) : base(api) { }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public async Task<ChatResult> CreateCompletionAsync(ChatRequest request)
		{
			return await HttpPost<ChatResult>(postData: request);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <param name="numOutputs"></param>
		/// <returns></returns>
		public Task<ChatResult> CreateCompletionsAsync(ChatRequest request, int numOutputs = 5)
		{
			request.NumChoicesPerPrompt = numOutputs;
			return CreateCompletionAsync(request);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="messages"></param>
		/// <param name="model"></param>
		/// <param name="temperature"></param>
		/// <param name="topP"></param>
		/// <param name="numChoicesPerPrompt"></param>
		/// <param name="maxTokens"></param>
		/// <param name="frequencyPenalty"></param>
		/// <param name="presencePenalty"></param>
		/// <param name="logitBias"></param>
		/// <param name="multipleStopSequences"></param>
		/// <returns></returns>
		public Task<ChatResult> CreateCompletionAsync(List<ChatMessage> messages = null,
			Model model = null,
			double? temperature = null,
			double? topP = null,
			int? numChoicesPerPrompt = null,
			int? maxTokens = null,
			double? frequencyPenalty = null,
			double? presencePenalty = null,
			Dictionary<string, float> logitBias = null,
			params string[] multipleStopSequences)
		{
			ChatRequest request = new ChatRequest(DefaultCompletionRequestArgs)
			{
				Messages = messages,
				Model = model ?? DefaultCompletionRequestArgs.Model,
				Temperature = temperature ?? null,
				TopP = topP ?? null,
				NumChoicesPerPrompt = numChoicesPerPrompt ?? null,
				MultipleStopSequences = multipleStopSequences ?? null,
				MaxTokens = maxTokens ?? null,
				FrequencyPenalty = frequencyPenalty ?? null,
				PresencePenalty = presencePenalty ?? null,
				LogitBias = logitBias ?? null
			};
			return CreateCompletionAsync(request);
		}

	}
}
