using System.Threading.Tasks;

namespace OpenAI_API.Images
{
    public interface IImageEditEndpoint
    {
        Task<ImageResult> EditImageAsync(byte[] image, string prompt, byte[] mask, int? numberOfImages = null, ImageSize size = null, string user = null);
        Task<ImageResult> EditImageAsync(ImageEditRequest request);
    }
}