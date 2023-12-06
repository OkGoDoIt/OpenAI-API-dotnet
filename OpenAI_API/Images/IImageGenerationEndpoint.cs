using System.Threading.Tasks;
using OpenAI_API.Models;

namespace OpenAI_API.Images
{
	/// <summary>
	/// An interface for <see cref="ImageGenerationEndpoint"/>.  Given a prompt, the model will generate a new image.
	/// </summary>
	public interface IImageGenerationEndpoint
	{
		/// <summary>
		/// Ask the API to Creates an image given a prompt.
		/// </summary>
		/// <param name="request">Request to be send</param>
		/// <returns>Asynchronously returns the image result. Look in its <see cref="Data.Url"/> </returns>
		Task<ImageResult> CreateImageAsync(ImageGenerationRequest request);

		/// <summary>
		/// Ask the API to Creates an image given a prompt.
		/// </summary>
		/// <param name="input">A text description of the desired image(s)</param>
		/// <param name="model">The model to use for generating the image.  Defaults to <see cref="OpenAI_API.Models.Model.DALLE2"/>.</param>
		/// <returns>Asynchronously returns the image result. Look in its <see cref="Data.Url"/> </returns>
		Task<ImageResult> CreateImageAsync(string input, Model model = null);
	}
}