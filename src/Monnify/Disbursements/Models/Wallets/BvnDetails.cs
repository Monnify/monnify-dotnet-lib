using System.Text.Json.Serialization;

namespace Monnify.Disbursements;

public sealed class BvnDetails
{
    [JsonPropertyName("bvn")]
    public string Bvn { get; set; } = string.Empty;

    /// <summary>Format: <c>yyyy-MM-dd</c>.</summary>
    [JsonPropertyName("bvnDateOfBirth")]
    public string BvnDateOfBirth { get; set; } = string.Empty;
}
