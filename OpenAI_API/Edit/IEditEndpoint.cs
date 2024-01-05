using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_API.Edits
{
    /// <summary>
	/// An interface for <see cref="EditEndpoint"/>, for ease of mock testing, etc
	/// </summary>
    public interface IEditEndpoint
    {
        /// <summary>
		/// This allows you to set default parameters for every request, for example to set a default temperature or max tokens.  For every request, if you do not have a parameter set on the request but do have it set here as a default, the request will automatically pick up the default value.
		/// </summary>
		EditRequest DefaultEditRequestArgs { get; set; }

        /// <summary>
        /// Ask the API to edit the prompt using the specified request.  This is non-streaming, so it will wait until the API returns the full result.
        /// </summary>
        /// <param name="request">The request to send to the API.  This does not fall back to default values specified in <see cref="DefaultEditRequestArgs"/>.</param>
        /// <returns>Asynchronously returns the edits result.  Look in its <see cref="EditResult.Choices"/> property for the edits.</returns>
        Task<EditResult> CreateEditsAsync(EditRequest request);

        /// <summary>
        /// Ask the API to edit the prompt using the specified request and a requested number of outputs.  This is non-streaming, so it will wait until the API returns the full result.
        /// </summary>
        /// <param name="request">The request to send to the API.  This does not fall back to default values specified in <see cref="DefaultEditRequestArgs"/>.</param>
        /// <param name="numOutputs">Overrides <see cref="EditRequest.NumChoicesPerPrompt"/> as a convenience.</param>
        /// <returns>Asynchronously returns the edits result.  Look in its <see cref="EditResult.Choices"/> property for the edits, which should have a length equal to <paramref name="numOutputs"/>.</returns>
        Task<EditResult> CreateEditsAsync(EditRequest request, int numOutputs = 5);


        /// <summary>
        /// Ask the API to edit the prompt. This is non-streaming, so it will wait until the API returns the full result.  Any non-specified parameters will fall back to default values specified in <see cref="DefaultEditRequestArgs"/> if present.
        /// </summary>
        /// <param name="prompt">The input text to use as a starting point for the edit. Defaults to an empty string.</param>
        /// <param name="instruction">The instruction that tells the model how to edit the prompt. (Required)</param>
        /// <param name="model">ID of the model to use. You can use <see cref="Model.TextDavinciEdit"/> or <see cref="Model.CodeDavinciEdit"/> for edit endpoint.</param>
        /// <param name="temperature">What sampling temperature to use. Higher values means the model will take more risks. Try 0.9 for more creative applications, and 0 (argmax sampling) for ones with a well-defined answer. It is generally recommend to use this or <see cref="TopP"/> but not both.</param>
        /// <param name="top_p">An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are considered. It is generally recommend to use this or <see cref="Temperature"/> but not both.</param>
        /// <param name="numOutputs">How many edits to generate for the input and instruction.</param>
        /// <returns></returns>
        Task<EditResult> CreateEditsAsync(string prompt,
            string instruction,
            Model model = null,
            double? temperature = null,
            double? top_p = null,
            int? numOutputs = null
            );

       

        /// <summary>
        /// Simply returns edited prompt string
        /// </summary>
        /// <param name="request">The request to send to the API.  This does not fall back to default values specified in <see cref="DefaultEditRequestArgs"/>.</param>
        /// <returns>The best edited result </returns>
        Task<string> CreateAndFormatEdits(EditRequest request);

        /// <summary>
        /// Simply returns the best edit
        /// </summary>
        /// <param name="prompt">The input prompt to be edited</param>
        /// <param name="instruction">The instruction that tells model how to edit the prompt</param>
        /// <returns>The best edited result</returns>
        Task<string> GetEdits(string prompt, string instruction);

    }
}
