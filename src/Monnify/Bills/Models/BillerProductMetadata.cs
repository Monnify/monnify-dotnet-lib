using System.Text.Json.Serialization;

namespace Monnify.Bills;

public sealed class BillerProductMetadata
{
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("volume")]
    public decimal? Volume { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }

    [JsonPropertyName("productType")]
    public ProductType? ProductType { get; set; }

    [JsonPropertyName("durationUnit")]
    public string? DurationUnit { get; set; }

    [JsonPropertyName("productCategory")]
    public string? ProductCategory { get; set; }
}
