using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

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
			this.Api = api;
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
		public static async Task<List<Engine>> GetEnginesAsync(APIAuthentication auth = null)
		{
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", auth.ThisOrDefault().ApiKey);
			client.DefaultRequestHeaders.Add("User-Agent", "okgodoit/dotnet_openai_api");

			var response = await client.GetAsync(@"https://api.openai.com/v1/engines");
			string resultAsString = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				var engines = JsonConvert.DeserializeObject<JsonHelperRoot>(resultAsString).data;
				return engines;
			}
			else
			{
				throw new HttpRequestException("Error calling OpenAi API to get list of engines.  HTTP status code: " + response.StatusCode.ToString() + ". Content: " + resultAsString);
			}
		}

		/// <summary>
		/// Get details about a particular Engine from the API, specifically properties such as <see cref="Engine.Owner"/> and <see cref="Engine.Ready"/>
		/// </summary>
		/// <param name="id">The id/name of the engine to get more details about</param>
		/// <param name="auth">API authentication in order to call the API endpoint.  If not specified, attempts to use a default.</param>
		/// <returns>Asynchronously returns the <see cref="Engine"/> with all available properties</returns>
		public static async Task<Engine> RetrieveEngineDetailsAsync(string id, APIAuthentication auth = null)
		{
			if (auth.ThisOrDefault()?.ApiKey is null)
			{
				throw new AuthenticationException("You must provide API authentication.  Please refer to https://github.com/OkGoDoIt/OpenAI-API-dotnet#authentication for details.");
			}

			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", auth.ThisOrDefault().ApiKey);
			client.DefaultRequestHeaders.Add("User-Agent", "okgodoit/dotnet_openai_api");

			var response = await client.GetAsync(@"https://api.openai.com/v1/engines/" + id);
			if (response.IsSuccessStatusCode)
			{
				string resultAsString = await response.Content.ReadAsStringAsync();
				var engine = JsonConvert.DeserializeObject<Engine>(resultAsString);
				return engine;
			}
			else
			{
				throw new HttpRequestException("Error calling OpenAi API to get engine details.  HTTP status code: " + response.StatusCode.ToString());
			}
		}

		/// <summary>
		/// A helper class to deserialize the JSON API responses.  This should not be used directly.
		/// </summary>
		private class JsonHelperRoot
		{
			[JsonProperty("data")]
			public List<Engine> data { get; set; }
			[JsonProperty("object")]
			public string obj { get; set; }

		}
	}
}
