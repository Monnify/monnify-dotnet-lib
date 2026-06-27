using System.Text.Json.Serialization;

namespace Monnify.Webhooks;

/// <summary>Event data for <see cref="MonnifyWebhookEventTypes.Settlement"/>.</summary>
public sealed class SettlementEventData
{
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("settlementTime")]
    public string SettlementTime { get; set; } = string.Empty;

    [JsonPropertyName("settlementReference")]
    public string SettlementReference { get; set; } = string.Empty;

    [JsonPropertyName("destinationAccountNumber")]
    public string DestinationAccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("destinationBankName")]
    public string DestinationBankName { get; set; } = string.Empty;

    [JsonPropertyName("destinationAccountName")]
    public string DestinationAccountName { get; set; } = string.Empty;

    [JsonPropertyName("transactionsCount")]
    public int TransactionsCount { get; set; }

    [JsonPropertyName("transactions")]
    public IReadOnlyList<SettlementTransaction> Transactions { get; set; } = Array.Empty<SettlementTransaction>();
}
