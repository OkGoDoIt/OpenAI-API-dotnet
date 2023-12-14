using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using static OpenAI_API.Audio.TextToSpeechRequest;

namespace OpenAI_API.Audio
{
	/// <summary>
	/// Parameters for requests made by the <see cref="TranscriptionEndpoint"/>.
	/// </summary>
	public class AudioRequest
	{
		/// <summary>
		/// The model to use for this request.  Currently only <see cref="OpenAI_API.Models.Model.Whisper1"/> is supported.
		/// </summary>
		[JsonProperty("model")]
		public string Model { get; set; } = OpenAI_API.Models.Model.DefaultTranscriptionModel;

		/// <summary>
		/// An optional text to guide the model's style or continue a previous audio segment. The prompt should match the audio language for transcriptions, or English for translations.
		/// </summary>
		[JsonProperty("prompt", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string Prompt { get; set; } = null;

		/// <summary>
		/// The language of the input audio. Supplying the input language in ISO-639-1 format will improve accuracy and latency.
		/// </summary>
		[JsonProperty("language", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string Language { get; set; } = null;

		/// <summary>
		/// The format of the transcript output, should be one of the options in <see cref="AudioRequest.ResponseFormats"/>.  See <seealso href="https://platform.openai.com/docs/api-reference/audio/createTranscription#audio-createtranscription-response_format"/>
		/// </summary>
		[JsonProperty("response_format", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string ResponseFormat { get; set; } = null;

		/// <summary>
		/// The sampling temperature, between 0 and 1. Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic. If set to 0, the model will use log probability to automatically increase the temperature until certain thresholds are hit.
		/// </summary>
		[JsonProperty("temperature", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public double Temperature { get; set; } = 0;


		/// <summary>
		/// The format of the transcript output.  See <seealso href="https://platform.openai.com/docs/api-reference/audio/createTranscription#audio-createtranscription-response_format"/>
		/// </summary>
		public static class ResponseFormats
		{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
			public const string JSON = "json";
			public const string Text = "text";
			public const string SRT = "srt";
			public const string VerboseJson = "verbose_json";
			public const string VTT = "vtt";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
		}
	}
}
