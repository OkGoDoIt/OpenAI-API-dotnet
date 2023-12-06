using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenAI_API.Models
{
	/// <summary>
	/// Represents a language model
	/// </summary>
	public class Model
	{
		/// <summary>
		/// The id/name of the model
		/// </summary>
		[JsonProperty("id")]
		public string ModelID { get; set; }

		/// <summary>
		/// The owner of this model.  Generally "openai" is a generic OpenAI model, or the organization if a custom or finetuned model.
		/// </summary>
		[JsonProperty("owned_by")]
		public string OwnedBy { get; set; }

		/// <summary>
		/// The type of object. Should always be 'model'.
		/// </summary>
		[JsonProperty("object")]
		public string Object { get; set; }

		/// The time when the model was created
		[JsonIgnore]
		public DateTime? Created => CreatedUnixTime.HasValue ? (DateTime?)(DateTimeOffset.FromUnixTimeSeconds(CreatedUnixTime.Value).DateTime) : null;

		/// <summary>
		/// The time when the model was created in unix epoch format
		/// </summary>
		[JsonProperty("created")]
		public long? CreatedUnixTime { get; set; }

		/// <summary>
		/// Permissions for use of the model
		/// </summary>
		[JsonProperty("permission")]
		public List<Permissions> Permission { get; set; } = new List<Permissions>();

		/// <summary>
		/// Currently (2023-01-27) seems like this is duplicate of <see cref="ModelID"/> but including for completeness.
		/// </summary>
		[JsonProperty("root")]
		public string Root { get; set; }

		/// <summary>
		/// Currently (2023-01-27) seems unused, probably intended for nesting of models in a later release
		/// </summary>
		[JsonProperty("parent")]
		public string Parent { get; set; }

		/// <summary>
		/// Allows a model to be implicitly cast to the string of its <see cref="ModelID"/>
		/// </summary>
		/// <param name="model">The <see cref="Model"/> to cast to a string.</param>
		public static implicit operator string(Model model)
		{
			return model?.ModelID;
		}

		/// <summary>
		/// Allows a string to be implicitly cast as an <see cref="Model"/> with that <see cref="ModelID"/>
		/// </summary>
		/// <param name="name">The id/<see cref="ModelID"/> to use</param>
		public static implicit operator Model(string name)
		{
			return new Model(name);
		}

		/// <summary>
		/// Represents an Model with the given id/<see cref="ModelID"/>
		/// </summary>
		/// <param name="name">The id/<see cref="ModelID"/> to use.
		///	</param>
		public Model(string name)
		{
			this.ModelID = name;
		}

		/// <summary>
		/// Represents a generic Model/model
		/// </summary>
		public Model()
		{

		}

		/// <summary>
		/// The default model to use in requests if no other model is specified.
		/// </summary>
		public static Model DefaultModel { get; set; } = ChatGPTTurboInstruct;

		/// <summary>
		/// The default model to use in chat requests if no other model is specified.
		/// </summary>
		public static Model DefaultChatModel { get; set; } = ChatGPTTurbo;

		/// <summary>
		/// Gets more details about this Model from the API, specifically properties such as <see cref="OwnedBy"/> and permissions.
		/// </summary>
		/// <param name="api">An instance of the API with authentication in order to call the endpoint.</param>
		/// <returns>Asynchronously returns an Model with all relevant properties filled in</returns>
		public async Task<Model> RetrieveModelDetailsAsync(OpenAI_API.OpenAIAPI api)
		{
			return await api.Models.RetrieveModelDetailsAsync(this.ModelID);
		}


#region GPT-4 and GPT-4 Turbo
		/// <summary>
		/// More capable than any GPT-3.5 model, able to do more complex tasks, and optimized for chat. Will be updated with the latest model iteration.
		/// </summary>
		public static Model GPT4 => new Model("gpt-4") { OwnedBy = "openai" };

		/// <summary>
		/// Same capabilities as the base gpt-4 model but with 4x the context length. Will be updated with the latest model iteration.
		/// </summary>
		public static Model GPT4_32k_Context => new Model("gpt-4-32k") { OwnedBy = "openai" };

		/// <summary>
		/// Ability to understand images, in addition to all other GPT-4 Turbo capabilities. Returns a maximum of 4,096 output tokens. This is a preview model version and not suited yet for production traffic.
		/// </summary>
		public static Model GPT4_Vision => new Model("gpt-4-vision-preview") { OwnedBy = "openai" };

		/// <summary>
		///	The latest GPT-4 model with improved instruction following, JSON mode, reproducible outputs, parallel function calling, and more. Returns a maximum of 4,096 output tokens. This preview model is not yet suited for production traffic. 
		/// </summary>
		public static Model GPT4_Turbo => new Model("gpt-4-1106-preview") { OwnedBy = "openai" };
#endregion

#region GPT-3.5
		/// <summary>
		/// Most capable GPT-3.5 model and optimized for chat at 1/10th the cost of text-davinci-003. Will be updated with the latest model iteration.
		/// </summary>
		public static Model ChatGPTTurbo => new Model("gpt-3.5-turbo") { OwnedBy = "openai" };

		/// <summary>
		/// The latest GPT-3.5 Turbo model with 16k context window, improved instruction following, JSON mode, reproducible outputs, parallel function calling, and more. Returns a maximum of 4,096 output tokens.
		/// </summary>
		public static Model ChatGPTTurbo_16k => new Model("gpt-3.5-turbo-16k") { OwnedBy = "openai" };

		/// <summary>
		/// Similar capabilities as text-davinci-003 but compatible with legacy Completions endpoint and not Chat Completions.
		/// </summary>
		public static Model ChatGPTTurboInstruct => new Model("gpt-3.5-turbo-instruct") { OwnedBy = "openai" };
#endregion

#region GPT Base
		/// <summary>
		/// Replacement for the GPT-3 ada and babbage base models. GPT base models can understand and generate natural language or code but are not trained with instruction following. These models are made to be replacements for our original GPT-3 base models and use the legacy Completions API. Most customers should use GPT-3.5 or GPT-4.
		/// </summary>
		public static Model Babbage => new Model("babbage-002") { OwnedBy = "openai" };


		/// <summary>
		/// Replacement for the GPT-3 curie and davinci base model. GPT base models can understand and generate natural language or code but are not trained with instruction following. These models are made to be replacements for our original GPT-3 base models and use the legacy Completions API. Most customers should use GPT-3.5 or GPT-4.
		/// </summary>
		public static Model Davinci => new Model("davinci-002") { OwnedBy = "openai" };
#endregion

#region GPT-3 Legacy
		/// <summary>
		/// Capable of very simple tasks, usually the fastest model in the GPT-3 series, and lowest cost
		/// </summary>
		public static Model AdaText => new Model("text-ada-001") { OwnedBy = "openai" };

		/// <summary>
		/// Capable of straightforward tasks, very fast, and lower cost.
		/// </summary>
		public static Model BabbageText => new Model("text-babbage-001") { OwnedBy = "openai" };

		/// <summary>
		/// Very capable, but faster and lower cost than Davinci.
		/// </summary>
		public static Model CurieText => new Model("text-curie-001") { OwnedBy = "openai" };

		/// <summary>
		/// Will be deprecated by OpenAI on Jan 4th 2024. Use <see cref="Davinci"/> ("davinci-002") instead.
		/// </summary>
		[Obsolete("Will be deprecated by OpenAI on Jan 4th 2024. Use Davinci (\"davinci-002\") instead")]
		public static Model DavinciText => new Model("text-davinci-003") { OwnedBy = "openai" };

		/// <summary>
		/// Almost as capable as Davinci Codex, but slightly faster. This speed advantage may make it preferable for real-time applications.
		/// </summary>
		[Obsolete("No longer supported by OpenAI", true)]
		public static Model CushmanCode => new Model("code-cushman-001") { OwnedBy = "openai" };

		/// <summary>
		/// Will be deprecated by OpenAI on Jan 4th 2024.
		/// </summary>
		[Obsolete("Will be deprecated by OpenAI on Jan 4th 2024.")]
		public static Model DavinciCode => new Model("code-davinci-002") { OwnedBy = "openai" };
#endregion

#region Embeddings
		/// <summary>
		/// OpenAI offers one second-generation embedding model for use with the embeddings API endpoint.
		/// </summary>
		public static Model AdaTextEmbedding => new Model("text-embedding-ada-002") { OwnedBy = "openai" };
#endregion

#region Moderation
		/// <summary>
		/// Stable text moderation model that may provide lower accuracy compared to TextModerationLatest.
		/// OpenAI states they will provide advanced notice before updating this model.
		/// </summary>
		public static Model TextModerationStable => new Model("text-moderation-stable") { OwnedBy = "openai" };

		/// <summary>
		/// The latest text moderation model. This model will be automatically upgraded over time.
		/// </summary>
		public static Model TextModerationLatest => new Model("text-moderation-latest") { OwnedBy = "openai" };
#endregion

#region DALL-E
		/// <summary>
		/// The previous DALL·E model released in Nov 2022. The 2nd iteration of DALL·E with more realistic, accurate, and 4x greater resolution images than the original model.
		/// </summary>
		public static Model DALLE2 => new Model("dall-e-2") { OwnedBy = "openai" };

		/// <summary>
		/// The latest DALL·E model released in Nov 2023.
		/// </summary>
		public static Model DALLE3 => new Model("dall-e-3") { OwnedBy = "openai" };
#endregion

#region TTS
		/// <summary>
		/// The latest text to speech model, optimized for speed and real time text to speech use cases.
		/// </summary>
		public static Model TTS_Speed => new Model("tts-1") { OwnedBy = "openai" };

		/// <summary>
		/// The latest text to speech model, optimized for quality.
		/// </summary>
		public static Model TTS_HD=> new Model("tts-1-hd") { OwnedBy = "openai" };
#endregion

}

	/// <summary>
	/// Permissions for using the model
	/// </summary>
	public class Permissions
	{
		/// <summary>
		/// Permission Id (not to be confused with ModelId)
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// Object type, should always be 'model_permission'
		/// </summary>
		[JsonProperty("object")]
		public string Object { get; set; }

		/// The time when the permission was created
		[JsonIgnore]
		public DateTime Created => DateTimeOffset.FromUnixTimeSeconds(CreatedUnixTime).DateTime;

		/// <summary>
		/// Unix timestamp for creation date/time
		/// </summary>
		[JsonProperty("created")]
		public long CreatedUnixTime { get; set; }

		/// <summary>
		/// Can the model be created?
		/// </summary>
		[JsonProperty("allow_create_engine")]
		public bool AllowCreateEngine { get; set; }

		/// <summary>
		/// Does the model support temperature sampling?
		/// https://beta.openai.com/docs/api-reference/completions/create#completions/create-temperature
		/// </summary>
		[JsonProperty("allow_sampling")]
		public bool AllowSampling { get; set; }

		/// <summary>
		/// Does the model support logprobs?
		/// https://beta.openai.com/docs/api-reference/completions/create#completions/create-logprobs
		/// </summary>
		[JsonProperty("allow_logprobs")]
		public bool AllowLogProbs { get; set; }

		/// <summary>
		/// Does the model support search indices?
		/// </summary>
		[JsonProperty("allow_search_indices")]
		public bool AllowSearchIndices { get; set; }

		[JsonProperty("allow_view")]
		public bool AllowView { get; set; }

		/// <summary>
		/// Does the model allow fine tuning?
		/// https://beta.openai.com/docs/api-reference/fine-tunes
		/// </summary>
		[JsonProperty("allow_fine_tuning")]
		public bool AllowFineTuning { get; set; }

		/// <summary>
		/// Is the model only allowed for a particular organization? May not be implemented yet.
		/// </summary>
		[JsonProperty("organization")]
		public string Organization { get; set; }

		/// <summary>
		/// Is the model part of a group? Seems not implemented yet. Always null.
		/// </summary>
		[JsonProperty("group")]
		public string Group { get; set; }

		[JsonProperty("is_blocking")]
		public bool IsBlocking { get; set; }
	}

}
