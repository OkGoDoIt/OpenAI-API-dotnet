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
		/// This allows you to send request to a default model without needing to specify for each request.
		/// </summary>
		public EmbeddingRequest DefaultEmbeddingRequestArgs { get; set; } = new EmbeddingRequest() { Model = Model.DefaultEmbeddingModel };

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
		/// Ask the API to embed text using the default embedding model <see cref="Model.DefaultEmbeddingModel"/>
		/// </summary>
		/// <param name="input">Text to be embedded</param>
		/// <returns>Asynchronously returns the embedding result. Look in its <see cref="Data.Embedding"/> property of <see cref="EmbeddingResult.Data"/> to find the vector of floating point numbers</returns>
		public async Task<EmbeddingResult> CreateEmbeddingAsync(string input)
		{
			EmbeddingRequest req = new EmbeddingRequest(DefaultEmbeddingRequestArgs.Model, input);
			return await CreateEmbeddingAsync(req);
		}

		/// <summary>
		/// Ask the API to embed text using a custom request
		/// </summary>
		/// <param name="request">Request to be send</param>
		/// <returns>Asynchronously returns the embedding result. Look in its <see cref="Data.Embedding"/> property of <see cref="EmbeddingResult.Data"/> to find the vector of floating point numbers</returns>
		public async Task<EmbeddingResult> CreateEmbeddingAsync(EmbeddingRequest request)
		{
			return await HttpPost<EmbeddingResult>(postData: request);
		}

		/// <inheritdoc/>
		public async Task<float[]> GetEmbeddingsAsync(string input)
		{
			EmbeddingRequest req = new EmbeddingRequest(DefaultEmbeddingRequestArgs.Model, input);
			var embeddingResult = await CreateEmbeddingAsync(req);
			return embeddingResult?.Data?[0]?.Embedding;
		}

		/// <inheritdoc/>
		public async Task<float[]> GetEmbeddingsAsync(string input, Model model=null, int? dimensions = null)
		{
			EmbeddingRequest req = new EmbeddingRequest(model ?? Model.DefaultEmbeddingModel, input, dimensions);
			var embeddingResult = await CreateEmbeddingAsync(req);
			return embeddingResult?.Data?[0]?.Embedding;
		}
	}
}
