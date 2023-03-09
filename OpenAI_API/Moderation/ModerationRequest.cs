using Newtonsoft.Json;
using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OpenAI_API.Moderation
{
	/// <summary>
	/// Represents a request to the Moderations API.
	/// </summary>
	public class ModerationRequest
	{

		/// <summary>
		/// Which Moderation model to use for this request.  Two content moderations models are available: <see cref="Model.TextModerationStable"/> and <see cref="Model.TextModerationLatest"/>.  The default is <see cref="Model.TextModerationLatest"/> which will be automatically upgraded over time.This ensures you are always using our most accurate model.If you use <see cref="Model.TextModerationStable"/>, we will provide advanced notice before updating the model. Accuracy of <see cref="Model.TextModerationStable"/> may be slightly lower than for <see cref="Model.TextModerationLatest"/>.
		/// </summary>
		[JsonProperty("model")]
		public string Model { get; set; }

		/// <summary>
		/// The input text to classify
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
		/// <param name="input">The prompt to classify</param>
		/// <param name="model">The model to use. You can use <see cref="ModelsEndpoint.GetModelsAsync()"/> to see all of your available models, or use a standard model like <see cref="Model.TextModerationLatest"/>.</param>
		public ModerationRequest(string input, Model model)
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
			Model = OpenAI_API.Models.Model.TextModerationLatest;
			this.Input = input;
		}
	}
}
