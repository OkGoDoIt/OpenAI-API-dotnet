using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_API
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
        /// Allows an model to be implicitly cast to the string of its <see cref="ModelID"/>
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
        /// Most capable GPT-3 model. Can do any task the other models can do, often with higher quality, longer output and better instruction-following. Also supports inserting completions within text.
        /// </summary>
        public static Model DavinciText => new Model("text-davinci-003") { OwnedBy = "openai" };

        /// <summary>
        /// Almost as capable as Davinci Codex, but slightly faster. This speed advantage may make it preferable for real-time applications.
        /// </summary>
        public static Model CushmanCode => new Model("code-cushman-001") { OwnedBy = "openai" };

        /// <summary>
        /// Most capable Codex model. Particularly good at translating natural language to code. In addition to completing code, also supports inserting completions within code.
        /// </summary>
        public static Model DavinciCode => new Model("code-davinci-002") { OwnedBy = "openai" };

        /// <summary>
        /// OpenAI offers one second-generation embedding model for use with the embeddings API endpoint.
        /// </summary>
        public static Model AdaTextEmbedding => new Model("text-embedding-ada-002") { OwnedBy = "openai" };



        /// <summary>
        /// Gets more details about this Model from the API, specifically properties such as <see cref="OwnedBy"/> and permissions.
        /// </summary>
        /// <param name="auth">API authentication in order to call the API endpoint.  If not specified, attempts to use a default.</param>
        /// <returns>Asynchronously returns an Model with all relevant properties filled in</returns>
        public async Task<Model> RetrieveModelDetailsAsync(APIAuthentication auth = null)
		{
			return await ModelsEndpoint.RetrieveModelDetailsAsync(this.ModelID, auth);
		}		
	}
}
