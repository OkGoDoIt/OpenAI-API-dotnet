using NUnit.Framework;
using OpenAI_API;
using OpenAI_API.Embedding;
using System;
using System.Linq;

namespace OpenAI_Tests
{
    public class EmbeddingEndpointTests
    {
        [SetUp]
        public void Setup()
        {
            OpenAI_API.APIAuthentication.Default = new OpenAI_API.APIAuthentication(Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY"));
        }

        [Test]
        public void GetBasicEmbedding()
        {
            var api = new OpenAI_API.OpenAIAPI();

            Assert.IsNotNull(api.Embeddings);

            var results = api.Embeddings.CreateEmbeddingAsync(new EmbeddingRequest(Model.AdaTextEmbedding, "A test text for embedding")).Result;
            Assert.IsNotNull(results);
            Assert.NotNull(results.Object);
            Assert.NotZero(results.Data.Count);
            Assert.That(results.Data.First().Embedding.Length == 1536);
        }

        [Test]
        public void GetSimpleEmbedding()
        {
            var api = new OpenAI_API.OpenAIAPI();

            Assert.IsNotNull(api.Embeddings);

            var results = api.Embeddings.GetEmbeddingsAsync("A test text for embedding").Result;
            Assert.IsNotNull(results);
            Assert.That(results.Length == 1536);
        }
    }
}
