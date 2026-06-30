using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class ChargeCardResult
{
    /// <summary>E.g. <c>SUCCESS</c> for a card that doesn't require OTP/3DS.</summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; set; } = string.Empty;

    [JsonPropertyName("paymentReference")]
    public string PaymentReference { get; set; } = string.Empty;

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("authorizedAmount")]
    public decimal AuthorizedAmount { get; set; }

    /// <summary>
    /// Present when the card requires OTP - pass it to <c>AuthorizeOtpAsync</c>. Our only documented
    /// sample response is for a card that doesn't need OTP, so this field doesn't appear there; its
    /// existence and name come from the OTP-authorize endpoint's own request docs ("the token ID
    /// from the charge card endpoint response"), not from a captured OTP-required sample.
    /// </summary>
    [JsonPropertyName("tokenId")]
    public string? TokenId { get; set; }
}
