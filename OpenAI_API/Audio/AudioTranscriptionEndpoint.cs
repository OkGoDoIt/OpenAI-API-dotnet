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
    /// 
    /// </summary>
    public class AudioTranscriptionEndpoint : EndpointBase, IAudioTranscriptionEndpoint
    {
        /// <summary>
        /// 
        /// </summary>
        protected override string Endpoint { get { return "audio/transcriptions"; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="api"></param>
        internal AudioTranscriptionEndpoint(OpenAIAPI api) : base(api) { }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="request"></param>
       /// <returns></returns>
        public async Task<AudioTranscriptionResult> CreateTranscriptionAsync(AudioTranscriptionRequest request)
        {           
            var httpClient = GetClient();
            var _request = new HttpRequestMessage(HttpMethod.Post, Url);
            _request.Headers.Add("Authorization", "Bearer "+ _Api.Auth.ApiKey);

            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(System.IO.File.OpenRead(request.File)), "file", request.File);
            content.Add(new StringContent(request.Model.ModelID), "model");
            
            _request.Content = content;

            var response = await httpClient.SendAsync(_request);

            try
            {
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();

                var testo = JsonConvert.DeserializeObject<AudioTranscriptionResult>(result).Text;
                return await Task<AudioTranscriptionResult>.FromResult(new AudioTranscriptionResult { Text = testo});
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<AudioTranscriptionResult> CreateTranscriptionAsync(
            string file, 
            Model? model, 
            string prompt = null,
            string response_format = null,
            double? temperature = 0,
            string language = null)
        {
            var request = new AudioTranscriptionRequest()
            {
                File = file,
                Model = model,
                Prompt = prompt,
                Temperature = temperature,
                ResponseFormat = response_format,
                Language = language
            };
            return CreateTranscriptionAsync(request);
        }
    }
}
