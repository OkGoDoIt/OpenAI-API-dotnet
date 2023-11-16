using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Images
{
    /// <summary>
    /// Represents available models for image endpoint
    /// </summary>
    public class ImageModel
    {
        private ImageModel(string value) { Value = value; }

        private string Value { get; set; }

        /// <summary>
        /// Requests model Dall-e 2
        /// </summary>
        public static ImageModel Dalle2 { get { return new ImageModel("dall-e-2"); } }

        /// <summary>
        /// Requests model Dall-e 3
        /// </summary>
        public static ImageModel Dalle3 { get { return new ImageModel("dall-e-3"); } }

        /// <summary>
        /// Gets the string value for this model to pass to the API
        /// </summary>
        /// <returns>The size as a string</returns>
        public override string ToString()
        {
            return Value;
        }



        /// <summary>
        /// Gets the string value for this size to pass to the API
        /// </summary>
        /// <param name="value">The ImageModel to convert</param>
        public static implicit operator String(ImageModel value) { return value; }

        internal class ImageModelJsonConverter : JsonConverter<ImageModel>
        {
            public override void WriteJson(JsonWriter writer, ImageModel value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }

            public override ImageModel ReadJson(JsonReader reader, Type objectType, ImageModel existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return new ImageModel(reader.ReadAsString());
            }
        }
    }
}
