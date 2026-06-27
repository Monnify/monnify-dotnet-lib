using System.Text.Json.Serialization;

namespace Monnify.Bills;

public sealed class BillerProduct
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Singular in the real sandbox response, despite our docs currently showing this as a
    /// <c>categories</c> array - confirmed by direct sandbox calls before shipping this field.
    /// </summary>
    [JsonPropertyName("category")]
    public BillerCategory? Category { get; set; }

    /// <summary>
    /// Singular in the real sandbox response, despite our docs currently showing this as a
    /// <c>billers</c> array - confirmed by direct sandbox calls before shipping this field.
    /// </summary>
    [JsonPropertyName("biller")]
    public BillerSummary? Biller { get; set; }

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
