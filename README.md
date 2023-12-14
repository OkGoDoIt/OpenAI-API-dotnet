# C#/.NET SDK for accessing the OpenAI APIs, including GPT-3.5/4, GPT-3.5/4-Turbo, and DALL-E 2/3

A simple C# .NET wrapper library to use with OpenAI's API.  More context [on my blog](https://rogerpincombe.com/openai-dotnet-api).  This is an unofficial wrapper library around the OpenAI API.  I am not affiliated with OpenAI and this library is not endorsed or supported by them.

## Quick Example

```csharp
var api = new OpenAI_API.OpenAIAPI("YOUR_API_KEY");
var result = await api.Chat.CreateChatCompletionAsync("Hello!");
Console.WriteLine(result);
// should print something like "Hi! How can I help you?"
```

## Readme

 * [Status](#Status)
 * [Requirements](#requirements)
 * [Installation](#install-from-nuget)
 * [Authentication](#authentication)
 * [Chat API](#chatapi)
	* [Conversations](#chat-conversations)
	* [Streaming Results](#chat-streaming)
	* [GPT Vision](#gpt-vision)
	* [Chat Endpoint](#chat-endpoint-requests)
	* [Conversation History Context Length Management](#Conversation-History-Context-Length-Management)
	* [JSON Mode](#json-mode)
 * [Completions API](#completions)
	* [Streaming completion results](#streaming)
 * [Audio](#audio)
	* [Text to Speech](#text-to-speech-tts)
	* [Transcribe Audio to Text](#transcription-speech-to-text)
	* [Translate Audio to English Text](#translations-non-english-speech-to-english-text)
 * [Embeddings API](#embeddings)
 * [Moderation API](#moderation)
 * [Files API](#files-for-fine-tuning)
 * [Image APIs (DALL-E)](#images)
	* [DALLE-E 3](#dall-e-3)
 * [Azure](#azure)
 * [Additional Documentation](#documentation)
 * [License](#license)

## Status
[![OpenAI](https://badgen.net/nuget/v/OpenAI)](https://www.nuget.org/packages/OpenAI/)

Adds updated models as of December 13, 2023, including the new [GPT-4 Vision](#gpt-vision), GPT-4 Turbo, and [DALL-E 3](#dall-e-3). Adds [text-to-speech](#text-to-speech-tts) as well as [audio transcriptions](#transcription-speech-to-text) and [translations](#translations-non-english-speech-to-english-text) (Whisper).  Adds [json result format](#json-mode). Fixes chat result streaming bug.
Support for assistants and other new features shown at OpenAI DevDay will be coming soon, but are not yet implemented.

## Requirements

This library is based on .NET Standard 2.0, so it should work across .NET Framework >=4.7.2 and .NET Core >= 3.0.  It should work across console apps, winforms, wpf, asp.net, etc (although I have not yet tested with asp.net).  It should work across Windows, Linux, and Mac, although I have only tested on Windows so far.

## Getting started

### Install from NuGet

Install package [`OpenAI` from Nuget](https://www.nuget.org/packages/OpenAI/).  Here's how via commandline:
```powershell
Install-Package OpenAI
```

### Authentication
There are 3 ways to provide your API keys, in order of precedence:
1.  Pass keys directly to `APIAuthentication(string key)` constructor
2.  Set environment var for OPENAI_API_KEY (or OPENAI_KEY for backwards compatibility)
3.  Include a config file in the local directory or in your user directory named `.openai` and containing the line:
```shell
OPENAI_API_KEY=sk-aaaabbbbbccccddddd
```

You use the `APIAuthentication` when you initialize the API as shown:
```csharp
// for example
OpenAIAPI api = new OpenAIAPI("YOUR_API_KEY"); // shorthand
// or
OpenAIAPI api = new OpenAIAPI(new APIAuthentication("YOUR_API_KEY")); // create object manually
// or
OpenAIAPI api = new OpenAIAPI(APIAuthentication LoadFromEnv()); // use env vars
// or
OpenAIAPI api = new OpenAIAPI(APIAuthentication LoadFromPath()); // use config file (can optionally specify where to look)
// or
OpenAIAPI api = new OpenAIAPI(); // uses default, env, or config file
```

You may optionally include an openAIOrganization (OPENAI_ORGANIZATION in env or config file) specifying which organization is used for an API request. Usage from these API requests will count against the specified organization's subscription quota.  Organization IDs can be found on your [Organization settings](https://beta.openai.com/account/org-settings) page.
```csharp
// for example
OpenAIAPI api = new OpenAIAPI(new APIAuthentication("YOUR_API_KEY","org-yourOrgHere"));
```

### Chat API
The Chat API is accessed via `OpenAIAPI.Chat`.  There are two ways to use the Chat Endpoint, either via simplified conversations or with the full Request/Response methods.

#### Chat Conversations
The Conversation Class allows you to easily interact with ChatGPT by adding messages to a chat and asking ChatGPT to reply.
```csharp
var chat = api.Chat.CreateConversation();
chat.Model = Model.GPT4_Turbo;
chat.RequestParameters.Temperature = 0;

/// give instruction as System
chat.AppendSystemMessage("You are a teacher who helps children understand if things are animals or not.  If the user tells you an animal, you say \"yes\".  If the user tells you something that is not an animal, you say \"no\".  You only ever respond with \"yes\" or \"no\".  You do not say anything else.");

// give a few examples as user and assistant
chat.AppendUserInput("Is this an animal? Cat");
chat.AppendExampleChatbotOutput("Yes");
chat.AppendUserInput("Is this an animal? House");
chat.AppendExampleChatbotOutput("No");

// now let's ask it a question
chat.AppendUserInput("Is this an animal? Dog");
// and get the response
string response = await chat.GetResponseFromChatbotAsync();
Console.WriteLine(response); // "Yes"

// and continue the conversation by asking another
chat.AppendUserInput("Is this an animal? Chair");
// and get another response
response = await chat.GetResponseFromChatbotAsync();
Console.WriteLine(response); // "No"

// the entire chat history is available in chat.Messages
foreach (ChatMessage msg in chat.Messages)
{
	Console.WriteLine($"{msg.Role}: {msg.Content}");
}
```

#### Chat Streaming

Streaming allows you to get results are they are generated, which can help your application feel more responsive.

Using the new C# 8.0 async iterators:
```csharp
var chat = api.Chat.CreateConversation();
chat.AppendUserInput("How to make a hamburger?");

await foreach (var res in chat.StreamResponseEnumerableFromChatbotAsync())
{
	Console.Write(res);
}
```

Or if using classic .NET Framework or C# <8.0:
```csharp
var chat = api.Chat.CreateConversation();
chat.AppendUserInput("How to make a hamburger?");

await chat.StreamResponseFromChatbotAsync(res =>
{
	Console.Write(res);
});
```

#### GPT Vision

You can send images to the chat to use the new GPT-4 Vision model.  This only works with the `Model.GPT4_Vision` model.  Please see https://platform.openai.com/docs/guides/vision for more information and limitations.

```csharp
// the simplest form
var result = await api.Chat.CreateChatCompletionAsync("What is the primary non-white color in this logo?", ImageInput.FromFile("path/to/logo.png"));

// or in a conversation
var chat = api.Chat.CreateConversation();
chat.Model = Model.GPT4_Vision;
chat.AppendSystemMessage("You are a graphic design assistant who helps identify colors.");
chat.AppendUserInput("What are the primary non-white colors in this logo?", ImageInput.FromFile("path/to/logo.png"));
string response = await chat.GetResponseFromChatbotAsync();
Console.WriteLine(response); // "Blue and purple"
chat.AppendUserInput("What are the primary non-white colors in this logo?", ImageInput.FromImageUrl("https://rogerpincombe.com/templates/rp/center-aligned-no-shadow-small.png"));
response = await chat.GetResponseFromChatbotAsync();
Console.WriteLine(response); // "Blue, red, and yellow"

// or when manually creating the ChatMessage
messageWithImage = new ChatMessage(ChatMessageRole.User, "What colors do these logos have in common?");
messageWithImage.images.Add(ImageInput.FromFile("path/to/logo.png"));
messageWithImage.images.Add(ImageInput.FromImageUrl("https://rogerpincombe.com/templates/rp/center-aligned-no-shadow-small.png"));

// you can specify multiple images at once
chat.AppendUserInput("What colors do these logos have in common?", ImageInput.FromFile("path/to/logo.png"), ImageInput.FromImageUrl("https://rogerpincombe.com/templates/rp/center-aligned-no-shadow-small.png"));
```


#### Conversation History Context Length Management
If the chat conversation history gets too long, it may not fit into the context length of the model.  By default, the earliest non-system message(s) will be removed from the chat history and the API call will be retried.  You may disable this by setting `chat.AutoTruncateOnContextLengthExceeded = false`, or you can override the truncation algorithm like this:

```csharp
chat.OnTruncationNeeded += (sender, args) =>
{
	// args is a List<ChatMessage> with the current chat history.  Remove or edit as nessisary.
	// replace this with more sophisticated logic for your use-case, such as summarizing the chat history
	for (int i = 0; i < args.Count; i++)
	{
		if (args[i].Role != ChatMessageRole.System)
		{
			args.RemoveAt(i);
			return;
		}
	}
};
```

You may also wish to use a new model with a larger context length.  You can do this by setting `chat.Model = Model.GPT4_Turbo` or `chat.Model = Model.ChatGPTTurbo_16k`, etc.

You can see token usage via `chat.MostRecentApiResult.Usage.PromptTokens` and related properties. 

#### Chat Endpoint Requests
You can access full control of the Chat API by using the `OpenAIAPI.Chat.CreateChatCompletionAsync()` and related methods.

```csharp
async Task<ChatResult> CreateChatCompletionAsync(ChatRequest request);

// for example
var result = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
	{
		Model = Model.ChatGPTTurbo,
		Temperature = 0.1,
		MaxTokens = 50,
		Messages = new ChatMessage[] {
			new ChatMessage(ChatMessageRole.User, "Hello!")
		}
	})
// or
var result = api.Chat.CreateChatCompletionAsync("Hello!");

var reply = results.Choices[0].Message;
Console.WriteLine($"{reply.Role}: {reply.Content.Trim()}");
// or
Console.WriteLine(results);
```

It returns a `ChatResult` which is mostly metadata, so use its `.ToString()` method to get the text if all you want is assistant's reply text.

There's also an async streaming API which works similarly to the [Completions endpoint streaming results](#streaming). 

#### JSON Mode

With the new `Model.GPT4_Turbo` or `gpt-3.5-turbo-1106` models, you can set the `ChatRequest.ResponseFormat` to `ChatRequest.ResponseFormats.JsonObject` to enable JSON mode.
When JSON mode is enabled, the model is constrained to only generate strings that parse into valid JSON object.
See https://platform.openai.com/docs/guides/text-generation/json-mode for more details.

```csharp
ChatRequest chatRequest = new ChatRequest()
{
	Model = model,
	Temperature = 0.0,
	MaxTokens = 500,
	ResponseFormat = ChatRequest.ResponseFormats.JsonObject,
	Messages = new ChatMessage[] {
		new ChatMessage(ChatMessageRole.System, "You are a helpful assistant designed to output JSON."),
		new ChatMessage(ChatMessageRole.User, "Who won the world series in 2020?  Return JSON of a 'wins' dictionary with the year as the numeric key and the winning team as the string value.")
	}
};

var results = await api.Chat.CreateChatCompletionAsync(chatRequest);
Console.WriteLine(results);
/* prints:
{
  "wins": {
	2020: "Los Angeles Dodgers"
  }
}
*/
```



### Completions API
Completions are considered legacy by OpenAI.  The Completion API is accessed via `OpenAIAPI.Completions`:

```csharp
async Task<CompletionResult> CreateCompletionAsync(CompletionRequest request);

// for example
var result = await api.Completions.CreateCompletionAsync(new CompletionRequest("One Two Three One Two", model: Model.CurieText, temperature: 0.1));
// or
var result = await api.Completions.CreateCompletionAsync("One Two Three One Two", temperature: 0.1);
// or other convenience overloads
```
You can create your `CompletionRequest` ahead of time or use one of the helper overloads for convenience.  It returns a `CompletionResult` which is mostly metadata, so use its `.ToString()` method to get the text if all you want is the completion.

#### Streaming
Streaming allows you to get results are they are generated, which can help your application feel more responsive, especially on slow models like Davinci.

Using the new C# 8.0 async iterators:
```csharp
IAsyncEnumerable<CompletionResult> StreamCompletionEnumerableAsync(CompletionRequest request);

// for example
await foreach (var token in api.Completions.StreamCompletionEnumerableAsync(new CompletionRequest("My name is Roger and I am a principal software engineer at Salesforce.  This is my resume:", Model.DavinciText, 200, 0.5, presencePenalty: 0.1, frequencyPenalty: 0.1)))
{
	Console.Write(token);
}
```

Or if using classic .NET framework or C# <8.0:
```csharp
async Task StreamCompletionAsync(CompletionRequest request, Action<CompletionResult> resultHandler);

// for example
await api.Completions.StreamCompletionAsync(
	new CompletionRequest("My name is Roger and I am a principal software engineer at Salesforce.  This is my resume:", Model.DavinciText, 200, 0.5, presencePenalty: 0.1, frequencyPenalty: 0.1),
	res => ResumeTextbox.Text += res.ToString());
```

### Audio
The Audio API's are Text to Speech, Transcription (speech to text), and Translation (non-English speech to English text).

#### Text to Speech (TTS)
The TTS API is accessed via `OpenAIAPI.TextToSpeech`:

```csharp
await api.TextToSpeech.SaveSpeechToFileAsync("Hello, brave new world!  This is a test.", outputPath);
// You can open it in the defaul audio player like this:
Process.Start(outputPath);
```

You can also specify all of the request parameters with a `TextToSpeechRequest` object:

```csharp
var request = new TextToSpeechRequest()
{
	Input = "Hello, brave new world!  This is a test.",
	ResponseFormat = ResponseFormats.AAC,
	Model = Model.TTS_HD,
	Voice = Voices.Nova,
	Speed = 0.9
};
await api.TextToSpeech.SaveSpeechToFileAsync(request, "test.aac");
```

Instead of saving to a file, you can get audio byte stream with `api.TextToSpeech.GetSpeechAsStreamAsync(request)`:

```csharp
using (Stream result = await api.TextToSpeech.GetSpeechAsStreamAsync("Hello, brave new world!", Voices.Fable))
using (StreamReader reader = new StreamReader(result))
{
	// do something with the audio stream here
}
```

#### Transcription (Speech to Text)

The Audio Transcription API allows you to generate text from audio, in any of the supported languages.  It is accessed via `OpenAIAPI.Transcriptions`:

```csharp
string resultText = await api.Transcriptions.GetTextAsync("path/to/file.mp3");
```

You can ask for verbose results, which will give you segment and token-level information, as well as the standard OpenAI metadata such as processing time:

```csharp
AudioResultVerbose result = await api.Transcriptions.GetWithDetailsAsync("path/to/file.m4a");
Console.WriteLine(result.ProcessingTime.TotalMilliseconds); // 496ms
Console.WriteLine(result.text); // "Hello, this is a test of the transcription function."
Console.WriteLine(result.language); // "english"
Console.WriteLine(result.segments[0].no_speech_prob); // 0.03712
// etc
```

You can also ask for results in SRT or VTT format, which is useful for generating subtitles for videos:

```csharp
string result = await api.Transcriptions.GetAsFormatAsync("path/to/file.m4a", AudioRequest.ResponseFormats.SRT);
```

Additional parameters such as temperature, prompt, language, etc can be specified either per-request or as a default:

```csharp
// inline
result = await api.Transcriptions.GetTextAsync("conversation.mp3", "en", "This is a transcript of a conversation between a medical doctor and her patient: ", 0.3);

// set defaults
api.Transcriptions.DefaultTranscriptionRequestArgs.Language = "en";
```

Instead of providing a local file on disk, you can provide a stream of audio bytes.  This can be useful for streaming audio from the microphone or another source without having to first write to disk.  Please not you must specify a filename, which does not have to exist, but which must have an accurate extension for the type of audio that you are sending.  OpenAI uses the filename extension to determine what format your audio stream is in.

```csharp
using (var audioStream = File.OpenRead("path-here.mp3"))
{
	return await api.Transcriptions.GetTextAsync(audioStream, "file.mp3");
}
```

#### Translations (Non-English Speech to English Text)

Translations allow you to transcribe text from any of the supported languages to English.  OpenAI does not support translating into any other language, only English.  It is accessed via `OpenAIAPI.Translations`.
It supports all of the same functionality as the Transcriptions.

```csharp
string result = await api.Translations.GetTextAsync("chinese-example.m4a");
```

### Embeddings
The Embedding API is accessed via `OpenAIAPI.Embeddings`:

```csharp
async Task<EmbeddingResult> CreateEmbeddingAsync(EmbeddingRequest request);

// for example
var result = await api.Embeddings.CreateEmbeddingAsync(new EmbeddingRequest("A test text for embedding", model: Model.AdaTextEmbedding));
// or
var result = await api.Embeddings.CreateEmbeddingAsync("A test text for embedding");
```

The embedding result contains a lot of metadata, the actual vector of floats is in result.Data[].Embedding.

For simplicity, you can directly ask for the vector of floats and disgard the extra metadata with `api.Embeddings.GetEmbeddingsAsync("test text here")`


### Moderation
The Moderation API is accessed via `OpenAIAPI.Moderation`:

```csharp
async Task<ModerationResult> CreateEmbeddingAsync(ModerationRequest request);

// for example
var result = await api.Moderation.CallModerationAsync(new ModerationRequest("A test text for moderating", Model.TextModerationLatest));
// or
var result = await api.Moderation.CallModerationAsync("A test text for moderating");

Console.WriteLine(result.results[0].MainContentFlag);
```

The results are in `.results[0]` and have nice helper methods like `FlaggedCategories` and `MainContentFlag`.


### Files (for fine-tuning)
The Files API endpoint is accessed via `OpenAIAPI.Files`:

```csharp
// uploading
async Task<File> UploadFileAsync(string filePath, string purpose = "fine-tune");

// for example
var response = await api.Files.UploadFileAsync("fine-tuning-data.jsonl");
Console.Write(response.Id); //the id of the uploaded file

// listing
async Task<List<File>> GetFilesAsync();

// for example
var response = await api.Files.GetFilesAsync();
foreach (var file in response)
{
	Console.WriteLine(file.Name);
}
```

There are also methods to get file contents, delete a file, etc.

The fine-tuning endpoint itself has not yet been implemented, but will be added soon.

### Images
The DALL-E Image Generation API is accessed via `OpenAIAPI.ImageGenerations`:

```csharp
async Task<ImageResult> CreateImageAsync(ImageGenerationRequest request);

// for example
var result = await api.ImageGenerations.CreateImageAsync(new ImageGenerationRequest("A drawing of a computer writing a test", 1, ImageSize._512));
// or
var result = await api.ImageGenerations.CreateImageAsync("A drawing of a computer writing a test");

Console.WriteLine(result.Data[0].Url);
```

The image result contains a URL for an online image or a base64-encoded image, depending on the ImageGenerationRequest.ResponseFormat (url is the default).

#### DALL-E 3

Use DALL-E 3 like this:

```csharp
async Task<ImageResult> CreateImageAsync(ImageGenerationRequest request);

// for example
var result = await api.ImageGenerations.CreateImageAsync(new ImageGenerationRequest("A drawing of a computer writing a test", OpenAI_API.Models.Model.DALLE3, ImageSize._1024x1792, "hd"));
// or
var result = await api.ImageGenerations.CreateImageAsync("A drawing of a computer writing a test", OpenAI_API.Models.Model.DALLE3);

Console.WriteLine(result.Data[0].Url);
```

## Azure

For using the Azure OpenAI Service, you need to specify the name of your Azure OpenAI resource as well as your model deployment id.

_I do not have access to the Microsoft Azure OpenAI service, so I am unable to test this functionality.  If you have access and can test, please submit an issue describing your results.  A PR with integration tests would also be greatly appreciated.  Specifically, it is unclear to me that specifying models works the same way with Azure._

Refer the [Azure OpenAI documentation](https://learn.microsoft.com/en-us/azure/cognitive-services/openai/reference) and [detailed screenshots in #64](https://github.com/OkGoDoIt/OpenAI-API-dotnet/issues/64#issuecomment-1479276020) for further information.

Configuration should look something like this for the Azure service:

```csharp
OpenAIAPI api = OpenAIAPI.ForAzure("YourResourceName", "deploymentId", "api-key");
api.ApiVersion = "2023-03-15-preview"; // needed to access chat endpoint on Azure
```

You may then use the `api` object like normal.  You may also specify the `APIAuthentication` is any of the other ways listed in the [Authentication](#authentication) section above.  Currently this library only supports the api-key flow, not the AD-Flow.

As of April 2, 2023, you need to manually select api version `2023-03-15-preview` as shown above to access the chat endpoint on Azure.  Once this is out of preview I will update the default.

## IHttpClientFactory
You may specify an `IHttpClientFactory` to be used for HTTP requests, which allows for tweaking http request properties, connection pooling, and mocking.  Details in [#103](https://github.com/OkGoDoIt/OpenAI-API-dotnet/pull/103).

```csharp
OpenAIAPI api = new OpenAIAPI();
api.HttpClientFactory = myIHttpClientFactoryObject;
```

## Documentation

Every single class, method, and property has extensive XML documentation, so it should show up automatically in IntelliSense.  That combined with the official OpenAI documentation should be enough to get started.  Feel free to open an issue here if you have any questions.  Better documentation may come later.

## License

CC-0 Public Domain

This library is licensed CC-0, in the public domain.  You can use it for whatever you want, publicly or privately, without worrying about permission or licensing or whatever.  It's just a wrapper around the OpenAI API, so you still need to get access to OpenAI from them directly.  I am not affiliated with OpenAI and this library is not endorsed by them, I just have beta access and wanted to make a C# library to access it more easily.  Hopefully others find this useful as well.  Feel free to open a PR if there's anything you want to contribute.
