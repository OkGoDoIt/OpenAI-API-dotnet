using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_API.Audio
{
	/// <summary>
	/// Transcribe audio into text, with optional translation into English.
	/// </summary>
	public class TranscriptionEndpoint : EndpointBase, ITranscriptionEndpoint
	{
		/// <inheritdoc/>
		protected override string Endpoint
		{
			get
			{
				if (TranslateToEnglish)
				{
					return "audio/translations";
				}
				else
				{
					return "audio/transcriptions";
				}
			}
		}

		/// <summary>
		/// Constructor of the api endpoint.  Rather than instantiating this yourself, access it through an instance of <see cref="OpenAIAPI"/> as <see cref="OpenAIAPI.Transcriptions"/>.
		/// </summary>
		/// <param name="api">Pass in the instance of the api</param>
		/// <param name="translate">If <see langword="true"/>, the response will translate non-English audio into English.  Otherwise the returned text will be in the spoken language.</param>
		internal TranscriptionEndpoint(OpenAIAPI api, bool translate) : base(api)
		{
			TranslateToEnglish = translate;
		}

		/// <summary>
		/// This allows you to set default parameters for every request, for example to set a default language.  For every request, if you do not have a parameter set on the request but do have it set here as a default, the request will automatically pick up the default value.
		/// </summary>
		public AudioRequest DefaultRequestArgs { get; set; } = new AudioRequest();

		/// <summary>
		/// If <see langword="true"/>, the response will translate non-English audio into English.  Otherwise the returned text will be in the spoken language.
		/// </summary>
		private bool TranslateToEnglish { get; }

		/// <summary>
		/// Gets the transcription of the audio stream as a text string
		/// </summary>
		/// <param name="audioStream">The stream containing audio data, in one of these formats: flac, mp3, mp4, mpeg, mpga, m4a, ogg, wav, or webm.</param>
		/// <param name="filename">The name of the audio file in the stream.  This does not have to be real, but it must contain the correct file extension.  For example, "file.mp3" if you are supplying an mp3 audio stream.</param>
		/// <param name="language">The language of the input audio. Supplying the input language in ISO-639-1 format will improve accuracy and latency.</param>
		/// <param name="prompt">An optional text to guide the model's style or continue a previous audio segment. The prompt should match the audio language.</param>
		/// <param name="temperature">The sampling temperature, between 0 and 1. Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic. If set to 0, the model will use log probability to automatically increase the temperature until certain thresholds are hit.</param>
		/// <returns>A string of the transcribed text</returns>
		public async Task<string> GetTextAsync(Stream audioStream, string filename, string language = null, string prompt = null, double? temperature = null)
			=> await GetAsFormatAsync(audioStream, filename, AudioRequest.ResponseFormats.Text, language, prompt, temperature);

		/// <summary>
		/// Gets the transcription of the audio file as a text string
		/// </summary>
		/// <param name="audioFilePath">The local path to the audio file, in one of these formats: flac, mp3, mp4, mpeg, mpga, m4a, ogg, wav, or webm.</param>
		/// <param name="language">The language of the input audio. Supplying the input language in ISO-639-1 format will improve accuracy and latency.</param>
		/// <param name="prompt">An optional text to guide the model's style or continue a previous audio segment. The prompt should match the audio language.</param>
		/// <param name="temperature">The sampling temperature, between 0 and 1. Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic. If set to 0, the model will use log probability to automatically increase the temperature until certain thresholds are hit.</param>
		/// <returns>A string of the transcribed text</returns>
		public async Task<string> GetTextAsync(string audioFilePath, string language = null, string prompt = null, double? temperature = null)
		{
			using (var fileStream = File.OpenRead(audioFilePath))
			{
				return await GetTextAsync(fileStream, Path.GetFileName(audioFilePath), language, prompt, temperature);
			}
		}

		/// <summary>
		/// Gets the transcription of the audio stream, with full metadata
		/// </summary>
		/// <param name="audioStream">The stream containing audio data, in one of these formats: flac, mp3, mp4, mpeg, mpga, m4a, ogg, wav, or webm.</param>
		/// <param name="filename">The name of the audio file in the stream.  This does not have to be real, but it must contain the correct file extension.  For example, "file.mp3" if you are supplying an mp3 audio stream.</param>
		/// <param name="language">The language of the input audio. Supplying the input language in ISO-639-1 format will improve accuracy and latency.</param>
		/// <param name="prompt">An optional text to guide the model's style or continue a previous audio segment. The prompt should match the audio language.</param>
		/// <param name="temperature">The sampling temperature, between 0 and 1. Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic. If set to 0, the model will use log probability to automatically increase the temperature until certain thresholds are hit.</param>
		/// <returns>A string of the transcribed text</returns>
		public async Task<AudioResultVerbose> GetWithDetailsAsync(Stream audioStream, string filename, string language = null, string prompt = null, double? temperature = null)
		{
			var request = new AudioRequest()
			{
				Language = language ?? DefaultRequestArgs.Language,
				Model = DefaultRequestArgs.Model,
				Prompt = prompt ?? DefaultRequestArgs.Prompt,
				Temperature = temperature ?? DefaultRequestArgs.Temperature
			};
			request.ResponseFormat = AudioRequest.ResponseFormats.VerboseJson;
			MultipartFormDataContent content;
			using (var memoryStream = new MemoryStream())
			{
				audioStream.CopyTo(memoryStream);
				content = new MultipartFormDataContent
				{
					{ new StringContent(request.Model), "model" },
					{ new StringContent(request.ResponseFormat), "response_format" },
					{ new ByteArrayContent(memoryStream.ToArray()), "file", filename }
				};
				if (!string.IsNullOrEmpty(request.Language))
					content.Add(new StringContent(request.Language), "language");
				if (!string.IsNullOrEmpty(request.Prompt))
					content.Add(new StringContent(request.Prompt), "prompt");
				if (request.Temperature != 0)
					content.Add(new StringContent(request.Temperature.ToString()), "temperature");
			}
			return await HttpPost<AudioResultVerbose>(Url, content);
		}

		/// <summary>
		/// Gets the transcription of the audio file, with full metadata
		/// </summary>
		/// <param name="audioFilePath">The local path to the audio file, in one of these formats: flac, mp3, mp4, mpeg, mpga, m4a, ogg, wav, or webm.</param>
		/// <param name="language">The language of the input audio. Supplying the input language in ISO-639-1 format will improve accuracy and latency.</param>
		/// <param name="prompt">An optional text to guide the model's style or continue a previous audio segment. The prompt should match the audio language.</param>
		/// <param name="temperature">The sampling temperature, between 0 and 1. Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic. If set to 0, the model will use log probability to automatically increase the temperature until certain thresholds are hit.</param>
		/// <returns>A string of the transcribed text</returns>
		public async Task<AudioResultVerbose> GetWithDetailsAsync(string audioFilePath, string language = null, string prompt = null, double? temperature = null)
		{
			using (var fileStream = File.OpenRead(audioFilePath))
			{
				return await GetWithDetailsAsync(fileStream, Path.GetFileName(audioFilePath), language, prompt, temperature);
			}
		}

		/// <summary>
		/// Gets the transcription of the audio stream, in the specified format
		/// </summary>
		/// <param name="audioStream">The stream containing audio data, in one of these formats: flac, mp3, mp4, mpeg, mpga, m4a, ogg, wav, or webm.</param>
		/// <param name="filename">The name of the audio file in the stream.  This does not have to be real, but it must contain the correct file extension.  For example, "file.mp3" if you are supplying an mp3 audio stream.</param>
		/// <param name="responseFormat">The format of the response.  Suggested value are <see cref="AudioRequest.ResponseFormats.SRT"/> or <see cref="AudioRequest.ResponseFormats.VTT"/>.  For text and Json formats, try <see cref="GetTextAsync(Stream, string, string, string, double?)"/> or <see cref="GetWithDetailsAsync(Stream, string, string, string, double?)"/> instead.</param>
		/// <param name="language">The language of the input audio. Supplying the input language in ISO-639-1 format will improve accuracy and latency.</param>
		/// <param name="prompt">An optional text to guide the model's style or continue a previous audio segment. The prompt should match the audio language.</param>
		/// <param name="temperature">The sampling temperature, between 0 and 1. Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic. If set to 0, the model will use log probability to automatically increase the temperature until certain thresholds are hit.</param>
		/// <returns>A string of the transcribed text</returns>
		public async Task<string> GetAsFormatAsync(Stream audioStream, string filename, string responseFormat, string language = null, string prompt = null, double? temperature = null)
		{
			var request = new AudioRequest()
			{
				Language = language ?? DefaultRequestArgs.Language,
				Model = DefaultRequestArgs.Model,
				Prompt = prompt ?? DefaultRequestArgs.Prompt,
				Temperature = temperature ?? DefaultRequestArgs.Temperature,
				ResponseFormat = responseFormat ?? DefaultRequestArgs.ResponseFormat
			};
			MultipartFormDataContent content;
			using (var memoryStream = new MemoryStream())
			{
				audioStream.CopyTo(memoryStream);
				content = new MultipartFormDataContent
				{
					{ new StringContent(request.Model), "model" },
					{ new StringContent(request.ResponseFormat), "response_format" },
					{ new ByteArrayContent(memoryStream.ToArray()), "file", filename }
				};
				if (!string.IsNullOrEmpty(request.Language))
					content.Add(new StringContent(request.Language), "language");
				if (!string.IsNullOrEmpty(request.Prompt))
					content.Add(new StringContent(request.Prompt), "prompt");
				if (request.Temperature != 0)
					content.Add(new StringContent(request.Temperature.ToString()), "temperature");
			}
			return await HttpGetContent(Url, HttpMethod.Post, content);
		}

		/// <summary>
		/// Gets the transcription of the audio file, in the specified format
		/// </summary>
		/// <param name="audioFilePath">The local path to the audio file, in one of these formats: flac, mp3, mp4, mpeg, mpga, m4a, ogg, wav, or webm.</param>
		/// <param name="responseFormat">The format of the response.  Suggested value are <see cref="AudioRequest.ResponseFormats.SRT"/> or <see cref="AudioRequest.ResponseFormats.VTT"/>.  For text and Json formats, try <see cref="GetTextAsync(string, string, string, double?)"/> or <see cref="GetWithDetailsAsync(string, string, string, double?)"/> instead.</param>
		/// <param name="language">The language of the input audio. Supplying the input language in ISO-639-1 format will improve accuracy and latency.</param>
		/// <param name="prompt">An optional text to guide the model's style or continue a previous audio segment. The prompt should match the audio language.</param>
		/// <param name="temperature">The sampling temperature, between 0 and 1. Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic. If set to 0, the model will use log probability to automatically increase the temperature until certain thresholds are hit.</param>
		/// <returns>A string of the transcribed text</returns>
		public async Task<string> GetAsFormatAsync(string audioFilePath, string responseFormat, string language = null, string prompt = null, double? temperature = null)
		{
			using (var fileStream = File.OpenRead(audioFilePath))
			{
				return await GetAsFormatAsync(fileStream, Path.GetFileName(audioFilePath), responseFormat, language, prompt, temperature);
			}
		}
	}
}
