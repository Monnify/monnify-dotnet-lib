using System.Text.Json.Serialization;

namespace Monnify.Disbursements;

public sealed class WalletTransaction
{
    [JsonPropertyName("walletTransactionReference")]
    public string WalletTransactionReference { get; set; } = string.Empty;

    [JsonPropertyName("monnifyTransactionReference")]
    public string MonnifyTransactionReference { get; set; } = string.Empty;

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("availableBalanceBefore")]
    public decimal AvailableBalanceBefore { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("availableBalanceAfter")]
    public decimal AvailableBalanceAfter { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("transactionDate")]
    public string TransactionDate { get; set; } = string.Empty;

    /// <summary>E.g. <c>DEBIT</c> or <c>CREDIT</c>.</summary>
    [JsonPropertyName("transactionType")]
    public string TransactionType { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("narration")]
    public string? Narration { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
}
