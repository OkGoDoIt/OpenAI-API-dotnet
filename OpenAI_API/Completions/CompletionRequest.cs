using Newtonsoft.Json;
using OpenAI_API.Models;
using System.Linq;

namespace OpenAI_API.Completions
{
	/// <summary>
	/// Represents a request to the Completions API.  Mostly matches the parameters in <see href="https://beta.openai.com/api-ref#create-completion-post">the OpenAI docs</see>, although some have been renamed or expanded into single/multiple properties for ease of use.
	/// </summary>
	public class CompletionRequest
	{
		/// <summary>
		/// ID of the model to use. You can use <see cref="ModelsEndpoint.GetModelsAsync()"/> to see all of your available models, or use a standard model like <see cref="Model.DavinciText"/>.
		/// </summary>
		[JsonProperty("model")]
		public string Model { get; set; } = OpenAI_API.Models.Model.DefaultModel;

		/// <summary>
		/// This is only used for serializing the request into JSON, do not use it directly.
		/// </summary>
		[JsonProperty("prompt")]
		public object CompiledPrompt
		{
			get
			{
				if (MultiplePrompts?.Length == 1)
					return Prompt;
				else
					return MultiplePrompts;
			}
		}

		/// <summary>
		/// If you are requesting more than one prompt, specify them as an array of strings.
		/// </summary>
		[JsonIgnore]
		public string[] MultiplePrompts { get; set; }

		/// <summary>
		/// For convenience, if you are only requesting a single prompt, set it here
		/// </summary>
		[JsonIgnore]
		public string Prompt
		{
			get => MultiplePrompts.FirstOrDefault();
			set
			{
				MultiplePrompts = new string[] { value };
			}
		}

		/// <summary>
		/// The suffix that comes after a completion of inserted text.  Defaults to null.
		/// </summary>
		[JsonProperty("suffix")]
		public string Suffix { get; set; }

		/// <summary>
		/// How many tokens to complete to. Can return fewer if a stop sequence is hit.  Defaults to 16.
		/// </summary>
		[JsonProperty("max_tokens")]
		public int? MaxTokens { get; set; }

		/// <summary>
		/// What sampling temperature to use. Higher values means the model will take more risks. Try 0.9 for more creative applications, and 0 (argmax sampling) for ones with a well-defined answer. It is generally recommend to use this or <see cref="TopP"/> but not both.
		/// </summary>
		[JsonProperty("temperature")]
		public double? Temperature { get; set; }

		/// <summary>
		/// An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are considered. It is generally recommend to use this or <see cref="Temperature"/> but not both.
		/// </summary>
		[JsonProperty("top_p")]
		public double? TopP { get; set; }

		/// <summary>
		/// The scale of the penalty applied if a token is already present at all.  Should generally be between 0 and 1, although negative numbers are allowed to encourage token reuse.  Defaults to 0.
		/// </summary>
		[JsonProperty("presence_penalty")]
		public double? PresencePenalty { get; set; }

		/// <summary>
		/// The scale of the penalty for how often a token is used.  Should generally be between 0 and 1, although negative numbers are allowed to encourage token reuse.  Defaults to 0.
		/// </summary>
		[JsonProperty("frequency_penalty")]
		public double? FrequencyPenalty { get; set; }

		/// <summary>
		/// How many different choices to request for each prompt.  Defaults to 1.
		/// </summary>
		[JsonProperty("n")]
		public int? NumChoicesPerPrompt { get; set; }

		/// <summary>
		/// Specifies where the results should stream and be returned at one time.  Do not set this yourself, use the appropriate methods on <see cref="CompletionEndpoint"/> instead.
		/// </summary>
		[JsonProperty("stream")]
		public bool Stream { get; internal set; } = false;

		/// <summary>
		/// Include the log probabilities on the logprobs most likely tokens, which can be found in <see cref="CompletionResult.Completions"/> -> <see cref="Choice.Logprobs"/>. So for example, if logprobs is 5, the API will return a list of the 5 most likely tokens. If logprobs is supplied, the API will always return the logprob of the sampled token, so there may be up to logprobs+1 elements in the response.  The maximum value for logprobs is 5.
		/// </summary>
		[JsonProperty("logprobs")]
		public int? Logprobs { get; set; }

		/// <summary>
		/// Echo back the prompt in addition to the completion.  Defaults to false.
		/// </summary>
		[JsonProperty("echo")]
		public bool? Echo { get; set; }

		/// <summary>
		/// This is only used for serializing the request into JSON, do not use it directly.
		/// </summary>
		[JsonProperty("stop")]
		public object CompiledStop
		{
			get
			{
				if (MultipleStopSequences?.Length == 1)
					return StopSequence;
				else if (MultipleStopSequences?.Length > 0)
					return MultipleStopSequences;
				else
					return null;
			}
		}

		/// <summary>
		/// One or more sequences where the API will stop generating further tokens. The returned text will not contain the stop sequence.
		/// </summary>
		[JsonIgnore]
		public string[] MultipleStopSequences { get; set; }


