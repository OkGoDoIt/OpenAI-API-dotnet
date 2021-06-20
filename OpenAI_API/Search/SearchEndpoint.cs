﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using OpenAI_API.Helpers;

namespace OpenAI_API
{
	/// <summary>
	/// The API lets you do semantic search over documents. This means that you can provide a query, such as a natural language question or a statement, and find documents that answer the question or are semantically related to the statement. The “documents” can be words, sentences, paragraphs or even longer documents. For example, if you provide documents "White House", "hospital", "school" and query "the president", you’ll get a different similarity score for each document. The higher the similarity score, the more semantically similar the document is to the query (in this example, “White House” will be most similar to “the president”).
	/// </summary>
	public class SearchEndpoint
	{
		OpenAIAPI Api;

		/// <summary>
		/// Constructor of the api endpoint.  Rather than instantiating this yourself, access it through an instance of <see cref="OpenAIAPI"/> as <see cref="OpenAIAPI.Search"/>.
		/// </summary>
		/// <param name="api"></param>
		internal SearchEndpoint(OpenAIAPI api)
		{
			this.Api = api;
		}



		#region GetSearchResults
		/// <summary>
		/// Perform a semantic search over a list of documents
		/// </summary>
		/// <param name="request">The request containing the query and the documents to match against</param>
		/// <returns>Asynchronously returns a Dictionary mapping each document to the score for that document.  The similarity score is a positive score that usually ranges from 0 to 300 (but can sometimes go higher), where a score above 200 usually means the document is semantically similar to the query.</returns>
		public async Task<Dictionary<string, double>> GetSearchResultsAsync(SearchRequest request)
		{
			OpenAiRequestHelper.CheckApiKey(Api.Auth?.ApiKey);

			var client = OpenAiRequestHelper.GetHttpClient(Api?.Auth?.ApiKey);

			var jsonContent = request.GetJsonContent();
			var stringContent = jsonContent.GetStringContent();

			var response = await client.PostAsync($"https://api.openai.com/v1/engines/{Api.UsingEngine.EngineName}/search", stringContent);
			await OpenAiResponseHelper.CheckForServerError(response, jsonContent);
			
			string resultAsString = await response.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<SearchResponse>(resultAsString);

			OpenAiResponseHelper.FillCompletionResultMetadata(res, response);

			if (res.Results == null || res.Results.Count == 0)
				throw new HttpRequestException("API returned no search results.  HTTP status code: " + response.StatusCode + ". Response body: " + resultAsString);

			return res.Results.ToDictionary(r => request.Documents[r.DocumentIndex], r => r.Score);
		}

		/// <summary>
		/// Perform a semantic search over a list of documents, with a specific query
		/// </summary>
		/// <param name="request">The request containing the documents to match against</param>
		/// <param name="query">A query to search for, overriding whatever was provided in <paramref name="request"/></param>
		/// <returns>Asynchronously returns a Dictionary mapping each document to the score for that document.  The similarity score is a positive score that usually ranges from 0 to 300 (but can sometimes go higher), where a score above 200 usually means the document is semantically similar to the query.</returns>
		public Task<Dictionary<string, double>> GetSearchResultsAsync(SearchRequest request, string query)
		{
			request.Query = query;
			return GetSearchResultsAsync(request);
		}

		/// <summary>
		/// Perform a semantic search of a query over a list of documents
		/// </summary>
		/// <param name="query">A query to match against</param>
		/// <param name="documents">Documents to search over, provided as a list of strings</param>
		/// <returns>Asynchronously returns a Dictionary mapping each document to the score for that document.  The similarity score is a positive score that usually ranges from 0 to 300 (but can sometimes go higher), where a score above 200 usually means the document is semantically similar to the query.</returns>
		public Task<Dictionary<string, double>> GetSearchResultsAsync(string query, params string[] documents)
		{
			SearchRequest request = new SearchRequest(query, documents);
			return GetSearchResultsAsync(request);
		}

		#endregion

		#region GetBestMatch

		/// <summary>
		/// Perform a semantic search over a list of documents to get the single best match
		/// </summary>
		/// <param name="request">The request containing the query and the documents to match against</param>
		/// <returns>Asynchronously returns the best matching document</returns>
		public async Task<string> GetBestMatchAsync(SearchRequest request)
		{
			var results = await GetSearchResultsAsync(request);
			if (results.Count == 0)
				return null;

			return results.ToList().OrderByDescending(kv => kv.Value).FirstOrDefault().Key;
		}

		/// <summary>
		/// Perform a semantic search over a list of documents with a specific query to get the single best match
		/// </summary>
		/// <param name="request">The request containing the documents to match against</param>
		/// <param name="query">A query to search for, overriding whatever was provided in <paramref name="request"/></param>
		/// <returns>Asynchronously returns the best matching document</returns>
		public Task<string> GetBestMatchAsync(SearchRequest request, string query)
		{
			request.Query = query;
			return GetBestMatchAsync(request);
		}

		/// <summary>
		/// Perform a semantic search of a query over a list of documents to get the single best match
		/// </summary>
		/// <param name="query">A query to match against</param>
		/// <param name="documents">Documents to search over, provided as a list of strings</param>
		/// <returns>Asynchronously returns the best matching document</returns>
		public Task<string> GetBestMatchAsync(string query, params string[] documents)
		{
			SearchRequest request = new SearchRequest(query, documents);
			return GetBestMatchAsync(request);
		}

		#endregion

		#region GetBestMatchWithScore

		/// <summary>
		/// Perform a semantic search over a list of documents to get the single best match and its score
		/// </summary>
		/// <param name="request">The request containing the query and the documents to match against</param>
		/// <returns>Asynchronously returns a tuple of the best matching document and its score.  The similarity score is a positive score that usually ranges from 0 to 300 (but can sometimes go higher), where a score above 200 usually means the document is semantically similar to the query.</returns>
		public async Task<Tuple<string, double>> GetBestMatchWithScoreAsync(SearchRequest request)
		{
			var results = await GetSearchResultsAsync(request);
			var best = results.ToList().OrderByDescending(kv => kv.Value).FirstOrDefault();
			return new Tuple<string, double>(best.Key, best.Value);
		}

		/// <summary>
		/// Perform a semantic search over a list of documents with a specific query to get the single best match and its score
		/// </summary>
		/// <param name="request">The request containing the documents to match against</param>
		/// <param name="query">A query to search for, overriding whatever was provided in <paramref name="request"/></param>
		/// <returns>Asynchronously returns a tuple of the best matching document and its score.  The similarity score is a positive score that usually ranges from 0 to 300 (but can sometimes go higher), where a score above 200 usually means the document is semantically similar to the query.</returns>
		public Task<Tuple<string, double>> GetBestMatchWithScoreAsync(SearchRequest request, string query)
		{
			request.Query = query;
			return GetBestMatchWithScoreAsync(request);
		}

		/// <summary>
		/// Perform a semantic search of a query over a list of documents to get the single best match and its score
		/// </summary>
		/// <param name="query">A query to match against</param>
		/// <param name="documents">Documents to search over, provided as a list of strings</param>
		/// <returns>Asynchronously returns a tuple of the best matching document and its score.  The similarity score is a positive score that usually ranges from 0 to 300 (but can sometimes go higher), where a score above 200 usually means the document is semantically similar to the query.</returns>
		public Task<Tuple<string, double>> GetBestMatchWithScoreAsync(string query, params string[] documents)
		{
			SearchRequest request = new SearchRequest(query, documents);
			return GetBestMatchWithScoreAsync(request);
		}

		#endregion
	}
}
