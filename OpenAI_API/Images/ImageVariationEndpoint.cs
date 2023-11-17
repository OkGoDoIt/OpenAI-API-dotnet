using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_API.Images
{
    /// <summary>
    /// Interface for the Image Variation endpoint
    /// </summary>
    public class ImageVariationEndpoint : EndpointBase, IImageVariationEndpoint
    {
        /// <summary>
        /// The name of the endpoint, which is the final path segment in the API URL.  For example, "image".
        /// </summary>
        protected override string Endpoint { get { return "images/variations"; } }

        /// <summary>
        /// Constructor of the api endpoint.  Rather than instantiating this yourself, access it through an instance of <see cref="OpenAIAPI"/> as <see cref="OpenAIAPI.ImageGenerations"/>.
        /// </summary>
        /// <param name="api"></param>
        internal ImageVariationEndpoint(OpenAIAPI api) : base(api) { }

        /// <summary>
        /// Request the API to vary an image.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="numberOfImages"></param>
        /// <param name="size"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ImageResult> VaryImageAsync(byte[] image, int? numberOfImages = null, ImageSize size = null, string user = null)
        {
            ImageVariationRequest req = new ImageVariationRequest(image, numberOfImages, size, user, null, ImageModel.Dalle2);
            return await VaryImageAsync(req);
        }

        /// <summary>
        /// Request the API to vary an image.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ImageResult> VaryImageAsync(ImageVariationRequest request)
        {
            var content = request.GetMultipartFormDataContent();
            return await HttpPost<ImageResult>(postData: content);
        }
    }
}
