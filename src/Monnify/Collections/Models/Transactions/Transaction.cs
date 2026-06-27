using System.Text.Json.Serialization;

namespace Monnify.Collections;

/// <summary>Full transaction status detail, returned by <c>GetTransactionAsync</c> and <c>QueryTransactionAsync</c>.</summary>
public sealed class Transaction
{
    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; set; } = string.Empty;

    [JsonPropertyName("paymentReference")]
    public string PaymentReference { get; set; } = string.Empty;

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("amountPaid")]
    public decimal AmountPaid { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("totalPayable")]
    public decimal TotalPayable { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("settlementAmount")]
    public decimal? SettlementAmount { get; set; }

    [JsonPropertyName("paidOn")]
    public string? PaidOn { get; set; }

    [JsonPropertyName("paymentStatus")]
    public string PaymentStatus { get; set; } = string.Empty;

    [JsonPropertyName("paymentDescription")]
    public string? PaymentDescription { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("paymentMethod")]
    public string PaymentMethod { get; set; } = string.Empty;

    [JsonPropertyName("product")]
    public TransactionProduct? Product { get; set; }

    [JsonPropertyName("cardDetails")]
    public TransactionCardDetails? CardDetails { get; set; }

    [JsonPropertyName("customer")]
    public TransactionCustomer? Customer { get; set; }

    [JsonPropertyName("paymentScope")]
    public string? PaymentScope { get; set; }
}
