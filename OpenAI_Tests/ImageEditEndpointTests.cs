using NUnit.Framework;
using OpenAI_API.Images;
using OpenAI_API.Moderation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenAI_Tests
{
    public class ImageEditEndpointTests
    {
        [SetUp]
        public void Setup()
        {
			OpenAI_API.APIAuthentication.Default = new OpenAI_API.APIAuthentication(Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY"));
        }

        private string GetImage(string path)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(path);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            return base64ImageRepresentation;
        }

        [Test]
        public void EditImage()
        {
            string imageFilepath = Path.Combine(AppContext.BaseDirectory, "images\\EditImage.png");
            string prompt = "add flowers, digital art";

            Assert.That(File.Exists(imageFilepath));
            EditImage(imageFilepath, prompt);
        }

        private void EditImage(string imageFilepath, string prompt)
        {
            var api = new OpenAI_API.OpenAIAPI();

            Assert.IsNotNull(api.ImageEdit);
            Assert.IsTrue(File.Exists(imageFilepath));
            var imageEditRequest = new ImageEditRequest(imageFilepath, prompt, 1, ImageSize._256);

            ImageResult results = null;

            try
            {
                results = api.ImageEdit.EditImageAsync(imageEditRequest).Result;
            }
            catch (Exception ex)
            {
            }

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

            Assert.NotZero(results.Data.Count);
            Assert.NotNull(results.Data.First().Url);
            Assert.That(results.Data.First().Url.Length > 0);
            Assert.That(results.Data.First().Url.StartsWith("https://"));
        }
    }
}
