using System;
using Newtonsoft.Json;

namespace OpenAI_API.Interfaces
{
	/// <summary>
	/// defines OpenAI response metadata (response time etc.)
	/// </summary>
	public interface IOpenAiMetadataResult
	{
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

		/// <summary>
		/// The server-side processing time as reported by the API.  This can be useful for debugging where a delay occurs.
		/// </summary>
		[JsonIgnore]
		public TimeSpan ProcessingTime { get; set; }
	}
}
