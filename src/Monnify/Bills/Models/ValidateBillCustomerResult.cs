using System.Text.Json.Serialization;

namespace Monnify.Bills;

public sealed class ValidateBillCustomerResult
{
    [JsonPropertyName("priceType")]
    public string PriceType { get; set; } = string.Empty;

    [JsonPropertyName("customerName")]
    public string? CustomerName { get; set; }

    /// <summary>
    /// Use <see cref="VendInstruction.ValidationReference"/> (nested here), not a top-level field -
    /// confirmed against a real sandbox response. Our docs sample only showed the "no reference
    /// required" case, which doesn't reveal where the reference actually lives.
    /// </summary>
    [JsonPropertyName("vendInstruction")]
    public VendInstruction? VendInstruction { get; set; }

    /// <summary>
    /// Present for at least some OPEN-priced products (confirmed via a real sandbox electricity
    /// prepaid validation) - not in our documented sample, which only covered a fixed-price
    /// airtime product.
    /// </summary>
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("minAmount")]
    public decimal? MinAmount { get; set; }

    /// <summary>See the remarks on <see cref="MinAmount"/>.</summary>
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("maxAmount")]
    public decimal? MaxAmount { get; set; }
}
