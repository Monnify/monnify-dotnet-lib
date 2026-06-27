using System.Text.Json;
using System.Text.Json.Serialization;

namespace Monnify.Webhooks;

/// <summary>Event data for <see cref="MonnifyWebhookEventTypes.RejectedPayment"/>.</summary>
public sealed class RejectedPaymentEventData
{
    /// <summary>Arbitrary merchant-supplied data; our own docs sample for this field is malformed, so this is read as a raw element rather than assuming object or string.</summary>
    [JsonPropertyName("metaData")]
    public JsonElement? MetaData { get; set; }

    [JsonPropertyName("product")]
    public ProductInfo? Product { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    /// <summary>
    /// The payment leg that was rejected. Unlike <see cref="CollectionTransactionEventData.PaymentSourceInformation"/>
    /// this comes back as a single populated object here, not an array - either shape deserializes.
    /// </summary>
    [JsonConverter(typeof(SingleOrArrayJsonConverter<PaymentSourceInfo>))]
    [JsonPropertyName("paymentSourceInformation")]
    public IReadOnlyList<PaymentSourceInfo> PaymentSourceInformation { get; set; } = Array.Empty<PaymentSourceInfo>();

    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; set; } = string.Empty;

    /// <summary>Note the snake_case field name - inconsistent with every other event type's <c>createdOn</c>.</summary>
    [JsonPropertyName("created_on")]
    public string CreatedOn { get; set; } = string.Empty;

    [JsonPropertyName("paymentReference")]
    public string PaymentReference { get; set; } = string.Empty;

    [JsonPropertyName("paymentRejectionInformation")]
    public PaymentRejectionInformation? PaymentRejectionInformation { get; set; }

    [JsonPropertyName("paymentDescription")]
    public string? PaymentDescription { get; set; }

    [JsonPropertyName("customer")]
    public WebhookCustomerInfo? Customer { get; set; }
}
