namespace OpenAI_API.Audio
{
    /// <summary>
    /// Transcribes audio into the input language.
    /// </summary>
    public class TranscriptionRequest:TranslationRequest
    {
        /// <summary>
        /// The language of the input audio. Supplying the input language in ISO-639-1 format will improve accuracy and latency.Visit to list ISO-639-1 formats <see href="https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes"/>
        /// </summary>
        public string Language { get; set; }

    }
}
