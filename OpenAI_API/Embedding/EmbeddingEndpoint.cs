using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_API.Embedding
{
    /// <summary>
    /// OpenAI’s text embeddings measure the relatedness of text strings by generating an embedding, which is a vector (list) of floating point numbers. The distance between two vectors measures their relatedness. Small distances suggest high relatedness and large distances suggest low relatedness.
    /// </summary>
    public class EmbeddingEndpoint
    {
        OpenAIAPI Api;
        /// <summary>
        /// This allows you to send request to the recommended model without needing to specify. Every request uses the <see cref="Model.AdaTextEmbedding"/> model
        /// </summary>
        public EmbeddingRequest DefaultEmbeddingRequestArgs { get; set; } = new EmbeddingRequest() { Model = Model.AdaTextEmbedding };

        /// <summary>
        /// Constructor of the api endpoint.  Rather than instantiating this yourself, access it through an instance of <see cref="OpenAIAPI"/> as <see cref="OpenAIAPI.Embeddings"/>.
        /// </summary>
        /// <param name="api"></param>
        internal EmbeddingEndpoint(OpenAIAPI api)
        {
            this.Api = api;
        }

        /// <summary>
        /// Ask the API to embedd text using the default embedding model <see cref="Model.AdaTextEmbedding"/>
        /// </summary>
        /// <param name="input">Text to be embedded</param>
        /// <returns>Asynchronously returns the embedding result. Look in its <see cref="Data.Embedding"/> property of <see cref="EmbeddingResult.Data"/> to find the vector of floating point numbers</returns>
        public async Task<EmbeddingResult> CreateEmbeddingAsync(string input)
        {
            DefaultEmbeddingRequestArgs.Input = input;
            return await CreateEmbeddingAsync(DefaultEmbeddingRequestArgs);
        }

        /// <summary>
        /// Ask the API to embedd text using a custom request
        /// </summary>
        /// <param name="request">Request to be send</param>
        /// <returns>Asynchronously returns the embedding result. Look in its <see cref="Data.Embedding"/> property of <see cref="EmbeddingResult.Data"/> to find the vector of floating point numbers</returns>
        public async Task<EmbeddingResult> CreateEmbeddingAsync(EmbeddingRequest request)
        {
            if (Api.Auth?.ApiKey is null)
            {
                throw new AuthenticationException("You must provide API authentication.  Please refer to https://github.com/OkGoDoIt/OpenAI-API-dotnet#authentication for details.");
            }

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Api.Auth.ApiKey);
            client.DefaultRequestHeaders.Add("User-Agent", "okgodoit/dotnet_openai_api");

            string jsonContent = JsonConvert.SerializeObject(request, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"https://api.openai.com/v1/embeddings", stringContent);
            if (response.IsSuccessStatusCode)
            {
                string resultAsString = await response.Content.ReadAsStringAsync();

                var res = JsonConvert.DeserializeObject<EmbeddingResult>(resultAsString);

                return res;
            }
            else
            {
                throw new HttpRequestException("Error calling OpenAi API to get completion.  HTTP status code: " + response.StatusCode.ToString() + ". Request body: " + jsonContent);
            }
        }

    }
}
