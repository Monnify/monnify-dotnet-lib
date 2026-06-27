using System.Text.Json.Serialization;

namespace Monnify.Webhooks;

/// <summary>
/// Event data for <see cref="MonnifyWebhookEventTypes.AccountActivity"/>. The envelope for this
/// event type also carries a sibling <c>metaData</c> field outside <c>eventData</c> - read it via
/// <see cref="MonnifyWebhookParser.ParseMetaData{TMetaData}"/> on the envelope, not from this class.
/// </summary>
public sealed class WalletActivityEventData
{
    [JsonPropertyName("accountType")]
    public string AccountType { get; set; } = string.Empty;

    [JsonPropertyName("accountName")]
    public string AccountName { get; set; } = string.Empty;

    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("accountNuban")]
    public string? AccountNuban { get; set; }

    [JsonPropertyName("activityType")]
    public string ActivityType { get; set; } = string.Empty;

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("balanceBefore")]
    public decimal BalanceBefore { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("balanceAfter")]
    public decimal BalanceAfter { get; set; }

    [JsonPropertyName("reference")]
    public string Reference { get; set; } = string.Empty;

    [JsonPropertyName("narration")]
    public string? Narration { get; set; }

    [JsonPropertyName("activityTime")]
    public string ActivityTime { get; set; } = string.Empty;
}
