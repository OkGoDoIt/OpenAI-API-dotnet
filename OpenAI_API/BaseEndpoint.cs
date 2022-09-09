using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_API
{
	public abstract class BaseEndpoint
	{
		protected readonly OpenAIAPI api;

		public BaseEndpoint(OpenAIAPI api)
		{
			this.api = api;
		}

		protected abstract string GetEndpoint();

		protected string GetUrl()
		{
			return $"{OpenAIAPI.API_URL}{GetEndpoint()}";
		}

		protected HttpClient GetClient()
		{
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", api.Auth?.ThisOrDefault().ApiKey);
			client.DefaultRequestHeaders.Add("User-Agent", "okgodoit/dotnet_openai_api");
			return client;
		}

		protected string GetErrorMessage(string resultAsString, HttpResponseMessage response, string name, string description = "")
		{
			return $"Error at {name} ( {description} ) with  HTTP status code: {response.StatusCode} . Content: {resultAsString}";
		}


	}
}
