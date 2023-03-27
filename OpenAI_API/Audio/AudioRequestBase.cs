using Newtonsoft.Json;
using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// 
    /// </summary>
    public class AudioRequestBase
    {
        /// <summary>
        /// The audio file to transcribe, in one of these formats: mp3, mp4, mpeg, mpga, m4a, wav, or webm
        /// </summary>
        [JsonProperty("file")]
        public string File { get; set; }

        /// <summary>
        /// ID of the model to use. Only whisper-1 is currently available.
        /// </summary>
        [JsonProperty("model")]
        public Model Model { get; set; } = Model.Whisper;

        /// <summary>
        /// An optional text to guide the model's style or continue a previous audio segment. 
        /// The prompt should match the audio language.
        /// </summary>
        [JsonProperty("prompt")]
        public string Prompt { get; set; }

        /// <summary>
        /// The format of the transcript output, in one of these options: json, text, srt, verbose_json, or vtt.
        /// </summary>
        [JsonProperty("response_format")]
        public string ResponseFormat { get; set; }

        /// <summary>
        /// The sampling temperature, between 0 and 1. Higher values like 0.8 will make the output more random, 
        /// while lower values like 0.2 will make it more focused and deterministic. 
        /// If set to 0, the model will use log probability to automatically increase the temperature 
        /// until certain thresholds are hit.
        /// </summary>
        [JsonProperty("temperature")]
        public double? Temperature { get; set; }
    }
}
