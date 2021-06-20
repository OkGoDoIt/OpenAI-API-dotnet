using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

using OpenAI_API.Dto;
using OpenAI_API.Interfaces;

namespace OpenAI_API.Helpers
{
	/// <summary>
	/// provides helper method for processing OpenAI responses
	/// </summary>
	public static class OpenAiResponseHelper
	{
		/// <summary>
		/// checks if OpenAI has responded with error status code and raises a <see cref="HttpRequestException"/> with a meaningful exception
		/// </summary>
		/// <param name="response">HTTP response message</param>
		/// <param name="requestJsonContent">JSON content of the request</param>
		/// <returns></returns>
		public static async Task CheckForServerError(HttpResponseMessage response, string requestJsonContent)
		{
			if (response.IsSuccessStatusCode)
				return;

			string resultAsString = await response.Content.ReadAsStringAsync();
			var errorRes = JsonConvert.DeserializeObject<OpenAiErrorDto>(resultAsString);
			throw new HttpRequestException(
				$"Error calling OpenAi API. HTTP status code: {response.StatusCode}"
				+ $". Request body: {requestJsonContent}. Error: {errorRes}");
		}

		/// <summary>
		/// fetches OpenAI metadata from the HTTP response and feeds the result
		/// </summary>
		/// <param name="res"></param>
		/// <param name="response"></param>
		public static void FillCompletionResultMetadata(IOpenAiMetadataResult res, HttpResponseMessage response)
		{
			res.Organization = response.Headers.GetValues("Openai-Organization").FirstOrDefault();
			res.RequestId = response.Headers.GetValues("X-Request-ID").FirstOrDefault();

			string processingTimeStr = response.Headers.GetValues("Openai-Processing-Ms").FirstOrDefault();
			if (int.TryParse(processingTimeStr, out int processingTime))
				res.ProcessingTime = TimeSpan.FromMilliseconds(processingTime);
		}
	}
}
