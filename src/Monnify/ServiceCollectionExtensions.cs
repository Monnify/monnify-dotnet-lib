using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Monnify.Authentication;
using Monnify.Banks;
using Monnify.Bills;
using Monnify.Collections;
using Monnify.Disbursements;
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

        // allowAutomaticRetry: false - ChargeAsync directly debits a card, same reasoning as
        // Disbursements/Bills below: an ambiguous failure must be resolved by querying the
        // transaction's status, not retried with the same card details. This is more conservative
        // than the rest of this client strictly needs (its other methods set up a payment
        // instrument rather than moving money directly), traded for keeping every collection
        // method, including card charges, on one client.
        services.AddHttpClient<IMonnifyCollectionsClient, MonnifyCollectionsClient>(ConfigureBaseAddress)
            .AddMonnifyDefaults(allowAutomaticRetry: false);

        // allowAutomaticRetry: false - an ambiguous failure on a transfer-initiating call must be
        // resolved by querying status with the same reference, not by blindly resending the same
        // request (which risks a double disbursement).
        services.AddHttpClient<IMonnifyDisbursementsClient, MonnifyDisbursementsClient>(ConfigureBaseAddress)
            .AddMonnifyDefaults(allowAutomaticRetry: false);

        // allowAutomaticRetry: false - VendAsync moves money, same reasoning as Disbursements above:
        // an ambiguous failure must be resolved via RequeryAsync with the same reference, not retried.
        services.AddHttpClient<IMonnifyBillsClient, MonnifyBillsClient>(ConfigureBaseAddress)
            .AddMonnifyDefaults(allowAutomaticRetry: false);

        services.AddTransient<MonnifyClient>();

        return services;
    }

    internal static void ConfigureBaseAddress(IServiceProvider serviceProvider, HttpClient httpClient)
    {
        var options = serviceProvider.GetRequiredService<IOptionsMonitor<MonnifyClientOptions>>().CurrentValue;
        httpClient.BaseAddress = options.EffectiveBaseUrl;
    }
}
