# C#/.NET SDK for accessing the OpenAI GPT-3 API 

A simple C# .NET wrapper library to use with OpenAI's GPT-3 API.  More context [on my blog](https://rogerpincombe.com/openai-dotnet-api).

## Quick Example

```csharp
var api = new OpenAI_API.OpenAIAPI("YOUR_API_KEY");
var result = await api.Completions.GetCompletion("One Two Three One Two");
Console.WriteLine(result);
// should print something starting with "Three"
```

## Readme

 * [Status](#Status)
 * [Requirements](#requirements)
 * [Installation](#install-from-nuget)
 * [Authentication](#authentication)
 * [Completions API](#completions)
	* [Streaming completion results](#streaming)
 * [Embeddings API](#embeddings)
 * [Files API](#files-for-fine-tuning)
 * [Additonal Documentation](#documentation)
 * [License](#license)

## Status
Updated to work with the current API as of February 16, 2023.  Fixed several minor bugs.

Now also should work with the Azure OpenAI Service, although this is untested. See [Azure](#azure) section for further details.

Thank you [@GotMike](https://github.com/gotmike), [@ncface](https://github.com/ncface), [@KeithHenry](https://github.com/KeithHenry), [@gmilano](https://github.com/gmilano), [@metjuperry](https://github.com/metjuperry), and [@Alexei000](https://github.com/Alexei000) for your contributions!

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



### Completions
The Completion API is accessed via `OpenAIAPI.Completions`:

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

### Azure

For using the Azure OpenAI Service, you need to specify the name of your Azure OpenAI resource as well as your model deployment id.  Additionally you may specify the Api version which defaults to `2022-12-01`.

_I do not have access to the Microsoft Azure OpenAI service, so I am unable to test this functionality.  If you have access and can test, please submit an issue describing your results.  A PR with integration tests would also be greatly appreciated.  Specifically, it is unclear to me that specifying models works the same way with Azure._

Refer the [Azure OpenAI documentation](https://learn.microsoft.com/en-us/azure/cognitive-services/openai/reference) for further information.

Configuration should look something like this for the Azure service:

```csharp
OpenAIAPI api = OpenAIAPI.ForAzure("YourResourceName", "deploymentId", "api-key");
```

You may then use the `api` object like normal.  You may also specify the `APIAuthentication` is any of the other ways listed in the [Authentication](#authentication) section above.  Currently this library only supports the api-key flow, not the AD-Flow.

## Documentation

Every single class, method, and property has extensive XML documentation, so it should show up automatically in IntelliSense.  That combined with the official OpenAI documentation should be enough to get started.  Feel free to open an issue here if you have any questions.  Better documentation may come later.

## License
![CC-0 Public Domain](https://licensebuttons.net/p/zero/1.0/88x31.png)

This library is licensed CC-0, in the public domain.  You can use it for whatever you want, publicly or privately, without worrying about permission or licensing or whatever.  It's just a wrapper around the OpenAI API, so you still need to get access to OpenAI from them directly.  I am not affiliated with OpenAI and this library is not endorsed by them, I just have beta access and wanted to make a C# library to access it more easily.  Hopefully others find this useful as well.  Feel free to open a PR if there's anything you want to contribute.
