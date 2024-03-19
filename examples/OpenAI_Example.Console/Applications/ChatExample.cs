using OpenAI_API;
using System;
using System.Threading.Tasks;

namespace OpenAI_Example.ConsoleApp.Applications
{
    internal sealed class ChatExample : IProgram
    {
        public async Task RunAsync(string apiKey)
        {
            Console.WriteLine($"Running the {GetType()}....");
            Console.WriteLine("Please ask a question:");
            var api = new OpenAIAPI(apiKey);

            var str = Console.ReadLine();
            var result = await api.Chat.CreateChatCompletionAsync(str);

            var reply = result.Choices[0].Message;
            Console.WriteLine($"{reply.Role}: {reply.Content.Trim()}");
        }
    }
}