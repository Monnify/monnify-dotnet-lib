using Microsoft.Extensions.DependencyInjection;
using Monnify.Authentication;

namespace Monnify.Http;

/// <summary>
/// Adds Monnify's standard pipeline to a typed client already registered via
/// <c>AddHttpClient&lt;TInterface, TImplementation&gt;</c>: bearer-token authentication on every
/// request, and, on net8.0, timeout/circuit-breaker resilience. Resilience is net8.0-only because
/// Microsoft.Extensions.Http.Resilience's dependency chain doesn't ship real netstandard2.0
/// binaries despite its nuspec claiming support (see Monnify.csproj for details).
/// </summary>
internal static class MonnifyHttpClientBuilderExtensions
{
    /// <param name="builder">The typed client builder to attach Monnify's pipeline to.</param>
    /// <param name="allowAutomaticRetry">
    /// Whether a transient failure (timeout, 5xx, 429) is automatically retried. Pass
    /// <see langword="false"/> for a client whose calls can move money or otherwise aren't safe to
    /// resend blindly (e.g. Disbursements) — an ambiguous failure must be resolved by querying
    /// status with the same reference, not by retrying the same request. Timeout and
    /// circuit-breaker protection still apply either way.
    /// </param>
    public static IHttpClientBuilder AddMonnifyDefaults(this IHttpClientBuilder builder, bool allowAutomaticRetry = true)
    {
        builder.AddHttpMessageHandler<MonnifyAuthHandler>();
#if MONNIFY_RESILIENCE
        if (allowAutomaticRetry)
        {
            builder.AddStandardResilienceHandler();
        }
        else
        {
            // MaxRetryAttempts must be >= 1, so disabling retry means making ShouldHandle never
            // match instead - timeout and circuit-breaker stages are unaffected.
            builder.AddStandardResilienceHandler(options => options.Retry.ShouldHandle = _ => Polly.PredicateResult.False());
        }
#endif
        return builder;
    }
}
