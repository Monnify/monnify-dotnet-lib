using System.Text.Json.Serialization;

namespace Monnify.Webhooks;

public sealed class ProductInfo
{
    [JsonPropertyName("reference")]
    public string Reference { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
}
