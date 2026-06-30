using System.Text.Json.Serialization;

namespace Monnify.Disbursements;

public sealed class CreateWalletRequest
{
    [JsonPropertyName("walletReference")]
    public string WalletReference { get; set; } = string.Empty;

    [JsonPropertyName("walletName")]
    public string WalletName { get; set; } = string.Empty;

    [JsonPropertyName("customerName")]
    public string CustomerName { get; set; } = string.Empty;

    [JsonPropertyName("customerEmail")]
    public string CustomerEmail { get; set; } = string.Empty;

    [JsonPropertyName("bvnDetails")]
    public BvnDetails BvnDetails { get; set; } = new();
}
