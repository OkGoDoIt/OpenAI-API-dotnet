using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenAI_API;

namespace OpenAI_Tests
{
	public class CompletionEndpointTests
	{
		[SetUp]
		public void Setup()
		{
			APIAuthentication.Default = new APIAuthentication(Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY"));
		}

		private OpenAIAPI GetApi => new OpenAIAPI(engine: Engine.Ada);

		[Test]
		public async Task CreateCompletionAsync_GetBasicCompletion_ShouldReturnData()
		{
			var api = GetApi;

			Assert.IsNotNull(api.Completions);
			
			var results = await api.Completions.CreateCompletionsAsync(new CompletionRequest("One Two Three Four Five Six Seven Eight Nine One Two Three Four Five Six Seven Eight", temperature: 0.1, max_tokens: 5));
			results.ShouldNotBeEmpty();
			results.ShouldContainAStringStartingWith("nine");
		}

		[Test]
		public async Task CreateCompletionAsync_MultiplePrompts_ShouldReturnResult()
		{
			var api = GetApi;

			var completionReq = new CompletionRequest
			{
				MultiplePrompts = new []
				{
					"one two three",
					"10 11 12 13"
				},
				Temperature = 0,
				MaxTokens = 3
			};

			var results = await api.Completions.CreateCompletionsAsync(completionReq);
			results.ShouldNotBeEmpty();
			results.ShouldContainAStringStartingWith("four", "completion should contain next digit");
			results.ShouldContainAStringStartingWith("14", "completion should contain next number");
		}

		[TestCase(2)]
		[TestCase(5)]
		[TestCase(10)]
		public async Task CreateCompletionAsync_ShouldReturnProvidedMaxTokens(int maxTokens)
		{
			var api = GetApi;

			var completionReq = new CompletionRequest
			{
				Prompt = "three four five",
				Temperature = 0,
				MaxTokens = maxTokens
			};

			var results = await api.Completions.CreateCompletionsAsync(completionReq, 1);
			results.ShouldNotBeEmpty();

			results.Completions[0].Text?.Split(" ", StringSplitOptions.RemoveEmptyEntries).Length
				.Should().Be(maxTokens, $"{maxTokens} should be generated");
		}

		[TestCase(-0.2)]
		[TestCase(3)]
		public void CreateCompletionAsync_ShouldNotAllowTemperatureOutside01(double temperature)
		{
			var api = GetApi;

			var completionReq = new CompletionRequest
			{
				Prompt = "three four five",
				Temperature = temperature,
				MaxTokens = 10
			};

			Func<Task> act = () => api.Completions.CreateCompletionsAsync(completionReq, 1);
			act.Should()
				.Throw<HttpRequestException>()
				.Where(exc => exc.Message.Contains("temperature"));
		}

		[TestCase(1.8)]
		[TestCase(1.9)]
		[TestCase(2.0)]
		public async Task ShouldBeMoreCreativeWithHighTemperature(double temperature)
		{
			var api = GetApi;

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
			var api = GetApi;

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

		[TestCase(-0.5)]
		[TestCase(0.0)]
		[TestCase(0.5)]
		[TestCase(1.0)]
		public async Task CreateCompletionAsync_ShouldReturnSomeResultsForPresencePenalty(double presencePenalty)
		{
			var api = GetApi;

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

		[TestCase(-0.5)]
		[TestCase(0.0)]
		[TestCase(0.5)]
		[TestCase(1.0)]
		public async Task CreateCompletionAsync_ShouldReturnSomeResultsForFrequencyPenalty(double frequencyPenalty)
		{
			var api = GetApi;

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
			var api = GetApi;

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
		[TestCase(10)]
		public async Task CreateCompletionAsync_ShouldAlsoReturnLogProps(int logProps)
		{
			var api = GetApi;

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
			var api = GetApi;

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

		[TestCase("eight")]
		[TestCase("nine")]
		public async Task CreateCompletionAsync_ShouldStopOnStopSequence(string stopSeq)
		{
			var api = GetApi;

			var completionReq = new CompletionRequest
			{
				Prompt = "three four five",
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
			var api = GetApi;

			var r = new CompletionRequest
			{
				Prompt = "three four five",
				MaxTokens = 5,
				Temperature = 0,
				TopP = 0.1,
				PresencePenalty = 0.5,
				FrequencyPenalty = 0.3,
				NumChoicesPerPrompt = 2,
				Logprobs = 3,
				Echo = true
			};

			var resultOneParam = await api.Completions.CreateCompletionsAsync(r);
			resultOneParam.ShouldNotBeEmpty();

			var resultsMultipleParams = await api.Completions.CreateCompletionAsync(
				r.Prompt, r.MaxTokens, r.Temperature, r.TopP, r.NumChoicesPerPrompt, r.PresencePenalty,
				r.FrequencyPenalty,
				r.Logprobs, r.Echo);
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
			var api = GetApi;

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
			var api = GetApi;

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
			var api = GetApi;

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
			var api = GetApi;

			var r = new CompletionRequest
			{
				Prompt = "three four five",
				MaxTokens = 5,
				Temperature = 0,
				TopP = 0.1,
				PresencePenalty = 0.5,
				FrequencyPenalty = 0.3,
				NumChoicesPerPrompt = 2,
				Logprobs = 3,
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
				r.Prompt, r.MaxTokens, r.Temperature, r.TopP, r.NumChoicesPerPrompt, r.PresencePenalty,
				r.FrequencyPenalty,
				r.Logprobs, r.Echo))
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
}