using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenAI_API.Models
{
	/// <summary>
	/// The API endpoint for querying available models
	/// </summary>
	public class ModelsEndpoint : EndpointBase
	{
		/// <summary>
		/// The name of the endpoint, which is the final path segment in the API URL.  For example, "models".
		/// </summary>
		protected override string Endpoint { get { return "models"; } }

		/// <summary>
		/// Constructor of the api endpoint.  Rather than instantiating this yourself, access it through an instance of <see cref="OpenAIAPI"/> as <see cref="OpenAIAPI.Models"/>.
		/// </summary>
		/// <param name="api"></param>
		internal ModelsEndpoint(OpenAIAPI api) : base(api) { }

		/// <summary>
		/// Get details about a particular Model from the API, specifically properties such as <see cref="Model.OwnedBy"/> and permissions.
		/// </summary>
		/// <param name="id">The id/name of the model to get more details about</param>
		/// <returns>Asynchronously returns the <see cref="Model"/> with all available properties</returns>
		public Task<Model> RetrieveModelDetailsAsync(string id)
		{
			return RetrieveModelDetailsAsync(id, _Api?.Auth);
		}

		/// <summary>
		/// List all models via the API
		/// </summary>
		/// <returns>Asynchronously returns the list of all <see cref="Model"/>s</returns>
		public async Task<List<Model>> GetModelsAsync()
		{
			return (await HttpGet<JsonHelperRoot>()).data;
		}

		/// <summary>
		/// Get details about a particular Model from the API, specifically properties such as <see cref="Model.OwnedBy"/> and permissions.
		/// </summary>
		/// <param name="id">The id/name of the model to get more details about</param>
		/// <param name="auth">API authentication in order to call the API endpoint.  If not specified, attempts to use a default.</param>
		/// <returns>Asynchronously returns the <see cref="Model"/> with all available properties</returns>
		public async Task<Model> RetrieveModelDetailsAsync(string id, APIAuthentication auth = null)
		{
			string resultAsString = await HttpGetContent<JsonHelperRoot>($"{Url}/{id}");
			var model = JsonConvert.DeserializeObject<Model>(resultAsString);
			return model;
		}

		/// <summary>
		/// A helper class to deserialize the JSON API responses.  This should not be used directly.
		/// </summary>
		private class JsonHelperRoot : ApiResultBase
		{
			[JsonProperty("data")]
			public List<Model> data { get; set; }
			[JsonProperty("object")]
			public string obj { get; set; }

		}
	}
}
