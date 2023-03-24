using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_API.Moderation
{
	/// <summary>
	/// This endpoint classifies text against the OpenAI Content Policy
	/// </summary>
	public class ModerationEndpoint : EndpointBase, IModerationEndpoint
	{
		/// <summary>
		/// This allows you to send request to the recommended model without needing to specify. OpenAI recommends using the <see cref="Model.TextModerationLatest"/> model
		/// </summary>
		public ModerationRequest DefaultModerationRequestArgs { get; set; } = new ModerationRequest() { Model = Model.TextModerationLatest };

		/// <summary>
		/// The name of the endpoint, which is the final path segment in the API URL.  For example, "completions". 
		/// </summary>
		protected override string Endpoint { get { return "moderations"; } }

		/// <summary>
		/// Constructor of the api endpoint. Rather than instantiating this yourself, access it through an instance of <see cref="OpenAIAPI"/> as <see cref="OpenAIAPI.Moderation"/>.
		/// </summary>
		/// <param name="api"></param>
		internal ModerationEndpoint(OpenAIAPI api) : base(api) { }

		/// <summary>
		/// Ask the API to classify the text using the default model.
		/// </summary>
		/// <param name="input">Text to classify</param>
		/// <returns>Asynchronously returns the classification result</returns>
		public async Task<ModerationResult> CallModerationAsync(string input)
		{
			ModerationRequest req = new ModerationRequest(input, DefaultModerationRequestArgs.Model);
			return await CallModerationAsync(req);
		}

		/// <summary>
		/// Ask the API to classify the text using a custom request.
		/// </summary>
		/// <param name="request">Request to send to the API</param>
		/// <returns>Asynchronously returns the classification result</returns>
		public async Task<ModerationResult> CallModerationAsync(ModerationRequest request)
		{
			return await HttpPost<ModerationResult>(postData: request);
		}
	}
}
