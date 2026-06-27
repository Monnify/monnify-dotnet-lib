using System.Text.Json.Serialization;

namespace Monnify.Disbursements;

public sealed class BulkTransferResult
{
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("totalAmount")]
    public decimal TotalAmount { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("totalFee")]
    public decimal TotalFee { get; set; }

    [JsonPropertyName("batchReference")]
    public string BatchReference { get; set; } = string.Empty;

    /// <summary>Monnify's own internal batch reference, distinct from the merchant-supplied <see cref="BatchReference"/>.</summary>
    [JsonPropertyName("transactionBatchReference")]
    public string? TransactionBatchReference { get; set; }

    [JsonPropertyName("batchStatus")]
    public string BatchStatus { get; set; } = string.Empty;

    [JsonPropertyName("totalTransactionsCount")]
    public int TotalTransactionsCount { get; set; }

    [JsonPropertyName("dateCreated")]
    public string DateCreated { get; set; } = string.Empty;
}
