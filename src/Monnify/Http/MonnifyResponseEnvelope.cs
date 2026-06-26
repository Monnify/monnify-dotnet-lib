using System.Text.Json.Serialization;

namespace Monnify.Http;

/// <summary>Monnify's standard response envelope, observed across the endpoints checked so far.</summary>
internal sealed class MonnifyResponseEnvelope<TResponseBody>
{
    [JsonPropertyName("requestSuccessful")]
    public bool RequestSuccessful { get; set; }

    [JsonPropertyName("responseMessage")]
    public string ResponseMessage { get; set; } = string.Empty;

    [JsonPropertyName("responseCode")]
    public string ResponseCode { get; set; } = string.Empty;

    [JsonPropertyName("responseBody")]
    public TResponseBody? ResponseBody { get; set; }
}
