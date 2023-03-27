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
    public class AudioTranslationEndpoint : EndpointBase, IAudioTranslationEndpoint
    {
        /// <summary>
        /// 
        /// </summary>
        protected override string Endpoint { get { return "audio/translations"; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="api"></param>
        internal AudioTranslationEndpoint(OpenAIAPI api) : base(api) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="model"></param>
        /// <param name="prompt"></param>
        /// <param name="response_format"></param>
        /// <param name="temperature"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<AudioTranslationResult> CreateTranslationAsync(string file, Model model, string prompt, string response_format, double? temperature)
        {
           var request = new AudioTranslationRequest 
           { 
               File = file, 
               Model = model,
               Prompt = prompt,
               ResponseFormat = response_format,
               Temperature = temperature
           };

            return CreateTranslationAsync(request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AudioTranslationResult> CreateTranslationAsync(AudioTranslationRequest request)
        {
            var httpClient = GetClient();
            var _request = new HttpRequestMessage(HttpMethod.Post, Url);
            _request.Headers.Add("Authorization", "Bearer " + _Api.Auth.ApiKey);

            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(System.IO.File.OpenRead(request.File)), "file", request.File);
            content.Add(new StringContent(request.Model.ModelID), "model");

            _request.Content = content;

            var response = await httpClient.SendAsync(_request);

            try
            {
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();

                var testo = JsonConvert.DeserializeObject<AudioTranslationResult>(result).Text;
                return await Task<AudioTranslationResult>.FromResult(new AudioTranslationResult { Text = testo });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
