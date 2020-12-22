using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_API
{
	/// <summary>
	/// Represents a language model, aka Engine
	/// </summary>
	public class Engine
	{
		/// <summary>
		/// The id/name of the engine
		/// </summary>
		[JsonProperty("id")]
		public string EngineName { get; set; }

		/// <summary>
		/// Allows an engine to be implicitly cast to the string of its <see cref="EngineName"/>
		/// </summary>
		/// <param name="engine">The <see cref="Engine"/> to cast to a string.</param>
		public static implicit operator string(Engine engine)
		{
			return engine.EngineName;
		}

		/// <summary>
		/// Allows a string to be implicitly cast as an <see cref="Engine"/> with that <see cref="EngineName"/>
		/// </summary>
		/// <param name="name">The id/<see cref="EngineName"/> to use</param>
		public static implicit operator Engine(string name)
		{
			return new Engine(name);
		}

		/// <summary>
		/// Represents an Engine with the given id/<see cref="EngineName"/>
		/// </summary>
		/// <param name="name">The id/<see cref="EngineName"/> to use.
		///						If the <paramref name="name"/> contains a colon (as is the case in the API's <see cref="CompletionResult.Model"/> response),
		///						the part before the colon is treated as the id/<see cref="EngineName"/> and the following portion is considered the <see cref="ModelRevision"/>
		///	</param>
		public Engine(string name)
		{
			if (name.Contains(":"))
			{
				this.EngineName = name.Split(':')[0];
				this.ModelRevision = name.Split(':')[1];
			}
			else
				this.EngineName = name;
		}

		/// <summary>
		/// Represents a generic Engine/model
		/// </summary>
		public Engine()
		{

		}

		/// <summary>
		/// The owner of the model as returned from the API
		/// </summary>
		[JsonProperty("owner")]
		public string Owner { get; set; }

		/// <summary>
		/// Whether the model reports itself as being ready or not.  The meaning is underspecified in the API currently.
		/// </summary>
		[JsonProperty("ready")]
		public bool? Ready { get; set; }

		/// <summary>
		/// The revision of the model as indicated in responses from the API which specify a <see cref="CompletionResult.Model"/>.
		/// </summary>
		[JsonIgnore]
		public string ModelRevision { get; set; }

		/// <summary>
		/// The smallest, fastest engine available, although the quality of results may be poor.
		/// </summary>
		public static Engine Ada => new Engine("ada") { Owner = "openai", Ready = true };

		/// <summary>
		/// The 2nd fastest engine, a bit more powerful than <see cref="Ada"/>, and a bit slower.
		/// </summary>
		public static Engine Babbage => new Engine("babbage") { Owner = "openai", Ready = true };

		/// <summary>
		/// The 2nd most powerful engine, a bit faster than <see cref=Davinci"/>, and a bit faster.
		/// </summary>
		public static Engine Curie => new Engine("curie") { Owner = "openai", Ready = true };
		/// <summary>
		/// The most powerful, largest engine available, although the speed is quite slow.
		/// </summary>
		public static Engine Davinci => new Engine("davinci") { Owner = "openai", Ready = true };

		/// <summary>
		/// The default Engine to use in the case no other is specified.  Defaults to <see cref="Davinci"/>
		/// </summary>
		public static Engine Default => Davinci;

		/// <summary>
		/// Gets more details about this Engine from the API, specifically properties such as <see cref="Owner"/> and <see cref="Ready"/>
		/// </summary>
		/// <param name="auth">API authentication in order to call the API endpoint.  If not specified, attempts to use a default.</param>
		/// <returns>Asynchronously returns an Engine with all relevant properties filled in</returns>
		public async Task<Engine> RetrieveEngineDetailsAsync(APIAuthentication auth = null)
		{
			return await EnginesEndpoint.RetrieveEngineDetailsAsync(this.EngineName, auth);
		}


		
	}
}
