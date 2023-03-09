using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenAI_API.Models
{
	/// <summary>
	/// An interface for <see cref="ModelsEndpoint"/>, for ease of mock testing, etc
	/// </summary>
	public interface IModelsEndpoint
    {
        /// <summary>
        /// Get details about a particular Model from the API, specifically properties such as <see cref="Model.OwnedBy"/> and permissions.
        /// </summary>
        /// <param name="id">The id/name of the model to get more details about</param>
        /// <returns>Asynchronously returns the <see cref="Model"/> with all available properties</returns>
        Task<Model> RetrieveModelDetailsAsync(string id);

        /// <summary>
        /// Get details about a particular Model from the API, specifically properties such as <see cref="Model.OwnedBy"/> and permissions.
        /// </summary>
        /// <param name="id">The id/name of the model to get more details about</param>
        /// <param name="auth">Obsolete: IGNORED</param>
        /// <returns>Asynchronously returns the <see cref="Model"/> with all available properties</returns>
        Task<Model> RetrieveModelDetailsAsync(string id, APIAuthentication auth = null);

        /// <summary>
        /// List all models via the API
        /// </summary>
        /// <returns>Asynchronously returns the list of all <see cref="Model"/>s</returns>
        Task<List<Model>> GetModelsAsync();
    }
}