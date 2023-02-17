using Microsoft.Extensions.Logging;
using OpenAI_API.Completions;
using OpenAI_API.Embedding;
using OpenAI_API.Files;
using OpenAI_API.Models;
using System.Net.Http;

namespace OpenAI_API
{
	/// <summary>
	/// Entry point to the OpenAPI API, handling auth and allowing access to the various API endpoints
	/// </summary>
	public class OpenAIAPI: IOpenAI
    {
		/// <summary>
		/// Base url for OpenAI
		/// </summary>
		public string ApiUrlBase = "https://api.openai.com/v1/";

        /// <summary>The HTTP client configured with the authentication headers.</summary>
        internal HttpClient Client { get; }

        private OpenAIAPI()
        {
            this.Completions = new CompletionEndpoint(this);
            this.Models = new ModelsEndpoint(this);
            this.Files = new FilesEndpoint(this);
            this.Embeddings = new EmbeddingEndpoint(this);
        }

        /// <summary>
        /// Creates a new entry point to the OpenAPI API, handling auth and allowing access to the various API endpoints
        /// </summary>
        /// <param name="httpClient">The HTTP client configured with the authentication headers.</param>
        public OpenAIAPI(HttpClient httpClient) : this() =>
            this.Client = httpClient;

        /// <summary>
        /// Creates a new entry point to the OpenAPI API, handling auth and allowing access to the various API endpoints
        /// </summary>
        /// <param name="auth">The authentication details for the API</param>
        [Obsolete("""
        This constructor will generate a new HTTP client every time it is called.
        Do not use this in scenarios where multiple instances of the API are required.
        This is provided for backwards compatibility, use .NET Dependency Injection instead.
        """, false)]
        public OpenAIAPI(APIAuthentication auth) : 
            this(EndpointBase.ConfigureClient(new HttpClient(), auth)) { }

        /// <summary>
        /// Creates a new entry point to the OpenAPI API, handling auth and allowing access to the various API endpoints
        /// </summary>
        /// <param name="key">The authentication key for the API</param>
        [Obsolete("""
        This constructor will generate a new HTTP client every time it is called.
        Do not use this in scenarios where multiple instances of the API are required.
        This is provided for backwards compatibility, use .NET Dependency Injection instead.
        """, false)]
        public OpenAIAPI(string key) :
            this(EndpointBase.ConfigureClient(new HttpClient(), new APIAuthentication(key))) { }

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
