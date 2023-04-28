using NUnit.Framework;
using OpenAI_API.Audio;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OpenAI_Tests
{
    public class AudioEndpointTests
    {
        private const string TEST_FILE_NAME = "audio_test.mp3";

        [SetUp]
        public void Setup()
        {
            OpenAI_API.APIAuthentication.Default = new OpenAI_API.APIAuthentication(Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY"));
        }

        [Test]
        public async Task Test_TranscriptionAsync()
        {
            var api = new OpenAI_API.OpenAIAPI();
            var request = new TranscriptionRequest { File = new AudioFile { File = new FileStream(TEST_FILE_NAME, FileMode.Open), Name = TEST_FILE_NAME, ContentType = "audio/mp3" } };
            var result = await api.Audio.CreateTranscriptionAsync(request);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Text);
            Assert.IsNotNull(result.Segments);
            Assert.Greater(result.Segments.Count, 0);
        }

        [Test]
        public async Task Test_TranslateAsync() 
        {
            var api = new OpenAI_API.OpenAIAPI();
            var request = new TranslationRequest { File = new AudioFile { File = new FileStream(TEST_FILE_NAME, FileMode.Open), Name = TEST_FILE_NAME, ContentType = "audio/mp3" } };
            var result = await api.Audio.CreateTranslationAsync(request);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Text);
            Assert.IsNotNull(result.Segments);
            Assert.Greater(result.Segments.Count, 0);
        }
    }
}
