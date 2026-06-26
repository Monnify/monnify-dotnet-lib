using System.Text.Json.Serialization;

namespace Monnify.Collections;

/// <summary>How a payment should be split with a sub-account.</summary>
public sealed class IncomeSplitConfig
{
    [JsonPropertyName("subAccountCode")]
    public string SubAccountCode { get; set; } = string.Empty;

    [JsonPropertyName("feePercentage")]
    public decimal? FeePercentage { get; set; }

    [JsonPropertyName("splitPercentage")]
    public decimal? SplitPercentage { get; set; }

    [JsonPropertyName("feeBearer")]
    public bool FeeBearer { get; set; }

    [JsonPropertyName("splitAmount")]
    public decimal? SplitAmount { get; set; }
}
