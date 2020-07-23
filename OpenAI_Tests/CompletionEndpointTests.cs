using NUnit.Framework;
using OpenAI_API;
using System;
using System.IO;
using System.Linq;

namespace OpenAI_Tests
{
	public class CompletionEndpointTests
	{
		[SetUp]
		public void Setup()
		{
			OpenAI_API.APIAuthentication.Default = new OpenAI_API.APIAuthentication(Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY"));
		}

		[Test]
		public void GetBasicCompletion()
		{
			var api = new OpenAI_API.OpenAIAPI(engine: Engine.Davinci);

			Assert.IsNotNull(api.Completions);
			
			var results = api.Completions.CreateCompletionsAsync(new CompletionRequest("One Two Three Four Five Six Seven Eight Nine One Two Three Four Five Six Seven Eight", temperature: 0.1, max_tokens: 5), 5).Result;
			Assert.IsNotNull(results);
			Assert.NotNull(results.Completions);
			Assert.NotZero(results.Completions.Count);
			Assert.That(results.Completions.Any(c => c.Text.Trim().ToLower().StartsWith("nine")));
		}

		// TODO: More tests needed but this covers basic functionality at least
	}
}