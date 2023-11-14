using Newtonsoft.Json;
using OpenAI_API.Images;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// Represents available response formats for audio speech endpoint
    /// </summary>
    public class AudioSpeechResponseFormat
    {
        private AudioSpeechResponseFormat(string value) { Value = value; }

        private string Value { get; set; }

        /// <summary>
        /// Requests an audio file in mp3 format
        /// </summary>
        public static AudioSpeechResponseFormat Mp3 { get { return new AudioSpeechResponseFormat("mp3"); } }

        /// <summary>
        /// Requests an audio file in opus format
        /// </summary>
        public static AudioSpeechResponseFormat Opus { get { return new AudioSpeechResponseFormat("opus"); } }

        /// <summary>
        /// Requests an audio file in aac format
        /// </summary>
        public static AudioSpeechResponseFormat Aac { get { return new AudioSpeechResponseFormat("aac"); } }

        /// <summary>
        /// Requests an audio file in flac format
        /// </summary>
        public static AudioSpeechResponseFormat Flac { get { return new AudioSpeechResponseFormat("flac"); } }


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
        public static implicit operator String(AudioSpeechResponseFormat value) { return value; }

        internal class AudioSpeechResponseJsonConverter : JsonConverter<AudioSpeechResponseFormat>
        {
            public override AudioSpeechResponseFormat ReadJson(JsonReader reader, Type objectType, AudioSpeechResponseFormat existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return new AudioSpeechResponseFormat(reader.ReadAsString());
            }

            public override void WriteJson(JsonWriter writer, AudioSpeechResponseFormat value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }
        }
    }
}
