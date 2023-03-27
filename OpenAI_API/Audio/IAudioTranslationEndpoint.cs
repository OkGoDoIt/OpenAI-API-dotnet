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
    public interface IAudioTranslationEndpoint
    {
        /// <summary>
        /// POST https://api.openai.com/v1/audio/translations
        /// Translates audio into into English
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<AudioTranslationResult> CreateTranslationAsync(AudioTranslationRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="model"></param>
        /// <param name="prompt"></param>
        /// <param name="response_format"></param>
        /// <param name="temperature"></param>
        /// <returns></returns>
        public Task<AudioTranslationResult> CreateTranslationAsync(string file, Model? model,
          string prompt, string response_format, double? temperature);
    }
}
