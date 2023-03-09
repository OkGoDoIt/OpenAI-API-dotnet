using NUnit.Framework;
using OpenAI_API.Chat;
using OpenAI_API.Completions;
using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpenAI_Tests
{
	public class ChatEndpointTests
	{
		[SetUp]
		public void Setup()
		{
			OpenAI_API.APIAuthentication.Default = new OpenAI_API.APIAuthentication(Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY"));
		}

		[Test]
		public void BasicCompletion()
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.Chat);

			var results = api.Chat.CreateChatCompletionAsync(new ChatRequest()
			{
				Model = Model.ChatGPTTurbo,
				Temperature = 0.1,
				MaxTokens = 5,
				Messages = new ChatMessage[] {
					new ChatMessage(ChatMessageRole.User, "Hello!")
				}
			}).Result;
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
			Assert.NotNull(results.Object);
			Assert.NotNull(results.Choices);
			Assert.NotZero(results.Choices.Count);
			Assert.AreEqual(ChatMessageRole.Assistant, results.Choices[0].Message.Role);
			Assert.That(results.Choices.All(c => c.Message.Role.Equals(ChatMessageRole.Assistant)));
			Assert.That(results.Choices.All(c => c.Message.Content.Length > 1));
		}

		[Test]
		public void SimpleCompletion()
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.Chat);

			var results = api.Chat.CreateChatCompletionAsync("Hello!").Result;
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
			Assert.NotNull(results.Object);
			Assert.NotNull(results.Choices);
			Assert.NotZero(results.Choices.Count);
			Assert.AreEqual(ChatMessageRole.Assistant, results.Choices[0].Message.Role);
			Assert.That(results.Choices.All(c => c.Message.Role.Equals(ChatMessageRole.Assistant)));
			Assert.That(results.Choices.All(c => c.Message.Role == ChatMessageRole.Assistant));
			Assert.That(results.Choices.All(c => c.Message.Content.Length > 1));
			Assert.IsNotEmpty(results.ToString());
		}

		[Test]
		public void ChatBackAndForth()
		{
			var api = new OpenAI_API.OpenAIAPI();

			var chat = api.Chat.CreateConversation();

			chat.AppendSystemMessage("You are a teacher who helps children understand if things are animals or not.  If the user tells you an animal, you say \"yes\".  If the user tells you something that is not an animal, you say \"no\".  You only ever respond with \"yes\" or \"no\".  You do not say anything else.");
			chat.AppendUserInput("Is this an animal? Cat");
			chat.AppendExampleChatbotOutput("Yes");
			chat.AppendUserInput("Is this an animal? House");
			chat.AppendExampleChatbotOutput("No");
			chat.AppendUserInput("Is this an animal? Dog");
			string res = chat.GetResponseFromChatbot().Result;
			Assert.NotNull(res);
			Assert.IsNotEmpty(res);
			Assert.AreEqual("Yes", res.Trim());
			chat.AppendUserInput("Is this an animal? Chair");
			res = chat.GetResponseFromChatbot().Result;
			Assert.NotNull(res);
			Assert.IsNotEmpty(res);
			Assert.AreEqual("No", res.Trim());

			foreach (ChatMessage msg in chat.Messages)
			{
				Console.WriteLine($"{msg.Role}: {msg.Content}");
			}
		}

		[Test]
		public async Task StreamCompletionEnumerableAsync_ShouldStreamData()
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.Chat);

			var req = new ChatRequest()
			{
				Model = Model.ChatGPTTurbo,
				Temperature = 0.2,
				MaxTokens = 500,
				Messages = new ChatMessage[] {
					new ChatMessage(ChatMessageRole.User, "Please explain how mountains are formed in great detail.")
				}
			};

			var chatResults = new List<ChatResult>();
			await foreach (var res in api.Chat.StreamChatEnumerableAsync(req))
			{
				chatResults.Add(res);
			}

			Assert.Greater(chatResults.Count, 100);
			Assert.That(chatResults.Select(cr => cr.Choices[0].Delta.Content).Count(c => !string.IsNullOrEmpty(c)) > 50);
		}

	}
}
