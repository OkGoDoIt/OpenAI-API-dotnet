using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace OpenAI_API
{
    /// <summary>
    /// Provides access to Files endpoint, upload, list, retrieve, delete
    /// </summary>
    public class FilesEndpoint
    {
        OpenAIAPI Api;
        private const int RetryCount = 10;

        internal FilesEndpoint(OpenAIAPI api)
        {
            this.Api = api;
        }

        /// <summary>
        /// Returns a list of files that belong to the user's organization
        /// https://platform.openai.com/docs/api-reference/files/list
        /// </summary>
        /// <returns></returns>
        public Task<List<DataFile>> ListFilesAsync()
        {
            return ListFilesV1(Api?.Auth);
        }

        /// <summary>
        /// Upload a file that contains document(s) to be used across various endpoints/features. Currently, the size of all the files uploaded by one organization can be up to 1 GB. Please contact us if you need to increase the storage limit.
        /// https://platform.openai.com/docs/api-reference/files/upload
        /// </summary>
        /// <param name="fileUploadRequest"></param>
        /// <param name="fileLocation"></param>
        /// <returns></returns>
        public Task<DataFile> UploadFile(FileUploadRequest fileUploadRequest, string fileLocation)
        {
            return UploadFileV1(fileUploadRequest, fileLocation, Api?.Auth);
        }

        /// <summary>
        /// Returns information about a specific file.
        /// https://platform.openai.com/docs/api-reference/files/retrieve
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public Task<DataFile> RetrieveFile(string fileId)
        {
            return RetrieveFileV1(fileId, Api?.Auth);
        }

        /// <summary>
        /// Delete a file.
        /// https://platform.openai.com/docs/api-reference/files/delete
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public Task<DataFile> DeleteFile(string fileId)
        {
            return DeleteFileV1(fileId, Api?.Auth);
        }
        private static async Task<List<DataFile>> ListFilesV1(APIAuthentication auth = null)
        {
            if (auth.ThisOrDefault()?.ApiKey is null)
            {
                throw new AuthenticationException("You must provide API authentication.  Please refer to https://github.com/OkGoDoIt/OpenAI-API-dotnet#authentication for details.");
            }

            var client = GetAuthClient();

            var response = await client.GetAsync(@"https://api.openai.com/v1/files");

            var dataFileList = await HandleDataFileListResponse(response);

            return dataFileList.Files;

        }

        private static async Task<DataFile> UploadFileV1(FileUploadRequest fileUploadRequest, string fileLocation, APIAuthentication auth = null)
        {

            var client = GetAuthClient();

            var byteArray = File.ReadAllBytes(fileLocation);

            var multipartFormDataContent = new MultipartFormDataContent
            {
                { new StringContent(fileUploadRequest.Purpose), "purpose" },
                { new ByteArrayContent(byteArray), "file", fileUploadRequest.Filename }
            };

            var response = await client.PostAsync(@"https://api.openai.com/v1/files", multipartFormDataContent);

            var dataFile = await HandleDataFileResponse(response);

            return dataFile;
        }

        private static async Task<DataFile> RetrieveFileV1(string fileId, APIAuthentication auth = null)
        {

            var client = GetAuthClient();

            var response = await client.GetAsync(@$"https://api.openai.com/v1/files/{fileId}");

            var dataFile = await HandleDataFileResponse(response);

            return dataFile;

        }

        private static async Task<DataFile> DeleteFileV1(string fileId, APIAuthentication auth = null)
        {

            var client = GetAuthClient();

            var response = await client.DeleteAsync(@$"https://api.openai.com/v1/files/{fileId}");
            var retryCount = 0;

            // WHILE LOOP HANDLES ISSUE WHERE FILE UPLOAD IS STILL PROCESSING WHILE ATTEMPTING TO DELETE
            while (response.StatusCode == HttpStatusCode.Conflict)
            {
                retryCount++;
                Thread.Sleep(1000);
                response = await client.DeleteAsync(@$"https://api.openai.com/v1/files/{fileId}");
                if (retryCount > RetryCount) break;
            }

            var dataFile = await HandleDataFileResponse(response);

            return dataFile;
        }

        private static async Task<FilesListResponse> HandleDataFileListResponse(HttpResponseMessage response)
        {
            var resultAsString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var filesList = JsonConvert.DeserializeObject<FilesListResponse>(resultAsString);
                return filesList;
            }
            else
            {
                throw new HttpRequestException("Error calling OpenAi API file endpoint.  HTTP status code: " +
                                               response.StatusCode.ToString() + ". Content: " + resultAsString);
            }

        }

        private static async Task<DataFile> HandleDataFileResponse(HttpResponseMessage response)
        {
            var resultAsString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var file = JsonConvert.DeserializeObject<DataFile>(resultAsString);
                return file;
            }
            else
            {
                throw new HttpRequestException("Error calling OpenAi API file endpoint.  HTTP status code: " +
                                               response.StatusCode.ToString() + ". Content: " + resultAsString);
            }
            
        }
        private static HttpClient GetAuthClient(APIAuthentication auth = null)
        {
            if (auth.ThisOrDefault()?.ApiKey is null)
            {
                throw new AuthenticationException(
                    "You must provide API authentication.  Please refer to https://github.com/OkGoDoIt/OpenAI-API-dotnet#authentication for details.");
            }

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", auth.ThisOrDefault().ApiKey);
            client.DefaultRequestHeaders.Add("User-Agent", "okgodoit/dotnet_openai_api");
            if (!string.IsNullOrEmpty(auth.ThisOrDefault().OpenAIOrganization))
                client.DefaultRequestHeaders.Add("OpenAI-Organization", auth.ThisOrDefault().OpenAIOrganization);

            return client;
        }
    }
}
