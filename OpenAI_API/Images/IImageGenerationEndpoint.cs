using System.Threading.Tasks;

namespace OpenAI_API.Images
{
	/// <summary>
	/// An interface for <see cref="ImageGenerationEndpoint"/>, for ease of mock testing, etc
	/// </summary>
	public interface IImageGenerationEndpoint
	{
		Task<ImageResult> CreateImageAsync(ImageGenerationRequest request);
		Task<ImageResult> CreateImageAsync(string input);
	}
}