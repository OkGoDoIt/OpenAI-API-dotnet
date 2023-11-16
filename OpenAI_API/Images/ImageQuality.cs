using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Images
{
    /// <summary>
    /// Represents available qualities for image endpoint
    /// </summary>
    public class ImageQuality
    {
        private ImageQuality(string value) { Value = value; }

        private string Value { get; set; }

        /// <summary>
        /// Requests HD quality (Dall-e 3)
        /// </summary>
        public static ImageQuality Hd { get { return new ImageQuality("hd"); } }

        /// <summary>
        /// Requests standard quality (default)
        /// </summary>
        public static ImageQuality Standard { get { return new ImageQuality("standard"); } }

        /// <summary>
        /// Gets the string value for this style to pass to the API
        /// </summary>
        /// <returns>The size as a string</returns>
        public override string ToString()
        {
            return Value;
        }

        /// <summary>
        /// Gets the string value for this quality to pass to the API
        /// </summary>
        /// <param name="value">The ImageQuality to convert</param>
        public static implicit operator String(ImageQuality value) { return value; }

        internal class ImageQualityJsonConverter : JsonConverter<ImageQuality>
        {
            public override void WriteJson(JsonWriter writer, ImageQuality value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }

            public override ImageQuality ReadJson(JsonReader reader, Type objectType, ImageQuality existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return new ImageQuality(reader.ReadAsString());
            }
        }
    }
}
