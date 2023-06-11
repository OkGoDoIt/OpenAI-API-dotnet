using Newtonsoft.Json;
using OpenAI_API.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Edits
{
    /// <summary>
    /// Represents a result from calling the Edit API
    /// </summary>
    public class EditResult : ApiResultBase
    {
        /// <summary>
        /// The list of choices that the user was presented with during the edit interation
        /// </summary>
        [JsonProperty("choices")]
        public IReadOnlyList<EditChoice> Choices { get; set; }

        /// <summary>
        /// The usage statistics for the edit call
        /// </summary>
        [JsonProperty("usage")]
        public EditUsage Usage { get; set; }

        /// <summary>
        /// A convenience method to return the content of the message in the first choice of this response
        /// </summary>
        /// <returns>The edited text returned by the API as reponse.</returns>
        public override string ToString()
        {
            if (Choices != null && Choices.Count > 0)
                return Choices[0].ToString();
            else
                return null;
        }
    }

    /// <summary>
    /// A message received from the API, including the text and index.
    /// </summary>
    public class EditChoice
    {
        /// <summary>
        /// The index of the choice in the list of choices
        /// </summary>
        [JsonProperty("index")]
        public int Index { get; set; }

        /// <summary>
        /// The edited text that was presented to the user as the choice. This is returned as response from API
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// A convenience method to return the content of the message in this response
        /// </summary>
        /// <returns>The edited text returned by the API as reponse.</returns>
        public override string ToString()
        {
            return Text;
        }
    }

    /// <summary>
    /// How many tokens were used in this edit message.
    /// </summary>
    public class EditUsage : Usage
    {
        /// <summary>
        /// The number of completion tokens used during the edit
        /// </summary>
        [JsonProperty("completion_tokens")]
        public int CompletionTokens { get; set; }
    }
}
