using System.Threading.Tasks;

namespace OpenAI_API.Audio
{
    public interface IAudioTranslationEndpoint
    {
        Task<string> GetTranslationAsync(AudioTranslationRequest request);
        Task<string> GetTranslationAsync(byte[] fileData, AudioTransFileFormat fileFormat, string prompt = null, AudioTransModel model = null, float? temperature = null, AudioTransResponseFormat response_format = null);
        Task<string> GetTranslationAsync(string filePath, string prompt = null, AudioTransModel model = null, float? temperature = null, AudioTransResponseFormat response_format = null);
    }
}