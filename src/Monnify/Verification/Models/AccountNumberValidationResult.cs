using System.Text.Json.Serialization;

namespace Monnify.Verification;

/// <summary>The resolved account name for a given account number + bank code (a "name enquiry").</summary>
public sealed class AccountNumberValidationResult
{
    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("accountName")]
    public string AccountName { get; set; } = string.Empty;

    [JsonPropertyName("bankCode")]
    public string BankCode { get; set; } = string.Empty;

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; } = string.Empty;
}
