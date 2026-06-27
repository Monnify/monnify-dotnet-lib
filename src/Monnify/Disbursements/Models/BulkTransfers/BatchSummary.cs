using System.Text.Json.Serialization;

namespace Monnify.Disbursements;

public sealed class BatchSummary
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("totalAmount")]
    public decimal TotalAmount { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("totalFee")]
    public decimal TotalFee { get; set; }

    [JsonPropertyName("batchReference")]
    public string BatchReference { get; set; } = string.Empty;

    [JsonPropertyName("totalTransactionsCount")]
    public int TotalTransactionsCount { get; set; }

    [JsonPropertyName("initiator")]
    public string? Initiator { get; set; }

    [JsonPropertyName("failedCount")]
    public int FailedCount { get; set; }

    [JsonPropertyName("successfulCount")]
    public int SuccessfulCount { get; set; }

    [JsonPropertyName("pendingCount")]
    public int PendingCount { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("pendingAmount")]
    public decimal PendingAmount { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("failedAmount")]
    public decimal FailedAmount { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("successfulAmount")]
    public decimal SuccessfulAmount { get; set; }

    [JsonPropertyName("batchStatus")]
    public string BatchStatus { get; set; } = string.Empty;

    [JsonPropertyName("dateCreated")]
    public string DateCreated { get; set; } = string.Empty;
}
