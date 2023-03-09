using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAI_API.Models;

namespace OpenAI_API.Completions
{
	/// <summary>
	/// An interface for <see cref="CompletionEndpoint"/>, for ease of mock testing, etc
	/// </summary>
	public interface ICompletionEndpoint
    {
        /// <summary>
        /// This allows you to set default parameters for every request, for example to set a default temperature or max tokens.  For every request, if you do not have a parameter set on the request but do have it set here as a default, the request will automatically pick up the default value.
        /// </summary>
        CompletionRequest DefaultCompletionRequestArgs { get; set; }

        /// <summary>
        /// Ask the API to complete the prompt(s) using the specified request.  This is non-streaming, so it will wait until the API returns the full result.
        /// </summary>
        /// <param name="request">The request to send to the API.  This does not fall back to default values specified in <see cref="DefaultCompletionRequestArgs"/>.</param>
        /// <returns>Asynchronously returns the completion result.  Look in its <see cref="CompletionResult.Completions"/> property for the completions.</returns>
        Task<CompletionResult> CreateCompletionAsync(CompletionRequest request);

        /// <summary>
        /// Ask the API to complete the prompt(s) using the specified parameters.  This is non-streaming, so it will wait until the API returns the full result.  Any non-specified parameters will fall back to default values specified in <see cref="DefaultCompletionRequestArgs"/> if present.
        /// </summary>
        /// <param name="prompt">The prompt to generate from</param>
        /// <param name="model">The model to use. You can use <see cref="ModelsEndpoint.GetModelsAsync()"/> to see all of your available models, or use a standard model like <see cref="Model.DavinciText"/>.</param>
        /// <param name="max_tokens">How many tokens to complete to. Can return fewer if a stop sequence is hit.</param>
        /// <param name="temperature">What sampling temperature to use. Higher values means the model will take more risks. Try 0.9 for more creative applications, and 0 (argmax sampling) for ones with a well-defined answer. It is generally recommend to use this or <paramref name="top_p"/> but not both.</param>
        /// <param name="top_p">An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are considered. It is generally recommend to use this or <paramref name="temperature"/> but not both.</param>
        /// <param name="numOutputs">How many different choices to request for each prompt.</param>
        /// <param name="presencePenalty">The scale of the penalty applied if a token is already present at all.  Should generally be between 0 and 1, although negative numbers are allowed to encourage token reuse.</param>
        /// <param name="frequencyPenalty">The scale of the penalty for how often a token is used.  Should generally be between 0 and 1, although negative numbers are allowed to encourage token reuse.</param>
        /// <param name="logProbs">Include the log probabilities on the logprobs most likely tokens, which can be found in <see cref="CompletionResult.Completions"/> -> <see cref="Choice.Logprobs"/>. So for example, if logprobs is 10, the API will return a list of the 10 most likely tokens. If logprobs is supplied, the API will always return the logprob of the sampled token, so there may be up to logprobs+1 elements in the response.</param>
        /// <param name="echo">Echo back the prompt in addition to the completion.</param>
        /// <param name="stopSequences">One or more sequences where the API will stop generating further tokens. The returned text will not contain the stop sequence.</param>
        /// <returns>Asynchronously returns the completion result.  Look in its <see cref="CompletionResult.Completions"/> property for the completions.</returns>
        Task<CompletionResult> CreateCompletionAsync(string prompt,
            Model model = null,
            int? max_tokens = null,
            double? temperature = null,
            double? top_p = null,
            int? numOutputs = null,
            double? presencePenalty = null,
            double? frequencyPenalty = null,
            int? logProbs = null,
            bool? echo = null,
            params string[] stopSequences
        );

        /// <summary>
        /// Ask the API to complete the prompt(s) using the specified promptes, with other paramets being drawn from default values specified in <see cref="DefaultCompletionRequestArgs"/> if present.  This is non-streaming, so it will wait until the API returns the full result.
        /// </summary>
        /// <param name="prompts">One or more prompts to generate from</param>
        /// <returns></returns>
        Task<CompletionResult> CreateCompletionAsync(params string[] prompts);

        /// <summary>
        /// Ask the API to complete the prompt(s) using the specified request and a requested number of outputs.  This is non-streaming, so it will wait until the API returns the full result.
        /// </summary>
        /// <param name="request">The request to send to the API.  This does not fall back to default values specified in <see cref="DefaultCompletionRequestArgs"/>.</param>
        /// <param name="numOutputs">Overrides <see cref="CompletionRequest.NumChoicesPerPrompt"/> as a convenience.</param>
        /// <returns>Asynchronously returns the completion result.  Look in its <see cref="CompletionResult.Completions"/> property for the completions, which should have a length equal to <paramref name="numOutputs"/>.</returns>
        Task<CompletionResult> CreateCompletionsAsync(CompletionRequest request, int numOutputs = 5);

