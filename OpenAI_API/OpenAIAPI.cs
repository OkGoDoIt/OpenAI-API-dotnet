using OpenAI_API.Chat;
using OpenAI_API.Completions;
using OpenAI_API.Embedding;
using OpenAI_API.Files;
using OpenAI_API.Images;
using OpenAI_API.Models;
using OpenAI_API.Moderation;
using System.Xml.Linq;

namespace OpenAI_API
{
	/// <summary>
	/// Entry point to the OpenAPI API, handling auth and allowing access to the various API endpoints
	/// </summary>
	public class OpenAIAPI : IOpenAIAPI
	{
		/// <summary>
		/// Base url for OpenAI
		/// for OpenAI, should be "https://api.openai.com/{0}/{1}"
		/// for Azure, should be "https://(your-resource-name.openai.azure.com/openai/deployments/(deployment-id)/{1}?api-version={0}"
		/// </summary>
		public string ApiUrlFormat { get; set; } = "https://api.openai.com/{0}/{1}";

		/// <summary>
		/// Version of the Rest Api
		/// </summary>
		public string ApiVersion { get; set; } = "v1";

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
			Chat = new ChatEndpoint(this);
			Moderation = new ModerationEndpoint(this);
			ImageGenerations = new ImageGenerationEndpoint(this);
		}

		/// <summary>
		/// Instantiates a version of the API for connecting to the Azure OpenAI endpoint instead of the main OpenAI endpoint.
		/// </summary>
		/// <param name="YourResourceName">The name of your Azure OpenAI Resource</param>
		/// <param name="deploymentId">The name of your model deployment. You're required to first deploy a model before you can make calls.</param>
		/// <param name="apiKey">The API authentication information to use for API calls, or <see langword="null"/> to attempt to use the <see cref="APIAuthentication.Default"/>, potentially loading from environment vars or from a config file.  Currently this library only supports the api-key flow, not the AD-Flow.</param>
		/// <returns></returns>
		public static OpenAIAPI ForAzure(string YourResourceName, string deploymentId, APIAuthentication apiKey = null)
		{
			OpenAIAPI api = new OpenAIAPI(apiKey);
			api.ApiVersion = "2022-12-01";
			api.ApiUrlFormat = $"https://{YourResourceName}.openai.azure.com/openai/deployments/{deploymentId}/" + "{1}?api-version={0}";
			return api;
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
		/// Text generation in the form of chat messages. This interacts with the ChatGPT API.
		/// </summary>
		public ChatEndpoint Chat { get; }

		/// <summary>
		/// Classify text against the OpenAI Content Policy.
		/// </summary>
		public ModerationEndpoint Moderation { get; }

		/// <summary>
		/// The API endpoint for querying available Engines/models
		/// </summary>
		public ModelsEndpoint Models { get; }

		/// <summary>
		/// The API lets you do operations with files. You can upload, delete or retrieve files. Files can be used for fine-tuning, search, etc.
		/// </summary>
		public FilesEndpoint Files { get; }

		/// <summary>
		/// The API lets you do operations with images. You can Given a prompt and/or an input image, the model will generate a new image.
		/// </summary>
		public ImageGenerationEndpoint ImageGenerations { get; }

	}
}
