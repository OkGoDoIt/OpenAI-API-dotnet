using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// The object used to pass in the request to the audio translation endpoint
    /// </summary>
    public class AudioTranslationRequest
    {
        /// <summary>
        /// File Path to an audio file to translate
        /// </summary>
        public string filePath { get; set; }

        /// <summary>
        /// Byte array of an audio file to translate
        /// </summary>
        public byte[] fileData { get; set; }

        /// <summary>
        /// The format of the audio file
        /// </summary>
        public AudioTransFileFormat fileFormat { get; set; }

        /// <summary>
        /// The model to use for translation, currently only Whisper-1 is supported
        /// </summary>
        public AudioTransModel model { get; set; }

        /// <summary>
        /// Prompt (in English) to help the translation
        /// </summary>
        public string prompt { get; set; }

        /// <summary>
        /// The format of the response
        /// </summary>
        public AudioTransResponseFormat response_format { get; set; }

        /// <summary>
        /// The temperature to use for the translation
        /// </summary>
        public float? temperature { get; set; }

        /// <summary>
        /// Provides a multipart form data content object for the request
        /// </summary>
        /// <returns></returns>
        public MultipartFormDataContent GetMultipartFormDataContent()
        {   
            var content = new MultipartFormDataContent
            {
                {  new ByteArrayContent(fileData), "file", $"audio.{fileFormat}" }
            };
            if (content != null)
            {
                if (model == null)
                {
                    model = OpenAI_API.Audio.AudioTransModel.Whisper1;
                }
                content.Add(new StringContent(model), "model");

                if (!string.IsNullOrEmpty(prompt))
                {
                    content.Add(new StringContent(prompt), "prompt");
                }
                if (response_format != null)
                {
                    content.Add(new StringContent(response_format), "response_format");
                }
                if (temperature != null)
                {
                    content.Add(new StringContent(((float)temperature).ToString()), "temperature");
                }
            }
            return content;
        }

    }
}
