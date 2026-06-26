using Microsoft.Extensions.DependencyInjection;
using Monnify.Authentication;

namespace Monnify.Http;

/// <summary>
/// Adds Monnify's standard pipeline to a typed client already registered via
/// <c>AddHttpClient&lt;TInterface, TImplementation&gt;</c>: bearer-token authentication on every
/// request, and, on net8.0, retry/circuit-breaker resilience. Resilience is net8.0-only because
/// Microsoft.Extensions.Http.Resilience's dependency chain doesn't ship real netstandard2.0
/// binaries despite its nuspec claiming support (see Monnify.csproj for details).
/// </summary>
internal static class MonnifyHttpClientBuilderExtensions
{
    public static IHttpClientBuilder AddMonnifyDefaults(this IHttpClientBuilder builder)
    {
        builder.AddHttpMessageHandler<MonnifyAuthHandler>();
#if MONNIFY_RESILIENCE
        builder.AddStandardResilienceHandler();
#endif
        return builder;
    }
}
