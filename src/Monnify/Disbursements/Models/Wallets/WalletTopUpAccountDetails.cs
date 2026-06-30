using System.Text.Json.Serialization;

namespace Monnify.Disbursements;

public sealed class WalletTopUpAccountDetails
{
    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("accountName")]
    public string AccountName { get; set; } = string.Empty;

    [JsonPropertyName("bankCode")]
    public string BankCode { get; set; } = string.Empty;

    [JsonPropertyName("bankName")]
    public string BankName { get; set; } = string.Empty;

    [JsonPropertyName("createdOn")]
    public string? CreatedOn { get; set; }
}
