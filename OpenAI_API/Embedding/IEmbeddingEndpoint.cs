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
        /// This allows you to send request to the recommended model without needing to specify. Every request uses the <see cref="Model.AdaTextEmbedding"/> model
        /// </summary>
        EmbeddingRequest DefaultEmbeddingRequestArgs { get; set; }

        /// <summary>
        /// Ask the API to embedd text using the default embedding model <see cref="Model.AdaTextEmbedding"/>
        /// </summary>
        /// <param name="input">Text to be embedded</param>
        /// <returns>Asynchronously returns the embedding result. Look in its <see cref="Data.Embedding"/> property of <see cref="EmbeddingResult.Data"/> to find the vector of floating point numbers</returns>
        Task<EmbeddingResult> CreateEmbeddingAsync(string input);

        /// <summary>
        /// Ask the API to embedd text using a custom request
        /// </summary>
        /// <param name="request">Request to be send</param>
        /// <returns>Asynchronously returns the embedding result. Look in its <see cref="Data.Embedding"/> property of <see cref="EmbeddingResult.Data"/> to find the vector of floating point numbers</returns>
        Task<EmbeddingResult> CreateEmbeddingAsync(EmbeddingRequest request);

        /// <summary>
        /// Ask the API to embedd text using the default embedding model <see cref="Model.AdaTextEmbedding"/>
        /// </summary>
        /// <param name="input">Text to be embedded</param>
        /// <returns>Asynchronously returns the first embedding result as an array of floats.</returns>
        Task<float[]> GetEmbeddingsAsync(string input);
    }
}