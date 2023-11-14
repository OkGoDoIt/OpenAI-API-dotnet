using System.Threading.Tasks;

namespace OpenAI_API.Audio
{
    public interface IAudioSpeechEndpoint
    {
        Task<AudioSpeechResult> CreateSpeechAsync(AudioSpeechModel model, string input, AudioSpeechVoice voice);
        Task<AudioSpeechResult> CreateSpeechAsync(AudioSpeechRequest request);
    }
}