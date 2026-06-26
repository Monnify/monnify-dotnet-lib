using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class ReservedAccount
{
    [JsonPropertyName("contractCode")]
    public string ContractCode { get; set; } = string.Empty;

    [JsonPropertyName("accountReference")]
    public string AccountReference { get; set; } = string.Empty;

    [JsonPropertyName("accountName")]
    public string AccountName { get; set; } = string.Empty;

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; } = string.Empty;

    [JsonPropertyName("customerEmail")]
    public string CustomerEmail { get; set; } = string.Empty;

    [JsonPropertyName("customerName")]
    public string CustomerName { get; set; } = string.Empty;

    [JsonPropertyName("accounts")]
    public IReadOnlyList<ReservedAccountBank> Accounts { get; set; } = Array.Empty<ReservedAccountBank>();

    [JsonPropertyName("collectionChannel")]
    public string CollectionChannel { get; set; } = string.Empty;

    [JsonPropertyName("reservationReference")]
    public string ReservationReference { get; set; } = string.Empty;

    [JsonPropertyName("reservedAccountType")]
    public string ReservedAccountType { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>Monnify's timestamp format/precision varies by endpoint, so this is left as the raw string.</summary>
    [JsonPropertyName("createdOn")]
    public string CreatedOn { get; set; } = string.Empty;

    [JsonPropertyName("restrictPaymentSource")]
    public bool RestrictPaymentSource { get; set; }

    [JsonPropertyName("incomeSplitConfig")]
    public IReadOnlyList<IncomeSplitConfig> IncomeSplitConfig { get; set; } = Array.Empty<IncomeSplitConfig>();

    /// <summary>Only present when fetched via <c>GetReservedAccountAsync</c>, not on creation.</summary>
    [JsonPropertyName("transactionCount")]
    public int? TransactionCount { get; set; }
}
