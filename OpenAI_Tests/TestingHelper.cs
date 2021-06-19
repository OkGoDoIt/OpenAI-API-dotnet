using FluentAssertions;
using OpenAI_API;

namespace OpenAI_Tests
{
	public static class TestingHelper
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
