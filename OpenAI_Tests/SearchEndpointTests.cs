using NUnit.Framework;
using OpenAI_API;
using System;
using System.IO;
using System.Linq;

namespace OpenAI_Tests
{
	public class SearchEndpointTests
	{
		[SetUp]
		public void Setup()
		{
			OpenAI_API.APIAuthentication.Default = new OpenAI_API.APIAuthentication(Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY"));
		}

        /*
		[Test]
		public void TestBasicSearch()
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.Search);

			var result = api.Search.GetBestMatchAsync("Washington DC", "Canada", "China", "USA", "Spain").Result;
			Assert.IsNotNull(result);
			Assert.AreEqual("USA", result);
		}
		*/
        // TODO: Maybe implement a shim based on https://github.com/openai/openai-cookbook/blob/main/transition_guides_for_deprecated_API_endpoints/search_functionality_example.py ?

    }
}