using System.Threading.Tasks;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// Interface for the Audio Speech endpoint
    /// </summary>
    public interface IAudioSpeechEndpoint
    {
        /// <summary>
        /// Creates a speech from the given input.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="input"></param>
        /// <param name="voice"></param>
        /// <returns></returns>
        Task<byte[]> CreateSpeechAsync(AudioSpeechModel model, string input, AudioSpeechVoice voice);

        /// <summary>
        /// Creates a speech from the given input.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<byte[]> CreateSpeechAsync(AudioSpeechRequest request);
    }
}