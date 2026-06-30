using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class UpdateSubAccountRequest
{
    [JsonPropertyName("subAccountCode")]
    public string SubAccountCode { get; set; } = string.Empty;

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; } = "NGN";

    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("bankCode")]
    public string BankCode { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("defaultSplitPercentage")]
    public decimal DefaultSplitPercentage { get; set; }
}
