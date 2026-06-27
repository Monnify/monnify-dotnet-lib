using System.Text.Json.Serialization;

namespace Monnify.Bills;

/// <summary>The billers that offer a given <see cref="BillerProduct"/>.</summary>
public sealed class BillerSummary
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
