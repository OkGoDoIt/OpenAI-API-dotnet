using Newtonsoft.Json;
using OpenAI_API.Embedding;
using System.Collections.Generic;

namespace OpenAI_API.Completions
{
	/// <summary>
	/// Represents a completion choice returned by the Completion API.  
	/// </summary>
	public class Choice
	{
		/// <summary>
		/// The main text of the completion
		/// </summary>
		[JsonProperty("text")]
		public string Text { get; set; }

		/// <summary>
		/// If multiple completion choices we returned, this is the index withing the various choices
		/// </summary>
		[JsonProperty("index")]
		public int Index { get; set; }

		/// <summary>
		/// If the request specified <see cref="CompletionRequest.Logprobs"/>, this contains the list of the most likely tokens.
		/// </summary>
		[JsonProperty("logprobs")]
		public Logprobs Logprobs { get; set; }

		/// <summary>
		/// If this is the last segment of the completion result, this specifies why the completion has ended.
		/// </summary>
		[JsonProperty("finish_reason")]
		public string FinishReason { get; set; }

		/// <summary>
		/// Gets the main text of this completion
		/// </summary>
		public override string ToString()
		{
			return Text;
		}
	}

	/// <summary>
	/// API usage as reported by the OpenAI API for this request
	/// </summary>
	public class CompletionUsage : Usage
	{
		/// <summary>
		/// How many tokens are in the completion(s)
		/// </summary>
		[JsonProperty("completion_tokens")]
		public short CompletionTokens { get; set; }
	}

	/// <summary>
	/// Represents a result from calling the Completion API
	/// </summary>
	public class CompletionResult : ApiResultBase
	{
		/// <summary>
		/// The identifier of the result, which may be used during troubleshooting
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// The completions returned by the API.  Depending on your request, there may be 1 or many choices.
		/// </summary>
		[JsonProperty("choices")]
		public List<Choice> Completions { get; set; }

		/// <summary>
		/// API token usage as reported by the OpenAI API for this request
		/// </summary>
		[JsonProperty("usage")]
		public CompletionUsage Usage { get; set; }

		/// <summary>
		/// Gets the text of the first completion, representing the main result
		/// </summary>
		public override string ToString()
		{
			if (Completions != null && Completions.Count > 0)
				return Completions[0].ToString();
			else
				return $"CompletionResult {Id} has no valid output";
		}
	}


	public class Logprobs
	{
		[JsonProperty("tokens")]
		public List<string> Tokens { get; set; }

		[JsonProperty("token_logprobs")]
		public List<double?> TokenLogprobs { get; set; }

		[JsonProperty("top_logprobs")]
		public IList<IDictionary<string, double>> TopLogprobs { get; set; }

		[JsonProperty("text_offset")]
		public List<int> TextOffsets { get; set; }
	}

}
