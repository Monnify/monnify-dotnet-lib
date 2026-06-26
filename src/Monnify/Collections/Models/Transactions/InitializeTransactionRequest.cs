using System.Text.Json.Serialization;

namespace Monnify.Collections;

/// <summary>Starts a checkout for a one-time payment, returning a hosted checkout URL.</summary>
public sealed class InitializeTransactionRequest
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("customerName")]
    public string CustomerName { get; set; } = string.Empty;

    [JsonPropertyName("customerEmail")]
    public string CustomerEmail { get; set; } = string.Empty;

    [JsonPropertyName("paymentReference")]
    public string PaymentReference { get; set; } = string.Empty;

    [JsonPropertyName("paymentDescription")]
    public string PaymentDescription { get; set; } = string.Empty;

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; } = "NGN";

    [JsonPropertyName("contractCode")]
    public string ContractCode { get; set; } = string.Empty;

    [JsonPropertyName("redirectUrl")]
    public string? RedirectUrl { get; set; }

    /// <summary>e.g. <c>["ACCOUNT_TRANSFER", "CARD"]</c>. Leave null to use the contract's defaults.</summary>
    [JsonPropertyName("paymentMethods")]
    public IReadOnlyList<string>? PaymentMethods { get; set; }
}
