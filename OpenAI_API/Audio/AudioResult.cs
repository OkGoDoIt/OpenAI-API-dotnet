using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Audio
{
	/// <summary>
	/// Represents a verbose_json output from the OpenAI Transcribe or Translate endpoints.
	/// </summary>
	public class AudioResultVerbose : ApiResultBase
	{
		public double duration { get; set; }
		public string language { get; set; }
		public List<Segment> segments { get; set; }
		public string task { get; set; }
		public string text { get; set; }

		public class Segment
		{
			public double avg_logprob { get; set; }
			public double compression_ratio { get; set; }
			public double end { get; set; }
			public int id { get; set; }
			public double no_speech_prob { get; set; }
			public int seek { get; set; }
			public double start { get; set; }
			public double temperature { get; set; }
			public string text { get; set; }
			public List<int> tokens { get; set; }
		}
	}
}
