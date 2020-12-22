using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OpenAI_API
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
	/// Represents a result from calling the Completion API
	/// </summary>
	public class CompletionResult
	{
		/// <summary>
		/// The identifier of the result, which may be used during troubleshooting
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// The time when the result was generated in unix epoch format
		/// </summary>
		[JsonProperty("created")]
		public int CreatedUnixTime { get; set; }

		/// The time when the result was generated
		[JsonIgnore]
		public DateTime Created => DateTimeOffset.FromUnixTimeSeconds(CreatedUnixTime).DateTime;

		/// <summary>
		/// Which model was used to generate this result.  Be sure to check <see cref="Engine.ModelRevision"/> for the specific revision.
		/// </summary>
		[JsonProperty("model")]
		public Engine Model { get; set; }

		/// <summary>
		/// The completions returned by the API.  Depending on your request, there may be 1 or many choices.
		/// </summary>
		[JsonProperty("choices")]
		public List<Choice> Completions { get; set; }

		/// <summary>
		/// The server-side processing time as reported by the API.  This can be useful for debugging where a delay occurs.
		/// </summary>
		[JsonIgnore]
		public TimeSpan ProcessingTime { get; set; }

		/// <summary>
		/// The organization associated with the API request, as reported by the API.
		/// </summary>
		[JsonIgnore]
		public string Organization{ get; set; }

		/// <summary>
		/// The request id of this API call, as reported in the response headers.  This may be useful for troubleshooting or when contacting OpenAI support in reference to a specific request.
		/// </summary>
		[JsonIgnore]
		public string RequestId { get; set; }


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
		public List<double> TokenLogprobs { get; set; }

		[JsonProperty("top_logprobs")]
		public IList<IDictionary<string, double>> TopLogprobs { get; set; }

		[JsonProperty("text_offset")]
		public List<int> TextOffsets { get; set; }
	}

}
