using System.Text.Json.Serialization;

namespace Monnify.Collections;

/// <summary>
/// Charges a previously tokenized, reusable card - see <see cref="TransactionCardDetails.CardToken"/>
/// on a prior charge's result (only present when <see cref="TransactionCardDetails.Reusable"/> is
/// <see langword="true"/>). Unlike <see cref="ChargeCardRequest"/>, this needs no raw card details
/// and completes in one call - no OTP/3DS follow-up.
/// </summary>
public sealed class ChargeCardTokenRequest
{
    [JsonPropertyName("cardToken")]
    public string CardToken { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("customerName")]
    public string? CustomerName { get; set; }

    /// <summary>Must match the customer email used on the original charge that produced this card token.</summary>
    [JsonPropertyName("customerEmail")]
    public string CustomerEmail { get; set; } = string.Empty;

    [JsonPropertyName("paymentReference")]
    public string PaymentReference { get; set; } = string.Empty;

    [JsonPropertyName("paymentDescription")]
    public string? PaymentDescription { get; set; }

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; } = "NGN";

    [JsonPropertyName("contractCode")]
    public string ContractCode { get; set; } = string.Empty;

    /// <summary>The merchant's API key (your <c>MonnifyClientOptions.ApiKey</c>).</summary>
    [JsonPropertyName("apiKey")]
    public string ApiKey { get; set; } = string.Empty;

    [JsonPropertyName("metaData")]
    public IDictionary<string, object?>? MetaData { get; set; }

    /// <summary>Splits this payment among sub-accounts, same as on <see cref="CreateInvoiceRequest"/>.</summary>
    [JsonPropertyName("incomeSplitConfig")]
    public IReadOnlyList<IncomeSplitConfig>? IncomeSplitConfig { get; set; }
}
