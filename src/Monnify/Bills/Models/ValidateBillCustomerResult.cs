using System.Text.Json.Serialization;

namespace Monnify.Bills;

public sealed class ValidateBillCustomerResult
{
    [JsonPropertyName("priceType")]
    public string PriceType { get; set; } = string.Empty;

    [JsonPropertyName("customerName")]
    public string? CustomerName { get; set; }

    [JsonPropertyName("vendInstruction")]
    public VendInstruction? VendInstruction { get; set; }

    /// <summary>
    /// Inferred, not confirmed: Monnify's docs describe a <c>validationReference</c> obtained from
    /// this endpoint for use in <c>VendAsync</c> when <see cref="VendInstruction.RequireValidationRef"/>
    /// is true, but the only response sample available when this was written used the
    /// "without a required reference" example variant, which omits this field entirely.
    /// </summary>
    [JsonPropertyName("validationReference")]
    public string? ValidationReference { get; set; }
}
