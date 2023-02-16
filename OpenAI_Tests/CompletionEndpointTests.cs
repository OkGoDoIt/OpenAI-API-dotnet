using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using System.Collections.Generic;
using System.Net.Http;
using OpenAI_API.Completions;
using OpenAI_API.Models;

namespace OpenAI_Tests
{
	public class CompletionEndpointTests
	{
		[SetUp]
		public void Setup()
		{
			OpenAI_API.APIAuthentication.Default = new OpenAI_API.APIAuthentication(Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY"));
		}

		[Test]
		public void GetBasicCompletion()
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.Completions);

			var results = api.Completions.CreateCompletionsAsync(new CompletionRequest("One Two Three Four Five Six Seven Eight Nine One Two Three Four Five Six Seven Eight", model: Model.CurieText, temperature: 0.1, max_tokens: 5)).Result;
			Assert.IsNotNull(results);
			Assert.NotNull(results.CreatedUnixTime);
			Assert.NotZero(results.CreatedUnixTime.Value);
			Assert.NotNull(results.Created);
			Assert.Greater(results.Created.Value, DateTime.UtcNow.AddSeconds(-30));
			Assert.Less(results.Created.Value, DateTime.UtcNow.AddSeconds(30));
			Assert.NotNull(results.Completions);
			Assert.NotZero(results.Completions.Count);
			Assert.That(results.Completions.Any(c => c.Text.Trim().ToLower().StartsWith("nine")));
		}


