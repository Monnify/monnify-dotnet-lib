using System.Text.Json.Serialization;

namespace Monnify.Cards;

public sealed class ChargeCardRequest
{
    /// <summary>A transaction reference from a prior <c>InitializeTransactionAsync</c> call.</summary>
    [JsonPropertyName("transactionReference")]
    public string TransactionReference { get; set; } = string.Empty;

    [JsonPropertyName("collectionChannel")]
    public string CollectionChannel { get; set; } = "API_NOTIFICATION";

    [JsonPropertyName("card")]
    public Card Card { get; set; } = new();

    [JsonPropertyName("deviceInformation")]
    public DeviceInformation DeviceInformation { get; set; } = new();
}
