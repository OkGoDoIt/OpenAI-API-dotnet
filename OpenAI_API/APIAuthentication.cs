using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

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
		/// The Organization ID to count API requests against.  This can be found at https://beta.openai.com/account/org-settings.
		/// </summary>
		public string OpenAIOrganization { get; set; }

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


		/// <summary>
		/// Instantiates a new Authentication object with the given <paramref name="apiKey"/>, which may be <see langword="null"/>.  For users who belong to multiple organizations, you can specify which organization is used. Usage from these API requests will count against the specified organization's subscription quota.
		/// </summary>
		/// <param name="apiKey">The API key, required to access the API endpoint.</param>
		/// <param name="openAIOrganization">The Organization ID to count API requests against.  This can be found at https://beta.openai.com/account/org-settings.</param>
		public APIAuthentication(string apiKey, string openAIOrganization)
		{
			this.ApiKey = apiKey;
			this.OpenAIOrganization = openAIOrganization;
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
		/// Attempts to load api key from environment variables, as "OPENAI_KEY" or "OPENAI_API_KEY".  Also loads org if from "OPENAI_ORGANIZATION" if present.
		/// </summary>
		/// <returns>Returns the loaded <see cref="APIAuthentication"/> any api keys were found, or <see langword="null"/> if there were no matching environment vars.</returns>
		public static APIAuthentication LoadFromEnv()
		{
			string key = Environment.GetEnvironmentVariable("OPENAI_KEY");

			if (string.IsNullOrEmpty(key))
			{
				key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

				if (string.IsNullOrEmpty(key))
					return null;
			}

			string org = Environment.GetEnvironmentVariable("OPENAI_ORGANIZATION");

			return new APIAuthentication(key, org);
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
			string org = null;
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
								case "OPENAI_API_KEY":
									key = parts[1].Trim();
									break;
								case "OPENAI_ORGANIZATION":
									org = parts[1].Trim();
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

			return new APIAuthentication(key, org);
		}


		/// <summary>
		/// Tests the api key against the OpenAI API, to ensure it is valid.  This hits the models endpoint so should not be charged for usage.
		/// </summary>
		/// <returns><see langword="true"/> if the api key is valid, or <see langword="false"/> if empty or not accepted by the OpenAI API.</returns>
		public async Task<bool> ValidateAPIKey()
		{
			if (string.IsNullOrEmpty(ApiKey))
				return false;

			var api = new OpenAIAPI(this);

			List<Models.Model> results;

			try
			{
				results = await api.Models.GetModelsAsync();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				return false;
			}

			return (results.Count > 0);
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
