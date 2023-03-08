using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Images
{
	/// <summary>
	/// Represents available response formats for image generation endpoints
	/// </summary>
	public class ImageResponseFormat
	{
		private ImageResponseFormat(string value) { Value = value; }

		private string Value { get; set; }

		/// <summary>
		/// Requests an image that is 256x256
		/// </summary>
		public static ImageResponseFormat Url { get { return new ImageResponseFormat("url"); } }
		/// <summary>
		/// Requests an image that is 512x512
		/// </summary>
		public static ImageResponseFormat B64_json { get { return new ImageResponseFormat("b64_json"); } }


		/// <summary>
		/// Gets the string value for this response format to pass to the API
		/// </summary>
		/// <returns>The response format as a string</returns>
		public override string ToString()
		{
			return Value;
		}

		/// <summary>
		/// Gets the string value for this response format to pass to the API
		/// </summary>
		/// <param name="value">The ImageResponseFormat to convert</param>
		public static implicit operator String(ImageResponseFormat value) { return value; }

		internal class ImageResponseJsonConverter : JsonConverter<ImageResponseFormat>
		{
			public override ImageResponseFormat ReadJson(JsonReader reader, Type objectType, ImageResponseFormat existingValue, bool hasExistingValue, JsonSerializer serializer)
			{
				return new ImageResponseFormat(reader.ReadAsString());
			}

			public override void WriteJson(JsonWriter writer, ImageResponseFormat value, JsonSerializer serializer)
			{
				writer.WriteValue(value.ToString());
			}
		}
	}

}
