using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// 
    /// </summary>
    public class AudioTranslationResult: ApiResultBase
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
