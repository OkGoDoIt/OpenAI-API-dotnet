using Newtonsoft.Json;
using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// Transcribes audio into the input language.
    /// </summary>
    public class AudioTranscriptionEndpoint : EndpointBase, IAudioTranscriptionEndpoint
    {
        /// <summary>
        /// The name of the endpoint, which is the final path segment in the API URL.  For example, "audio".
        /// </summary>
        protected override string Endpoint { get { return "audio/transcriptions"; } }

        /// <summary>
        /// Constructor of the api endpoint.  Rather than instantiating this yourself, access it through an instance of <see cref="OpenAIAPI"/> as <see cref="OpenAIAPI.ImageGenerations"/>.
        /// </summary>
        /// <param name="api"></param>
        internal AudioTranscriptionEndpoint(OpenAIAPI api) : base(api) { }

        /// <summary>
        /// Ask the API to Transcribe audio into the input language.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="prompt"></param>
        /// <param name="language"></param>
        /// <param name="model"></param>
        /// <param name="temperature"></param>
        /// <param name="response_format"></param>
        /// <returns></returns>
        public async Task<string> GetTranscriptionAsync(string filePath, string prompt = null, AudioTranscriptionLanguage language = null, AudioTransModel model = null, float? temperature = null, AudioTransResponseFormat response_format = null)
        {
            var fileExt = AudioTransFileFormat.GetValidFileFormat(System.IO.Path.GetExtension(filePath));
            var transcriptionRequest = new AudioTranscriptionRequest()
            {
                filePath = filePath,
                fileFormat = fileExt,
                prompt = prompt,
                language = language,
                model = model,
                temperature = temperature,
                response_format = response_format
            };
            return await GetTranscriptionAsync(transcriptionRequest);
        }

        /// <summary>
        /// Ask the API to Transcribe audio into the input language.
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="fileFormat"></param>
        /// <param name="prompt"></param>
        /// <param name="language"></param>
        /// <param name="model"></param>
        /// <param name="temperature"></param>
        /// <param name="response_format"></param>
        /// <returns></returns>
        public async Task<string> GetTranscriptionAsync(byte[] fileData, AudioTransFileFormat fileFormat, string prompt = null, AudioTranscriptionLanguage language = null, AudioTransModel model = null, float? temperature = null, AudioTransResponseFormat response_format = null)
        {
            var transcriptionRequest = new AudioTranscriptionRequest()
            {
                fileData = fileData,
                fileFormat = fileFormat,
                prompt = prompt,
                language = language,
                model = model,
                temperature = temperature,
                response_format = response_format
            };
            return await GetTranscriptionAsync(transcriptionRequest);
        }

        /// <summary>
        /// Ask the API to Transcribe audio into the input language.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> GetTranscriptionAsync(AudioTranscriptionRequest request)
        {
            if ( request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if ( request.fileFormat == null)
            {
                throw new ArgumentNullException(nameof(request.fileFormat));
            }

            string audioFileName = $"audio.{request.fileFormat}";
            if (request.fileData == null && System.IO.File.Exists(request.filePath))
            {
                request.fileData = System.IO.File.ReadAllBytes(request.filePath);
            }

            if (request.fileData == null)
            {
                throw new ArgumentNullException(nameof(request.fileData));
            }
            MultipartFormDataContent audioContent = new MultipartFormDataContent
            {
                {  new ByteArrayContent(request.fileData), "file", $"audio.{request.fileFormat}" }
            };
            if (audioContent != null)
            {
                if (request.model==null)
                {
                    request.model = OpenAI_API.Audio.AudioTransModel.Whisper1;
                }
                audioContent.Add(new StringContent(request.model), "model");

                if (request.language!=null)
                {
                    audioContent.Add(new StringContent(request.language), "language");
                }
                if (!string.IsNullOrEmpty(request.prompt))
                {
                    audioContent.Add(new StringContent(request.prompt), "prompt");
                }
                if (request.response_format!=null)
                {
                    audioContent.Add(new StringContent(request.response_format), "response_format");
                }
                if (request.temperature != null)
                {
                    audioContent.Add(new StringContent(((float)request.temperature).ToString()), "temperature");
                }
            }
            return await StringHttpRequest(postData: audioContent, verb: HttpMethod.Post);
        }

    }
}
