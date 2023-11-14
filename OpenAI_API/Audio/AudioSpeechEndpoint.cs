using OpenAI_API.Images;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// Generates audio from the input text.
    /// </summary>
    public class AudioSpeechEndpoint : EndpointBase, IAudioSpeechEndpoint
    {
        /// <summary>
        /// The name of the endpoint, which is the final path segment in the API URL.  For example, "audio".
        /// </summary>
        protected override string Endpoint { get { return "audio/speech"; } }

        /// <summary>
        /// Constructor of the api endpoint.  Rather than instantiating this yourself, access it through an instance of <see cref="OpenAIAPI"/> as <see cref="OpenAIAPI.AudioSpeech"/>.
        /// </summary>
        /// <param name="api"></param>
        internal AudioSpeechEndpoint(OpenAIAPI api) : base(api) { }

        /// <summary>
        /// Ask the API to Generate audio from the input text.
        /// </summary>
        /// <param name="model">One of the available TTS models: tts-1 or tts-1-hd</param>
        /// <param name="input">The text to generate audio for. The maximum length is 4096 characters.</param>
        /// <param name="voice">The voice to use when generating the audio. Supported voices are alloy, echo, fable, onyx, nova, and shimmer.</param>
        /// <returns>Asynchronously returns the image result. Look in its <see cref="Data.Url"/> </returns>
        public async Task<AudioSpeechResult> CreateSpeechAsync(AudioSpeechModel model,
            string input,
            AudioSpeechVoice voice)
        {
            AudioSpeechRequest req = new AudioSpeechRequest(model: model, voice: voice, input: input);
            return await CreateSpeechAsync(req);
        }

        /// <summary>
        /// Ask the API to Generate audio from the input text.
        /// </summary>
        /// <param name="request">Request to be send</param>
        /// <returns>Asynchronously returns the image result. Look in its <see cref="Data.Url"/> </returns>
        public async Task<AudioSpeechResult> CreateSpeechAsync(AudioSpeechRequest request)
        {
            return await HttpPost<AudioSpeechResult>(postData: request);
        }

    }
}
