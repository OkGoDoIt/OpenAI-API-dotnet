using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using Newtonsoft.Json;
using OpenAI_API.General;
using OpenAI_API.Interfaces;

namespace OpenAI_API.Helpers
{
	public static class OpenAiRequestHelper
	{
		public static void CheckApiKey(string apiKey)
		{
			if (apiKey == null)
				throw new AuthenticationException("You must provide API authentication.  Please refer to https://github.com/OkGoDoIt/OpenAI-API-dotnet#authentication for details.");
		}

		public static HttpClient GetHttpClient(string apiKey)
		{
			//TODO: try using IHttpClientFactory https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.1#use-ihttpclientfactory-in-a-console-app
			// might be useful: https://stackoverflow.com/questions/50028005/passing-ihttpclientfactory-to-net-standard-class-library

			HttpClient client = new HttpClient();
			AddDefaultHeaders(client.DefaultRequestHeaders, apiKey);
			return client;
		}

		public static void AddDefaultHeaders(HttpRequestHeaders headers, string apiKey)
		{
			headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey); ;
			headers.Add("User-Agent", Constants.Requests.UserAgent);
		}

		public static string GetJsonContent(this IOpenAiRequest request)
		{
			string jsonContent = JsonConvert.SerializeObject(request, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
			return jsonContent;
		}

		public static StringContent GetStringContent(this string jsonContent)
		{
			var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
			return stringContent;
		}
	}
}
