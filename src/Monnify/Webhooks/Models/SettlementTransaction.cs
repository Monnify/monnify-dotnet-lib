using System.Text.Json.Serialization;

namespace Monnify.Webhooks;

/// <summary>One settled collection payment within a <see cref="SettlementEventData"/>.</summary>
public sealed class SettlementTransaction
{
    [JsonPropertyName("product")]
    public ProductInfo? Product { get; set; }

    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; set; } = string.Empty;

    [JsonPropertyName("paymentReference")]
    public string PaymentReference { get; set; } = string.Empty;

    [JsonPropertyName("paidOn")]
    public string PaidOn { get; set; } = string.Empty;

    [JsonPropertyName("paymentDescription")]
    public string? PaymentDescription { get; set; }

    [JsonPropertyName("accountPayments")]
    public IReadOnlyList<SettlementAccountPayment> AccountPayments { get; set; } = Array.Empty<SettlementAccountPayment>();

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("amountPaid")]
    public decimal AmountPaid { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("totalPayable")]
    public decimal TotalPayable { get; set; }

    [JsonPropertyName("accountDetails")]
    public SettlementAccountPayment? AccountDetails { get; set; }

    [JsonPropertyName("paymentMethod")]
    public string PaymentMethod { get; set; } = string.Empty;

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("paymentStatus")]
    public string PaymentStatus { get; set; } = string.Empty;

    [JsonPropertyName("customer")]
    public WebhookCustomerInfo? Customer { get; set; }
}
