using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Images
{
	/// <summary>
	/// Represents an image result returned by the Image API.  
	/// </summary>
	public class ImageResult : ApiResultBase
	{
		/// <summary>
		/// List of results of the embedding
		/// </summary>
		[JsonProperty("data")]
		public List<Data> Data { get; set; }

		/// <summary>
		/// Gets the url or base64-encoded image data of the first result, or null if there are no results
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if (Data?.Count > 0)
			{
				return Data[0].Url ?? Data[0].Base64Data;
			}
			else
			{
				return null;
			}
		}
	}

	/// <summary>
	/// Data returned from the Image API.
	/// </summary>
	public class Data
	{
		/// <summary>
		/// The url of the image result
		/// </summary>
		[JsonProperty("url")]

		public string Url { get; set; }

		/// <summary>
		/// The base64-encoded image data as returned by the API
		/// </summary>
		[JsonProperty("b64_json")]
		public string Base64Data { get; set; }

	}
}
