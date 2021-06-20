using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenAI_API;

namespace OpenAI_Tests
{
	public class SearchEndpointsTest
	{
		[SetUp]
		public void Setup()
		{
			APIAuthentication.Default = new APIAuthentication(Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY"));
		}

		private OpenAIAPI GetApi => new OpenAIAPI(engine: Engine.Ada);

		//Credit: https://www.pragnakalp.com/question-answering-using-gpt3-examples/
		private List<string> Docs = new List<string>
		{
			"Google was founded in 1998 by Larry Page and Sergey Brin while they were Ph.D. students at Stanford University in California. Together they own about 14 percent of its shares and control 56 percent of the stockholder voting power through supervoting stock. They incorporated Google as a privately held company on September 4, 1998. An initial public offering (IPO) took place on August 19, 2004, and Google moved to its headquarters in Mountain View, California, nicknamed the Googleplex. In August 2015, Google announced plans to reorganize its various interests as a conglomerate called Alphabet Inc. Google is Alphabet's leading subsidiary and will continue to be the umbrella company for Alphabet's Internet interests. Sundar Pichai was appointed CEO of Google, replacing Larry Page who became the CEO of Alphabet.",
			"Amazon is an American multinational technology company based in Seattle, Washington, which focuses on e-commerce, cloud computing, digital streaming, and artificial intelligence. It is one of the Big Five companies in the U.S. information technology industry, along with Google, Apple, Microsoft, and Facebook. The company has been referred to as 'one of the most influential economic and cultural forces in the world', as well as the world's most valuable brand. Jeff Bezos founded Amazon from his garage in Bellevue, Washington on July 5, 1994. It started as an online marketplace for books but expanded to sell electronics, software, video games, apparel, furniture, food, toys, and jewelry. In 2015, Amazon surpassed Walmart as the most valuable retailer in the United States by market capitalization."
		};


		[Test]
		public async Task GetSearchResultsAsync_ShouldReturnResults()
		{
			var api = GetApi;

			var searchReq = new SearchRequest
			{
				Documents = Docs,
				Query = "When was google founded?"
			};
			var result = await api.Search.GetSearchResultsAsync(searchReq);
			result.Should().NotBeEmpty("there should be a search result");
			result.Keys.ToList()[0].Should().Contain("1998");
		}

		[Test]
		public void GetSearchResultsAsync_ShouldFailOnEmptyQuery()
		{
			var api = GetApi;

			var searchReq = new SearchRequest
			{
				Documents = Docs,
				Query = ""
			};
			Func<Task> act = () => api.Search.GetSearchResultsAsync(searchReq);
			act
				.Should().Throw<HttpRequestException>("empty query should fail")
				.Where(e => e.Message.Contains("is too short"));
		}

		[Test]
		public async Task GetSearchResultsAsync_ShouldWorkWithSeparateQuery()
		{
			var api = GetApi;

			var searchReq = new SearchRequest
			{
				Documents = Docs
			};
			var result = await api.Search.GetSearchResultsAsync(searchReq, "When was google founded?");
			result.Should().NotBeEmpty("there should be a search result");
			result.Keys.ToList()[0].Should().Contain("1998");
		}

		[Test]
		public async Task GetSearchResultsAsync_ShouldAllowAVariableNumberOfDocuments()
		{
			var api = GetApi;

			var result = await api.Search.GetSearchResultsAsync("When was google founded?", Docs.ToArray());
			result.Should().NotBeEmpty("there should be a search result");
			result.Keys.ToList()[0].Should().Contain("1998");
		}

		[Test]
		public async Task GetBestMatchAsync_ShouldReturnTheBestMatch()
		{
			var api = GetApi;

			var searchReq = new SearchRequest
			{
				Documents = Docs,
				Query = "When was google founded?"
			};

			var result = await api.Search.GetBestMatchAsync(searchReq);
			result.Should().Be(Docs[0]);
		}

		[Test]
		public async Task GetBestMatchAsync_ShouldReturnTheBestMatch_SeparateQuery()
		{
			var api = GetApi;

			var searchReq = new SearchRequest
			{
				Documents = Docs
			};

			var result = await api.Search.GetBestMatchAsync(searchReq, "When was google founded?");
			result.Should().Be(Docs[0]);
		}

		[Test]
		public async Task GetBestMatchWithScoreAsync_ShouldReturnTheBestMatch()
		{
			var api = GetApi;

			var searchReq = new SearchRequest
			{
				Documents = Docs,
				Query = "When was google founded?"
			};

			var result = await api.Search.GetBestMatchWithScoreAsync(searchReq);
			result.Item1.Should().Be(Docs[0]);
		}

		[Test]
		public async Task GetBestMatchWithScoreAsync_ShouldReturnTheBestMatch_SeparateQuery()
		{
			var api = GetApi;

			var searchReq = new SearchRequest
			{
				Documents = Docs
			};

			var result = await api.Search.GetBestMatchWithScoreAsync(searchReq, "When was google founded?");
			result.Item1.Should().Be(Docs[0]);
		}

		[Test]
		public async Task GetBestMatchWithScoreAsync_ShouldAllowAVariableNumberOfDocuments()
		{
			var api = GetApi;

			var result = await api.Search.GetBestMatchWithScoreAsync("When was google founded?", Docs.ToArray());
			result.Item1.Should().Be(Docs[0]);
		}
	}
}