		/// <summary>
		/// The stop sequence where the API will stop generating further tokens. The returned text will not contain the stop sequence.  For convenience, if you are only requesting a single stop sequence, set it here
		/// </summary>
		[JsonIgnore]
		public string StopSequence
		{
			get => MultipleStopSequences?.FirstOrDefault() ?? null;
			set
			{
				if (value != null)
					MultipleStopSequences = new string[] { value };
			}
		}

		/// <summary>
		/// Generates best_of completions server-side and returns the "best" (the one with the highest log probability per token). Results cannot be streamed.
		/// When used with n, best_of controls the number of candidate completions and n specifies how many to return – best_of must be greater than n.
		/// Note: Because this parameter generates many completions, it can quickly consume your token quota.Use carefully and ensure that you have reasonable settings for max_tokens and stop.
		/// </summary>
		[JsonProperty("best_of")]
		public int? BestOf { get; set; }

		/// <summary>
		/// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
		/// </summary>
		[JsonProperty("user")]
		public string user { get; set; }

		/// <summary>
		/// Cretes a new, empty <see cref="CompletionRequest"/>
		/// </summary>
		public CompletionRequest()
		{
			this.Model = OpenAI_API.Models.Model.DefaultModel;
		}

		/// <summary>
		/// Creates a new <see cref="CompletionRequest"/>, inheriting any parameters set in <paramref name="basedOn"/>.
		/// </summary>
		/// <param name="basedOn">The <see cref="CompletionRequest"/> to copy</param>
		public CompletionRequest(CompletionRequest basedOn)
		{
			this.Model = basedOn.Model;
			this.MultiplePrompts = basedOn.MultiplePrompts;
			this.MaxTokens = basedOn.MaxTokens;
			this.Temperature = basedOn.Temperature;
			this.TopP = basedOn.TopP;
			this.NumChoicesPerPrompt = basedOn.NumChoicesPerPrompt;
			this.PresencePenalty = basedOn.PresencePenalty;
			this.FrequencyPenalty = basedOn.FrequencyPenalty;
			this.Logprobs = basedOn.Logprobs;
			this.Echo = basedOn.Echo;
			this.MultipleStopSequences = basedOn.MultipleStopSequences;
			this.BestOf = basedOn.BestOf;
			this.user = basedOn.user;
			this.Suffix = basedOn.Suffix;
		}

		/// <summary>
		/// Creates a new <see cref="CompletionRequest"/>, using the specified prompts
		/// </summary>
		/// <param name="prompts">One or more prompts to generate from</param>
		public CompletionRequest(params string[] prompts)
		{
			this.MultiplePrompts = prompts;
		}

		/// <summary>
		/// Creates a new <see cref="CompletionRequest"/> with the specified parameters
		/// </summary>
		/// <param name="prompt">The prompt to generate from</param>
		/// <param name="model">The model to use. You can use <see cref="ModelsEndpoint.GetModelsAsync()"/> to see all of your available models, or use a standard model like <see cref="Model.DavinciText"/>.</param>
		/// <param name="max_tokens">How many tokens to complete to. Can return fewer if a stop sequence is hit.</param>
		/// <param name="temperature">What sampling temperature to use. Higher values means the model will take more risks. Try 0.9 for more creative applications, and 0 (argmax sampling) for ones with a well-defined answer. It is generally recommend to use this or <paramref name="top_p"/> but not both.</param>
		/// <param name="suffix">The suffix that comes after a completion of inserted text</param>
		/// <param name="top_p">An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are considered. It is generally recommend to use this or <paramref name="temperature"/> but not both.</param>
		/// <param name="numOutputs">How many different choices to request for each prompt.</param>
		/// <param name="presencePenalty">The scale of the penalty applied if a token is already present at all.  Should generally be between 0 and 1, although negative numbers are allowed to encourage token reuse.</param>
		/// <param name="frequencyPenalty">The scale of the penalty for how often a token is used.  Should generally be between 0 and 1, although negative numbers are allowed to encourage token reuse.</param>
		/// <param name="logProbs">Include the log probabilities on the logprobs most likely tokens, which can be found in <see cref="CompletionResult.Completions"/> -> <see cref="Choice.Logprobs"/>. So for example, if logprobs is 10, the API will return a list of the 10 most likely tokens. If logprobs is supplied, the API will always return the logprob of the sampled token, so there may be up to logprobs+1 elements in the response.</param>
		/// <param name="echo">Echo back the prompt in addition to the completion.</param>
		/// <param name="stopSequences">One or more sequences where the API will stop generating further tokens. The returned text will not contain the stop sequence.</param>
		public CompletionRequest(
			string prompt,
			Model model = null,
			int? max_tokens = null,
			double? temperature = null,
			string suffix = null,
			double? top_p = null,
			int? numOutputs = null,
			double? presencePenalty = null,
			double? frequencyPenalty = null,
			int? logProbs = null,
			bool? echo = null,
			params string[] stopSequences)
		{
			this.Model = model;
			this.Prompt = prompt;
			this.MaxTokens = max_tokens;
			this.Temperature = temperature;
			this.Suffix = suffix;
			this.TopP = top_p;
			this.NumChoicesPerPrompt = numOutputs;
			this.PresencePenalty = presencePenalty;
			this.FrequencyPenalty = frequencyPenalty;
			this.Logprobs = logProbs;
			this.Echo = echo;
			this.MultipleStopSequences = stopSequences;
		}


	}

}
