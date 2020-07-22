using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenAI_API
{
	public class SearchRequest
	{

		[JsonProperty("documents")]
		public List<string> Documents { get; set; }

		[JsonProperty("query")]
		public string Query { get; set; }

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
