using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAudioTranscriptionEndpoint
    {
        /// <summary>
        /// POST  https://api.openai.com/v1/audio/transcriptions
        /// Transcribes audio into the input language.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<AudioTranscriptionResult> CreateTranscriptionAsync(AudioTranscriptionRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="model"></param>
        /// <param name="prompt"></param>
        /// <param name="response_format"></param>
        /// <param name="temperature"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public Task<AudioTranscriptionResult> CreateTranscriptionAsync(string file, Model? model,
           string prompt, string response_format, double? temperature, string language);
    }
}
