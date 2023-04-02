using NUnit.Framework;
using OpenAI_API.Models;
using System;
using System.Linq;
using OpenAI_API.Moderation;

namespace OpenAI_Tests
{
	public class ModerationEndpointTests
	{
		[SetUp]
		public void Setup()
		{
			OpenAI_API.APIAuthentication.Default = new OpenAI_API.APIAuthentication(Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY"));
		}

		[Test]
		public void NoViolations()
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.Moderation);

			var results = api.Moderation.CallModerationAsync(new ModerationRequest("Hello world")).Result;
			Assert.IsNotNull(results);
			if (results.CreatedUnixTime.HasValue)
			{
				Assert.NotZero(results.CreatedUnixTime.Value);
				Assert.NotNull(results.Created);
				Assert.Greater(results.Created.Value, new DateTime(2018, 1, 1));
				Assert.Less(results.Created.Value, DateTime.Now.AddDays(1));
			}
			else
			{
				Assert.Null(results.Created);
			}
			Assert.NotNull(results.Results);
			Assert.NotZero(results.Results.Count);
			var result = results.Results[0];
			Assert.False(result.Flagged);
			Assert.Zero(result.FlaggedCategories.Count);
			Assert.Greater(result.HighestFlagScore, 0d);
			Assert.Null(result.MainContentFlag);
		}


		[Test]
		public void MultipleInputs()
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.Moderation);

			var results = api.Moderation.CallModerationAsync(new ModerationRequest("Hello world", "Good morning")).Result;
			Assert.IsNotNull(results);
			if (results.CreatedUnixTime.HasValue)
			{
				Assert.NotZero(results.CreatedUnixTime.Value);
				Assert.NotNull(results.Created);
				Assert.Greater(results.Created.Value, new DateTime(2018, 1, 1));
				Assert.Less(results.Created.Value, DateTime.Now.AddDays(1));
			}
			else
			{
				Assert.Null(results.Created);
			}
			Assert.NotNull(results.Results);
			Assert.AreEqual(2, results.Results.Count);
			foreach (var result in results.Results)
			{
				Assert.False(result.Flagged);
				Assert.Zero(result.FlaggedCategories.Count);
				Assert.Greater(result.HighestFlagScore, 0d);
				Assert.Null(result.MainContentFlag);
			}
		}



		[Test]
		public void MultipleInputsFailing()
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.Moderation);

			var results = api.Moderation.CallModerationAsync(new ModerationRequest("You are going to die, you scum", "I want to kill them")).Result;
			Assert.IsNotNull(results);
			if (results.CreatedUnixTime.HasValue)
			{
				Assert.NotZero(results.CreatedUnixTime.Value);
				Assert.NotNull(results.Created);
				Assert.Greater(results.Created.Value, new DateTime(2018, 1, 1));
				Assert.Less(results.Created.Value, DateTime.Now.AddDays(1));
			}
			else
			{
				Assert.Null(results.Created);
			}
			Assert.NotNull(results.Results);
			Assert.AreEqual(2, results.Results.Count);
			foreach (var result in results.Results)
			{
				Assert.True(result.Flagged);
				Assert.NotZero(result.FlaggedCategories.Count);
				Assert.Greater(result.HighestFlagScore, 0.5d);
				Assert.NotNull(result.MainContentFlag);
			}
		}

		[Test]
		public void ViolenceExample()
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.Moderation);

			var results = api.Moderation.CallModerationAsync("I want to kill them.").Result;
			Assert.IsNotNull(results);
			if (results.CreatedUnixTime.HasValue)
			{
				Assert.NotZero(results.CreatedUnixTime.Value);
				Assert.NotNull(results.Created);
				Assert.Greater(results.Created.Value, new DateTime(2018, 1, 1));
				Assert.Less(results.Created.Value, DateTime.Now.AddDays(1));
			}
			else
			{
				Assert.Null(results.Created);
			}
			Assert.NotNull(results.Results);
			Assert.NotZero(results.Results.Count);
			var result = results.Results[0];
			Assert.True(result.Flagged);
			Assert.NotZero(result.FlaggedCategories.Count);
			Assert.Greater(result.HighestFlagScore, 0.5d);
			Assert.AreEqual("violence", result.MainContentFlag);
			Assert.AreEqual(result.HighestFlagScore, result.CategoryScores["violence"]);
			Assert.AreEqual("violence", result.FlaggedCategories.First());
		}

	}
}
