using System.Text.Json;
using System.Text.Json.Serialization;

namespace Monnify.Webhooks;

/// <summary>
/// Event data for <see cref="MonnifyWebhookEventTypes.SuccessfulTransaction"/>, covering both a
/// regular collection payment and an offline (cash/agent) payment - the latter additionally
/// populates <see cref="InvoiceReference"/> and <see cref="OfflineProductInformation"/>.
/// </summary>
public sealed class CollectionTransactionEventData
{
    [JsonPropertyName("product")]
    public ProductInfo? Product { get; set; }

    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; set; } = string.Empty;

    [JsonPropertyName("paymentReference")]
    public string PaymentReference { get; set; } = string.Empty;

    /// <summary>Present only on offline payments.</summary>
    [JsonPropertyName("invoiceReference")]
    public string? InvoiceReference { get; set; }

    [JsonPropertyName("paidOn")]
    public string PaidOn { get; set; } = string.Empty;

    [JsonPropertyName("paymentDescription")]
    public string? PaymentDescription { get; set; }

    /// <summary>Arbitrary merchant-supplied data passed in when the original transaction was initialized.</summary>
    [JsonPropertyName("metaData")]
    public Dictionary<string, JsonElement>? MetaData { get; set; }

    /// <summary>
    /// The bank-transfer legs that funded this payment. Comes back as an empty object instead of
    /// an array for some payment methods (e.g. offline payments) - either shape deserializes here.
    /// </summary>
    [JsonConverter(typeof(SingleOrArrayJsonConverter<PaymentSourceInfo>))]
    [JsonPropertyName("paymentSourceInformation")]
    public IReadOnlyList<PaymentSourceInfo> PaymentSourceInformation { get; set; } = Array.Empty<PaymentSourceInfo>();

    [JsonPropertyName("destinationAccountInformation")]
    public DestinationAccountInfo? DestinationAccountInformation { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("amountPaid")]
    public decimal AmountPaid { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("totalPayable")]
    public decimal TotalPayable { get; set; }

    /// <summary>Present only on offline payments.</summary>
    [JsonPropertyName("offlineProductInformation")]
    public OfflineProductInformation? OfflineProductInformation { get; set; }

    [JsonPropertyName("paymentMethod")]
    public string PaymentMethod { get; set; } = string.Empty;

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("settlementAmount")]
    public decimal? SettlementAmount { get; set; }

    [JsonPropertyName("paymentStatus")]
    public string PaymentStatus { get; set; } = string.Empty;

    [JsonPropertyName("customer")]
    public WebhookCustomerInfo? Customer { get; set; }
}
