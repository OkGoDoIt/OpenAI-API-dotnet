using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace OpenAI_API.Embedding
{
	/// <summary>
	/// Represents an embedding result returned by the Embedding API.  
	/// </summary>
	public class EmbeddingResult : ApiResultBase
	{
		/// <summary>
		/// List of results of the embedding
		/// </summary>
		[JsonProperty("data")]
		public List<Data> Data { get; set; }

		/// <summary>
		/// Usage statistics of how many tokens have been used for this request
		/// </summary>
		[JsonProperty("usage")]
		public Usage Usage { get; set; }

		/// <summary>
		/// Allows an EmbeddingResult to be implicitly cast to the array of floats repsresenting the first ebmedding result
		/// </summary>
		/// <param name="embeddingResult">The <see cref="EmbeddingResult"/> to cast to an array of floats.</param>
		public static implicit operator float[](EmbeddingResult embeddingResult)
		{
			return embeddingResult.Data.FirstOrDefault()?.Embedding;
		}
	}

	/// <summary>
	/// Data returned from the Embedding API.
	/// </summary>
	public class Data
	{
		/// <summary>
		/// Type of the response. In case of Data, this will be "embedding"  
		/// </summary>
		[JsonProperty("object")]

		public string Object { get; set; }

		/// <summary>
		/// The input text represented as a vector (list) of floating point numbers
		/// </summary>
		[JsonProperty("embedding")]
		public float[] Embedding { get; set; }

		/// <summary>
		/// Index
		/// </summary>
		[JsonProperty("index")]
		public int Index { get; set; }

	}

}
