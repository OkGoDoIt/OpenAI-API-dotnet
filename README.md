# C#/.NET SDK for accessing the OpenAI GPT-3 API 

A simple C# .NET wrapper library to use with OpenAI's GPT-3 API.  More context [on my blog](https://rogerpincombe.com/openai-dotnet-api).

## Examples

```csharp
var api = new OpenAI_API.OpenAIAPI(engine: Engine.Davinci);

var result = await api.Completions.CreateCompletionAsync("One Two Three One Two", temperature: 0.1);
Console.WriteLine(result.ToString());
// should print something starting with "Three"
```

```csharp
var api = new OpenAI_API.OpenAIAPI("sk-myapikeyhere"););

var result = await api.Search.GetBestMatchAsync("Washington DC", "Canada", "China", "USA", "Spain");
Console.WriteLine(result);
// should print "USA"
```

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
2.  Set environment var for OPENAI_KEY
3.  Include a config file in the local directory or in your user directory named `.openai` and containing the line:
```shell
OPENAI_KEY=sk-aaaabbbbbccccddddd
```

You use the `APIAuthentication` when you initialize the API as shown:
```csharp
// for example
OpenAIAPI api = new OpenAIAPI("sk-mykeyhere"); // shorthand
// or
OpenAIAPI api = new OpenAIAPI(new APIAuthentication("sk-secretkey")); // create object manually
// or
OpenAIAPI api = new OpenAIAPI(APIAuthentication LoadFromEnv()); // use env vars
// or
OpenAIAPI api = new OpenAIAPI(APIAuthentication LoadFromPath()); // use config file (can optionally specify where to look)
// or
OpenAIAPI api = new OpenAIAPI(); // uses default, env, or config file
```

### Completions
The Completion API is accessed via `OpenAIAPI.Completions`:

```csharp
CreateCompletionAsync(CompletionRequest request)

// for example
var result = await api.Completions.CreateCompletionAsync(new CompletionRequest("One Two Three One Two", temperature: 0.1));
// or
var result = await api.Completions.CreateCompletionAsync("One Two Three One Two", temperature: 0.1);
// or other convenience overloads
```
You can create your `CompletionRequest` ahead of time or use one of the helper overloads for convenience.  It returns a `CompletionResult` which is mostly metadata, so use its `.ToString()` method to get the text if all you want is the completion.

#### Streaming
Streaming allows you to get results are they are generated, which can help your application feel more responsive, especially on slow models like Davinci.

Using the new C# 8.0 async iterators:
```csharp
IAsyncEnumerable<CompletionResult> StreamCompletionEnumerableAsync(CompletionRequest request)

// for example
await foreach (var token in api.Completions.StreamCompletionEnumerableAsync(new CompletionRequest("My name is Roger and I am a principal software engineer at Salesforce.  This is my resume:", 200, 0.5, presencePenalty: 0.1, frequencyPenalty: 0.1)))
{
	Console.Write(token);
}
```

Or if using .NET framework or C# <8.0:
```csharp
StreamCompletionAsync(CompletionRequest request, Action<CompletionResult> resultHandler)

// for example
await api.Completions.StreamCompletionAsync(
	new CompletionRequest("My name is Roger and I am a principal software engineer at Salesforce.  This is my resume:", 200, 0.5, presencePenalty: 0.1, frequencyPenalty: 0.1),
	res => ResumeTextbox.Text += res.ToString());
```

### Document Search
The Search API is accessed via `OpenAIAPI.Search`:

You can get all results as a dictionary using
```csharp
GetSearchResultsAsync(SearchRequest request)

// for example
var request = new SearchRequest()
{
	Query = "Washington DC",
	Documents = new List<string> { "Canada", "China", "USA", "Spain" }
};
var result = await api.Search.GetSearchResultsAsync(request);
// result["USA"] == 294.22
// result["Spain"] == 73.81
```

The returned dictionary maps documents to scores.  You can create your `SearchRequest` ahead of time or use one of the helper overloads for convenience, such as
```csharp
GetSearchResultsAsync(string query, params string[] documents)

// for example
var result = await api.Search.GetSearchResultsAsync("Washington DC", "Canada", "China", "USA", "Spain");
```

You can get only the best match using
```csharp
GetBestMatchAsync(request)
```

And if you only want the best match but still want to know the score, use
```csharp
GetBestMatchWithScoreAsync(request)
```
Each of those methods has similar convenience overloads to specify the request inline.

### Finetuning
I don't yet have access to finetuning, but once I do I will add it to this SDK.  Subscribe to this repo if you want to be alerted.


## Documentation

Every single class, method, and property has extensive XML documentation, so it should show up automatically in IntelliSense.  That combined with the official OpenAI documentation should be enough to get started.  Feel free to ping me on Twitter [@OkGoDoIt](https://twitter.com/OkGoDoIt) if you have any questions.  Better documentation may come later.

## License
![CC-0 Public Domain](https://licensebuttons.net/p/zero/1.0/88x31.png)

This library is licensed CC-0, in the public domain.  You can use it for whatever you want, publicly or privately, without worrying about permission or licensing or whatever.  It's just a wrapper around the OpenAI API, so you still need to get access to OpenAI from them directly.  I am not affiliated with OpenAI and this library is not endorsed by them, I just have beta access and wanted to make a C# library to access it more easily.  Hopefully others find this useful as well.  Feel free to open a PR if there's anything you want to contribute.
