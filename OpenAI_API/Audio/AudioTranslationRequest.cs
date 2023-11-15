using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Audio
{
    public class AudioTranslationRequest
    {
        public string filePath { get; set; }
        public byte[] fileData { get; set; }
        public AudioTransFileFormat fileFormat { get; set; }
        public AudioTransModel model { get; set; }
        public string prompt { get; set; }
        public AudioTransResponseFormat response_format { get; set; }
        public float? temperature { get; set; }
    }
}
