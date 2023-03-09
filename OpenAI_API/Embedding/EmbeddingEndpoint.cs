using OpenAI_API.Models;
using System.Threading.Tasks;

namespace OpenAI_API.Embedding
{
	/// <summary>
	/// OpenAI’s text embeddings measure the relatedness of text strings by generating an embedding, which is a vector (list) of floating point numbers. The distance between two vectors measures their relatedness. Small distances suggest high relatedness and large distances suggest low relatedness.
	/// </summary>
	public class EmbeddingEndpoint : EndpointBase, IEmbeddingEndpoint
	{
		/// <summary>
		/// This allows you to send request to the recommended model without needing to specify. Every request uses the <see cref="Model.AdaTextEmbedding"/> model
		/// </summary>
		public EmbeddingRequest DefaultEmbeddingRequestArgs { get; set; } = new EmbeddingRequest() { Model = Model.AdaTextEmbedding };

		/// <summary>
		/// The name of the endpoint, which is the final path segment in the API URL.  For example, "embeddings".
		/// </summary>
		protected override string Endpoint { get { return "embeddings"; } }

		/// <summary>
		/// Constructor of the api endpoint.  Rather than instantiating this yourself, access it through an instance of <see cref="OpenAIAPI"/> as <see cref="OpenAIAPI.Embeddings"/>.
		/// </summary>
		/// <param name="api"></param>
		internal EmbeddingEndpoint(OpenAIAPI api) : base(api) { }

		/// <summary>
		/// Ask the API to embedd text using the default embedding model <see cref="Model.AdaTextEmbedding"/>
		/// </summary>
		/// <param name="input">Text to be embedded</param>
		/// <returns>Asynchronously returns the embedding result. Look in its <see cref="Data.Embedding"/> property of <see cref="EmbeddingResult.Data"/> to find the vector of floating point numbers</returns>
		public async Task<EmbeddingResult> CreateEmbeddingAsync(string input)
		{
			EmbeddingRequest req = new EmbeddingRequest(DefaultEmbeddingRequestArgs.Model, input);
			return await CreateEmbeddingAsync(req);
		}

		/// <summary>
		/// Ask the API to embedd text using a custom request
		/// </summary>
		/// <param name="request">Request to be send</param>
		/// <returns>Asynchronously returns the embedding result. Look in its <see cref="Data.Embedding"/> property of <see cref="EmbeddingResult.Data"/> to find the vector of floating point numbers</returns>
		public async Task<EmbeddingResult> CreateEmbeddingAsync(EmbeddingRequest request)
		{
			return await HttpPost<EmbeddingResult>(postData: request);
		}

		/// <summary>
		/// Ask the API to embedd text using the default embedding model <see cref="Model.AdaTextEmbedding"/>
		/// </summary>
		/// <param name="input">Text to be embedded</param>
		/// <returns>Asynchronously returns the first embedding result as an array of floats.</returns>
		public async Task<float[]> GetEmbeddingsAsync(string input)
		{
			EmbeddingRequest req = new EmbeddingRequest(DefaultEmbeddingRequestArgs.Model, input);
			var embeddingResult = await CreateEmbeddingAsync(req);
			return embeddingResult?.Data?[0]?.Embedding;
		}
	}
}
