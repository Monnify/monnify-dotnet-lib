using System.Net;
using System.Net.Http.Headers;

namespace Monnify.Authentication;

/// <summary>
/// Injects the cached Monnify bearer token into every outgoing request on the typed clients it's
/// attached to, and retries once on a 401 after invalidating the cache (handles clock skew or
/// Monnify revoking a token early). Request content is buffered up front because an
/// <see cref="HttpRequestMessage"/> can't be sent twice; this is safe for the SDK's own requests
/// since they're always small in-memory JSON bodies (StringContent), not forward-only streams.
/// </summary>
internal sealed class MonnifyAuthHandler : DelegatingHandler
{
    private readonly IMonnifyTokenProvider _tokenProvider;

    public MonnifyAuthHandler(IMonnifyTokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenProvider.GetAccessTokenAsync(cancellationToken).ConfigureAwait(false);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var bufferedContent = await BufferContentAsync(request, cancellationToken).ConfigureAwait(false);

        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        if (response.StatusCode != HttpStatusCode.Unauthorized)
        {
            return response;
        }

        _tokenProvider.Invalidate();
        response.Dispose();

        var refreshedToken = await _tokenProvider.GetAccessTokenAsync(cancellationToken).ConfigureAwait(false);
        using var retryRequest = CloneRequest(request, bufferedContent);
        retryRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", refreshedToken);
        return await base.SendAsync(retryRequest, cancellationToken).ConfigureAwait(false);
    }

    private static async Task<byte[]?> BufferContentAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content is null)
        {
            return null;
        }

#if NET8_0_OR_GREATER
        return await request.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
#else
        return await request.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
#endif
    }

    private static HttpRequestMessage CloneRequest(HttpRequestMessage original, byte[]? bufferedContent)
    {
        var clone = new HttpRequestMessage(original.Method, original.RequestUri)
        {
            Version = original.Version,
        };

        foreach (var header in original.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        if (bufferedContent is not null)
        {
            var content = new ByteArrayContent(bufferedContent);
            if (original.Content is not null)
            {
                foreach (var header in original.Content.Headers)
                {
                    content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            clone.Content = content;
        }

        return clone;
    }
}
