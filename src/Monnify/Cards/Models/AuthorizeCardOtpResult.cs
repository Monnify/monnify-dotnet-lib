using System.Text.Json.Serialization;

namespace Monnify.Cards;

/// <summary>Returned by both <c>AuthorizeOtpAsync</c> and <c>Authorize3dsAsync</c> - they share the same shape.</summary>
public sealed class AuthorizeCardOtpResult
{
    /// <summary>E.g. <c>SUCCESSFUL</c>.</summary>
    [JsonPropertyName("paymentStatus")]
    public string PaymentStatus { get; set; } = string.Empty;

    [JsonPropertyName("paymentDescription")]
    public string PaymentDescription { get; set; } = string.Empty;

    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; set; } = string.Empty;

    [JsonPropertyName("paymentReference")]
    public string PaymentReference { get; set; } = string.Empty;

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("amountPaid")]
    public decimal AmountPaid { get; set; }

    [JsonPropertyName("currencyPaid")]
    public string CurrencyPaid { get; set; } = string.Empty;
}
