using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OpenAI_Example.ConsoleApp
{
    internal sealed class Program
    {
        private static async Task Main()
        {
            Console.WriteLine("Welcome to the OpenAI Example program.");
            Console.WriteLine("Please provide a valid OpenAI Api Key:");
            var apiKey = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("Cannot authorize with OpenAI when no valid API Key is provided.");
            }

            Console.WriteLine("What do you want to do?");

            Type[] types = GetExamples();
            _ = int.TryParse(Console.ReadLine(), out var programToRun);

            var instance = (IProgram?)Activator.CreateInstance(types[programToRun]);
            if (instance is not null)
            {
                await instance.RunAsync(apiKey);
            }

            Console.WriteLine("Thank you for using the OpenAI Example program.");
            Console.WriteLine("Exiting now....");
        }

        private static Type[] GetExamples()
        {
            var type = typeof(IProgram);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                                               .SelectMany(s => s.GetTypes())
                                               .Where(type.IsAssignableFrom)
                                               .ToArray();

            for (var i = 0; i < types.Length; i++)
            {
                var option = types[i];
                if (option.Name == nameof(IProgram)) // Don't include the IProgram type.
                {
                    continue;
                }
                Console.WriteLine($"\t{i}){option.Name}");
            }

            return types;
        }
    }
}