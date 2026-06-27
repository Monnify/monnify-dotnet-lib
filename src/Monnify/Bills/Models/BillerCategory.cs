using System.Text.Json.Serialization;

namespace Monnify.Bills;

public sealed class BillerCategory
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
