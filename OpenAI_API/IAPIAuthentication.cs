using System.Threading.Tasks;

namespace OpenAI_API
{
    /// <summary>
    /// Represents authentication to the OpenAPI API endpoint
    /// </summary>
    public interface IAPIAuthentication
    {
        /// <summary>
        /// The API key, required to access the API endpoint.
        /// </summary>
        string ApiKey { get; set; }

        /// <summary>
        /// The Organization ID to count API requests against.  This can be found at https://beta.openai.com/account/org-settings.
        /// </summary>
        string OpenAIOrganization { get; set; }

        /// <summary>
        /// Tests the api key against the OpenAI API, to ensure it is valid.  This hits the models endpoint so should not be charged for usage.
        /// </summary>
        /// <returns><see langword="true"/> if the api key is valid, or <see langword="false"/> if empty or not accepted by the OpenAI API.</returns>
        Task<bool> ValidateAPIKey();
    }
}