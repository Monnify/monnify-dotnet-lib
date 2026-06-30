using System.Text.Json.Serialization;

namespace Monnify.Cards;

public sealed class Authorize3dsCardRequest
{
    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; set; } = string.Empty;

    /// <summary>Your Monnify API key. Unlike every other call in this SDK, this endpoint's contract
    /// puts it in the request body rather than relying solely on the bearer token.</summary>
    [JsonPropertyName("apiKey")]
    public string ApiKey { get; set; } = string.Empty;

    [JsonPropertyName("collectionChannel")]
    public string CollectionChannel { get; set; } = "API_NOTIFICATION";

    [JsonPropertyName("card")]
    public Card Card { get; set; } = new();
}
