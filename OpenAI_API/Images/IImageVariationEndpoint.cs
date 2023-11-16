using System.Threading.Tasks;

namespace OpenAI_API.Images
{
    public interface IImageVariationEndpoint
    {
        Task<ImageResult> VaryImageAsync(byte[] image, int? numberOfImages = null, ImageSize size = null, string user = null);
        Task<ImageResult> VaryImageAsync(ImageVariationRequest request);
    }
}