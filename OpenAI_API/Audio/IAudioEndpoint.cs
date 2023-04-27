using System.Threading.Tasks;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// You can use this endpoint for audio transcription or translation.
    /// </summary>
    public interface IAudioEndpoint
    {
        /// <summary>
        /// Sends transcript request to openai and returns verbose_json result.
        /// </summary>
        Task<TranscriptionVerboseJsonResult> CreateTranscriptionAsync(TranscriptionRequest request);


        /// <summary>
        /// Translates audio into into English.
        /// </summary>
        public Task<TranscriptionVerboseJsonResult> CreateTranslationAsync(TranslationRequest request);
    }
}
