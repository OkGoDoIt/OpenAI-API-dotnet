using NUnit.Framework;
using OpenAI_API.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenAI_Tests
{
    public class ImageEndpointTests
    {
        [SetUp]
        public void Setup()
        {
            OpenAI_API.APIAuthentication.Default = new OpenAI_API.APIAuthentication(Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY"));
        }

        [Test]
        public void CreateBasicImage()
        {
            var api = new OpenAI_API.OpenAIAPI();

            Assert.IsNotNull(api.Images);

            var results = api.Images.CreateImageAsync(new ImageRequest("A cyberpunk monkey hacker dreaming of a beautiful bunch of bananas, digital art")).Result;
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
            //Assert.NotNull(results.Object);
            Assert.NotZero(results.Data.Count);
            Assert.That(results.Data.First().Url.Length > 0);
        }

    }
}
