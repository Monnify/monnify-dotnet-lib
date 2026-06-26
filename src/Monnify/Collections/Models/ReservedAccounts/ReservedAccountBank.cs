using System.Text.Json.Serialization;

namespace Monnify.Collections;

/// <summary>One of the (possibly several) bank accounts allocated to a reserved account.</summary>
public sealed class ReservedAccountBank
{
    [JsonPropertyName("bankCode")]
    public string BankCode { get; set; } = string.Empty;

    [JsonPropertyName("bankName")]
    public string BankName { get; set; } = string.Empty;

    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("accountName")]
    public string AccountName { get; set; } = string.Empty;
}
