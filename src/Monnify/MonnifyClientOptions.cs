namespace Monnify;

/// <summary>
/// Credentials and environment configuration for <see cref="ServiceCollectionExtensions.AddMonnify"/>.
/// </summary>
public sealed class MonnifyClientOptions
{
    /// <summary>Your Monnify API key.</summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Your Monnify secret key. Used both for the Basic-auth login exchange and for computing
    /// webhook signatures. Never log this value.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>Which Monnify environment to call. Defaults to <see cref="MonnifyEnvironment.Sandbox"/>.</summary>
    public MonnifyEnvironment Environment { get; set; } = MonnifyEnvironment.Sandbox;

    /// <summary>
    /// Overrides the base URL that would otherwise be derived from <see cref="Environment"/>.
    /// Only set this for testing against a proxy or a non-standard endpoint; in normal use,
    /// set <see cref="Environment"/> instead. Must use HTTPS.
    /// </summary>
    public Uri? BaseUrl { get; set; }

    internal Uri EffectiveBaseUrl => BaseUrl ?? DefaultBaseUrlFor(Environment);

    /// <summary>Returns Monnify's documented base URL for the given environment.</summary>
    public static Uri DefaultBaseUrlFor(MonnifyEnvironment environment) => environment switch
    {
        MonnifyEnvironment.Live => new Uri("https://api.monnify.com"),
        _ => new Uri("https://sandbox.monnify.com"),
    };
}
