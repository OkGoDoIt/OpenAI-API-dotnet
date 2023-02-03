using FluentAssertions;
using NUnit.Framework;
using OpenAI_API;
using OpenAI_API.Models;
using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace OpenAI_Tests
{
	public class ModelEndpointTests
	{
		[SetUp]
		public void Setup()
		{
			OpenAI_API.APIAuthentication.Default = new OpenAI_API.APIAuthentication(Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY"));
		}

		[Test]
		public void GetAllModels()
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.Models);

			var results = api.Models.GetModelsAsync().Result;
			Assert.IsNotNull(results);
			Assert.NotZero(results.Count);
			Assert.That(results.Any(c => c.ModelID.ToLower().StartsWith("text-davinci")));
		}

		[Test]
		public void GetModelDetails()
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.Models);

			var result = api.Models.RetrieveModelDetailsAsync(Model.DavinciText).Result;
			Assert.IsNotNull(result);

			Assert.NotNull(result.CreatedUnixTime);
			Assert.NotZero(result.CreatedUnixTime.Value);
			Assert.NotNull(result.Created);
			Assert.Greater(result.Created.Value, new DateTime(2018, 1, 1));
			Assert.Less(result.Created.Value, DateTime.Now.AddDays(1));

			Assert.IsNotNull(result.ModelID);
			Assert.IsNotNull(result.OwnedBy);
			Assert.AreEqual(Model.DavinciText.ModelID.ToLower(), result.ModelID.ToLower());
		}


		[Test]
		public async Task GetEnginesAsync_ShouldReturnTheEngineList()
		{
			var api = new OpenAI_API.OpenAIAPI();
			var models = await api.Models.GetModelsAsync();
			models.Count.Should().BeGreaterOrEqualTo(5, "most engines should be returned");
		}

		[Test]
		public void GetEnginesAsync_ShouldFailIfInvalidAuthIsProvided()
		{
			var api = new OpenAIAPI(new APIAuthentication(Guid.NewGuid().ToString()));
			Func<Task> act = () => api.Models.GetModelsAsync();
			act.Should()
				.ThrowAsync<AuthenticationException>()
				.Where(exc => exc.Message.Contains("Incorrect API key provided"));
		}

		[TestCase("ada")]
		[TestCase("babbage")]
		[TestCase("curie")]
		[TestCase("davinci")]
		public async Task RetrieveEngineDetailsAsync_ShouldRetrieveEngineDetails(string modelId)
		{
			var api = new OpenAI_API.OpenAIAPI();
			var modelData = await api.Models.RetrieveModelDetailsAsync(modelId);
			modelData?.ModelID?.Should()?.Be(modelId);
			modelData.Created.Should().BeAfter(new DateTime(2018, 1, 1), "the model has a created date no earlier than 2018");
			modelData.Created.Should().BeBefore(DateTime.Now.AddDays(1), "the model has a created date before today");
		}
	}
}