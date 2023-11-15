using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// Represents available response formats for audio transcription endpoint
    /// </summary>
    public class AudioTransResponseFormat
    {
        private AudioTransResponseFormat(string value) { Value = value; }

        private string Value { get; set; }

        /// <summary>
        /// Requests an result in json format
        /// </summary>
        public static AudioTransResponseFormat Json { get { return new AudioTransResponseFormat("json"); } }

        /// <summary>
        /// Requests an result in text format
        /// </summary>
        public static AudioTransResponseFormat Text { get { return new AudioTransResponseFormat("text"); } }

        /// <summary>
        /// Requests an result in srt format
        /// </summary>
        public static AudioTransResponseFormat Srt { get { return new AudioTransResponseFormat("srt"); } }

        /// <summary>
        /// Requests an result in verbose json format
        /// </summary>
        public static AudioTransResponseFormat VerboseJson { get { return new AudioTransResponseFormat("verbose_json"); } }
        
        /// <summary>
        /// Requests an result in vtt format
        /// </summary>
        public static AudioTransResponseFormat Vtt { get { return new AudioTransResponseFormat("vtt"); } }

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
        public static implicit operator String(AudioTransResponseFormat value) { return value; }

        internal class AudioTransResponseJsonConverter : JsonConverter<AudioTransResponseFormat>
        {
            public override AudioTransResponseFormat ReadJson(JsonReader reader, Type objectType, AudioTransResponseFormat existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return new AudioTransResponseFormat(reader.ReadAsString());
            }

            public override void WriteJson(JsonWriter writer, AudioTransResponseFormat value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }
        }
    }
}
