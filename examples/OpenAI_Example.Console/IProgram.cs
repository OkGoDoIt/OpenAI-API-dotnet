using System.Threading.Tasks;

namespace OpenAI_Example.ConsoleApp
{
    /// <summary>
    /// Defines an interface for different types of programs to run as an example.
    /// </summary>
    public interface IProgram
    {
        /// <summary>
        /// Runs a dedicated application example.
        /// </summary>
        /// <param name="apiKey">The Api key to authorize with OpenAI.</param>
        /// <returns>A task which can be awaited.</returns>
        Task RunAsync(string apiKey);
    }
}