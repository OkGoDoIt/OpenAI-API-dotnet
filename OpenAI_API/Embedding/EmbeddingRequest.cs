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
		public EmbeddingRequest(Model model, string input)
		{
			Model = model;
			this.Input = input;
		}

		/// <summary>
		/// Creates a new <see cref="EmbeddingRequest"/> with the specified input and the <see cref="Model.AdaTextEmbedding"/> model.
		/// </summary>
		/// <param name="input">The prompt to transform</param>
		public EmbeddingRequest(string input)
		{
			Model = OpenAI_API.Models.Model.AdaTextEmbedding;
			this.Input = input;
		}
	}
}
