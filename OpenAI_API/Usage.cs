using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API
{
	/// <summary>
	/// Usage statistics of how many tokens have been used for this request.
	/// </summary>
	public class Usage
	{
		/// <summary>
		/// How many tokens did the prompt consist of
		/// </summary>
		[JsonProperty("prompt_tokens")]
		public int PromptTokens { get; set; }

		/// <summary>
		/// How many tokens did the request consume total
		/// </summary>
		[JsonProperty("total_tokens")]
		public int TotalTokens { get; set; }

	}
}
