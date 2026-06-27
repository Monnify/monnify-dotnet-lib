using System.Text.Json.Serialization;

namespace Monnify.Webhooks;

/// <summary>
/// Event data shared by <see cref="MonnifyWebhookEventTypes.SuccessfulRefund"/> and
/// <see cref="MonnifyWebhookEventTypes.FailedRefund"/> - the two event types have identical
/// eventData shapes; <see cref="RefundStatus"/> tells you which actually happened.
/// </summary>
public sealed class RefundEventData
{
    [JsonPropertyName("merchantReason")]
    public string? MerchantReason { get; set; }

    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; set; } = string.Empty;

    [JsonPropertyName("completedOn")]
    public string? CompletedOn { get; set; }

    /// <summary><c>COMPLETED</c> or <c>FAILED</c>.</summary>
    [JsonPropertyName("refundStatus")]
    public string RefundStatus { get; set; } = string.Empty;

    [JsonPropertyName("customerNote")]
    public string? CustomerNote { get; set; }

    [JsonPropertyName("createdOn")]
    public string CreatedOn { get; set; } = string.Empty;

    [JsonPropertyName("refundReference")]
    public string RefundReference { get; set; } = string.Empty;

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("refundAmount")]
    public decimal RefundAmount { get; set; }
}
