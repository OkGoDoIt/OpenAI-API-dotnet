using NUnit.Framework;
using OpenAI_API;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenAI_Tests
{
	public class CompletionEndpointTests
	{
		private OpenAIAPI GetApi => new OpenAIAPI(engine: Engine.Ada);

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

		[TestCase("eight")]
		[TestCase("nine")]
		public async Task CreateCompletionAsync_ShouldStopOnStopSequence(string stopSeq)
		{
			var api = GetApi;

			var completionReq = new CompletionRequest
			{
				Prompt = "three four five",
				Temperature = 0,
				MaxTokens = 5,
				Echo = true,
				StopSequence = stopSeq
			};

			var results = await api.Completions.CreateCompletionsAsync(completionReq);
			Assert.NotZero(results.Completions.Count);
		}

		// TODO: More tests needed but this covers basic functionality at least
	}
}