using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// Represents available voices for audio speech endpoint
    /// </summary>
    public class AudioSpeechVoice
    {
        private AudioSpeechVoice(string value) { Value = value; }

        private string Value { get; set; }

        /// <summary>
        /// Requests alloy voice
        /// </summary>
        public static AudioSpeechVoice Alloy { get { return new AudioSpeechVoice("alloy"); } }

        /// <summary>
        /// Requests echo voice
        /// </summary>
        public static AudioSpeechVoice Echo { get { return new AudioSpeechVoice("echo"); } }

        /// <summary>
        /// Requests fable voice
        /// </summary>
        public static AudioSpeechVoice Fable { get { return new AudioSpeechVoice("fable"); } }

        /// <summary>
        /// Requests onyx voice
        /// </summary>
        public static AudioSpeechVoice Onyx { get { return new AudioSpeechVoice("onyx"); } }

        /// <summary>
        /// Requests nova voice
        /// </summary>
        public static AudioSpeechVoice Nova { get { return new AudioSpeechVoice("nova"); } }

        /// <summary>
        /// Requests shimmer voice
        /// </summary>
        public static AudioSpeechVoice Shimmer { get { return new AudioSpeechVoice("shimmer"); } }

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
        public static implicit operator String(AudioSpeechVoice value) { return value; }

        internal class AudioSpeechVoiceJsonConverter : JsonConverter<AudioSpeechVoice>
        {
            public override AudioSpeechVoice ReadJson(JsonReader reader, Type objectType, AudioSpeechVoice existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return new AudioSpeechVoice(reader.ReadAsString());
            }

            public override void WriteJson(JsonWriter writer, AudioSpeechVoice value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }
        }
    }
}
