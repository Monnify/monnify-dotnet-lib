using System.Text.Json.Serialization;

namespace Monnify.Cards;

public sealed class AuthorizeCardOtpRequest
{
    /// <summary>The transaction reference from the <c>ChargeAsync</c> call.</summary>
    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; set; } = string.Empty;

    [JsonPropertyName("collectionChannel")]
    public string CollectionChannel { get; set; } = "API_NOTIFICATION";

    /// <summary>The token ID returned by the <c>ChargeAsync</c> response for a card that requires OTP.</summary>
    [JsonPropertyName("tokenId")]
    public string TokenId { get; set; } = string.Empty;

    /// <summary>The OTP sent to the cardholder's device.</summary>
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;
}
