// See https://aka.ms/new-console-template for more information

using OpenAI_API;
using OpenAI_API.Models;

OpenAIAPI api = new OpenAIAPI("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");
var at = api.AudioTranscription;

var trans = await at.CreateTranscriptionAsync(file: @"c:\temp\audio.mp3", model: Model.Whisper);

Console.WriteLine("-------------------  test ottenuto dall'audio -------------------------");
Console.WriteLine(trans.Text);
Console.WriteLine("--------------------------- fine testo --------------------------------");