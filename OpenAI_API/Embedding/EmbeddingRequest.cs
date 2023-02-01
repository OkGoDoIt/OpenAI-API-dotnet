using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Embedding
{
    /// <summary>
    /// Represents a request to the Completions API. Matches with the docs at <see href="https://platform.openai.com/docs/api-reference/embeddings">the OpenAI docs</see>
    /// </summary>
    public class EmbeddingRequest
    {
        /// <summary>
        /// ID of the model to use. You can use <see cref="ModelsEndpoint.GetModelsAsync()"/> to see all of your available models, or use a standard model like <see cref="Model.AdaTextEmbedding"/>.
        /// </summary>
        [JsonIgnore]
        public Model Model { get; set; }

        /// <summary>
        /// The id/name of the model
        /// </summary>
        [JsonProperty("model")]
        public string ModelName => Model.ModelID;

        /// <summary>
        /// Main text to be embedded
        /// </summary>
        [JsonProperty("input")]
        public string Input { get; set; }

        /// <summary>
        /// Cretes a new, empty <see cref="EmbeddingRequest"/>
        /// </summary>
        public EmbeddingRequest()
        {

        }

        /// <summary>
        /// Creates a new <see cref="EmbeddingRequest"/> with the specified parameters
        /// </summary>
        /// <param name="model">The model to use. You can use <see cref="ModelsEndpoint.GetModelsAsync()"/> to see all of your available models, or use a standard model like <see cref="Model.AdaTextEmbedding"/>.</param>
        /// <param name="input">The prompt to transform</param>
        public EmbeddingRequest(Model model, string input)
        {
            Model = model;
            this.Input = input;
        }
    }
}
