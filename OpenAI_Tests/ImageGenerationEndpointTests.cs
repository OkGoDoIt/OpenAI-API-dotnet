using NUnit.Framework;
using OpenAI_API.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenAI_Tests
{
	public class ImageGenerationEndpointTests
	{
		[SetUp]
		public void Setup()
		{
			OpenAI_API.APIAuthentication.Default = new OpenAI_API.APIAuthentication(Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY"));
		}

		[TestCase(null)]
		[TestCase("dall-e-2")]
		[TestCase("dall-e-3")]
		public void SimpleImageCreation(string model)
		{
			var api = new OpenAI_API.OpenAIAPI();
			Assert.IsNotNull(api.ImageGenerations);
			var results = api.ImageGenerations.CreateImageAsync("A drawing of a computer writing a test", model).Result;
			Assert.IsNotNull(results);
			if (results.CreatedUnixTime.HasValue)
			{
				Assert.NotZero(results.CreatedUnixTime.Value);
				Assert.NotNull(results.Created);
				Assert.Greater(results.Created.Value, new DateTime(2023, 1, 1));
				Assert.Less(results.Created.Value, DateTime.Now.AddDays(1));
			}
			else
			{
				Assert.Null(results.Created);
			}

			Assert.NotZero(results.Data.Count);
			Assert.AreEqual(results.Data.Count, 1);
			Assert.NotNull(results.Data.First().Url);
			Assert.That(results.Data.First().Url.Length > 0);
			Assert.That(results.Data.First().Url.StartsWith("https://"));
		}

		[TestCase("256x256")]
		[TestCase("512x512")]
		[TestCase("1024x1024")]
		public void CreateDALLE2ImageWithUrl(string size)
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.ImageGenerations);

			var results = api.ImageGenerations.CreateImageAsync(new ImageGenerationRequest("A cyberpunk monkey hacker dreaming of a beautiful bunch of bananas, digital art", 2, new ImageSize(size))).Result;
			Assert.IsNotNull(results);
			if (results.CreatedUnixTime.HasValue)
			{
				Assert.NotZero(results.CreatedUnixTime.Value);
				Assert.NotNull(results.Created);
				Assert.Greater(results.Created.Value, new DateTime(2023, 1, 1));
				Assert.Less(results.Created.Value, DateTime.Now.AddDays(1));
			}
			else
			{
				Assert.Null(results.Created);
			}
		 
			Assert.NotZero(results.Data.Count);
			Assert.AreEqual(results.Data.Count, 2);
			Assert.NotNull(results.Data.First().Url);
			Assert.That(results.Data.First().Url.Length > 0);
			Assert.That(results.Data.First().Url.StartsWith("https://"));
		}


		[Test]
		public void CreateDALLE2ImageBase64Enc()
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.ImageGenerations);

			var results = api.ImageGenerations.CreateImageAsync(new ImageGenerationRequest("A cyberpunk monkey hacker dreaming of a beautiful bunch of bananas, digital art", 1, ImageSize._256, responseFormat: ImageResponseFormat.B64_json)).Result;
			Assert.IsNotNull(results);
			if (results.CreatedUnixTime.HasValue)
			{
				Assert.NotZero(results.CreatedUnixTime.Value);
				Assert.NotNull(results.Created);
				Assert.Greater(results.Created.Value, new DateTime(2023, 1, 1));
				Assert.Less(results.Created.Value, DateTime.Now.AddDays(1));
			}
			else
			{
				Assert.Null(results.Created);
			}

			Assert.NotZero(results.Data.Count);
			Assert.NotNull(results.Data.First().Base64Data);
			Assert.That(results.Data.First().Base64Data.Length > 0);
		}

		[TestCase("standard", "1024x1024")]
		[TestCase("hd", "1024x1024")]
		[TestCase("standard", "1024x1792")]
		[TestCase("standard", "1792x1024")]
		public void CreateDALLE3ImageWithUrl(string quality, string size)
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.ImageGenerations);

			var results = api.ImageGenerations.CreateImageAsync(new ImageGenerationRequest("A cyberpunk monkey hacker dreaming of a beautiful bunch of bananas, digital art", OpenAI_API.Models.Model.DALLE3, new ImageSize(size), quality)).Result;
			Assert.IsNotNull(results);
			if (results.CreatedUnixTime.HasValue)
			{
				Assert.NotZero(results.CreatedUnixTime.Value);
				Assert.NotNull(results.Created);
				Assert.Greater(results.Created.Value, new DateTime(2023, 1, 1));
				Assert.Less(results.Created.Value, DateTime.Now.AddDays(1));
			}
			else
			{
				Assert.Null(results.Created);
			}

			Assert.NotZero(results.Data.Count);
			Assert.NotNull(results.Data.First().Url);
			Assert.That(results.Data.First().Url.Length > 0);
			Assert.That(results.Data.First().Url.StartsWith("https://"));
		}

		[TestCase("dall-e-2", "hd", "1024x1024")]
		[TestCase("dall-e-2", "invalid-quality", "1024x1024")]
		[TestCase("dall-e-2", "standard", "1024x1792")]
		[TestCase("dall-e-3", "standard", "256x256")]
		[TestCase("dall-e-3", "invalid-quality", "256x256")]
		public void BadParameterCombosShouldFail(string model, string quality, string size)
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.ImageGenerations);

			Assert.ThrowsAsync<ArgumentException>(async () => await api.ImageGenerations.CreateImageAsync(new ImageGenerationRequest("A cyberpunk monkey hacker dreaming of a beautiful bunch of bananas, digital art", model, new ImageSize(size), quality)));
		}

		[Test]
		public void BadNumImagesWithDalle3ShouldFail()
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.ImageGenerations);

			var req = new ImageGenerationRequest("A cyberpunk monkey hacker dreaming of a beautiful bunch of bananas, digital art", OpenAI_API.Models.Model.DALLE3);
			req.NumOfImages = 2;

			Assert.ThrowsAsync<Newtonsoft.Json.JsonSerializationException>(async () => await api.ImageGenerations.CreateImageAsync(req));
		}


	}
}
