using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// Represents available models for audio speech endpoint
    /// </summary>
    public class AudioSpeechModel
    {
        private AudioSpeechModel(string value) { Value = value; }

        private string Value { get; set; }

        /// <summary>
        /// Requests an audio file in mp3 format
        /// </summary>
        public static AudioSpeechModel Tts1 { get { return new AudioSpeechModel("tts-1"); } }

        /// <summary>
        /// Requests an audio file in opus format
        /// </summary>
        public static AudioSpeechModel Tts1Hd { get { return new AudioSpeechModel("tts-1-hd"); } }

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
        public static implicit operator String(AudioSpeechModel value) { return value; }

        internal class AudioSpeechModelJsonConverter : JsonConverter<AudioSpeechModel>
        {
            public override AudioSpeechModel ReadJson(JsonReader reader, Type objectType, AudioSpeechModel existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return new AudioSpeechModel(reader.ReadAsString());
            }

            public override void WriteJson(JsonWriter writer, AudioSpeechModel value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }
        }
    }
}
