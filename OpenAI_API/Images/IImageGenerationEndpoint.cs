using System.Threading.Tasks;

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
		/// <returns>Asynchronously returns the image result. Look in its <see cref="Data.Url"/> </returns>
		Task<ImageResult> CreateImageAsync(string input);
	}
}