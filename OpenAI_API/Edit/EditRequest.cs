using Newtonsoft.Json;
using OpenAI_API.Completions;
using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenAI_API.Edits
{
    /// <summary>
    /// Represents a request to the Edit API.  Mostly matches the parameters in <see href="https://platform.openai.com/docs/api-reference/edits">the OpenAI docs</see>, although some might have been renamed for ease of use.
    /// </summary>
    public class EditRequest
    {
        /// <summary>
		/// ID of the model to use. You can use <see cref="Model.TextDavinciEdit"/> or <see cref="Model.CodeDavinciEdit"/> for edit endpoint.
		/// </summary>
        [JsonProperty("model")]
        public string Model { get; set; } = OpenAI_API.Models.Model.TextDavinciEdit;

        /// <summary>
        /// The input text to use as a starting point for the edit. Defaults to an empty string.
        /// </summary>
        [JsonProperty("input")]
        public string Input { get; set; }

        /// <summary>
        /// The instruction that tells the model how to edit the prompt. (Required)
        /// </summary>
        [JsonProperty("instruction")]
        public string Instruction { get; set; }

        /// <summary>
        /// How many edits to generate for the input and instruction.
        /// </summary>
        [JsonProperty("n")]
        public int? NumChoicesPerPrompt { get; set; }

        /// <summary>
		/// What sampling temperature to use. Higher values means the model will take more risks. Try 0.9 for more creative applications, and 0 (argmax sampling) for ones with a well-defined answer. It is generally recommend to use this or <see cref="TopP"/> but not both.
		/// </summary>
		[JsonProperty("temperature")]
        public double? Temperature { get; set; }

        /// <summary>
        /// An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are considered. It is generally recommend to use this or <see cref="Temperature"/> but not both.
        /// </summary>
        [JsonProperty("top_p")]
        public double? TopP { get; set; }

        
        /// <summary>
		/// Cretes a new, empty <see cref="CompletionRequest"/>
		/// </summary>
		public EditRequest()
        {
            this.Model = OpenAI_API.Models.Model.TextDavinciEdit;
        }

        /// <summary>
        /// Creates a new <see cref="EditRequest"/>, inheriting any parameters set in <paramref name="basedOn"/>.
        /// </summary>
        /// <param name="basedOn">The <see cref="CompletionRequest"/> to copy</param>
        public EditRequest(EditRequest basedOn)
        {
            this.Model = basedOn.Model;
            this.Input = basedOn.Input;
            this.Instruction = basedOn.Instruction;
            this.Temperature = basedOn.Temperature;
            this.TopP = basedOn.TopP;
            this.NumChoicesPerPrompt = basedOn.NumChoicesPerPrompt;
        }


        /// <summary>
        /// Creates a new <see cref="CompletionRequest"/> with the specified parameters
        /// </summary>
        /// <param name="input">The input text to use as a starting point for the edit. Defaults to an empty string.</param>
        /// <param name="instruction">The instruction that tells the model how to edit the prompt. (Required)</param>
        /// <param name="model">ID of the model to use. You can use <see cref="Model.TextDavinciEdit"/> or <see cref="Model.CodeDavinciEdit"/> for edit endpoint.</param>
        /// <param name="temperature">What sampling temperature to use. Higher values means the model will take more risks. Try 0.9 for more creative applications, and 0 (argmax sampling) for ones with a well-defined answer. It is generally recommend to use this or <see cref="TopP"/> but not both.</param>
        /// <param name="top_p">An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are considered. It is generally recommend to use this or <see cref="Temperature"/> but not both.</param>
        /// <param name="numOutputs">How many edits to generate for the input and instruction.</param>
        public EditRequest(
            string input,
            string instruction,
            Model model = null,
            double? temperature = null,
            double? top_p = null,
            int? numOutputs = null)
        {
            this.Model = model;
            this.Input = input;
            this.Instruction = instruction;
            this.Temperature = temperature;
            this.TopP = top_p;
            this.NumChoicesPerPrompt = numOutputs;
           
        }
    }
}
