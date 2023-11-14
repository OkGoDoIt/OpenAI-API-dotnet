using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// Transcribes audio into the input language.
    /// </summary>
    public class AudioTranscriptionEndpoint : EndpointBase
    {
        /// <summary>
        /// The name of the endpoint, which is the final path segment in the API URL.  For example, "audio".
        /// </summary>
        protected override string Endpoint { get { return "audio/transcription"; } }

        /// <summary>
        /// Constructor of the api endpoint.  Rather than instantiating this yourself, access it through an instance of <see cref="OpenAIAPI"/> as <see cref="OpenAIAPI.ImageGenerations"/>.
        /// </summary>
        /// <param name="api"></param>
        internal AudioTranscriptionEndpoint(OpenAIAPI api) : base(api) { }
    }
}
