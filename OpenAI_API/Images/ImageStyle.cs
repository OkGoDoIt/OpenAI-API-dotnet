using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Images
{
    /// <summary>
    /// Represents available styles for image endpoint
    /// </summary>
    public class ImageStyle
    {
        private ImageStyle(string value) { Value = value; }

        private string Value { get; set; }

        /// <summary>
        /// Requests vivid style
        /// </summary>
        public static ImageStyle Vivid { get { return new ImageStyle("vivid"); } }

        /// <summary>
        /// Requests natural style
        /// </summary>
        public static ImageStyle Natural { get { return new ImageStyle("natural"); } }

        /// <summary>
        /// Gets the string value for this style to pass to the API
        /// </summary>
        /// <returns>The size as a string</returns>
        public override string ToString()
        {
            return Value;
        }



        /// <summary>
        /// Gets the string value for this size to pass to the API
        /// </summary>
        /// <param name="value">The ImageStyle to convert</param>
        public static implicit operator String(ImageStyle value) { return value; }

        internal class ImageStyleJsonConverter : JsonConverter<ImageStyle>
        {
            public override void WriteJson(JsonWriter writer, ImageStyle value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }

            public override ImageStyle ReadJson(JsonReader reader, Type objectType, ImageStyle existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return new ImageStyle(reader.ReadAsString());
            }
        }
    }
}
