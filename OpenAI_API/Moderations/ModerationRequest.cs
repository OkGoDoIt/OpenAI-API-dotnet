using Newtonsoft.Json;
using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Moderations
{
    /// <summary>
    /// Represents a request to the Moderations API.
    /// </summary>
    public class ModerationRequest
    {
		/// <summary>
		/// Which Moderation model to use for this request
		/// </summary>
		[JsonProperty("model")]
		public string Model { get; set; }

		/// <summary>
		/// Main text to classify
		/// </summary>
		[JsonProperty("input")]
		public string Input { get; set; }

		/// <summary>
		/// Cretes a new, empty <see cref="ModerationRequest"/>
		/// </summary>
		public ModerationRequest()
		{

		}

		/// <summary>
		/// Creates a new <see cref="ModerationRequest"/> with the specified parameters
		/// </summary>
		/// <param name="model">The model to use. You can use <see cref="ModelsEndpoint.GetModelsAsync()"/> to see all of your available models, or use a standard model like <see cref="Model.TextModerationLatest"/>.</param>
		/// <param name="input">The prompt to classify</param>
		public ModerationRequest(Model model, string input)
		{
			Model = model;
			this.Input = input;
		}

		/// <summary>
		/// Creates a new <see cref="ModerationRequest"/> with the specified input and the <see cref="Model.TextModerationLatest"/> model.
		/// </summary>
		/// <param name="input">The prompt to classify</param>
		public ModerationRequest(string input)
		{
			Model = OpenAI_API.Models.Model.AdaTextEmbedding;
			this.Input = input;
		}
	}
}
