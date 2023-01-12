using NUnit.Framework;
using OpenAI_API;
using System;
using System.IO;
using System.Linq;

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
        // TODO: More tests needed but this covers basic functionality at least
    }
}