using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_API.Images
{
    /// <summary>
    /// Interface for the Image Edit endpoint
    /// </summary>
    public class ImageEditEndpoint : EndpointBase, IImageEditEndpoint
    {
        /// <summary>
        /// The name of the endpoint, which is the final path segment in the API URL.  For example, "image".
        /// </summary>
        protected override string Endpoint { get { return "images/edits"; } }

        /// <summary>
        /// Constructor of the api endpoint.  Rather than instantiating this yourself, access it through an instance of <see cref="OpenAIAPI"/> as <see cref="OpenAIAPI.ImageGenerations"/>.
        /// </summary>
        /// <param name="api"></param>
        internal ImageEditEndpoint(OpenAIAPI api) : base(api) { }

        /// <summary>
        /// Request the API to edit an image.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="prompt"></param>
        /// <param name="mask"></param>
        /// <param name="numberOfImages"></param>
        /// <param name="size"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ImageResult> EditImageAsync(byte[] image, string prompt, byte[] mask, int? numberOfImages = null, ImageSize size = null, string user = null)
        {
            ImageEditRequest req = new ImageEditRequest(image, prompt, mask, numberOfImages, size, user, model: ImageModel.Dalle2);
            return await EditImageAsync(req);
        }

        /// <summary>
        /// Request the API to edit an image.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ImageResult> EditImageAsync(ImageEditRequest request)
        {
            var content = request.GetMultipartFormDataContent();
            return await HttpPost<ImageResult>(postData: content);
        }
    }
}
