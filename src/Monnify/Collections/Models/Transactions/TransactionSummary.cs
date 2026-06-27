using System.Text.Json.Serialization;

namespace Monnify.Collections;

/// <summary>A single row from <see cref="IMonnifyCollectionsClient.SearchTransactionsAsync"/>.</summary>
public sealed class TransactionSummary
{
    [JsonPropertyName("customerDTO")]
    public TransactionCustomer? Customer { get; set; }

    [JsonPropertyName("paymentMethod")]
    public string? PaymentMethod { get; set; }

    [JsonPropertyName("createdOn")]
    public string CreatedOn { get; set; } = string.Empty;

    [JsonPropertyName("completedOn")]
    public string? CompletedOn { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; } = string.Empty;

    [JsonPropertyName("paymentDescription")]
    public string? PaymentDescription { get; set; }

    [JsonPropertyName("paymentStatus")]
    public string PaymentStatus { get; set; } = string.Empty;

    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; set; } = string.Empty;

    [JsonPropertyName("paymentReference")]
    public string PaymentReference { get; set; } = string.Empty;

    [JsonPropertyName("merchantName")]
    public string MerchantName { get; set; } = string.Empty;

    [JsonPropertyName("completed")]
    public bool Completed { get; set; }

    [JsonPropertyName("paymentMethodList")]
    public IReadOnlyList<string> PaymentMethodList { get; set; } = Array.Empty<string>();

    [JsonPropertyName("collectionChannel")]
    public string CollectionChannel { get; set; } = string.Empty;

    [JsonPropertyName("paymentScope")]
    public string? PaymentScope { get; set; }
}
