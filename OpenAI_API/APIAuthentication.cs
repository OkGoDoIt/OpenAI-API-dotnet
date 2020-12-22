using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace OpenAI_API
{
	/// <summary>
	/// Represents authentication to the OpenAPI API endpoint
	/// </summary>
	public class APIAuthentication
	{
		/// <summary>
		/// The API key, required to access the API endpoint.
		/// </summary>
		public string ApiKey { get; set; }

		/// <summary>
		/// Allows implicit casting from a string, so that a simple string API key can be provided in place of an instance of <see cref="APIAuthentication"/>
		/// </summary>
		/// <param name="key">The API key to convert into a <see cref="APIAuthentication"/>.</param>
		public static implicit operator APIAuthentication(string key)
		{
			return new APIAuthentication(key);
		}

		/// <summary>
		/// Instantiates a new Authentication object with the given <paramref name="apiKey"/>, which may be <see langword="null"/>.
		/// </summary>
		/// <param name="apiKey">The API key, required to access the API endpoint.</param>
		public APIAuthentication(string apiKey)
		{
			this.ApiKey = apiKey;
		}

		private static APIAuthentication cachedDefault = null;

		/// <summary>
		/// The default authentication to use when no other auth is specified.  This can be set manually, or automatically loaded via environment variables or a config file.  <seealso cref="LoadFromEnv"/><seealso cref="LoadFromPath(string, string, bool)"/>
		/// </summary>
		public static APIAuthentication Default
		{
			get
			{
				if (cachedDefault != null)
					return cachedDefault;

				APIAuthentication auth = LoadFromEnv();
				if (auth == null)
					auth = LoadFromPath();
				if (auth == null)
					auth = LoadFromPath(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

				cachedDefault = auth;
				return auth;
			}
			set
			{
				cachedDefault = value;
			}
		}

		/// <summary>
		/// Attempts to load api keys from environment variables, as "OPENAI_KEY" (or "OPENAI_SECRET_KEY", for backwards compatibility)
		/// </summary>
		/// <returns>Returns the loaded <see cref="APIAuthentication"/> any api keys were found, or <see langword="null"/> if there were no matching environment vars.</returns>
		public static APIAuthentication LoadFromEnv()
		{
			var key = Environment.GetEnvironmentVariable("OPENAI_KEY");
			
			if (string.IsNullOrEmpty(key))
				key = Environment.GetEnvironmentVariable("OPENAI_SECRET_KEY");
			
			if (string.IsNullOrEmpty(key))
				return null;

			return new APIAuthentication(key);
		}

		/// <summary>
		/// Attempts to load api keys from a configuration file, by default ".openai" in the current directory, optionally traversing up the directory tree
		/// </summary>
		/// <param name="directory">The directory to look in, or <see langword="null"/> for the current directory</param>
		/// <param name="filename">The filename of the config file</param>
		/// <param name="searchUp">Whether to recursively traverse up the directory tree if the <paramref name="filename"/> is not found in the <paramref name="directory"/></param>
		/// <returns>Returns the loaded <see cref="APIAuthentication"/> any api keys were found, or <see langword="null"/> if it was not successful in finding a config (or if the config file didn't contain correctly formatted API keys)</returns>
		public static APIAuthentication LoadFromPath(string directory = null, string filename = ".openai", bool searchUp = true)
		{
			if (directory == null)
				directory = Environment.CurrentDirectory;

			string key = null;
			var curDirectory = new DirectoryInfo(directory);

			while (key == null && curDirectory.Parent != null)
			{
				if (File.Exists(Path.Combine(curDirectory.FullName, filename)))
				{
					var lines = File.ReadAllLines(Path.Combine(curDirectory.FullName, filename));
					foreach (var l in lines)
					{
						var parts = l.Split('=', ':');
						if (parts.Length == 2)
						{
							switch (parts[0].ToUpper())
							{
								case "OPENAI_KEY":
									key = parts[1].Trim();
									break;
								case "OPENAI_SECRET_KEY":
									key = parts[1].Trim();
									break;
								default:
									break;
							}
						}
					}
				}

				if (searchUp)
				{
					curDirectory = curDirectory.Parent;
				}
				else
				{
					break;
				}
			}

			if (string.IsNullOrEmpty(key))
				return null;

			return new APIAuthentication(key);
		}


	}

	internal static class AuthHelpers
	{
		/// <summary>
		/// A helper method to swap out <see langword="null"/> <see cref="APIAuthentication"/> objects with the <see cref="APIAuthentication.Default"/> authentication, possibly loaded from ENV or a config file.
		/// </summary>
		/// <param name="auth">The specific authentication to use if not <see langword="null"/></param>
		/// <returns>Either the provided <paramref name="auth"/> or the <see cref="APIAuthentication.Default"/></returns>
		public static APIAuthentication ThisOrDefault(this APIAuthentication auth)
		{
			if (auth == null)
				auth = APIAuthentication.Default;

			return auth;
		}
	}
}
