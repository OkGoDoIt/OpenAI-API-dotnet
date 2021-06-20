using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;
using OpenAI_API.Helpers;

namespace OpenAI_API
{
	/// <summary>
	/// The API endpoint for querying available Engines/models
	/// </summary>
	public class EnginesEndpoint
	{
		OpenAIAPI Api;

		/// <summary>
		/// Constructor of the api endpoint.  Rather than instantiating this yourself, access it through an instance of <see cref="OpenAIAPI"/> as <see cref="OpenAIAPI.Engines"/>.
		/// </summary>
		/// <param name="api"></param>
		internal EnginesEndpoint(OpenAIAPI api)
		{
			Api = api;
		}

		/// <summary>
		/// List all engines via the API
		/// </summary>
		/// <returns>Asynchronously returns the list of all <see cref="Engine"/>s</returns>
		public Task<List<Engine>> GetEnginesAsync()
		{
			return GetEnginesAsync(Api?.Auth);
		}

		/// <summary>
		/// Get details about a particular Engine from the API, specifically properties such as <see cref="Engine.Owner"/> and <see cref="Engine.Ready"/>
		/// </summary>
		/// <param name="id">The id/name of the engine to get more details about</param>
		/// <returns>Asynchronously returns the <see cref="Engine"/> with all available properties</returns>
		public Task<Engine> RetrieveEngineDetailsAsync(string id)
		{
			return RetrieveEngineDetailsAsync(id, Api?.Auth);
		}

		/// <summary>
		/// List all engines via the API
		/// </summary>
		/// <param name="auth">API authentication in order to call the API endpoint.  If not specified, attempts to use a default.</param>
		/// <returns>Asynchronously returns the list of all <see cref="Engine"/>s</returns>
		private static async Task<List<Engine>> GetEnginesAsync(APIAuthentication auth)
		{
			var client = OpenAiRequestHelper.GetHttpClient(auth.ApiKey);

			var response = await client.GetAsync(@"https://api.openai.com/v1/engines");
			string resultAsString = await response.Content.ReadAsStringAsync();

			await OpenAiResponseHelper.CheckForServerError(response, "");

			var engines = JsonConvert.DeserializeObject<JsonHelperRoot>(resultAsString).Data;
			return engines;
		}

		/// <summary>
		/// Get details about a particular Engine from the API, specifically properties such as <see cref="Engine.Owner"/> and <see cref="Engine.Ready"/>
		/// </summary>
		/// <param name="id">The id/name of the engine to get more details about</param>
		/// <param name="auth">API authentication in order to call the API endpoint.  If not specified, attempts to use a default.</param>
		/// <returns>Asynchronously returns the <see cref="Engine"/> with all available properties</returns>
		public static async Task<Engine> RetrieveEngineDetailsAsync(string id, APIAuthentication auth)
		{
			if (auth.ThisOrDefault()?.ApiKey is null)
			{
				throw new AuthenticationException("You must provide API authentication.  Please refer to https://github.com/OkGoDoIt/OpenAI-API-dotnet#authentication for details.");
			}

			var client = OpenAiRequestHelper.GetHttpClient(auth.ThisOrDefault()?.ApiKey);

			var response = await client.GetAsync(@"https://api.openai.com/v1/engines/" + id);
			await OpenAiResponseHelper.CheckForServerError(response, "");

			string resultAsString = await response.Content.ReadAsStringAsync();
			var engine = JsonConvert.DeserializeObject<Engine>(resultAsString);
			return engine;
		}

		/// <summary>
		/// A helper class to deserialize the JSON API responses.  This should not be used directly.
		/// </summary>
		private class JsonHelperRoot
		{
			[JsonProperty("data")]
			public List<Engine> Data { get; set; }
			[JsonProperty("object")]
			public string Obj { get; set; }

		}
	}
}
