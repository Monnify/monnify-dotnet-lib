using System.Text.Json.Serialization;

namespace Monnify.Bills;

public sealed class VendBillRequest
{
    [JsonPropertyName("productCode")]
    public string ProductCode { get; set; } = string.Empty;

    [JsonPropertyName("customerId")]
    public string CustomerId { get; set; } = string.Empty;

    /// <summary>Required only when <see cref="ValidateBillCustomerResult.VendInstruction"/> said so - see <see cref="VendInstruction.RequireValidationRef"/>.</summary>
    [JsonPropertyName("validationReference")]
    public string? ValidationReference { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("emailAddress")]
    public string? EmailAddress { get; set; }

    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }

    /// <summary>Your own unique reference for this vend. Use a new one on every retry - see the remarks on <see cref="IMonnifyBillsClient"/>.</summary>
    [JsonPropertyName("reference")]
    public string Reference { get; set; } = string.Empty;
}