        /// <summary>
        /// Ask the API to complete the prompt(s) using the specified request, and stream the results to the <paramref name="resultHandler"/> as they come in.
        /// If you are on the latest C# supporting async enumerables, you may prefer the cleaner syntax of <see cref="StreamCompletionEnumerableAsync(CompletionRequest)"/> instead.
        /// </summary>
        /// <param name="request">The request to send to the API.  This does not fall back to default values specified in <see cref="DefaultCompletionRequestArgs"/>.</param>
        /// <param name="resultHandler">An action to be called as each new result arrives, which includes the index of the result in the overall result set.</param>
        Task StreamCompletionAsync(CompletionRequest request, Action<int, CompletionResult> resultHandler);

        /// <summary>
        /// Ask the API to complete the prompt(s) using the specified request, and stream the results to the <paramref name="resultHandler"/> as they come in.
        /// If you are on the latest C# supporting async enumerables, you may prefer the cleaner syntax of <see cref="StreamCompletionEnumerableAsync(CompletionRequest)"/> instead.
        /// </summary>
        /// <param name="request">The request to send to the API.  This does not fall back to default values specified in <see cref="DefaultCompletionRequestArgs"/>.</param>
        /// <param name="resultHandler">An action to be called as each new result arrives.</param>
        Task StreamCompletionAsync(CompletionRequest request, Action<CompletionResult> resultHandler);

        /// <summary>
        /// Ask the API to complete the prompt(s) using the specified request, and stream the results as they come in.
        /// If you are not using C# 8 supporting async enumerables or if you are using the .NET Framework, you may need to use <see cref="StreamCompletionAsync(CompletionRequest, Action{CompletionResult})"/> instead.
        /// </summary>
        /// <param name="request">The request to send to the API.  This does not fall back to default values specified in <see cref="DefaultCompletionRequestArgs"/>.</param>
        /// <returns>An async enumerable with each of the results as they come in.  See <see href="https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#asynchronous-streams"/> for more details on how to consume an async enumerable.</returns>
        IAsyncEnumerable<CompletionResult> StreamCompletionEnumerableAsync(CompletionRequest request);

        /// <summary>
        /// Ask the API to complete the prompt(s) using the specified parameters. 
        /// Any non-specified parameters will fall back to default values specified in <see cref="DefaultCompletionRequestArgs"/> if present.
        /// If you are not using C# 8 supporting async enumerables or if you are using the .NET Framework, you may need to use <see cref="StreamCompletionAsync(CompletionRequest, Action{CompletionResult})"/> instead.
        /// </summary>
        /// <param name="prompt">The prompt to generate from</param>
        /// <param name="model">The model to use. You can use <see cref="ModelsEndpoint.GetModelsAsync()"/> to see all of your available models, or use a standard model like <see cref="Model.DavinciText"/>.</param>
        /// <param name="max_tokens">How many tokens to complete to. Can return fewer if a stop sequence is hit.</param>
        /// <param name="temperature">What sampling temperature to use. Higher values means the model will take more risks. Try 0.9 for more creative applications, and 0 (argmax sampling) for ones with a well-defined answer. It is generally recommend to use this or <paramref name="top_p"/> but not both.</param>
        /// <param name="top_p">An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are considered. It is generally recommend to use this or <paramref name="temperature"/> but not both.</param>
        /// <param name="numOutputs">How many different choices to request for each prompt.</param>
        /// <param name="presencePenalty">The scale of the penalty applied if a token is already present at all.  Should generally be between 0 and 1, although negative numbers are allowed to encourage token reuse.</param>
        /// <param name="frequencyPenalty">The scale of the penalty for how often a token is used.  Should generally be between 0 and 1, although negative numbers are allowed to encourage token reuse.</param>
        /// <param name="logProbs">Include the log probabilities on the logprobs most likely tokens, which can be found in <see cref="CompletionResult.Completions"/> -> <see cref="Choice.Logprobs"/>. So for example, if logprobs is 10, the API will return a list of the 10 most likely tokens. If logprobs is supplied, the API will always return the logprob of the sampled token, so there may be up to logprobs+1 elements in the response.</param>
        /// <param name="echo">Echo back the prompt in addition to the completion.</param>
        /// <param name="stopSequences">One or more sequences where the API will stop generating further tokens. The returned text will not contain the stop sequence.</param>
        /// <returns>An async enumerable with each of the results as they come in.  See <see href="https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#asynchronous-streams">the C# docs</see> for more details on how to consume an async enumerable.</returns>
        IAsyncEnumerable<CompletionResult> StreamCompletionEnumerableAsync(string prompt,
            Model model = null,
            int? max_tokens = null,
            double? temperature = null,
            double? top_p = null,
            int? numOutputs = null,
            double? presencePenalty = null,
            double? frequencyPenalty = null,
            int? logProbs = null,
            bool? echo = null,
            params string[] stopSequences);

        /// <summary>
        /// Simply returns a string of the prompt followed by the best completion
        /// </summary>
        /// <param name="request">The request to send to the API.  This does not fall back to default values specified in <see cref="DefaultCompletionRequestArgs"/>.</param>
        /// <returns>A string of the prompt followed by the best completion</returns>
        Task<string> CreateAndFormatCompletion(CompletionRequest request);

        /// <summary>
        /// Simply returns the best completion
        /// </summary>
        /// <param name="prompt">The prompt to complete</param>
        /// <returns>The best completion</returns>
        Task<string> GetCompletion(string prompt);
    }
}