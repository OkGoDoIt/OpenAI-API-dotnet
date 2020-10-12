using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API
{
	/// <summary>
	/// Used internally to deserialize a result from the Document Search API
	/// </summary>
	public class SearchResult
	{
		/// <summary>
		/// The index of the document as originally supplied
		/// </summary>
		[JsonProperty("document")]
		public int DocumentIndex { get; set; }

		/// <summary>
		/// The relative score of this document
		/// </summary>
		[JsonProperty("score")]
		public double Score { get; set; }

	}

	/// <summary>
	/// Used internally to deserialize the json response from the Document Search API
	/// </summary>
	public class SearchResponse
	{
		/// <summary>
		/// The list of results
		/// </summary>
		[JsonProperty("data")]
		public List<SearchResult> Results { get; set; }

		/// <summary>
		/// The server-side processing time as reported by the API.  This can be useful for debugging where a delay occurs.
		/// </summary>
		[JsonIgnore]
		public TimeSpan ProcessingTime { get; set; }

		/// <summary>
		/// The organization associated with the API request, as reported by the API.
		/// </summary>
		[JsonIgnore]
		public string Organization { get; set; }

		/// <summary>
		/// The request id of this API call, as reported in the response headers.  This may be useful for troubleshooting or when contacting OpenAI support in reference to a specific request.
		/// </summary>
		[JsonIgnore]
		public string RequestId { get; set; }
	}
}
