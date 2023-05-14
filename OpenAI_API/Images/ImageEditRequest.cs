using Newtonsoft.Json;
using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenAI_API.Images
{
    /// <summary>
	/// Represents a request to the Images API.  Mostly matches the parameters in <see href="https://platform.openai.com/docs/api-reference/images/create">the OpenAI docs</see>, although some have been renamed or expanded into single/multiple properties for ease of use.
	/// </summary>
    public class ImageEditRequest
    {
        /// <summary>
        /// The image to edit. Must be a valid PNG file, less than 4MB, and square. If mask is not provided, image must have transparency, which will be used as the mask.
        /// </summary>
        [JsonProperty("image")]
        public string Image { get; set; }

        /// <summary>
        /// An additional image whose fully transparent areas (e.g. where alpha is zero) indicate where image should be edited. Must be a valid PNG file, less than 4MB, and have the same.
        /// </summary>
        [JsonProperty("mask")]
        public string Mask { get; set; }

        /// <summary>
        /// A text description of the desired image(s). The maximum length is 1000 characters.
        /// </summary>
        [JsonProperty("prompt")]
        public string Prompt { get; set; }


        [JsonProperty("n")]
        public int? NumOfImages { get; set; } = 1;

        /// <summary>
        /// The size of the generated images. Must be one of 256x256, 512x512, or 1024x1024. Defauls to 1024x1024
        /// </summary>
        [JsonProperty("size"), JsonConverter(typeof(ImageSize.ImageSizeJsonConverter))]
        public ImageSize Size { get; set; }

        /// <summary>
        /// The format in which the generated images are returned. Must be one of url or b64_json. Defaults to Url.
        /// </summary>
        [JsonProperty("response_format"), JsonConverter(typeof(ImageResponseFormat.ImageResponseJsonConverter))]
        public ImageResponseFormat ResponseFormat { get; set; }

        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse. Optional.
        /// </summary>
        [JsonProperty("user")]
		public string User { get; set; }

		
		/// <summary>
		/// Cretes a new, empty <see cref="ImageGenerationRequest"/>
		/// </summary>
		public ImageEditRequest()
		{

		}

        /// <summary>
        /// Creates a new <see cref="ImageEditRequest"/> with the specified parameters
        /// </summary>
        /// <param name="image"></param>
        /// <param name="prompt">A text description of the desired image(s). The maximum length is 1000 characters.</param>
        /// <param name="numOfImages">How many different choices to request for each prompt.  Defaults to 1.</param>
        /// <param name="size">The size of the generated images. Must be one of 256x256, 512x512, or 1024x1024.</param>
        /// <param name="user">A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.</param>
        /// <param name="responseFormat">The format in which the generated images are returned. Must be one of url or b64_json.</param>
        public ImageEditRequest(
			string image,
			string prompt,
            int? numOfImages = 1,
			ImageSize size = null,
			string user = null,
			ImageResponseFormat responseFormat = null)
		{
			this.Image = image;
			this.Prompt = prompt;
            this.NumOfImages = numOfImages;
			this.User = user;
			this.Size = size ?? ImageSize._1024;
			this.ResponseFormat = responseFormat ?? ImageResponseFormat.Url;
		}

	}
}
