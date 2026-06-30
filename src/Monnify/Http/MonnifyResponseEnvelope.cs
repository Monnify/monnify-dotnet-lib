using System.Text.Json.Serialization;

namespace Monnify.Http;

/// <summary>Monnify's standard response envelope, observed across the endpoints checked so far.</summary>
internal sealed class MonnifyResponseEnvelope<TResponseBody>
{
    /// <summary>
    /// Most endpoints send this as <c>true</c>/<c>false</c>; a few (e.g. paycodes) omit it entirely.
    /// Deserializes as <c>null</c> when absent — treated as a success attempt by the base client.
    /// Only an explicit <c>false</c> is a definitive API failure.
    /// </summary>
    [JsonPropertyName("requestSuccessful")]
    public bool? RequestSuccessful { get; set; }

    // Nullable with no default: a missing field must deserialize to null, not "", so callers'
    // `envelope.ResponseMessage ?? fallback` actually fires for non-Monnify-shaped error bodies
    // (e.g. a gateway-level rejection) instead of silently producing an empty string.
    [JsonPropertyName("responseMessage")]
    public string? ResponseMessage { get; set; }

    [JsonPropertyName("responseCode")]
    public string? ResponseCode { get; set; }

    [JsonPropertyName("responseBody")]
    public TResponseBody? ResponseBody { get; set; }
}
