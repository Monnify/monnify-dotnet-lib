using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Monnify.Exceptions;
using Monnify.Http;

namespace Monnify.Authentication;

/// <summary>
/// Caches Monnify's bearer access token and refreshes it before expiry. Registered as a singleton
/// so the cache is actually effective across requests; it pulls a pooled <see cref="HttpClient"/>
/// from <see cref="IHttpClientFactory"/> on demand rather than having one injected directly, which
/// would otherwise force this into a typed-client (transient) lifetime and defeat the cache.
/// </summary>
internal sealed class MonnifyTokenProvider : IMonnifyTokenProvider, IDisposable
{
    internal const string AuthHttpClientName = "Monnify.Auth";

    private static readonly TimeSpan RefreshBuffer = TimeSpan.FromSeconds(60);
    private const int DefaultExpirySeconds = 3600;

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptionsMonitor<MonnifyClientOptions> _optionsMonitor;
    private readonly SemaphoreSlim _refreshLock = new(1, 1);
    private MonnifyAccessToken? _cached;

    public MonnifyTokenProvider(IHttpClientFactory httpClientFactory, IOptionsMonitor<MonnifyClientOptions> optionsMonitor)
    {
        _httpClientFactory = httpClientFactory;
        _optionsMonitor = optionsMonitor;
    }

    public async ValueTask<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        var current = _cached;
        if (IsUsable(current))
        {
            return current!.Token;
        }

        await _refreshLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            // Re-check: another caller may have refreshed while we were waiting for the lock.
            current = _cached;
            if (IsUsable(current))
            {
                return current!.Token;
            }

            var fresh = await LoginAsync(cancellationToken).ConfigureAwait(false);
            _cached = fresh;
            return fresh.Token;
        }
        finally
        {
            _refreshLock.Release();
        }
    }

    public void Invalidate() => _cached = null;

    private static bool IsUsable(MonnifyAccessToken? token) =>
        token is not null && token.ExpiresAtUtc - RefreshBuffer > DateTimeOffset.UtcNow;

    private async Task<MonnifyAccessToken> LoginAsync(CancellationToken cancellationToken)
    {
        var options = _optionsMonitor.CurrentValue;
        var httpClient = _httpClientFactory.CreateClient(AuthHttpClientName);

        using var request = new HttpRequestMessage(HttpMethod.Post, MonnifyApiPaths.Auth.Login)
        {
            Headers = { Authorization = BuildBasicAuthHeader(options.ApiKey, options.SecretKey) },
        };

        using var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var json = await ReadAsStringAsync(response, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw new MonnifyAuthenticationException(
                $"Monnify authentication failed with HTTP status {(int)response.StatusCode}. Verify the configured API key and secret key.");
        }

        MonnifyResponseEnvelope<MonnifyLoginResponseBody>? envelope;
        try
        {
            envelope = JsonSerializer.Deserialize<MonnifyResponseEnvelope<MonnifyLoginResponseBody>>(json, MonnifyJsonOptions.Default);
        }
        catch (JsonException ex)
        {
            throw new MonnifyAuthenticationException("Could not parse Monnify's authentication response.", ex);
        }

        var responseBody = envelope?.ResponseBody;
        if (envelope is null || !envelope.RequestSuccessful || string.IsNullOrEmpty(responseBody?.AccessToken))
        {
            throw new MonnifyAuthenticationException(
                $"Monnify rejected the authentication request: {envelope?.ResponseMessage ?? "unknown error"}.");
        }

        // responseBody can't be null here: the guard above throws if its AccessToken is null/empty.
        var expiresInSeconds = responseBody!.ExpiresIn > 0 ? responseBody.ExpiresIn : DefaultExpirySeconds;
        return new MonnifyAccessToken(responseBody.AccessToken, DateTimeOffset.UtcNow.AddSeconds(expiresInSeconds));
    }

    private static AuthenticationHeaderValue BuildBasicAuthHeader(string apiKey, string secretKey)
    {
        var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiKey}:{secretKey}"));
        return new AuthenticationHeaderValue("Basic", encoded);
    }

    private static Task<string> ReadAsStringAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
#if NET8_0_OR_GREATER
        return response.Content.ReadAsStringAsync(cancellationToken);
#else
        return response.Content.ReadAsStringAsync();
#endif
    }

    public void Dispose() => _refreshLock.Dispose();
}
