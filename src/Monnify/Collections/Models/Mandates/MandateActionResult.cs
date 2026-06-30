using System.Text.Json.Serialization;

namespace Monnify.Collections;

/// <summary>Returned by both <c>CreateMandateAsync</c> and <c>CancelMandateAsync</c> - they share the same shape, minus <see cref="RedirectUrl"/> on cancel.</summary>
public sealed class MandateActionResult
{
    [JsonPropertyName("responseMessage")]
    public string ResponseMessage { get; set; } = string.Empty;

    [JsonPropertyName("mandateReference")]
    public string MandateReference { get; set; } = string.Empty;

    [JsonPropertyName("mandateCode")]
    public string MandateCode { get; set; } = string.Empty;

    /// <summary>E.g. <c>INITIATED</c> after creation, or the mandate's prior status on cancellation.</summary>
    [JsonPropertyName("mandateStatus")]
    public string MandateStatus { get; set; } = string.Empty;

    /// <summary>Only populated by <c>CreateMandateAsync</c>.</summary>
    [JsonPropertyName("redirectUrl")]
    public string? RedirectUrl { get; set; }
}
