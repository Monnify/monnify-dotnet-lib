using System.Text.Json.Serialization;

namespace Monnify.Disbursements;

/// <summary>
/// Initiates a batch of transfers. Requires Monnify to have enabled the Transfer feature for the
/// merchant; contact sales@monnify.com to request access.
/// </summary>
public sealed class BulkTransferRequest
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("batchReference")]
    public string BatchReference { get; set; } = string.Empty;

    [JsonPropertyName("narration")]
    public string Narration { get; set; } = string.Empty;

    /// <summary>Your Monnify wallet account number.</summary>
    [JsonPropertyName("sourceAccountNumber")]
    public string SourceAccountNumber { get; set; } = string.Empty;

    /// <summary>Either <c>CONTINUE</c> or <c>BREAK</c> — what to do if one transfer in the batch fails validation.</summary>
    [JsonPropertyName("onValidationFailure")]
    public string OnValidationFailure { get; set; } = "CONTINUE";

    /// <summary>How often (in number of transactions processed) Monnify should notify the merchant of progress.</summary>
    [JsonPropertyName("notificationInterval")]
    public int NotificationInterval { get; set; }

    [JsonPropertyName("transactionList")]
    public IReadOnlyList<BulkTransferItem> TransactionList { get; set; } = Array.Empty<BulkTransferItem>();
}
