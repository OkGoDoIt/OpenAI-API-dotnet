// See https://aka.ms/new-console-template for more information

using OpenAI_API;
using OpenAI_API.Models;

var OPENAI_KEY = Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY", EnvironmentVariableTarget.User);

OpenAIAPI api = new OpenAIAPI(OPENAI_KEY);
var at = api.AudioTranscription;

var trans = await at.CreateTranscriptionAsync(file: @"c:\temp\SchoolEducationSystemEngland.mp3", model: Model.Whisper);

Console.WriteLine("-------------------  test ottenuto dall'audio -------------------------");
File.WriteAllText(@"c:\temp\SchoolEducationSystemEngland.txt", trans.Text);
Console.WriteLine(trans.Text);
Console.WriteLine("--------------------------- fine testo --------------------------------");