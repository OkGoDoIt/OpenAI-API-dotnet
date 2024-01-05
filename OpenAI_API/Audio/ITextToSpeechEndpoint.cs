using System.IO;
using System.Threading.Tasks;
using OpenAI_API.Models;

namespace OpenAI_API.Audio
{
	/// <summary>
	/// The Endpoint for the Text to Speech API.  This allows you to generate audio from text.  See <seealso href="https://platform.openai.com/docs/guides/text-to-speech"/>
	/// </summary>
	public interface ITextToSpeechEndpoint
	{
		/// <summary>
		/// This allows you to set default parameters for every request, for example to set a default voice or model.  For every request, if you do not have a parameter set on the request but do have it set here as a default, the request will automatically pick up the default value.
		/// </summary>
		TextToSpeechRequest DefaultTTSRequestArgs { get; set; }

		/// <summary>
		/// Calls the API to create speech from text, and returns the raw stream of the audio file.
		/// </summary>
		/// <param name="request">The text to speech request to submit to the API</param>
		/// <returns>A stream of the audio file in the requested format.</returns>
		Task<Stream> GetSpeechAsStreamAsync(TextToSpeechRequest request);

		/// <summary>
		/// Calls the API to create speech from text, and returns the raw stream of the audio file.
		/// </summary>
		/// <param name="input">The text to generate audio for. The maximum length is 4096 characters.</param>
		/// <param name="voice">The voice to use when generating the audio. Supported voices can be found in <see cref="TextToSpeechRequest.Voices"/>.</param>
		/// <param name="speed">The speed of the generated audio. Select a value from 0.25 to 4.0. 1.0 is the default.</param>
		/// <param name="responseFormat">The default response format is "mp3", but other formats are available in <see cref="TextToSpeechRequest.ResponseFormats"/>.  See <seealso href="https://platform.openai.com/docs/guides/text-to-speech/supported-output-formats"/></param>
		/// <param name="model">TTS is an AI model that converts text to natural sounding spoken text. OpenAI offers two different model variates, <see cref="Model.TTS_Speed"/> is optimized for real time text to speech use cases and <see cref="Model.TTS_HD"/> is optimized for quality.</param>
		/// <returns>A stream of the audio file in the requested format.</returns>
		Task<Stream> GetSpeechAsStreamAsync(string input, string voice = null, double? speed = null, string responseFormat = null, Model model = null);

		/// <summary>
		/// Calls the API to create speech from text, and saves the audio file to disk.
		/// </summary>
		/// <param name="request">The text to speech request to submit to the API</param>
		/// <param name="localPath">The local path to save the audio file to.</param>
		/// <returns>A <see cref="FileInfo"/> representing the saved speech file.</returns>
		Task<FileInfo> SaveSpeechToFileAsync(TextToSpeechRequest request, string localPath);

		/// <summary>
		/// Calls the API to create speech from text, and saves the audio file to disk.
		/// </summary>
		/// <param name="input">The text to generate audio for. The maximum length is 4096 characters.</param>
		/// <param name="localPath">The local path to save the audio file to.</param>
		/// <param name="voice">The voice to use when generating the audio. Supported voices can be found in <see cref="TextToSpeechRequest.Voices"/>.</param>
		/// <param name="speed">The speed of the generated audio. Select a value from 0.25 to 4.0. 1.0 is the default.</param>
		/// <param name="responseFormat">The default response format is "mp3", but other formats are available in <see cref="TextToSpeechRequest.ResponseFormats"/>.  See <seealso href="https://platform.openai.com/docs/guides/text-to-speech/supported-output-formats"/></param>
		/// <param name="model">TTS is an AI model that converts text to natural sounding spoken text. OpenAI offers two different model variates, <see cref="Model.TTS_Speed"/> is optimized for real time text to speech use cases and <see cref="Model.TTS_HD"/> is optimized for quality.</param>
		/// <returns>A stream of the audio file in the requested format.</returns>
		Task<FileInfo> SaveSpeechToFileAsync(string input, string localPath, string voice = null, double? speed = null, string responseFormat = null, Model model = null);

		
	}
}