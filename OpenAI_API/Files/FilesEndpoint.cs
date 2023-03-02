using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenAI_API.Files
{
	/// <summary>
	/// The API endpoint for operations List, Upload, Delete, Retrieve files
	/// </summary>
	public class FilesEndpoint : EndpointBase, IFilesEndpoint
	{
		/// <summary>
		/// Constructor of the api endpoint.  Rather than instantiating this yourself, access it through an instance of <see cref="OpenAIAPI"/> as <see cref="OpenAIAPI.Files"/>.
		/// </summary>
		/// <param name="api"></param>
		internal FilesEndpoint(OpenAIAPI api) : base(api) { }

		/// <summary>
		/// The name of the endpoint, which is the final path segment in the API URL.  For example, "files".
		/// </summary>
		protected override string Endpoint { get { return "files"; } }

		/// <summary>
		/// Get the list of all files
		/// </summary>
		/// <returns></returns>
		/// <exception cref="HttpRequestException"></exception>
		public async Task<List<File>> GetFilesAsync()
		{
			return (await HttpGet<FilesData>()).Data;
		}

		/// <summary>
		/// Returns information about a specific file
		/// </summary>
		/// <param name="fileId">The ID of the file to use for this request</param>
		/// <returns></returns>
		public async Task<File> GetFileAsync(string fileId)
		{
			return await HttpGet<File>($"{Url}/{fileId}");
		}


		/// <summary>
		/// Returns the contents of the specific file as string
		/// </summary>
		/// <param name="fileId">The ID of the file to use for this request</param>
		/// <returns></returns>
		public async Task<string> GetFileContentAsStringAsync(string fileId)
		{
			return await HttpGetContent<File>($"{Url}/{fileId}/content");
		}

		/// <summary>
		/// Delete a file
		///	</summary>
		///	 <param name="fileId">The ID of the file to use for this request</param>
		/// <returns></returns>
		public async Task<File> DeleteFileAsync(string fileId)
		{
			return await HttpDelete<File>($"{Url}/{fileId}");
		}


		/// <summary>
		/// Upload a file that contains document(s) to be used across various endpoints/features. Currently, the size of all the files uploaded by one organization can be up to 1 GB. Please contact OpenAI if you need to increase the storage limit
		/// </summary>
		/// <param name="filePath">The name of the file to use for this request</param>
		/// <param name="purpose">The intendend purpose of the uploaded documents. Use "fine-tune" for Fine-tuning. This allows us to validate the format of the uploaded file.</param>
		public async Task<File> UploadFileAsync(string filePath, string purpose = "fine-tune")
		{
			var content = new MultipartFormDataContent
			{
				{ new StringContent(purpose), "purpose" },
				{ new ByteArrayContent(System.IO.File.ReadAllBytes(filePath)), "file", Path.GetFileName(filePath) }
			};

			return await HttpPost<File>(Url, content);
		}

		/// <summary>
		/// A helper class to deserialize the JSON API responses.  This should not be used directly.
		/// </summary>
		private class FilesData : ApiResultBase
		{
			[JsonProperty("data")]
			public List<File> Data { get; set; }
			[JsonProperty("object")]
			public string Obj { get; set; }
		}
	}


}
