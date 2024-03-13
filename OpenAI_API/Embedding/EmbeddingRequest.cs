using Newtonsoft.Json;
using OpenAI_API.Models;

namespace OpenAI_API.Embedding
{
	/// <summary>
	/// Represents a request to the Completions API. Matches with the docs at <see href="https://platform.openai.com/docs/api-reference/embeddings">the OpenAI docs</see>
	/// </summary>
	public class EmbeddingRequest
	{
		/// <summary>
		/// ID of the model to use. You can use <see cref="ModelsEndpoint.GetModelsAsync()"/> to see all of your available models, or use a standard model like <see cref="Model.AdaTextEmbedding"/>.
		/// </summary>
		[JsonProperty("model")]
		public string Model { get; set; }

		/// <summary>
		/// Main text to be embedded
		/// </summary>
		[JsonProperty("input")]
		public string Input { get; set; }

		/// <summary>
		/// The number of dimensions the resulting output embeddings should have. Only supported in text-embedding-3 and later models.
		/// </summary>
		[JsonProperty("dimensions", NullValueHandling =NullValueHandling.Ignore)]
		public int? Dimensions { get; set; }

		/// <summary>
		/// Cretes a new, empty <see cref="EmbeddingRequest"/>
		/// </summary>
		public EmbeddingRequest()
		{

		}

		/// <summary>
		/// Creates a new <see cref="EmbeddingRequest"/> with the specified parameters
		/// </summary>
		/// <param name="model">The model to use. You can use <see cref="ModelsEndpoint.GetModelsAsync()"/> to see all of your available models, or use a standard model like <see cref="Model.AdaTextEmbedding"/>.</param>
		/// <param name="input">The prompt to transform</param>
		/// <param name="dimensions">The number of dimensions the resulting output embeddings should have. Only supported in text-embedding-3 and later models.</param>
		public EmbeddingRequest(Model model, string input, int? dimensions = null)
		{
			this.Model = model;
			this.Input = input;
			this.Dimensions = dimensions;
		}

		/// <summary>
		/// Creates a new <see cref="EmbeddingRequest"/> with the specified input and the <see cref="Model.DefaultEmbeddingModel"/> model.
		/// </summary>
		/// <param name="input">The prompt to transform</param>
		public EmbeddingRequest(string input)
		{
			this.Model = OpenAI_API.Models.Model.DefaultEmbeddingModel;
			this.Input = input;
		}
	}
}
