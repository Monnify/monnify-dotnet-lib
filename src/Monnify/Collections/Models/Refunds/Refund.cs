using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class Refund
{
    [JsonPropertyName("refundReference")]
    public string RefundReference { get; set; } = string.Empty;

    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; set; } = string.Empty;

    [JsonPropertyName("refundReason")]
    public string RefundReason { get; set; } = string.Empty;

    [JsonPropertyName("customerNote")]
    public string CustomerNote { get; set; } = string.Empty;

    [JsonPropertyName("refundAmount")]
    public decimal RefundAmount { get; set; }

    /// <summary>E.g. <c>PARTIAL_REFUND</c> or <c>FULL_REFUND</c>.</summary>
    [JsonPropertyName("refundType")]
    public string RefundType { get; set; } = string.Empty;

    /// <summary>E.g. <c>PENDING</c>, <c>COMPLETED</c>, <c>FAILED</c>.</summary>
    [JsonPropertyName("refundStatus")]
    public string RefundStatus { get; set; } = string.Empty;

    /// <summary>E.g. <c>MERCHANT_WALLET</c> or <c>ORIGINAL_PAYMENT_SOURCE</c>.</summary>
    [JsonPropertyName("refundStrategy")]
    public string RefundStrategy { get; set; } = string.Empty;

    [JsonPropertyName("comment")]
    public string? Comment { get; set; }

    [JsonPropertyName("createdOn")]
    public string CreatedOn { get; set; } = string.Empty;

    /// <summary>Monnify's internal transfer reference (e.g. <c>TRFD|...</c>). Not in our published docs sample but returned by the real API.</summary>
    [JsonPropertyName("reference")]
    public string? Reference { get; set; }

    /// <summary>Populated once the refund has settled; empty string when still pending.</summary>
    [JsonPropertyName("completedOn")]
    public string? CompletedOn { get; set; }

    [JsonPropertyName("destinationAccountNumber")]
    public string? DestinationAccountNumber { get; set; }

    [JsonPropertyName("destinationAccountName")]
    public string? DestinationAccountName { get; set; }

    [JsonPropertyName("destinationAccountBankCode")]
    public string? DestinationAccountBankCode { get; set; }

    [JsonPropertyName("destinationBankName")]
    public string? DestinationBankName { get; set; }

    [JsonPropertyName("currencyCode")]
    public string? CurrencyCode { get; set; }
}
