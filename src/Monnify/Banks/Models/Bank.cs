using System.Text.Json.Serialization;

namespace Monnify.Banks;

/// <summary>A Nigerian bank as returned by Monnify's bank list endpoints.</summary>
public sealed class Bank
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    /// <summary>Monnify's NIP bank code, used by some disbursement/validation endpoints.</summary>
    [JsonPropertyName("nipBankCode")]
    public string? NipBankCode { get; set; }

    /// <summary>USSD template for paying a specific amount, e.g. <c>*901*Amount*AccountNumber#</c>. Null if the bank doesn't support USSD.</summary>
    [JsonPropertyName("ussdTemplate")]
    public string? UssdTemplate { get; set; }

    /// <summary>The bank's base USSD dial code, e.g. <c>*901#</c>.</summary>
    [JsonPropertyName("baseUssdCode")]
    public string? BaseUssdCode { get; set; }

    /// <summary>USSD template for transferring to a specific account, e.g. <c>*901*AccountNumber#</c>.</summary>
    [JsonPropertyName("transferUssdTemplate")]
    public string? TransferUssdTemplate { get; set; }
}
