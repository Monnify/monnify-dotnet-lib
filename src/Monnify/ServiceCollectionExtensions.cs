using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Monnify.Authentication;
using Monnify.Banks;
using Monnify.Http;
using Monnify.Verification;

namespace Monnify;

/// <summary>
/// Registers every Monnify SDK service into an <see cref="IServiceCollection"/>: authentication
/// infrastructure plus each typed client (added via
/// <c>AddHttpClient&lt;TInterface, TImplementation&gt;(...).AddMonnifyDefaults()</c>, which attaches
/// bearer-token auth and, on net8.0, resilience).
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Monnify SDK services. Call this once, typically in <c>Program.cs</c>, then inject the
    /// individual typed clients (or the <see cref="MonnifyClient"/> facade) wherever you need them.
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

        services.AddHttpClient<IMonnifyBanksClient, MonnifyBanksClient>(ConfigureBaseAddress).AddMonnifyDefaults();
        services.AddHttpClient<IMonnifyVerificationClient, MonnifyVerificationClient>(ConfigureBaseAddress).AddMonnifyDefaults();

        services.AddTransient<MonnifyClient>();

        return services;
    }

    internal static void ConfigureBaseAddress(IServiceProvider serviceProvider, HttpClient httpClient)
    {
        var options = serviceProvider.GetRequiredService<IOptionsMonitor<MonnifyClientOptions>>().CurrentValue;
        httpClient.BaseAddress = options.EffectiveBaseUrl;
    }
}
