using OpenAI_API.Completions;
using OpenAI_API.Embedding;
using OpenAI_API.Files;
using OpenAI_API.Models;

namespace OpenAI_API
{
	/// <summary>
	/// An interface for <see cref="OpenAIAPI"/>, for ease of mock testing, etc
	/// </summary>
	public interface IOpenAIAPI
    {
        /// <summary>
        /// Base url for OpenAI
        /// for OpenAI, should be "https://api.openai.com/{0}/{1}"
        /// for Azure, should be "https://(your-resource-name.openai.azure.com/openai/deployments/(deployment-id)/{1}?api-version={0}"
        /// </summary>
        string ApiUrlFormat { get; set; }

        /// <summary>
        /// Version of the Rest Api
        /// </summary>
        string ApiVersion { get; set; }

        /// <summary>
        /// The API authentication information to use for API calls
        /// </summary>
        APIAuthentication Auth { get; set; }

        /// <summary>
        /// Text generation is the core function of the API. You give the API a prompt, and it generates a completion. The way you “program” the API to do a task is by simply describing the task in plain english or providing a few written examples. This simple approach works for a wide range of use cases, including summarization, translation, grammar correction, question answering, chatbots, composing emails, and much more (see the prompt library for inspiration).
        /// </summary>
        CompletionEndpoint Completions { get; }

        /// <summary>
        /// The API lets you transform text into a vector (list) of floating point numbers. The distance between two vectors measures their relatedness. Small distances suggest high relatedness and large distances suggest low relatedness.
        /// </summary>
        EmbeddingEndpoint Embeddings { get; }

        /// <summary>
        /// The API endpoint for querying available Engines/models
        /// </summary>
        ModelsEndpoint Models { get; }

        /// <summary>
        /// The API lets you do operations with files. You can upload, delete or retrieve files. Files can be used for fine-tuning, search, etc.
        /// </summary>
        FilesEndpoint Files { get; }
    }
}