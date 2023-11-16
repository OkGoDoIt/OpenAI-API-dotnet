using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// The Request object for the Audio Transcription endpoint
    /// </summary>
    public class AudioTranscriptionRequest : AudioTranslationRequest
    {
        /// <summary>
        /// The language to transcribe from, assists Whisper-1 model
        /// </summary>
        public AudioTranscriptionLanguage language { get; set; }

        /// <summary>
        /// Provides a multipart form data content object for the request
        /// </summary>
        /// <returns></returns>
        public new MultipartFormDataContent GetMultipartFormDataContent()
        {
            var content = base.GetMultipartFormDataContent();
            if (language != null)
            {
                content.Add(new StringContent(language.ToString()), "language");
            }
            return content;
        }
    }
}
