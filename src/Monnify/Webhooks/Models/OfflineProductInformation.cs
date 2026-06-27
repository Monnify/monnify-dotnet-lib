using System.Text.Json.Serialization;

namespace Monnify.Webhooks;

/// <summary>Present only on offline-payment variants of <see cref="MonnifyWebhookEventTypes.SuccessfulTransaction"/>.</summary>
public sealed class OfflineProductInformation
{
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("amount")]
    public decimal? Amount { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
}
