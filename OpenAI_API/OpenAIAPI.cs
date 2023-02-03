using OpenAI_API.Completions;
using OpenAI_API.Embedding;
using OpenAI_API.Files;
using OpenAI_API.Models;

namespace OpenAI_API
{
	/// <summary>
	/// Entry point to the OpenAPI API, handling auth and allowing access to the various API endpoints
	/// </summary>
	public class OpenAIAPI
	{
		/// <summary>
		/// Base url for OpenAI
		/// </summary>
		public string ApiUrlBase = "https://api.openai.com/v1/";
		
		/// <summary>
		/// The API authentication information to use for API calls
		/// </summary>
		public APIAuthentication Auth { get; set; }

		/// <summary>
		/// Creates a new entry point to the OpenAPI API, handling auth and allowing access to the various API endpoints
		/// </summary>
		/// <param name="apiKeys">The API authentication information to use for API calls, or <see langword="null"/> to attempt to use the <see cref="APIAuthentication.Default"/>, potentially loading from environment vars or from a config file.</param>
		public OpenAIAPI(APIAuthentication apiKeys = null)
		{
			this.Auth = apiKeys.ThisOrDefault();
			Completions = new CompletionEndpoint(this);
			Models = new ModelsEndpoint(this);
			Files = new FilesEndpoint(this);
			Embeddings = new EmbeddingEndpoint(this);
		}

		/// <summary>
		/// Text generation is the core function of the API. You give the API a prompt, and it generates a completion. The way you “program” the API to do a task is by simply describing the task in plain english or providing a few written examples. This simple approach works for a wide range of use cases, including summarization, translation, grammar correction, question answering, chatbots, composing emails, and much more (see the prompt library for inspiration).
		/// </summary>
		public CompletionEndpoint Completions { get; }

		/// <summary>
		/// The API lets you transform text into a vector (list) of floating point numbers. The distance between two vectors measures their relatedness. Small distances suggest high relatedness and large distances suggest low relatedness.
		/// </summary>
		public EmbeddingEndpoint Embeddings { get; }

		/// <summary>
		/// The API endpoint for querying available Engines/models
		/// </summary>
		public ModelsEndpoint Models { get; }

		/// <summary>
		/// The API lets you do operations with files. You can upload, delete or retrieve files. Files can be used for fine-tuning, search, etc.
		/// </summary>
		public FilesEndpoint Files { get; }



	}
}
