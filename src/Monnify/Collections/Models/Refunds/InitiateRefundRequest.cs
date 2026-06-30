using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class InitiateRefundRequest
{
    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; set; } = string.Empty;

    [JsonPropertyName("refundAmount")]
    public decimal RefundAmount { get; set; }

    [JsonPropertyName("refundReference")]
    public string RefundReference { get; set; } = string.Empty;

    [JsonPropertyName("refundReason")]
    public string RefundReason { get; set; } = string.Empty;

    [JsonPropertyName("customerNote")]
    public string CustomerNote { get; set; } = string.Empty;

    /// <summary>Account to credit for the refund. Defaults to the original payment source when omitted.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("destinationAccountNumber")]
    public string? DestinationAccountNumber { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("destinationAccountBankCode")]
    public string? DestinationAccountBankCode { get; set; }
}
