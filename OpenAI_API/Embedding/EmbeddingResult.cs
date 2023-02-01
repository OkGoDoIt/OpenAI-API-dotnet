using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Embedding
{
    /// <summary>
    /// Represents an embedding result returned by the Embedding API.  
    /// </summary>
    public class EmbeddingResult
    {
        /// <summary>
        /// Type of the response. In case of embeddings, this will be "list"  
        /// </summary>
        [JsonProperty("object")]

        public string Object { get; set; }

        /// <summary>
        /// List of results of the embedding
        /// </summary>
        [JsonProperty("data")]
        public Data[] Data { get; set; }

        /// <summary>
        /// Name of the model used to generate this embedding
        /// </summary>
        [JsonProperty("model")]
        public string Model { get; set; }

        /// <summary>
        /// Usage statistics of how many tokens have been used for this request
        /// </summary>
        [JsonProperty("usage")]
        public Usage Usage { get; set; }
    }

    /// <summary>
    /// Data returned from the Embedding API.
    /// </summary>
    public class Data
    {
        /// <summary>
        /// Type of the response. In case of Data, this will be "embedding"  
        /// </summary>
        [JsonProperty("object")]

        public string Object { get; set; }

        /// <summary>
        /// The input text represented as a vector (list) of floating point numbers
        /// </summary>
        [JsonProperty("embedding")]
        public float[] Embedding { get; set; }

        /// <summary>
        /// Index
        /// </summary>
        [JsonProperty("index")]
        public int Index { get; set; }

    }

    /// <summary>
    /// Usage statistics of how many tokens have been used for this request.
    /// </summary>
    public class Usage
    {
        /// <summary>
        /// How many tokens did the prompt consist of
        /// </summary>
        [JsonProperty("prompt_tokens")]
        public int PromptTokens { get; set; }

        /// <summary>
        /// How many tokens did the request consume total
        /// </summary>
        [JsonProperty("total_tokens")]
        public int TotalTokens { get; set; }

    }

}
