using System.Text.Json.Serialization;

namespace Monnify.Verification;

public sealed class NinVerificationResult
{
    [JsonPropertyName("nin")]
    public string Nin { get; set; } = string.Empty;

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("middleName")]
    public string? MiddleName { get; set; }

    [JsonPropertyName("dateOfBirth")]
    public string DateOfBirth { get; set; } = string.Empty;

    [JsonPropertyName("gender")]
    public string Gender { get; set; } = string.Empty;

    [JsonPropertyName("mobileNumber")]
    public string MobileNumber { get; set; } = string.Empty;
}
