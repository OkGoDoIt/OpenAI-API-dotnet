using FluentAssertions;
using NUnit.Framework;
using OpenAI_API.Edits;
using OpenAI_API.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenAI_Tests
{
    public class EditEndpointTests
    {
        [SetUp]
        public void Setup()
        {
            OpenAI_API.APIAuthentication.Default = new OpenAI_API.APIAuthentication("TEST_OPENAI_SECRET_KEY");
        }

        [Test]
        public void EditRequest_CanOnlyUseValidModels()
        {
            var api = new OpenAI_API.OpenAIAPI();

            Assert.IsNotNull(api.Edit);

            Assert.That(async () => await api.Edit.CreateEditsAsync(new EditRequest("B A D C E G H F", "Correct the alphabets order", model: Model.ChatGPTTurbo, temperature: 0.0)), 
                Throws.Exception
                .TypeOf<ArgumentException>());
            
        }

        [Test]
        public async Task GetBasicEdit()
        {
            var api = new OpenAI_API.OpenAIAPI();

            Assert.IsNotNull(api.Edit);

            var results = await api.Edit.CreateEditsAsync(new EditRequest("B A D C E G H F", "Correct the alphabets order", model: Model.TextDavinciEdit, temperature: 0.0));
            Assert.IsNotNull(results);
            Assert.NotNull(results.CreatedUnixTime);
            Assert.NotZero(results.CreatedUnixTime.Value);
            Assert.NotNull(results.Created);
            Assert.NotNull(results.Choices);
            Assert.NotZero(results.Choices.Count);
            Assert.That(results.Choices.Any(c => c.Text.Trim().ToLower().StartsWith("a")));
        }

        [Test]
        public async Task GetSimpleEdit()
        {
            var api = new OpenAI_API.OpenAIAPI();

            Assert.IsNotNull(api.Edit);

            var results = await api.Edit.CreateEditsAsync("B A D C E G H F", "correct the lphabets sequence", temperature: 0);
            Assert.IsNotNull(results);
            Assert.NotNull(results.Choices);
            Assert.NotZero(results.Choices.Count);
            Assert.That(results.Choices.Any(c => c.Text.Trim().ToLower().StartsWith("a")));
        }


        [Test]
        public async Task EditsUsageDataWorks()
        {
            var api = new OpenAI_API.OpenAIAPI();

            Assert.IsNotNull(api.Edit);

            var results = await api.Edit.CreateEditsAsync(new EditRequest("B A D C E G H F", "Correct the alphabets sequence", model: Model.TextDavinciEdit, temperature: 0));
            Assert.IsNotNull(results);
            Assert.IsNotNull(results.Usage);
            Assert.Greater(results.Usage.PromptTokens, 1);
            Assert.Greater(results.Usage.CompletionTokens, 0);
            Assert.GreaterOrEqual(results.Usage.TotalTokens, results.Usage.PromptTokens + results.Usage.CompletionTokens);
        }

        [Test]
        public async Task GetEdits_ReturnsResultString()
        {
            var api = new OpenAI_API.OpenAIAPI();

            Assert.IsNotNull(api.Edit);

            var result = await api.Edit.GetEdits("B A D C E G H F", "Correct the alphabets sequence");
            Assert.IsNotNull(result);
            Assert.That(result.ToLower().StartsWith("a"));
        }

        [Test]
        public async Task CreateEditsAsync_ShouldGenerateMultipleChoices()
        {
            var api = new OpenAI_API.OpenAIAPI();

            var req = new EditRequest
            {
                Input = "B A D C E G H F",
                Instruction = "Correct the alphabets sequence",
                Temperature = 0,
                NumChoicesPerPrompt = 2
            };

            var result = await api.Edit.CreateEditsAsync(req);
            Assert.IsNotNull(result);
            result.Choices.Count.Should().Be(2, "NumChoisesPerPropmt is set to 2");
        }
    }
}
