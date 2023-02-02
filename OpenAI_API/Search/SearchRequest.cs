using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenAI_API
{
    [Obsolete("OpenAI no long supports the Search endpoint")]
    public class SearchRequest
	{

		[JsonProperty("documents")]
		public List<string> Documents { get; set; }

		[JsonProperty("query")]
		public string Query { get; set; }

        /// <summary>
        /// ID of the model to use. You can use <see cref="ModelsEndpoint.GetModelsAsync()"/> to see all of your available models, or use a standard model like <see cref="Model.DavinciText"/>.  Defaults to <see cref="Model.DavinciText"/>.
        /// </summary>
        [JsonProperty("model")]
        public string Model { get; set; } = OpenAI_API.Model.DavinciCode;

        public SearchRequest(string query = null, params string[] documents)
		{
			Query = query;
			Documents = documents?.ToList() ?? new List<string>();
		}

		public SearchRequest(IEnumerable<string> documents)
		{
			Documents = documents.ToList();
		}
	}
}
