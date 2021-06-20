using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenAI_API;

namespace OpenAI_Tests
{
	public class EnginesEndpointTests
	{
		[SetUp]
		public void Setup()
		{
			APIAuthentication.Default = new APIAuthentication(Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY"));
		}

		private OpenAIAPI GetApi => new OpenAIAPI(engine: Engine.Ada);

		[Test]
		public async Task GetEnginesAsync_ShouldReturnTheEngineList()
		{
			var api = GetApi;
			var engines = await api.Engines.GetEnginesAsync();
			engines.Count.Should().BeGreaterOrEqualTo(5, "most engines should be returned");
		}

		[Test]
		public void GetEnginesAsync_ShouldFailIfInvalidAuthIsProvided()
		{
			var api = new OpenAIAPI(new APIAuthentication(Guid.NewGuid().ToString()), Engine.Ada);
			Func<Task> act = () => api.Engines.GetEnginesAsync();
			act.Should()
				.Throw<HttpRequestException>()
				.Where(exc => exc.Message.Contains("Incorrect API key provided"));
		}

		[TestCase("ada")]
		[TestCase("babbage")]
		[TestCase("curie")]
		[TestCase("davinci")]
		public async Task RetrieveEngineDetailsAsync_ShouldRetrieveEngineDetails(string engineId)
		{
			var api = GetApi;
			var engineData = await api.Engines.RetrieveEngineDetailsAsync(engineId);
			engineData?.EngineName?.Should()?.Be(engineId);
		}

	}
}
