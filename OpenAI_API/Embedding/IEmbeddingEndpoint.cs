using OpenAI_API.Models;
using System.Threading.Tasks;

namespace OpenAI_API.Embedding
{
	/// <summary>
	/// An interface for <see cref="EmbeddingEndpoint"/>, for ease of mock testing, etc
	/// </summary>
	public interface IEmbeddingEndpoint
    {
		/// <summary>
		/// This allows you to send request to a default model without needing to specify for each request
		/// </summary>
		EmbeddingRequest DefaultEmbeddingRequestArgs { get; set; }

		/// <summary>
		/// Ask the API to embed text using the default embedding model <see cref="Model.DefaultEmbeddingModel"/>
		/// </summary>
		/// <param name="input">Text to be embedded</param>
		/// <returns>Asynchronously returns the embedding result. Look in its <see cref="Data.Embedding"/> property of <see cref="EmbeddingResult.Data"/> to find the vector of floating point numbers</returns>
		Task<EmbeddingResult> CreateEmbeddingAsync(string input);

        /// <summary>
        /// Ask the API to embed text using a custom request
        /// </summary>
        /// <param name="request">Request to be send</param>
        /// <returns>Asynchronously returns the embedding result. Look in its <see cref="Data.Embedding"/> property of <see cref="EmbeddingResult.Data"/> to find the vector of floating point numbers</returns>
        Task<EmbeddingResult> CreateEmbeddingAsync(EmbeddingRequest request);

		/// <summary>
		/// Ask the API to embed text <see cref="Model.DefaultEmbeddingModel"/>
		/// </summary>
		/// <param name="input">Text to be embedded</param>
		/// <param name="model">The model to use. You can use <see cref="ModelsEndpoint.GetModelsAsync()"/> to see all of your available models, or use a standard model like <see cref="Model.AdaTextEmbedding"/>.</param>
		/// <param name="dimensions">The number of dimensions the resulting output embeddings should have. Only supported in text-embedding-3 and later models.</param>
		/// <returns>Asynchronously returns the first embedding result as an array of floats.</returns>
		Task<float[]> GetEmbeddingsAsync(string input, Model model = null, int? dimensions = null);
    }
}