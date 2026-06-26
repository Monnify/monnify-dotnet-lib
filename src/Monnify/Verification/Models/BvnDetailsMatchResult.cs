using System.Text.Json.Serialization;

namespace Monnify.Verification;

/// <summary>
/// The match result for each detail checked against the BVN. Note that Monnify's response shapes
/// these inconsistently per field: <see cref="Name"/> is a structured match object, while
/// <see cref="DateOfBirth"/> and <see cref="MobileNo"/> are plain match-status strings (e.g.
/// <c>"FULL_MATCH"</c>, <c>"NO_MATCH"</c>) — this mirrors the documented response exactly.
/// </summary>
public sealed class BvnDetailsMatchResult
{
    [JsonPropertyName("bvn")]
    public string Bvn { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public BvnFieldMatch? Name { get; set; }

    [JsonPropertyName("dateOfBirth")]
    public string DateOfBirth { get; set; } = string.Empty;

    [JsonPropertyName("mobileNo")]
    public string MobileNo { get; set; } = string.Empty;
}

/// <summary>A structured match result, e.g. for the <see cref="BvnDetailsMatchResult.Name"/> field.</summary>
public sealed class BvnFieldMatch
{
    [JsonPropertyName("matchStatus")]
    public string MatchStatus { get; set; } = string.Empty;

    [JsonPropertyName("matchPercentage")]
    public int MatchPercentage { get; set; }
}
