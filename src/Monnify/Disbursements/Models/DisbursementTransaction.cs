using System.Text.Json.Serialization;

namespace Monnify.Disbursements;

/// <summary>
/// A single transfer's status detail, returned by <c>GetSingleTransferAsync</c> and as list items
/// from <c>GetSingleTransfersAsync</c>, <c>SearchTransactionsAsync</c>, and
/// <c>GetBulkTransferTransactionsAsync</c>.
/// </summary>
public sealed class DisbursementTransaction
{
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("reference")]
    public string Reference { get; set; } = string.Empty;

    [JsonPropertyName("narration")]
    public string? Narration { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("fee")]
    public decimal Fee { get; set; }

    [JsonPropertyName("twoFaEnabled")]
    public bool TwoFaEnabled { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("transactionDescription")]
    public string? TransactionDescription { get; set; }

    [JsonPropertyName("transactionReference")]
    public string? TransactionReference { get; set; }

    [JsonPropertyName("createdOn")]
    public string CreatedOn { get; set; } = string.Empty;

    [JsonPropertyName("sourceAccountNumber")]
    public string? SourceAccountNumber { get; set; }

    [JsonPropertyName("destinationAccountNumber")]
    public string DestinationAccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("destinationAccountName")]
    public string? DestinationAccountName { get; set; }

    [JsonPropertyName("destinationBankCode")]
    public string DestinationBankCode { get; set; } = string.Empty;

    [JsonPropertyName("destinationBankName")]
    public string? DestinationBankName { get; set; }
}
