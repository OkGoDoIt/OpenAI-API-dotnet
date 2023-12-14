using Newtonsoft.Json;
using NUnit.Framework;
using OpenAI_API.Audio;
using OpenAI_API.Chat;
using OpenAI_API.Completions;
using OpenAI_API.Models;
using OpenAI_API.Moderation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static OpenAI_API.Audio.TextToSpeechRequest;
using static OpenAI_API.Chat.ChatMessage;

namespace OpenAI_Tests
{
	public class TranscriptionTests
	{
		[SetUp]
		public void Setup()
		{
			OpenAI_API.APIAuthentication.Default = new OpenAI_API.APIAuthentication(Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY"));
		}

		[Test]
		public async Task EnglishTranscribeToText()
		{
			var api = new OpenAI_API.OpenAIAPI();
						
			string result = await api.Transcriptions.GetTextAsync("english-test.m4a");
			Assert.IsNotNull(result);
			Assert.AreEqual("Hello, this is a test of the transcription function. Is it coming out okay?", result.Trim());

			result = await api.Transcriptions.GetTextAsync("english-test.m4a", "en");
			Assert.IsNotNull(result);
			Assert.AreEqual("Hello, this is a test of the transcription function. Is it coming out okay?", result.Trim());
		}

		[Test]
		public async Task ChineseTranscribeToText()
		{
			var api = new OpenAI_API.OpenAIAPI();
			string result = await api.Transcriptions.GetTextAsync("chinese-test.m4a");
			Assert.IsNotNull(result);
			Assert.AreEqual("你好,我的名字是初培。我会说一点点普通话。你呢?", result.Trim());

			result = await api.Transcriptions.GetTextAsync("chinese-test.m4a", "zh");
			Assert.IsNotNull(result);
			Assert.AreEqual("你好,我的名字是初培。我会说一点点普通话。你呢?", result.Trim());
		}

		[Test]
		public async Task ChineseTranslateToEnglishText()
		{
			var api = new OpenAI_API.OpenAIAPI();
			string result = await api.Translations.GetTextAsync("chinese-test.m4a");
			Assert.IsNotNull(result);
			Assert.AreEqual("Hello, my name is Chu Pei. I can speak a little Mandarin. How about you?", result.Trim());
		}

		[TestCase("json", "\"text\": ")]
		[TestCase("srt", "00:00:00,000")]
		[TestCase("vtt", "00:00:00.000")]
		public async Task TranscribeToFormat(string format, string searchFor)
		{
			var api = new OpenAI_API.OpenAIAPI();
			string result = await api.Transcriptions.GetAsFormatAsync("english-test.m4a", format);
			Assert.IsNotNull(result);
			Assert.IsNotEmpty(result);
			Assert.True(result.Contains("Hello, this is a test of the transcription function. Is it coming out okay?"));
			Assert.True(result.Contains(searchFor), "Did not contain the format indicator: "+searchFor);
			result = await api.Transcriptions.GetAsFormatAsync("chinese-test.m4a",format, "zh");
			Assert.IsNotNull(result);
			Assert.IsNotEmpty(result);
			Assert.True(result.Contains("你好,我的名字是初培。我会说一点点普通话。你呢?"));
			Assert.True(result.Contains(searchFor), "Did not contain the format indicator: " + searchFor);
		}

		[Test]
		public async Task GetDetailedTranscribeJson()
		{
			var api = new OpenAI_API.OpenAIAPI();
			AudioResultVerbose result = await api.Transcriptions.GetWithDetailsAsync("english-test.m4a");
			Assert.IsNotNull(result);
			Assert.IsNotEmpty(result.RequestId);
			Assert.Greater(result.ProcessingTime.TotalMilliseconds, 100);
			Assert.AreEqual(6.99, result.duration, 0.05);
			Assert.AreEqual("english", result.language);
			Assert.AreEqual("transcribe", result.task);
			Assert.AreEqual("Hello, this is a test of the transcription function. Is it coming out okay?", result.text.Trim());
			Assert.AreEqual(1,result.segments.Count);
			Assert.AreEqual("Hello, this is a test of the transcription function. Is it coming out okay?", result.segments[0].text.Trim());
			Assert.AreEqual(19, result.segments[0].tokens.Count);
		}


		[Test]
		public async Task GetDetailedTranslateJson()
		{
			var api = new OpenAI_API.OpenAIAPI();
			var result = await api.Translations.GetWithDetailsAsync("chinese-test.m4a");
			Assert.IsNotNull(result);
			Assert.IsNotEmpty(result.RequestId);
			Assert.Greater(result.ProcessingTime.TotalMilliseconds, 100);
			Assert.AreEqual(10.62, result.duration, 0.05);
			Assert.AreEqual("translate", result.task);
			Assert.AreEqual("Hello, my name is Chu Pei. I can speak a little Mandarin. How about you?", result.text.Trim());
			Assert.AreEqual(1, result.segments.Count);
			Assert.AreEqual("Hello, my name is Chu Pei. I can speak a little Mandarin. How about you?", result.segments[0].text.Trim());
			Assert.AreEqual(22, result.segments[0].tokens.Count);
		}
	}
}
