using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenAI_API.Edits
{
    /// <summary>
    /// This API lets you edit the prompt. Given a prompt and instruction, this will return an edited version of the prompt. This API lets you edit the prompt. Given a prompt and instruction, this will return an edited version of the prompt. <see href="https://platform.openai.com/docs/api-reference/edits"/>
    /// </summary>
    public class EditEndpoint : EndpointBase, IEditEndpoint
    {
        /// <summary>
		/// This allows you to set default parameters for every request, for example to set a default temperature or max tokens.  For every request, if you do not have a parameter set on the request but do have it set here as a default, the request will automatically pick up the default value.
		/// </summary>
		public EditRequest DefaultEditRequestArgs { get; set; } = new EditRequest() { Model = Model.TextDavinciEdit };

        /// <summary>
        /// The name of the endpoint, which is the final path segment in the API URL.  For example, "edits".
        /// </summary>
        protected override string Endpoint { get { return "edits"; } }

        /// <summary>
        /// Constructor of the api endpoint.  Rather than instantiating this yourself, access it through an instance of <see cref="OpenAIAPI"/> as <see cref="OpenAIAPI.Edit"/>.
        /// </summary>
        /// <param name="api"></param>
        internal EditEndpoint(OpenAIAPI api) : base(api) { }

        /// <summary>
        /// Ask the API to edit the prompt using the specified request.  This is non-streaming, so it will wait until the API returns the full result.
        /// </summary>
        /// <param name="request">The request to send to the API.  This does not fall back to default values specified in <see cref="DefaultEditRequestArgs"/>.</param>
        /// <returns>Asynchronously returns the edits result.  Look in its <see cref="EditResult.Choices"/> property for the edits.</returns>
        public async Task<EditResult> CreateEditsAsync(EditRequest request)
        {
            if(request.Model != Model.TextDavinciEdit.ModelID && request.Model != Model.CodeDavinciEdit.ModelID)
                throw new ArgumentException($"Model must be either '{Model.TextDavinciEdit.ModelID}' or '{Model.CodeDavinciEdit.ModelID}'. For more details, refer https://platform.openai.com/docs/api-reference/edits");
            return await HttpPost<EditResult>(postData: request);
        }

        /// <summary>
        /// Ask the API to edit the prompt using the specified request and a requested number of outputs.  This is non-streaming, so it will wait until the API returns the full result.
        /// </summary>
        /// <param name="request">The request to send to the API.  This does not fall back to default values specified in <see cref="DefaultEditRequestArgs"/>.</param>
        /// <param name="numOutputs">Overrides <see cref="EditRequest.NumChoicesPerPrompt"/> as a convenience.</param>
        /// <returns>Asynchronously returns the edits result.  Look in its <see cref="EditResult.Choices"/> property for the edits, which should have a length equal to <paramref name="numOutputs"/>.</returns>
        public Task<EditResult> CreateEditsAsync(EditRequest request, int numOutputs = 5)
        {
            request.NumChoicesPerPrompt = numOutputs;
            return CreateEditsAsync(request);
        }

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
        public Task<EditResult> CreateEditsAsync(string prompt,
            string instruction,
            Model model = null,
            double? temperature = null,
            double? top_p = null,
            int? numOutputs = null
            )
        {
            EditRequest request = new EditRequest(DefaultEditRequestArgs)
            {
                Input = prompt,
                Model = model ?? DefaultEditRequestArgs.Model,
                Instruction = string.IsNullOrEmpty(instruction) ? DefaultEditRequestArgs.Instruction : instruction,
                Temperature = temperature ?? DefaultEditRequestArgs.Temperature,
                TopP = top_p ?? DefaultEditRequestArgs.TopP,
                NumChoicesPerPrompt = numOutputs ?? DefaultEditRequestArgs.NumChoicesPerPrompt,
            };
            return CreateEditsAsync(request);
        }


        /// <summary>
        /// Simply returns edited prompt string
        /// </summary>
        /// <param name="request">The request to send to the API.  This does not fall back to default values specified in <see cref="DefaultEditRequestArgs"/>.</param>
        /// <returns>The best edited result </returns>
        public async Task<string> CreateAndFormatEdits(EditRequest request)
        {
            string prompt = request.Input;
            var result = await CreateEditsAsync(request);
            return result.ToString();
        }

        /// <summary>
        /// Simply returns the best edit
        /// </summary>
        /// <param name="prompt">The input prompt to be edited</param>
        /// <param name="instruction">The instruction that tells model how to edit the prompt</param>
        /// <returns>The best edited result</returns>
        public async Task<string> GetEdits(string prompt, string instruction)
        {
            EditRequest request = new EditRequest(DefaultEditRequestArgs)
            {
                Input = prompt,
                Instruction = instruction,
                NumChoicesPerPrompt = 1
            };
            var result = await CreateEditsAsync(request);
            return result.ToString();
        }

    }
}
