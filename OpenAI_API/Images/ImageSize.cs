using Newtonsoft.Json;
using OpenAI_API.Models;
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
		internal ImageSize(string value) { Value = value; }

		private string Value { get; set; }

		/// <summary>
		/// Only for DALL-E 2. Requests an image that is 256x256
		/// </summary>
		public static ImageSize _256 { get { return new ImageSize("256x256"); } }
		/// <summary>
		/// Only for DALL-E 2. Requests an image that is 512x512
		/// </summary>
		public static ImageSize _512 { get { return new ImageSize("512x512"); } }
		/// <summary>
		/// Works with both DALL-E 2 and 3. Requests and image that is 1024x1024.
		/// </summary>
		public static ImageSize _1024 { get { return new ImageSize("1024x1024"); } }

		/// <summary>
		/// Only for DALL-E 3. Requests a tall image that is 1024x1792.
		/// </summary>
		public static ImageSize _1024x1792 { get { return new ImageSize("1024x1792"); } }

		/// <summary>
		/// Only for DALL-E 3. Requests a wide image that is 1792x1024.
		/// </summary>
		public static ImageSize _1792x1024 { get { return new ImageSize("1792x1024"); } }

		/// <summary>
		/// Gets the string value for this size to pass to the API
		/// </summary>
		/// <returns>The size as a string</returns>
		public override string ToString()
		{
			return Value;
		}

		/// <summary>
		/// Returns true is the string value of the sizes match
		/// </summary>
		/// <param name="obj">The other object to compare to</param>
		/// <returns>True is the sizes are the same</returns>
		public override bool Equals(object obj)
		{
			if (obj is null)
				return false;
			else if (obj is ImageSize)
				return this.Value.Equals(((ImageSize)obj).Value);
			else if (obj is string)
				return this.Value.Equals((string)obj);
			else
				return false;
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public static bool operator ==(ImageSize a, ImageSize b)
		{
			return a.Equals(b);
		}
		public static bool operator !=(ImageSize a, ImageSize b)
		{
			return !a.Equals(b);
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
