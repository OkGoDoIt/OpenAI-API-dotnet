using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// 
    /// </summary>
    public class AudioTranscriptionRequest : AudioRequestBase
    {
        /// <summary>
        /// The language of the input audio. Supplying the input language in ISO-639-1 format will improve accuracy and latency.
        /// </summary>
        [JsonProperty("language", Required = Required.Default)]
        public string Language { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AudioTranscriptionRequest()
        { 
        }

    }
}
