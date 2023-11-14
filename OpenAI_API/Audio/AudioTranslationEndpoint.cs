using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// Translates audio into English.
    /// </summary>
    public class AudioTranslationEndpoint : EndpointBase
    {
        /// <summary>
        /// The name of the endpoint, which is the final path segment in the API URL.  For example, "audio".
        /// </summary>
        protected override string Endpoint { get { return "audio/translation"; } }

        /// <summary>
        /// Constructor of the api endpoint.  Rather than instantiating this yourself, access it through an instance of <see cref="OpenAIAPI"/> as <see cref="OpenAIAPI.ImageGenerations"/>.
        /// </summary>
        /// <param name="api"></param>
        internal AudioTranslationEndpoint(OpenAIAPI api) : base(api) { }
    }
}
