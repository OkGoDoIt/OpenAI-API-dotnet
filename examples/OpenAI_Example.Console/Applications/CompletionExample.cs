using OpenAI_API;
using System;
using System.Threading.Tasks;

namespace OpenAI_Example.ConsoleApp.Applications
{
    internal class CompletionExample : IProgram
    {
        public async Task RunAsync(string apiKey)
        {
            Console.WriteLine($"Running the {GetType()}....");
            var api = new OpenAIAPI(apiKey);
            var result = await api.Completions.GetCompletion("One Two Three One Two");
            Console.WriteLine(result);
        }
    }
}