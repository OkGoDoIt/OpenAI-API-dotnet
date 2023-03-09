using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenAI_API.Files
{
	/// <summary>
	/// An interface for <see cref="FilesEndpoint"/>, for ease of mock testing, etc
	/// </summary>
	public interface IFilesEndpoint
    {
        /// <summary>
        /// Get the list of all files
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        Task<List<File>> GetFilesAsync();

        /// <summary>
        /// Returns information about a specific file
        /// </summary>
        /// <param name="fileId">The ID of the file to use for this request</param>
        /// <returns></returns>
        Task<File> GetFileAsync(string fileId);

        /// <summary>
        /// Returns the contents of the specific file as string
        /// </summary>
        /// <param name="fileId">The ID of the file to use for this request</param>
        /// <returns></returns>
        Task<string> GetFileContentAsStringAsync(string fileId);

        /// <summary>
        /// Delete a file
        ///	</summary>
        ///	 <param name="fileId">The ID of the file to use for this request</param>
        /// <returns></returns>
        Task<File> DeleteFileAsync(string fileId);

        /// <summary>
        /// Upload a file that contains document(s) to be used across various endpoints/features. Currently, the size of all the files uploaded by one organization can be up to 1 GB. Please contact OpenAI if you need to increase the storage limit
        /// </summary>
        /// <param name="filePath">The name of the file to use for this request</param>
        /// <param name="purpose">The intendend purpose of the uploaded documents. Use "fine-tune" for Fine-tuning. This allows us to validate the format of the uploaded file.</param>
        Task<File> UploadFileAsync(string filePath, string purpose = "fine-tune");
    }
}