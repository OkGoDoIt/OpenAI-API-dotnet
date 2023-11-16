using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace OpenAI_API.Images
{
    public class ImageEditRequest
    {
        /// <summary>
        /// PNG image, less than 4MB, square, to be edited.
        /// </summary>
        public byte[] Image { get; set; }

        /// <summary>
        /// A text description of the desired image(s). The maximum length is 1000 characters.
        /// </summary>
        public string Prompt { get; set; }

        /// <summary>
        /// PNG format Mask image with transparency indicating where the image will be edited.
        /// </summary>
        public byte[] Mask { get; set; }

        /// <summary>
        /// The model to use for generating the images.  Defaults to <see cref="ImageModel.Dalle2"/>.  See <see cref="ImageModel"/> for more information.
        /// </summary>
        public ImageModel Model { get; set; }

        /// <summary>
        /// How many different choices to request for each prompt.  Defaults to 1.
        /// </summary>
        public int? NumOfImages { get; set; } = 1;

        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse. Optional.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// The size of the generated images. Must be one of 256x256, 512x512, or 1024x1024. Dall-e 3 expands this to 1792x1024 and 1024x1792. Defauls to 1024x1024
        /// </summary>
        public ImageSize Size { get; set; }

        /// <summary>
        /// The format in which the generated images are returned. Must be one of url or b64_json. Defaults to Url.
        /// </summary>
        public ImageResponseFormat ResponseFormat { get; set; }

        /// <summary>
        /// Cretes a new, empty <see cref="ImageGenerationRequest"/>
        /// </summary>
        public ImageEditRequest()
        {

        }

        /// <summary>
        /// Creates a new <see cref="ImageEditRequest"/> with the specified parameters
        /// </summary>
        /// <param name="prompt">A text description of the desired image(s). The maximum length is 1000 characters.</param>
        /// <param name="numOfImages">How many different choices to request for each prompt.  Defaults to 1.</param>
        /// <param name="size">The size of the generated images. Must be one of 256x256, 512x512, or 1024x1024.</param>
        /// <param name="user">A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.</param>
        /// <param name="responseFormat">The format in which the generated images are returned. Must be one of url or b64_json.</param>
        public ImageEditRequest(
            byte[] image,
            string prompt,
            byte[] mask = null,
            int? numOfImages = 1,
            ImageSize size = null,
            string user = null,
            ImageResponseFormat responseFormat = null,
            ImageModel model = null)
        {
            this.Image = image;
            this.Prompt = prompt;
            this.NumOfImages = numOfImages;
            this.Mask = mask;
            this.User = user;
            this.Size = size ?? ImageSize._1024;
            this.ResponseFormat = responseFormat ?? ImageResponseFormat.Url;
            this.Model = model ?? ImageModel.Dalle2;
        }

        /// <summary>
        /// Provides a <see cref="MultipartFormDataContent"/> object with the appropriate parameters
        /// </summary>
        /// <returns></returns>
        public MultipartFormDataContent GetMultipartFormDataContent()
        {
            var content = new MultipartFormDataContent();

            content.Add(new ByteArrayContent(Image), "image", "image.png");
            content.Add(new StringContent(Prompt), "prompt");
            if (NumOfImages != null)
            {
                content.Add(new StringContent(NumOfImages.ToString()), "n");
            }
            if (Mask != null && Mask.Length > 0)
            {
                content.Add(new ByteArrayContent(Mask), "mask", "mask.png");
            }
            if (Model != null)
            {
                content.Add(new StringContent(Model.ToString()), "model");
            }
            if (Size != null)
            {
                content.Add(new StringContent(Size.ToString()), "size");
            }
            if (ResponseFormat != null)
            {
                content.Add(new StringContent(ResponseFormat.ToString()), "response_format");
            }
            if (!string.IsNullOrWhiteSpace(User))
            {
                content.Add(new StringContent(User), "user");
            }
            return content;
        }
    }
}
