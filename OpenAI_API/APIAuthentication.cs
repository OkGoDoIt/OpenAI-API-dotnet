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
		/// The publishable API key.  Some of the API endpoints do not appear to work with the public API key, so this is only used as a fallback if there is no <see cref="Secretkey"/> provided.  When better guidance is provided by OpenAI, this behavior may change.
		/// </summary>
		public string APIKey { get; set; }

		/// <summary>
		/// The secret API key.  Some of the API endpoints appear to require a secret API key.
		/// </summary>
		public string Secretkey { get; set; }

		/// <summary>
		/// Allows implicit casting from a string, so that a simple string API key can be provided in place of an instance of <see cref="APIAuthentication"/>
		/// </summary>
		/// <param name="key">The API key to convert into a <see cref="APIAuthentication"/>.  If the key starts with "pk-" then it is interpreted as a publishable <see cref="APIKey"/>, otherwise it is interpreted as a <see cref="Secretkey"/>.</param>
		public static implicit operator APIAuthentication(string key)
		{
			return new APIAuthentication(key);
		}

		/// <summary>
		/// Instantiates a new Authentication object with the given <paramref name="apiKey"/> and/or <paramref name="secretKey"/>, either of which may be <see langword="null"/>.
		/// </summary>
		/// <param name="apiKey">The publishable API key.  Some of the API endpoints do not appear to work with the public API key, so this is only used as a fallback if there is no <paramref name="secretKey"/> provided.  When better guidance is provided by OpenAI, this behavior may change.</param>
		/// <param name="secretKey">The secret API key.  Some of the API endpoints appear to require a secret API key.</param>
		public APIAuthentication(string apiKey, string secretKey)
		{
			this.APIKey = apiKey;
			this.Secretkey = secretKey;
		}

		/// <summary>
		/// Instantiates a new Authentication object with the given key.
		/// </summary>
		/// <param name="key">If the key starts with "pk-" then it is interpreted as a publishable <see cref="APIKey"/>, otherwise it is interpreted as a <see cref="Secretkey"/></param>
		public APIAuthentication(string key)
		{
			if (key != null && key.StartsWith("pk-"))
				this.APIKey = key;
			else
				this.Secretkey = key;
		}

		/// <summary>
		/// Gets a usable API key, preferring the <see cref="Secretkey"/> if there is one.
		/// </summary>
		/// <returns></returns>
		public string GetKey()
		{
			return Secretkey ?? APIKey;
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
		/// Attempts to load api keys from environment variables, as "OPENAI_KEY" and/or "OPENAI_SECRET_KEY"
		/// </summary>
		/// <returns>Returns the loaded <see cref="APIAuthentication"/> any api keys were found, or <see langword="null"/> if there were no matching environment vars.</returns>
		public static APIAuthentication LoadFromEnv()
		{
			var secretKey = Environment.GetEnvironmentVariable("OPENAI_SECRET_KEY");
			var key = Environment.GetEnvironmentVariable("OPENAI_KEY");

			if (string.IsNullOrEmpty(key) && string.IsNullOrEmpty(secretKey))
				return null;

			return new APIAuthentication(key, secretKey);
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

			string secretKey = null;
			string key = null;
			var curDirectory = new DirectoryInfo(directory);

			while (secretKey == null && key == null && curDirectory.Parent != null)
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
									secretKey = parts[1].Trim();
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

			if (string.IsNullOrEmpty(key) && string.IsNullOrEmpty(secretKey))
				return null;

			return new APIAuthentication(key, secretKey);
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
