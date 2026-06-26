using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Monnify.Authentication;

namespace Monnify;

/// <summary>
/// Registers the Monnify SDK's authentication infrastructure into an <see cref="IServiceCollection"/>.
/// Typed clients added in later phases call <c>AddHttpClient&lt;TInterface, TImplementation&gt;</c>
/// with <c>.AddHttpMessageHandler&lt;MonnifyAuthHandler&gt;()</c> to pick up authentication automatically.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Monnify SDK services. Call this once, typically in <c>Program.cs</c>, then inject the
    /// individual typed clients (or the <c>MonnifyClient</c> facade, once a client exists)
    /// wherever you need them.
    /// </summary>
    public static IServiceCollection AddMonnify(this IServiceCollection services, Action<MonnifyClientOptions> configureOptions)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configureOptions is null)
        {
            throw new ArgumentNullException(nameof(configureOptions));
        }

        services.AddOptions<MonnifyClientOptions>()
            .Configure(configureOptions)
            .Validate(o => !string.IsNullOrWhiteSpace(o.ApiKey), "Monnify ApiKey must be configured.")
            .Validate(o => !string.IsNullOrWhiteSpace(o.SecretKey), "Monnify SecretKey must be configured.")
            .Validate(o => o.EffectiveBaseUrl.Scheme == Uri.UriSchemeHttps, "Monnify BaseUrl must use HTTPS.");

        services.AddHttpClient(MonnifyTokenProvider.AuthHttpClientName, ConfigureBaseAddress);

        services.TryAddSingleton<IMonnifyTokenProvider, MonnifyTokenProvider>();
        services.AddTransient<MonnifyAuthHandler>();

        return services;
    }

    internal static void ConfigureBaseAddress(IServiceProvider serviceProvider, HttpClient httpClient)
    {
        var options = serviceProvider.GetRequiredService<IOptionsMonitor<MonnifyClientOptions>>().CurrentValue;
        httpClient.BaseAddress = options.EffectiveBaseUrl;
    }
}
