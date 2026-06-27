using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class TransactionProduct
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("reference")]
    public string Reference { get; set; } = string.Empty;
}
