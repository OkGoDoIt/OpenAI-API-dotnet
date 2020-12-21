using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_API
{
	/// <summary>
	/// Entry point to the OpenAPI API, handling auth and allowing access to the various API endpoints
	/// </summary>
	public class OpenAIAPI
	{
		/// <summary>
		/// The API authentication information to use for API calls
		/// </summary>
		public APIAuthentication Auth { get; set; }

		/// <summary>
		/// Specifies which <see cref="Engine"/>/model to use for API calls
		/// </summary>
		public Engine UsingEngine { get; set; } = Engine.Default;

		/// <summary>
		/// Creates a new entry point to the OpenAPI API, handling auth and allowing access to the various API endpoints
		/// </summary>
		/// <param name="apiKeys">The API authentication information to use for API calls, or <see langword="null"/> to attempt to use the <see cref="APIAuthentication.Default"/>, potentially loading from environment vars or from a config file.</param>
		/// <param name="engine">The <see cref="Engine"/>/model to use for API calls, defaulting to <see cref="Engine.Davinci"/> if not specified.</param>
		public OpenAIAPI(APIAuthentication apiKeys = null, Engine engine = null)
		{
			this.Auth = apiKeys.ThisOrDefault();
			this.UsingEngine = engine ?? Engine.Default;
			Completions = new CompletionEndpoint(this);
			Engines = new EnginesEndpoint(this);
			Search = new SearchEndpoint(this);
		}

		/// <summary>
		/// Text generation is the core function of the API. You give the API a prompt, and it generates a completion. The way you “program” the API to do a task is by simply describing the task in plain english or providing a few written examples. This simple approach works for a wide range of use cases, including summarization, translation, grammar correction, question answering, chatbots, composing emails, and much more (see the prompt library for inspiration).
		/// </summary>
		public CompletionEndpoint Completions { get; }

		/// <summary>
		/// The API endpoint for querying available Engines/models
		/// </summary>
		public EnginesEndpoint Engines { get; }

		/// <summary>
		/// The API lets you do semantic search over documents. This means that you can provide a query, such as a natural language question or a statement, and find documents that answer the question or are semantically related to the statement. The “documents” can be words, sentences, paragraphs or even longer documents. For example, if you provide documents "White House", "hospital", "school" and query "the president", you’ll get a different similarity score for each document. The higher the similarity score, the more semantically similar the document is to the query (in this example, “White House” will be most similar to “the president”).
		/// </summary>
		public SearchEndpoint Search { get; }





	}
}
