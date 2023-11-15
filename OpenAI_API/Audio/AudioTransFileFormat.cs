using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// Represents the available file formats of the submitted binary data
    /// </summary>
    public class AudioTransFileFormat
    {
        private AudioTransFileFormat(string value) { Value = value; }

        private string Value { get; set; }

        private static string[] _allowedFormats = new string[] { "wav", "mp3", "mp4", "mpeg", "mpga", "mp4a", "ogg", "aac", "flac", "webm" };

        /// <summary>
        /// Requests an audio file in mp3 format
        /// </summary>
        public static AudioTransFileFormat Mp3 { get { return new AudioTransFileFormat("mp3"); } }

        /// <summary>
        /// Requests an audio file in mp3 format
        /// </summary>
        public static AudioTransFileFormat Mp4 { get { return new AudioTransFileFormat("mp4"); } }

        /// <summary>
        /// Requests an audio file in mpeg format
        /// </summary>
        public static AudioTransFileFormat Mpeg { get { return new AudioTransFileFormat("mpeg"); } }

        /// <summary>
        /// Requests an audio file in mpga format
        /// </summary>
        public static AudioTransFileFormat Mpga { get { return new AudioTransFileFormat("mpga"); } }

        /// <summary>
        /// Requests an audio file in mp4a format
        /// </summary>
        public static AudioTransFileFormat Mp4a { get { return new AudioTransFileFormat("mp4a"); } }

        /// <summary>
        /// Requests an audio file in ogg format
        /// </summary>
        public static AudioTransFileFormat Ogg { get { return new AudioTransFileFormat("ogg"); } }

        /// <summary>
        /// Requests an audio file in aac format
        /// </summary>
        public static AudioTransFileFormat Wav { get { return new AudioTransFileFormat("wav"); } }

        /// <summary>
        /// Requests an audio file in flac format
        /// </summary>
        public static AudioTransFileFormat Flac { get { return new AudioTransFileFormat("flac"); } }

        /// <summary>
        /// Requests an audio file in webm format
        /// </summary>
        public static AudioTransFileFormat Webm { get { return new AudioTransFileFormat("webm"); } }

        /// <summary>
        /// Gets a valid AudioTransFileFormat from a string
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static AudioTransFileFormat GetValidFileFormat(string format)
        {
            if (!string.IsNullOrEmpty(format))
            {
                format = format.ToLower().Replace(".", "").Trim();
                if (_allowedFormats.Contains(format))
                {
                    return new AudioTransFileFormat(format);
                }
            }
            return null;
        }

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
        /// <param name="value">The AudioTransFileFormat to convert</param>
        public static implicit operator String(AudioTransFileFormat value) { return value; }

        internal class AudioTransFileFormatJsonConverter : JsonConverter<AudioTransFileFormat>
        {
            public override AudioTransFileFormat ReadJson(JsonReader reader, Type objectType, AudioTransFileFormat existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return new AudioTransFileFormat(reader.ReadAsString());
            }

            public override void WriteJson(JsonWriter writer, AudioTransFileFormat value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }
        }
    }
}
