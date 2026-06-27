using System.Text.Json.Serialization;

namespace Monnify.Bills;

public sealed class BillerProduct
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("categories")]
    public IReadOnlyList<BillerCategory> Categories { get; set; } = Array.Empty<BillerCategory>();

    [JsonPropertyName("billers")]
    public IReadOnlyList<BillerSummary> Billers { get; set; } = Array.Empty<BillerSummary>();

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("minAmount")]
    public decimal? MinAmount { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("maxAmount")]
    public decimal? MaxAmount { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("price")]
    public decimal? Price { get; set; }

    /// <summary><c>OPEN</c> (the customer picks the amount, within MinAmount/MaxAmount) or <c>FIXED</c> (Price applies).</summary>
    [JsonPropertyName("priceType")]
    public string PriceType { get; set; } = string.Empty;

    [JsonPropertyName("metadata")]
    public BillerProductMetadata? Metadata { get; set; }
}
