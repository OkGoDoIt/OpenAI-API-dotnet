using System.IO;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// Creates translation request object for translate audios to english language.
    /// </summary>
    public class TranslationRequest
    {
        /// <summary>
        /// The audio file to transcribe, in one of these formats: mp3, mp4, mpeg, mpga, m4a, wav, or webm
        /// </summary>
        public AudioFile File { get; set; }

        /// <summary>
        /// ID of the model to use. Only whisper-1 is currently available.
        /// </summary>
        public string Model { get; set; } = Models.Model.Whisper_1;

        /// <summary>
        /// An optional text to guide the model's style or continue a previous audio segment. The Prompt should match the audio language. Please review href="https://platform.openai.com/docs/guides/speech-to-text/prompting"/>
        /// </summary>
        public string Prompt { get; set; }

        /// <summary>
        /// The format of the transcript output, in one of these options: json, text, srt, verbose_json, or vtt.
        /// </summary>
        public string ResponseFormat { get; set; } = "verbose_json";

        /// <summary>
        /// The sampling temperature, between 0 and 1. Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic. If set to 0, the model will use log probability to automatically increase the temperature until certain thresholds are hit.
        /// </summary>  
        public float? Temperature { get; set; }
    }

  
}
