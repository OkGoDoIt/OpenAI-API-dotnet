using OpenAI_API.Models;
using System.Threading.Tasks;

namespace OpenAI_API.Moderation
{
	/// <summary>
	/// An interface for <see cref="ModerationEndpoint"/>, which classifies text against the OpenAI Content Policy
	/// </summary>
	public interface IModerationEndpoint
	{
		/// <summary>
		/// This allows you to send request to the recommended model without needing to specify. OpenAI recommends using the <see cref="Model.TextModerationLatest"/> model
		/// </summary>
		ModerationRequest DefaultModerationRequestArgs { get; set; }

		/// <summary>
		/// Ask the API to classify the text using a custom request.
		/// </summary>
		/// <param name="request">Request to send to the API</param>
		/// <returns>Asynchronously returns the classification result</returns>
		Task<ModerationResult> CallModerationAsync(ModerationRequest request);

		/// <summary>
		/// Ask the API to classify the text using the default model.
		/// </summary>
		/// <param name="input">Text to classify</param>
		/// <returns>Asynchronously returns the classification result</returns>
		Task<ModerationResult> CallModerationAsync(string input);
	}
}