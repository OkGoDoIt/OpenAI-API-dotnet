using System.Net.Http;
using System.Threading.Tasks;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// You can use this endpoint for audio transcription or translation.
    /// </summary>
    public class AudioEndpoint : EndpointBase, IAudioEndpoint
    {
        /// <summary>
        /// Creates audio endpoint object.
        /// </summary>
        /// <param name="api"></param>
        public AudioEndpoint(OpenAIAPI api) : base(api)
        {
        }

        /// <summary>
        /// Audio endpoint.
        /// </summary>
        protected override string Endpoint { get { return "audio/transcriptions"; } }

        /// <summary>
        /// Sends transcript request to openai and returns verbose_json result.
        /// </summary>
        public Task<TranscriptionVerboseJsonResult> CreateTranscriptionAsync(TranscriptionRequest request)
        {
            var content = new MultipartFormDataContent();

            var fileContent = new StreamContent(request.File.File);
            fileContent.Headers.ContentLength = request.File.ContentLength;
            fileContent.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue(request.File.ContentType);

            content.Add(fileContent, "file", request.File.Name);
            content.Add(new StringContent(request.Model), "model");

            if (!IsNullOrWhiteSpace(request.Prompt))
                content.Add(new StringContent(request.Prompt), "prompt");

            if (!IsNullOrWhiteSpace(request.ResponseFormat))
                content.Add(new StringContent(request.ResponseFormat), "response_format");

            if (!request.Temperature.HasValue)
                content.Add(new StringContent(request.Temperature.ToString()), "temperature");

            if (!IsNullOrWhiteSpace(request.Language))
                content.Add(new StringContent(request.Language), "language");

            return HttpPost<TranscriptionVerboseJsonResult>(postData: content);
        }

        /// <summary>
        /// Translates audio into into English.
        /// </summary>
        public Task<TranscriptionVerboseJsonResult> CreateTranslationAsync(TranslationRequest request) 
        {
            return CreateTranscriptionAsync(new TranscriptionRequest { 
                File = request.File,
                Model = request.Model,
                Prompt = request.Prompt,
                ResponseFormat = request.ResponseFormat,
                Temperature = request.Temperature}
            );
        }

        private bool IsNullOrWhiteSpace(string str) => string.IsNullOrWhiteSpace(str);
    }
}
