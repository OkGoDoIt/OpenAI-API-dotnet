using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_API.Images
{
    /// <summary>
	/// Given a prompt and/or an input image, the model will generate a new image.
	/// </summary>
    public class ImageEndpoint : EndpointBase
    {
		/// <summary>
		/// This allows you to send request to the recommended model without needing to specify.
		/// </summary>
		public ImageRequest DefaultImageRequestArgs { get; set; } = new ImageRequest() {};

		/// <summary>
		/// The name of the endpoint, which is the final path segment in the API URL.  For example, "image".
		/// </summary>
		protected override string Endpoint { get { return "images/generations"; } }

		/// <summary>
		/// Constructor of the api endpoint.  Rather than instantiating this yourself, access it through an instance of <see cref="OpenAIAPI"/> as <see cref="OpenAIAPI.Images"/>.
		/// </summary>
		/// <param name="api"></param>
		internal ImageEndpoint(OpenAIAPI api) : base(api) { }

		/// <summary>
		/// Ask the API to Creates an image given a prompt.
		/// </summary>
		/// <param name="input">A text description of the desired image(s)</param>
		/// <returns>Asynchronously returns the image result. Look in its <see cref="Data.Url"/> </returns>
		public async Task<ImageResult> CreateImageAsync(string input)
		{
			ImageRequest req = new ImageRequest(prompt: input);
			return await CreateImageAsync(req);
		}

		/// <summary>
		/// Ask the API to Creates an image given a prompt.
		/// </summary>
		/// <param name="request">Request to be send</param>
		/// <returns>Asynchronously returns the image result. Look in its <see cref="Data.Url"/> </returns>
		public async Task<ImageResult> CreateImageAsync(ImageRequest request)
		{
			return await HttpPost<ImageResult>(postData: request);
		}
	}
}
