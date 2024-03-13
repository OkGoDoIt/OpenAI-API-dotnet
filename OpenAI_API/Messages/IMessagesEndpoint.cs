using System.Threading.Tasks;
using OpenAI_API.Common;

namespace OpenAI_API.Messages
{
    /// <summary>
    /// An interface for the Messages API endpoint.
    /// </summary>
    public interface IMessagesEndpoint
    {
        /// <summary>
        /// Creates a message.
        /// </summary>
        /// 
        /// <param name="threadId">
        /// The ID of the thread to create the message in.
        /// </param>
        /// <param name="request">
        /// The request object.
        /// </param>
        /// 
        /// <returns>
        /// The created message object.
        /// </returns>
        public Task<MessageResult> CreateMessage(string threadId, MessageRequest request);

        /// <summary>
        /// List the messages in a thread.
        /// </summary>
        /// 
        /// <param name="threadId">
        /// The ID of the thread for which to list the messages.
        /// </param>
        /// <param name="queryParams">
        /// The parameters to use to list the messages. If unspecified, the default parameters are used.
        /// </param>
        /// 
        /// <returns>
        /// A list of message objects.
        /// </returns>
        public Task<ResultsList<MessageResult>> ListMessages(string threadId, QueryParams queryParams = null);

        /// <summary>
        /// Lists the message files of a message.
        /// </summary>
        /// 
        /// <param name="threadId">
        /// The ID of the thread the message and file belong to.
        /// </param>
        /// <param name="messageId">
        /// The ID of the message that the file belong to.
        /// </param>
        /// <param name="queryParams">
        /// The parameters to use to list the message files. If unspecified, the default parameters are used.
        /// </param>
        /// 
        /// <returns>
        /// A list of message file objects.
        /// </returns>
        public Task<ResultsList<MessageFileResult>> ListMessageFiles(
            string threadId,
            string messageId,
            QueryParams queryParams = null
        );

        /// <summary>
        /// Retrieves a message.
        /// </summary>
        /// 
        /// <param name="threadId">
        /// The ID of the thread to retrieve the message from.
        /// </param>
        /// <param name="messageId">
        /// The ID of the message to retrieve.
        /// </param>
        /// 
        /// <returns>
        /// The retrieved message object.
        /// </returns>
        public Task<MessageResult> RetrieveMessage(string threadId, string messageId);
        
        /// <summary>
        /// Retrieves a message file.
        /// </summary>
        /// 
        /// <param name="threadId">
        /// The ID of the thread to which the message and file belong.
        /// </param>
        /// <param name="messageId">
        /// The ID of the message the file belongs to.
        /// </param>
        /// <param name="fileId">
        /// The ID of the file being retrieved.
        /// </param>
        /// 
        /// <returns>
        /// The message file object matching the ID.
        /// </returns>
        public Task<MessageFileResult> RetrieveMessageFile(string threadId, string messageId, string fileId);
        
        /// <summary>
        /// Modifies a message.
        /// </summary>
        /// 
        /// <param name="threadId">
        /// The ID of the thread to which the message belongs.
        /// </param>
        /// <param name="messageId">
        /// The ID of the message to modify.
        /// </param>
        /// <param name="request">
        /// The request object.
        /// </param>
        /// 
        /// <returns>
        /// The modified message object matching the specified ID.
        /// </returns>
        public Task<MessageResult> ModifyMessage(string threadId, string messageId, MetadataRequest request);
    }
}