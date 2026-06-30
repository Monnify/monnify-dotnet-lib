using System.Text.Json.Serialization;

namespace Monnify.Disbursements;

public sealed class CustomerWallet
{
    [JsonPropertyName("walletReference")]
    public string WalletReference { get; set; } = string.Empty;

    [JsonPropertyName("walletName")]
    public string WalletName { get; set; } = string.Empty;

    [JsonPropertyName("customerName")]
    public string CustomerName { get; set; } = string.Empty;

    [JsonPropertyName("customerEmail")]
    public string CustomerEmail { get; set; } = string.Empty;

    /// <summary>E.g. <c>SELF</c> — who bears wallet fees.</summary>
    [JsonPropertyName("feeBearer")]
    public string FeeBearer { get; set; } = string.Empty;

    [JsonPropertyName("bvnDetails")]
    public BvnDetails? BvnDetails { get; set; }

    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("accountName")]
    public string AccountName { get; set; } = string.Empty;

    [JsonPropertyName("topUpAccountDetails")]
    public WalletTopUpAccountDetails? TopUpAccountDetails { get; set; }

    /// <summary>Only present in the list response, not in the create response.</summary>
    [JsonPropertyName("createdOn")]
    public string? CreatedOn { get; set; }
}
