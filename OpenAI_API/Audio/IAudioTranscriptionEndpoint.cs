using System.Threading.Tasks;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// Interface for the Audio Transcription endpoint
    /// </summary>
    public interface IAudioTranscriptionEndpoint
    {
        /// <summary>
        /// Creates a transcription from the given input.
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="fileFormat"></param>
        /// <param name="prompt"></param>
        /// <param name="language"></param>
        /// <param name="model"></param>
        /// <param name="temperature"></param>
        /// <param name="response_format"></param>
        /// <returns></returns>
        Task<string> GetTranscriptionAsync(byte[] fileData, AudioTransFileFormat fileFormat, string prompt = null, AudioTranscriptionLanguage language = null, AudioTransModel model = null, float? temperature = null, AudioTransResponseFormat response_format = null);

        /// <summary>
        /// Creates a transcription from the given input.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="prompt"></param>
        /// <param name="language"></param>
        /// <param name="model"></param>
        /// <param name="temperature"></param>
        /// <param name="response_format"></param>
        /// <returns></returns>
        Task<string> GetTranscriptionAsync(string filePath, string prompt = null, AudioTranscriptionLanguage language = null, AudioTransModel model = null, float? temperature = null, AudioTransResponseFormat response_format = null);

        /// <summary>
        /// Creates a transcription from the given input.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<string> GetTranscriptionAsync(AudioTranscriptionRequest request);
    }
}