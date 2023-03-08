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
	}

	/// <summary>
	/// Data returned from the Image API.
	/// </summary>
	public class Data
	{
		/// <summary>
		/// The url of the image.
		/// </summary>
		[JsonProperty("url")]

		public string Url { get; set; }

	}
}
