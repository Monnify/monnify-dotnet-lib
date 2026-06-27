using System.Text.Json.Serialization;

namespace Monnify.Webhooks;

/// <summary>Event data for <see cref="MonnifyWebhookEventTypes.LowBalanceAlert"/>.</summary>
public sealed class LowBalanceAlertEventData
{
    [JsonPropertyName("transactionTime")]
    public string TransactionTime { get; set; } = string.Empty;

    [JsonPropertyName("merchantCode")]
    public string MerchantCode { get; set; } = string.Empty;

    [JsonPropertyName("walletAccountNumber")]
    public string WalletAccountNumber { get; set; } = string.Empty;

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("walletBalance")]
    public decimal WalletBalance { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("lowBalanceThreshold")]
    public decimal LowBalanceThreshold { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}
