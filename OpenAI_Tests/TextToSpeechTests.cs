using Newtonsoft.Json;
using NUnit.Framework;
using OpenAI_API.Audio;
using OpenAI_API.Chat;
using OpenAI_API.Completions;
using OpenAI_API.Models;
using OpenAI_API.Moderation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static OpenAI_API.Audio.TextToSpeechRequest;
using static OpenAI_API.Chat.ChatMessage;

namespace OpenAI_Tests
{
	public class TextToSpeechTests
	{
		[SetUp]
		public void Setup()
		{
			OpenAI_API.APIAuthentication.Default = new OpenAI_API.APIAuthentication(Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY"));
		}

		[TestCase("alloy", false, null)]
		[TestCase("echo", true, null)]
		[TestCase("fable", false, 1)]
		[TestCase("onyx", true, 1.25)]
		[TestCase("nova", false, 0.5)]
		public async Task SimpleTTSStreamTest(string voice, bool hd, double? speed)
		{
			var api = new OpenAI_API.OpenAIAPI();
			using (Stream result = await api.TextToSpeech.GetSpeechAsStreamAsync("Hello, brave new world!  This is a test.", voice, speed, TextToSpeechRequest.ResponseFormats.FLAC, hd ? Model.TTS_HD : null))
			{
				Assert.IsNotNull(result);
				using (StreamReader reader = new StreamReader(result))
				{
					Assert.Greater(result.Length, 10000);
					string asString = await reader.ReadToEndAsync();
					Assert.AreEqual("fLaC", asString.Substring(0, 4));
				}
			}
		}

		[Test]
		public async Task SimpleTTSFileTest()
		{
			string tempPath = Path.GetTempFileName();

			var api = new OpenAI_API.OpenAIAPI();
			var result = await api.TextToSpeech.SaveSpeechToFileAsync("Hello, brave new world!  This is a test.", tempPath, responseFormat: TextToSpeechRequest.ResponseFormats.FLAC);
			Assert.IsNotNull(result);
			Assert.Greater(result.Length, 10000);
			string asString = File.ReadAllText(tempPath);
			Assert.AreEqual("fLaC", asString.Substring(0, 4));
		}

		[TestCase(null)]
		[TestCase("mp3")]
		[TestCase("opus")]
		[TestCase("aac")]
		public async Task ManualTTSStreamTest(string format)
		{
			var api = new OpenAI_API.OpenAIAPI();

			var request = new TextToSpeechRequest()
			{
				Input = "Hello, brave new world!  This is a test.",
				ResponseFormat = format,
			};
			using (var result = await api.TextToSpeech.GetSpeechAsStreamAsync(request))
			{
				Assert.IsNotNull(result);
				Assert.Greater(result.Length, 10000);
			}
		}
	}
}
