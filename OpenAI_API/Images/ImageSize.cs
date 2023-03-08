using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Images
{
	/// <summary>
	/// Represents available sizes for image generation endpoints
	/// </summary>
	public class ImageSize
	{
		private ImageSize(string value) { Value = value; }

		private string Value { get; set; }

		/// <summary>
		/// Requests an image that is 256x256
		/// </summary>
		public static ImageSize _256 { get { return new ImageSize("256x256"); } }
		/// <summary>
		/// Requests an image that is 512x512
		/// </summary>
		public static ImageSize _512 { get { return new ImageSize("512x512"); } }
		/// <summary>
		/// Requests and image that is 1024x1024
		/// </summary>
		public static ImageSize _1024 { get { return new ImageSize("1024x1024"); } }

		/// <summary>
		/// Gets the string value for this size to pass to the API
		/// </summary>
		/// <returns>The size as a string</returns>
		public override string ToString()
		{
			return Value;
		}



		/// <summary>
		/// Gets the string value for this size to pass to the API
		/// </summary>
		/// <param name="value">The ImageSize to convert</param>
		public static implicit operator String(ImageSize value) { return value; }

		internal class ImageSizeJsonConverter : JsonConverter<ImageSize>
		{
			public override void WriteJson(JsonWriter writer, ImageSize value, JsonSerializer serializer)
			{
				writer.WriteValue(value.ToString());
			}

			public override ImageSize ReadJson(JsonReader reader, Type objectType, ImageSize existingValue, bool hasExistingValue, JsonSerializer serializer)
			{
				return new ImageSize(reader.ReadAsString());
			}
		}
	}

}
