using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API
{

	public class SerachResult
	{

		[JsonProperty("document")]
		public int DocumentIndex { get; set; }

		[JsonProperty("score")]
		public double Score { get; set; }

	}

	public class SearchResponse
	{

		[JsonProperty("data")]
		public List<SerachResult> Results { get; set; }

	}
}
