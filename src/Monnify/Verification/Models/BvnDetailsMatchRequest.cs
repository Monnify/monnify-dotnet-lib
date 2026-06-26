using System.Text.Json.Serialization;

namespace Monnify.Verification;

/// <summary>Checks whether a person's claimed details match the BVN's official records.</summary>
public sealed class BvnDetailsMatchRequest
{
    [JsonPropertyName("bvn")]
    public string Bvn { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("dateOfBirth")]
    public string DateOfBirth { get; set; } = string.Empty;

    [JsonPropertyName("mobileNo")]
    public string MobileNo { get; set; } = string.Empty;
}
