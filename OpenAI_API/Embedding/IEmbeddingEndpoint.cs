using System.Threading.Tasks;

namespace OpenAI_API.Embedding
{
    /// <summary>
    /// OpenAI’s text embeddings measure the relatedness of text strings by generating an embedding, which is a vector (list) of floating point numbers. The distance between two vectors measures their relatedness. Small distances suggest high relatedness and large distances suggest low relatedness.
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