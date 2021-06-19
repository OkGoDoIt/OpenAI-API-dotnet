using System;
using System.Linq;
using System.Net.Http;
using OpenAI_API.Interfaces;

namespace OpenAI_API.Helpers
{
	/// <summary>
	/// provides helper method for processing OpenAI responses
	/// </summary>
	public static class OpenAiResponseHelper
	{
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
