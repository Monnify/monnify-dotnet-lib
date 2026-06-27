using System.Text.Json.Serialization;

namespace Monnify.Bills;

public sealed class ValidateBillCustomerRequest
{
    [JsonPropertyName("productCode")]
    public string ProductCode { get; set; } = string.Empty;

    [JsonPropertyName("customerId")]
    public string CustomerId { get; set; } = string.Empty;
}
