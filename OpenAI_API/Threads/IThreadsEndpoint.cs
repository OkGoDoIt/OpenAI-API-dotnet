using System.Threading.Tasks;
using OpenAI_API.Common;

namespace OpenAI_API.Threads
{
    /// <summary>
    /// An interface for the Threads API endpoint.
    /// </summary>
    public interface IThreadsEndpoint
    {
        /// <summary>
        /// Creates a thread.
        /// </summary>
        /// 
        /// <param name="request">
        /// The request object.
        /// </param>
        /// 
        /// <returns>
        /// The created thread object.
        /// </returns>
        public Task<ThreadResult> CreateThread(ThreadRequest request);
        
        /// <summary>
        /// Retrieves a thread.
        /// </summary>
        /// 
        /// <param name="threadId">
        /// The ID of the thread to retrieve.
        /// </param>
        /// 
        /// <returns>
        /// The thread object matching the specified ID.
        /// </returns>
        public Task<ThreadResult> RetrieveThread(string threadId);

        /// <summary>
        /// Modifies a thread. Only the <c>metadata</c> can be modified.
        /// </summary>
        /// 
        /// <param name="threadId">
        /// The ID of the thread to modify.
        /// </param>
        /// <param name="request">
        /// The request object.
        /// </param>
        /// 
        /// <returns>
        /// The modified thread object matching the specified ID.
        /// </returns>
        public Task<ThreadResult> ModifyThread(string threadId, MetadataRequest request);
        
        /// <summary>
        /// Deletes a thread.
        /// </summary>
        /// 
        /// <param name="threadId">
        /// The ID of the thread to delete.
        /// </param>
        /// 
        /// <returns>
        /// The status of the deletion.
        /// </returns>
        public Task<DeletionStatus> DeleteThread(string threadId);
    }
}