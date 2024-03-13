using System.Threading.Tasks;
using OpenAI_API.Common;

namespace OpenAI_API.Assistants
{
    /// <summary>
    /// An interface for the Assistants API endpoint.
    /// </summary>
    public interface IAssistantsEndpoint
    {
        /// <summary>
        /// Creates an assistant with a model and instructions.
        /// </summary>
        /// 
        /// <param name="request">
        /// The request to create an assistant.
        /// </param>
        /// 
        /// <returns>
        /// The created assistant object.
        /// </returns>
        Task<AssistantResult> CreateAssistant(AssistantRequest request);

        /// <summary>
        /// Creates an assistant file by attaching a file to an assistant.
        /// </summary>
        /// 
        /// <param name="assistantId">
        /// The ID of the assistant to which to create the file.
        /// </param>
        /// <param name="request">
        /// The request to create an assistant file.
        /// </param>
        /// 
        /// <returns>
        /// The created assistant file object.
        /// </returns>
        Task<AssistantFileResult> CreateAssistantFile(string assistantId, AssistantFileRequest request);

        /// <summary>
        /// Lists the assistants.
        /// </summary>
        /// 
        /// <param name="queryParams">
        /// The parameters to use to list the assistants. If unspecified, the default parameters are used.
        /// </param>
        /// 
        /// <returns>
        /// A list of assistant objects.
        /// </returns>
        Task<ResultsList<AssistantResult>> ListAssistants(QueryParams queryParams = null);

        /// <summary>
        /// Lists the assistant files.
        /// </summary>
        /// 
        /// <param name="assistantId">
        /// The ID of the assistant for which to list the files.
        /// </param>
        /// <param name="queryParams">
        /// The parameters to use to list the assistant files. If unspecified, the default parameters are used.
        /// </param>
        /// 
        /// <returns>
        /// A list of assistant file objects.
        /// </returns>
        Task<ResultsList<AssistantFileResult>> ListAssistantFiles(string assistantId, QueryParams queryParams = null);

        /// <summary>
        /// Retrieves an assistant.
        /// </summary>
        /// 
        /// <param name="assistantId">
        /// The ID of the assistant to retrieve.
        /// </param>
        /// 
        /// <returns>
        /// The assistant object matching the ID.
        /// </returns>
        Task<AssistantResult> RetrieveAssistant(string assistantId);

        /// <summary>
        /// Retrieves an assistant file.
        /// </summary>
        /// 
        /// <param name="assistantId">
        /// The ID of the assistant to which the file is attached.
        /// </param>
        /// <param name="fileId">
        /// The ID of the file to retrieve.
        /// </param>
        /// 
        /// <returns>
        /// The assistant file object matching the ID.
        /// </returns>
        Task<AssistantFileResult> RetrieveAssistantFile(string assistantId, string fileId);

        /// <summary>
        /// Modifies an assistant.
        /// </summary>
        /// 
        /// <param name="assistantId">
        /// The ID of the assistant to modify.
        /// </param>
        /// <param name="request">
        /// The request to modify the assistant.
        /// </param>
        /// 
        /// <returns>
        /// The modified assistant object matching the specified ID.
        /// </returns>
        Task<AssistantResult> ModifyAssistant(string assistantId, AssistantRequest request);

        /// <summary>
        /// Deletes an assistant.
        /// </summary>
        /// 
        /// <param name="assistantId">
        /// The ID of the assistant to delete.
        /// </param>
        /// 
        /// <returns>
        /// The status of the deletion.
        /// </returns>
        Task<DeletionStatus> DeleteAssistant(string assistantId);

        /// <summary>
        /// Deletes an assistant file.
        /// </summary>
        /// 
        /// <param name="assistantId">
        /// The ID of the assistant from which to delete the file.
        /// </param>
        /// <param name="fileId">
        /// The ID of the file to delete.
        /// </param>
        /// 
        /// <returns>
        /// The status of the deletion.
        /// </returns>
        Task<DeletionStatus> DeleteAssistantFile(string assistantId, string fileId);
    }
}