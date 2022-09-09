using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_API.Files
{
	/// <summary>
	/// The API endpoint for operations List, Upload, Delete, Retrieve files
	/// </summary>
	public class FilesEndpoint : BaseEndpoint
	{
		public FilesEndpoint(OpenAIAPI api) : base(api) {}

		protected override string GetEndpoint() { return "files"; }

		/// <summary>
		/// Get the list of all files
		/// </summary>
		/// <returns></returns>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<List<File>> GetFilesAsync()
		{
			var response = await GetClient().GetAsync(GetUrl());
			string resultAsString = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				var files = JsonConvert.DeserializeObject<FilesData>(resultAsString).Data;
				return files;
			}
			throw new HttpRequestException(GetErrorMessage(resultAsString, response, "List files", "Get the list of all files"));
		}

		/// <summary>
		/// Returns information about a specific file
		/// </summary>
		/// <param name="fileId">The ID of the file to use for this request</param>
		/// <returns></returns>
		public async Task<File> GetFileAsync(string fileId)
		{
			var response = await GetClient().GetAsync($"{GetUrl()}/{fileId}");
			string resultAsString = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				var file = JsonConvert.DeserializeObject<File>(resultAsString);
				return file;
			}
			throw new HttpRequestException(GetErrorMessage(resultAsString, response, "Retrieve file"));
		}


		/// <summary>
		/// Returns the contents of the specific file as string
		/// </summary>
		/// <param name="fileId">The ID of the file to use for this request</param>
		/// <returns></returns>
		public async Task<string> GetFileContentAsStringAsync(string fileId)
		{
			var response = await GetClient().GetAsync($"{GetUrl()}/{fileId}/content");
			string resultAsString = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				return resultAsString;
				
			}
			throw new HttpRequestException(GetErrorMessage(resultAsString, response, "Retrieve file content"));
		}

		/// <summary>
		/// Delete a file
		///	</summary>
		///	 <param name="fileId">The ID of the file to use for this request</param>
		/// <returns></returns>
		public async Task<File> DeleteFileAsync(string fileId)
		{
			var response = await GetClient().DeleteAsync($"{GetUrl()}/{fileId}");
			string resultAsString = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				var file = JsonConvert.DeserializeObject<File>(resultAsString);
				return file;
			}
			throw new HttpRequestException(GetErrorMessage(resultAsString, response, "Delete file"));
		}

		/// <summary>
		/// Upload a file that contains document(s) to be used across various endpoints/features. Currently, the size of all the files uploaded by one organization can be up to 1 GB. Please contact us if you need to increase the storage limit
		/// </summary>
		/// <param name="file">The name of the file to use for this request</param>
		/// <param name="purpose">The intendend purpose of the uploaded documents. Use "fine-tune" for Fine-tuning. This allows us to validate the format of the uploaded file.</param>
		public async Task<File> UploadFileAsync(string file, string purpose = "fine-tune")
		{
			HttpClient client = GetClient();
			var content = new MultipartFormDataContent
			{
				{ new StringContent(purpose), "purpose" },
				{ new ByteArrayContent(System.IO.File.ReadAllBytes(file)), "file", Path.GetFileName(file) }
			};
			var response = await client.PostAsync($"{GetUrl()}", content);
			string resultAsString = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				return JsonConvert.DeserializeObject<File>(resultAsString);
			}
			throw new HttpRequestException(GetErrorMessage(resultAsString, response, "Upload file", "Upload a file that contains document(s) to be used across various endpoints/features."));
		}
	}
	/// <summary>
	/// A helper class to deserialize the JSON API responses.  This should not be used directly.
	/// </summary>
	class FilesData
	{
		[JsonProperty("data")]
		public List<File> Data { get; set; }
		[JsonProperty("object")]
		public string Obj { get; set; }
	}
}
