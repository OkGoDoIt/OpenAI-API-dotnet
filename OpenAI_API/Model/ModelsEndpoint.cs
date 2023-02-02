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
	/// The API endpoint for querying available models
	/// </summary>
	public class ModelsEndpoint
	{
		OpenAIAPI Api;

		/// <summary>
		/// Constructor of the api endpoint.  Rather than instantiating this yourself, access it through an instance of <see cref="OpenAIAPI"/> as <see cref="OpenAIAPI.Models"/>.
		/// </summary>
		/// <param name="api"></param>
		internal ModelsEndpoint(OpenAIAPI api)
		{
			this.Api = api;
		}

		/// <summary>
		/// List all models via the API
		/// </summary>
		/// <returns>Asynchronously returns the list of all <see cref="Model"/>s</returns>
		public Task<List<Model>> GetModelsAsync()
		{
			return GetModelsAsync(Api?.Auth);
		}

		/// <summary>
		/// Get details about a particular Model from the API, specifically properties such as <see cref="Model.OwnedBy"/> and permissions.
		/// </summary>
		/// <param name="id">The id/name of the model to get more details about</param>
		/// <returns>Asynchronously returns the <see cref="Model"/> with all available properties</returns>
		public Task<Model> RetrieveModelDetailsAsync(string id)
		{
			return RetrieveModelDetailsAsync(id, Api?.Auth);
		}

		/// <summary>
		/// List all models via the API
		/// </summary>
		/// <param name="auth">API authentication in order to call the API endpoint.  If not specified, attempts to use a default.</param>
		/// <returns>Asynchronously returns the list of all <see cref="Model"/>s</returns>
		public static async Task<List<Model>> GetModelsAsync(APIAuthentication auth = null)
		{
            if (auth.ThisOrDefault()?.ApiKey is null)
            {
                throw new AuthenticationException("You must provide API authentication.  Please refer to https://github.com/OkGoDoIt/OpenAI-API-dotnet#authentication for details.");
            }
            HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", auth.ThisOrDefault().ApiKey);
			client.DefaultRequestHeaders.Add("User-Agent", "okgodoit/dotnet_openai_api");
            if (!string.IsNullOrEmpty(auth.ThisOrDefault().OpenAIOrganization)) client.DefaultRequestHeaders.Add("OpenAI-Organization", auth.ThisOrDefault().OpenAIOrganization);

			var response = await client.GetAsync(@"https://api.openai.com/v1/models");
			string resultAsString = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				var models = JsonConvert.DeserializeObject<JsonHelperRoot>(resultAsString).data;
				return models;
			}
			else
			{
				throw new HttpRequestException("Error calling OpenAi API to get list of models.  HTTP status code: " + response.StatusCode.ToString() + ". Content: " + resultAsString);
			}
		}

        /// <summary>
        /// Get details about a particular Model from the API, specifically properties such as <see cref="Model.OwnedBy"/> and permissions.
        /// </summary>
        /// <param name="id">The id/name of the model to get more details about</param>
        /// <param name="auth">API authentication in order to call the API endpoint.  If not specified, attempts to use a default.</param>
        /// <returns>Asynchronously returns the <see cref="Model"/> with all available properties</returns>
        public static async Task<Model> RetrieveModelDetailsAsync(string id, APIAuthentication auth = null)
		{
			if (auth.ThisOrDefault()?.ApiKey is null)
			{
				throw new AuthenticationException("You must provide API authentication.  Please refer to https://github.com/OkGoDoIt/OpenAI-API-dotnet#authentication for details.");
			}

			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", auth.ThisOrDefault().ApiKey);
			client.DefaultRequestHeaders.Add("User-Agent", "okgodoit/dotnet_openai_api");
            if (!string.IsNullOrEmpty(auth.ThisOrDefault().OpenAIOrganization)) client.DefaultRequestHeaders.Add("OpenAI-Organization", auth.ThisOrDefault().OpenAIOrganization);

            var response = await client.GetAsync(@"https://api.openai.com/v1/models/" + id);
			if (response.IsSuccessStatusCode)
			{
				string resultAsString = await response.Content.ReadAsStringAsync();
				var model = JsonConvert.DeserializeObject<Model>(resultAsString);
				return model;
			}
			else
			{
				throw new HttpRequestException("Error calling OpenAi API to get model details.  HTTP status code: " + response.StatusCode.ToString());
			}
		}

		/// <summary>
		/// A helper class to deserialize the JSON API responses.  This should not be used directly.
		/// </summary>
		private class JsonHelperRoot
		{
			[JsonProperty("data")]
			public List<Model> data { get; set; }
			[JsonProperty("object")]
			public string obj { get; set; }

		}
	}
}
