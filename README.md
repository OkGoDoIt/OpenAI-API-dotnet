# C#/.NET SDK for accessing the OpenAI GPT-3 API 

A simple C# .NET wrapper library to use with OpenAI's GPT-3 API.

## Requirements

This library is based on .NET Standard 2.0, so it should work across .NET Framework >=4.7.2 and .NET Core >= 3.0.  It should work across console apps, winforms, wpf, asp.net, etc (although I have not yet testing with asp.net).  It should work across Windows, Liux, and Mac, although I have only tested on Windows so far.

## Getting started

### Install from NuGet
(Coming soon)

### Authentication
There are 3 ways to provide your API keys, in order of precedence:
1.  Pass keys directly to `APIAuthentication(string key)` constructor
2.  Set environment vars for OPENAI_KEY and/or OPENAI_SECRET_KEY
3.  Include a config file in the local directory or in your user directory named `.openai` and containing one of both lines:
```
OPENAI_KEY=pk-aaaabbbbbccccddddd
OPENAI_SECRET_KEY=sk-aaaabbbbbccccddddd
```

### Completions
The Completion API is accessed via `OpenAIAPI.Completions`:

#### Non-streaming
`CreateCompletionAsync(CompletionRequest request)`

#### Streaming
Using the new C# 8.0 async interators:
`IAsyncEnumerable<CompletionResult> StreamCompletionEnumerableAsync(CompletionRequest request)`

Or if using .NET framework or C# <8.0:
`StreamCompletionAsync(CompletionRequest request, Action<int, CompletionResult> resultHandler)`

### Document Search
The Search API is accessed via `OpenAIAPI.Search`:

You can get all results as a dictionary using `GetSearchResultsAsync(SearchRequest request)`

You can get only the best match using `GetBestMatchAsync(...)`

nd if you only want the best match but still want to know the score, use `GetBestMatchWithScoreAsync(...)`

## Documentation

Every single class, method, and property has extensive XML documentation, so it should show up automatically in IntelliSense.  That combined with the official OpenAI documentation should be enough to get started.  Better documentation may come later.

## Examples

```csharp
var api = new OpenAI_API.OpenAIAPI(engine: Engine.Davinci);

var result = await api.Completions.CreateCompletionAsync("One Two Three Four Five Six Seven Eight Nine One Two Three Four Five Six Seven Eight", temperature: 0.1);
Console.WriteLine(result.ToString());
// should print "Nine"
```

```csharp
var api = new OpenAI_API.OpenAIAPI(engine: Engine.Curie);

var result = await api.Search.GetBestMatchAsync("Washington DC", "Canada", "China", "USA", "Spain");
Console.WriteLine(result);
// should print "USA"
```