		[Test]
		public void GetSimpleCompletion()
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.Completions);

			var results = api.Completions.CreateCompletionAsync("One Two Three Four Five Six Seven Eight Nine One Two Three Four Five Six Seven Eight", temperature: 0.1, max_tokens: 5).Result;
			Assert.IsNotNull(results);
			Assert.NotNull(results.Completions);
			Assert.NotZero(results.Completions.Count);
			Assert.That(results.Completions.Any(c => c.Text.Trim().ToLower().StartsWith("nine")));
		}


		[Test]
		public void CompletionUsageDataWorks()
		{
			var api = new OpenAI_API.OpenAIAPI();

			Assert.IsNotNull(api.Completions);

			var results = api.Completions.CreateCompletionsAsync(new CompletionRequest("One Two Three Four Five Six Seven Eight Nine One Two Three Four Five Six Seven Eight", model: Model.CurieText, temperature: 0.1, max_tokens: 5)).Result;
			Assert.IsNotNull(results);
			Assert.IsNotNull(results.Usage);
			Assert.Greater(results.Usage.PromptTokens, 15);
			Assert.Greater(results.Usage.CompletionTokens, 0);
			Assert.GreaterOrEqual(results.Usage.TotalTokens, results.Usage.PromptTokens + results.Usage.CompletionTokens);
		}


		[Test]
		public async Task CreateCompletionAsync_MultiplePrompts_ShouldReturnResult()
		{
			var api = new OpenAI_API.OpenAIAPI();

			var completionReq = new CompletionRequest
			{
				MultiplePrompts = new[]
				{
					"Today is Monday, tomorrow is",
					"10 11 12 13 14"
				},
				Temperature = 0,
				MaxTokens = 3
			};

			var results = await api.Completions.CreateCompletionsAsync(completionReq);
			results.ShouldNotBeEmpty();
			results.ShouldContainAStringStartingWith("tuesday", "completion should contain next day");
			results.ShouldContainAStringStartingWith("15", "completion should contain next number");
		}


		[TestCase(-0.2)]
		[TestCase(3)]
		public void CreateCompletionAsync_ShouldNotAllowTemperatureOutside01(double temperature)
		{
			var api = new OpenAI_API.OpenAIAPI();

			var completionReq = new CompletionRequest
			{
				Prompt = "three four five",
				Temperature = temperature,
				MaxTokens = 10
			};

			Func<Task> act = () => api.Completions.CreateCompletionsAsync(completionReq, 1);
			act.Should()
				.ThrowAsync<HttpRequestException>()
				.Where(exc => exc.Message.Contains("temperature"));
		}

		[TestCase(1.8)]
		[TestCase(1.9)]
		[TestCase(2.0)]
		public async Task ShouldBeMoreCreativeWithHighTemperature(double temperature)
		{
			var api = new OpenAI_API.OpenAIAPI();

			var completionReq = new CompletionRequest
			{
				Prompt = "three four five",
				Temperature = temperature,
				MaxTokens = 5
			};

			var results = await api.Completions.CreateCompletionsAsync(completionReq);
			results.ShouldNotBeEmpty();
			results.Completions.Count.Should().Be(5, "completion count should be the default");
			results.Completions.Distinct().Count().Should().Be(results.Completions.Count);
		}

		[TestCase(0.05)]
		[TestCase(0.1)]
		public async Task CreateCompletionAsync_ShouldGetSomeResultsWithVariousTopPValues(double topP)
		{
			var api = new OpenAI_API.OpenAIAPI();

			var completionReq = new CompletionRequest
			{
				Prompt = "three four five",
				Temperature = 0,
				MaxTokens = 5,
				TopP = topP
			};

			var results = await api.Completions.CreateCompletionsAsync(completionReq);
			results.ShouldNotBeEmpty();
			results.Completions.Count.Should().Be(5, "completion count should be the default");
		}

		//[TestCase(-0.5)]  OpenAI returns a 500 error, is this supposed to work?
		[TestCase(0.0)]
		[TestCase(0.5)]
		[TestCase(1.0)]
		public async Task CreateCompletionAsync_ShouldReturnSomeResultsForPresencePenalty(double presencePenalty)
		{
			var api = new OpenAI_API.OpenAIAPI();

			var completionReq = new CompletionRequest
			{
				Prompt = "three four five",
				Temperature = 0,
				MaxTokens = 5,
				PresencePenalty = presencePenalty
			};

			var results = await api.Completions.CreateCompletionsAsync(completionReq);
			results.ShouldNotBeEmpty();
			results.Completions.Count.Should().Be(5, "completion count should be the default");
		}

		//[TestCase(-0.5)]  OpenAI returns a 500 error, is this supposed to work?
		[TestCase(0.0)]
		[TestCase(0.5)]
		[TestCase(1.0)]
		public async Task CreateCompletionAsync_ShouldReturnSomeResultsForFrequencyPenalty(double frequencyPenalty)
		{
			var api = new OpenAI_API.OpenAIAPI();

			var completionReq = new CompletionRequest
			{
				Prompt = "three four five",
				Temperature = 0,
				MaxTokens = 5,
				FrequencyPenalty = frequencyPenalty
			};

			var results = await api.Completions.CreateCompletionsAsync(completionReq);
			results.ShouldNotBeEmpty();
			results.Completions.Count.Should().Be(5, "completion count should be the default");
		}

		[Test]
		public async Task CreateCompletionAsync_ShouldWorkForBiggerNumberOfCompletions()
		{
			var api = new OpenAI_API.OpenAIAPI();

			var completionReq = new CompletionRequest
			{
				Prompt = "three four five",
				Temperature = 0,
				MaxTokens = 5,
				NumChoicesPerPrompt = 2
			};

			var results = await api.Completions.CreateCompletionsAsync(completionReq);
			results.ShouldNotBeEmpty();
			results.Completions.Count.Should().Be(5, "completion count should be the default");
		}

		[TestCase(1)]
		[TestCase(2)]
		[TestCase(5)]
		public async Task CreateCompletionAsync_ShouldAlsoReturnLogProps(int logProps)
		{
			var api = new OpenAI_API.OpenAIAPI();

			var completionReq = new CompletionRequest
			{
				Prompt = "three four five",
				Temperature = 0,
				MaxTokens = 5,
				Logprobs = logProps
			};

			var results = await api.Completions.CreateCompletionsAsync(completionReq);
			results.ShouldNotBeEmpty();
			results.Completions.Count.Should().Be(5, "completion count should be the default");
			results.Completions[0].Logprobs.TopLogprobs.Count.Should()
				.Be(5, "logprobs should be returned for each completion");
			results.Completions[0].Logprobs.TopLogprobs[0].Keys.Count.Should().Be(logProps,
				"because logprops count should be the same as requested");
		}

		[Test]
		public async Task CreateCompletionAsync_Echo_ShouldReturnTheInput()
		{
			var api = new OpenAI_API.OpenAIAPI();

			var completionReq = new CompletionRequest
			{
				Prompt = "three four five",
				Temperature = 0,
				MaxTokens = 5,
				Echo = true
			};

			var results = await api.Completions.CreateCompletionsAsync(completionReq);
			results.ShouldNotBeEmpty();
			results.Completions.Should().OnlyContain(c => c.Text.StartsWith(completionReq.Prompt), "Echo should get the prompt back");
		}

		[TestCase("Thursday")]
		[TestCase("Friday")]
		public async Task CreateCompletionAsync_ShouldStopOnStopSequence(string stopSeq)
		{
			var api = new OpenAI_API.OpenAIAPI();

			var completionReq = new CompletionRequest
			{
				Prompt = "Monday Tuesday Wednesday",
				Temperature = 0,
				MaxTokens = 5,
				Echo = true,
				StopSequence = stopSeq
			};

			var results = await api.Completions.CreateCompletionsAsync(completionReq);
			results.ShouldNotBeEmpty();
			results.Completions.Should().OnlyContain(c => !c.Text.Contains(stopSeq), "Stop sequence must not be returned");
			results.Completions.Should().OnlyContain(c => c.FinishReason == "stop", "must end due to stop sequence");
		}

		[Test]
		public async Task CreateCompletionAsync_MultipleParamShouldReturnTheSameDataAsSingleParamVersion()
		{
			var api = new OpenAI_API.OpenAIAPI();

			var r = new CompletionRequest
			{
				Prompt = "three four five",
				MaxTokens = 5,
				Temperature = 0,
				TopP = 0.1,
				PresencePenalty = 0.5,
				FrequencyPenalty = 0.3,
				NumChoicesPerPrompt = 2,
				Echo = true
			};

			var resultOneParam = await api.Completions.CreateCompletionsAsync(r);
			resultOneParam.ShouldNotBeEmpty();

			var resultsMultipleParams = await api.Completions.CreateCompletionAsync(
				r.Prompt, Model.DefaultModel, r.MaxTokens, r.Temperature, r.TopP, r.NumChoicesPerPrompt, r.PresencePenalty,
				r.FrequencyPenalty,
				null, r.Echo);
			resultsMultipleParams.ShouldNotBeEmpty();

			resultOneParam.Should().BeEquivalentTo(resultsMultipleParams, opt => opt
					.Excluding(o => o.Id)
					.Excluding(o => o.CreatedUnixTime)
					.Excluding(o => o.Created)
					.Excluding(o => o.ProcessingTime)
					.Excluding(o => o.RequestId)
			);
		}

		[TestCase(5, 3)]
		[TestCase(7, 2)]
		public async Task StreamCompletionAsync_ShouldStreamIndexAndData(int maxTokens, int numOutputs)
		{
			var api = new OpenAI_API.OpenAIAPI();

			var completionRequest = new CompletionRequest
			{
				Prompt = "three four five",
				MaxTokens = maxTokens,
				NumChoicesPerPrompt = numOutputs,
				Temperature = 0,
				TopP = 0.1,
				PresencePenalty = 0.5,
				FrequencyPenalty = 0.3,
				Logprobs = 3,
				Echo = true,
			};

			var streamIndexes = new List<int>();
			var completionResults = new List<CompletionResult>();
			await api.Completions.StreamCompletionAsync(completionRequest, (index, result) =>
			{
				streamIndexes.Add(index);
				completionResults.Add(result);
			});

			int expectedCount = maxTokens * numOutputs;
			streamIndexes.Count.Should().Be(expectedCount);
			completionResults.Count.Should().Be(expectedCount);
		}

		[TestCase(5, 3)]
		[TestCase(7, 2)]
		public async Task StreamCompletionAsync_ShouldStreamData(int maxTokens, int numOutputs)
		{
			var api = new OpenAI_API.OpenAIAPI();

			var completionRequest = new CompletionRequest
			{
				Prompt = "three four five",
				MaxTokens = maxTokens,
				NumChoicesPerPrompt = numOutputs,
				Temperature = 0,
				TopP = 0.1,
				PresencePenalty = 0.5,
				FrequencyPenalty = 0.3,
				Logprobs = 3,
				Echo = true,
			};

			var completionResults = new List<CompletionResult>();
			await api.Completions.StreamCompletionAsync(completionRequest, result =>
			{
				completionResults.Add(result);
			});

			int expectedCount = maxTokens * numOutputs;
			completionResults.Count.Should().Be(expectedCount);
		}

		[TestCase(5, 3)]
		[TestCase(7, 2)]
		public async Task StreamCompletionEnumerableAsync_ShouldStreamData(int maxTokens, int numOutputs)
		{
			var api = new OpenAI_API.OpenAIAPI();

			var completionRequest = new CompletionRequest
			{
				Prompt = "three four five",
				MaxTokens = maxTokens,
				NumChoicesPerPrompt = numOutputs,
				Temperature = 0,
				TopP = 0.1,
				PresencePenalty = 0.5,
				FrequencyPenalty = 0.3,
				Logprobs = 3,
				Echo = true,
			};

			var completionResults = new List<CompletionResult>();
			await foreach (var res in api.Completions.StreamCompletionEnumerableAsync(completionRequest))
			{
				completionResults.Add(res);
			}

			int expectedCount = maxTokens * numOutputs;
			completionResults.Count.Should().Be(expectedCount);
		}

		[Test]
		public async Task StreamCompletionEnumerableAsync_MultipleParamShouldReturnTheSameDataAsSingleParamVersion()
		{
			var api = new OpenAI_API.OpenAIAPI();

			var r = new CompletionRequest
			{
				Prompt = "three four five",
				MaxTokens = 5,
				Temperature = 0,
				TopP = 0.1,
				PresencePenalty = 0.5,
				FrequencyPenalty = 0.3,
				NumChoicesPerPrompt = 2,
				Logprobs = null,
				Echo = true
			};

			var resultsOneParam = new List<CompletionResult>();
			await foreach (var res in api.Completions.StreamCompletionEnumerableAsync(r))
			{
				resultsOneParam.Add(res);
			}

			resultsOneParam.Should().NotBeEmpty("At least one result should be fetched");

			var resultsMultipleParams = new List<CompletionResult>();
			await foreach (var res in api.Completions.StreamCompletionEnumerableAsync(
				r.Prompt, Model.DefaultModel, r.MaxTokens, r.Temperature, r.TopP, r.NumChoicesPerPrompt, r.PresencePenalty,
				r.FrequencyPenalty,
				null, r.Echo))
			{
				resultsMultipleParams.Add(res);
			}
			resultsMultipleParams.Should().NotBeEmpty();

			resultsOneParam.Should().BeEquivalentTo(resultsMultipleParams, opt => opt
				.Excluding(o => o.Id)
				.Excluding(o => o.CreatedUnixTime)
				.Excluding(o => o.Created)
				.Excluding(o => o.ProcessingTime)
				.Excluding(o => o.RequestId)
			);
		}
	}

	public static class CompletionTestingHelper
	{
		public static void ShouldNotBeEmpty(this CompletionResult results)
		{
			results.Should().NotBeNull("a result must be received");
			results.Completions.Should().NotBeNull("completions must be received");
			results.Completions.Should().NotBeEmpty("completions must be non-empty");
		}

		public static void ShouldContainAStringStartingWith(this CompletionResult results, string startToken, string because = "")
		{
			results.Completions.Should().Contain(c => c.Text.Trim().ToLower().StartsWith(startToken), because);
		}
	}
}