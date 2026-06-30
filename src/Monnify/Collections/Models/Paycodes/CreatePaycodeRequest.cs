using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class CreatePaycodeRequest
{
    [JsonPropertyName("beneficiaryName")]
    public string BeneficiaryName { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("paycodeReference")]
    public string PaycodeReference { get; set; } = string.Empty;

    /// <summary>Format: <c>YYYY-MM-DD HH:MM:SS</c>.</summary>
    [JsonPropertyName("expiryDate")]
    public string ExpiryDate { get; set; } = string.Empty;

    /// <summary>The merchant's API key (your <c>MonnifyClientOptions.ApiKey</c>).</summary>
    [JsonPropertyName("clientId")]
    public string ClientId { get; set; } = string.Empty;
}
