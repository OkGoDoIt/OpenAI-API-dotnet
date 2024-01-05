using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace OpenAI_API.Audio
{
	/// <summary>
	/// A request to the <see cref="TextToSpeechEndpoint"/>.
	/// </summary>
	public class TextToSpeechRequest
	{
		/// <summary>
		/// The model to use for this request
		/// </summary>
		[JsonProperty("model")]
		public string Model { get; set; } = OpenAI_API.Models.Model.DefaultTTSModel;

		/// <summary>
		/// The text to generate audio for. The maximum length is 4096 characters.
		/// </summary>
		[JsonProperty("input")]
		public string Input { get; set; }

		/// <summary>
		/// The voice to use when generating the audio. Supported voices can be found in <see cref="Voices"/>.
		/// </summary>
		[JsonProperty("voice")]
		public string Voice { get; set; } = Voices.Alloy;

		/// <summary>
		/// The default response format is "mp3", but other formats are available in <see cref="TextToSpeechRequest.ResponseFormats"/>.  See <seealso href="https://platform.openai.com/docs/guides/text-to-speech/supported-output-formats"/>
		/// </summary>
		[JsonProperty("response_format", DefaultValueHandling=DefaultValueHandling.Ignore)]
		public string ResponseFormat { get; set; } = null;

		/// <summary>
		/// The speed of the generated audio. Select a value from 0.25 to 4.0. 1.0 is the default.
		/// </summary>
		[JsonProperty("speed", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public double? Speed { get; set; } = null;

		/// <summary>
		/// Supported voices are alloy, echo, fable, onyx, nova, and shimmer. Previews of the voices are available in the Text to speech guide. See <seealso href="https://platform.openai.com/docs/guides/text-to-speech/voice-options"/>.
		/// </summary>
		public static class Voices
		{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
			public const string Alloy = "alloy";
			public const string Echo = "echo";
			public const string Fable = "fable";
			public const string Onyx = "onyx";
			public const string Nova = "nova";
			public const string Shimmer = "shimmer";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
		}

		/// <summary>
		/// The format to return for the generated audio.  See <seealso href="https://platform.openai.com/docs/guides/text-to-speech/supported-output-formats"/>
		/// </summary>
		public static class ResponseFormats
		{
			/// <summary>
			/// The default, industry-standard audio format
			/// </summary>
			public const string MP3 = "mp3";
			/// <summary>
			/// For lossless audio compression, favored by audio enthusiasts for archiving
			/// </summary>
			public const string FLAC = "flac";
			/// <summary>
			/// For digital audio compression, preferred by YouTube, Android, iOS
			/// </summary>
			public const string AAC = "aac";
			/// <summary>
			/// For internet streaming and communication, low latency.
			/// </summary>
			public const string OPUS = "opus";
		}
	}
}
