using Newtonsoft.Json;
using OpenAI_API.Images;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Audio
{
    public class AudioSpeechRequest
    {
        /// <summary>
        /// One of the available TTS models: tts-1 or tts-1-hd
        /// </summary>
        [JsonProperty("model"), JsonConverter(typeof(AudioSpeechModel.AudioSpeechModelJsonConverter))]
        public AudioSpeechModel Model { get; set; }

        /// <summary>
        /// The text to generate audio for. The maximum length is 4096 characters.
        /// </summary>
        [JsonProperty("input")]
        public string Input { get; set; }

        /// <summary>
        /// The voice to use when generating the audio. Supported voices are alloy, echo, fable, onyx, nova, and shimmer.
        /// </summary>
        [JsonProperty("voice"), JsonConverter(typeof(AudioSpeechVoice.AudioSpeechVoiceJsonConverter))]
        public AudioSpeechVoice Voice { get; set; }

        /// <summary>
        /// The size of the generated images. Must be one of 256x256, 512x512, or 1024x1024. Defauls to 1024x1024
        /// </summary>
        [JsonProperty("speed")]
        public float? Speed { get; set; }

        /// <summary>
        /// The format to audio in. Supported formats are mp3, opus, aac, and flac.
        /// </summary>
        [JsonProperty("response_format"), JsonConverter(typeof(AudioSpeechResponseFormat.AudioSpeechResponseJsonConverter))]
        public AudioSpeechResponseFormat ResponseFormat { get; set; }

        /// <summary>
        /// Cretes a new, empty <see cref="AudioSpeechRequest"/>
        /// </summary>
        public AudioSpeechRequest()
        {

        }

        /// <summary>
        /// Creates a new <see cref="AudioSpeechRequest"/> with the specified parameters
        /// </summary>
        /// <param name="model">A text description of the desired image(s). The maximum length is 1000 characters.</param>
        /// <param name="input">How many different choices to request for each prompt.  Defaults to 1.</param>
        /// <param name="voice">The size of the generated images. Must be one of 256x256, 512x512, or 1024x1024.</param>
        /// <param name="responseFormat">The format in which the generated images are returned. Must be one of url or b64_json.</param>
        /// <param name="speed">A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.</param>
        public AudioSpeechRequest(
            AudioSpeechModel model,
            string input,
            AudioSpeechVoice voice,
            AudioSpeechResponseFormat responseFormat = null,
            float? speed = null)
        {
            this.Model = model;
            this.Input = input;
            this.Voice = voice;
            this.ResponseFormat = responseFormat ?? AudioSpeechResponseFormat.Mp3;
            this.Speed = speed ?? 1;
        }
    }
}
