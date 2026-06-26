using System.Text.Json.Serialization;

namespace Monnify.Verification;

/// <summary>Checks whether a BVN matches the owner of a given bank account.</summary>
public sealed class BvnAccountMatchRequest
{
    [JsonPropertyName("bankCode")]
    public string BankCode { get; set; } = string.Empty;

    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("bvn")]
    public string Bvn { get; set; } = string.Empty;
}
