using Newtonsoft.Json;
using NUnit.Framework;
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
using static OpenAI_API.Chat.ChatMessage;

namespace OpenAI_Tests
{
	public class ChatVisionTests
	{
		[SetUp]
		public void Setup()
		{
			OpenAI_API.APIAuthentication.Default = new OpenAI_API.APIAuthentication(Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY"));
			OpenAI_API.Models.Model.DefaultChatModel = Model.GPT4_Vision;
		}

		[Test]
		public async Task SimpleVisionTest()
		{
			var api = new OpenAI_API.OpenAIAPI();
			var result = await api.Chat.CreateChatCompletionAsync("What is the primary non-white color in this logo's gradient? Just tell me the one main color.", ImageInput.FromFile("../../../../OpenAI_API/nuget_logo.png"));
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Choices);
			Assert.AreEqual(1, result.Choices.Count);
			Assert.That(result.Choices[0].Message.TextContent.ToLower().Contains("blue") || result.Choices[0].Message.TextContent.ToLower().Contains("purple"));
		}

		[Test]
		public void TestVisionFromPath()
		{
			var api = new OpenAI_API.OpenAIAPI();
			ChatRequest request = new ChatRequest()
			{
				Model = Model.GPT4_Vision,
				Temperature = 0.0,
				MaxTokens = 500,
				Messages = new ChatMessage[] {
					new ChatMessage(ChatMessageRole.System, "You are a helpful assistant"),
					new ChatMessage(ChatMessageRole.User, "What is the primary color in this logo?",ImageInput.FromFile("../../../../OpenAI_API/nuget_logo.png"))
				}
			};
			var result = api.Chat.CreateChatCompletionAsync(request).Result;
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Choices);
			Assert.AreEqual(1, result.Choices.Count);
			Assert.That(result.Choices[0].Message.TextContent.ToLower().Contains("blue") || result.Choices[0].Message.TextContent.ToLower().Contains("purple") || result.Choices[0].Message.TextContent.ToLower().Contains("pink"));
		}

		[Test]
		public void TestVisionFromUrl()
		{
			var api = new OpenAI_API.OpenAIAPI();
			ChatRequest request = new ChatRequest()
			{
				Model = Model.GPT4_Vision,
				Temperature = 0.0,
				MaxTokens = 500,
				Messages = new ChatMessage[] {
					new ChatMessage(ChatMessageRole.System, "You are a helpful assistant"),
					new ChatMessage(ChatMessageRole.User, "This logo consists of many small shapes.  What shape are they?",ImageInput.FromImageUrl("https://rogerpincombe.com/templates/rp/center-aligned-no-shadow-small.png"))
				}
			};
			var result = api.Chat.CreateChatCompletionAsync(request).Result;
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Choices);
			Assert.AreEqual(1, result.Choices.Count);
			Assert.That(result.Choices[0].Message.TextContent.ToLower().Contains("circle") || result.Choices[0].Message.TextContent.ToLower().Contains("spiral"));
		}

		[Test]
		public void TestVisionWithMultipleImages()
		{
			var api = new OpenAI_API.OpenAIAPI();
			ChatRequest request = new ChatRequest()
			{
				Model = Model.GPT4_Vision,
				Temperature = 0.0,
				MaxTokens = 500,
				Messages = new ChatMessage[] {
					new ChatMessage(ChatMessageRole.User, "Here are two logos. What is the one common color (aside from white) that is used in both logos?",ImageInput.FromFile("../../../../OpenAI_API/nuget_logo.png"),ImageInput.FromImageUrl("https://rogerpincombe.com/templates/rp/center-aligned-no-shadow-small.png"))
				}
			};
			var result = api.Chat.CreateChatCompletionAsync(request).Result;
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Choices);
			Assert.AreEqual(1, result.Choices.Count);
			Assert.That(result.Choices[0].Message.TextContent.ToLower().Contains("blue") || result.Choices[0].Message.TextContent.ToLower().Contains("purple") || result.Choices[0].Message.TextContent.ToLower().Contains("pink"));
		}

		[Test]
		public void ChatBackAndForth()
		{
			var api = new OpenAI_API.OpenAIAPI();

			var chat = api.Chat.CreateConversation();
			chat.Model = Model.GPT4_Vision;
			chat.RequestParameters.Temperature = 0;

			chat.AppendSystemMessage("You are a graphic design assistant who helps identify colors.");
			chat.AppendUserInput("What are the primary non-white colors in this logo?", ImageInput.FromFile("../../../../OpenAI_API/nuget_logo.png"));
			chat.AppendExampleChatbotOutput("Blue and purple");
			chat.AppendUserInput("What are the primary non-white colors in this logo?", ImageInput.FromImageUrl("https://rogerpincombe.com/templates/rp/center-aligned-no-shadow-small.png"));
			string res = chat.GetResponseFromChatbotAsync().Result;
			Assert.NotNull(res);
			Assert.IsNotEmpty(res);
			Assert.That(res.ToLower().Contains("blue"));
			Assert.That(res.ToLower().Contains("red"));
			Assert.That(res.ToLower().Contains("yellow") || res.ToLower().Contains("gold"));
			chat.AppendUserInput("What are the primary non-white colors in this logo?", ImageInput.FromImageUrl("https://www.greatstartheater.org/images/logo.png"));
			res = chat.GetResponseFromChatbotAsync().Result;
			Assert.NotNull(res);
			Assert.IsNotEmpty(res);
			Assert.That(res.ToLower().Contains("red"));
			Assert.That(res.ToLower().Contains("black"));
		}

		[Test]
		public void VisionStreaming()
		{
			var api = new OpenAI_API.OpenAIAPI();
			ChatRequest request = new ChatRequest()
			{
				Model = Model.GPT4_Vision,
				Temperature = 0.0,
				MaxTokens = 500,
				Messages = new ChatMessage[] {
					new ChatMessage(ChatMessageRole.System, "You are a helpful assistant"),
					new ChatMessage(ChatMessageRole.User, "This logo consists of many small shapes.  What shape are they?",ImageInput.FromImageUrl("https://rogerpincombe.com/templates/rp/center-aligned-no-shadow-small.png"))
				}
			};
			string resultText = "";
			api.Chat.StreamChatAsync(request, delta => resultText += delta?.ToString() ?? "").Wait();
			Assert.IsNotEmpty(resultText);
			Assert.That(resultText.ToLower().Contains("circle") || resultText.ToLower().Contains("spiral"));
		}

		[Test]
		public void VisionConversationStreaming()
		{
			var api = new OpenAI_API.OpenAIAPI();

			var chat = api.Chat.CreateConversation();
			chat.Model = Model.GPT4_Vision;
			chat.RequestParameters.Temperature = 0;

			chat.AppendSystemMessage("You are a graphic design assistant who helps identify colors.");
			chat.AppendUserInput("What are the primary non-white colors in this logo?", ImageInput.FromFile("../../../../OpenAI_API/nuget_logo.png"));
			chat.AppendExampleChatbotOutput("Blue and purple");
			chat.AppendUserInput("What are the primary non-white colors in this logo?", ImageInput.FromImageUrl("https://rogerpincombe.com/templates/rp/center-aligned-no-shadow-small.png"));
			string resultText = "";
			chat.StreamResponseFromChatbotAsync(delta=>resultText+=delta.ToString()).Wait();
			Assert.IsNotEmpty(resultText);
			Assert.That(resultText.ToLower().Contains("blue"));
			Assert.That(resultText.ToLower().Contains("red"));
		}
	}
}
