using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// Represents available models for audio translation and transcription endpoints
    /// </summary>
    public class AudioTransModel
    {
        private AudioTransModel(string value) { Value = value; }

        private string Value { get; set; }

        /// <summary>
        /// Requests an result in json format
        /// </summary>
        public static AudioTransModel Whisper1 { get { return new AudioTransModel("whisper-1"); } }

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
        public static implicit operator String(AudioTransModel value) { return value.ToString(); }

        internal class AudioTransModelJsonConverter : JsonConverter<AudioTransModel>
        {
            public override AudioTransModel ReadJson(JsonReader reader, Type objectType, AudioTransModel existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return new AudioTransModel(reader.ReadAsString());
            }

            public override void WriteJson(JsonWriter writer, AudioTransModel value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }
        }
    }
}
