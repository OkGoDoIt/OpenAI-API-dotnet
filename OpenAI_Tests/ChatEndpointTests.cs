using NUnit.Framework;
using OpenAI_API.Chat;
using OpenAI_API.Completions;
using OpenAI_API.Models;
using OpenAI_API.Moderation;
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
		public void BasicCompletionWithNames()
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.Chat);

			var results = api.Chat.CreateChatCompletionAsync(new ChatRequest()
			{
				Model = Model.ChatGPTTurbo,
				Temperature = 0.1,
				MaxTokens = 5,
				Messages = new ChatMessage[] {
					new ChatMessage(ChatMessageRole.System, "You are the moderator in this workplace chat.  Answer any questions asked of the participants."),
					new ChatMessage(ChatMessageRole.User, "Hello everyone") { Name="John"},
					new ChatMessage(ChatMessageRole.User, "Good morning all")  { Name="Edward"},
					new ChatMessage(ChatMessageRole.User, "Is John here?  Answer yes or no.") { Name = "Cindy" }
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
			Assert.That(results.ToString().ToLower().Contains("yes"));
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

		[TestCase("gpt-3.5-turbo")]
		[TestCase("gpt-4")]
		public void ChatBackAndForth(string model)
		{
			var api = new OpenAI_API.OpenAIAPI();

			var chat = api.Chat.CreateConversation();
			chat.Model = model;
			chat.RequestParameters.Temperature = 0;

			chat.AppendSystemMessage("You are a teacher who helps children understand if things are animals or not.  If the user tells you an animal, you say \"yes\".  If the user tells you something that is not an animal, you say \"no\".  You only ever respond with \"yes\" or \"no\".  You do not say anything else.");
			chat.AppendUserInput("Is this an animal? Cat");
			chat.AppendExampleChatbotOutput("Yes");
			chat.AppendUserInput("Is this an animal? House");
			chat.AppendExampleChatbotOutput("No");
			chat.AppendUserInput("Is this an animal? Dog");
			string res = chat.GetResponseFromChatbotAsync().Result;
			Assert.NotNull(res);
			Assert.IsNotEmpty(res);
			Assert.AreEqual("Yes", res.Trim());
			chat.AppendUserInput("Is this an animal? Chair");
			res = chat.GetResponseFromChatbotAsync().Result;
			Assert.NotNull(res);
			Assert.IsNotEmpty(res);
			Assert.AreEqual("No", res.Trim());
		}

		[Test]
		public void ChatWithNames()
		{
			var api = new OpenAI_API.OpenAIAPI();

			var chat = api.Chat.CreateConversation();
			chat.RequestParameters.Temperature = 0;

			chat.AppendSystemMessage("You are the moderator in this workplace chat.  Answer any questions asked of the participants.");
			chat.AppendUserInputWithName("John", "Hello everyone");
			chat.AppendUserInputWithName("Edward", "Good morning all");
			chat.AppendUserInputWithName("Cindy", "Is John here?  Answer yes or no.");
			chat.AppendExampleChatbotOutput("Yes");
			chat.AppendUserInputWithName("Cindy", "Is Monica here?  Answer yes or no.");
			string res = chat.GetResponseFromChatbotAsync().Result;
			Assert.NotNull(res);
			Assert.IsNotEmpty(res);
			Assert.That(res.ToLower().Contains("no"));
			chat.AppendUserInputWithName("Cindy", "Is Edward here?  Answer yes or no.");
			res = chat.GetResponseFromChatbotAsync().Result;
			Assert.NotNull(res);
			Assert.IsNotEmpty(res);
			Assert.That(res.ToLower().Contains("yes"));
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

		[Test]
		public async Task StreamingConversation()
		{
			var api = new OpenAI_API.OpenAIAPI();

			var chat = api.Chat.CreateConversation();
			chat.RequestParameters.MaxTokens = 500;
			chat.RequestParameters.Temperature = 0.2;
			chat.Model = Model.ChatGPTTurbo;

			chat.AppendSystemMessage("You are a helpful assistant who is really good at explaining things to students.");
			chat.AppendUserInput("Please explain to me how mountains are formed in great detail.");

			string result = "";
			int streamParts = 0;

			await foreach (var streamResultPart in chat.StreamResponseEnumerableFromChatbotAsync())
			{
				result += streamResultPart;
				streamParts++;
			}

			Assert.NotNull(result);
			Assert.IsNotEmpty(result);
			Assert.That(result.ToLower().Contains("mountains"));
			Assert.Greater(result.Length, 200);
			Assert.Greater(streamParts, 5);

			Assert.AreEqual(ChatMessageRole.User, chat.Messages.Last().Role);
			Assert.AreEqual(result, chat.Messages.Last().Content);
		}

	}
}
