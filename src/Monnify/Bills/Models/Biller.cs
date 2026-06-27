using System.Text.Json.Serialization;

namespace Monnify.Bills;

public sealed class Biller
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("categories")]
    public IReadOnlyList<BillerCategory> Categories { get; set; } = Array.Empty<BillerCategory>();
}
