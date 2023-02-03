using FluentAssertions;
using NUnit.Framework;
using OpenAI_API;
using System;
using System.Linq;
using System.Net.Http;
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

			var result = api.Models.RetrieveModelDetailsAsync(Model.DavinciText.ModelID).Result;
			Assert.IsNotNull(result);
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
				.ThrowAsync<HttpRequestException>()
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
		}
	}
}