using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class InitializeTransactionResult
{
    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; set; } = string.Empty;

    [JsonPropertyName("paymentReference")]
    public string PaymentReference { get; set; } = string.Empty;

    [JsonPropertyName("merchantName")]
    public string MerchantName { get; set; } = string.Empty;

    [JsonPropertyName("apiKey")]
    public string ApiKey { get; set; } = string.Empty;

    [JsonPropertyName("redirectUrl")]
    public string? RedirectUrl { get; set; }

    [JsonPropertyName("enabledPaymentMethod")]
    public IReadOnlyList<string> EnabledPaymentMethod { get; set; } = Array.Empty<string>();

    /// <summary>The hosted checkout page to redirect the customer to.</summary>
    [JsonPropertyName("checkoutUrl")]
    public string CheckoutUrl { get; set; } = string.Empty;
}
