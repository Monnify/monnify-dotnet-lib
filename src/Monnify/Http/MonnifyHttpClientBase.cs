using System.Text.Json;
using Monnify.Exceptions;

namespace Monnify.Http;

/// <summary>
/// Shared request/response handling for typed Monnify clients: sends a request, unwraps Monnify's
/// standard envelope, and throws a typed exception on failure or on an unparsable response.
/// </summary>
internal abstract class MonnifyHttpClientBase
{
    private readonly HttpClient _httpClient;

    protected MonnifyHttpClientBase(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    protected async Task<TResponseBody> SendAsync<TResponseBody>(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        using var response = await _httpClient
            .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        var json = await ReadContentAsStringAsync(response, cancellationToken).ConfigureAwait(false);

        MonnifyResponseEnvelope<TResponseBody>? envelope;
        try
        {
            envelope = JsonSerializer.Deserialize<MonnifyResponseEnvelope<TResponseBody>>(json, MonnifyJsonOptions.Default);
        }
        catch (JsonException ex)
        {
            throw new MonnifyDeserializationException("Failed to parse the response received from Monnify.", json, ex);
        }

        if (envelope is null || !envelope.RequestSuccessful)
        {
            throw new MonnifyApiException(
                envelope?.ResponseCode ?? "UNKNOWN",
                envelope?.ResponseMessage ?? $"Monnify returned an unsuccessful response (HTTP {(int)response.StatusCode}).",
                (int)response.StatusCode,
                json);
        }

        if (envelope.ResponseBody is null)
        {
            throw new MonnifyDeserializationException(
                $"Monnify reported success but returned no {typeof(TResponseBody).Name} response body.", json,
                new InvalidOperationException("responseBody was null."));
        }

        return envelope.ResponseBody;
    }

    private static Task<string> ReadContentAsStringAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
#if NET8_0_OR_GREATER
        return response.Content.ReadAsStringAsync(cancellationToken);
#else
        return response.Content.ReadAsStringAsync();
#endif
    }
}
