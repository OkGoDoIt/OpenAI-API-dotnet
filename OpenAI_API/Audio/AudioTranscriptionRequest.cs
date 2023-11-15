using System;
using System.Collections.Generic;
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
    }
}
