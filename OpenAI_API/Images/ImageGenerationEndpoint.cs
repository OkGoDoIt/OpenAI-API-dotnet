using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_API.Images
{
	/// <summary>
	/// Given a prompt, the model will generate a new image.
	/// </summary>
	public class ImageGenerationEndpoint : EndpointBase, IImageGenerationEndpoint
	{
		/// <summary>
		/// The name of the endpoint, which is the final path segment in the API URL.  For example, "image".
		/// </summary>
		protected override string Endpoint { get { return "images/generations"; } }

		/// <summary>
		/// Constructor of the api endpoint.  Rather than instantiating this yourself, access it through an instance of <see cref="OpenAIAPI"/> as <see cref="OpenAIAPI.ImageGenerations"/>.
		/// </summary>
		/// <param name="api"></param>
		internal ImageGenerationEndpoint(OpenAIAPI api) : base(api) { }

		/// <summary>
		/// Ask the API to Creates an image given a prompt.
		/// </summary>
		/// <param name="input">A text description of the desired image(s)</param>
		/// <param name="model">The model to use for generating the image.  Defaults to <see cref="OpenAI_API.Models.Model.DALLE2"/>.</param>
		/// <returns>Asynchronously returns the image result. Look in its <see cref="Data.Url"/> </returns>
		public async Task<ImageResult> CreateImageAsync(string input, Model model = null)
		{
			ImageGenerationRequest req = new ImageGenerationRequest(prompt: input, model: model);
			return await CreateImageAsync(req);
		}

		/// <summary>
		/// Ask the API to Creates an image given a prompt.
		/// </summary>
		/// <param name="request">Request to be send</param>
		/// <returns>Asynchronously returns the image result. Look in its <see cref="Data.Url"/> </returns>
		public async Task<ImageResult> CreateImageAsync(ImageGenerationRequest request)
		{
			return await HttpPost<ImageResult>(postData: request);
		}
	}
}
