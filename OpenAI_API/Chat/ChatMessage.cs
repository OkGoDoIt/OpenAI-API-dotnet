using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Net.WebRequestMethods;

namespace OpenAI_API.Chat
{
	/// <summary>
	/// Chat message sent or received from the API. Includes who is speaking in the "role" and the message text in the "content"
	/// </summary>
	public class ChatMessage
	{
		/// <summary>
		/// Creates an empty <see cref="ChatMessage"/>, with <see cref="Role"/> defaulting to <see cref="ChatMessageRole.User"/>
		/// </summary>
		public ChatMessage()
		{
			this.Role = ChatMessageRole.User;
		}

		/// <summary>
		/// Constructor for a new Chat Message
		/// </summary>
		/// <param name="role">The role of the message, which can be "system", "assistant" or "user"</param>
		/// <param name="text">The text to send in the message</param>
		public ChatMessage(ChatMessageRole role, string text)
		{
			this.Role = role;
			this.TextContent = text;
		}

		/// <summary>
		/// Constructor for a new Chat Message with text and one or more images
		/// </summary>
		/// <param name="role">The role of the message, which can be "system", "assistant" or "user"</param>
		/// <param name="text">The text to send in the message.  May be null if only sending image(s).</param>
		/// <param name="imageInputs">Optionally add one or more images to the message if using a GPT Vision model.  Consider using <see cref="ImageInput.FromFile(string, string)"/> to load an image from a local file, or <see cref="ImageInput.FromImageUrl(string, string)"/> to point to an image via URL.  Please see <seealso href="https://platform.openai.com/docs/guides/vision"/> for more information and limitations.</param>
		public ChatMessage(ChatMessageRole role, string text, params ImageInput[] imageInputs)
		{
			this.Role = role;
			this.TextContent = text;
			this.Images.AddRange(imageInputs);
		}

		[JsonProperty("role")]
		internal string rawRole { get; set; }

		/// <summary>
		/// The role of the message, which can be "system", "assistant" or "user"
		/// </summary>
		[JsonIgnore]
		public ChatMessageRole Role
		{
			get
			{
				return ChatMessageRole.FromString(rawRole);
			}
			set
			{
				rawRole = value.ToString();
			}
		}

		/// <summary>
		/// The text content of the message.
		/// </summary>
		[JsonIgnore]
		public string TextContent { get; set; }

		/// <summary>
		/// To support multi-modal messages, this property has been renamed to <see cref="TextContent"/>.  Please use that instead."/>
		/// </summary>
		[Obsolete("This property has been renamed to TextContent.")]
		[JsonIgnore]
		public string Content { get => TextContent; set => TextContent = value; }

		/// <summary>
		/// This is only used for serializing the request into JSON, do not use it directly.
		/// </summary>
		[JsonProperty("content")]
		[JsonConverter(typeof(ContentDataConverter))]
		internal IList<ContentItem> ContentItems
		{
			get
			{
				List<ContentItem> items = new List<ContentItem>();
				if (!string.IsNullOrEmpty(TextContent))
				{
					items.Add(new ContentItem(TextContent));
				}
				if (Images != null && Images.Count > 0)
				{
					foreach (var image in Images)
					{
						items.Add(new ContentItem(image));
					}
				}

				return items;
			}
			set
			{
				foreach (var item in value)
				{
					if (item.Type == "text")
					{
						TextContent = item.Text;
					}
					else if (item.Type == "image_url")
					{
						Images.Add(item.Image);
					}
				}
			}
		}

		/// <summary>
		/// An optional name of the user in a multi-user chat 
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// Optionally add one or more images to the message if using a GPT Vision model.  Please see <seealso href="https://platform.openai.com/docs/guides/vision"/> for more information and limitations.
		/// </summary>
		[JsonIgnore]
		public List<ImageInput> Images { get; set; } = new List<ImageInput>();

		/// <summary>
		/// This is a helper class to serialize the content of the message to JSON
		/// </summary>
		internal class ContentItem
		{
			private string text;
			private ImageInput image;

			/// <summary>
			/// The type of content to send to the API.  This can be "text" or "image_url".
			/// </summary>
			[JsonProperty("type")]
			public string Type { get; set; } = "text";

			/// <summary>
			/// Sends text to the API.  This is the default type.
			/// </summary>
			[JsonProperty("text")]
			public string Text
			{
				get
				{
					if (Type == "text")
						return text;
					else
						return null;
				}

				set
				{
					text = value;
					image = null;
					Type = "text";
				}
			}

			/// <summary>
			/// Send an image to GPT Vision.  Please see <seealso href="https://platform.openai.com/docs/guides/vision"/> for more information and limitations."/>
			/// </summary>
			[JsonProperty("image_url")]
			public ImageInput Image
			{
				get
				{
					if (Type == "image_url")
						return image;
					else
						return null;
				}

				set
				{
					image = value;
					text = null;
					Type = "image_url";
				}
			}

			/// <summary>
			/// Creates an empty <see cref="ContentItem"/>
			/// </summary>
			public ContentItem()
			{

			}

