using System.Text.Json.Serialization;

namespace Monnify.Webhooks;

/// <summary>
/// Event data shared by <see cref="MonnifyWebhookEventTypes.SuccessfulDisbursement"/>,
/// <see cref="MonnifyWebhookEventTypes.FailedDisbursement"/>, and
/// <see cref="MonnifyWebhookEventTypes.ReversedDisbursement"/> - the three event types have
/// identical eventData shapes; <see cref="Status"/> tells you which actually happened.
/// </summary>
public sealed class DisbursementStatusEventData
{
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; set; } = string.Empty;

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("fee")]
    public decimal Fee { get; set; }

    [JsonPropertyName("transactionDescription")]
    public string? TransactionDescription { get; set; }

    [JsonPropertyName("destinationAccountNumber")]
    public string DestinationAccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("sessionId")]
    public string? SessionId { get; set; }

    [JsonPropertyName("createdOn")]
    public string CreatedOn { get; set; } = string.Empty;

    [JsonPropertyName("destinationAccountName")]
    public string DestinationAccountName { get; set; } = string.Empty;

    /// <summary>The merchant's own reference for this transfer.</summary>
    [JsonPropertyName("reference")]
    public string Reference { get; set; } = string.Empty;

    [JsonPropertyName("destinationBankCode")]
    public string DestinationBankCode { get; set; } = string.Empty;

    [JsonPropertyName("completedOn")]
    public string? CompletedOn { get; set; }

    [JsonPropertyName("narration")]
    public string Narration { get; set; } = string.Empty;

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("destinationBankName")]
    public string DestinationBankName { get; set; } = string.Empty;

    /// <summary><c>SUCCESS</c>, <c>FAILED</c>, or <c>REVERSED</c>.</summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
}
