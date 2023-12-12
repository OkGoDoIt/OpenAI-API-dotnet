using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace OpenAI_API.Chat
{
	/// <summary>
	/// Represents on ongoing chat with back-and-forth interactions between the user and the chatbot.  This is the simplest way to interact with the ChatGPT API, rather than manually using the ChatEnpoint methods.  You do lose some flexibility though.
	/// </summary>
	public class Conversation
	{
		/// <summary>
		/// An internal reference to the API endpoint, needed for API requests
		/// </summary>
		private ChatEndpoint _endpoint;

		/// <summary>
		/// Allows setting the parameters to use when calling the ChatGPT API.  Can be useful for setting temperature, presence_penalty, and more.  <see href="https://platform.openai.com/docs/api-reference/chat/create">Se  OpenAI documentation for a list of possible parameters to tweak.</see>
		/// </summary>
		public ChatRequest RequestParameters { get; private set; }

		/// <summary>
		/// Specifies the model to use for ChatGPT requests.  This is just a shorthand to access <see cref="RequestParameters"/>.Model
		/// </summary>
		public OpenAI_API.Models.Model Model
		{
			get
			{
				return RequestParameters.Model;
			}
			set
			{
				RequestParameters.Model = value;
			}
		}

		/// <summary>
		/// After calling <see cref="GetResponseFromChatbotAsync"/>, this contains the full response object which can contain useful metadata like token usages, <see cref="ChatChoice.FinishReason"/>, etc.  This is overwritten with every call to <see cref="GetResponseFromChatbotAsync"/> and only contains the most recent result.
		/// </summary>
		public ChatResult MostRecentApiResult { get; private set; }

		/// <summary>
		/// Creates a new conversation with ChatGPT chat
		/// </summary>
		/// <param name="endpoint">A reference to the API endpoint, needed for API requests.  Generally should be <see cref="OpenAIAPI.Chat"/>.</param>
		/// <param name="model">Optionally specify the model to use for ChatGPT requests.  If not specified, used <paramref name="defaultChatRequestArgs"/>.Model or falls back to <see cref="OpenAI_API.Models.Model.DefaultChatModel"/></param>
		/// <param name="defaultChatRequestArgs">Allows setting the parameters to use when calling the ChatGPT API.  Can be useful for setting temperature, presence_penalty, and more.  See <see href="https://platform.openai.com/docs/api-reference/chat/create">OpenAI documentation for a list of possible parameters to tweak.</see></param>
		public Conversation(ChatEndpoint endpoint, OpenAI_API.Models.Model model = null, ChatRequest defaultChatRequestArgs = null)
		{
			RequestParameters = new ChatRequest(defaultChatRequestArgs);
			if (model != null)
				RequestParameters.Model = model;
			if (RequestParameters.Model == null)
				RequestParameters.Model = Models.Model.DefaultChatModel;

			_Messages = new List<ChatMessage>();
			_endpoint = endpoint;
			RequestParameters.NumChoicesPerMessage = 1;
			RequestParameters.Stream = false;
		}

		/// <summary>
		/// A list of messages exchanged so far. To append to this list, use <see cref="AppendMessage(ChatMessage)"/>, <see cref="AppendUserInput(string)"/>, <see cref="AppendSystemMessage(string)"/>, or <see cref="AppendExampleChatbotOutput(string)"/>.
		/// </summary>
		public IList<ChatMessage> Messages { get => _Messages; }
		private List<ChatMessage> _Messages;

		/// <summary>
		/// Appends a <see cref="ChatMessage"/> to the chat history
		/// </summary>
		/// <param name="message">The <see cref="ChatMessage"/> to append to the chat history</param>
		public void AppendMessage(ChatMessage message)
		{
			_Messages.Add(message);
		}

		/// <summary>
		/// Creates and appends a <see cref="ChatMessage"/> to the chat history
		/// </summary>
		/// <param name="role">The <see cref="ChatMessageRole"/> for the message.  Typically, a conversation is formatted with a system message first, followed by alternating user and assistant messages.  See <see href="https://platform.openai.com/docs/guides/chat/introduction">the OpenAI docs</see> for more details about usage.</param>
		/// <param name="text">The text content of the message</param>
		/// <param name="images">Optionally, one or more images to include in the message. Only works with GPT Vision models, so be sure to set your model correctly.  Consider using <see cref="ChatMessage.ImageInput.FromFile(string, string)"/> to load an image from a local file, or <see cref="ChatMessage.ImageInput.FromImageUrl(string, string)"/> to point to an image via URL.  Please see <seealso href="https://platform.openai.com/docs/guides/vision"/> for more information and limitations.</param>
		public void AppendMessage(ChatMessageRole role, string text, params ChatMessage.ImageInput[] images) => this.AppendMessage(new ChatMessage(role, text, images));

		/// <summary>
		/// Creates and appends a <see cref="ChatMessage"/> to the chat history with the Role of <see cref="ChatMessageRole.User"/>.  The user messages help instruct the assistant. They can be generated by the end users of an application, or set by a developer as an instruction.
		/// </summary>
		/// <param name="text">Text content generated by the end users of an application, or set by a developer as an instruction</param>
		/// <param name="images">Optionally, one or more images to include in the message. Only works with GPT Vision models, so be sure to set your model correctly.  Consider using <see cref="ChatMessage.ImageInput.FromFile(string, string)"/> to load an image from a local file, or <see cref="ChatMessage.ImageInput.FromImageUrl(string, string)"/> to point to an image via URL.  Please see <seealso href="https://platform.openai.com/docs/guides/vision"/> for more information and limitations.</param>
		public void AppendUserInput(string text, params ChatMessage.ImageInput[] images) => this.AppendMessage(new ChatMessage(ChatMessageRole.User, text, images));

		/// <summary>
		/// Creates and appends a <see cref="ChatMessage"/> to the chat history with the Role of <see cref="ChatMessageRole.User"/>.  The user messages help instruct the assistant. They can be generated by the end users of an application, or set by a developer as an instruction.
		/// </summary>
		/// <param name="userName">The name of the user in a multi-user chat</param>
		/// <param name="text">Text content generated by the end users of an application, or set by a developer as an instruction</param>
		/// <param name="images">Optionally, one or more images to include in the message. Only works with GPT Vision models, so be sure to set your model correctly.  Consider using <see cref="ChatMessage.ImageInput.FromFile(string, string)"/> to load an image from a local file, or <see cref="ChatMessage.ImageInput.FromImageUrl(string, string)"/> to point to an image via URL.  Please see <seealso href="https://platform.openai.com/docs/guides/vision"/> for more information and limitations.</param>
		public void AppendUserInputWithName(string userName, string text, params ChatMessage.ImageInput[] images) => this.AppendMessage(new ChatMessage(ChatMessageRole.User, text, images) { Name = userName });


		/// <summary>
		/// Creates and appends a <see cref="ChatMessage"/> to the chat history with the Role of <see cref="ChatMessageRole.System"/>.  The system message helps set the behavior of the assistant.
		/// </summary>
		/// <param name="content">text content that helps set the behavior of the assistant</param>
		public void AppendSystemMessage(string content) => this.AppendMessage(new ChatMessage(ChatMessageRole.System, content));
		/// <summary>
		/// Creates and appends a <see cref="ChatMessage"/> to the chat history with the Role of <see cref="ChatMessageRole.Assistant"/>.  Assistant messages can be written by a developer to help give examples of desired behavior.
		/// </summary>
		/// <param name="content">Text content written by a developer to help give examples of desired behavior</param>
		public void AppendExampleChatbotOutput(string content) => this.AppendMessage(new ChatMessage(ChatMessageRole.Assistant, content));

		/// <summary>
		/// An event called when the chat message history is too long, which should reduce message history length through whatever means is appropriate for your use case.  You may want to remove the first entry in the <see cref="List{ChatMessage}"/> in the <see cref="EventArgs"/>
		/// </summary>
		public event EventHandler<List<ChatMessage>> OnTruncationNeeded;

		/// <summary>
		/// Sometimes the total length of your conversation can get too long to fit in the ChatGPT context window.  In this case, the <see cref="OnTruncationNeeded"/> event will be called, if supplied.  If not supplied and this is <see langword="true"/>, then the first one or more user or assistant messages will be automatically deleted from the beginning of the conversation message history until the API call succeeds.  This may take some time as it may need to loop several times to clear enough messages.  If this is set to false and no <see cref="OnTruncationNeeded"/> is supplied, then an <see cref="ArgumentOutOfRangeException"/> will be raised when the API returns a context_length_exceeded error.
		/// </summary>
		public bool AutoTruncateOnContextLengthExceeded { get; set; } = true;

		#region Non-streaming

		/// <summary>
		/// Calls the API to get a response, which is appended to the current chat's <see cref="Messages"/> as an <see cref="ChatMessageRole.Assistant"/> <see cref="ChatMessage"/>.
		/// </summary>
		/// <returns>The string of the response from the chatbot API</returns>
		public async Task<string> GetResponseFromChatbotAsync()
		{
			try
			{
				ChatRequest req = new ChatRequest(RequestParameters);
				req.Messages = _Messages.ToList();

				var res = await _endpoint.CreateChatCompletionAsync(req);
				MostRecentApiResult = res;

				if (res.Choices.Count > 0)
				{
					var newMsg = res.Choices[0].Message;
					AppendMessage(newMsg);
					return newMsg.TextContent;
				}
			}
			catch (HttpRequestException ex)
			{
				if (ex.Data.Contains("code") && (!string.IsNullOrEmpty(ex.Data["code"] as string)) && ex.Data["code"].Equals("context_length_exceeded"))
				{
					string message = "The context length of this conversation is too long for the OpenAI API to handle.  Consider shortening the message history by handling the OnTruncationNeeded event and removing some of the messages in the argument.";
					if (ex.Data.Contains("message"))
					{
						message += "  " + ex.Data["message"].ToString();
					}

					if (OnTruncationNeeded != null)
					{
						var prevLength = this.Messages.Sum(m => m.TextContent.Length);
						OnTruncationNeeded(this, this._Messages);
						if (prevLength > this.Messages.Sum(m => m.TextContent.Length))
						{
							// the messages have been truncated, so try again
							return await GetResponseFromChatbotAsync();
						}
						else
						{
							// no truncation happened, so throw error instead
							throw new ArgumentOutOfRangeException("OnTruncationNeeded was called but it did not reduce the message history length.  " + message, ex);
						}
					}
					else if (AutoTruncateOnContextLengthExceeded)
					{
						for (int i = 0; i < _Messages.Count; i++)
						{
							if (_Messages[i].Role != ChatMessageRole.System)
							{
								_Messages.RemoveAt(i);
								// the messages have been truncated, so try again
								return await GetResponseFromChatbotAsync();
							}
						}
					}
					else
					{
						throw new ArgumentOutOfRangeException(message, ex);
					}
				}
				else
				{
					throw ex;
				}
			}
			return null;

		}

		/// <summary>
		/// OBSOLETE: GetResponseFromChatbot() has been renamed to <see cref="GetResponseFromChatbotAsync"/> to follow .NET naming guidelines.  This alias will be removed in a future version.
		/// </summary>
		/// <returns>The string of the response from the chatbot API</returns>
		[Obsolete("Conversation.GetResponseFromChatbot() has been renamed to GetResponseFromChatbotAsync to follow .NET naming guidelines.  Please update any references to GetResponseFromChatbotAsync().  This alias will be removed in a future version.", false)]
		public Task<string> GetResponseFromChatbot() => GetResponseFromChatbotAsync();


		#endregion

		#region Streaming

		/// <summary>
		/// Calls the API to get a response, which is appended to the current chat's <see cref="Messages"/> as an <see cref="ChatMessageRole.Assistant"/> <see cref="ChatMessage"/>, and streams the results to the <paramref name="resultHandler"/> as they come in. <br/>
		/// If you are on the latest C# supporting async enumerables, you may prefer the cleaner syntax of <see cref="StreamResponseEnumerableFromChatbotAsync"/> instead.
		///  </summary>
		/// <param name="resultHandler">An action to be called as each new result arrives.</param>
		public async Task StreamResponseFromChatbotAsync(Action<string> resultHandler)
		{
			await foreach (string res in StreamResponseEnumerableFromChatbotAsync())
			{
				resultHandler(res);
			}
		}

		/// <summary>
		/// Calls the API to get a response, which is appended to the current chat's <see cref="Messages"/> as an <see cref="ChatMessageRole.Assistant"/> <see cref="ChatMessage"/>, and streams the results to the <paramref name="resultHandler"/> as they come in. <br/>
		/// If you are on the latest C# supporting async enumerables, you may prefer the cleaner syntax of <see cref="StreamResponseEnumerableFromChatbotAsync"/> instead.
		///  </summary>
		/// <param name="resultHandler">An action to be called as each new result arrives, which includes the index of the result in the overall result set.</param>
		public async Task StreamResponseFromChatbotAsync(Action<int, string> resultHandler)
		{
			int index = 0;
			await foreach (string res in StreamResponseEnumerableFromChatbotAsync())
			{
				resultHandler(index++, res);
			}
		}

		/// <summary>
		/// Calls the API to get a response, which is appended to the current chat's <see cref="Messages"/> as an <see cref="ChatMessageRole.Assistant"/> <see cref="ChatMessage"/>, and streams the results as they come in. <br/>
		/// If you are not using C# 8 supporting async enumerables or if you are using the .NET Framework, you may need to use <see cref="StreamResponseFromChatbotAsync(Action{string})"/> instead.
		/// </summary>
		/// <returns>An async enumerable with each of the results as they come in.  See <see href="https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#asynchronous-streams"/> for more details on how to consume an async enumerable.</returns>
		public async IAsyncEnumerable<string> StreamResponseEnumerableFromChatbotAsync()
		{
			ChatRequest req = null;

			StringBuilder responseStringBuilder = new StringBuilder();
			ChatMessageRole responseRole = null;

			IAsyncEnumerable<ChatResult> resStream = null;

			bool retrying = true;
			ChatResult firstStreamedResult = null;
			IAsyncEnumerator<ChatResult> enumerator = null;
			while (retrying)
			{
				retrying = false;
				req = new ChatRequest(RequestParameters);
				req.Messages = _Messages.ToList();
				try
				{
					resStream = _endpoint.StreamChatEnumerableAsync(req);
					enumerator = resStream.GetAsyncEnumerator();
					await enumerator.MoveNextAsync();
					firstStreamedResult = enumerator.Current;
				}
				catch (HttpRequestException ex)
				{
					if (ex.Data.Contains("code") && (!string.IsNullOrEmpty(ex.Data["code"] as string)) && ex.Data["code"].Equals("context_length_exceeded"))
					{
						string message = "The context length of this conversation is too long for the OpenAI API to handle.  Consider shortening the message history by handling the OnTruncationNeeded event and removing some of the messages in the argument.";
						if (ex.Data.Contains("message"))
						{
							message += "  " + ex.Data["message"].ToString();
						}

						if (OnTruncationNeeded != null)
						{
							var prevLength = this.Messages.Sum(m => m.TextContent.Length);
							OnTruncationNeeded(this, this._Messages);
							if (prevLength > this.Messages.Sum(m => m.TextContent.Length))
							{
								// the messages have been truncated, so try again
								retrying = true;
							}
							else
							{
								// no truncation happened, so throw error instead
								retrying = false;
								throw new ArgumentOutOfRangeException("OnTruncationNeeded was called but it did not reduce the message history length.  " + message, ex);
							}
						}
						else if (AutoTruncateOnContextLengthExceeded)
						{
							for (int i = 0; i < _Messages.Count; i++)
							{
								if (_Messages[i].Role != ChatMessageRole.System)
								{
									_Messages.RemoveAt(i);
									// the messages have been truncated, so try again
									retrying = true;
									break;
								}
							}
						}
						else
						{
							retrying = false;
							throw new ArgumentOutOfRangeException(message, ex);
						}
					}
					else
					{
						throw ex;
					}
				}
			}

			if (resStream == null)
			{
				throw new Exception("The chat result stream is null, but it shouldn't be");
			}

			do
			{
				ChatResult res = enumerator.Current;
				if (res.Choices.FirstOrDefault()?.Delta is ChatMessage delta)
				{
					if (delta.Role != null)
						responseRole = delta.Role;

					string deltaTextContent = delta.TextContent;

					if (!string.IsNullOrEmpty(deltaTextContent))
					{
						responseStringBuilder.Append(deltaTextContent);
						yield return deltaTextContent;
					}
				}
				MostRecentApiResult = res;
			} while (await enumerator.MoveNextAsync());
			
			if (responseRole != null)
			{
				AppendMessage(responseRole, responseStringBuilder.ToString());
			}
		}

		#endregion
	}
}
