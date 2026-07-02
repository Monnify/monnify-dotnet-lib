using System.Text;
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

    protected async Task SendVoidAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        using var response = await _httpClient
            .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        var json = await ReadContentAsStringAsync(response, cancellationToken).ConfigureAwait(false);

        MonnifyResponseEnvelope<object?>? envelope;
        try
        {
            envelope = JsonSerializer.Deserialize<MonnifyResponseEnvelope<object?>>(json, MonnifyJsonOptions.Default);
        }
        catch (JsonException ex)
        {
            throw CreateExceptionForUnparsableBody(response, json, ex);
        }

        if (envelope is null || envelope.RequestSuccessful == false)
        {
            throw new MonnifyApiException(
                envelope?.ResponseCode ?? "UNKNOWN",
                envelope?.ResponseMessage ?? $"Monnify returned an unsuccessful response (HTTP {(int)response.StatusCode}).",
                (int)response.StatusCode,
                json);
        }
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
            throw CreateExceptionForUnparsableBody(response, json, ex);
        }

        // Treat as a definitive failure if RequestSuccessful is explicitly false, or if it is
        // absent (null) and there is no responseBody — which indicates a non-Monnify error body
        // (e.g. a gateway rejection) rather than a paycode-style response that omits the field.
        bool definiteFailure = envelope is null || envelope.RequestSuccessful == false;
        bool ambiguousNoBody = envelope is not null && envelope.RequestSuccessful is null && envelope.ResponseBody is null;

        if (definiteFailure || ambiguousNoBody)
        {
            throw new MonnifyApiException(
                envelope?.ResponseCode ?? "UNKNOWN",
                envelope?.ResponseMessage ?? $"Monnify returned an unsuccessful response (HTTP {(int)response.StatusCode}).",
                (int)response.StatusCode,
                json);
        }

        if (envelope!.ResponseBody is null)
        {
            throw new MonnifyDeserializationException(
                $"Monnify reported success but returned no {typeof(TResponseBody).Name} response body.", json,
                new InvalidOperationException("responseBody was null."));
        }

        return envelope.ResponseBody;
    }

    /// <summary>
    /// A body that fails to parse as JSON at all (not just as an envelope) is unambiguously a
    /// failure whenever the HTTP status itself already says so - e.g. a proxy/gateway's own error
    /// page (plain text or HTML, not even JSON) for a 502/504. Surface that the same way as any
    /// other API failure rather than a deserialization error, which is reserved for the genuinely
    /// unexpected case: Monnify reporting HTTP success while returning a body that doesn't parse.
    /// </summary>
    private static Exception CreateExceptionForUnparsableBody(HttpResponseMessage response, string json, JsonException innerException)
    {
        if (!response.IsSuccessStatusCode)
        {
            return new MonnifyApiException(
                "UNKNOWN",
                $"Monnify returned an unsuccessful response (HTTP {(int)response.StatusCode}).",
                (int)response.StatusCode,
                json);
        }

        return new MonnifyDeserializationException("Failed to parse the response received from Monnify.", json, innerException);
    }

    private static Task<string> ReadContentAsStringAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
#if NET8_0_OR_GREATER
        return response.Content.ReadAsStringAsync(cancellationToken);
#else
        return response.Content.ReadAsStringAsync();
#endif
    }

    protected static HttpContent CreateJsonContent<TRequestBody>(TRequestBody value) =>
        new StringContent(JsonSerializer.Serialize(value, MonnifyJsonOptions.Default), Encoding.UTF8, "application/json");
}
