using System.Threading.Tasks;

namespace OpenAI_API.Moderation
{
	/// <summary>
	/// An interface for <see cref="ModerationEndpoint"/>, for ease of mock testing, etc
	/// </summary>
	public interface IModerationEndpoint
	{
		ModerationRequest DefaultModerationRequestArgs { get; set; }

		Task<ModerationResult> CallModerationAsync(ModerationRequest request);
		Task<ModerationResult> CallModerationAsync(string input);
	}
}