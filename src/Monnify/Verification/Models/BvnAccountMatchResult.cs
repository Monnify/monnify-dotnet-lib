using System.Text.Json.Serialization;

namespace Monnify.Verification;

public sealed class BvnAccountMatchResult
{
    [JsonPropertyName("bvn")]
    public string Bvn { get; set; } = string.Empty;

    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("accountName")]
    public string AccountName { get; set; } = string.Empty;

    [JsonPropertyName("matchStatus")]
    public string MatchStatus { get; set; } = string.Empty;

    [JsonPropertyName("matchPercentage")]
    public int MatchPercentage { get; set; }
}