			/// <summary>
			/// Creates a new <see cref="ContentItem"/> with the given text
			/// </summary>
			/// <param name="text">The text to send to the API</param>
			public ContentItem(string text)
			{
				this.Text = text;
				this.Type = "text";
			}

			/// <summary>
			/// Creates a new <see cref="ContentItem"/> with the given image
			/// </summary>
			/// <param name="image">The image to send to the API.  Consider using <see cref="ImageInput.FromFile(string, string)"/> to load an image from a local file, or <see cref="ImageInput.FromImageUrl(string, string)"/> to point to an image via URL.</param>
			public ContentItem(ImageInput image)
			{
				this.Image = image;
				this.Type = "image_url";
			}
		}

		/// <summary>
		/// Represents an image to send to the API in a chat message as part of GPT Vision.
		/// </summary>
		public class ImageInput
		{
			/// <summary>
			/// Either a URL of the image or the base64 encoded image data
			/// </summary>
			[JsonProperty("url")]
			public string Url { get; set; }

			/// <summary>
			/// By controlling the detail parameter, which has three options, low, high, or auto, you have control over how the model processes the image and generates its textual understanding.
			/// </summary>
			[JsonProperty("detail")]
			public string Detail { get; set; } = "auto";

			/// <summary>
			/// Instantiates a new ImageInput object with the given url
			/// </summary>
			/// <param name="url">A link to the image</param>
			/// <param name="detail">By controlling the detail parameter, which has three options, low, high, or auto, you have control over how the model processes the image and generates its textual understanding</param>
			public ImageInput(string url, string detail = "auto")
			{
				this.Url = url;
				this.Detail = detail;
			}

			/// <summary>
			/// Instantiates a new ImageInput object with the given image data bytes
			/// </summary>
			/// <param name="imageData">The image as bytes to be base64 encoded.  OpenAI currently supports PNG (.png), JPEG (.jpeg and .jpg), WEBP (.webp), and non-animated GIF (.gif)</param>
			/// <param name="detail">By controlling the detail parameter, which has three options, low, high, or auto, you have control over how the model processes the image and generates its textual understanding</param>
			public ImageInput(byte[] imageData, string detail = "auto")
			{
				this.Url = "data:image/jpeg;base64," + Convert.ToBase64String(imageData);
				this.Detail = detail;
			}

			/// <summary>
			/// Instantiates a new ImageInput object with the given image loaded from disk
			/// </summary>
			/// <param name="filePath">The local file path of the image.  OpenAI currently supports PNG (.png), JPEG (.jpeg and .jpg), WEBP (.webp), and non-animated GIF (.gif)</param>
			/// <param name="detail">By controlling the detail parameter, which has three options, low, high, or auto, you have control over how the model processes the image and generates its textual understanding</param>
			/// <returns></returns>
			public static ImageInput FromFile(string filePath, string detail = "auto")
			{
				return new ImageInput(System.IO.File.ReadAllBytes(filePath), detail);
			}

			/// <summary>
			/// Instantiates a new ImageInput object with the given image data bytes
			/// </summary>
			/// <param name="imageData">The image as bytes to be base64 encoded</param>
			/// <param name="detail">By controlling the detail parameter, which has three options, low, high, or auto, you have control over how the model processes the image and generates its textual understanding</param>
			/// <returns></returns>
			public static ImageInput FromImageBytes(byte[] imageData, string detail = "auto")
			{
				return new ImageInput(imageData, detail);
			}

			/// <summary>
			/// Instantiates a new ImageInput object with the given url
			/// </summary>
			/// <param name="url">A link to the image</param>
			/// <param name="detail">By controlling the detail parameter, which has three options, low, high, or auto, you have control over how the model processes the image and generates its textual understanding</param>
			/// <returns></returns>
			public static ImageInput FromImageUrl(string url, string detail = "auto")
			{
				return new ImageInput(url, detail);
			}

			/// <summary>
			/// By default, the model will use the auto setting which will look at the image input size and decide if it should use the low or high setting.
			/// </summary>
			public const string DetailAuto = "auto";
			/// <summary>
			/// low will disable the “high res” model. The model will receive a low-res 512px x 512px version of the image, and represent the image with a budget of 65 tokens. This allows the API to return faster responses and consume fewer input tokens for use cases that do not require high detail.
			/// </summary>
			public const string DetailLow = "low";
			/// <summary>
			/// high will enable “high res” mode, which first allows the model to see the low res image and then creates detailed crops of input images as 512px squares based on the input image size. Each of the detailed crops uses twice the token budget (65 tokens) for a total of 129 tokens.
			/// </summary>
			public const string DetailHigh = "high";
		}

		internal class ContentDataConverter : JsonConverter
		{
			public override bool CanConvert(Type objectType)
			{
				return true;
			}

			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
			{
				JToken token = JToken.Load(reader);
				if (token.Type == JTokenType.Object)
				{
					return token.ToObject<IList<ContentItem>>();
				}
				else if (token.Type == JTokenType.String)
				{
					List<ContentItem> content = new List<ContentItem>();
					content.Add(new ContentItem(token.ToObject<string>()));
					return content;
				}
				else
				{
					return null;
				}
			}

			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
			{
				serializer.Serialize(writer, value);
			}
		}

	}
}
