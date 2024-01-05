using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using static System.Net.WebRequestMethods;

namespace OpenAI_API.Audio
{
	/// <summary>
	/// The Endpoint for the Text to Speech API.  This allows you to generate audio from text.  See <seealso href="https://platform.openai.com/docs/guides/text-to-speech"/>
	/// </summary>
	public class TextToSpeechEndpoint : EndpointBase, ITextToSpeechEndpoint
	{
		/// <inheritdoc/>
		protected override string Endpoint => "audio/speech";

		/// <summary>
		/// This allows you to set default parameters for every request, for example to set a default voice or model.  For every request, if you do not have a parameter set on the request but do have it set here as a default, the request will automatically pick up the default value.
		/// </summary>
		public TextToSpeechRequest DefaultTTSRequestArgs { get; set; } = new TextToSpeechRequest();

		/// <summary>
		/// Constructor of the api endpoint.  Rather than instantiating this yourself, access it through an instance of <see cref="OpenAIAPI"/> as <see cref="OpenAIAPI.TextToSpeech"/>.
		/// </summary>
		/// <param name="api">Pass in the instance of the api</param>
		internal TextToSpeechEndpoint(OpenAIAPI api) : base(api) { }

		/// <summary>
		/// Calls the API to create speech from text, and returns the raw stream of the audio file.
		/// </summary>
		/// <param name="request">The text to speech request to submit to the API</param>
		/// <returns>A stream of the audio file in the requested format.</returns>
		public async Task<Stream> GetSpeechAsStreamAsync(TextToSpeechRequest request)
		{
			return await HttpRequest(verb: HttpMethod.Post, postData: request);
		}

		/// <summary>
		/// Calls the API to create speech from text, and returns the raw stream of the audio file.
		/// </summary>
		/// <param name="input">The text to generate audio for. The maximum length is 4096 characters.</param>
		/// <param name="voice">The voice to use when generating the audio. Supported voices can be found in <see cref="TextToSpeechRequest.Voices"/>.</param>
		/// <param name="speed">The speed of the generated audio. Select a value from 0.25 to 4.0. 1.0 is the default.</param>
		/// <param name="responseFormat">The default response format is "mp3", but other formats are available in <see cref="TextToSpeechRequest.ResponseFormats"/>.  See <seealso href="https://platform.openai.com/docs/guides/text-to-speech/supported-output-formats"/></param>
		/// <param name="model">TTS is an AI model that converts text to natural sounding spoken text. OpenAI offers two different model variates, <see cref="Model.TTS_Speed"/> is optimized for real time text to speech use cases and <see cref="Model.TTS_HD"/> is optimized for quality.</param>
		/// <returns>A stream of the audio file in the requested format.</returns>
		public async Task<Stream> GetSpeechAsStreamAsync(string input, string voice = null, double? speed = null, string responseFormat = null, Model model = null)
		{
			var request = new TextToSpeechRequest()
			{
				Input = input,
				Voice = voice ?? DefaultTTSRequestArgs.Voice,
				Speed = speed ?? DefaultTTSRequestArgs.Speed,
				Model = model ?? DefaultTTSRequestArgs.Model,
				ResponseFormat = responseFormat ?? DefaultTTSRequestArgs.ResponseFormat
			};
			return await HttpRequest(verb: HttpMethod.Post, postData: request);
		}

		/// <summary>
		/// Calls the API to create speech from text, and saves the audio file to disk.
		/// </summary>
		/// <param name="request">The text to speech request to submit to the API</param>
		/// <param name="localPath">The local path to save the audio file to.</param>
		/// <returns>A <see cref="FileInfo"/> representing the saved speech file.</returns>
		public async Task<FileInfo> SaveSpeechToFileAsync(TextToSpeechRequest request, string localPath)
		{
			using (var stream = await GetSpeechAsStreamAsync(request))
			using (var outputFileStream = new FileStream(localPath, FileMode.Create))
			{
				await stream.CopyToAsync(outputFileStream);
			}
			return new FileInfo(localPath);
		}

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
		public async Task<FileInfo> SaveSpeechToFileAsync(string input, string localPath, string voice = null, double? speed = null, string responseFormat = null, Model model = null)
		{
			var request = new TextToSpeechRequest()
			{
				Input = input,
				Voice = voice ?? DefaultTTSRequestArgs.Voice,
				Speed = speed ?? DefaultTTSRequestArgs.Speed,
				Model = model ?? DefaultTTSRequestArgs.Model,
				ResponseFormat = responseFormat ?? DefaultTTSRequestArgs.ResponseFormat
			};
			return await SaveSpeechToFileAsync(request, localPath);
		}



	}
}
