#nullable enable
namespace Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OpenAI_API;

static class OpenAIApiExtensions
{
    /// <summary>Register <see cref="IOpenAI"/> for DI services. Read configuration from appsettings <code>"openAI": { "key": "", "org": "" }</code></summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddOpenAIService(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("openAI");
        if (!section.Exists()) return services;

        string? key = section["key"];
        if (key is null) return services;

        string? organisation = section["org"];
        return services.AddOpenAIService(new APIAuthentication(key, organisation));
    }

    public static IServiceCollection AddOpenAIService(this IServiceCollection services, APIAuthentication auth)
    {
        services.AddHttpClient<IOpenAI, OpenAIAPI>(client =>
            EndpointBase.ConfigureClient(client, auth));

        return services;
    }
}