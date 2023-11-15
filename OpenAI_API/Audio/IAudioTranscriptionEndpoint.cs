using System.Threading.Tasks;

namespace OpenAI_API.Audio
{
    public interface IAudioTranscriptionEndpoint
    {
        Task<string> GetTranscriptionAsync(byte[] fileData, AudioTransFileFormat fileFormat, string prompt = null, AudioTranscriptionLanguage language = null, AudioTransModel model = null, float? temperature = null, AudioTransResponseFormat response_format = null);
        Task<string> GetTranscriptionAsync(string filePath, string prompt = null, AudioTranscriptionLanguage language = null, AudioTransModel model = null, float? temperature = null, AudioTransResponseFormat response_format = null);
        Task<string> GetTranscriptionAsync(AudioTranscriptionRequest request);
    }
}