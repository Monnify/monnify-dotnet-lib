using System.Text.Json.Serialization;

namespace Monnify.Collections;

public sealed class TransactionCardDetails
{
    [JsonPropertyName("cardType")]
    public string? CardType { get; set; }

    [JsonPropertyName("last4")]
    public string? Last4 { get; set; }

    [JsonPropertyName("expMonth")]
    public string? ExpMonth { get; set; }

    [JsonPropertyName("expYear")]
    public string? ExpYear { get; set; }

    [JsonPropertyName("bin")]
    public string? Bin { get; set; }

    [JsonPropertyName("bankCode")]
    public string? BankCode { get; set; }

    [JsonPropertyName("bankName")]
    public string? BankName { get; set; }

    [JsonPropertyName("reusable")]
    public bool Reusable { get; set; }

    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; set; }

    [JsonPropertyName("cardToken")]
    public string? CardToken { get; set; }

    [JsonPropertyName("supportsTokenization")]
    public bool SupportsTokenization { get; set; }

    [JsonPropertyName("maskedPan")]
    public string? MaskedPan { get; set; }
}
