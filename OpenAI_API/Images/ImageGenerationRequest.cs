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
	public class ImageGenerationRequest
	{
		private int? numOfImages = 1;
		private ImageSize size = ImageSize._1024;
		private string quality = "standard";

		/// <summary>
		/// A text description of the desired image(s). The maximum length is 1000 characters.
		/// </summary>
		[JsonProperty("prompt")]
		public string Prompt { get; set; }

		/// <summary>
		/// How many different choices to request for each prompt.  Defaults to 1.  Only for DALL-E 2.  For DALL-E 3, only 1 is allowed.
		/// </summary>
		[JsonProperty("n")]
		public int? NumOfImages
		{
			get
			{
				if (this.Model == OpenAI_API.Models.Model.DALLE3 && numOfImages != 1)
					throw new ArgumentException("For DALL-E 3, only 1 NumOfImages is allowed.");
				return numOfImages;
			}
			set => numOfImages = value;
		}
		/// <summary>
		/// The model to use for this request.  Defaults to DALL-E 2.
		/// </summary>
		[JsonProperty("model")]
		public string Model { get; set; } = OpenAI_API.Models.Model.DALLE2;

		/// <summary>
		/// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse. Optional.
		/// </summary>
		[JsonProperty("user")]
		public string User { get; set; }

		/// <summary>
		/// The size of the generated images. Defaults to 1024x1024.
		/// </summary>
		[JsonProperty("size"), JsonConverter(typeof(ImageSize.ImageSizeJsonConverter))]
		public ImageSize Size
		{
			get
			{
				if (this.Model == OpenAI_API.Models.Model.DALLE3 && (this.size == ImageSize._256 || this.size == ImageSize._512))
					throw new ArgumentException("For DALL-E 3, only 1024x1024, 1024x1792, or 1792x1024 is allowed.");
				if (this.Model == OpenAI_API.Models.Model.DALLE2 && (this.size == ImageSize._1792x1024 || this.size == ImageSize._1024x1792))
					throw new ArgumentException("For DALL-E 2, only 256x256, 512x512, or 1024x1024 is allowed.");
				return size;
			}
			set => size = value;
		}

		/// <summary>
		/// By default, images are generated at `standard` quality, but when using DALL·E 3 you can set quality to `hd` for enhanced detail. Square, standard quality images are the fastest to generate.
		/// </summary>
		[JsonProperty("quality", NullValueHandling=NullValueHandling.Ignore)]
		public string Quality
		{
			get
			{
				if (this.Model == OpenAI_API.Models.Model.DALLE2 && this.quality == "hd")
					throw new ArgumentException("For DALL-E 2, hd quality is not available.");
				if (this.Model == OpenAI_API.Models.Model.DALLE3 && this.quality == "standard")
					return null;
				return quality;
			}
			set
			{
				switch (value.ToLower().Trim())
				{
					case "standard":
						quality="standard";
						break;
					case "hd":
						quality = "hd";
						break;
					default:
						throw new ArgumentException("Quality must be either 'standard' or 'hd'.");
				}
			}
		}

		/// <summary>
		/// The format in which the generated images are returned. Must be one of url or b64_json. Defaults to Url.
		/// </summary>
		[JsonProperty("response_format"), JsonConverter(typeof(ImageResponseFormat.ImageResponseJsonConverter))]
		public ImageResponseFormat ResponseFormat { get; set; }

		/// <summary>
		/// Cretes a new, empty <see cref="ImageGenerationRequest"/>
		/// </summary>
		public ImageGenerationRequest()
		{

		}

		/// <summary>
		/// Creates a new <see cref="ImageGenerationRequest"/> with the specified parameters
		/// </summary>
		/// <param name="prompt">A text description of the desired image(s). The maximum length is 1000 characters.</param>
		/// <param name="model">The model to use for this request. Defaults to DALL-E 2.</param>
		/// <param name="size">The size of the generated images. Must be one of 256x256, 512x512, or 1024x1024.</param>
		/// <param name="quality">By default, images are generated at `standard` quality, but when using DALL·E 3 you can set quality to `hd` for enhanced detail.</param>
		/// <param name="user">A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.</param>
		/// <param name="responseFormat">The format in which the generated images are returned. Must be one of url or b64_json.</param>
		public ImageGenerationRequest(
			string prompt,
			Model model,
			ImageSize size = null,
			string quality = "standard",
			string user = null,
			ImageResponseFormat responseFormat = null)
		{
			this.Prompt = prompt;
			this.Model = model ?? OpenAI_API.Models.Model.DALLE2;
			this.Quality = quality ?? "standard";
			this.User = user;
			this.Size = size ?? ImageSize._1024;
			this.ResponseFormat = responseFormat ?? ImageResponseFormat.Url;

			// check for incompatible parameters
			if (this.Model == OpenAI_API.Models.Model.DALLE3)
			{
				if (this.Size == ImageSize._256 || this.Size == ImageSize._512)
					throw new ArgumentException("For DALL-E 3, only sizes 1024x1024, 1024x1792, or 1792x1024 are allowed.");
				if (this.quality != "standard" && this.quality != "hd")
					throw new ArgumentException("Quality must be one of 'standard' or 'hd'");
			}
			else
			{
				if (this.Size == ImageSize._1792x1024 || this.Size == ImageSize._1024x1792)
					throw new ArgumentException("For DALL-E 2, only sizes 256x256, 512x512, or 1024x1024 are allowed.");
				if (this.quality != "standard")
					throw new ArgumentException("For DALL-E 2, only 'standard' quality is available");
			}
		}

		/// <summary>
		/// Creates a new <see cref="ImageGenerationRequest"/> with the specified parameters
		/// </summary>
		/// <param name="prompt">A text description of the desired image(s). The maximum length is 1000 characters.</param>
		/// <param name="numOfImages">How many different choices to request for each prompt.  Defaults to 1.</param>
		/// <param name="size">The size of the generated images. Must be one of 256x256, 512x512, or 1024x1024.</param>
		/// <param name="user">A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.</param>
		/// <param name="responseFormat">The format in which the generated images are returned. Must be one of url or b64_json.</param>
		public ImageGenerationRequest(
			string prompt,
			int? numOfImages = 1,
			ImageSize size = null,
			string user = null,
			ImageResponseFormat responseFormat = null)
		{
			this.Prompt = prompt;
			this.NumOfImages = numOfImages;
			this.User = user;
			this.Size = size ?? ImageSize._1024;
			this.ResponseFormat = responseFormat ?? ImageResponseFormat.Url;
		}

	}
}
